using System;
using System.Drawing;
using System.Windows.Forms;

namespace SharpSlugsEngine
{
    /// <summary>
    /// Handles low level actions relating to the <see cref="System.Windows.Forms.Form"/> class for the parent <see cref="Game"/>
    /// </summary>
    internal class Platform
    {
        internal readonly Reform Form;
        protected readonly Game Game;

        private IAsyncResult result;
        private MethodInvoker invoker;

        /// <summary>
        /// Initializes a new instance of the <see cref="Platform"/> class with "<paramref name="game"/>" as the parent <see cref="SharpSlugsEngine.Game"/>
        /// </summary>
        /// <param name="game">The parent of the new <see cref="Platform"/> instance</param>
        public Platform(Game game)
        {
            Game = game;

            // Placeholder settings for now
            Form = new Reform(game)
            {
                Text = "Game Title Placeholder",
                Size = new Size(1280, 720),
                FormBorderStyle = FormBorderStyle.FixedSingle,
                BackColor = Color.HotPink
            };

            invoker = new MethodInvoker(PlatformIdle);

            Form.Hook();
        }

        /// <summary>
        /// Begin running the main loop for the parent <see cref="SharpSlugsEngine.Game"/>
        /// </summary>
        public void BeginRun()
        {
            // Hook the app idle and begin the game
            Application.Idle += PlatformIdle;
            Application.Run(Form);

            // Unhook after the game closes
            // Not strictly necessary, but good practice
            Application.Idle -= PlatformIdle;
        }

        /// <summary>
        /// Resize <see cref="Form"/> to an inner window size of "<paramref name="x"/>" width and "<paramref name="y"/>" height
        /// </summary>
        /// <param name="x">The new inner width of <see cref="Form"/></param>
        /// <param name="y">The new inner height of <see cref="Form"/></param>
        public void ResizeWindow(int x, int y)
        {
            Form.ClientSize = new Size(x, y);
            Form.Center();
            Game.Graphics.RecreateGraphics();
        }

        /// <summary>
        /// Overload of <see cref="PlatformIdle()"/> to be used by the <see cref="Application.Idle"/> event
        /// </summary>
        /// <param name="sender">The sender of the event (ignored)</param>
        /// <param name="e">The arguments of the event (ignored)</param>
        private void PlatformIdle(object sender, EventArgs e) => PlatformIdle();

        /// <summary>
        /// Calls <see cref="Game.ProcessFrame"/> repeatedly during application idle
        /// </summary>
        private void PlatformIdle()
        {
            if (result != null)
            {
                Form.EndInvoke(result);
                result = null;
            }
            else
            {
                result = Form.BeginInvoke(invoker);
            }

            Game.ProcessFrame();
        }
    }
}
