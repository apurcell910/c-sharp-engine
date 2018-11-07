using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpSlugsEngine.Input
{
    public class InputAction
    {
        private Game _game;

        private List<Xbox360Controller> controllers360;

        private readonly List<Keys> keys;
        private Xbox360Controller.ButtonType buttons360;

        public InputAction(Game game)
        {
            _game = game;

            keys = new List<Keys>();
            controllers360 = new List<Xbox360Controller>();
            buttons360 = 0;
        }

        public void AddDevice(GameController controller)
        {
            switch (controller.Type)
            {
                case ControllerType.Xbox:
                    break;
                case ControllerType.Xbox360:
                    if (!controllers360.Contains(controller))
                    {
                        controllers360.Add((Xbox360Controller)controller);
                    }
                    break;
                case ControllerType.XboxOne:
                    break;
                case ControllerType.Playstation3:
                    break;
                case ControllerType.Playstation4:
                    break;
            }
        }

        public void RemoveDevice(GameController controller)
        {
            switch (controller.Type)
            {
                case ControllerType.Xbox:
                    break;
                case ControllerType.Xbox360:
                    if (controllers360.Contains(controller))
                    {
                        controllers360.Remove((Xbox360Controller)controller);
                    }
                    break;
                case ControllerType.XboxOne:
                    break;
                case ControllerType.Playstation3:
                    break;
                case ControllerType.Playstation4:
                    break;
            }
        }

        public void Add360Buttons(Xbox360Controller.ButtonType buttons)
        {
            buttons360 |= buttons;
        }

        public void Remove360Buttons(Xbox360Controller.ButtonType buttons)
        {
            buttons360 &= ~buttons;
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

                foreach (Xbox360Controller controller in controllers360)
                {
                    if (controller.AnyIsPressed(buttons360)) return true;
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

                foreach (Xbox360Controller controller in controllers360)
                {
                    if (controller.AnyWasPressed(buttons360)) return true;
                }

                return false;
            }
        }
    }
}
