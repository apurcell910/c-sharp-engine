namespace SharpSlugsEngine.Physics
{
    /// <summary>
    /// Ellipse collider class to use for ellipse shapes.
    /// </summary>
    public class EllipseCollider : Collider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EllipseCollider"/> class.
        /// </summary>
        /// <param name="x">Center x point of ellipse.</param>
        /// <param name="y">Center y point of ellipse.</param>
        /// <param name="w">Width of ellipse.</param>
        /// <param name="h">Height of ellipse.</param>
        public EllipseCollider(int x, int y, int w, int h)
        {
            TrianglesInternal = new PEllipse(new Vector2(x, y), h, w).Triangles.ToArray();
        }
    }
}
