using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SharpSlugsEngine
{
    public class GraphicsManager
    {
        internal const int defaultScale = 100;

        private SolidBrush brush;
        private Pen pen;
        private Graphics formGraphics;

        private Bitmap buffer;
        private Graphics bitmapGraphics;

        private readonly Game game;
        private readonly Platform platform;
        private float scaleFactor = defaultScale;

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

        private Vector2 ToWorldScale(Vector2 unscaledVector)
        {
            return (unscaledVector * scaleFactor) / game.Resolution;
        }

        private Vector2 ToResolutionScale(Vector2 scaledVector)
        {
            return (scaledVector / scaleFactor) * game.Resolution;
        }

        internal void Begin()
        {
            buffer = new Bitmap(platform.form.Width, platform.form.Height, formGraphics);
            bitmapGraphics = Graphics.FromImage(buffer);

            brush.Color = platform.form.BackColor;
            bitmapGraphics.FillRectangle(brush, new Rectangle(0, 0, buffer.Width, buffer.Height));
        }

        internal void End()
        {
            formGraphics.DrawImage(buffer, 0, 0);
            buffer.Dispose();
            bitmapGraphics.Dispose();
        }


        //Feel like I should just make these take a SpriteObj as well. Maybe for later, shouldn't be too
        //difficult to do.

        //To draw rectangle at angle:
        //https://stackoverflow.com/questions/10210134/using-a-matrix-to-rotate-rectangles-individually
        public void DrawRectangle(Rectangle rect, Color color, bool fill = true, float angle = 0, double xAnchor = 0, double yAnchor = 0)
        {
            brush.Color = color;

            using (Matrix m = new Matrix()) {
                m.RotateAt(angle, new PointF((float)(rect.X + (rect.Width * xAnchor)), (float)(rect.Y + (rect.Height * yAnchor))));
                bitmapGraphics.Transform = m;
                if (fill)
                    bitmapGraphics.FillRectangle(brush, rect);
                else
                    bitmapGraphics.DrawRectangle(pen, rect);
                bitmapGraphics.ResetTransform();
            }
        }

        public void DrawRectangle(int x, int y, int w, int h, Color color, bool fill = true, float angle = 0, double xAnchor = 0, double yAnchor = 0)
            => DrawRectangle(new Rectangle(x, y, w, h), color, fill, angle, xAnchor, yAnchor);

        public void DrawRectangle(Vector2 worldScaledCenter, int w, int h, Color color, bool fill = true, float angle = 0, double xAnchor = 0, double yAnchor = 0)
        {
            Vector2 toResolution = ToResolutionScale(worldScaledCenter);

            Console.WriteLine(toResolution);

            DrawRectangle((int)toResolution.X, (int)toResolution.Y, w, h, color, fill, angle, xAnchor, yAnchor);
        }

        public void DrawLine(int a, int b, int x, int y, Color color)
        {
            brush.Color = color;
            pen?.Dispose();
            pen = new Pen(brush);
            bitmapGraphics.DrawLine(pen, a, b, x, y);
        }

        public void DrawLine(Point p1, Point p2, Color color)
            => DrawLine(p1.X, p1.Y, p2.X, p2.Y, color);

        public void DrawLine(Vector2 worldScaledA, Vector2 worldScaledB, Color color)
        {
            Vector2 toResoultionA = ToResolutionScale(worldScaledA);
            Vector2 toResoultionB = ToResolutionScale(worldScaledB);

            DrawLine((int)toResoultionA.X, (int)toResoultionA.Y, (int)toResoultionB.X, (int)toResoultionB.Y, color);
        }

        public void DrawCircle(int x, int y, int r, Color color, bool fill = true)
        {
            brush.Color = color;

            if (fill)
                bitmapGraphics.FillEllipse(brush, x - r, y - r, 2 * r, 2 * r);
            else
                bitmapGraphics.DrawEllipse(pen, x - r, y - r, 2 * r, 2 * r);
        }

        public void DrawCircle(Vector2 worldScaledCenter, int r, Color color, bool fill = true)
        {
            Vector2 resolutionCenter = ToResolutionScale(worldScaledCenter);
            DrawCircle((int)resolutionCenter.X, (int)resolutionCenter.Y, r, color, fill);
        }

        public void DrawCircle(Point p, int r, Color color, bool fill = true)
            => DrawCircle(p.X, p.Y, r, color, fill);

        //Other way to draw an ellipse by defining bounds with rectangle
        public void DrawEllipse(int x, int y, int w, int h, Color color, bool fill = true, float angle = 0, double xAnchor = 0, double yAnchor = 0)
        {
            brush.Color = color;

            using (Matrix m = new Matrix()) {
                m.RotateAt(angle, new PointF((float)(x + (w * xAnchor)), (float)(y + (h * yAnchor))));
                bitmapGraphics.Transform = m;
                if (fill)
                    bitmapGraphics.FillEllipse(brush, x, y, w, h);
                else
                    bitmapGraphics.DrawEllipse(pen, x, y, w, h);
                bitmapGraphics.ResetTransform();
            }
        }

        public void DrawEllipse(Vector2 worldScaledCenter, int w, int h, Color color, bool fill = true, float angle = 0, double xAnchor = 0, double yAnchor = 0)
        {
            Vector2 resolutionCenter = ToResolutionScale(worldScaledCenter);
            DrawEllipse((int)resolutionCenter.X, (int)resolutionCenter.Y, w, h, color, fill, angle, xAnchor, yAnchor);
        }

        public void DrawBMP(Bitmap bmp, int x, int y)
        {
            bitmapGraphics.DrawImage(bmp, x, y);
        }

        //Doing this for scaling purposes, if above is changed, change this as well
        public void DrawBMP(Bitmap bmp, int x, int y, int w, int h) {
            bitmapGraphics.DrawImage(bmp, x, y, w, h);
        }

        public void DrawBMP(Bitmap bmp, int x, int y, int w, int h, float r)
        {
            using (Matrix m = new Matrix())
            {
                m.RotateAt(r, new PointF(x + w / 2f, y + h / 2f));
                bitmapGraphics.Transform = m;
                bitmapGraphics.DrawImage(bmp, x, y, w, h);
                bitmapGraphics.ResetTransform();
            }
        }

        public void DrawBMP(Bitmap bmp, int x, int y, int w, int h, int ix, int iy, int iw, int ih) {
            
            bitmapGraphics.DrawImage(bmp, new Rectangle(x, y, w, h), ix, iy, iw, ih, GraphicsUnit.Pixel);
        }

        public void DrawBMP(Bitmap bmp, Vector2 worldScaled, int w, int h, int ix, int iy, int iw, int ih)
        {
            Vector2 resolutionScaled = ToResolutionScale(worldScaled);
            DrawBMP(bmp, (int)resolutionScaled.X, (int)resolutionScaled.Y, w, h, ix, iy, iw, ih);
        }
        
        public void SetWorldScale(float scaleFactor)
        {
            this.scaleFactor = scaleFactor;
        }

        public void printWorldToResolution(Vector2 worldPoint)
        {
            Console.WriteLine(ToResolutionScale(worldPoint));
        }
    }
}
