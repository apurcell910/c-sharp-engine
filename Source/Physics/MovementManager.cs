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
                    obj.Value.addVelocityX(obj.Value.gravityX);
                    obj.Value.addVelocityY(obj.Value.gravityY);
                    obj.Value.move(obj.Value.velocityX, obj.Value.velocityY);

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
                                        obj.Value.moveX(-1);
                                    }
                                    obj.Value.setVelocityX(0);
                                }
                                else
                                {
                                    while (obj.Value.collider.IsTouching(sprites[collision].collider))
                                    {
                                        obj.Value.moveX(1);
                                    }
                                    obj.Value.setVelocityX(0);
                                }
                                if (obj.Value.velocityY > 0)
                                {
                                    while (obj.Value.collider.IsTouching(sprites[collision].collider))
                                    {
                                        obj.Value.moveY(-1);
                                    }
                                    obj.Value.setVelocityY(0);
                                }
                                else
                                {
                                    while (obj.Value.collider.IsTouching(sprites[collision].collider))
                                    {
                                        obj.Value.moveY(1);
                                    }
                                    obj.Value.setVelocityY(0);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
