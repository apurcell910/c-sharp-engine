using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using SharpSlugsEngine.Input;
using SharpSlugsEngine.Sprites;

namespace SharpSlugsEngine
{
    /// <summary>
    /// Provides functionality to create a game. Must be overridden to be used.
    /// </summary>
    public abstract class Game
    {
        // Backing fields
        private bool lockCursorInternal;

        // Fields used to store changes to IUpdatables and IDrawables during Update/Draw cycle
        // because the real lists cannot be safely changed during this time
        private List<IUpdatable> newUpdatables = new List<IUpdatable>();
        private List<IUpdatable> removedUpdatables = new List<IUpdatable>();
        private List<IDrawable> newDrawables = new List<IDrawable>();
        private List<IDrawable> removedDrawables = new List<IDrawable>();

        private Stopwatch globalClock = new Stopwatch();
        private TimeSpan deltaUpdate = new TimeSpan(0);
        private TimeSpan deltaDraw = new TimeSpan(0);
        private TimeSpan targetSpf = new TimeSpan(0);    // targetted seconds per frame

        private List<IUpdatable> updateReceivers;
        private List<IDrawable> drawReceivers;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class
        /// </summary>
        public Game()
        {
            // Set default value for all settings
            Vsync = false;
            TargetFramerate = -1;
            FixedTimestep = false;
            ResolutionInternal = new Vector2(1280, 720);

            // Instantiate lists
            updateReceivers = new List<IUpdatable>();
            drawReceivers = new List<IDrawable>();

            // Create platform
            Platform = new Platform(this);

            // Create graphics manager
            Graphics = new GraphicsManager(this, Platform);

            // Create input managers
            Controllers = new DeviceManager(this);
            Keyboard = new KeyboardManager(this);
            Mouse = new MouseManager(this);

            // Create Content Manager
            Content = new ContentManager(this);

            // Create sprites
            Sprites = new SpriteList(Graphics, this);

            Actions = new SpriteEvents(Sprites);

            // Create camera manager
            Cameras = new CameraManager(this);
        }

        /// <summary>
        /// Gets the <see cref="DeviceManager"/> containing this <see cref="Game"/>'s controllers
        /// </summary>
        public DeviceManager Controllers { get; private set; }

        /// <summary>
        /// Gets the <see cref="KeyboardManager"/> that tracks this <see cref="Game"/>'s keyboard input
        /// </summary>
        public KeyboardManager Keyboard { get; private set; }

        /// <summary>
        /// Gets the <see cref="MouseManager"/> that tracks this <see cref="Game"/>'s mouse input
        /// </summary>
        public MouseManager Mouse { get; private set; }

        /// <summary>
        /// Gets the <see cref="CameraManager"/> containing this <see cref="Game"/>'s <see cref="Camera"/>s
        /// </summary>
        public CameraManager Cameras { get; private set; }

        /// <summary>
        /// Gets the sprites manager for anything that needs to be displayed each call to <see cref="Draw(GameTime)"/>
        /// </summary>
        public SpriteList Sprites { get; private set; }

        /// <summary>
        /// Gets the <see cref="SpriteEvents"/> instance that handles all sprite events for this <see cref="Game"/>
        /// </summary>
        public SpriteEvents Actions { get; private set; }

        /// <summary>
        /// Gets the <see cref="GraphicsManager"/> for this <see cref="Game"/>
        /// </summary>
        public GraphicsManager Graphics { get; private set; }

