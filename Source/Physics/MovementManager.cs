using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine.Physics
{
    /// <summary>
    /// Manages movement of sprites in SpriteList according to their velocity and gravity values.
    /// </summary>
    public class MovementManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovementManager"/> class.
        /// </summary>
        public MovementManager()
        {
        }

        /// <summary>
        /// Updates all of the sprites according to velocity and gravity.
        /// </summary>
        /// <param name="sprites">Reference to dictionary of the various sprites.</param>
        /// <param name="gameTime">The current game time.</param>
        public void UpdateSprites(ref Dictionary<string, Sprite> sprites, GameTime gameTime)
        {
            foreach (KeyValuePair<string, Sprite> obj in sprites)
            {
                if (obj.Value.alive)
                {
                    obj.Value.AddVelocityX(obj.Value.gravityX);
                    obj.Value.AddVelocityY(obj.Value.gravityY);
                    obj.Value.Move(obj.Value.velocityX * gameTime.DeltaTime.TotalSeconds, obj.Value.velocityY * gameTime.DeltaTime.TotalSeconds);

                    if (obj.Value.collisions != null)
                    {
                        foreach (string collision in obj.Value.collisions)
                        {
                            if (obj.Value.collider.IsTouching(sprites[collision].collider))
                            {
                                if (obj.Value.velocityX < 0)
                                {
                                    while (obj.Value.collider.IsTouching(sprites[collision].collider))
                                    {
                                        obj.Value.MoveX(-1);
                                    }

                                    obj.Value.SetVelocityX(0);
                                }
                                else
                                {
                                    while (obj.Value.collider.IsTouching(sprites[collision].collider))
                                    {
                                        obj.Value.MoveX(1);
                                    }

                                    obj.Value.SetVelocityX(0);
                                }

                                if (obj.Value.velocityY > 0)
                                {
                                    while (obj.Value.collider.IsTouching(sprites[collision].collider))
                                    {
                                        obj.Value.MoveY(-1);
                                    }

                                    obj.Value.SetVelocityY(0);
                                }
                                else
                                {
                                    while (obj.Value.collider.IsTouching(sprites[collision].collider))
                                    {
                                        obj.Value.MoveY(1);
                                    }

                                    obj.Value.SetVelocityY(0);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
