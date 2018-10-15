using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SharpSlugsEngine
{
    internal class WindowsPlatform : Platform
    {
        //Constructor needs to exist, even if we're just calling base
        public WindowsPlatform(Game game) : base(game)
        {
            
        }

        public override void BeginRun()
        {
            //Hook the app idle and begin the game
            Application.Idle += PlatformIdle;
            Application.Run(form);

            //Unhook after the game closes
            //Not strictly necessary, but good practice
            Application.Idle -= PlatformIdle;
        }

        private void PlatformIdle(object sender, EventArgs e)
        {
            while (true)
            {
                //If there's a system message to process, stop looping to allow it
                //We should enter back into this loop fine afterwards via the Application.Idle hook
                if (PeekMessage(out Message msg))
                {
                    break;
                }

                game.ProcessFrame();
            }
        }

        public override void ResizeWindow(int x, int y)
        {
            form.Size = new Size(x, y);
            form.Center();
        }
        #region Native Calls
        /// <summary>
        /// Retrieves a message from the queue associated with the current thread.
        /// Non-blocking, does not wait for a message.
        /// </summary>
        /// <param name="message">Empty struct to place the captured message in.</param>
        /// <returns>Bool indicating if a message was found in the queue.</returns>
        private static bool PeekMessage(out Message message) => PeekMessage(out message, IntPtr.Zero, 0, 0, 0);
        
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PeekMessage(out Message lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);

        [StructLayout(LayoutKind.Sequential)]
        private struct Message
        {
            public IntPtr handle;
            public uint msg;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public Point p;
        }
        #endregion
    }
}
