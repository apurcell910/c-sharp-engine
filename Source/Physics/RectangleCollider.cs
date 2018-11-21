using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine.Physics
{
    class RectangleCollider : Collider
    {
        public RectangleCollider(int x, int y, int w, int h) {

            Vertices = new Vector2[]
            {
                new Vector2(x, y),
                new Vector2(x + w, y),
                new Vector2(x + w, y + h),
                new Vector2(x, y + h)
            };
            
            PRectangle rectangle = new PRectangle(Vertices[0], Vertices[1], Vertices[2], Vertices[3]);

            Triangles = new PTriangle[]
            {
                rectangle.TriOne,
                rectangle.TriTwo
            };
        }
    }
}
