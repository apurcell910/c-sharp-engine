using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using SharpSlugsEngine.Input;

namespace SharpSlugsEngine
{
    public abstract class Game
    {
        private Stopwatch globalClock = new Stopwatch();
        private TimeSpan deltaUpdate = new TimeSpan(0);
        private TimeSpan deltaDraw = new TimeSpan(0);
        private TimeSpan targetSpf = new TimeSpan(0);    //targetted seconds per frame

        private List<IUpdatable> updateReceivers;
        private List<IDrawable> drawReceivers;

        public DeviceManager Controllers { get; private set; }

        public KeyboardManager Keyboard { get; private set; }

        public MouseManager Mouse { get; private set; }

        public CameraManager Cameras { get; private set; }

        /// <summary>
        /// Sprites manager for anything that needs to be displayed each goaround;
        /// </summary>
        public SpriteList sprites { get; private set; }

        /// <summary>
        /// Graphics Manager for the Game object.
        /// </summary>
        public GraphicsManager Graphics { get; private set; }

        public ContentManager Content { get; private set; }

        /// <summary>
        /// Controls whether or not the game should lock to the monitor's refresh rate.
        /// Overrides <see cref="TargetFramerate"/> if applicable.
        /// </summary>
        public bool Vsync { get; protected set; } //TODO: Sprint 1, user story 1, task 6 (Timothy)

        /// <summary>
        /// The framerate the game will attempt to reach (But not exceed). Set to -1 for no target.
        /// Overriden by <see cref="Vsync"/> if applicable.
        /// </summary>
        public int TargetFramerate { get; protected set; } //TODO: Sprint 1, user story 1, task 6 (Timothy)

        /// <summary>
        /// If enabled, the GameTime passed to Update and Draw will always assume the target framerate is being met.
        /// <see cref="TargetFramerate"/> must be set for this to take effect.
        /// </summary>
        public bool FixedTimestep { get; protected set; }

        //Backing field for the Resolution property
        internal Vector2 _resolution;

        /// <summary>
        /// The resolution of the game window. <see cref="Vector2.X" /> controls width and <see cref="Vector2.Y"/> controls height.
        /// </summary>
        public Vector2 Resolution
        {
            get => _resolution;
            set
            {
                _resolution = value;

                platform.ResizeWindow((int)_resolution.X, (int)_resolution.Y);
                //Update resolution of game window
                //TODO: Sprint 1, user story 1, task 5 (Harpreet)
            }
        }

        /// <summary>
        /// Whether or not to show the cursor when hovering over the game window.
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

        //This shouldn't need to be accessed from outside of the library
        internal Platform platform;

        public Game()
        {
            //Set default value for all settings
            Vsync = false;
            TargetFramerate = -1;
            FixedTimestep = false;
            _resolution = new Vector2(1280, 720);

            //Instantiate lists
            updateReceivers = new List<IUpdatable>();
            drawReceivers = new List<IDrawable>();

            //Create platform
            platform = new Platform(this);

            //Create graphics manager
            Graphics = new GraphicsManager(this, platform);
            
            //Create input managers
            Controllers = new DeviceManager(this);
            Keyboard = new KeyboardManager(this);
            Mouse = new MouseManager(this);
            
            //Create Content Manager
            Content = new ContentManager(this);

            //Create sprites
            sprites = new SpriteList(Graphics);

            //Create camera manager
            Cameras = new CameraManager(this);
        }

        /// <summary>
        /// Sets up the Game class and begins the main loop of <see cref="Update"/> and <see cref="Draw"/>
        /// </summary>
        public void Run()
        {
            //Call setup functions
            Initialize();
            Cameras.Main = Cameras.Create(0, 0, Graphics.WorldScale.X, Graphics.WorldScale.Y);
            LoadContent();
            
            CheckValues();
            
            //Begin running the game loop
            globalClock.Start();
            platform.BeginRun();
        }

