using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine.Physics
{
    /// <summary>
    /// Creates a Triangle Collider
    /// </summary>
    public class TriangleCollider : Collider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TriangleCollider"/> class using 3 verticies
        /// </summary>
        /// <param name="v1">Vertex 1 of the triangle</param>
        /// <param name="v2">Vertex 2 of the triangle</param>
        /// <param name="v3">Vertex 3 of the triangle</param>
        public TriangleCollider(Vector2 v1, Vector2 v2, Vector2 v3) : this(new PTriangle(v1, v2, v3))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TriangleCollider"/> class using a triangle
        /// </summary>
        /// <param name="tri">Triangle to use for creating Collider</param>
        public TriangleCollider(PTriangle tri)
        {
            TrianglesInternal = new PTriangle[] { tri };
            VerticesInternal = new Vector2[] { tri.VertexOne, tri.VertexTwo, tri.VertexThree };
        }

        /// <summary>
        /// Gets a new Triangle
        /// </summary>
        public PTriangle Triangle => Triangles[0];
    }
}
