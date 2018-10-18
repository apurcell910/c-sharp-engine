using System;
using System.Collections;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine {
    class Sprite {
        private Hashtable spr = new Hashtable();
        public Sprite() { }

        /// <summary>
        /// Add sprite to list of sprites
        /// </summary>
        /// <param name="key">The key value of the sprite, unique</param>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        /// <param name="fill"></param>
        public void add(String key, Rectangle rect, Color color, bool fill = true) => spr.Add(key, new SpriteObj(rect, color, fill));
        public void add(String key, int x1, int y1, int x2, int y2, Color color, bool fill = true) => spr.Add(key, new SpriteObj(x1, y1, x2, y2, color, fill));
    }
}
