using System;
using System.Drawing;
using System.Windows.Forms;

namespace SharpSlugsEngine
{
    internal abstract class Platform
    {
        protected readonly Game game;
        internal readonly Reform form;

        protected Platform(Game game)
        {
            this.game = game;

            //Placeholder settings for now
            form = new Reform
            {
                Text = "Game Title Placeholder",
                Size = new Size(1280, 720),
                FormBorderStyle = FormBorderStyle.FixedSingle,
                BackColor = Color.HotPink
            };
        }
        
        /// <summary>
        /// Creates an OS specific Platform object.
        /// </summary>
        /// <param name="game">The Game object that runs this Platform.</param>
        /// <returns>The new Platform object.</returns>
        public static Platform Create(Game game)
        {
            //Handle platforms here so the Game class doesn't need to know what system it's running on
            if (Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                //TODO: Implement OSXPlatform class
                throw new InvalidOperatingSystemException();
            }
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                //TODO: Implement UnixPlatform class
                throw new InvalidOperatingSystemException();
            }
            else if (Environment.OSVersion.Platform == PlatformID.Xbox)
            {
                //Not gonna worry about Xbox, nothing to do here
                throw new InvalidOperatingSystemException();
            }
            else
            {
                return new WindowsPlatform(game);
            }
        }

        public abstract void ResizeWindow(int x, int y);

        public abstract void BeginRun();
    }

    public class InvalidOperatingSystemException : Exception
    {
        public InvalidOperatingSystemException() : base($"Unsupported operating system: {Environment.OSVersion.Platform}") { }
    }
}