        internal void ProcessFrame()
        {
            //TODO: Sprint 1, user story 1, task 4 (Harpreet)
            GameTime updateTime = new GameTime();
            GameTime drawTime = new GameTime();

            updateTime.deltaTime = globalClock.Elapsed - deltaUpdate;
            deltaUpdate = globalClock.Elapsed;
            updateTime.totalTime = globalClock.Elapsed;

            Controllers.Update(updateTime);
            Keyboard.Update();
            Mouse.Update();

            //Handle IUpdatables
            updateReceivers.AddRange(newUpdatables);
            newUpdatables.Clear();

            updateReceivers.RemoveAll(item => removedUpdatables.Contains(item));
            removedUpdatables.Clear();

            updateReceivers.RemoveAll(updatable => !updatable.Alive);
            updateReceivers.ForEach(updatable => updatable.Update(updateTime));

            Update(updateTime);
            sprites.Update();//Should I add updateTime to this? Not sure how it is worked in.

            //TODO: Sprint 1, user story 1, task 7 (Timothy)

            
            drawTime.deltaTime = globalClock.Elapsed - deltaDraw;
            if (targetSpf != TimeSpan.Zero)
            {
                //TODO: FIX THIS TIMOHTY
                if (targetSpf.TotalMilliseconds >= drawTime.deltaTime.TotalMilliseconds) //if the targetUpdate is within 1/4 of a second of time passed
                {
                    deltaDraw = globalClock.Elapsed;
                    drawTime.totalTime = globalClock.Elapsed;

                    Graphics.Begin();
                    sprites.Draw();

                    //Handle IDrawables
                    drawReceivers.AddRange(newDrawables);
                    newDrawables.Clear();

                    drawReceivers.RemoveAll(item => removedDrawables.Contains(item));
                    removedDrawables.Clear();

                    drawReceivers.RemoveAll(drawable => !drawable.Alive);
                    drawReceivers.ForEach(drawable => drawable.Draw(drawTime));

                    Draw(drawTime);
                    Graphics.End();
                }
                //else skip graphics step.
            }
            else
            {
                deltaDraw = globalClock.Elapsed;
                drawTime.totalTime = globalClock.Elapsed;

                Graphics.Begin();
                sprites.Draw();

                //Handle IDrawables
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

        private List<IUpdatable> newUpdatables = new List<IUpdatable>();
        /// <summary>
        /// Register component "<paramref name="updatable"/>" to receive updates before the main <see cref="Update"/> call
        /// </summary>
        public void AddUpdatable(IUpdatable updatable)
        {
            if (!newUpdatables.Contains(updatable))
            {
                newUpdatables.Add(updatable);
            }
        }

        private List<IUpdatable> removedUpdatables = new List<IUpdatable>();
        /// <summary>
        /// Remove component "<paramref name="updatable"/>" from update list
        /// </summary>
        public void RemoveUpdatable(IUpdatable updatable)
        {
            if (!removedUpdatables.Contains(updatable))
            {
                removedUpdatables.Add(updatable);
            }
        }

        private List<IDrawable> newDrawables = new List<IDrawable>();
        /// <summary>
        /// Register component "<paramref name="drawable"/>" to receive updates before the main <see cref="Draw"/> call
        /// </summary>
        public void AddDrawable(IDrawable drawable)
        {
            if (!newDrawables.Contains(drawable))
            {
                newDrawables.Add(drawable);
            }
        }

        private List<IDrawable> removedDrawables = new List<IDrawable>();
        /// <summary>
        /// Remove component "<paramref name="drawable"/>" from draw list
        /// </summary>
        public void RemoveDrawable(IDrawable drawable)
        {
            if (!removedDrawables.Contains(drawable))
            {
                removedDrawables.Add(drawable);
            }
        }

        internal void CheckValues() //Use initialized values to fill out rest of needed values.
        {
            if (Vsync == false)
            {
                if (TargetFramerate == -1)
                {
                    targetSpf = TimeSpan.Zero;
                }
                else
                {
                    targetSpf = new TimeSpan((long)((TimeSpan.TicksPerSecond) / TargetFramerate));
                }
            }
            else
            {
                targetSpf = TimeSpan.Zero;//todo: Implement VSync
            }
            
        }

        /// <summary>
        /// Optionally override in order to change settings or setup other game variables.
        /// Called directly before <see cref="LoadContent"/>.
        /// </summary>
        protected virtual void Initialize() { }

        /// <summary>
        /// Optionally override in order to load from the content manager.
        /// Called after <see cref="Initialize"/> and before beginning the main loop.
        /// </summary>
        protected virtual void LoadContent() { }

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
