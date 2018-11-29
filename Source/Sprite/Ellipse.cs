using System.Drawing;

namespace SharpSlugsEngine
{
    /// <summary>
    /// Ellipse sprite class. One color.
    /// </summary>
    public class Ellipse : Sprite
    {
        public Color color;
        public bool fill;

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipse"/> class.
        /// </summary>
        /// <param name="x">Upper left hand corner x position.</param>
        /// <param name="y">Upper left hand corner y position.</param>
        /// <param name="w">Width of ellipse.</param>
        /// <param name="h">Height of ellipse.</param>
        /// <param name="color">Color of ellipse.</param>
        /// <param name="fill">Whether or not to fill the ellipse.</param>
        public Ellipse(Game game, double x, double y, double w, double h, Color color, bool fill = true)
        {
            this.game = game;
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.color = color;
            this.fill = fill;
            disp = false;
            alive = true;
            angle = 0;
            xAnchor = yAnchor = 0;
            collider = new Physics.EllipseCollider(0, 0, w, h);
        }

        /// <summary>
        /// Draw function for the ellipse.
        /// </summary>
        /// <param name="graphics">Graphics manager to use.</param>
        public override void Draw(GraphicsManager graphics)
        {
            graphics.DrawEllipse((float)x, (float)y, (float)w, (float)h, color, fill, angle, xAnchor, yAnchor);
        }
    }
}
