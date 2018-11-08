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
        public bool disp;
        public virtual void Draw(GraphicsManager graphics) { }
        public virtual void Update() { }

        /// <summary>
        /// For any sprite that will never be used again, "kills" it.
        /// </summary>
        public void kill() {
            alive = false;
            disp = false;
        }

        /// <summary>
        /// "Reincarnates" a dead sprite, must separately set display to true.
        /// </summary>
        public void reincarnate() {
            alive = true;
        }

        public void display(bool disp) {
            this.disp = disp;
        }

        public void move(int x, int y) {
            this.x += x;
            this.y += y;
        }

        public void moveto(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void moveX(int x) {
            this.x += x;
        }

        public void moveY(int y) {
            this.y += y;
        }

        /// <summary>
        /// Sets the sprite rotation to r
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="r">How many degrees to rotate the sprite by.</param>
        public void setRotation(float r) {
            this.angle = r;
        }

        /// <summary>
        /// Rotate the sprite by r (degrees or radians?)
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="r">How many degrees to rotate the sprite by.</param>
        public void rotate(float r) {
            this.angle += r;
        }

        /// <summary>
        /// Scale the x value of the sprite
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="scale">How much to scale it by(decimal to make it smaller)</param>
        public void scaleX(double scale) {
            w = (int)Math.Round(w * scale);
        }

        /// <summary>
        /// Scale the Y value of the sprite
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="scale">How much to scale it by(decimal to make it smaller)</param>
        public void scaleY(double scale) {
            h = (int)Math.Round(scale * h);
        }

        /// <summary>
        /// Scale entire sprite
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="scale">How much to scale it by(decimal to make it smaller)</param>
        public void scale(double scale) {
            w = (int)Math.Round(w * scale);
            h = (int)Math.Round(scale * h);
        }

        public void setAnchor(double anchor) {
            xAnchor = anchor;
            yAnchor = anchor;
        }

        public void setAnchorX(double anchor) {
            xAnchor = anchor;
        }

        public void setAnchorY(double anchor) {
            yAnchor = anchor;
        }
    }
}
