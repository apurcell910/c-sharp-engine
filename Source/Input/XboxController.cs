using System;

namespace SharpSlugsEngine.Input
{
    public class XboxController : GameController
    {
        private static readonly ButtonType[] allButtons = (ButtonType[])Enum.GetValues(typeof(ButtonType));

        private InputDevice _device;

        private ButtonState _asyncButtonState;
        private ButtonState _oldButtonState;
        private ButtonState _currentButtonState;

        private ControlStick _asyncLStickState;
        private ControlStick _oldLStickState;
        private ControlStick _currentLStickState;

        private ControlStick _asyncRStickState;
        private ControlStick _oldRStickState;
        private ControlStick _currentRStickState;

        internal override string Path => _device.DevicePath;

        private ControllerType _type;
        public override ControllerType Type => _type;
        public bool IsConnected => _device._connected;

        public ButtonState Buttons { get; private set; }
        public ControlStick LeftStick { get; private set; }
        public ControlStick RightStick { get; private set; }

        internal XboxController(InputDevice device)
        {
            //Everything is gonna break anyway if device is null, might as well throw an obvious exception
            _device = device ?? throw new ArgumentNullException();

            //Make sure this is actually an xbox controller and figure out which kind it is
            if (device.VendorID != DeviceManager.VID_MICROSOFT) throw new ArgumentException("Device is not an Xbox controller");
            switch (device.ProductID)
            {
                case DeviceManager.PID_XBOX:
                    _type = ControllerType.Xbox;
                    break;
                case DeviceManager.PID_XBOX_360:
                    _type = ControllerType.Xbox360;
                    break;
                case DeviceManager.PID_XBOX_ONE:
                    _type = ControllerType.XboxOne;
                    break;
                case DeviceManager.PID_XBOX_ONE_S:
                    _type = ControllerType.XboxOneS;
                    break;
                default:
                    throw new ArgumentException("Device is not an Xbox controller");
            }

            //Just hooking this to be able to pass the event up further. Could implement better
            _device.OnDisconnect += OnDisconnect;

            //Begin reading data from the controller
            _device.ReadAsync(ReadDeviceBytes);
        }
        
        public bool AnyIsPressed(ButtonType buttons)
        {
            foreach (ButtonType type in allButtons)
            {
                if ((buttons & type) == type)
                {
                    if (GetButtonState(type).IsPressed) return true;
                }
            }

            return false;
        }

        public bool AnyWasPressed(ButtonType buttons)
        {
            foreach (ButtonType type in allButtons)
            {
                if ((buttons & type) == type)
                {
                    if (GetButtonState(type).WasPressed) return true;
                }
            }

            return false;
        }

        public Button GetButtonState(ButtonType button)
        {
            switch (button)
            {
                case ButtonType.A:
                    return Buttons.A;
                case ButtonType.B:
                    return Buttons.B;
                case ButtonType.X:
                    return Buttons.X;
                case ButtonType.Y:
                    return Buttons.Y;
                case ButtonType.Back:
                    return Buttons.Back;
                case ButtonType.Start:
                    return Buttons.Start;
                case ButtonType.LB:
                    return Buttons.LB;
                case ButtonType.RB:
                    return Buttons.RB;
                case ButtonType.DPadLeft:
                    return Buttons.DPadLeft;
                case ButtonType.DPadRight:
                    return Buttons.DPadRight;
                case ButtonType.DPadUp:
                    return Buttons.DPadUp;
                case ButtonType.DPadDown:
                    return Buttons.DPadDown;
                case ButtonType.LStick:
                    return LeftStick.Button;
                case ButtonType.RStick:
                    return RightStick.Button;
                default:
                    return default(Button);
            }
        }

        private void OnDisconnect()
        {
            _disconnected?.Invoke();
        }

        private void OnConnect()
        {
            //Pass the event upward and restart reading data
            _connected?.Invoke();
            _device.ReadAsync(ReadDeviceBytes);
        }

