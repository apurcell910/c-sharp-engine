using System;
using System.Drawing;
using System.Windows.Forms;

namespace SharpSlugsEngine 
{
    /// <summary>
    /// An expansion of the <see cref="Form"/> class to add useful functionality for games
    /// </summary>
    internal class Reform : Form
    {
        private readonly Game game;

        /// <summary>
        /// Initializes a new instance of the <see cref="Reform"/> class with "<paramref name="game"/>" as the parent <see cref="Game"/>
        /// </summary>
        /// <param name="game">The parent <see cref="Game"/> object of the new <see cref="Reform"/></param>
        public Reform(Game game)
        {
            this.game = game;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Reform"/> class by unlocking the system cursor
        /// </summary>
        ~Reform()
        {
            UnlockCursor();
        }

        /// <summary>
        /// Hooks the functions within this <see cref="Reform"/> onto various <see cref="Form"/> events.
        /// Must be called after initialization.
        /// </summary>
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

        /// <summary>
        /// Locks the mouse cursor within the inner area of the <see cref="Form"/> window
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="args">The parameter is not used.</param>
        public void LockCursor(object sender = null, EventArgs args = null)
        {
            if (!game.LockCursor)
            {
                return;
            }

            Cursor.Clip = new Rectangle(Location.X - PointToClient(Location).X, Location.Y - PointToClient(Location).Y, ClientSize.Width, ClientSize.Height);
        }

        /// <summary>
        /// Unlocks the mouse cursor
        /// </summary>
        public void UnlockCursor()
        {
            Cursor.Clip = Rectangle.Empty;
        }

        /// <summary>
        /// Moves the <see cref="Form"/> window to the center of the current <see cref="Screen"/>
        /// </summary>
        public void Center()
        {
            Screen currentScreen = Screen.FromControl(this);
            Rectangle area = currentScreen.WorkingArea;

            Top = (area.Height - Height) / 2;
            Left = (area.Width - Width) / 2;

            LockCursor();
        }

        /// <summary>
        /// Recreates the graphics objects associated with <see cref="game"/>
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void RecreateGraphics(object sender, EventArgs e)
        {
            game.Graphics.RecreateGraphics();
            game.Cameras.Resize(game.ResolutionInternal, ClientSize);
            game.ResolutionInternal = new Vector2(ClientSize.Width, ClientSize.Height);
        }
    }
}
