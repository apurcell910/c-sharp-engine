using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine {
    class SpriteObj {
        internal int x, y, w, h;
        internal Color color;
        internal bool fill;
        internal bool disp;

        public SpriteObj(Rectangle rect, Color color, bool fill = true) {
            this.x = rect.X;
            this.y = rect.Y;
            this.w = rect.Width;
            this.h = rect.Height;
            this.color = color;
            this.fill = fill;
        }

        public SpriteObj(int x, int y, int w, int h, Color color, bool fill = true) {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.color = color;
            this.fill = fill;
            disp = false;
        }
    }
}
