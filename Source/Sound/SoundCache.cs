using System.Collections.Generic;

namespace SharpSlugsEngine.Sound
{
    internal class SoundCache
    {
        private Game _game;

        public static bool Loading { get; private set;  }
        private static List<SoundCache> loadWaiters = new List<SoundCache>();

        public string Path { get; private set; }
        public int CacheSize { get; private set; }

        private List<Sound> cache;
        private List<Sound> unavailable;

        private int waiting;

        public SoundCache(Game game, string path, int cacheSize)
        {
            _game = game;

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

        internal void Reclaim(Sound sound)
        {
            if (sound.LoadState == SoundState.Loaded)
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

        private void CreateSound()
        {
            waiting++;

            Sound sound = new Sound(this);
            sound.SoundLoaded += SoundLoaded;

            _game.AddUpdatable(sound);
        }

        internal void SoundLoaded(Sound sound)
        {
            sound.SoundLoaded -= SoundLoaded;
            waiting--;

            if (sound.LoadState == SoundState.Loaded)
            {
                cache.Add(sound);
            }
            else
            {
                sound.ResetEvents();
                CreateSound();
            }

            if (waiting <= 0)
            {
                int totalNeeded = CacheSize - (cache.Count + unavailable.Count);

                if (totalNeeded > 0)
                {
                    for (int i = 0; i < (totalNeeded > 5 ? 5 : totalNeeded); i++)
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
    }
}
