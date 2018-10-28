using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine {
    class Line : Sprite {
        public Color color;

        public Line(Point p1, Point p2, Color color) {
            this.x = p1.X;
            this.y = p1.Y;
            this.w = p2.X-p1.X;
            this.h = p2.Y-p1.Y; //These are due to how it is stored in Sprite, to avoid confusion
            this.color = color;
            display = false;
            alive = true;
            angle = 0; //Unused, just here so it doesn't complain;
            xAnchor = yAnchor = 0;
        }

        public override void Draw(GraphicsManager graphics) {
            graphics.DrawLine(x, y, x+w, y+h, color);
        }
    }
}
