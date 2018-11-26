using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using SharpSlugsEngine.Sound;

namespace SharpSlugsEngine
{
    /// <summary>
    /// A class that handles external files for use within the game and has helper functions to use those assets
    /// </summary>
    public class ContentManager
    {
        private Game game;

        private List<string> paths;
        private Dictionary<string, Bitmap> images;

        private Dictionary<string, SoundCache> sounds;
        private Dictionary<string, FontCache> fonts;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentManager"/> class with given game
        /// </summary>
        /// <param name="game">The current game for which the Content Manager belongs to</param>
        internal ContentManager(Game game)
        {
            this.game = game;

            paths = new List<string>();
            images = new Dictionary<string, Bitmap>();
            sounds = new Dictionary<string, SoundCache>();
            fonts = new Dictionary<string, FontCache>();
        }

        /// <summary>
        /// Adds an external image to the Content Manager
        /// </summary>
        /// <param name="filePath">Path to the image which is to be added to the Content Manager</param>
        /// <param name="fileName">An optional name for the new image</param>
        /// <param name="scale">A scaling factor for the image to change its size</param>
        public void AddImage(string filePath, string fileName = "image", int scale = 1)
        {
            Bitmap bmp = new Bitmap(filePath);
            bmp = new Bitmap(bmp, bmp.Width / scale, bmp.Height / scale);
            images.Add(fileName, bmp);
            return;
        }

        /// <summary>
        /// Add a font to the Content Manager from an external path
        /// </summary>
        /// <param name="filePath">File path which leads to a font file</param>
        /// <param name="fileName">Name of the file that should be opened</param>
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
            catch
            {
            }
        }

        /// <summary>
        /// Pull a font from the Content Manager
        /// </summary>
        /// <param name="name">Name of the font</param>
        /// <param name="fontSize">Size of the font</param>
        /// <returns>Return a font</returns>
        public Font GetFont(string name, int fontSize = 32)
        {
            if (fonts.TryGetValue(name, out FontCache f))
            {
                return f.Get(fontSize);
            }

            return null;
        }

        /// <summary>
        /// Add an external sound source to the Content Manager
        /// </summary>
        /// <param name="filePath">File path which should lead to a sound file</param>
        /// <param name="fileName">Name of the file to be added</param>
        /// <param name="cacheSize">Size of the sound cache</param>
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

                sounds.Add(fileName, new SoundCache(game, path, cacheSize));
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Get a cached sound from the Content Manager
        /// </summary>
        /// <param name="name">Name of the sound to be pulled from the Content Manager</param>
        /// <returns>Return the sound pulled from Content Manager</returns>
        public Sound.Sound GetSound(string name)
        {
            if (sounds.TryGetValue(name, out SoundCache cache))
            {
                return cache.GetSound();
            }

            return null;
        }

        /// <summary>
        /// Scale an image with a given scale
        /// </summary>
        /// <param name="bmp">Bitmap to be scaled</param>
        /// <param name="scale">Integer scale to be used to scale image</param>
        /// <returns>Return the scaled image</returns>
        public Bitmap ScaleImage(Bitmap bmp, int scale)
        {
            Bitmap ret = new Bitmap(bmp, (int)bmp.Width / scale, (int)bmp.Height / scale);
            return ret;
        }

        /// <summary>
        /// Scale an array of images all with a given scale
        /// </summary>
        /// <param name="bmp">Bitmap array to be scaled</param>
        /// <param name="scale">Integer scale to be used for scaling</param>
        /// <returns>The new scaled Bitmap array</returns>
        public Bitmap[] ScaleImage(Bitmap[] bmp, int scale)
        {
            Bitmap[] ret = new Bitmap[bmp.Length];
            for (int i = 0; i < bmp.Length; i++)
            {
                ret[i] = new Bitmap(bmp[i], bmp[i].Width / scale, bmp[i].Height / scale);
            }

            return ret;
        }

