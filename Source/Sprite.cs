﻿using System;
using System.Collections;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine {
    public class Sprite {
        internal Dictionary<String, SpriteObj> spr = new Dictionary<String, SpriteObj>();
        private GraphicsManager Graphics;
        public Sprite(GraphicsManager Graphics) {
            this.Graphics = Graphics;
        }

        /// <summary>
        /// Add sprite to list of sprites
        /// </summary>
        /// <param name="key">The key value of the sprite, unique</param>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        /// <param name="fill"></param>
        public void add(String key, Rectangle rect, Color color, bool fill = true) => spr.Add(key, new SpriteObj(rect, color, fill));
        public void add(String key, int x1, int y1, int x2, int y2, Color color, bool fill = true) => spr.Add(key, new SpriteObj(x1, y1, x2, y2, color, fill));

        public void spriteDraw() {
            foreach (KeyValuePair<String, SpriteObj> obj in spr) {
                if (obj.Value.disp) {
                    Graphics.DrawRectangle(obj.Value.x, obj.Value.y, obj.Value.w, obj.Value.h, obj.Value.color, obj.Value.fill);
                }
            }
        }

        /// <summary>
        /// Change wheter or not to display the sprite
        /// </summary>
        /// <param name="key">The name of the sprite to edit</param>
        /// <param name="disp">true to display, false to not display.</param>
        public void display(string key, bool disp) {
            this.spr[key].disp = disp;
        }

        /// <summary>
        /// Move the sprite x and y pixels
        /// </summary>
        /// <param name="key">The name of the sprite to move</param>
        /// <param name="x">How much to change the x value by. A negative value moves the sprite to the left.</param>
        /// <param name="y">How much to change the y value by. A negative value moves the sprite up.</param>
        public void move(string key, int x, int y) {
            this.spr[key].x += x;
            this.spr[key].y += y;
        }

        /// <summary>
        /// Rotate the sprite by r (degrees or radians?)
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="r">How many (degrees/radians?) to rotate the sprite by.</param>
        public void rotate(string key, double r) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Scale the x value of the sprite
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="scale">How much to scale it by(decimal to make it smaller)</param>
        public void scaleX(string key, double scale) {
            spr[key].w = (int)Math.Round(spr[key].w * scale);
        }

        /// <summary>
        /// Scale the Y value of the sprite
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="scale">How much to scale it by(decimal to make it smaller)</param>
        public void scaleY(string key, double scale) {
            spr[key].h = (int)Math.Round(scale * spr[key].h);
        }

        /// <summary>
        /// Scale entire sprite
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="scale">How much to scale it by(decimal to make it smaller)</param>
        public void scale(string key, double scale) {
            spr[key].w = (int)Math.Round(spr[key].w * scale);
            spr[key].h = (int)Math.Round(scale * spr[key].h);
        }
    }
}