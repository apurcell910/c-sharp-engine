using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine.Physics
{
    public struct pRectangle
    {
        public Triangle TriOne;
        public Triangle TriTwo;
        public float Area { get; private set; }

        public pRectangle(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4)
        {
            TriOne = new Triangle(v1, v2, v3);
            TriTwo = new Triangle(v3, v2, v4);
            Area = TriOne.Area + TriTwo.Area;
        }

        public pRectangle(Vector2 v1, Vector2 v2)
        {
            TriOne = new Triangle(v1, v2, new Vector2(v2.X , v1.Y));
            TriTwo = new Triangle(v1, v2, new Vector2(v1.X, v2.Y));
            Area = TriOne.Area + TriTwo.Area;
        }

        public bool ContainsPoint(Vector2 point)
        {
            return (TriOne.ContainsPoint(point) || TriTwo.ContainsPoint(point));
        }
    }
}
