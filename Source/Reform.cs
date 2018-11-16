using System;
using System.Drawing;
using System.Windows.Forms;

namespace SharpSlugsEngine 
{
    internal class Reform : Form
    {
        private readonly Game game;

        public Reform(Game game)
        {
            this.game = game;
        }

        ~Reform()
        {
            UnlockCursor();
        }

        public void Hook()
        {
            Resize -= RecreateGraphics;
            Resize += RecreateGraphics;

            GotFocus -= LockCursor;
            GotFocus += LockCursor;

            Resize -= LockCursor;
            Resize += LockCursor;

            ResizeEnd -= LockCursor;
            ResizeEnd += LockCursor;
        }

        public void LockCursor(object sender = null, EventArgs args = null)
        {
            if (!game.LockCursor)
            {
                return;
            }

            Cursor.Clip = new Rectangle(Location.X - PointToClient(Location).X, Location.Y - PointToClient(Location).Y, ClientSize.Width, ClientSize.Height);
        }

        public void UnlockCursor()
        {
            Cursor.Clip = Rectangle.Empty;
        }

        private void RecreateGraphics(object sender, EventArgs e)
        {
            game.Graphics.RecreateGraphics();
            game.Cameras.Resize(game._resolution, ClientSize);
            game._resolution = new Vector2(ClientSize.Width, ClientSize.Height);
        }

        public void Center()
        {
            Screen currentScreen = Screen.FromControl(this);
            Rectangle area = currentScreen.WorkingArea;

            Top = (area.Height - Height) / 2;
            Left = (area.Width - Width) / 2;

            LockCursor();
        }
    }
}
