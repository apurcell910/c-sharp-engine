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
            game.Resolution = new Vector2(Width, Height);
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
