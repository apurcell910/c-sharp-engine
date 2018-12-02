using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpSlugsEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine.Tests
{
    [TestClass()]
    public class SerializationUtilityTests
    {
        [TestMethod()]
        public void SerializeDeserializeTest()
        {
            Serializable serializeTest = new Serializable()
            {
                int1 = 5,
                int2 = 7,
                array = new int[]
                {
                    1,
                    2,
                    3,
                    4,
                    5
                },
                short1 = -3,
                short2 = 30000,
                str = "This string is for testing purposes"
            };

            byte[] serialized = SerializationUtility.Serialize(serializeTest);
            Serializable deserialized = SerializationUtility.Deserialize<Serializable>(serialized);

            Assert.IsTrue(serializeTest.int1 == deserialized.int1);
            Assert.IsTrue(serializeTest.int2 == deserialized.int2);

            for (int i = 0; i < 5; i++)
            {
                Assert.IsTrue(serializeTest.array[i] == deserialized.array[i]);
            }

            Assert.IsTrue(serializeTest.short1 == deserialized.short1);
            Assert.IsTrue(serializeTest.short2 == deserialized.short2);
            Assert.IsNull(deserialized.str, "Field 'str' is marked as non-serializable and must be ignored");
        }

        [TestMethod()]
        public void IsSerializableTest()
        {
            object testObject = null;

            Assert.IsFalse(SerializationUtility.IsSerializable(testObject), "Null is not serializable");
            Assert.IsFalse(SerializationUtility.IsSerializable(new NotSerializable()), "Object is marked as not serializable");
            Assert.IsFalse(SerializationUtility.IsSerializable(new AlsoNotSerializable()), "Object base type is marked as not serializable");
            Assert.IsTrue(SerializationUtility.IsSerializable(new Serializable()), "Object is serializable");
        }

        [TestMethod()]
        public void IsSerializableTest1()
        {
            try
            {
                Type testType = null;
                SerializationUtility.IsSerializable(testType);
                Assert.Fail("Null type should cause a NullReferenceException.");
            }
            catch (NullReferenceException)
            {
            }

            Assert.IsFalse(SerializationUtility.IsSerializable(typeof(NotSerializable)), "Type is marked as not serializable");
            Assert.IsFalse(SerializationUtility.IsSerializable(typeof(AlsoNotSerializable)), "Type's base type is marked as not serializable");
            Assert.IsTrue(SerializationUtility.IsSerializable(typeof(Serializable)), "Type is serializable");
        }

        [SerializationUtility.NonSerializable]
        private class NotSerializable
        {
        }

        private class AlsoNotSerializable : NotSerializable
        {
        }

        private class Serializable
        {
            public int int1;
            public int int2;
            internal int[] array;

            public short short1;
            public short short2;

            [SerializationUtility.NonSerializable]
            internal string str;
        }
    }
}