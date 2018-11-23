using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SharpSlugsEngine.Input
{
    /// <summary>
    /// Provides a method for binding multiple buttons to one action
    /// </summary>
    public class InputAction
    {
        private readonly List<Keys> keys;

        private Game game;
        private List<XboxController> xboxControllers;
        private XboxController.ButtonType xboxButtons;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputAction"/> class with the given parent <see cref="Game"/>
        /// </summary>
        /// <param name="game">The parent <see cref="Game"/> of the new <see cref="InputAction"/></param>
        public InputAction(Game game)
        {
            this.game = game;

            keys = new List<Keys>();
            xboxControllers = new List<XboxController>();
            xboxButtons = 0;
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="InputAction"/> is pressed
        /// </summary>
        public bool IsPressed
        {
            get
            {
                foreach (Keys key in keys)
                {
                    if (game.Keyboard.IsPressed(key))
                    {
                        return true;
                    }
                }

                foreach (XboxController controller in xboxControllers)
                {
                    if (controller.AnyIsPressed(xboxButtons))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="InputAction"/> was pressed in the last frame
        /// </summary>
        public bool WasPressed
        {
            get
            {
                foreach (Keys key in keys)
                {
                    if (game.Keyboard.WasPressed(key))
                    {
                        return true;
                    }
                }

                foreach (XboxController controller in xboxControllers)
                {
                    if (controller.AnyWasPressed(xboxButtons))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Adds a <see cref="GameController"/> to the list of devices to check input on
        /// </summary>
        /// <param name="controller">The <see cref="GameController"/> to add</param>
        public void AddDevice(GameController controller)
        {
            if (controller is XboxController xboxController && !xboxControllers.Contains(controller))
            {
                xboxControllers.Add(xboxController);
            }
        }

        /// <summary>
        /// Removes a <see cref="GameController"/> from the list of devices to check input on
        /// </summary>
        /// <param name="controller">The <see cref="GameController"/> to remove</param>
        public void RemoveDevice(GameController controller)
        {
            if (controller is XboxController xboxController)
            {
                xboxControllers.RemoveAll(c => c == xboxController);
            }
        }

        /// <summary>
        /// Adds specified Xbox buttons to the list of inputs to check
        /// </summary>
        /// <param name="buttons">The buttons to add</param>
        public void AddXboxButtons(XboxController.ButtonType buttons)
        {
            xboxButtons |= buttons;
        }

        /// <summary>
        /// Removes specified Xbox buttons from the list of inputs to check
        /// </summary>
        /// <param name="buttons">The buttons to remove</param>
        public void RemoveXboxButtons(XboxController.ButtonType buttons)
        {
            xboxButtons &= ~buttons;
        }

        /// <summary>
        /// Adds the specified keyboard key to the list of inputs to check
        /// </summary>
        /// <param name="key">The keyboard key to add</param>
        public void AddKey(Keys key)
        {
            if (!keys.Contains(key))
            {
                keys.Add(key);
            }
        }

        /// <summary>
        /// Removes the specified keyboard key from the list of inputs to check
        /// </summary>
        /// <param name="key">The keyboard key to remove</param>
        public void RemoveKey(Keys key)
        {
            if (keys.Contains(key))
            {
                keys.Remove(key);
            }
        }
    }
}
