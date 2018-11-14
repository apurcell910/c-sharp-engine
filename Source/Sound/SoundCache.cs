using System.Collections.Generic;

namespace SharpSlugsEngine.Sound
{
    internal class SoundCache
    {
        public string Path { get; private set; }
        public int CacheSize { get; private set; }

        private List<Sound> cache;
        private List<Sound> unavailable;

        public SoundCache(string path, int cacheSize)
        {
            Path = path;
            CacheSize = cacheSize;
            cache = new List<Sound>();
            unavailable = new List<Sound>();

            for (int i = 0; i < CacheSize; i++)
            {
                CreateSound();
            }
        }

        public Sound GetSound()
        {
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
            Sound sound = new Sound(this);
            sound.SoundLoaded += SoundLoaded;
        }

        internal void SoundLoaded(Sound sound)
        {
            sound.SoundLoaded -= SoundLoaded;

            if (sound.LoadState == SoundState.Loaded)
            {
                cache.Add(sound);
            }
            else
            {
                sound.ResetEvents();
                CreateSound();
            }
        }
    }
}
