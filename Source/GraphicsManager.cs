using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using SharpSlugsEngine.Physics;

namespace SharpSlugsEngine
{
    public class GraphicsManager
    {
        internal static readonly Vector2 defaultScale = new Vector2(100, 56.25f);

        private SolidBrush brush;
        private Pen pen;
        private Graphics formGraphics;

        private Bitmap buffer;
        private Graphics bitmapGraphics;

        private readonly Game game;
        private readonly Platform platform;

        public Vector2 WorldScale { get; private set; } = defaultScale;

        public Color BackColor
        {
            get => platform.form.BackColor;
            set => platform.form.BackColor = value;
        }

        internal GraphicsManager(Game game, Platform platform)
        {
            this.game = game;
            this.platform = platform;

            brush = new SolidBrush(Color.Red);
            pen = new Pen(brush);

            formGraphics = platform.form.CreateGraphics();
        }

        internal void RecreateGraphics()
        {
            formGraphics.Dispose();
            formGraphics = platform.form.CreateGraphics();
        }

        public Vector2 ToWorldScale(Vector2 unscaledVector)
        {
            return (unscaledVector * WorldScale) / game.Resolution;
        }

        public Vector2 ToResolutionScale(Vector2 scaledVector)
        {
            return (scaledVector / WorldScale) * game.Resolution;
        }

        internal void Begin()
        {
            foreach (Camera cam in GetCameras())
            {
                cam.Begin();
            }

            buffer = new Bitmap(platform.form.ClientSize.Width, platform.form.ClientSize.Height, formGraphics);
            bitmapGraphics = Graphics.FromImage(buffer);

            SetColor(BackColor);
            bitmapGraphics.FillRectangle(brush, new Rectangle(0, 0, buffer.Width, buffer.Height));
        }

        internal void End()
        {
            foreach (Camera cam in GetCameras())
            {
                DrawBMP(cam.buffer, cam.DrawPosition.X, cam.DrawPosition.Y, cam.DrawSize.X, cam.DrawSize.Y, 0, DrawType.Screen);
                cam.End();
            }

            formGraphics.DrawImage(buffer, 0, 0);
            buffer.Dispose();
            bitmapGraphics.Dispose();
        }

        internal void SetColor(Color color)
        {
            brush.Color = color;
            pen?.Dispose();
            pen = new Pen(brush);
        }

        internal void Rotate(PointF center, float angle)
        {
            Matrix m = new Matrix();
            m.RotateAt(angle, center);
            bitmapGraphics.Transform = m;
            m.Dispose();
        }

        internal void ResetRotation()
        {
            bitmapGraphics.ResetTransform();
        }

        private Camera[] GetCameras()
        {
            if (!game.Cameras.DisplayAll) return new Camera[] { game.Cameras.Main };

            return game.Cameras.All;
        }

        private RectangleF WorldToCameraRect(Camera cam, RectangleF rect)
        {
            return new RectangleF(cam.WorldToCameraPixels(rect.Location), cam.WorldToCameraPixels(rect.Size));
        }

        private RectangleF WorldToCameraRect(Camera cam, Vector2 pos, Vector2 size)
            => WorldToCameraRect(cam, new RectangleF(pos, size));

        //Feel like I should just make these take a SpriteObj as well. Maybe for later, shouldn't be too
        //difficult to do.

        //To draw rectangle at angle:
        //https://stackoverflow.com/questions/10210134/using-a-matrix-to-rotate-rectangles-individually
        /// <summary>
        /// The actual DrawRectangle function. All overloads should use this to draw
        /// </summary>
        private void DrawRectangle(Graphics g, int x, int y, int w, int h, Color color, bool fill = true, float angle = 0, double xAnchor = 0, double yAnchor = 0)
        {
            SetColor(color);
            Rotate(new PointF((float)(x + (w * xAnchor)), (float)(y + (h * yAnchor))), angle);

            if (fill)
            {
                g.FillRectangle(brush, new Rectangle(x, y, w, h));
            }
            else
            {
                g.DrawRectangle(pen, new Rectangle(x, y, w, h));
            }

            ResetRotation();
        }

