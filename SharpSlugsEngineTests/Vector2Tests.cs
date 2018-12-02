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
    public class Vector2Tests
    {
        [TestMethod()]
        public void CrossProductTest()
        {
            Vector2 v1 = new Vector2(50, 20);
            Vector2 v2 = new Vector2(20.3f, -36);
            Vector2 sub = new Vector2(63.11f, 90.2f);

            Assert.IsTrue(Vector2.CrossProduct(sub, v1, v2) == Vector2.CrossProduct(v1 - sub, v2 - sub));
        }

        [TestMethod()]
        public void RotateTest()
        {
            Vector2 rotated = Vector2.One.Rotate(Vector2.Zero, 90);

            Assert.IsTrue(Math.Abs(rotated.X + 1) <= 0.001f);
            Assert.IsTrue(Math.Abs(rotated.Y - 1) <= 0.001f);

            rotated = rotated.Rotate(Vector2.Zero, 90);

            Assert.IsTrue(Math.Abs(rotated.X + 1) <= 0.001f);
            Assert.IsTrue(Math.Abs(rotated.Y + 1) <= 0.001f);

            rotated = rotated.Rotate(Vector2.Zero, 90);

            Assert.IsTrue(Math.Abs(rotated.X - 1) <= 0.001f);
            Assert.IsTrue(Math.Abs(rotated.Y + 1) <= 0.001f);

            rotated = rotated.Rotate(Vector2.Zero, 90);

            Assert.IsTrue(Math.Abs(rotated.X - 1) <= 0.001f);
            Assert.IsTrue(Math.Abs(rotated.Y - 1) <= 0.001f);
        }

        [TestMethod()]
        public void NormalizeTest()
        {
            Random rnd = new Random();
            for (int i = 0; i < 10000; i++)
            {
                Vector2 normalized = new Vector2((float)rnd.NextDouble(), (float)rnd.NextDouble()).Normalize();
                Assert.IsTrue(Math.Abs(normalized.Length - 1) <= 0.001f);
            }
        }

        [TestMethod()]
        public void EqualsTest()
        {
            Assert.IsFalse(Vector2.Zero.Equals(null));
            Assert.IsFalse(new Vector2(76.3f, 1).Equals(new Vector2(38, 1)));

            Assert.IsTrue(new Vector2(50.62f, 50).Equals(new Vector2(50.62f, 50)));
            Assert.IsTrue(new Vector2(50, 50).Equals(new Vector2?(new Vector2(50, 50))));

            Random rnd = new Random();
            for (int i = 0; i < 10000; i++)
            {
                Vector2 vec1 = new Vector2(rnd.Next(10), rnd.Next(10));
                Vector2 vec2 = new Vector2(rnd.Next(10), rnd.Next(10));

                if (vec1.X == vec2.X && vec1.Y == vec2.Y)
                {
                    Assert.IsTrue(vec1.Equals(vec2));
                    Assert.IsTrue(vec2.Equals(vec1));
                }
                else
                {
                    Assert.IsFalse(vec1.Equals(vec2));
                    Assert.IsFalse(vec2.Equals(vec1));
                }
            }
        }

        [TestMethod()]
        public void ToStringTest()
        {
            Random rnd = new Random();
            for (int i = 0; i < 10000; i++)
            {
                Vector2 vec = new Vector2((float)rnd.NextDouble(), (float)rnd.NextDouble());
                string vecStr = vec.ToString();

                Assert.IsTrue(vecStr == $"(X: {vec.X}, Y: {vec.Y})");
            }
        }
    }
}