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
        public EllipseCollider(double x, double y, double w, double h)
        {
            TrianglesInternal = new PEllipse(new Vector2((float)x, (float)y), (float)h, (float)w).Triangles.ToArray();

            VerticesInternal = new Vector2[TrianglesInternal.Length];
            for (int i = 0; i < VerticesInternal.Length; i++)
            {
                VerticesInternal[i] = TrianglesInternal[i].VertexTwo;
            }
        }
    }
}