        public void DrawRectangle(Vector2 pos, Vector2 size, Color color, bool fill = true, float angle = 0, double xAnchor = 0, double yAnchor = 0, DrawType type = DrawType.World)
        {
            if (type == DrawType.World)
            {
                foreach (Camera cam in GetCameras())
                {
                    Vector2 camPos = cam.WorldToCameraPixels(pos);
                    Vector2 camSize = cam.WorldToCameraPixels(size, true);
                    DrawRectangle(cam.bitmapGraphics, (int)camPos.X, (int)camPos.Y, (int)camSize.X, (int)camSize.Y, color, fill, angle, xAnchor, yAnchor);
                }
            }
            else
            {
                DrawRectangle(bitmapGraphics, (int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y, color, fill, angle, xAnchor, yAnchor);
            }
        }

        public void DrawRectangle(float x, float y, float w, float h, Color color, bool fill = true, float angle = 0, double xAnchor = 0, double yAnchor = 0, DrawType type = DrawType.World)
            => DrawRectangle(new Vector2(x, y), new Vector2(w, h), color, fill, angle, xAnchor, yAnchor, type);

        public void DrawRectangle(RectangleF rect, Color color, bool fill = true, float angle = 0, double xAnchor = 0, double yAnchor = 0, DrawType type = DrawType.World)
            => DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height, color, fill, angle, xAnchor, yAnchor, type);

        /// <summary>
        /// The actual DrawLine function. All overloads should use this to draw
        /// </summary>
        private void DrawLine(Graphics g, int x1, int y1, int x2, int y2, Color color)
        {
            SetColor(color);
            g.DrawLine(pen, x1, y1, x2, y2);
        }

        public void DrawLine(Vector2 v1, Vector2 v2, Color color, DrawType type = DrawType.World)
        {
            if (type == DrawType.World)
            {
                foreach (Camera cam in GetCameras())
                {
                    Vector2 camV1 = cam.WorldToCameraPixels(v1);
                    Vector2 camV2 = cam.WorldToCameraPixels(v2);
                    DrawLine(cam.bitmapGraphics, (int)camV1.X, (int)camV1.Y, (int)camV2.X, (int)camV2.Y, color);
                }
            } else
            {
                DrawLine(bitmapGraphics, (int)v1.X, (int)v1.Y, (int)v2.X, (int)v2.Y, color);
            }
        }

        public void DrawLine(float x1, float y1, float x2, float y2, Color color, DrawType type = DrawType.World)
            => DrawLine(new Vector2(x1, y1), new Vector2(x2, y2), color, type);

        /// <summary>
        /// The actual DrawEllipse function. All overloads should use this to draw
        /// </summary>
        private void DrawEllipse(Graphics g, int x, int y, int w, int h, Color color, bool fill = true, float r = 0, double xAnchor = 0.5f, double yAnchor = 0.5f)
        {
            SetColor(color);
            Rotate(new PointF((float)(x + w * xAnchor), (float)(y + h * yAnchor)), r);

            if (fill)
            {
                g.FillEllipse(brush, x, y, w, h);
            }
            else
            {
                g.DrawEllipse(pen, x, y, w, h);
            }

            ResetRotation();
        }

