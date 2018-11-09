namespace SharpSlugsEngine.Physics
{
    public struct Triangle
    {
        public Vector2 VertexOne { get; private set; }
        public Vector2 VertexTwo { get; private set; }
        public Vector2 VertexThree { get; private set; }

        public Triangle(Vector2 vert1, Vector2 vert2, Vector2 vert3)
        {
            VertexOne = vert1;
            VertexTwo = vert2;
            VertexThree = vert3;
        }
    }
}
