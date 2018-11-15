using System;
using System.Collections.Generic;

namespace SharpSlugsEngine
{
    public class CameraManager
    {
        private Game _game;
        private List<Camera> _cameras = new List<Camera>();

        private Camera _main;
        public Camera Main
        {
            get => _main;
            set
            {
                if (value != null)
                {
                    _main = value;
                }
            }
        }

        public Camera[] All => _cameras.ToArray();

        public bool DisplayAll { get; set; } = true;

        internal CameraManager(Game game)
        {
            _game = game;
        }

        public Camera Create(float x, float y, float w, float h)
        {
            Camera cam = new Camera(this, _game, x, y, w, h);
            cam.DrawPosition = Vector2.Zero;
            cam.DrawSize = _game.Resolution;
            _cameras.Add(cam);

            return cam;
        }

        internal void Resize(Vector2 oldRes, Vector2 newRes)
        {
            Vector2 scaler = newRes / oldRes;

            foreach (Camera cam in All)
            {
                cam.DrawPosition *= scaler;
                cam.DrawSize *= scaler;
            }
        }

        public void RemoveCamera(Camera cam)
        {
            if (cam != null && _cameras.Contains(cam))
            {
                cam.Dispose();

                if (cam == _main)
                {
                    if (_cameras.Count != 0)
                    {
                        _main = _cameras[0];
                    }
                    else
                    {
                        _main = null;
                    }
                }
            }
        }
    }
}
