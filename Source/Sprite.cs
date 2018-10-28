using System;
using System.Collections;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine {
    public class Sprite
    {
        public int x, y, w, h;
        public float angle;
        public double xAnchor, yAnchor;
        public bool alive;
        public bool display;
        public virtual void Draw(GraphicsManager graphics) { }
        public virtual void Update() { }

        /// <summary>
        /// For any sprite that will never be used again, "kills" it.
        /// </summary>
        public void kill() {
            alive = false;
            display = false;
        }

        /// <summary>
        /// "Reincarnates" a dead sprite, must separately set display to true.
        /// </summary>
        public void reincarnate() {
            alive = true;
        }
    }
}
