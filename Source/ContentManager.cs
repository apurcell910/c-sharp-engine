using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
namespace SharpSlugsEngine
{
    public class ContentManager
    {
        private List<string> paths;
        private Dictionary<string, Bitmap> images;

        public ContentManager()
        {
            this.paths = new List<string>();
            this.images = new Dictionary<string, Bitmap>();
        }

        public void AddImage(string filePath, string fileName = "image", int scale = 1)
        {
            Bitmap bmp = new Bitmap(filePath);
            bmp = new Bitmap(bmp, (int)bmp.VerticalResolution / scale, (int)bmp.HorizontalResolution / scale);
            images.Add(fileName, bmp);
            return;
        }

        private void AddImage(string fileName, Bitmap bmp)
        {
            images.Add(fileName, bmp);
            return;
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
    }
}
