using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine.Physics
{
    class EllipseCollider : Collider
    {
        public EllipseCollider(int x, int y, int w, int h) {
            Triangles = new PEllipse(new Vector2(x, y), h, w).Triangles.ToArray();
        }
    }
}
