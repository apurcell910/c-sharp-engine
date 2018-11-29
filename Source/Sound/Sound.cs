using System;
using System.Windows.Media;

namespace SharpSlugsEngine.Sound
{
    /// <summary>
    /// The state in the loading process of a <see cref="Sound"/> object
    /// </summary>
    internal enum SoundState
    {
        Loading,
        Loaded,
        LoadFailed
    }

    /// <summary>
    /// Provides methods for playing audio
    /// </summary>
    public class Sound : IUpdatable, IDisposable
    {
        private MediaPlayer player;
        private SoundCache cache;

        // Fields used for IUpdatable
        private float aliveTime;
        private bool aliveInternal = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sound"/> class with "<paramref name="parent"/>" as the parent <see cref="SoundCache"/>
        /// </summary>
        /// <param name="parent">The parent <see cref="SoundCache"/> object</param>
        internal Sound(SoundCache parent)
        {
            cache = parent;

            player = new MediaPlayer();
            player.MediaOpened += MediaOpened;
            player.MediaFailed += MediaFailed;
            player.MediaEnded += MediaEnded;
            player.Open(new Uri(cache.Path));
        }

        /// <summary>
        /// Used for all events relating to the <see cref="Sound"/> class
        /// </summary>
        /// <param name="sound">The <see cref="Sound"/> object calling the event</param>
        public delegate void SoundEvent(Sound sound);

        /// <summary>
        /// Called when the Sound completes playing
        /// </summary>
        public event SoundEvent PlaybackFinished
        {
            add => PlaybackFinishedInternal += value;
            remove => PlaybackFinishedInternal -= value;
        }

        /// <summary>
        /// Called when the Sound either finishes loading or fails to load. Check <see cref="LoadState"/> to see which.
        /// </summary>
        internal event SoundEvent SoundLoaded
        {
            add => SoundLoadedInternal += value;
            remove => SoundLoadedInternal -= value;
        }

        /// <summary>
        /// Private backing field for the <see cref="SoundLoaded"/> event
        /// </summary>
        private event SoundEvent SoundLoadedInternal;

        /// <summary>
        /// Private backing field for the <see cref="PlaybackFinished"/> event
        /// </summary>
        private event SoundEvent PlaybackFinishedInternal;

        /// <summary>
        /// Gets or sets a value indicating whether or not to loop the Sound after completion
        /// </summary>
        public bool Loop { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to dispose this Sound after finishing playing
        /// </summary>
        public bool DisposeOnFinished { get; set; }

        /// <summary>
        /// Gets or sets the volume of the Sound object, on a scale from 0-100
        /// </summary>
        public int Volume
        {
            get => (int)(player.Volume * 100);
            set
            {
                if (value >= 0 && value <= 100)
                {
                    player.Volume = value / 100f;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the Sound is muted
        /// </summary>
        public bool Muted
        {
            get => player.IsMuted;
            set => player.IsMuted = value;
        }

        /// <summary>
        /// Gets or sets the position of the current Sound, on a scale of 0-<see cref="Duration"/>
        /// </summary>
        public float Position
        {
            get => (float)player.Position.TotalSeconds;
            set
            {
                if (value >= 0 && value <= Duration)
                {
                    player.Position = TimeSpan.FromSeconds(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the speed this Sound plays at, default 1
        /// </summary>
        public float Speed
        {
            get => (float)player.SpeedRatio;
            set
            {
                if (value > 0f && !float.IsPositiveInfinity(value))
                {
                    player.SpeedRatio = value;
                }
            }
        }

        /// <summary>
        /// The length of the Sound in seconds
        /// </summary>
        public float Duration => (float)player.NaturalDuration.TimeSpan.TotalSeconds;
        
        /// <summary>
        /// Gets a value indicating whether the <see cref="Game"/> should continue updating this object
        /// </summary>
        bool IUpdatable.Alive => aliveInternal;

        /// <summary>
        /// Gets the state of the Sound
        /// </summary>
        internal SoundState LoadState { get; private set; } = SoundState.Loading;

        /// <summary>
        /// Pause the Sound
        /// </summary>
        public void Pause()
        {
            player.Pause();
        }

        /// <summary>
        /// Stop the Sound
        /// </summary>
        public void Stop()
        {
            player.Stop();
        }

        /// <summary>
        /// Play the Sound
        /// </summary>
        public void Play()
        {
            player.Play();
        }

        /// <summary>
        /// Causes the parent <see cref="SoundCache"/> to reclaim this <see cref="Sound"/>
        /// </summary>
        public void Dispose()
        {
            cache.Reclaim(this);
        }

        /// <summary>
        /// Causes the parent <see cref="SoundCache"/> to reclaim this <see cref="Sound"/>
        /// </summary>
        void IDisposable.Dispose()
        {
            Dispose();
        }

        /// <summary>
        /// Causes the <see cref="Sound"/> loading to time out after 2 seconds have passed
        /// </summary>
        /// <param name="gameTime">Delta time information used to calculate when 2 seconds have passed</param>
        void IUpdatable.Update(GameTime gameTime)
        {
            aliveTime += (float)gameTime.deltaTime.TotalSeconds;

            if (aliveTime >= 2)
            {
                aliveInternal = false;

                if (LoadState == SoundState.Loading)
                {
                    player = null;
                    LoadState = SoundState.LoadFailed;
                    SoundLoadedInternal?.Invoke(this);
                }
            }
        }

        /// <summary>
        /// Empties all events on this <see cref="Sound"/>
        /// </summary>
        internal void ResetEvents()
        {
            SoundLoadedInternal = null;
            PlaybackFinishedInternal = null;
        }

        /// <summary>
        /// Called by the child <see cref="player"/> object, passes the event upward
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void MediaOpened(object sender, EventArgs e)
        {
            aliveInternal = false;

            LoadState = SoundState.Loaded;
            SoundLoadedInternal?.Invoke(this);
        }

        /// <summary>
        /// Called by the child <see cref="player"/> object, passes the event upward
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void MediaFailed(object sender, EventArgs e)
        {
            aliveInternal = false;

            LoadState = SoundState.LoadFailed;
            SoundLoadedInternal?.Invoke(this);
        }

        /// <summary>
        /// Called by the child <see cref="player"/> object, passes the event upward.
        /// Additionally, loops or disposes the object based on <see cref="Loop"/> and <see cref="DisposeOnFinished"/>
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void MediaEnded(object sender, EventArgs e)
        {
            PlaybackFinishedInternal?.Invoke(this);

            if (DisposeOnFinished)
            {
                Dispose();
            }
            else if (Loop)
            {
                Play();
            }
        }
    }
}
