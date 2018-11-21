using System.Collections.Generic;

namespace SharpSlugsEngine.Sound
{
    /// <summary>
    /// Stores <see cref="CacheSize"/> <see cref="Sound"/> objects, loaded from <see cref="Path"/>
    /// </summary>
    internal class SoundCache
    {
        private static List<SoundCache> loadWaiters = new List<SoundCache>();

        private Game game;
        private List<Sound> cache;
        private List<Sound> unavailable;
        private int waiting;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundCache"/> class that attempts to load "<paramref name="cacheSize"/>"
        /// <see cref="Sound"/>s from "<paramref name="path"/>"
        /// </summary>
        /// <param name="game">The parent <see cref="Game"/> object</param>
        /// <param name="path">The path to load sounds from</param>
        /// <param name="cacheSize">The amount of sounds to load</param>
        public SoundCache(Game game, string path, int cacheSize)
        {
            this.game = game;

            Path = path;
            CacheSize = cacheSize;
            cache = new List<Sound>();
            unavailable = new List<Sound>();

            if (!Loading)
            {
                Loading = true;
                CreateSound();
            }
            else
            {
                loadWaiters.Add(this);
            }
        }

        /// <summary>
        /// Gets a value indicating whether any <see cref="Sound"/> objects are currently being loaded
        /// </summary>
        public static bool Loading { get; private set; }

        /// <summary>
        /// Gets the path the <see cref="Sound"/> objects are loaded from
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Gets the amount of <see cref="Sound"/> objects this <see cref="SoundCache"/> attempts to load
        /// </summary>
        public int CacheSize { get; private set; }

        /// <summary>
        /// Attempts to retrieve a <see cref="Sound"/> from the cache
        /// </summary>
        /// <returns>A <see cref="Sound"/> object if successful, otherwise null</returns>
        public Sound GetSound()
        {
            if (Loading)
            {
                return null;
            }

            if (cache.Count > 0)
            {
                Sound snd = cache[0];
                cache.RemoveAt(0);
                unavailable.Add(snd);

                return snd;
            }

            return null;
        }

        /// <summary>
        /// Resets a <see cref="Sound"/> object and returns it to the cache
        /// </summary>
        /// <param name="sound">The <see cref="Sound"/> object to reclaim</param>
        internal void Reclaim(Sound sound)
        {
            if (sound.LoadState == SoundState.Loaded && unavailable.Contains(sound))
            {
                sound.Stop();
                sound.Position = 0;
                sound.Muted = false;
                sound.Volume = 100;
                sound.Speed = 1;
                sound.ResetEvents();

                unavailable.Remove(sound);
                cache.Add(sound);
            }
        }

        /// <summary>
        /// Handles <see cref="Sound"/>s after they load or fail to load
        /// </summary>
        /// <param name="sound">The <see cref="Sound"/> object that has finished attempting to load</param>
        internal void SoundLoaded(Sound sound)
        {
            // Dehook the sound and subtract from the waiting count
            sound.SoundLoaded -= SoundLoaded;
            waiting--;

            // If it loaded properly, add it to the cache. Otherwise, unhook make a new one
            if (sound.LoadState == SoundState.Loaded)
            {
                cache.Add(sound);
            }
            else
            {
                sound.ResetEvents();
                CreateSound();
            }

            // Begin loading a new set of sounds or allow another cache to load
            if (waiting <= 0)
            {
                int totalNeeded = CacheSize - (cache.Count + unavailable.Count);

                if (totalNeeded > 0)
                {
                    int max = totalNeeded > 5 ? 5 : totalNeeded;
                    for (int i = 0; i < max; i++)
                    {
                        CreateSound();
                    }
                }
                else
                {
                    if (loadWaiters.Count != 0)
                    {
                        loadWaiters[0].CreateSound();
                        loadWaiters.RemoveAt(0);
                    }
                    else
                    {
                        Loading = false;
                    }
                }
            }
        }

        /// <summary>
        /// Creates a new <see cref="Sound"/> object and begins waiting for it to load
        /// </summary>
        private void CreateSound()
        {
            waiting++;

            Sound sound = new Sound(this);
            sound.SoundLoaded += SoundLoaded;

            game.AddUpdatable(sound);
        }
    }
}
