using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine.Physics
{
    public class TriangleCollider : Collider
    {
        public Triangle triangle => Triangles[0];

        public TriangleCollider(Vector2 v1, Vector2 v2, Vector2 v3) : this(new Triangle(v1, v2, v3)) { }

        public TriangleCollider(Triangle tri)
        {
            Triangles = new Triangle[] { tri };
        }
    }
}
