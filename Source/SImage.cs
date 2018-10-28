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
            this.w = 0;//This and h not needed for now, but could be useful;
            this.h = 0;
            this.image = image;
            display = false;
            alive = true;
            angle = 0;
            xAnchor = yAnchor = 0;
        }
        /*public Image(int x, int y, string path) {
            this.x = x;
            this.y = y;
            this.w = 0;//This and h not needed for now, but could be useful;
            this.h = 0;
            


            display = false;
            alive = true;
            angle = 0;
            xAnchor = yAnchor = 0;
        }*/

        public override void Draw(GraphicsManager graphics) {
            graphics.DrawBMP(image, x, y);
        }
    }
}