        internal override void Update()
        {
            if (_device == null) throw new NullReferenceException("_device cannot be null");

            //Try to reconnect if applicable
            if (!_device._connected)
            {
                if (_device.TryReconnect()) OnConnect();
                return;
            }

            //Update the current/old controller state from the asynchronous reading
            _oldButtonState = _currentButtonState;
            _currentButtonState = _asyncButtonState;

            _oldLStickState = _currentLStickState;
            _currentLStickState = _asyncLStickState;

            _oldRStickState = _currentRStickState;
            _currentRStickState = _asyncRStickState;

            //Just a ton of boilerplate here
            Button a = new Button(_currentButtonState.A.IsPressed, _currentButtonState.A.IsPressed && !_oldButtonState.A.IsPressed);
            Button b = new Button(_currentButtonState.B.IsPressed, _currentButtonState.B.IsPressed && !_oldButtonState.B.IsPressed);
            Button x = new Button(_currentButtonState.X.IsPressed, _currentButtonState.X.IsPressed && !_oldButtonState.X.IsPressed);
            Button y = new Button(_currentButtonState.Y.IsPressed, _currentButtonState.Y.IsPressed && !_oldButtonState.Y.IsPressed);
            Button lb = new Button(_currentButtonState.LB.IsPressed, _currentButtonState.LB.IsPressed && !_oldButtonState.LB.IsPressed);
            Button rb = new Button(_currentButtonState.RB.IsPressed, _currentButtonState.RB.IsPressed && !_oldButtonState.RB.IsPressed);
            Button back = new Button(_currentButtonState.Back.IsPressed, _currentButtonState.Back.IsPressed && !_oldButtonState.Back.IsPressed);
            Button start = new Button(_currentButtonState.Start.IsPressed, _currentButtonState.Start.IsPressed && !_oldButtonState.Start.IsPressed);
            Button dpadUp = new Button(_currentButtonState.DPadUp.IsPressed, _currentButtonState.DPadUp.IsPressed && !_oldButtonState.DPadUp.IsPressed);
            Button dpadDown = new Button(_currentButtonState.DPadDown.IsPressed, _currentButtonState.DPadDown.IsPressed && !_oldButtonState.DPadDown.IsPressed);
            Button dpadLeft = new Button(_currentButtonState.DPadLeft.IsPressed, _currentButtonState.DPadLeft.IsPressed && !_oldButtonState.DPadLeft.IsPressed);
            Button dpadRight = new Button(_currentButtonState.DPadRight.IsPressed, _currentButtonState.DPadRight.IsPressed && !_oldButtonState.DPadRight.IsPressed);
            Button lStick = new Button(_currentLStickState.Button.IsPressed, _currentLStickState.Button.IsPressed && !_oldLStickState.Button.IsPressed);
            Button rStick = new Button(_currentRStickState.Button.IsPressed, _currentRStickState.Button.IsPressed && !_oldRStickState.Button.IsPressed);

            Buttons = new ButtonState(a, b, x, y, lb, rb, back, start, dpadLeft, dpadRight, dpadUp, dpadDown);
            LeftStick = new ControlStick(_currentLStickState.State, lStick);
            RightStick = new ControlStick(_currentRStickState.State, rStick);

            if (_aPressed != null && Buttons.A.WasPressed) _aPressed();
            if (_bPressed != null && Buttons.B.WasPressed) _bPressed();
            if (_xPressed != null && Buttons.X.WasPressed) _xPressed();
            if (_yPressed != null && Buttons.Y.WasPressed) _yPressed();
            if (_lbPressed != null && Buttons.LB.WasPressed) _lbPressed();
            if (_rbPressed != null && Buttons.RB.WasPressed) _rbPressed();
            if (_backPressed != null && Buttons.Back.WasPressed) _backPressed();
            if (_startPressed != null && Buttons.Start.WasPressed) _startPressed();
            if (_dpadUpPressed != null && Buttons.DPadUp.WasPressed) _dpadUpPressed();
            if (_dpadDownPressed != null && Buttons.DPadDown.WasPressed) _dpadDownPressed();
            if (_dpadLeftPressed != null && Buttons.DPadLeft.WasPressed) _dpadLeftPressed();
            if (_dpadRightPressed != null && Buttons.DPadRight.WasPressed) _dpadRightPressed();
            if (_leftStickPressed != null && LeftStick.Button.WasPressed) _leftStickPressed();
            if (_rightStickPressed != null && RightStick.Button.WasPressed) _rightStickPressed();
        }

