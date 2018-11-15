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

        public void Hook()
        {
            Resize -= RecreateGraphics;
            Resize += RecreateGraphics;
        }

        private void RecreateGraphics(object sender, System.EventArgs e)
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
        }
    }
}
