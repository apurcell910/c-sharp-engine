using System;

namespace SharpSlugsEngine
{
    public class Xbox360Controller
    {
        private InputDevice _device;
        private ButtonState _asyncState;
        private ButtonState _oldState;
        private ButtonState _currentState;

        internal string Path => _device.DevicePath;

        public ButtonState State { get; private set; }

        internal Xbox360Controller(InputDevice device)
        {
            _device = device ?? throw new ArgumentNullException();
            
            _device.OnDisconnect += OnDisconnect;

            _device.ReadAsync(ReadDeviceBytes);
        }

        private void OnDisconnect()
        {
            _disconnected?.Invoke();
        }

        private void OnConnect()
        {
            _connected?.Invoke();
            _device.ReadAsync(ReadDeviceBytes);
        }

        public void Update()
        {
            if (_device == null) return;

            if (!_device._connected)
            {
                if (_device.TryReconnect()) OnConnect();
                return;
            }

            _oldState = _currentState;
            _currentState = _asyncState;

            Button a = new Button(_currentState.A.IsPressed, _currentState.A.IsPressed && !_oldState.A.IsPressed);
            Button b = new Button(_currentState.B.IsPressed, _currentState.B.IsPressed && !_oldState.B.IsPressed);
            Button x = new Button(_currentState.X.IsPressed, _currentState.X.IsPressed && !_oldState.X.IsPressed);
            Button y = new Button(_currentState.Y.IsPressed, _currentState.Y.IsPressed && !_oldState.Y.IsPressed);
            Button lb = new Button(_currentState.LB.IsPressed, _currentState.LB.IsPressed && !_oldState.LB.IsPressed);
            Button rb = new Button(_currentState.RB.IsPressed, _currentState.RB.IsPressed && !_oldState.RB.IsPressed);
            Button back = new Button(_currentState.Back.IsPressed, _currentState.Back.IsPressed && !_oldState.Back.IsPressed);
            Button start = new Button(_currentState.Start.IsPressed, _currentState.Start.IsPressed && !_oldState.Start.IsPressed);
            Button dpadUp = new Button(_currentState.DPadUp.IsPressed, _currentState.DPadUp.IsPressed && !_oldState.DPadUp.IsPressed);
            Button dpadDown = new Button(_currentState.DPadDown.IsPressed, _currentState.DPadDown.IsPressed && !_oldState.DPadDown.IsPressed);
            Button dpadLeft = new Button(_currentState.DPadLeft.IsPressed, _currentState.DPadLeft.IsPressed && !_oldState.DPadLeft.IsPressed);
            Button dpadRight = new Button(_currentState.DPadRight.IsPressed, _currentState.DPadRight.IsPressed && !_oldState.DPadRight.IsPressed);

            State = new ButtonState(a, b, x, y, lb, rb, back, start, dpadLeft, dpadRight, dpadUp, dpadDown);

            if (_aPressed != null && State.A.WasPressed) _aPressed();
            if (_bPressed != null && State.B.WasPressed) _bPressed();
            if (_xPressed != null && State.X.WasPressed) _xPressed();
            if (_yPressed != null && State.Y.WasPressed) _yPressed();
            if (_lbPressed != null && State.LB.WasPressed) _lbPressed();
            if (_rbPressed != null && State.RB.WasPressed) _rbPressed();
            if (_backPressed != null && State.Back.WasPressed) _backPressed();
            if (_startPressed != null && State.Start.WasPressed) _startPressed();
            if (_dpadUpPressed != null && State.DPadUp.WasPressed) _dpadUpPressed();
            if (_dpadDownPressed != null && State.DPadDown.WasPressed) _dpadDownPressed();
            if (_dpadLeftPressed != null && State.DPadLeft.WasPressed) _dpadLeftPressed();
            if (_dpadRightPressed != null && State.DPadRight.WasPressed) _dpadRightPressed();
        }

        private void ReadDeviceBytes(byte[] bytes)
        {
            if (_device == null || !_device._connected) return;

            bool dpadUp = false;
            bool dpadDown = false;
            bool dpadLeft = false;
            bool dpadRight = false;

            switch (bytes[12] / 4)
            {
                case 1:
                    dpadUp = true;
                    break;
                case 2:
                    dpadUp = true;
                    dpadRight = true;
                    break;
                case 3:
                    dpadRight = true;
                    break;
                case 4:
                    dpadDown = true;
                    dpadRight = true;
                    break;
                case 5:
                    dpadDown = true;
                    break;
                case 6:
                    dpadDown = true;
                    dpadLeft = true;
                    break;
                case 7:
                    dpadLeft = true;
                    break;
                case 8:
                    dpadUp = true;
                    dpadLeft = true;
                    break;
            }

            bool a = (bytes[11] & 1) != 0;
            bool b = (bytes[11] & 2) != 0;
            bool x = (bytes[11] & 4) != 0;
            bool y = (bytes[11] & 8) != 0;
            bool lb = (bytes[11] & 16) != 0;
            bool rb = (bytes[11] & 32) != 0;
            bool back = (bytes[11] & 64) != 0;
            bool start = (bytes[11] & 128) != 0;

            _asyncState = new ButtonState(a, b, x, y, lb, rb, back, start, dpadLeft, dpadRight, dpadUp, dpadDown);

            _device.ReadAsync(ReadDeviceBytes);
        }

