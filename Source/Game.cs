using System;
using System.Diagnostics;

namespace SharpSlugsEngine
{
    public abstract class Game
    {
        private Stopwatch globalClock = new Stopwatch();
        private TimeSpan deltaUpdate = new TimeSpan(0);
        private TimeSpan deltaDraw = new TimeSpan(0);
        private TimeSpan targetSpf = new TimeSpan(0);    //targetted seconds per frame

        /// <summary>
        /// Sprites manager for anything that needs to be displayed each goaround;
        /// </summary>
        public Sprite sprites { get; private set; }

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
        private Vector2 _resolution;

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

        //This shouldn't need to be accessed from outside of the library
        internal Platform platform;

        public Game()
        {
            //Set default value for all settings
            Vsync = false;
            TargetFramerate = -1;
            FixedTimestep = false;
            _resolution = new Vector2(1280, 720);

            //Create platform
            platform = new Platform(this);

            //Create graphics manager
            Graphics = new GraphicsManager(this, platform);

            Content = new ContentManager();

            //Create sprites
            sprites = new Sprite(Graphics, Content);
        }

        /// <summary>
        /// Sets up the Game class and begins the main loop of <see cref="Update"/> and <see cref="Draw"/>
        /// </summary>
        public void Run()
        {
            //Call setup functions
            Initialize();
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
            Update(updateTime);

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
                    sprites.spriteDraw();
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
                sprites.spriteDraw();
                Draw(drawTime);
                Graphics.End();
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
