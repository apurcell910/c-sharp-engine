using System.Drawing;
using System.Windows.Forms;

namespace SharpSlugsEngine 
{
    internal class Reform : Form
    {
        public void Center()
        {
            Screen currentScreen = Screen.FromControl(this);
            Rectangle area = currentScreen.WorkingArea;

            Top = (area.Height - Height) / 2;
            Left = (area.Width - Width) / 2;
        }
    }
}
