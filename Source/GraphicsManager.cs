using System.Drawing;
using System.Drawing.Drawing2D;

namespace SharpSlugsEngine
{
    public class GraphicsManager
    {
        private SolidBrush brush;
        private Pen pen;
        private Graphics formGraphics;

        private Bitmap buffer;
        private Graphics bitmapGraphics;

        private readonly Game game;
        private readonly Platform platform;

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

        public void DrawLine(int a, int b, int x, int y, Color color)
        {
            brush.Color = color;
            bitmapGraphics.DrawLine(pen, a, b, x, y);
        }

        public void DrawLine(Point p1, Point p2, Color color)
            => DrawLine(p1.X, p1.Y, p2.X, p2.Y, color);

        public void DrawCircle(int x, int y, int r, Color color, bool fill = true)
        {
            brush.Color = color;

            if (fill)
                bitmapGraphics.FillEllipse(brush, x - r, y - r, 2 * r, 2 * r);
            else
                bitmapGraphics.DrawEllipse(pen, x - r, y - r, 2 * r, 2 * r);
        }

        //Other way to draw an ellipse by defining bounds with rectangle
        public void DrawEllipse(int x, int y, int w, int h, Color color, bool fill = true, float angle = 0, double xAnchor = 0, double yAnchor = 0) {
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

        public void DrawCircle(Point p, int r, Color color, bool fill = true)
            => DrawCircle(p.X, p.Y, r, color, fill);
    }
}