        /// <summary>
        /// Pull an image from the Content Manager
        /// </summary>
        /// <param name="name">Name of the image in the Content Manager</param>
        /// <returns>Bitmap image from Content Manager</returns>
        public Bitmap GetImage(string name)
        {
            images.TryGetValue(name, out Bitmap value);
            return value;
        }

        /// <summary>
        /// Check if a certain image is within the Content Manager
        /// </summary>
        /// <param name="name">Checks to see if a given image is in the Content Manager</param>
        /// <returns>True if image is found, false otherwise</returns>
        public bool InManager(string name)
        {
            return images.TryGetValue(name, out Bitmap value);
        }

        /// <summary>
        /// Print the names of all images within the Content Manager
        /// </summary>
        public void PrintNames()
        {
            foreach (string key in images.Keys)
            {
                Console.WriteLine(key);
            }
        }

        /// <summary>
        /// Split an external image into an array of images and store them in the Content Manager
        /// </summary>
        /// <param name="filePath">File path to the image to be split</param>
        /// <param name="numCuts">Number of divisions the image should be cut to</param>
        /// <param name="fileNames">Optional names for the new set of images</param>
        /// <returns>An array of Bitmap images</returns>
        public Bitmap[] SplitImage(string filePath, int numCuts, string fileNames = "file")
        {
            Bitmap orig = new Bitmap(filePath);
            Bitmap[] bmp = new Bitmap[numCuts];

            Rectangle cloneRect = new Rectangle(0, 0, orig.Width / (numCuts / 2), orig.Height / (numCuts / 2));
            System.Drawing.Imaging.PixelFormat format = orig.PixelFormat;
            int tracker = 0;
            float cutFactor = numCuts / 2;
            for (int i = 0; i < cutFactor; i++)
            {
                for (int j = 0; j < cutFactor; j++)
                {
                    cloneRect = new Rectangle((int)(orig.Width * (i / cutFactor)), (int)(orig.Height * (j / cutFactor)), (int)(orig.Width / cutFactor), (int)(orig.Height / cutFactor));
                    bmp[tracker] = orig.Clone(cloneRect, format);
                    tracker++;
                    AddImage(string.Concat(fileNames, tracker.ToString()), bmp[i]);
                }
            }

            return bmp;
        }

        /// <summary>
        /// Split an already existing image into multiple and store them within the Content Manager
        /// </summary>
        /// <param name="bmp">Bitmap image to be split</param>
        /// <param name="numCuts">Number of divisions the image should be cut to</param>
        /// <param name="fileNames">Optional names for the new Bitmap array</param>
        /// <returns>Bitmap array of split images</returns>
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

        /// <summary>
        /// Add an already existing Bitmap to the Content Manager
        /// </summary>
        /// <param name="fileName">Name of the image to be added to the Content Manager</param>
        /// <param name="bmp">Already existing bitmap to be added to Content Manager</param>
        private void AddImage(string fileName, Bitmap bmp)
        {
            images.Add(fileName, bmp);
            return;
        }

        /// <summary>
        /// Cache for Fonts
        /// </summary>
        private struct FontCache
        {
            public FontFamily Family;
            public Dictionary<int, Font> Fonts;

            /// <summary>
            /// Initializes a new instance of the <see cref="FontCache"/> struct
            /// </summary>
            /// <param name="fam">Family of font</param>
            public FontCache(FontFamily fam)
            {
                Family = fam;
                Fonts = new Dictionary<int, Font>();
            }

            /// <summary>
            /// Pull a font from the Content Manager
            /// </summary>
            /// <param name="i">Get the font from the Content Manager</param>
            /// <returns>The font from the given index</returns>
            public Font Get(int i)
            {
                if (Fonts.TryGetValue(i, out Font f))
                {
                    return f;
                }

                f = new Font(Family, i);
                Fonts.Add(i, f);
                return f;
            }
        }
    }
}
