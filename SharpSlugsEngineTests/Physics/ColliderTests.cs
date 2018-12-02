using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpSlugsEngine.Physics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine.Physics.Tests
{
    [TestClass()]
    public class ColliderTests
    {
        [TestMethod()]
        public void PolygonCreationErrorTest()
        {
            try
            {
                new PolygonCollider(new Vector2(100, 300));
                Assert.Fail("Less than 3 vertices is an invalid polygon");
            }
            catch (ArgumentException)
            {
            }

            try
            {
                new PolygonCollider(new Vector2(0, 0), new Vector2(100, 0), new Vector2(200, 0));
                Assert.Fail("Flat line is an invalid polygon");
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod()]
        public void TriangleIsTouchingTriangleTest()
        {
            Collider coll1 = new TriangleCollider(new Vector2(0, 0), new Vector2(100, 0f), new Vector2(50, 100));
            Collider coll2 = new TriangleCollider(new Vector2(200, 0), new Vector2(300, 0), new Vector2(250, 100));

            Assert.IsFalse(coll1.IsTouching(coll2));
            Assert.IsFalse(coll2.IsTouching(coll1));

            coll2.Position -= new Vector2(150, 15);

            Assert.IsTrue(coll1.IsTouching(coll2));
            Assert.IsTrue(coll2.IsTouching(coll1));
        }

        [TestMethod()]
        public void TriangleIsTouchingRectangleTest()
        {
            Collider coll1 = new TriangleCollider(new Vector2(0, 0), new Vector2(100, 0f), new Vector2(50, 100));
            Collider coll2 = new RectangleCollider(200, 100, 100, 100);

            Assert.IsFalse(coll1.IsTouching(coll2));
            Assert.IsFalse(coll2.IsTouching(coll1));

            coll2.Position -= new Vector2(150, 15);

            Assert.IsTrue(coll1.IsTouching(coll2));
            Assert.IsTrue(coll2.IsTouching(coll1));
        }

        [TestMethod()]
        public void TriangleIsTouchingEllipseTest()
        {
            Collider coll1 = new TriangleCollider(new Vector2(0, 0), new Vector2(100, 0f), new Vector2(50, 100));
            Collider coll2 = new EllipseCollider(200, 100, 100, 100);

            Assert.IsFalse(coll1.IsTouching(coll2));
            Assert.IsFalse(coll2.IsTouching(coll1));

            coll2.Position -= new Vector2(150, 15);

            Assert.IsTrue(coll1.IsTouching(coll2));
            Assert.IsTrue(coll2.IsTouching(coll1));
        }

        [TestMethod()]
        public void TriangleIsTouchingPolygonTest()
        {
            Collider coll1 = new TriangleCollider(new Vector2(0, 0), new Vector2(100, 0f), new Vector2(50, 100));
            Collider coll2 = new PolygonCollider(new Vector2(200, 0), new Vector2(300, 0), new Vector2(250, 100), new Vector2(100, 100));

            Assert.IsFalse(coll1.IsTouching(coll2));
            Assert.IsFalse(coll2.IsTouching(coll1));

            coll2.Position -= new Vector2(150, 15);

            Assert.IsTrue(coll1.IsTouching(coll2));
            Assert.IsTrue(coll2.IsTouching(coll1));
        }

        [TestMethod()]
        public void RectangleIsTouchingRectangleTest()
        {
            Collider coll1 = new RectangleCollider(0, 0, 100, 100);
            Collider coll2 = new RectangleCollider(200, 100, 100, 100);

            Assert.IsFalse(coll1.IsTouching(coll2));
            Assert.IsFalse(coll2.IsTouching(coll1));

            coll2.Position -= new Vector2(150, 15);

            Assert.IsTrue(coll1.IsTouching(coll2));
            Assert.IsTrue(coll2.IsTouching(coll1));
        }

        [TestMethod()]
        public void RectangleIsTouchingEllipseTest()
        {
            Collider coll1 = new RectangleCollider(0, 0, 100, 100);
            Collider coll2 = new EllipseCollider(200, 100, 100, 100);

            Assert.IsFalse(coll1.IsTouching(coll2));
            Assert.IsFalse(coll2.IsTouching(coll1));

            coll2.Position -= new Vector2(150, 15);

            Assert.IsTrue(coll1.IsTouching(coll2));
            Assert.IsTrue(coll2.IsTouching(coll1));
        }

        [TestMethod()]
        public void RectangleIsTouchingPolygonTest()
        {
            Collider coll1 = new RectangleCollider(0, 0, 100, 100);
            Collider coll2 = new PolygonCollider(new Vector2(200, 0), new Vector2(300, 0), new Vector2(250, 100), new Vector2(110, 100));

            Assert.IsFalse(coll1.IsTouching(coll2));
            Assert.IsFalse(coll2.IsTouching(coll1));

            coll2.Position -= new Vector2(150, 15);

            Assert.IsTrue(coll1.IsTouching(coll2));
            Assert.IsTrue(coll2.IsTouching(coll1));
        }

        [TestMethod()]
        public void EllipseIsTouchingEllipseTest()
        {
            Collider coll1 = new EllipseCollider(0, 100, 100, 100);
            Collider coll2 = new EllipseCollider(200, 100, 100, 100);

            Assert.IsFalse(coll1.IsTouching(coll2));
            Assert.IsFalse(coll2.IsTouching(coll1));

            coll2.Position -= new Vector2(150, 15);

            Assert.IsTrue(coll1.IsTouching(coll2));
            Assert.IsTrue(coll2.IsTouching(coll1));
        }

        [TestMethod()]
        public void EllipseIsTouchingPolygonTest()
        {
            Collider coll1 = new EllipseCollider(0, 100, 100, 100);
            Collider coll2 = new PolygonCollider(new Vector2(200, 0), new Vector2(300, 0), new Vector2(250, 100), new Vector2(200, 100));

            Assert.IsFalse(coll1.IsTouching(coll2));
            Assert.IsFalse(coll2.IsTouching(coll1));

            coll2.Position -= new Vector2(150, 15);

            Assert.IsTrue(coll1.IsTouching(coll2));
            Assert.IsTrue(coll2.IsTouching(coll1));
        }

        [TestMethod()]
        public void PolygonIsTouchingPolygonTest()
        {
            Collider coll1 = new PolygonCollider(new Vector2(0, 0), new Vector2(100, 0), new Vector2(50, 100), new Vector2(0, 100));
            Collider coll2 = new PolygonCollider(new Vector2(200, 0), new Vector2(300, 0), new Vector2(250, 100), new Vector2(100, 100));

            Assert.IsFalse(coll1.IsTouching(coll2));
            Assert.IsFalse(coll2.IsTouching(coll1));

            coll2.Position -= new Vector2(150, 15);

            Assert.IsTrue(coll1.IsTouching(coll2));
            Assert.IsTrue(coll2.IsTouching(coll1));
        }

        [TestMethod()]
        public void TriangleIsTouchingPointTest()
        {
            Collider coll = new TriangleCollider(new Vector2(0, 0), new Vector2(100, 0f), new Vector2(50, 100));

            Assert.IsFalse(coll.IsTouching(new Vector2(500, 500)));
            Assert.IsTrue(coll.IsTouching(new Vector2(50, 50)));
        }

        [TestMethod()]
        public void RectangleIsTouchingPointTest()
        {
            Collider coll = new RectangleCollider(0, 0, 100, 100);

            Assert.IsFalse(coll.IsTouching(new Vector2(500, 500)));
            Assert.IsTrue(coll.IsTouching(new Vector2(50, 50)));
        }

        [TestMethod()]
        public void EllipseIsTouchingPointTest()
        {
            Collider coll = new EllipseCollider(0, 0, 100, 100);

            Assert.IsFalse(coll.IsTouching(new Vector2(500, 500)));
            Assert.IsTrue(coll.IsTouching(new Vector2(50, 50)));
        }

        [TestMethod()]
        public void PolygonIsTouchingPointTest()
        {
            Collider coll = new PolygonCollider(new Vector2(0, 0), new Vector2(100, 0f), new Vector2(50, 100), new Vector2(-100, 300));

            Assert.IsFalse(coll.IsTouching(new Vector2(500, 500)));
            Assert.IsTrue(coll.IsTouching(new Vector2(50, 50)));
        }

        [TestMethod()]
        public void GetBoundingBoxTest()
        {
            EllipseCollider coll = new EllipseCollider(0, 0, 100, 100);
            RectangleF box = coll.GetBoundingBox();

            Assert.IsTrue(Math.Abs(box.X + 100) <= 0.5f);
            Assert.IsTrue(Math.Abs(box.Y + 100) <= 0.5f);
            Assert.IsTrue(Math.Abs(box.Width - 200) <= 0.5f);
            Assert.IsTrue(Math.Abs(box.Height - 200) <= 0.5f);
        }

        [TestMethod()]
        public void GetCenterTest()
        {
            EllipseCollider coll = new EllipseCollider(0, 0, 100, 100);
            Vector2 center = coll.GetCenter();

            Assert.IsTrue(Math.Abs(center.X) <= 0.01f);
            Assert.IsTrue(Math.Abs(center.Y) <= 0.01f);
        }
    }
}