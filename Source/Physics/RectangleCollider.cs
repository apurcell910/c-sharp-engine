namespace SharpSlugsEngine.Physics
{
    /// <summary>
    /// Class to provide colliders for rectangular objects.
    /// </summary>
    public class RectangleCollider : Collider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleCollider"/> class.
        /// </summary>
        /// <param name="x">X value of upper left hand corner.</param>
        /// <param name="y">Y value of upper left hand corner.</param>
        /// <param name="w">Width of rectangle.</param>
        /// <param name="h">Height of rectangle.</param>
        public RectangleCollider(int x, int y, int w, int h)
        {
            VerticesInternal = new Vector2[]
            {
                new Vector2(x, y),
                new Vector2(x + w, y),
                new Vector2(x + w, y + h),
                new Vector2(x, y + h)
            };
            
            PRectangle rectangle = new PRectangle(Vertices[0], Vertices[1], Vertices[2], Vertices[3]);

            TrianglesInternal = new PTriangle[]
            {
                rectangle.TriOne,
                rectangle.TriTwo
            };
        }
    }
}
