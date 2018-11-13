using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine
{
    public class CameraManager
    {
        private Game _game;
        private List<Camera> _cameras = new List<Camera>();

        public Camera[] All => _cameras.ToArray();

        internal CameraManager(Game game)
        {
            _game = game;
        }

        public Camera Create(float x, float y, float w, float h)
        {
            Camera cam = new Camera(this, _game, x, y, w, h);
            _cameras.Add(cam);

            return cam;
        }

        public void RemoveCamera(Camera cam)
        {
            if (cam != null && _cameras.Contains(cam))
            {
                cam.Dispose();
            }
        }
    }
}