        public delegate void ButtonPressed();

        //Hiding the actual events with backing fields prevents setting them to null
        private event ButtonPressed _aPressed;
        public event ButtonPressed APressed
        {
            add => _aPressed += value;
            remove => _aPressed -= value;
        }

        private event ButtonPressed _bPressed;
        public event ButtonPressed BPressed
        {
            add => _bPressed += value;
            remove => _bPressed -= value;
        }

        private event ButtonPressed _xPressed;
        public event ButtonPressed XPressed
        {
            add => _xPressed += value;
            remove => _xPressed -= value;
        }

        private event ButtonPressed _yPressed;
        public event ButtonPressed YPressed
        {
            add => _yPressed += value;
            remove => _yPressed -= value;
        }

        private event ButtonPressed _lbPressed;
        public event ButtonPressed LBPressed
        {
            add => _lbPressed += value;
            remove => _lbPressed -= value;
        }

        private event ButtonPressed _rbPressed;
        public event ButtonPressed RBPressed
        {
            add => _rbPressed += value;
            remove => _rbPressed -= value;
        }

        private event ButtonPressed _backPressed;
        public event ButtonPressed BackPressed
        {
            add => _backPressed += value;
            remove => _backPressed -= value;
        }

        private event ButtonPressed _startPressed;
        public event ButtonPressed StartPressed
        {
            add => _startPressed += value;
            remove => _startPressed -= value;
        }

        private event ButtonPressed _dpadUpPressed;
        public event ButtonPressed DPadUpPressed
        {
            add => _dpadUpPressed += value;
            remove => _dpadUpPressed -= value;
        }

        private event ButtonPressed _dpadDownPressed;
        public event ButtonPressed DPadDownPressed
        {
            add => _dpadDownPressed += value;
            remove => _dpadDownPressed -= value;
        }

        private event ButtonPressed _dpadLeftPressed;
        public event ButtonPressed DPadLeftPressed
        {
            add => _dpadLeftPressed += value;
            remove => _dpadLeftPressed -= value;
        }

        private event ButtonPressed _dpadRightPressed;
        public event ButtonPressed DPadRightPressed
        {
            add => _dpadRightPressed += value;
            remove => _dpadRightPressed -= value;
        }

        public delegate void ControllerConnection();

        private event ControllerConnection _disconnected;
        public event ControllerConnection Disconnected
        {
            add => _disconnected += value;
            remove => _disconnected -= value;
        }

        private event ControllerConnection _connected;
        public event ControllerConnection Connected
        {
            add => _connected += value;
            remove => _connected -= value;
        }
    }

    public struct ButtonState
    {
        internal ButtonState(bool A, bool B, bool X, bool Y, bool LB, bool RB, bool Back, bool Start, bool DPadLeft, bool DPadRight, bool DPadUp, bool DPadDown)
        {
            this.A = new Button(A);
            this.B = new Button(B);
            this.X = new Button(X);
            this.Y = new Button(Y);
            this.LB = new Button(LB);
            this.RB = new Button(RB);
            this.Back = new Button(Back);
            this.Start = new Button(Start);
            this.DPadLeft = new Button(DPadLeft);
            this.DPadRight = new Button(DPadRight);
            this.DPadUp = new Button(DPadUp);
            this.DPadDown = new Button(DPadDown);
        }

        internal ButtonState(Button A, Button B, Button X, Button Y, Button LB, Button RB, Button Back, Button Start, Button DPadLeft, Button DPadRight, Button DPadUp, Button DPadDown)
        {
            this.A = A;
            this.B = B;
            this.X = X;
            this.Y = Y;
            this.LB = LB;
            this.RB = RB;
            this.Back = Back;
            this.Start = Start;
            this.DPadLeft = DPadLeft;
            this.DPadRight = DPadRight;
            this.DPadUp = DPadUp;
            this.DPadDown = DPadDown;
        }

        public Button A { get; private set; }
        public Button B { get; private set; }
        public Button X { get; private set; }
        public Button Y { get; private set; }
        public Button LB { get; private set; }
        public Button RB { get; private set; }
        public Button Back { get; private set; }
        public Button Start { get; private set; }

        public Button DPadLeft { get; private set; }
        public Button DPadRight { get; private set; }
        public Button DPadUp { get; private set; }
        public Button DPadDown { get; private set; }
    }

    public struct Button
    {
        internal Button(bool pressed)
        {
            IsPressed = pressed;
            WasPressed = false;
        }

        internal Button(bool pressed, bool wasPressed)
        {
            IsPressed = pressed;
            WasPressed = wasPressed;
        }

        public bool IsPressed { get; private set; }
        public bool WasPressed { get; private set; }
    }
}
