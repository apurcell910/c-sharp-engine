using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine.Physics
{
    /// <summary>
    /// Creates a physical rectangle
    /// </summary>
    public struct PRectangle
    {
        public PTriangle TriOne;
        public PTriangle TriTwo;

        /// <summary>
        /// Initializes a new instance of the <see cref="PRectangle"/> struct using 4 verticies
        /// </summary>
        /// <param name="v1">Top left corner</param>
        /// <param name="v2">Top right corner</param>
        /// <param name="v3">Bottom left corner</param>
        /// <param name="v4">Bottom right corner</param>
        public PRectangle(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4)
        {
            TriOne = new PTriangle(v1, v2, v3);
            TriTwo = new PTriangle(v3, v2, v4);
            Area = TriOne.Area + TriTwo.Area;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PRectangle"/> struct using 2 verticies
        /// </summary>
        /// <param name="v1">Top left corner</param>
        /// <param name="v2">Bottom right corner</param>
        public PRectangle(Vector2 v1, Vector2 v2)
        {
            TriOne = new PTriangle(v1, v2, new Vector2(v2.X, v1.Y));
            TriTwo = new PTriangle(v1, v2, new Vector2(v1.X, v2.Y));
            Area = TriOne.Area + TriTwo.Area;
        }

        /// <summary>
        /// Gets an area for the Rectangle
        /// </summary>
        public float Area { get; private set; }

        /// <summary>
        /// Checks if a certain point exists within the Rect
        /// </summary>
        /// <param name="point">Point to check</param>
        /// <returns>True if point is in rectangle, false otherwise</returns>
        public bool ContainsPoint(Vector2 point)
        {
            return TriOne.ContainsPoint(point) || TriTwo.ContainsPoint(point);
        }
    }
}
