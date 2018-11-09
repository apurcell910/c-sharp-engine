using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SharpSlugsEngine.Input
{
    public class InputAction
    {
        private Game _game;

        private List<XboxController> xboxControllers;

        private readonly List<Keys> keys;
        private XboxController.ButtonType xboxButtons;

        public InputAction(Game game)
        {
            _game = game;

            keys = new List<Keys>();
            xboxControllers = new List<XboxController>();
            xboxButtons = 0;
        }

        public void AddDevice(GameController controller)
        {
            if (controller is XboxController xboxController && !xboxControllers.Contains(controller))
            {
                xboxControllers.Add(xboxController);
            }
        }

        public void RemoveDevice(GameController controller)
        {
            if (controller is XboxController xboxController)
            {
                xboxControllers.RemoveAll(c => c == xboxController);
            }
        }

        public void AddXboxButtons(XboxController.ButtonType buttons)
        {
            xboxButtons |= buttons;
        }

        public void RemoveXboxButtons(XboxController.ButtonType buttons)
        {
            xboxButtons &= ~buttons;
        }

        public void AddKey(Keys key)
        {
            if (!keys.Contains(key))
            {
                keys.Add(key);
            }
        }

        public void RemoveKey(Keys key)
        {
            if (keys.Contains(key))
            {
                keys.Remove(key);
            }
        }

        public bool IsPressed
        {
            get
            {
                foreach (Keys key in keys)
                {
                    if (_game.Keyboard.IsPressed(key)) return true;
                }

                foreach (XboxController controller in xboxControllers)
                {
                    if (controller.AnyIsPressed(xboxButtons)) return true;
                }

                return false;
            }
        }

        public bool WasPressed
        {
            get
            {
                foreach (Keys key in keys)
                {
                    if (_game.Keyboard.WasPressed(key)) return true;
                }

                foreach (XboxController controller in xboxControllers)
                {
                    if (controller.AnyWasPressed(xboxButtons)) return true;
                }

                return false;
            }
        }
    }
}
