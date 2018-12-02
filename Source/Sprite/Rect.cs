using System.Drawing;

namespace SharpSlugsEngine
{
    /// <summary>
    /// Rectangle Sprite, single color.
    /// </summary>
    public class Rect : Sprite
    {
        public Color color;
        public bool fill;

        /// <summary>
        /// Initializes a new instance of the <see cref="Rect"/> class.
        /// </summary>
        /// <param name="x">X position of new rectangle.</param>
        /// <param name="y">Y position of new rectangle.</param>
        /// <param name="w">Width of new rectangle.</param>
        /// <param name="h">Height of new rectangle.</param>
        /// <param name="color">Color of rectangle.</param>
        /// <param name="fill">Whether or not to fill the rectangle. True by default</param>
        public Rect(Game game, double x, double y, double w, double h, Color color, bool fill = true)
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
            collider = new Physics.RectangleCollider(0, 0, w, h);
        }

        /// <summary>
        /// Draws the rectangle.
        /// </summary>
        /// <param name="graphics">Graphics Manager to use.</param>
        public override void Draw(GraphicsManager graphics)
        {
            graphics.DrawRectangle((float)x, (float)y, (float)w, (float)h, color, fill, angle, xAnchor, yAnchor);
        }
    }
}
