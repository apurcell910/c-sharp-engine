using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SharpSlugsEngine
{
    /// <summary>
    /// Helper class for serializing objects
    /// </summary>
    public static class SerializationUtility
    {
        // Caching stuff to speed up the function calls
        private static readonly Dictionary<Type, FieldInfo[]> CachedFields = new Dictionary<Type, FieldInfo[]>();

        /// <summary>
        /// Serializes an object into a byte array
        /// </summary>
        /// <param name="obj">The object to serialize</param>
        /// <param name="cacheFields">Whether or not to cache information about the object type for future calls</param>
        /// <returns>A byte array containing the serialized object</returns>
        public static byte[] Serialize(object obj, bool cacheFields = true)
        {
            return Serialize(obj, cacheFields, 0);
        }

        /// <summary>
        /// Checks if an object is serializable
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <returns>A bool indicating whether the object is serializable</returns>
        public static bool IsSerializable(object obj)
        {
            // Obviously null can't be serialized
            if (obj == null)
            {
                return false;
            }

            return IsSerializable(obj.GetType());
        }

        /// <summary>
        /// Checks if a Type is serializable
        /// </summary>
        /// <param name="t">The Type to check</param>
        /// <returns>A bool indicating whether the Type is serializable</returns>
        public static bool IsSerializable(Type t)
        {
            // It's probably better to let the caller know they have a null Type in this situation
            if (t == null)
            {
                throw new NullReferenceException("Parameter \"t\" may not be null.");
            }

            // Need to loop to check all of the base types as well
            while (t != null)
            {
                if (t.CustomAttributes.Any(attr => attr.AttributeType == typeof(NonSerializedAttribute)))
                {
                    return false;
                }

                t = t.BaseType;
            }

            return true;
        }

        /// <summary>
        /// Serializes an object into a byte array
        /// </summary>
        /// <param name="obj">The object to serialize</param>
        /// <param name="cacheFields">Whether or not to cache information about the object type for future calls</param>
        /// <param name="currentDepth">The current depth in recursion</param>
        /// <returns>A byte array containing the serialized object</returns>
        private static byte[] Serialize(object obj, bool cacheFields, int currentDepth)
        {
            // Special case for null and max depth
            if (obj == null || currentDepth >= 10)
            {
                return new byte[] { 0 };
            }

            if (!IsSerializable(obj))
            {
                throw new ArgumentException("Parameter \"obj\" is not serializable.");
            }

            Type objType = obj.GetType();

            // Special cases for primitives
            if (objType == typeof(bool))
            {
                bool b = (bool)obj;
                return new byte[] { (byte)(b ? 1 : 0) };
            }
            else if (objType == typeof(byte))
            {
                return new byte[] { (byte)obj };
            }
            else if (objType == typeof(sbyte))
            {
                return new byte[] { (byte)((sbyte)obj + 128) };
            }
            else if (objType == typeof(short))
            {
                return BitConverter.GetBytes((short)obj);
            }
            else if (objType == typeof(ushort))
            {
                return BitConverter.GetBytes((ushort)obj);
            }
            else if (objType == typeof(int))
            {
                return BitConverter.GetBytes((int)obj);
            }
            else if (objType == typeof(uint))
            {
                return BitConverter.GetBytes((uint)obj);
            }
            else if (objType == typeof(long))
            {
                return BitConverter.GetBytes((long)obj);
            }
            else if (objType == typeof(ulong))
            {
                return BitConverter.GetBytes((ulong)obj);
            }
            else if (objType == typeof(char))
            {
                return BitConverter.GetBytes((char)obj);
            }
            else if (objType == typeof(double))
            {
                return BitConverter.GetBytes((double)obj);
            }
            else if (objType == typeof(float))
            {
                return BitConverter.GetBytes((float)obj);
            }
            else if (objType == typeof(string))
            {
                // Not a primitive but it is an inbuilt type
                byte[] nameBytes = Encoding.ASCII.GetBytes((string)obj);
                byte[] nameBytesLen = BitConverter.GetBytes(nameBytes.Length);

                byte[] strBytes = new byte[nameBytes.Length + nameBytesLen.Length];
                nameBytesLen.CopyTo(strBytes, 0);
                nameBytes.CopyTo(strBytes, nameBytesLen.Length);

                return strBytes;
            }

            if (!CachedFields.TryGetValue(objType, out FieldInfo[] fields))
            {
                // Get every instance field in this object, including private fields and base types
                fields = objType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

                if (cacheFields)
                {
                    CachedFields[objType] = fields;
                }
            }

            using (MemoryStream stream = new MemoryStream())
            {
                // Write type name and amount of fields
                byte[] nameBytes = Encoding.ASCII.GetBytes(objType.AssemblyQualifiedName);
                stream.Write(BitConverter.GetBytes(nameBytes.Length));
                stream.Write(nameBytes);
                stream.Write(BitConverter.GetBytes(fields.Length));

                foreach (FieldInfo field in fields)
                {
                    // Skip over non-serializable members
                    if (!IsSerializable(field.GetType()) || field.CustomAttributes.Any(attr => attr.AttributeType == typeof(NonSerializedAttribute)))
                    {
                        continue;
                    }

                    // Write field name
                    nameBytes = Encoding.ASCII.GetBytes(field.Name);
                    stream.Write(BitConverter.GetBytes(nameBytes.Length));
                    stream.Write(nameBytes);

                    // Get field value
                    object fieldVal = field.GetValue(obj);

                    // Recursively serialize
                    stream.Write(Serialize(fieldVal, cacheFields, currentDepth + 1));
                }

                return stream.ToArray();
            }
        }
    }
}
