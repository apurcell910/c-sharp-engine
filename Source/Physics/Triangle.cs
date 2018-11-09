using System;

namespace SharpSlugsEngine.Physics
{
    public struct Triangle
    {
        public Vector2 VertexOne { get; private set; }
        public Vector2 VertexTwo { get; private set; }
        public Vector2 VertexThree { get; private set; }
        public float Area { get; private set; }

        public Triangle(Vector2 vert1, Vector2 vert2, Vector2 vert3)
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
    }
}
