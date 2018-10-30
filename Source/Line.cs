using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine {
    public class Line : Sprite {
        public Color color;

        public Line(Point p1, Point p2, Color color) {
            this.x = p1.X;
            this.y = p1.Y;
            //These are due to how it is stored in Sprite, to avoid confusion
            //Also helps for when moving the line, the entire line is moved
            this.w = p2.X - p1.X;
            this.h = p2.Y - p1.Y; 
            this.color = color;
            disp = false;
            alive = true;
            angle = 0; //Unused, just here so it doesn't complain;
            xAnchor = yAnchor = 0;
        }

        public Line(int x1, int y1, int x2, int y2, Color color) {
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

        public override void Draw(GraphicsManager graphics) {
            graphics.DrawLine(x, y, x + w, y + h, color);
        }
    }
}