        public void DrawEllipse(Vector2 pos, Vector2 size, Color color, bool fill = true, float r = 0, double xAnchor = 0f, double yAnchor = 0f, DrawType type = DrawType.World)
        {
            if (type == DrawType.World)
            {
                foreach (Camera cam in GetCameras())
                {
                    Vector2 camPos = cam.WorldToCameraPixels(pos);
                    Vector2 camSize = cam.WorldToCameraPixels(size, true);

                    DrawEllipse(cam.bitmapGraphics, (int)camPos.X, (int)camPos.Y, (int)camSize.X, (int)camSize.Y, color, fill, r, xAnchor, yAnchor);
                }
            } else
            {
                DrawEllipse(bitmapGraphics, (int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y, color, fill, r, xAnchor, yAnchor);
            }
        }

        public void DrawEllipse(float x, float y, float w, float h, Color color, bool fill = true, float r = 0, double xAnchor = 0f, double yAnchor = 0f, DrawType type = DrawType.World)
            => DrawEllipse(new Vector2(x, y), new Vector2(w, h), color, fill, r, xAnchor, yAnchor, type);

        public void DrawCircle(Vector2 pos, float r, Color color, bool fill = true, DrawType type = DrawType.World)
        {
            Vector2 size = new Vector2(r * 2, r * 2);
            pos = new Vector2(pos.X - r, pos.Y - r);

            DrawEllipse(pos, size, color, fill, r, 0.5f, 0.5f, type);
        }

        public void DrawCircle(float x, float y, float r, Color color, bool fill = true, DrawType type = DrawType.World)
            => DrawCircle(new Vector2(x, y), r, color, fill, type);

        /// <summary>
        /// The actual DrawBMP function. All overloads should use this to draw
        /// </summary>
        private void DrawBMP(Graphics g, Bitmap bmp, int x, int y, int w, int h, float r, int ix, int iy, int iw, int ih)
        {
            Rotate(new PointF(x + w / 2f, y + h / 2f), r);
            
            g.DrawImage(bmp, new Rectangle(x, y, w, h), new Rectangle(ix, iy, iw, ih), GraphicsUnit.Pixel);
            ResetRotation();
        }

        public void DrawBMP(Bitmap bmp, float x, float y, float w, float h, float r = 0, DrawType type = DrawType.World)
        {
            if (type == DrawType.World)
            {
                Vector2 pos = new Vector2(x, y);
                Vector2 size = new Vector2(w, h);
                
                foreach (Camera cam in GetCameras())
                {
                    Vector2 camPos = cam.WorldToCameraPixels(pos);
                    Vector2 camSize = cam.WorldToCameraPixels(size, true);
                    DrawBMP(cam.bitmapGraphics, bmp, (int)camPos.X, (int)camPos.Y, (int)camSize.X, (int)camSize.Y, r, 0, 0, bmp.Width, bmp.Height);
                }
            } else
            {
                DrawBMP(bitmapGraphics, bmp, (int)x, (int)y, (int)w, (int)h, r, 0, 0, bmp.Width, bmp.Height);
            }
        }

        public void DrawBMP(Bitmap bmp, Vector2 position, Vector2 size, float r = 0, DrawType type = DrawType.World)
            => DrawBMP(bmp, position.X, position.Y, size.X, size.Y, r, type);

        public void DrawBMP(Bitmap bmp, RectangleF drawRect, float r = 0, DrawType type = DrawType.World)
            => DrawBMP(bmp, drawRect.X, drawRect.Y, drawRect.Width, drawRect.Height, r, type);

        public void DrawBMP(Bitmap bmp, float x, float y, float w, float h, int ix, int iy, int iw, int ih, float r = 0, DrawType type = DrawType.World) {
            if (type == DrawType.World) {
                Vector2 pos = new Vector2(x, y);
                Vector2 size = new Vector2(w, h);

                foreach (Camera cam in GetCameras()) {
                    Vector2 camPos = cam.WorldToCameraPixels(pos);
                    Vector2 camSize = cam.WorldToCameraPixels(size, true);
                    DrawBMP(cam.bitmapGraphics, bmp, (int)camPos.X, (int)camPos.Y, (int)camSize.X, (int)camSize.Y, r, ix, iy, iw, ih);
                }
            } else {
                DrawBMP(bitmapGraphics, bmp, (int)x, (int)y, (int)w, (int)h, r, ix, iy, iw, ih);
            }
        }

        //TODO: Find center point of triangle to support rotation
        /// <summary>
        /// The actual DrawTri function. All overloads should use this to draw
        /// </summary>
        private void DrawTri(Graphics g, int x1, int y1, int x2, int y2, int x3, int y3, Color color, bool fill = true, float r = 0)
        {
            SetColor(color);

            if (!fill)
            {
                g.DrawLine(pen, x1, y1, x2, y2);
                g.DrawLine(pen, x2, y2, x3, y3);
                g.DrawLine(pen, x3, y3, x1, y1);
            }
            else
            {
                g.FillPolygon(brush, new Point[] { new Point(x1, y1), new Point(x2, y2), new Point(x3, y3) });
            }
        }

        public void DrawTri(Vector2 v1, Vector2 v2, Vector2 v3, Color color, bool fill = true, float r = 0, DrawType type = DrawType.World)
        {
            if (type == DrawType.World)
            {
                foreach (Camera cam in GetCameras())
                {
                    Vector2 camV1 = cam.WorldToCameraPixels(v1);
                    Vector2 camV2 = cam.WorldToCameraPixels(v2);
                    Vector2 camV3 = cam.WorldToCameraPixels(v3);
                    DrawTri(cam.bitmapGraphics, (int)camV1.X, (int)camV1.Y, (int)camV2.X, (int)camV2.Y, (int)camV3.X, (int)camV3.Y, color, fill, r);
                }
            } else
            {
                DrawTri(bitmapGraphics, (int)v1.X, (int)v1.Y, (int)v2.X, (int)v2.Y, (int)v3.X, (int)v3.Y, color, fill, r);
            }
        }

        public void DrawTri(PTriangle tri, Color color, bool fill = true, float r = 0, DrawType type = DrawType.World)
            => DrawTri(tri.VertexOne, tri.VertexTwo, tri.VertexThree, color, fill, r, type);

        //TODO: Find center point of polygon to support rotation
        /// <summary>
        /// The actual DrawPolygon function. All overloads should use this to draw
        /// </summary>
        private void DrawPolygon(Graphics g, Point[] points, Color color, bool fill = true, float r = 0)
        {
            SetColor(color);

            if (fill)
            {
                g.FillPolygon(brush, points);
            }
            else
            {
                g.DrawPolygon(pen, points);
            }
        }

        public void DrawPolygon(Vector2[] vertices, Color color, bool fill = true, float r = 0, DrawType type = DrawType.World)
        {
            Point[] points = new Point[vertices.Length];
            if (type == DrawType.World)
            {
                foreach (Camera cam in GetCameras())
                {
                    for (int i = 0; i < points.Length; i++)
                    {
                        points[i] = (Point)cam.WorldToCameraPixels(vertices[i]);
                    }
                    DrawPolygon(cam.bitmapGraphics, points, color, fill, r);
                }
            } else
            {
                for (int i = 0; i < points.Length; i++)
                {
                    points[i] = (Point)vertices[i];
                }
                DrawPolygon(bitmapGraphics, points, color, fill, r);
            }
        }

        private Vector2 GetTextCoordinates(Vector2 pos, Vector2 size, TextAlign align)
        {
            float x = pos.X;
            float y = pos.Y;

            if (align == TextAlign.TopCenter || align == TextAlign.MiddleCenter || align == TextAlign.BottomCenter)
            {
                x = x - size.X / 2;
            }
            else if (align == TextAlign.TopRight || align == TextAlign.MiddleRight || align == TextAlign.BottomRight)
            {
                x = x - size.X;
            }

            if (align == TextAlign.MiddleLeft || align == TextAlign.MiddleCenter || align == TextAlign.MiddleRight)
            {
                y = y - size.Y / 2;
            }
            else if (align == TextAlign.BottomLeft || align == TextAlign.BottomCenter || align == TextAlign.BottomRight)
            {
                y = y - size.Y;
            }

            return new Vector2(x, y);
        }

        public void DrawText(string text, Font font, Color color, Vector2 pos, Vector2 size, float r = 0, TextAlign align = TextAlign.TopLeft, DrawType type = DrawType.World)
        {
            SetColor(color);
            
            Vector2 bmpSize = bitmapGraphics.MeasureString(text, font);
            using (Bitmap textBmp = new Bitmap((int)bmpSize.X, (int)bmpSize.Y))
            {
                using (Graphics textBmpGraphics = Graphics.FromImage(textBmp))
                {
                    textBmpGraphics.DrawString(text, font, brush, 0, 0);
                    DrawBMP(textBmp, GetTextCoordinates(pos, size, align), size, r, type);
                }
            }
        }

        public void SetWorldScale(Vector2 scaleFactor)
        {
            WorldScale = scaleFactor;
        }
    }

    public enum DrawType
    {
        Screen,
        World
    }

    public enum TextAlign
    {
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }
}
