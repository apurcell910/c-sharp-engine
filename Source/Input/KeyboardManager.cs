using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SharpSlugsEngine.Input
{
    public class KeyboardManager : IDisposable
    {
        //Caching this should improve performance a bit
        private static readonly Keys[] allKeys = (Keys[])Enum.GetValues(typeof(Keys));

        private readonly Game _game;

        private readonly Dictionary<Keys, bool> _asyncKeys;
        private readonly Dictionary<Keys, bool> _oldKeys;
        private readonly Dictionary<Keys, bool> _currentKeys;

        internal KeyboardManager(Game game)
        {
            _game = game;

            //Fill all dictionaries with false
            _asyncKeys = new Dictionary<Keys, bool>();
            foreach (Keys key in allKeys)
            {
                //This check is required because the Keys enum contains duplicate values (ex. return/enter are both 13)
                if (!_asyncKeys.ContainsKey(key))
                {
                    _asyncKeys.Add(key, false);
                }
            }

            _oldKeys = new Dictionary<Keys, bool>(_asyncKeys);
            _currentKeys = new Dictionary<Keys, bool>(_asyncKeys);

            //Begin looking for input
            HookForm();
        }

        //Synchronize the current/old keys with the main update loop
        internal void Update()
        {
            foreach (Keys key in allKeys)
            {
                _oldKeys[key] = _currentKeys[key];
                _currentKeys[key] = _asyncKeys[key];
            }
        }

        private void UnhookForm()
        {
            _game.platform.form.KeyDown -= RegisterKeyDown;
            _game.platform.form.KeyUp -= RegisterKeyUp;
        }

        private void HookForm()
        {
            //Prevent duplicate hooks
            UnhookForm();

            _game.platform.form.KeyDown += RegisterKeyDown;
            _game.platform.form.KeyUp += RegisterKeyUp;
        }

        //Update async state when keys are pressed/released
        private void RegisterKeyDown(object sender, KeyEventArgs args) => _asyncKeys[args.KeyCode] = true;
        private void RegisterKeyUp(object sender, KeyEventArgs args) => _asyncKeys[args.KeyCode] = false;

        //Using TryGetValue on these to prevent errors from user input outside the range of the Keys enum
        public bool IsPressed(Keys key)
        {
            _currentKeys.TryGetValue(key, out bool ret);
            return ret;
        }

        public bool WasPressed(Keys key)
        {
            _oldKeys.TryGetValue(key, out bool old);
            _currentKeys.TryGetValue(key, out bool current);

            return current && !old;
        }

        //Alias for the IsPressed/WasPressed functions for if people prefer accessing as an array
        public KeyState this[Keys key] => new KeyState(IsPressed(key), WasPressed(key));

        //Remove hooks so that the form doesn't keep the object alive
        void IDisposable.Dispose() => UnhookForm();
    }

    public struct KeyState
    {
        internal KeyState(bool isPressed, bool wasPressed)
        {
            IsPressed = isPressed;
            WasPressed = wasPressed;
        }

        public bool IsPressed { get; private set; }
        public bool WasPressed { get; private set; }
    }
}
