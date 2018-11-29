using System.Collections.Generic;

namespace SharpSlugsEngine 
{
    /// <summary>
    /// Contains the sprites used within the game world itself, automatically called to update and draw each draw/update cycle.
    /// </summary>
    public class SpriteList
    {
        private Game game;
        private Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
        private GraphicsManager graphics;
        private Physics.MovementManager movement = new Physics.MovementManager();

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteList" /> class.
        /// </summary>
        /// <param name="graphics">Graphics manager to use</param>
        public SpriteList(GraphicsManager graphics, Game game)
        {
            this.graphics = graphics;
            this.game = game;
        }

        /// <summary>
        /// Function to update the sprites. Not called by user.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            movement.UpdateSprites(ref sprites);
            foreach (KeyValuePair<string, Sprite> obj in sprites)
            {
                if (obj.Value.alive)
                {
                    obj.Value.Update(gameTime);
                }
            }
        }

        /// <summary>
        /// Function to draw the sprites. Not called by user.
        /// </summary>
        public void Draw()
        {
            foreach (KeyValuePair<string, Sprite> obj in sprites)
            {
                if (obj.Value.disp && obj.Value.alive)
                {
                    obj.Value.Draw(graphics);
                }
            }
        }

        /// <summary>
        /// Adds a new sprite to the SpriteList
        /// </summary>
        /// <param name="key">Name of sprite to add</param>
        /// <param name="sprite">Sprite object to add. Can be a user-created object.</param>
        public void Add(string key, Sprite sprite)
        {
            sprite.game = game;
            sprites.Add(key, sprite);
        }

        // Each of these simply links to the corresponding function in the Sprite class, where they are located to
        // allow a sprite to use the functions on itself, and for possible future use in a possible, but very
        // unlikely implementation.

        /// <summary>
        /// Change whether or not to display the sprite
        /// </summary>
        /// <param name="key">The name of the sprite to edit</param>
        /// <param name="disp">true to display, false to not display.</param>
        public void Display(string key, bool disp)
        {
            sprites[key].Display(disp);
        }

        /// <summary>
        /// Move the sprite x and y pixels
        /// </summary>
        /// <param name="key">The name of the sprite to move</param>
        /// <param name="x">How much to change the x value by. A negative value moves the sprite to the left.</param>
        /// <param name="y">How much to change the y value by. A negative value moves the sprite up.</param>
        public void Move(string key, int x, int y)
        {
            this.sprites[key].Move(x, y);
        }

        /// <summary>
        /// Moves the sprite to a specific point in the game
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="x">New x position of sprite</param>
        /// <param name="y">New y position of sprite</param>
        public void MoveTo(string key, int x, int y)
        {
            this.sprites[key].MoveTo(x, y);
        }

        /// <summary>
        /// Moves the sprite vertically by x amount
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="x">How much to move the sprite by.</param>
        public void MoveX(string key, int x)
        {
            this.sprites[key].MoveX(x);
        }

        /// <summary>
        /// Moves the sprite horizontally by y amount
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="y">How far to move the sprite</param>
        public void MoveY(string key, int y)
        {
            this.sprites[key].MoveY(y);
        }

        /// <summary>
        /// Sets the sprite rotation to r
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="r">How many degrees to rotate the sprite by.</param>
        public void SetRotation(string key, float r)
        {
            this.sprites[key].SetRotation(r);
        }

        /// <summary>
        /// Rotate the sprite by r (degrees or radians?)
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="r">How many degrees to rotate the sprite by.</param>
        public void Rotate(string key, float r)
        {
            this.sprites[key].Rotate(r);
        }

        /// <summary>
        /// Scale the x value of the sprite
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="scale">How much to scale it by(decimal to make it smaller)</param>
        public void ScaleX(string key, double scale)
        {
            sprites[key].ScaleX(scale);
        }

        /// <summary>
        /// Scale the Y value of the sprite
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="scale">How much to scale it by(decimal to make it smaller)</param>
        public void ScaleY(string key, double scale)
        {
            sprites[key].ScaleY(scale);
        }

        /// <summary>
        /// Returns the size of a sprite
        /// </summary>
        /// <param name="key">Sprite to request size of</param>
        /// <returns>Vector of width and height of the sprite</returns>
        public Vector2 GetSize(string key)
        {
            return new Vector2(sprites[key].w, sprites[key].h);
        }

        /// <summary>
        /// Scale entire sprite
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="scale">How much to scale it by(decimal to make it smaller)</param>
        public void Scale(string key, double scale)
        {
            sprites[key].Scale(scale);
        }

        /// <summary>
        /// Sets the overall anchor of a sprite, both x and y to the same value.
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="anchor">Value from 0.0 to 1.0, in terms of percentage to set anchor</param>
        public void SetAnchor(string key, double anchor)
        {
            sprites[key].SetAnchor(anchor);
        }

        /// <summary>
        /// Sets the x anchor of a sprite, mainly for rotation purposes.
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="anchor">Value from 0.0 to 1.0, in terms of percentage to set anchor</param>
        public void SetAnchorX(string key, double anchor)
        {
            sprites[key].SetAnchorX(anchor);
        }

        /// <summary>
        /// Sets the y anchor of a sprite, mainly for rotation purposes.
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="anchor">Value from 0.0 to 1.0, in terms of percentage to set anchor</param>
        public void SetAnchorY(string key, double anchor)
        {
            sprites[key].SetAnchorY(anchor);
        }

        /// <summary>
        /// Kills a sprite. Sprite can no longer be collided with or displayed.
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        public void Kill(string key)
        {
            sprites[key].Kill();
        }

        /// <summary>
        /// Checks if sprite is still alive.
        /// </summary>
        /// <param name="key">Sprite to check</param>
        /// <returns>True if sprite is alive, false if not</returns>
        public bool IsAlive(string key)
        {
            return sprites[key].alive;
        }

        /// <summary>
        /// Opposite of kill() command, allows for sprite to be interacted with
        /// in the world again. Must separately turn display back on
        /// </summary>
        /// <param name="key">Sprite to reincarnate</param>
        public void Reincarnate(string key)
        {
            sprites[key].Reincarnate();
        }

        /// <summary>
        /// Set the vertical velocity of a sprite
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="x">Value of velocity to set</param>
        public void SetVelocityX(string key, int x)
        {
            sprites[key].SetVelocityX(x);
        }
        
        /// <summary>
        /// Set the horizontal velocity of a sprite
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="y">Value of horizontal velocity</param>
        public void SetVelocityY(string key, int y)
        {
            sprites[key].SetVelocityY(y);
        }

        /// <summary>
        /// Set the vertical gravity of a sprite
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="x">Value of gravity to set</param>
        public void SetGravityX(string key, int x)
        {
            sprites[key].SetGravityX(x);
        }

        /// <summary>
        /// Set the horizontal gravity of a sprite
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="y">Value of gravity to set</param>
        public void SetGravityY(string key, int y)
        {
            sprites[key].SetGravityY(y);
        }

        /// <summary>
        /// Sets collider for the sprite
        /// </summary>
        /// <param name="key">Sprite to set collider for</param>
        /// <param name="collider">Collider to use.</param>
        public void SetCollider(string key, Physics.Collider collider)
        {
            sprites[key].SetCollider(collider);
        }

        /// <summary>
        /// Add another sprite to check collision against
        /// </summary>
        /// <param name="key">Sprite to edit</param>
        /// <param name="other">Other sprite to check collision against</param>
        public void AddCollision(string key, string other)
        {
            sprites[key].AddCollision(other);
        }
    }
}
