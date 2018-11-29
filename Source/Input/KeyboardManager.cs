using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SharpSlugsEngine.Input
{
    /// <summary>
    /// Contains the state of a keyboard key
    /// </summary>
    public struct KeyState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyState"/> struct with the given values
        /// </summary>
        /// <param name="isPressed">Whether the key is currently pressed</param>
        /// <param name="wasPressed">Whether the key was pressed on the last frame</param>
        internal KeyState(bool isPressed, bool wasPressed)
        {
            IsPressed = isPressed;
            WasPressed = wasPressed;
        }

        /// <summary>
        /// Gets a value indicating whether the key is pressed
        /// </summary>
        public bool IsPressed { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the key was pressed
        /// </summary>
        public bool WasPressed { get; private set; }
    }

    /// <summary>
    /// Tracks keyboard events and key states
    /// </summary>
    public class KeyboardManager : IDisposable
    {
        // Caching this should improve performance a bit
        private static readonly Keys[] AllKeys = (Keys[])Enum.GetValues(typeof(Keys));

        private readonly Game game;

        private readonly Dictionary<Keys, bool> asyncKeys;
        private readonly Dictionary<Keys, bool> oldKeys;
        private readonly Dictionary<Keys, bool> currentKeys;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardManager"/> class with the given parent <see cref="Game"/>
        /// </summary>
        /// <param name="game">The parent <see cref="Game"/> of the new <see cref="KeyboardManager"/></param>
        internal KeyboardManager(Game game)
        {
            this.game = game;

            // Fill all dictionaries with false
            asyncKeys = new Dictionary<Keys, bool>();
            foreach (Keys key in AllKeys)
            {
                // This check is required because the Keys enum contains duplicate values (ex. return/enter are both 13)
                if (!asyncKeys.ContainsKey(key))
                {
                    asyncKeys.Add(key, false);
                }
            }

            oldKeys = new Dictionary<Keys, bool>(asyncKeys);
            currentKeys = new Dictionary<Keys, bool>(asyncKeys);

            // Begin looking for input
            HookForm();
        }

        /// <summary>
        /// Alias for the <see cref="IsPressed(Keys)"/> and <see cref="WasPressed(Keys)"/> functions
        /// </summary>
        /// <param name="key">The key to check input on</param>
        /// <returns>A <see cref="KeyState"/> containing information about the specified key</returns>
        public KeyState this[Keys key] => new KeyState(IsPressed(key), WasPressed(key));

        /// <summary>
        /// Checks if a given key is pressed
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>A value indicating whether the key is pressed</returns>
        public bool IsPressed(Keys key)
        {
            currentKeys.TryGetValue(key, out bool ret);
            return ret;
        }

        /// <summary>
        /// Checks if a given key was pressed
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>A value indicating whether the key was pressed</returns>
        public bool WasPressed(Keys key)
        {
            oldKeys.TryGetValue(key, out bool old);
            currentKeys.TryGetValue(key, out bool current);

            return current && !old;
        }

        /// <summary>
        /// Removes hooks so that the form doesn't keep the object alive
        /// </summary>
        void IDisposable.Dispose() => UnhookForm();

        /// <summary>
        /// Synchronizes the current/old keys with the main update loop
        /// </summary>
        internal void Update()
        {
            foreach (Keys key in AllKeys)
            {
                oldKeys[key] = currentKeys[key];
                currentKeys[key] = asyncKeys[key];
            }

            _singleKey?.Invoke();
        }

        /// <summary>
        /// Hooks the key events on the parent <see cref="Game"/>'s <see cref="Form"/>
        /// </summary>
        private void HookForm()
        {
            // Prevent duplicate hooks
            UnhookForm();

            game.Platform.Form.KeyDown += RegisterKeyDown;
            game.Platform.Form.KeyUp += RegisterKeyUp;
        }

        /// <summary>
        /// Unhooks the key events on the parent <see cref="Game"/>'s <see cref="Form"/>
        /// </summary>
        private void UnhookForm()
        {
            game.Platform.Form.KeyDown -= RegisterKeyDown;
            game.Platform.Form.KeyUp -= RegisterKeyUp;
        }

        /// <summary>
        /// Marks a key as pressed in the asynchronous state
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="args">Contains the key to set as pressed</param>
        private void RegisterKeyDown(object sender, KeyEventArgs args) => asyncKeys[args.KeyCode] = true;

        /// <summary>
        /// Marks a key as not pressed in the asynchronous state
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="args">Contains the key to set as not pressed</param>
        private void RegisterKeyUp(object sender, KeyEventArgs args) => asyncKeys[args.KeyCode] = false;

        #region Timothy WIP
        /// <summary>
        /// Checks if any alphabetical key is pressed
        /// </summary>
        /// <returns>True if an alphabetical key is pressed, False otherwise</returns>
        public bool AlphaIsPressed()
        {
            foreach (KeyValuePair<Keys, bool> entry in currentKeys)
            {
                if (entry.Key <= Keys.Z && entry.Key >= Keys.A)
                {
                    if (entry.Value == true)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns a list of alphabetical <see cref="Keys"/> pressed
        /// </summary>
        /// <returns>List of alphabetical <see cref="Keys"/> pressed</returns>
        public List<Keys> ListAlphaPressed()
        {
            List<Keys> o = new List<Keys>();
            foreach (KeyValuePair<Keys, bool> entry in currentKeys)
            {
                if (entry.Key <= Keys.Z && entry.Key >= Keys.A)
                {
                    if (entry.Value == true)
                    { 
                        o.Add(entry.Key);
                    }
                }
            }

            return o;
        }

        /// <summary>
        /// checks if any numerical keys are pressed
        /// </summary>
        /// <returns>true if a numerical key is pressed, false otherwise</returns>
        public bool NumIsPressed()
        {
            foreach (KeyValuePair<Keys, bool> entry in currentKeys)
            {
                if ((entry.Key <= Keys.NumPad9 && entry.Key >= Keys.NumPad0)
                    || (entry.Key <= Keys.D9 && entry.Key >= Keys.D0))
                {
                    if (entry.Value == true)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns a list of numerical <see cref="Keys"/> pressed
        /// </summary>
        /// <returns>List of numerical keys pressed</returns>
        public List<Keys> ListNumPressed()
        {
            List<Keys> o = new List<Keys>();
            foreach (KeyValuePair<Keys, bool> entry in currentKeys)
            {
                if ((entry.Key <= Keys.NumPad9 && entry.Key >= Keys.NumPad0)
                    || (entry.Key <= Keys.D9 && entry.Key >= Keys.D0))
                {
                    if (entry.Value == true)
                    {
                        o.Add(entry.Key);
                    }
                }
            }

            return o;
        }

        public bool ArrowIsPressed() ////Todo Timothy
        {
            return false;
        }

        public delegate void KeyPress();

        private event KeyPress _singleKey;

        public event KeyPress SingleKey
        {
            add => _singleKey += value;
            remove => _singleKey -= value;
        }

        public void AddKeybind(Keys key, Event e)
        {
            SingleKey += () => 
            {
                if (IsPressed(key))
                {
                    e.CallEvent();
                }
            };
        }

        public void RemoveKeybind(Keys key, Event e)
        {
            SingleKey -= () => 
            {
                if (IsPressed(key))
                {
                    e.CallEvent();
                }
            };
        }

        public void AddMultiBind(List<Keys> l, Event e)
        {
            SingleKey += () => 
            {
                if (l.TrueForAll(c => IsPressed(c)))
                {
                    e.CallEvent();
                }
            };
        }

        public void AddAlphaBind(Event e)
        {
            SingleKey += () => 
            {
                if (AlphaIsPressed())
                {
                    e.CallEvent();
                }
            };
        }

        public void RemoveAlphaBind(Event e)
        {
            SingleKey -= () => 
            {
                if (AlphaIsPressed())
                {
                    e.CallEvent();
                }
            };
        }

        public void AddMassAlphaBind(Event e)
        {
            SingleKey += () => 
            {
                foreach (Keys key in ListAlphaPressed())
                {
                    e.CallEvent(key);
                }
            };
        }

        public void RemoveMassAlphaBind(Event e)
        {
            SingleKey -= () => 
            {
                foreach (Keys key in ListAlphaPressed())
                {
                    e.CallEvent(key);
                }
            };
        }

        public void AddNumBind(Event e)
        {
            SingleKey += () =>
            {
                if (NumIsPressed())
                {
                    e.CallEvent();
                }
            };
        }

        public void RemoveNumBind(Event e)
        {
            SingleKey -= () => 
            {
                if (NumIsPressed())
                {
                    e.CallEvent();
                }
            };
        }

        public void AddAlphaNumBind(Event e)
        {
            SingleKey += () => 
            {
                if (AlphaIsPressed() || NumIsPressed())
                {
                    e.CallEvent();
                }
            };
        }

        public void RemoveAlphaNumBind(Event e)
        {
            SingleKey -= () => 
            {
                if (AlphaIsPressed() || NumIsPressed())
                {
                    e.CallEvent();
                }
            };
        }
        #endregion
    }
}
