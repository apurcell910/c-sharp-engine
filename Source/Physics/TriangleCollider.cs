using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine.Physics
{
    class TriangleCollider : Collider
    {
        Triangle triangle;
        public TriangleCollider(Vector2 v1, Vector2 v2, Vector2 v3) : this(new Triangle(v1, v2, v3)) { }

        public TriangleCollider(Triangle tri)
        {
            this.triangle = tri;
        }
        /// <summary>
        /// This function will check if any vertex of a triangle collider is touching this triangle collider
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        /// https://stackoverflow.com/questions/2778240/detection-of-triangle-collision-in-2d-space
        private bool checkTriangleConditions(TriangleCollider other)
        {
            bool inPointOne = triangle.ContainsPoint(other.triangle.VertexOne);
            bool inPointTwo = triangle.ContainsPoint(other.triangle.VertexTwo);
            bool inPointThree = triangle.ContainsPoint(other.triangle.VertexThree);

            if (inPointOne || inPointTwo || inPointThree)
            {
                return true;
            }
            return false;
        }

        private bool checkEllipseConditions(EllipseCollider other)
        {
            return true;
        }

        private bool checkRectConditions(RectangleCollider other)
        {
            return true;
        }

        private bool checkPolygonConditions(PolygonCollider other)
        {
            return true;
        }

        public override bool IsTouching(Collider other)
        {
            if (other is TriangleCollider triCollider)
            {
                return checkTriangleConditions(triCollider);
            }
            else if (other is PolygonCollider polyCollider)
            {
                return checkPolygonConditions(polyCollider);
            }
            else if (other is EllipseCollider ellipseCollider)
            {
                return checkEllipseConditions(ellipseCollider);
            }
            else if(other is RectangleCollider rectCollider)
            {
                return checkRectConditions(rectCollider);
            }
            return false;
        }

        
    }
}
