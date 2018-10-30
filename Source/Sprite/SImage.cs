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
        Bitmap image;
        public SImage(int x, int y, Bitmap image) {
            this.x = x;
            this.y = y;
            this.image = image;
            this.w = image.Width;
            this.h = image.Height;
            disp = false;
            alive = true;
            angle = 0;
            xAnchor = yAnchor = 0;
        }
        public SImage(int x, int y, string path) {
            this.x = x;
            this.y = y;
            this.image = new Bitmap(path);
            this.w = this.image.Width;
            this.h = this.image.Height;
            disp = false;
            alive = true;
            angle = 0;
            xAnchor = yAnchor = 0;
        }
        public SImage(int x, int y, int w, int h, Bitmap image) {
            this.x = x;
            this.y = y;
            this.image = image;
            this.w = w;
            this.h = h;
            disp = false;
            alive = true;
            angle = 0;
            xAnchor = yAnchor = 0;
        }
        public SImage(int x, int y, int w, int h, string path) {
            this.x = x;
            this.y = y;
            this.image = new Bitmap(path);
            this.w = w;
            this.h = h;
            disp = false;
            alive = true;
            angle = 0;
            xAnchor = yAnchor = 0;
        }

        public override void Draw(GraphicsManager graphics) {
            graphics.DrawBMP(image, x, y, w, h);
        }
    }
}
