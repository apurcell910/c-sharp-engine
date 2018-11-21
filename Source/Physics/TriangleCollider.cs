using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine.Physics
{
    public class TriangleCollider : Collider
    {
        public PTriangle triangle => Triangles[0];

        public TriangleCollider(Vector2 v1, Vector2 v2, Vector2 v3) : this(new PTriangle(v1, v2, v3)) { }

        public TriangleCollider(PTriangle tri)
        {
            TrianglesInternal = new PTriangle[] { tri };
            VerticesInternal = new Vector2[] { tri.VertexOne, tri.VertexTwo, tri.VertexThree };
        }
    }
}
