using System;
using System.Drawing;
namespace SharpSlugsEngine.Physics
{
    public struct PTriangle
    {
        public Vector2 VertexOne { get; private set; }
        public Vector2 VertexTwo { get; private set; }
        public Vector2 VertexThree { get; private set; }
        public float Area { get; private set; }
        
        public PTriangle(Vector2 vert1, Vector2 vert2, Vector2 vert3)
        {
            VertexOne = vert1;
            VertexTwo = vert2;
            VertexThree = vert3;

            float side1 = (VertexOne - VertexTwo).Length;
            float side2 = (VertexTwo - VertexThree).Length;
            float side3 = (VertexThree - VertexOne).Length;

            float s = (side1 + side2 + side3) / 2f;
            Area = (float)Math.Sqrt(s * (s - side1) * (s - side2) * (s - side3));
        }

        /// <summary>
        /// Checks whether or not the triangle contains a given point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="tri1"></param>
        /// <param name="tri2"></param>
        /// <param name="tri3"></param>
        /// <returns></returns>
        public bool ContainsPoint(Vector2 point)
        {
            float crossProd1 = Vector2.CrossProduct(VertexOne, point, VertexTwo);
            float crossProd2 = Vector2.CrossProduct(VertexTwo, point, VertexThree);
            float crossProd3 = Vector2.CrossProduct(VertexThree, point, VertexOne);

            return (crossProd1 > 0 && crossProd2 > 0 && crossProd3 > 0)
                || (crossProd1 < 0 && crossProd2 < 0 && crossProd3 < 0);
        }
        
        /// <summary>
        /// This function will check if any vertex of a triangle is touching this triangle
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        /// https://stackoverflow.com/questions/2778240/detection-of-triangle-collision-in-2d-space
        public bool IsTouching(PTriangle other)
        {
            bool inPointOne = ContainsPoint(other.VertexOne);
            bool inPointTwo = ContainsPoint(other.VertexTwo);
            bool inPointThree = ContainsPoint(other.VertexThree);

            bool inPointFour = other.ContainsPoint(this.VertexOne);
            bool inPointFive = other.ContainsPoint(this.VertexTwo);
            bool inPointSix = other.ContainsPoint(this.VertexThree);

            if (inPointOne || inPointTwo || inPointThree)
            {
                return true;
            }

            if (inPointFour || inPointFive || inPointSix)
            {
                return true;
            }
            return false;
        }
    }
}
