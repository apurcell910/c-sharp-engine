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
        public RectangleCollider(double x, double y, double w, double h)
        {
            VerticesInternal = new Vector2[]
            {
                new Vector2((float)x, (float)y),
                new Vector2((float)(x + w), (float)y),
                new Vector2((float)(x + w), (float)(y + h)),
                new Vector2((float)x, (float)(y + h))
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
