using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
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
        /// Deserializes a byte array into an object.
        /// </summary>
        /// <typeparam name="T">The Type of the object to deserialize into.</typeparam>
        /// <param name="bytes">The byte array to deserialize</param>
        /// <param name="cacheFields">Whether or not to cache information about the object type for future calls</param>
        /// <returns>The deserialized object</returns>
        public static T Deserialize<T>(byte[] bytes, bool cacheFields = true)
        {
            int bytePos = 0;
            return (T)Deserialize(typeof(T), bytes, ref bytePos, cacheFields);
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
                if (t.CustomAttributes.Any(attr => attr.AttributeType == typeof(NonSerializable)))
                {
                    return false;
                }

                t = t.BaseType;
            }

            return true;
        }

        /// <summary>
        /// Deserializes a string object from a byte array
        /// </summary>
        /// <param name="bytes">The byte array to deserialize from</param>
        /// <param name="bytePos">The position in the byte array to deserialize from</param>
        /// <returns>The deserialized object</returns>
        private static string GetStringFromBytes(byte[] bytes, ref int bytePos)
        {
            int strLen = GetIntFromBytes(bytes, ref bytePos);
            if (bytes.Length - bytePos < strLen)
            {
                throw new ArgumentException("Unexpected end of array in deserialization");
            }

            bytePos += strLen;
            return Encoding.ASCII.GetString(bytes, bytePos - strLen, strLen);
        }

        /// <summary>
        /// Deserializes an int object from a byte array
        /// </summary>
        /// <param name="bytes">The byte array to deserialize from</param>
        /// <param name="bytePos">The position in the byte array to deserialize from</param>
        /// <returns>The deserialized object</returns>
        private static int GetIntFromBytes(byte[] bytes, ref int bytePos)
        {
            if (bytes.Length - bytePos < 4)
            {
                throw new ArgumentException("Unexpected end of array in deserialization");
            }

            bytePos += 4;
            return BitConverter.ToInt32(bytes, bytePos - 4);
        }

        /// <summary>
        /// Deserializes a short object from a byte array
        /// </summary>
        /// <param name="bytes">The byte array to deserialize from</param>
        /// <param name="bytePos">The position in the byte array to deserialize from</param>
        /// <returns>The deserialized object</returns>
        private static short GetShortFromBytes(byte[] bytes, ref int bytePos)
        {
            if (bytes.Length - bytePos < 2)
            {
                throw new ArgumentException("Unexpected end of array in deserialization");
            }

            bytePos += 2;
            return BitConverter.ToInt16(bytes, bytePos - 2);
        }

        /// <summary>
        /// Deserializes a ushort object from a byte array
        /// </summary>
        /// <param name="bytes">The byte array to deserialize from</param>
        /// <param name="bytePos">The position in the byte array to deserialize from</param>
        /// <returns>The deserialized object</returns>
        private static ushort GetUShortFromBytes(byte[] bytes, ref int bytePos)
        {
            if (bytes.Length - bytePos < 2)
            {
                throw new ArgumentException("Unexpected end of array in deserialization");
            }

            bytePos += 2;
            return BitConverter.ToUInt16(bytes, bytePos - 2);
        }

        /// <summary>
        /// Deserializes an unsigned integer object from a byte array
        /// </summary>
        /// <param name="bytes">The byte array to deserialize from</param>
        /// <param name="bytePos">The position in the byte array to deserialize from</param>
        /// <returns>The deserialized object</returns>
        private static uint GetUIntFromBytes(byte[] bytes, ref int bytePos)
        {
            if (bytes.Length - bytePos < 4)
            {
                throw new ArgumentException("Unexpected end of array in deserialization");
            }

            bytePos += 4;
            return BitConverter.ToUInt32(bytes, bytePos - 4);
        }

        /// <summary>
        /// Deserializes a long object from a byte array
        /// </summary>
        /// <param name="bytes">The byte array to deserialize from</param>
        /// <param name="bytePos">The position in the byte array to deserialize from</param>
        /// <returns>The deserialized object</returns>
        private static long GetLongFromBytes(byte[] bytes, ref int bytePos)
        {
            if (bytes.Length - bytePos < 8)
            {
                throw new ArgumentException("Unexpected end of array in deserialization");
            }

            bytePos += 8;
            return BitConverter.ToInt64(bytes, bytePos - 8);
        }

        /// <summary>
        /// Deserializes an unsigned long object from a byte array
        /// </summary>
        /// <param name="bytes">The byte array to deserialize from</param>
        /// <param name="bytePos">The position in the byte array to deserialize from</param>
        /// <returns>The deserialized object</returns>
        private static ulong GetULongFromBytes(byte[] bytes, ref int bytePos)
        {
            if (bytes.Length - bytePos < 8)
            {
                throw new ArgumentException("Unexpected end of array in deserialization");
            }

            bytePos += 8;
            return BitConverter.ToUInt64(bytes, bytePos - 8);
        }

        /// <summary>
        /// Deserializes a character object from a byte array
        /// </summary>
        /// <param name="bytes">The byte array to deserialize from</param>
        /// <param name="bytePos">The position in the byte array to deserialize from</param>
        /// <returns>The deserialized object</returns>
        private static char GetCharFromBytes(byte[] bytes, ref int bytePos)
        {
            if (bytes.Length - bytePos < 2)
            {
                throw new ArgumentException("Unexpected end of array in deserialization");
            }

            bytePos += 2;
            return BitConverter.ToChar(bytes, bytePos - 2);
        }

        /// <summary>
        /// Deserializes a double object from a byte array
        /// </summary>
        /// <param name="bytes">The byte array to deserialize from</param>
        /// <param name="bytePos">The position in the byte array to deserialize from</param>
        /// <returns>The deserialized object</returns>
        private static double GetDoubleFromBytes(byte[] bytes, ref int bytePos)
        {
            if (bytes.Length - bytePos < 8)
            {
                throw new ArgumentException("Unexpected end of array in deserialization");
            }

            bytePos += 8;
            return BitConverter.ToDouble(bytes, bytePos - 8);
        }

        /// <summary>
        /// Deserializes a float object from a byte array
        /// </summary>
        /// <param name="bytes">The byte array to deserialize from</param>
        /// <param name="bytePos">The position in the byte array to deserialize from</param>
        /// <returns>The deserialized object</returns>
        private static float GetFloatFromBytes(byte[] bytes, ref int bytePos)
        {
            if (bytes.Length - bytePos < 4)
            {
                throw new ArgumentException("Unexpected end of array in deserialization");
            }

            bytePos += 4;
            return BitConverter.ToSingle(bytes, bytePos - 4);
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
                return BitConverter.GetBytes(-1);
            }

            if (!IsSerializable(obj))
            {
                throw new ArgumentException("Parameter \"obj\" is not serializable.");
            }

            Type objType = obj.GetType();

            // Special case for arrays
            if (objType.IsArray)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    Array objArray = (Array)obj;
                    stream.Write(BitConverter.GetBytes(objArray.Length));

                    for (int i = 0; i < objArray.Length; i++)
                    {
                        stream.Write(Serialize(objArray.GetValue(i), cacheFields, currentDepth++));
                    }

                    return stream.ToArray();
                }
            }

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

                // Skip over non-serializable members
                fields = fields.Where(f => IsSerializable(f.FieldType) && !f.CustomAttributes.Any(attr => attr.AttributeType == typeof(NonSerializable))).ToArray();

                stream.Write(BitConverter.GetBytes(fields.Length));

                foreach (FieldInfo field in fields)
                {
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

        /// <summary>
        /// Deserializes a byte array into an object.
        /// </summary>
        /// <param name="returnType">The Type of the object to deserialize into.</param>
        /// <param name="bytes">The byte array to deserialize</param>
        /// <param name="bytePos">The current position in the byte array</param>
        /// <param name="cacheFields">Whether or not to cache information about the object type for future calls</param>
        /// <param name="objType">Optionally skip calculation of object type from bytes</param>
        /// <returns>The deserialized object</returns>
        private static object Deserialize(Type returnType, byte[] bytes, ref int bytePos, bool cacheFields, Type objType = null)
        {
            if (returnType == null)
            {
                throw new ArgumentNullException("Parameter \"returnType\" may not be null.");
            }

            if (bytes == null || bytes.Length == 0)
            {
                throw new ArgumentNullException("Parameter \"bytes\" may not be null or empty.");
            }

            if (objType == null)
            {
                int nullCheck = GetIntFromBytes(bytes, ref bytePos);
                if (nullCheck == -1)
                {
                    if (returnType.IsValueType)
                    {
                        return Activator.CreateInstance(returnType);
                    }

                    return null;
                }

                bytePos -= 4;

                objType = Type.GetType(GetStringFromBytes(bytes, ref bytePos));
            }

            if (!objType.IsAssignableFrom(returnType))
            {
                throw new ArgumentException($"Given type \"{returnType.Name}\" does not match type \"{objType.Name}\" in serialized bytes.");
            }

            if (!CachedFields.TryGetValue(returnType, out FieldInfo[] fields))
            {
                // Get every instance field in this object, including private fields and base types
                fields = returnType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

                if (cacheFields)
                {
                    CachedFields[returnType] = fields;
                }
            }

            if (returnType == typeof(string) || returnType.IsPrimitive)
            {
                return GetValueOfField(returnType, bytes, ref bytePos, cacheFields);
            }

            object returnObj = FormatterServices.GetUninitializedObject(returnType);

            int fieldCount = GetIntFromBytes(bytes, ref bytePos);
            while (fieldCount > 0)
            {
                string fieldName = GetStringFromBytes(bytes, ref bytePos);
                
                FieldInfo field = fields.Where(f => f.Name == fieldName).FirstOrDefault();
                
                if (field == null)
                {
                    throw new ArgumentException($"Given type \"{returnType}\" does not contain a field named \"{fieldName}\".");
                }

                field.SetValue(returnObj, GetValueOfField(field.FieldType, bytes, ref bytePos, cacheFields));

                fieldCount--;
            }

            return returnObj;
        }

        /// <summary>
        /// Gets the value of the current field in the byte array
        /// </summary>
        /// <param name="fieldType">The type of the field</param>
        /// <param name="bytes">The bytes to get the field from</param>
        /// <param name="bytePos">The current position in the byte array</param>
        /// <param name="cacheFields">Whether or not to cache fields in potential Deserialize calls</param>
        /// <returns>The field value</returns>
        private static object GetValueOfField(Type fieldType, byte[] bytes, ref int bytePos, bool cacheFields)
        {
            if (fieldType.IsArray)
            {
                int arrayLength = GetIntFromBytes(bytes, ref bytePos);

                if (arrayLength == -1)
                {
                    return null;
                }
                else
                {
                    Array array = (Array)Activator.CreateInstance(fieldType, arrayLength);
                    Type arrayType = array.GetType().GetElementType();

                    for (int i = 0; i < arrayLength; i++)
                    {
                        if (arrayType == typeof(string) || arrayType.IsPrimitive)
                        {
                            array.SetValue(Deserialize(arrayType, bytes, ref bytePos, cacheFields, arrayType), i);
                        }
                        else
                        {
                            array.SetValue(Deserialize(arrayType, bytes, ref bytePos, cacheFields), i);
                        }
                    }

                    return array;
                }
            }
            else if (fieldType == typeof(bool))
            {
                if (bytePos >= bytes.Length)
                {
                    throw new ArgumentException("Unexpected end of array in deserialization");
                }

                return bytes[bytePos++] == 1;
            }
            else if (fieldType == typeof(byte))
            {
                if (bytePos >= bytes.Length)
                {
                    throw new ArgumentException("Unexpected end of array in deserialization");
                }

                return bytes[bytePos++];
            }
            else if (fieldType == typeof(sbyte))
            {
                if (bytePos >= bytes.Length)
                {
                    throw new ArgumentException("Unexpected end of array in deserialization");
                }

                return (sbyte)(bytes[bytePos++] - 128);
            }
            else if (fieldType == typeof(short))
            {
                return GetShortFromBytes(bytes, ref bytePos);
            }
            else if (fieldType == typeof(ushort))
            {
                return GetUShortFromBytes(bytes, ref bytePos);
            }
            else if (fieldType == typeof(int))
            {
                return GetIntFromBytes(bytes, ref bytePos);
            }
            else if (fieldType == typeof(uint))
            {
                return GetUIntFromBytes(bytes, ref bytePos);
            }
            else if (fieldType == typeof(long))
            {
                return GetLongFromBytes(bytes, ref bytePos);
            }
            else if (fieldType == typeof(ulong))
            {
                return GetULongFromBytes(bytes, ref bytePos);
            }
            else if (fieldType == typeof(char))
            {
                return GetCharFromBytes(bytes, ref bytePos);
            }
            else if (fieldType == typeof(double))
            {
                return GetDoubleFromBytes(bytes, ref bytePos);
            }
            else if (fieldType == typeof(float))
            {
                return GetFloatFromBytes(bytes, ref bytePos);
            }
            else if (fieldType == typeof(string))
            {
                return GetStringFromBytes(bytes, ref bytePos);
            }
            else
            {
                return Deserialize(fieldType, bytes, ref bytePos, cacheFields);
            }
        }

        /// <summary>
        /// Indicates that a class or field should not be serialized
        /// </summary>
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
        public class NonSerializable : Attribute
        {
        }
    }
}
