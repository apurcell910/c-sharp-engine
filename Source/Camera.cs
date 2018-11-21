using System;
using System.Drawing;

namespace SharpSlugsEngine
{
    /// <summary>
    /// Defines a region in world space to draw on a corresponding section of screen space.
    /// Relevant objects are automatically drawn by the GraphicsManager if cameras are created.
    /// </summary>
    public class Camera : IDisposable
    {
        // Backing fields for properties
        private Vector2 positionInternal;
        private Vector2 sizeInternal;
        private Vector2 drawPositionInternal;
        private Vector2 drawSizeInternal;
        
        // References to the Game and its CameraManager
        private Game game;
        private CameraManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera"/> class and registers it with the provided CameraManager "<paramref name="cameras"/>"
        /// </summary>
        /// <param name="cameras">The CameraManager to place the Camera inside</param>
        /// <param name="game">The Game object that "<paramref name="cameras"/>" belongs to</param>
        /// <param name="x">The x coordinate of the new camera in world coordinates</param>
        /// <param name="y">The y coordinate of the new camera in world coordinates</param>
        /// <param name="w">The width of the new camera in world coordinates</param>
        /// <param name="h">The height of the new camera in world coordinates</param>
        internal Camera(CameraManager cameras, Game game, float x, float y, float w, float h)
        {
            manager = cameras;
            this.game = game;
            Position = new Vector2(x, y);
            Size = new Vector2(w, h);
        }

        /// <summary>
        /// Gets or sets the position of the camera in world space
        /// </summary>
        public Vector2 Position
        {
            get => positionInternal;
            set
            {
                if (!float.IsNaN(value.X) && !float.IsNaN(value.Y) && !float.IsInfinity(value.X) && !float.IsInfinity(value.Y))
                {
                    positionInternal = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the size of the camera in world space
        /// </summary>
        public Vector2 Size
        {
            get => sizeInternal;
            set
            {
                if (!float.IsNaN(value.X) && !float.IsNaN(value.Y) && !float.IsInfinity(value.X) && !float.IsInfinity(value.Y))
                {
                    sizeInternal = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the x coordinate of the camera in world space
        /// </summary>
        public float X
        {
            get => positionInternal.X;
            set => Position = new Vector2(value, positionInternal.Y);
        }

        /// <summary>
        /// Gets or sets the y coordinate of the camera in world space
        /// </summary>
        public float Y
        {
            get => positionInternal.Y;
            set => Position = new Vector2(positionInternal.X, value);
        }

        /// <summary>
        /// Gets or sets the width of the camera in world space
        /// </summary>
        public float Width
        {
            get => sizeInternal.X;
            set => Size = new Vector2(value, sizeInternal.Y);
        }

        /// <summary>
        /// Gets or sets the height of the camera in world space
        /// </summary>
        public float Height
        {
            get => sizeInternal.Y;
            set => Size = new Vector2(sizeInternal.X, value);
        }

        /// <summary>
        /// Gets or sets the draw position of this camera in pixels
        /// </summary>
        public Vector2 DrawPosition
        {
            get => drawPositionInternal;
            set
            {
                if (!float.IsNaN(value.X) && !float.IsNaN(value.Y) && !float.IsInfinity(value.X) && !float.IsInfinity(value.Y))
                {
                    drawPositionInternal = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the draw size of this camera in pixels
        /// </summary>
        public Vector2 DrawSize
        {
            get => drawSizeInternal;
            set
            {
                if (!float.IsNaN(value.X) && !float.IsNaN(value.Y) && !float.IsInfinity(value.X) && !float.IsInfinity(value.Y))
                {
                    drawSizeInternal = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the pixel x coordinate this camera is drawn at
        /// </summary>
        public int DrawX
        {
            get => (int)drawPositionInternal.X;
            set => DrawPosition = new Vector2(value, positionInternal.Y);
        }

        /// <summary>
        /// Gets or sets the pixel y coordinate this camera is drawn at
        /// </summary>
        public int DrawY
        {
            get => (int)drawPositionInternal.Y;
            set => DrawPosition = new Vector2(positionInternal.X, value);
        }

        /// <summary>
        /// Gets or sets the pixel width of this camera
        /// </summary>
        public int DrawWidth
        {
            get => (int)drawSizeInternal.X;
            set => DrawSize = new Vector2(value, sizeInternal.Y);
        }

        /// <summary>
        /// Gets or sets the pixel height of this camera
        /// </summary>
        public int DrawHeight
        {
            get => (int)drawSizeInternal.Y;
            set => DrawSize = new Vector2(sizeInternal.X, value);
        }

        /// <summary>
        /// Gets the buffer for world objects to be drawn to
        /// </summary>
        internal Bitmap Buffer { get; private set; }

        /// <summary>
        /// Gets the Graphics object corresponding to <see cref="Buffer"/>
        /// </summary>
        internal Graphics BitmapGraphics { get; private set; }

        /// <summary>
        /// Converts a pixel position on this camera into world coordinates.
        /// </summary>
        /// <param name="cameraCoord">The pixel coordinates to convert.</param>
        /// <param name="ignorePos">Set true to ignore the camera position (Useful for converting sizes).</param>
        /// <returns>The converted world coordinates.</returns>
        public Vector2 CameraToWorld(Vector2 cameraCoord, bool ignorePos = false)
        {
            cameraCoord /= Size / game.Graphics.WorldScale;
            cameraCoord *= Size;
            cameraCoord /= DrawSize;
            cameraCoord /= game.Graphics.WorldScale / Size;
            if (!ignorePos)
            {
                cameraCoord += Position;
            }

            return cameraCoord;
        }

        /// <summary>
        /// Converts a world coordinate position into a pixel position on this camera.
        /// </summary>
        /// <param name="worldCoord">The world coordinates to convert.</param>
        /// <param name="ignorePos">Set true to ignore the camera position (Useful for converting sizes).</param>
        /// <returns>The converted pixel coordinates.</returns>
        public Vector2 WorldToCamera(Vector2 worldCoord, bool ignorePos = false)
        {
            if (!ignorePos)
            {
                worldCoord -= Position;
            }

            worldCoord *= game.Graphics.WorldScale / Size;

            worldCoord = (worldCoord / Size) * DrawSize;

            return worldCoord * (Size / game.Graphics.WorldScale);
        }

        /// <summary>
        /// Converts this camera to a string
        /// </summary>
        /// <returns>A string containing the camera position and size in world and screen coordinates</returns>
        public override string ToString()
        {
            return "{Position: " + Position + ", Size: " + Size + ", DrawPosition: " + DrawPosition + ", DrawSize: " + DrawSize + "}";
        }

        /// <summary>
        /// Disposes this <see cref="Camera"/> object and removes it from its CameraManager
        /// </summary>
        public void Dispose()
        {
            BitmapGraphics?.Dispose();
            Buffer?.Dispose();
            manager?.RemoveCamera(this);
        }

        /// <summary>
        /// Disposes this <see cref="Camera"/> object and removes it from its CameraManager
        /// </summary>
        void IDisposable.Dispose()
        {
            Dispose();
        }

        /// <summary>
        /// Prepares the Camera to be drawn on
        /// </summary>
        internal void Begin()
        {
            Buffer = new Bitmap(DrawWidth, DrawHeight);
            BitmapGraphics = Graphics.FromImage(Buffer);
        }

        /// <summary>
        /// Disposes draw objects at the end of a frame
        /// </summary>
        internal void End()
        {
            Buffer.Dispose();
            BitmapGraphics.Dispose();
        }
    }
}
