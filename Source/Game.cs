﻿using System;
using System.Diagnostics;
using System.Windows.Forms;
namespace SharpSlugsEngine
{
    public abstract class Game
    {
        private Stopwatch globalClock = new Stopwatch();
        private TimeSpan deltaUpdate = new TimeSpan(0);
        private TimeSpan deltaDraw = new TimeSpan(0);
        /// <summary>
        /// Controls whether or not the game should lock to the monitor's refresh rate.
        /// Overrides <see cref="TargetFramerate"/> if applicable.
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

                platform.ResizeWindow(_resolution.X, _resolution.Y);
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
            platform = Platform.Create(this);
        }

        /// <summary>
        /// Sets up the Game class and begins the main loop of <see cref="Update"/> and <see cref="Draw"/>
        /// </summary>
        public void Run()
        {
            //Call setup functions
            Initialize();
            LoadContent();

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
            updateTime.totalTime = globalClock.Elapsed;
            Update(updateTime);
            deltaUpdate = globalClock.Elapsed;
            //TODO: Sprint 1, user story 1, task 7 (Timothy)
            drawTime.deltaTime = globalClock.Elapsed - deltaDraw;
            drawTime.totalTime = globalClock.Elapsed;
            Draw(drawTime);
            deltaDraw = globalClock.Elapsed;
            
            Console.WriteLine("Time elapsed since last update: {0}", updateTime.deltaTime);
            Console.WriteLine("Time elapsed since last draw: {0}", drawTime.deltaTime);
            Console.WriteLine("Time elapsed since beginning: {0}", globalClock.Elapsed);
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
