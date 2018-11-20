using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine {
    //Name is to distingish from System.Drawing.Image
    public class SImage : Sprite {
        string image;
        int ix, iy, iw, ih; //These are the portions of the source image to draw
        ContentManager manager;
        public SImage(int x, int y, string image, ref ContentManager manager, int ix = 0, int iy = 0, int iw = 0, int ih = 0) {
            this.x = x;
            this.y = y;
            this.image = image;
            this.manager = manager;
            this.w = manager.GetImage(image).Width;
            this.h = manager.GetImage(image).Height;
            this.ix = ix;
            this.iy = iy;
            this.iw = iw;
            this.ih = ih;
            disp = false;
            alive = true;
            angle = 0;
            xAnchor = yAnchor = 0;
            collider = new Physics.RectangleCollider(x, y, w, h);
        }

        public SImage(int x, int y, int w, int h, string image, ref ContentManager manager, int ix = 0, int iy = 0, int iw = 0, int ih = 0) {
            this.x = x;
            this.y = y;
            this.image = image;
            this.manager = manager;
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
            collider = new Physics.RectangleCollider(x, y, w, h);
        }

        public override void Draw(GraphicsManager graphics) {
            if (ix == 0 && iy == 0 && iw == 0 && ih == 0) {
                graphics.DrawBMP(image, x, y, w, h, angle);
            } else {
                graphics.DrawBMP(image, x, y, w, h, ix, iy, iw, ih, angle);
            }
        }
    }
}
