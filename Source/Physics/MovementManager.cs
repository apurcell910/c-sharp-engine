using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine.Physics {
    class MovementManager {
        public MovementManager() { }

        public void updateSprites(ref Dictionary<string, Sprite> sprites) {
            foreach (KeyValuePair<string, Sprite> obj in sprites) {
                if (obj.Value.alive) {
                    obj.Value.AddVelocityX(obj.Value.gravityX);
                    obj.Value.AddVelocityY(obj.Value.gravityY);
                    obj.Value.Move(obj.Value.velocityX, obj.Value.velocityY);

                    if (obj.Value.collisions != null)
                    {
                        foreach (string collision in obj.Value.collisions)
                        {
                            if (obj.Value.collider.IsTouching(sprites[collision].collider))
                            {
                                if (obj.Value.velocityX > 0)
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
