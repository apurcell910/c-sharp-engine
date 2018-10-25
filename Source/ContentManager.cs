using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
namespace SharpSlugsEngine
{
    public class ContentManager
    {
        internal List<string> paths;
        internal List<Bitmap> images;
        public ContentManager()
        {
            this.paths = new List<string>();
            this.images = new List<Bitmap>();
        }

        public Bitmap AddImage(string filePath, int factor = 0)
        {
            Bitmap bmp = new Bitmap(filePath);
            bmp = new Bitmap(bmp, (int)bmp.VerticalResolution / factor, (int)bmp.HorizontalResolution / factor);
            images.Add(bmp);
            return bmp;
        }

        public bool inManager(string name)
        {
            return paths.Contains(name);
        }


        //TODO: LOOK AT THIS https://docs.microsoft.com/en-us/dotnet/api/system.drawing.bitmap.clone?redirectedfrom=MSDN&view=netframework-4.7.2#System_Drawing_Bitmap_Clone_System_Drawing_Rectangle_System_Drawing_Imaging_PixelFormat_
        public Bitmap[] SplitImage(string filePath, int numCuts)
        {
            Bitmap orig = new Bitmap(filePath);
            Bitmap[] bmp = new Bitmap[numCuts];
            for(int i = 0; i < numCuts; i++)
            {
                orig = new Bitmap(orig, (int)orig.VerticalResolution / numCuts, (int)orig.HorizontalResolution / numCuts);
                bmp[i] = orig;
            }
            return bmp;
        }
    }
}
