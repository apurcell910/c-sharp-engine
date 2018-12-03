using System.Drawing;

namespace SharpSlugsEngine
{
    /// <summary>
    /// Line Sprite, one color
    /// </summary>
    public class Line : Sprite
    {
        public Color color;

        /// <summary>
        /// Initializes a new instance of the <see cref="Line"/> class.
        /// </summary>
        /// <param name="game">The game object using this sprite.</param>
        /// <param name="p1">First point of the line.</param>
        /// <param name="p2">Second point of the line.</param>
        /// <param name="color">Color of the line.</param>
        public Line(Game game, Point p1, Point p2, Color color)
        {
            this.game = game;
            this.x = p1.X;
            this.y = p1.Y;
            this.w = p2.X - p1.X;
            this.h = p2.Y - p1.Y; 
            this.color = color;
            disp = false;
            alive = true;
            angle = 0; // Unused, just here so it doesn't complain
            xAnchor = yAnchor = 0; // Again, unlikely to be used, but still in main, so here
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Line"/> class.
        /// </summary>
        /// <param name="game">The game object using this line.</param>
        /// <param name="x1">X of first point.</param>
        /// <param name="y1">Y of first point.</param>
        /// <param name="x2">X of second point.</param>
        /// <param name="y2">Y of second point.</param>
        /// <param name="color">Color of the line.</param>
        public Line(Game game, int x1, int y1, int x2, int y2, Color color)
        {
            this.game = game;
            this.x = x1;
            this.y = y1;
            this.w = x2 - x1;
            this.h = y2 - y1;
            this.color = color;
            disp = false;
            alive = true;
            angle = 0;
            xAnchor = yAnchor = 0;
        }

        /// <summary>
        /// Draws the line.
        /// </summary>
        /// <param name="graphics">Graphics manager to use.</param>
        public override void Draw(GraphicsManager graphics)
        {
            graphics.DrawLine((float)x, (float)y, (float)(x + w), (float)(y + h), color);
        }
    }
}