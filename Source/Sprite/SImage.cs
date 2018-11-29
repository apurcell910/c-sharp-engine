namespace SharpSlugsEngine
{
    /// <summary>
    /// Sprite Image class, for usage of external assets to use in a game.
    /// </summary>
    public class SImage : Sprite
    {
        public string image;
        public int ix, iy, iw, ih; // These are the portions of the source image to draw

        /// <summary>
        /// Initializes a new instance of the <see cref="SImage" /> class.
        /// </summary>
        /// <param name="x">X position of new sprite</param>
        /// <param name="y">Y position of new sprite</param>
        /// <param name="w">Width of new sprite</param>
        /// <param name="h">Height of new sprite.</param>
        /// <param name="image">Name of image to draw. Must have been added to Content Manager</param>
        /// <param name="ix">Portion of image to draw, upper left corner x position.</param>
        /// <param name="iy">Poriton of image to draw, upper right corner y position.</param>
        /// <param name="iw">Portion of image to draw, width of subimage.</param>
        /// <param name="ih">Portion of image to draw, height of subimage.</param>
        public SImage(Game game, double x, double y, double w, double h, string image, int ix = 0, int iy = 0, int iw = 0, int ih = 0)
        {
            this.game = game;
            this.x = x;
            this.y = y;
            this.image = image;
            this.w = w;
            this.h = h;
            this.ix = ix;
            this.iy = iy;
            this.iw = iw;
            this.ih = ih;
            disp = false;
            alive = true;
            angle = 0;
            xAnchor = yAnchor = 0;
            collider = new Physics.RectangleCollider(0, 0, w, h);

            collider.Position = new Vector2((float)x, (float)y);
        }

        /// <summary>
        /// Draw function for sprite
        /// </summary>
        /// <param name="graphics">Graphics manager to use.</param>
        public override void Draw(GraphicsManager graphics)
        {
            if (ix == 0 && iy == 0 && iw == 0 && ih == 0)
            {
                graphics.DrawBMP(image, (float)x, (float)y, (float)w, (float)h, angle);
            }
            else
            {
                graphics.DrawBMP(image, (float)x, (float)y, (float)w, (float)h, ix, iy, iw, ih, angle);
            }
        }
    }
}
