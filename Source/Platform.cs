using System;
using System.Drawing;
using System.Windows.Forms;

namespace SharpSlugsEngine
{
    internal class Platform
    {
        private IAsyncResult result;
        private MethodInvoker invoker;

        protected readonly Game game;
        internal readonly Reform form;

        public Platform(Game game)
        {
            this.game = game;

            //Placeholder settings for now
            form = new Reform(game)
            {
                Text = "Game Title Placeholder",
                Size = new Size(1280, 720),
                FormBorderStyle = FormBorderStyle.FixedSingle,
                BackColor = Color.HotPink
            };

            invoker = new MethodInvoker(PlatformIdle);

            form.Hook();
        }

        public void BeginRun()
        {
            //Hook the app idle and begin the game
            Application.Idle += PlatformIdle;
            Application.Run(form);

            //Unhook after the game closes
            //Not strictly necessary, but good practice
            Application.Idle -= PlatformIdle;
        }

        private void PlatformIdle(object sender, EventArgs e) => PlatformIdle();

        private void PlatformIdle()
        {
            if (result != null)
            {
                form.EndInvoke(result);
                result = null;
            }
            else
            {
                result = form.BeginInvoke(invoker);
            }

            game.ProcessFrame();
        }

        public void ResizeWindow(int x, int y)
        {
            form.ClientSize = new Size(x, y);
            form.Center();
            game.Graphics.RecreateGraphics();
        }
    }
}
