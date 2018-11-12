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
                    obj.Value.velocityX += obj.Value.gravityX;
                    obj.Value.velocityY += obj.Value.gravityY;
                    obj.Value.move(obj.Value.velocityX, obj.Value.velocityY);
                }
            }
        }
    }
}
