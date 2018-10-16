using System.Drawing;

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

        internal void Begin()
        {
            buffer = new Bitmap(platform.form.Width, platform.form.Height, formGraphics);
            bitmapGraphics = Graphics.FromImage(buffer);
        }

        internal void End()
        {
            formGraphics.DrawImage(buffer, 0, 0);
            buffer.Dispose();
            bitmapGraphics.Dispose();
        }

        public void DrawRectangle(Rectangle rect, Color color, bool fill = true)
        {
            brush.Color = color;

            if (fill)
                bitmapGraphics.FillRectangle(brush, rect);
            else
                bitmapGraphics.DrawRectangle(pen, rect);
        }

        public void DrawRectangle(int x, int y, int w, int h, Color color)
            => DrawRectangle(new Rectangle(x, y, w, h), color);

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

        public void DrawCircle(Point p, int r, Color color, bool fill = true)
            => DrawCircle(p.X, p.Y, r, color, fill);
    }
}
