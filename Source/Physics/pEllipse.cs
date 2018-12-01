using System;
using System.Collections.Generic;

namespace SharpSlugsEngine.Physics
{
    /// <summary>
    /// Struct to create a physical Ellipse
    /// </summary>
    public struct PEllipse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PEllipse"/> struct
        /// </summary>
        /// <param name="v1">Center of the ellipse</param>
        /// <param name="height">Height of the ellipse</param>
        /// <param name="width">Width of the ellipse</param>
        public PEllipse(Vector2 v1, float height, float width)
        {
            Center = v1;
            Triangles = new List<PTriangle>();
            int numTriangle = (int)50;
            float genericAngle = 360 / (float)numTriangle;
            float nextAngle = 0;

            Area = (float)Math.PI * height * width;

            for (int i = 0; i < numTriangle; i++)
            {
                Triangles.Add(
                    new PTriangle(
                        Center, 
                        Center + new Vector2(FindEllipseX(nextAngle, width, height), FindEllipseY(nextAngle, width, height)),
                        Center + new Vector2(FindEllipseX(nextAngle + genericAngle, width, height), FindEllipseY(nextAngle + genericAngle, width, height))));

                nextAngle += genericAngle;
            }
        }

        /// <summary>
        /// Gets the area of the Ellipse
        /// </summary>
        public float Area { get; private set; }

        /// <summary>
        /// Gets center of the PEllipse
        /// </summary>
        public Vector2 Center { get; private set; }

        /// <summary>
        /// Gets list of triangles the Ellipse is made of
        /// </summary>
        public List<PTriangle> Triangles { get; private set; }

        /// <summary>
        /// Finds the X component of the edge of an ellipse given angle, width, and height
        /// </summary>
        /// <param name="angle">Angle from the center</param>
        /// <param name="a">Width of the ellipse</param>
        /// <param name="b">Height of the ellipse</param>
        /// <returns>Returns a float of the X coordinate of the edge of the Ellipse</returns>
        private float FindEllipseX(float angle, float a, float b)
        {
            angle = (float)(angle * Math.PI) / 180f;
            float x = (float)(a * b / Math.Sqrt(Math.Pow(b, 2) + (Math.Pow(a, 2) * Math.Pow(Math.Tan(angle), 2))));

            if ((angle > -Math.PI / 2f && angle < Math.PI / 2f) || (angle > 1.5f * Math.PI))
            {
                return x;
            }
            else
            {
                return -x;
            }
        }

        /// <summary>
        /// Finds the Y component of the edge of an ellipse given angle, width, and height
        /// </summary>
        /// <param name="angle">Angle from the center</param>
        /// <param name="a">Width of the ellipse</param>
        /// <param name="b">Height of the ellipse</param>
        /// <returns>Returns a float of the Y coordinate of the edge of the Ellipse</returns>
        private float FindEllipseY(float angle, float a, float b)
        {
            angle = (float)(angle * Math.PI) / 180f;
            float y = (float)(a * b / Math.Sqrt(Math.Pow(a, 2) + (Math.Pow(b, 2) / Math.Pow(Math.Tan(angle), 2))));

            if (angle > -Math.PI / 2f && angle < Math.PI)
            {
                return y;
            }
            else
            {
                return -y;
            }
        }
    }
}
