using System;
using System.Collections.Generic;

namespace SharpSlugsEngine.Physics
{
    public struct pEllipse
    {
        public Vector2 center;
        public List<Triangle> Triangles;
        public float Area { get; private set; }

        private float findEllipseX(float angle, float a, float b)
        {
            angle = (float)(angle * Math.PI) / 180f;
            float x = (float)((a * b) / Math.Sqrt((Math.Pow(b, 2) + Math.Pow(a, 2) * Math.Pow(Math.Tan(angle), 2))));

            if((angle > -Math.PI / 2f && angle < Math.PI / 2f) || (angle > 1.5f * Math.PI))
            {
                return x;
            } else
            {
                return -x;
            }
        }

        private float findEllipseY(float angle, float a, float b)
        {
            angle = (float)(angle * Math.PI) / 180f;
            float y = (float)((a * b) / (Math.Sqrt(Math.Pow(a, 2) + (Math.Pow(b, 2) / (Math.Pow(Math.Tan(angle), 2))))));

            if (angle > -Math.PI / 2f && angle < Math.PI)
            {
                return y;
            }
            else
            {
                return -y;
            }
        }

        public pEllipse(Vector2 v1, float height, float width)
        {
            center = v1;
            Triangles = new List<Triangle>();
            int numTriangle = (int)(50);
            float genericAngle = 360 / (float)numTriangle;
            float nextAngle = 0;

            Area = (float)Math.PI * height * width;

            for (int i = 0; i < numTriangle; i++)
            {
                Triangles.Add(new Triangle(center, center + new Vector2(findEllipseX(nextAngle, width, height), findEllipseY(nextAngle, width, height)),
                        center + new Vector2(findEllipseX(nextAngle + genericAngle, width, height), findEllipseY(nextAngle + genericAngle, width, height))));
                nextAngle += genericAngle;
            }

            
        }
    }
}
