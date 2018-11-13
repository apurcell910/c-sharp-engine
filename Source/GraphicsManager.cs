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
            buffer = new Bitmap(platform.form.Width, platform.form.Height, formGraphics);
            bitmapGraphics = Graphics.FromImage(buffer);

            SetColor(BackColor);
            bitmapGraphics.FillRectangle(brush, new Rectangle(0, 0, buffer.Width, buffer.Height));
        }

        internal void End()
        {
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

        //Feel like I should just make these take a SpriteObj as well. Maybe for later, shouldn't be too
        //difficult to do.

        //To draw rectangle at angle:
        //https://stackoverflow.com/questions/10210134/using-a-matrix-to-rotate-rectangles-individually
        /// <summary>
        /// The actual DrawRectangle function. All overloads should use this to draw
        /// </summary>
        internal void DrawRectangle(int x, int y, int w, int h, Color color, bool fill = true, float angle = 0, double xAnchor = 0, double yAnchor = 0)
        {
            SetColor(color);
            Rotate(new PointF((float)(x + (w * xAnchor)), (float)(y + (h * yAnchor))), angle);

            if (fill)
            {
                bitmapGraphics.FillRectangle(brush, new Rectangle(x, y, w, h));
            }
            else
            {
                bitmapGraphics.DrawRectangle(pen, new Rectangle(x, y, w, h));
            }

            ResetRotation();
        }

        public void DrawRectangle(Vector2 pos, Vector2 size, Color color, bool fill = true, float angle = 0, double xAnchor = 0, double yAnchor = 0, DrawType type = DrawType.World)
        {
            if (type == DrawType.World)
            {
                pos = ToResolutionScale(pos);
                size = ToResolutionScale(size);
            }

            DrawRectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y, color, fill, angle, xAnchor, yAnchor);
        }

        public void DrawRectangle(float x, float y, float w, float h, Color color, bool fill = true, float angle = 0, double xAnchor = 0, double yAnchor = 0, DrawType type = DrawType.World)
            => DrawRectangle(new Vector2(x, y), new Vector2(w, h), color, fill, angle, xAnchor, yAnchor, type);

        public void DrawRectangle(RectangleF rect, Color color, bool fill = true, float angle = 0, double xAnchor = 0, double yAnchor = 0, DrawType type = DrawType.World)
            => DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height, color, fill, angle, xAnchor, yAnchor, type);

        /// <summary>
        /// The actual DrawLine function. All overloads should use this to draw
        /// </summary>
        internal void DrawLine(int x1, int y1, int x2, int y2, Color color)
        {
            SetColor(color);
            bitmapGraphics.DrawLine(pen, x1, y1, x2, y2);
        }

        public void DrawLine(Vector2 v1, Vector2 v2, Color color, DrawType type = DrawType.World)
        {
            if (type == DrawType.World)
            {
                v1 = ToResolutionScale(v1);
                v2 = ToResolutionScale(v2);
            }

            DrawLine((int)v1.X, (int)v1.Y, (int)v2.X, (int)v2.Y, color);
        }

        public void DrawLine(float x1, float y1, float x2, float y2, Color color, DrawType type = DrawType.World)
            => DrawLine(new Vector2(x1, y1), new Vector2(x2, y2), color, type);

        /// <summary>
        /// The actual DrawEllipse function. All overloads should use this to draw
        /// </summary>
        internal void DrawEllipse(int x, int y, int w, int h, Color color, bool fill = true, float r = 0, double xAnchor = 0.5f, double yAnchor = 0.5f)
        {
            SetColor(color);
            Rotate(new PointF((float)(x + w * xAnchor), (float)(y + h * yAnchor)), r);

            if (fill)
            {
                bitmapGraphics.FillEllipse(brush, x, y, w, h);
            }
            else
            {
                bitmapGraphics.DrawEllipse(pen, x, y, w, h);
            }

            ResetRotation();
        }

        public void DrawEllipse(Vector2 pos, Vector2 size, Color color, bool fill = true, float r = 0, double xAnchor = 0f, double yAnchor = 0f, DrawType type = DrawType.World)
        {
            if (type == DrawType.World)
            {
                pos = ToResolutionScale(pos);
                size = ToResolutionScale(size);
            }

            DrawEllipse((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y, color, fill, r, xAnchor, yAnchor);
        }

        public void DrawEllipse(float x, float y, float w, float h, Color color, bool fill = true, float r = 0, float xAnchor = 0f, float yAnchor = 0f, DrawType type = DrawType.World)
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
        internal void DrawBMP(Bitmap bmp, int x, int y, int w, int h, float r, int ix, int iy, int iw, int ih)
        {
            Rotate(new PointF(x + w / 2f, y + h / 2f), r);
            bitmapGraphics.DrawImage(bmp, new Rectangle(x, y, w, h), new Rectangle(ix, iy, iw, ih), GraphicsUnit.Pixel);
            ResetRotation();
        }

        public void DrawBMP(Bitmap bmp, float x, float y, float w, float h, float r = 0, DrawType type = DrawType.World)
        {
            if (type == DrawType.World)
            {
                Vector2 position = ToResolutionScale(new Vector2(x, y));
                Vector2 size = ToResolutionScale(new Vector2(w, h));

                x = position.X;
                y = position.Y;
                w = size.X;
                h = size.Y;
            }

            DrawBMP(bmp, (int)x, (int)y, (int)w, (int)h, r, 0, 0, bmp.Width, bmp.Height);
        }

        public void DrawBMP(Bitmap bmp, Vector2 position, Vector2 size, float r = 0, DrawType type = DrawType.World)
            => DrawBMP(bmp, position.X, position.Y, size.X, size.Y, r, type);

        public void DrawBMP(Bitmap bmp, RectangleF drawRect, float r = 0, DrawType type = DrawType.World)
            => DrawBMP(bmp, drawRect.X, drawRect.Y, drawRect.Width, drawRect.Height, r, type);

        //TODO: Find center point of triangle to support rotation
        /// <summary>
        /// The actual DrawTri function. All overloads should use this to draw
        /// </summary>
        internal void DrawTri(int x1, int y1, int x2, int y2, int x3, int y3, Color color, bool fill = true, float r = 0)
        {
            if (!fill)
            {
                DrawLine(x1, y1, x2, y2, color);
                DrawLine(x2, y2, x3, y3, color);
                DrawLine(x3, y3, x1, y1, color);
            }
            else
            {
                SetColor(color);
                bitmapGraphics.FillPolygon(brush, new Point[] { new Point(x1, y1), new Point(x2, y2), new Point(x3, y3) });
            }
        }

        public void DrawTri(Vector2 v1, Vector2 v2, Vector2 v3, Color color, bool fill = true, float r = 0, DrawType type = DrawType.World)
        {
            if (type == DrawType.World)
            {
                v1 = ToResolutionScale(v1);
                v2 = ToResolutionScale(v2);
                v3 = ToResolutionScale(v3);
            }

            DrawTri((int)v1.X, (int)v1.Y, (int)v2.X, (int)v2.Y, (int)v3.X, (int)v3.Y, color, fill, r);
        }

        public void DrawTri(Triangle tri, Color color, bool fill = true, float r = 0, DrawType type = DrawType.World)
            => DrawTri(tri.VertexOne, tri.VertexTwo, tri.VertexThree, color, fill, r, type);

        //TODO: Find center point of polygon to support rotation
        /// <summary>
        /// The actual DrawPolygon function. All overloads should use this to draw
        /// </summary>
        internal void DrawPolygon(Point[] points, Color color, bool fill = true, float r = 0)
        {
            SetColor(color);

            if (fill)
            {
                bitmapGraphics.FillPolygon(brush, points);
            }
            else
            {
                bitmapGraphics.DrawPolygon(pen, points);
            }
        }

        public void DrawPolygon(Vector2[] vertices, Color color, bool fill = true, float r = 0, DrawType type = DrawType.World)
        {
            Point[] points = new Point[vertices.Length];
            for (int i = 0; i < points.Length; i++)
            {
                if (type == DrawType.World)
                {
                    points[i] = (Point)ToResolutionScale(vertices[i]);
                }
                else
                {
                    points[i] = (Point)vertices[i];
                }
            }

            DrawPolygon(points, color, fill, r);
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
}
