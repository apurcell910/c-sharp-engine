using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine {
    public enum Shape {
        RECTANGLE,
        ELLIPSE,
        LINE,
        FILE
    }
    class SpriteObj {
        internal int x, y, w, h, scale;
        internal Color color;
        internal Shape type;
        internal bool fill;
        internal bool disp;
        internal float angle;
        internal double xAnchor, yAnchor;
        internal String path;

        public SpriteObj(Rectangle rect, Color color, Shape type, bool fill = true, float angle = 0) {
            this.x = rect.X;
            this.y = rect.Y;
            this.w = rect.Width;
            this.h = rect.Height;
            this.color = color;
            this.type = type;
            this.fill = fill;
            this.angle = angle;
            disp = false;
            xAnchor = yAnchor = 0;
        }

        public SpriteObj(int x, int y, int w, int h, Color color, Shape type, bool fill = true, float angle = 0) {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.color = color;
            this.type = type;
            this.fill = fill;
            this.angle = angle;
            disp = false;
            xAnchor = yAnchor = 0;
        }

        //HARPREET: THIS IS FOR THE CONTENT MANAGER
        public SpriteObj(int x, int y, int scale, String path, Shape type)
        {
            this.x = x;
            this.y = y;
            this.scale = scale;
            this.path = path;
            this.type = type;
        }
    }
}
