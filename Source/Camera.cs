using System;
using System.Drawing;

namespace SharpSlugsEngine
{
    public class Camera : IDisposable
    {
        private Vector2 _position;
        public Vector2 Position
        {
            get => _position;
            set
            {
                if (!float.IsNaN(value.X) && !float.IsNaN(value.Y) && !float.IsInfinity(value.X) && !float.IsInfinity(value.Y))
                {
                    _position = value;
                }
            }
        }

        private Vector2 _size;
        public Vector2 Size
        {
            get => _size;
            set
            {
                if (!float.IsNaN(value.X) && !float.IsNaN(value.Y) && !float.IsInfinity(value.X) && !float.IsInfinity(value.Y))
                {
                    _size = value;
                }
            }
        }

        public float X
        {
            get => _position.X;
            set => Position = new Vector2(value, _position.Y);
        }

        public float Y
        {
            get => _position.Y;
            set => Position = new Vector2(_position.X, value);
        }

        public float Width
        {
            get => _size.X;
            set => Size = new Vector2(value, _size.Y);
        }

        public float Height
        {
            get => _size.Y;
            set => Size = new Vector2(_size.X, value);
        }

        private Vector2 _drawPosition;
        public Vector2 DrawPosition
        {
            get => _drawPosition;
            set
            {
                if (!float.IsNaN(value.X) && !float.IsNaN(value.Y) && !float.IsInfinity(value.X) && !float.IsInfinity(value.Y))
                {
                    _drawPosition = value;
                }
            }
        }

        private Vector2 _drawSize;
        public Vector2 DrawSize
        {
            get => _drawSize;
            set
            {
                if (!float.IsNaN(value.X) && !float.IsNaN(value.Y) && !float.IsInfinity(value.X) && !float.IsInfinity(value.Y))
                {
                    _drawSize = value;
                }
            }
        }

        public int DrawX
        {
            get => (int)_drawPosition.X;
            set => DrawPosition = new Vector2(value, _position.Y);
        }

        public int DrawY
        {
            get => (int)_drawPosition.Y;
            set => DrawPosition = new Vector2(_position.X, value);
        }

        public int DrawWidth
        {
            get => (int)_drawSize.X;
            set => DrawSize = new Vector2(value, _size.Y);
        }

        public int DrawHeight
        {
            get => (int)_drawSize.Y;
            set => DrawSize = new Vector2(_size.X, value);
        }

        public Vector2 CameraToWorld(Vector2 cameraCoord, bool ignorePos = false)
        {
            cameraCoord /= (Size / _game.Graphics.WorldScale);
            cameraCoord *= Size;
            cameraCoord /= DrawSize;
            cameraCoord /= (_game.Graphics.WorldScale / Size);
            if (!ignorePos) cameraCoord += Position;

            return cameraCoord;
        }

        public Vector2 WorldToCameraPixels(Vector2 worldCoord, bool ignorePos = false)
        {
            if (!ignorePos) worldCoord -= Position;
            worldCoord *= (_game.Graphics.WorldScale / Size);

            worldCoord = (worldCoord / Size) * DrawSize;

            return worldCoord * (Size / _game.Graphics.WorldScale);
        }

        private Game _game;
        private CameraManager _manager;

        internal Bitmap buffer;
        internal Graphics bitmapGraphics;

        internal Camera(CameraManager cameras, Game game, float x, float y, float w, float h)
        {
            _manager = cameras;
            _game = game;
            Position = new Vector2(x, y);
            Size = new Vector2(w, h);
        }

        internal void Begin()
        {
            buffer = new Bitmap(DrawWidth, DrawHeight);
            bitmapGraphics = Graphics.FromImage(buffer);
        }

        internal void End()
        {
            buffer.Dispose();
            bitmapGraphics.Dispose();
        }

        public override string ToString()
        {
            return "{Position: " + Position + ", Size: " + Size + ", DrawPosition: " + DrawPosition + ", DrawSize: " + DrawSize + "}";
        }

        public void Dispose()
        {
            ((IDisposable)this).Dispose();
        }

        void IDisposable.Dispose()
        {
            bitmapGraphics?.Dispose();
            buffer?.Dispose();
            _manager.RemoveCamera(this);
        }
    }
}
