using System;
using System.Windows.Media;

namespace SharpSlugsEngine.Sound
{
    public class Sound : IUpdatable, IDisposable
    {
        private MediaPlayer player;
        private SoundCache cache;

        /// <summary>
        /// Whether or not to loop the Sound after completion
        /// </summary>
        public bool Loop { get; set; }

        /// <summary>
        /// Whether or not to dispose this Sound after finishing playing (Default false)
        /// </summary>
        public bool DisposeOnFinished { get; set; } = false;

        /// <summary>
        /// Volume of the Sound object, on a scale from 0-100
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
        /// Whether or not the Sound is muted
        /// </summary>
        public bool Muted
        {
            get => player.IsMuted;
            set => player.IsMuted = value;
        }

        /// <summary>
        /// The position of the current Sound, on a scale of 0-<see cref="Duration"/>
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
        /// The speed this Sound plays at, default 1
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
        /// The state of the Sound
        /// </summary>
        public SoundState LoadState { get; private set; } = SoundState.Loading;

        private float aliveTime;
        private bool _alive = true;
        bool IUpdatable.Alive => _alive;

        internal Sound(SoundCache parent)
        {
            cache = parent;

            player = new MediaPlayer();
            player.MediaOpened += MediaOpened;
            player.MediaFailed += MediaFailed;
            player.MediaEnded += MediaEnded;
            player.Open(new Uri(cache.Path));
        }

        private void MediaOpened(object sender, EventArgs e)
        {
            _alive = false;

            LoadState = SoundState.Loaded;
            _soundLoaded?.Invoke(this);
        }

        private void MediaFailed(object sender, EventArgs e)
        {
            _alive = false;

            LoadState = SoundState.LoadFailed;
            _soundLoaded?.Invoke(this);
        }

        private void MediaEnded(object sender, EventArgs e)
        {
            _playbackFinished?.Invoke(this);

            if (DisposeOnFinished)
            {
                Dispose();
            }
            else if (Loop)
            {
                Play();
            }
        }

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
        /// Play the Sound from <paramref name="position"/>
        /// </summary>
        public void Play(float position = 0f)
        {
            player.Stop();
            Position = position;
            player.Play();
        }

        internal void ResetEvents()
        {
            _soundLoaded = null;
            _playbackFinished = null;
        }

        public void Dispose()
        {
            ((IDisposable)this).Dispose();
        }

        void IDisposable.Dispose()
        {
            cache.Reclaim(this);
        }

        void IUpdatable.Update(GameTime gameTime)
        {
            aliveTime += (float)gameTime.deltaTime.TotalSeconds;

            if (aliveTime >= 2)
            {
                _alive = false;

                if (LoadState == SoundState.Loading)
                {
                    player = null;
                    LoadState = SoundState.LoadFailed;
                    _soundLoaded?.Invoke(this);
                }
            }
        }

        public delegate void SoundEvent(Sound sound);

        private event SoundEvent _soundLoaded;
        /// <summary>
        /// Called when the Sound either finishes loading or fails to load. Check <see cref="LoadState"/> to see which.
        /// </summary>
        public event SoundEvent SoundLoaded
        {
            add => _soundLoaded += value;
            remove => _soundLoaded -= value;
        }

        private event SoundEvent _playbackFinished;
        /// <summary>
        /// Called when the Sound completes playing
        /// </summary>
        public event SoundEvent PlaybackFinished
        {
            add => _playbackFinished += value;
            remove => _playbackFinished -= value;
        }
    }

    public enum SoundState
    {
        Loading,
        Loaded,
        LoadFailed
    }
}
