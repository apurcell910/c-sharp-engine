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
        public int velocityX = 0;
        public int velocityY = 0;
        public int gravityX = 0;
        public int gravityY = 0;
        public float angle;
        public double xAnchor, yAnchor;
        public bool alive;
        public bool disp;
        public Physics.Collider collider;
        public List<string> collisions;
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
            collider.Position = new Vector2(this.x, this.y)
        }

        public void moveto(int x, int y)
        {
            this.x = x;
            this.y = y;
            collider.Position = new Vector2(x, y);
        }

        public void moveX(int x) {
            this.x += x;
            collider.Position = new Vector2(this.x, collider.Position.Y);
        }

        public void moveY(int y) {
            this.y += y;
            collider.Position = new Vector2(collider.Position.X, this.y);
        }

        /// <summary>
        /// Sets the sprite rotation to r
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="r">How many degrees to rotate the sprite by.</param>
        public void setRotation(float r) {
            this.angle = r;
            this.collider.Rotation = this.angle;
        }

        /// <summary>
        /// Rotate the sprite by r (degrees or radians?)
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="r">How many degrees to rotate the sprite by.</param>
        public void rotate(float r) {
            this.angle += r;
            this.collider.Rotation = this.angle;
        }

        /// <summary>
        /// Scale the x value of the sprite
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="scale">How much to scale it by(decimal to make it smaller)</param>
        public void scaleX(double scale) {
            w = (int)Math.Round(w * scale);
            collider.Scale = new Vector2(w, h);
        }

        /// <summary>
        /// Scale the Y value of the sprite
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="scale">How much to scale it by(decimal to make it smaller)</param>
        public void scaleY(double scale) {
            h = (int)Math.Round(scale * h);
            collider.Scale = new Vector2(w, h);
        }

        /// <summary>
        /// Scale entire sprite
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="scale">How much to scale it by(decimal to make it smaller)</param>
        public void scale(double scale) {
            w = (int)Math.Round(w * scale);
            h = (int)Math.Round(scale * h);
            collider.Scale = new Vector2(w, h);
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

        public void setVelocityX(int x) {
            velocityX = x;
            collider.Velocity = new Vector2(x, collider.Velocity.Y);
        }

        public void setVelocityY(int y) {
            velocityY = y;
            collider.Velocity = new Vector2(collider.Velocity.X, y);
        }

        public void addVelocityX(int x) {
            velocityX += x;
            collider.Velocity = new Vector2(velocityX, collider.Velocity.Y);
        }

        public void addVelocityY(int y) {
            velocityY += y;
            collider.Velocity = new Vector2(collider.Velocity.X, velocityY);
        }

        public void setGravityX(int x) {
            gravityX = x;
        }

        public void setGravityY(int y) {
            gravityY = y;
        }

        public void setCollider(Physics.Collider collider) {
            this.collider = collider;
        }

        public void addCollision(string collision) {
            this.collisions.Add(collision);
        }
    }
}
