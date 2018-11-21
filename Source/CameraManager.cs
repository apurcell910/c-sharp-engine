using System.Collections.Generic;

namespace SharpSlugsEngine
{
    /// <summary>
    /// Holds a list of all <see cref="Camera"/> objects being used by the parent <see cref="Game"/>
    /// </summary>
    public class CameraManager
    {
        // Backing field for Main property
        private Camera mainInternal;

        private Game game;
        private List<Camera> cameras = new List<Camera>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraManager"/> class with "<paramref name="game"/>" as the parent <see cref="Game"/>
        /// </summary>
        /// <param name="game">The parent <see cref="Game"/> object for this <see cref="CameraManager"/></param>
        internal CameraManager(Game game)
        {
            this.game = game;
        }

        /// <summary>
        /// Gets or sets the main camera used by the game
        /// </summary>
        public Camera Main
        {
            get => mainInternal;
            set
            {
                if (value != null)
                {
                    mainInternal = value;
                }
            }
        }

        /// <summary>
        /// Gets all cameras currently in use
        /// </summary>
        public Camera[] All => cameras.ToArray();

        /// <summary>
        /// Gets or sets a value indicating whether all cameras should be displayed or only <see cref="Main"/>
        /// </summary>
        public bool DisplayAll { get; set; } = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera"/> class with this <see cref="CameraManager"/>
        /// as the parent.
        /// </summary>
        /// <param name="x">The x coordinate of the new <see cref="Camera"/> in world coordinates</param>
        /// <param name="y">The y coordinate of the new <see cref="Camera"/> in world coordinates</param>
        /// <param name="w">The width of the new <see cref="Camera"/> in world coordinates</param>
        /// <param name="h">The height of the new <see cref="Camera"/> in world coordinates</param>
        /// <param name="drawX">The x coordinate of the new <see cref="Camera"/> in pixel coordinates</param>
        /// <param name="drawY">The y coordinate of the new <see cref="Camera"/> in pixel coordinates</param>
        /// <param name="drawW">The width of the new <see cref="Camera"/> in pixel coordinates</param>
        /// <param name="drawH">The height of the new <see cref="Camera"/> in pixel coordinates</param>
        /// <returns>The newly created <see cref="Camera"/></returns>
        public Camera Create(float x, float y, float w, float h, int drawX, int drawY, int drawW, int drawH)
        {
            Camera cam = new Camera(this, game, x, y, w, h);
            cam.DrawPosition = new Vector2(drawX, drawY);
            cam.DrawSize = new Vector2(drawW, drawH);
            cameras.Add(cam);

            if (Main == null)
            {
                Main = cam;
            }

            return cam;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera"/> class with this <see cref="CameraManager"/>
        /// as the parent.
        /// </summary>
        /// <param name="x">The x coordinate of the new <see cref="Camera"/> in world coordinates</param>
        /// <param name="y">The y coordinate of the new <see cref="Camera"/> in world coordinates</param>
        /// <param name="w">The width of the new <see cref="Camera"/> in world coordinates</param>
        /// <param name="h">The height of the new <see cref="Camera"/> in world coordinates</param>
        /// <returns>The newly created <see cref="Camera"/></returns>
        public Camera Create(float x, float y, float w, float h)
            => Create(x, y, w, h, 0, 0, (int)game.Resolution.X, (int)game.Resolution.Y);

        /// <summary>
        /// Removes a <see cref="Camera"/> from this <see cref="CameraManager"/>
        /// </summary>
        /// <param name="cam">The <see cref="Camera"/> to remove</param>
        public void RemoveCamera(Camera cam)
        {
            if (cam != null && cameras.Contains(cam))
            {
                cam.Dispose();

                if (cam == mainInternal)
                {
                    if (cameras.Count != 0)
                    {
                        mainInternal = cameras[0];
                    }
                    else
                    {
                        mainInternal = null;
                    }
                }
            }
        }

        /// <summary>
        /// Resizes the draw coordinates of all <see cref="Camera"/>s based on a change in resolution
        /// </summary>
        /// <param name="oldRes">The old resolution of the <see cref="Game"/></param>
        /// <param name="newRes">The new resolution of the <see cref="Game"/></param>
        internal void Resize(Vector2 oldRes, Vector2 newRes)
        {
            Vector2 scaler = newRes / oldRes;

            foreach (Camera cam in All)
            {
                cam.DrawPosition *= scaler;
                cam.DrawSize *= scaler;
            }
        }
    }
}
