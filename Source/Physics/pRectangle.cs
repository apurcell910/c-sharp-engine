using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine.Physics
{
    public struct PRectangle
    {
        public PTriangle TriOne;
        public PTriangle TriTwo;
        public float Area { get; private set; }

        public PRectangle(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4)
        {
            TriOne = new PTriangle(v1, v2, v3);
            TriTwo = new PTriangle(v3, v2, v4);
            Area = TriOne.Area + TriTwo.Area;
        }

        public PRectangle(Vector2 v1, Vector2 v2)
        {
            TriOne = new PTriangle(v1, v2, new Vector2(v2.X , v1.Y));
            TriTwo = new PTriangle(v1, v2, new Vector2(v1.X, v2.Y));
            Area = TriOne.Area + TriTwo.Area;
        }

        public bool ContainsPoint(Vector2 point)
        {
            return (TriOne.ContainsPoint(point) || TriTwo.ContainsPoint(point));
        }
    }
}
