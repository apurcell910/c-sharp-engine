using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine {
    class SpriteObj {
        private Rectangle rect;
        private Color color;
        private bool fill;
        private bool disp;

        public SpriteObj(Rectangle rect, Color color, bool fill = true) {
            this.rect = rect;
            this.color = color;
            this.fill = fill;
        }

        public SpriteObj(int x1, int y1, int x2, int y2, Color color, bool fill = true) {
            this.rect.X = x1;
            this.rect.Y = y1;
            this.rect.Width = x2;
            this.rect.Height = y2;
            this.color = color;
            this.fill = fill;
        }

        /// <summary>
        /// Changes wheter or not to display the sprite
        /// </summary>
        /// <param name="disp">If true, display the sprite, if false, don't</param>
        public void display(bool disp) {
            this.disp = disp;
        }
    }
}
