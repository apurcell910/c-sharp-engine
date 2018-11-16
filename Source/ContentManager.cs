using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using SharpSlugsEngine.Sound;

namespace SharpSlugsEngine
{
    public class ContentManager
    {
        private Game _game;

        private List<string> paths;
        private Dictionary<string, Bitmap> images;

        private Dictionary<string, SoundCache> sounds;
        private Dictionary<string, FontCache> fonts;

        internal ContentManager(Game game)
        {
            _game = game;

            paths = new List<string>();
            images = new Dictionary<string, Bitmap>();
            sounds = new Dictionary<string, SoundCache>();
            fonts = new Dictionary<string, FontCache>();
        }

        public void AddImage(string filePath, string fileName = "image", int scale = 1)
        {
            Bitmap bmp = new Bitmap(filePath);
            bmp = new Bitmap(bmp, bmp.Width / scale, bmp.Height / scale);
            images.Add(fileName, bmp);
            return;
        }

        private void AddImage(string fileName, Bitmap bmp)
        {
            images.Add(fileName, bmp);
            return;
        }

        public void AddFont(string filePath, string fileName)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(fileName))
            {
                return;
            }

            if (fonts.ContainsKey(fileName))
            {
                return;
            }

            if (!File.Exists(filePath) || Path.GetExtension(filePath).ToLower() != ".ttf")
            {
                return;
            }

            try
            {
                using (System.Drawing.Text.PrivateFontCollection collection = new System.Drawing.Text.PrivateFontCollection())
                {
                    collection.AddFontFile(filePath);
                    fonts.Add(fileName, new FontCache(collection.Families[0]));
                }
            }
            catch { }
        }

        public Font GetFont(string name, int emSize = 32)
        {
            if (fonts.TryGetValue(name, out FontCache f))
            {
                return f.Get(emSize);
            }

            return null;
        }

        public void AddSound(string filePath, string fileName, int cacheSize = 5)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(fileName))
            {
                return;
            }

            if (sounds.ContainsKey(fileName))
            {
                return;
            }

            try
            {
                string path = Path.GetFullPath(filePath);

                if (!File.Exists(path))
                {
                    return;
                }

                string ext = Path.GetExtension(path).ToLower();

                if (!(ext == ".aif" || ext == ".aifc" || ext == ".aiff" || ext == ".au" || ext == ".mid"
                    || ext == ".mp3" || ext == ".snd" || ext == ".wav" || ext == ".wma"))
                {
                    return;
                }

                sounds.Add(fileName, new SoundCache(_game, path, cacheSize));
            }
            catch (Exception) { }
        }

        public Sound.Sound GetSound(string name)
        {
            if (sounds.TryGetValue(name, out SoundCache cache))
            {
                return cache.GetSound();
            }

            return null;
        }

        public Bitmap ScaleImage(Bitmap bmp, int scale)
        {
            Bitmap ret = new Bitmap(bmp, (int)bmp.Width / scale, ((int)bmp.Height / scale));
            return ret;
        }

        public Bitmap[] ScaleImage(Bitmap[] bmp, int scale)
        {
            Bitmap[] ret = new Bitmap[bmp.Length];
            for(int i = 0; i < bmp.Length; i++)
            {
                ret[i] = new Bitmap(bmp[i], (bmp[i].Width / scale), (bmp[i].Height / scale));
            }
            return ret;
        }

        public Bitmap GetImage(string name)
        {
            images.TryGetValue(name, out Bitmap value);
            return value;
        }

        public bool InManager(string name)
        {
            return images.TryGetValue(name, out Bitmap value);
        }

        public void printNames()
        {
            foreach(string key in images.Keys)
            {
                Console.WriteLine(key);
            }
        }

        public Bitmap[] SplitImage(string filePath, int numCuts, string fileNames = "file")
        {
            Bitmap orig = new Bitmap(filePath);
            Bitmap[] bmp = new Bitmap[numCuts];

            Rectangle cloneRect = new Rectangle(0, 0, orig.Width / (numCuts/2), orig.Height / (numCuts/2));
            System.Drawing.Imaging.PixelFormat format = orig.PixelFormat;
            int tracker = 0;
            float cutFactor = numCuts/2;
            for(int i = 0; i < cutFactor; i++)
            {
                for(int j = 0; j < cutFactor; j++)
                {
                    cloneRect = new Rectangle((int)(orig.Width * (i / cutFactor)), (int)(orig.Height * (j / cutFactor)), (int)(orig.Width / cutFactor), (int)(orig.Height / cutFactor));
                    bmp[tracker] = orig.Clone(cloneRect, format);
                    tracker++;
                    AddImage(string.Concat(fileNames, tracker.ToString()), bmp[i]);
                }
            }
            return bmp;
        }

        public Bitmap[] SplitImage(Bitmap bmp, int numCuts, string fileNames = "file")
        {
            Bitmap[] copy = new Bitmap[numCuts];

            Rectangle cloneRect = new Rectangle(0, 0, bmp.Width / (numCuts / 2), bmp.Height / (numCuts / 2));
            System.Drawing.Imaging.PixelFormat format = bmp.PixelFormat;
            int tracker = 0;
            float cutFactor = numCuts / 2;
            for (int i = 0; i < cutFactor; i++)
            {
                for (int j = 0; j < cutFactor; j++)
                {
                    cloneRect = new Rectangle((int)(bmp.Width * (i / cutFactor)), (int)(bmp.Height * (j / cutFactor)), (int)(bmp.Width / cutFactor), (int)(bmp.Height / cutFactor));
                    copy[tracker++] = bmp.Clone(cloneRect, format);
                    AddImage(string.Concat(fileNames, tracker.ToString()), copy[i]);
                }
            }
            return copy;
        }

        private struct FontCache
        {
            public FontFamily family;
            public Dictionary<int, Font> fonts;

            public FontCache(FontFamily fam)
            {
                family = fam;
                fonts = new Dictionary<int, Font>();
            }

            public Font Get(int i)
            {
                if (fonts.TryGetValue(i, out Font f))
                {
                    return f;
                }

                f = new Font(family, i);
                fonts.Add(i, f);
                return f;
            }
        }
    }
}