        /// <summary>
        /// Gets the <see cref="ContentManager"/> for this <see cref="Game"/>
        /// </summary>
        public ContentManager Content { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the game should lock to the monitor's refresh rate.
        /// Overrides <see cref="TargetFramerate"/> if applicable.
        /// </summary>
        public bool Vsync { get; protected set; }

        /// <summary>
        /// Gets or sets the framerate the game will attempt to reach (But not exceed). Set to -1 for no target.
        /// Overridden by <see cref="Vsync"/> if applicable.
        /// </summary>
        public int TargetFramerate { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether the GameTime passed to Update and Draw will always assume the target framerate is being met.
        /// <see cref="TargetFramerate"/> must be set for this to take effect.
        /// </summary>
        public bool FixedTimestep { get; protected set; }

        /// <summary>
        /// Gets or sets the resolution of the game window. <see cref="Vector2.X" /> controls width and <see cref="Vector2.Y"/> controls height.
        /// </summary>
        public Vector2 Resolution
        {
            get => ResolutionInternal;
            set
            {
                Cameras.Resize(ResolutionInternal, value);

                ResolutionInternal = value;

                // Update resolution of game window
                Platform.ResizeWindow((int)ResolutionInternal.X, (int)ResolutionInternal.Y);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not to show the cursor when hovering over the game window.
        /// </summary>
        public bool ShowCursor
        {
            get => Cursor.Current != null;
            set
            {
                if (value)
                {
                    Cursor.Show();
                }
                else
                {
                    Cursor.Hide();
                }
            }
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether or not to lock the mouse cursor to the game window
        /// </summary>
        public bool LockCursor
        {
            get => lockCursorInternal;
            set
            {
                if (value)
                {
                    Platform.Form.LockCursor();
                }
                else
                {
                    Platform.Form.UnlockCursor();
                }

                lockCursorInternal = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Game"/>'s resolution. Bypasses all logic related to this.
        /// In most cases, use of <see cref="Resolution"/> is preferred.
        /// </summary>
        internal Vector2 ResolutionInternal { get; set; }
        
        /// <summary>
        /// Gets the <see cref="SharpSlugsEngine.Platform"/> associated with this <see cref="Game"/>
        /// </summary>
        internal Platform Platform { get; private set; }

        /// <summary>
        /// Sets up the Game class and begins the main loop of <see cref="Update"/> and <see cref="Draw"/>
        /// </summary>
        public void Run()
        {
            // Call setup functions
            Initialize();
            Cameras.Main = Cameras.Create(0, 0, Graphics.WorldScale.X, Graphics.WorldScale.Y);
            LoadContent();
            
            CheckValues();
            
            // Begin running the game loop
            globalClock.Start();
            Platform.BeginRun();
        }
        
        /// <summary>
        /// Register component "<paramref name="updatable"/>" to receive updates before the main <see cref="Update"/> call
        /// </summary>
        /// <param name="updatable">The <see cref="IUpdatable"/> to begin calling <see cref="IUpdatable.Update(GameTime)"/> on</param>
        public void AddUpdatable(IUpdatable updatable)
        {
            if (!newUpdatables.Contains(updatable))
            {
                newUpdatables.Add(updatable);
            }
        }

        /// <summary>
        /// Remove component "<paramref name="updatable"/>" from update list
        /// </summary>
        /// <param name="updatable">The <see cref="IUpdatable"/> to stop calling <see cref="IUpdatable.Update(GameTime)"/> on</param>
        public void RemoveUpdatable(IUpdatable updatable)
        {
            if (!removedUpdatables.Contains(updatable))
            {
                removedUpdatables.Add(updatable);
            }
        }
        
        /// <summary>
        /// Register component "<paramref name="drawable"/>" to receive updates before the main <see cref="Draw"/> call
        /// </summary>
        /// <param name="drawable">The <see cref="IDrawable"/> to begin calling <see cref="IDrawable.Draw(GameTime)"/> on</param>
        public void AddDrawable(IDrawable drawable)
        {
            if (!newDrawables.Contains(drawable))
            {
                newDrawables.Add(drawable);
            }
        }

        /// <summary>
        /// Remove component "<paramref name="drawable"/>" from draw list
        /// </summary>
        /// <param name="drawable">The <see cref="IDrawable"/> to stop calling <see cref="IDrawable.Draw(GameTime)"/> on</param>
        public void RemoveDrawable(IDrawable drawable)
        {
            if (!removedDrawables.Contains(drawable))
            {
                removedDrawables.Add(drawable);
            }
        }

        /// <summary>
        /// Handles the updating and drawing for a single frame. Called from <see cref="Platform.PlatformIdle()"/>
        /// </summary>
        internal void ProcessFrame()
        {
            GameTime updateTime = new GameTime();
            GameTime drawTime = new GameTime();

            updateTime.deltaTime = globalClock.Elapsed - deltaUpdate;
            deltaUpdate = globalClock.Elapsed;
            updateTime.totalTime = globalClock.Elapsed;

            Controllers.Update(updateTime);
            Keyboard.Update();
            Mouse.Update();

            // Handle IUpdatables
            updateReceivers.AddRange(newUpdatables);
            newUpdatables.Clear();

            updateReceivers.RemoveAll(item => removedUpdatables.Contains(item));
            removedUpdatables.Clear();

            updateReceivers.RemoveAll(updatable => !updatable.Alive);
            updateReceivers.ForEach(updatable => updatable.Update(updateTime));

            Update(updateTime);
            Actions.Update();
            Sprites.Update(updateTime);

            drawTime.deltaTime = globalClock.Elapsed - deltaDraw;
            if (targetSpf != TimeSpan.Zero)
            {
                // TODO: FIX THIS TIMOHTY
                // if the targetUpdate is within 1/4 of a second of time passed
                if (targetSpf.TotalMilliseconds >= drawTime.deltaTime.TotalMilliseconds)
                {
                    deltaDraw = globalClock.Elapsed;
                    drawTime.totalTime = globalClock.Elapsed;

                    Graphics.Begin();
                    Sprites.Draw();

                    // Handle IDrawables
                    drawReceivers.AddRange(newDrawables);
                    newDrawables.Clear();

                    drawReceivers.RemoveAll(item => removedDrawables.Contains(item));
                    removedDrawables.Clear();

                    drawReceivers.RemoveAll(drawable => !drawable.Alive);
                    drawReceivers.ForEach(drawable => drawable.Draw(drawTime));

                    Draw(drawTime);
                    Graphics.End();
                }

                // else skip graphics step.
            }
            else
            {
                deltaDraw = globalClock.Elapsed;
                drawTime.totalTime = globalClock.Elapsed;

                Graphics.Begin();
                Sprites.Draw();

                // Handle IDrawables
                drawReceivers.AddRange(newDrawables);
                newDrawables.Clear();

                drawReceivers.RemoveAll(item => removedDrawables.Contains(item));
                removedDrawables.Clear();

                drawReceivers.RemoveAll(drawable => !drawable.Alive);
                drawReceivers.ForEach(drawable => drawable.Draw(drawTime));

                Draw(drawTime);
                Graphics.End();
            }
        }

        /// <summary>
        /// Calculates the targeted seconds per frame
        /// </summary>
        internal void CheckValues() // Use initialized values to fill out rest of needed values.
        {
            if (Vsync == false)
            {
                if (TargetFramerate == -1)
                {
                    targetSpf = TimeSpan.Zero;
                }
                else
                {
                    targetSpf = new TimeSpan(TimeSpan.TicksPerSecond / TargetFramerate);
                }
            }
            else
            {
                targetSpf = TimeSpan.Zero; // todo: Implement VSync
            }
        }

        /// <summary>
        /// Optionally override in order to change settings or setup other game variables.
        /// Called directly before <see cref="LoadContent"/>.
        /// </summary>
        protected virtual void Initialize()
        {
        }

        /// <summary>
        /// Optionally override in order to load from the content manager.
        /// Called after <see cref="Initialize"/> and before beginning the main loop.
        /// </summary>
        protected virtual void LoadContent()
        {
        }

        /// <summary>
        /// The main update loop of the Game class. Called before <see cref="Draw"/> on every frame.
        /// </summary>
        /// <param name="gameTime">The elapsed game time since the last call to <see cref="Update"/>.</param>
        protected abstract void Update(GameTime gameTime);

        /// <summary>
        /// The main draw loop of the Game class. Called after <see cref="Update"/> on every frame.
        /// Some draw calls may be skipped if required to reach <see cref="TargetFramerate"/>.
        /// </summary>
        /// <param name="gameTime">The elapsed game time since the last call to <see cref="Draw"/></param>
        protected abstract void Draw(GameTime gameTime);
    }
}