        private void ReadDeviceBytes(byte[] bytes)
        {
            if (_device == null) throw new NullReferenceException("_device cannot be null");
            if (!_device._connected) return;

            //Sticks are contained in bytes 1-8
            ushort leftStickX = BitConverter.ToUInt16(bytes, 1);
            ushort leftStickY = BitConverter.ToUInt16(bytes, 3);
            ushort rightStickX = BitConverter.ToUInt16(bytes, 5);
            ushort rightStickY = BitConverter.ToUInt16(bytes, 7);

            //These two bytes are definitely trigger info but I'm not sure how to get anything meaningful from them
            ushort triggers = BitConverter.ToUInt16(bytes, 9);

            //bytes[12] contains these in the first two bits
            bool lStickButton = (bytes[12] & 1) != 0;
            bool rStickButton = (bytes[12] & 2) != 0;

            //Despite this, it's not fully a bitmask so we have to clear those bits to get dpad info
            bytes[12] = (byte)(bytes[12] & ~3);

            //Create control stick objects
            _asyncLStickState = new ControlStick(leftStickX, leftStickY, new Button(lStickButton));
            _asyncRStickState = new ControlStick(rightStickX, rightStickY, new Button(rStickButton));

            bool dpadUp = false;
            bool dpadDown = false;
            bool dpadLeft = false;
            bool dpadRight = false;

            //bytes[12] appears to go clockwise around the dpad in increments of 4
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

            //bytes[11] contains a bitmask of all the buttons
            bool a = (bytes[11] & 1) != 0;
            bool b = (bytes[11] & 2) != 0;
            bool x = (bytes[11] & 4) != 0;
            bool y = (bytes[11] & 8) != 0;
            bool lb = (bytes[11] & 16) != 0;
            bool rb = (bytes[11] & 32) != 0;
            bool back = (bytes[11] & 64) != 0;
            bool start = (bytes[11] & 128) != 0;

            _asyncButtonState = new ButtonState(a, b, x, y, lb, rb, back, start, dpadLeft, dpadRight, dpadUp, dpadDown);

            //Can't forget to begin the next read
            _device.ReadAsync(ReadDeviceBytes);
        }

        //More boilerplate
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

        private event ButtonPressed _leftStickPressed;
        public event ButtonPressed LeftStickPressed
        {
            add => _leftStickPressed += value;
            remove => _leftStickPressed -= value;
        }

        private event ButtonPressed _rightStickPressed;
        public event ButtonPressed RightStickPressed
        {
            add => _rightStickPressed += value;
            remove => _rightStickPressed -= value;
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

        public enum ButtonType
        {
            A = 1,
            B = 2,
            X = 4,
            Y = 8,
            LB = 16,
            RB = 32,
            Back = 64,
            Start = 128,
            DPadLeft = 256,
            DPadRight = 512,
            DPadUp = 1024,
            DPadDown = 2048,
            LStick = 4096,
            RStick = 8192
        }
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

    public struct ControlStick
    {
        internal ControlStick(ushort x, ushort y, Button buttonState)
        {
            //Convert left and right to floats in the range 0-1
            float lFloat = x / (float)ushort.MaxValue;
            float rFloat = y / (float)ushort.MaxValue;

            //Scale these floats to -1 - 1
            State = new Vector2(lFloat * 2 - 1, rFloat * 2 - 1);

            Button = buttonState;
        }

        internal ControlStick(Vector2 pos, Button buttonState)
        {
            State = pos;
            Button = buttonState;
        }

        public Vector2 State { get; private set; }
        public Button Button { get; private set; }
    }
}
