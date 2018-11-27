using System;
using System.Collections.Generic;

namespace SharpSlugsEngine
{
    /// <summary>
    /// Overridable sprite class. Not meant to be used on it's own, is instead intended to
    /// be overridden by the user as needed for additional kinds of sprites.
    /// </summary>
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

        /// <summary>
        /// Overridable Draw() function that is looped.
        /// </summary>
        /// <param name="graphics">Graphics manager of the sprite.</param>
        public virtual void Draw(GraphicsManager graphics) { }

        /// <summary>
        /// Overridable Update() function that is looped.
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// For any sprite that will never be used again, "kills" it.
        /// </summary>
        public void Kill()
        {
            alive = false;
            disp = false;
        }

        /// <summary>
        /// "Reincarnates" a dead sprite, must separately set display to true.
        /// </summary>
        public void Reincarnate() 
        {
            alive = true;
        }

        /// <summary>
        /// Sets the display value of the sprite
        /// </summary>
        /// <param name="disp">True to display, false to hide.</param>
        public void Display(bool disp) 
        {
            this.disp = disp;
        }

        /// <summary>
        /// Moves the sprite
        /// </summary>
        /// <param name="x">Value to move the x value by.</param>
        /// <param name="y">Value to move the y value by.</param>
        public void Move(int x, int y) 
        {
            this.x += x;
            this.y += y;
            collider.Position = new Vector2(this.x, this.y);
        }

        /// <summary>
        /// Moves the sprite to a specific location
        /// </summary>
        /// <param name="x">New value of x.</param>
        /// <param name="y">New value of y.</param>
        public void MoveTo(int x, int y)
        {
            this.x = x;
            this.y = y;
            collider.Position = new Vector2(x, y);
        }

        /// <summary>
        /// Moves the sprite vertically
        /// </summary>
        /// <param name="x">Amount to move the sprite horizontally by.</param>
        public void MoveX(int x) 
        {
            this.x += x;
            collider.Position = new Vector2(this.x, collider.Position.Y);
        }

        /// <summary>
        /// Moves the sprite vertically.
        /// </summary>
        /// <param name="y">Amount to move the sprite vertically by.</param>
        public void MoveY(int y) 
        {
            this.y += y;
            collider.Position = new Vector2(collider.Position.X, this.y);
        }

        /// <summary>
        /// Sets the sprite rotation to r
        /// </summary>
        /// <param name="r">How many degrees to rotate the sprite by.</param>
        public void SetRotation(float r) 
        {
            this.angle = r;
            this.collider.Rotation = this.angle;
        }

        /// <summary>
        /// Rotate the sprite by r (degrees or radians?)
        /// </summary>
        /// <param name="r">How many degrees to rotate the sprite by.</param>
        public void Rotate(float r) 
        {
            this.angle += r;
            this.collider.Rotation = this.angle;
        }

        /// <summary>
        /// Scale the x value of the sprite
        /// </summary>
        /// <param name="scale">How much to scale it by(decimal to make it smaller)</param>
        public void ScaleX(double scale) 
        {
            w = (int)Math.Round(w * scale);
            collider.Scale = new Vector2(w, h);
        }

        /// <summary>
        /// Scale the Y value of the sprite
        /// </summary>
        /// <param name="scale">How much to scale it by(decimal to make it smaller)</param>
        public void ScaleY(double scale) 
        {
            h = (int)Math.Round(scale * h);
            collider.Scale = new Vector2(w, h);
        }

        /// <summary>
        /// Scale entire sprite
        /// </summary>
        /// <param name="scale">How much to scale it by(decimal to make it smaller)</param>
        public void Scale(double scale) 
        {
            w = (int)Math.Round(w * scale);
            h = (int)Math.Round(scale * h);
            collider.Scale = new Vector2(w, h);
        }

        /// <summary>
        /// Sets the anchor of the sprite. Mainly for rotation right now.
        /// </summary>
        /// <param name="anchor">Value from 0.0 to 1.0, in terms of percentage to set anchor</param>
        public void SetAnchor(double anchor) 
        {
            xAnchor = anchor;
            yAnchor = anchor;
        }

        /// <summary>
        /// Sets the x anchor of the sprite.
        /// </summary>
        /// <param name="anchor">Value from 0.0 to 1.0, in terms of percentage to set anchor</param>
        public void SetAnchorX(double anchor) 
        {
            xAnchor = anchor;
        }

        /// <summary>
        /// Sets the y anchor of the sprite.
        /// </summary>
        /// <param name="anchor">Value from 0.0 to 1.0, in terms of percentage to set anchor</param>
        public void SetAnchorY(double anchor) 
        {
            yAnchor = anchor;
        }

        /// <summary>
        /// Sets the vertical velocity of the sprite.
        /// </summary>
        /// <param name="x">Amount to set the vertical velocity to.</param>
        public void SetVelocityX(int x) 
        {
            velocityX = x;
            collider.Velocity = new Vector2(x, collider.Velocity.Y);
        }

        /// <summary>
        /// Sets the horizontal velocity of the sprite.
        /// </summary>
        /// <param name="y">Amount to set the horizontal velocity to.</param>
        public void SetVelocityY(int y) 
        {
            velocityY = y;
            collider.Velocity = new Vector2(collider.Velocity.X, y);
        }

        /// <summary>
        /// Adds to the vertical velocity of the sprite.
        /// </summary>
        /// <param name="x">Amount to add to the vertical velocity.</param>
        public void AddVelocityX(int x) 
        {
            velocityX += x;
            collider.Velocity = new Vector2(velocityX, collider.Velocity.Y);
        }

        /// <summary>
        /// Adds to the horizontal velocity of the sprite.
        /// </summary>
        /// <param name="y">Amount to add to the horizontal velocity.</param>
        public void AddVelocityY(int y) 
        {
            velocityY += y;
            collider.Velocity = new Vector2(collider.Velocity.X, velocityY);
        }

        /// <summary>
        /// Sets the vertical gravity of the sprite.
        /// </summary>
        /// <param name="x">Amount to set the vertical gravity to.</param>
        public void SetGravityX(int x) 
        {
            gravityX = x;
        }

        /// <summary>
        /// Sets the horizontal gravity of the sprite.
        /// </summary>
        /// <param name="y">Amount to set the horizontal gravity to.</param>
        public void SetGravityY(int y) 
        {
            gravityY = y;
        }

        /// <summary>
        /// Sets the collider of the sprite.
        /// </summary>
        /// <param name="collider">Collider to set.</param>
        public void SetCollider(Physics.Collider collider) 
        {
            this.collider = collider;
        }

        /// <summary>
        /// Adds other sprite as collision for this sprite.
        /// </summary>
        /// <param name="collision">Name of other sprite to check for collision.</param>
        public void AddCollision(string collision) 
        {
            this.collisions.Add(collision);
        }
    }
}
