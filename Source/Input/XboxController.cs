using System;
using System.Diagnostics.CodeAnalysis;

namespace SharpSlugsEngine.Input
{
    /// <summary>
    /// Handles input for all types of Xbox controllers
    /// </summary>
    public class XboxController : GameController
    {
        private static readonly ButtonType[] AllButtons = (ButtonType[])Enum.GetValues(typeof(ButtonType));
        
        private ButtonState asyncButtonState;
        private ButtonState oldButtonState;
        private ButtonState currentButtonState;

        private ControlStick asyncLStickState;
        private ControlStick oldLStickState;
        private ControlStick currentLStickState;

        private ControlStick asyncRStickState;
        private ControlStick oldRStickState;
        private ControlStick currentRStickState;

        /// <summary>
        /// Initializes a new instance of the <see cref="XboxController"/> class that pulls input from the given <see cref="InputDevice"/>
        /// </summary>
        /// <param name="device">The <see cref="InputDevice"/> to pull input from</param>
        internal XboxController(InputDevice device) : base(device)
        {
            // Make sure this is actually an xbox controller
            if (Type != ControllerType.Xbox && Type != ControllerType.Xbox360 && Type != ControllerType.XboxOne && Type != ControllerType.XboxOneS)
            {
                throw new ArgumentException("Device is not an Xbox controller");
            }
        }

        /// <summary>
        /// Used for all button events
        /// </summary>
        public delegate void ButtonPressed();

        /// <summary>
        /// Used for controller disconnect and reconnect events
        /// </summary>
        public delegate void ControllerConnection();

        /// <summary>
        /// Called when the A button is pressed
        /// </summary>
        public event ButtonPressed APressed
        {
            add => APressedInternal += value;
            remove => APressedInternal -= value;
        }

        /// <summary>
        /// Called when the B button is pressed
        /// </summary>
        public event ButtonPressed BPressed
        {
            add => BPressedInternal += value;
            remove => BPressedInternal -= value;
        }

        /// <summary>
        /// Called when the X button is pressed
        /// </summary>
        public event ButtonPressed XPressed
        {
            add => XPressedInternal += value;
            remove => XPressedInternal -= value;
        }

        /// <summary>
        /// Called when the Y button is pressed
        /// </summary>
        public event ButtonPressed YPressed
        {
            add => YPressedInternal += value;
            remove => YPressedInternal -= value;
        }

        /// <summary>
        /// Called when the left bumber is pressed
        /// </summary>
        public event ButtonPressed LBPressed
        {
            add => LBPressedInternal += value;
            remove => LBPressedInternal -= value;
        }

        /// <summary>
        /// Called when the right bumper is pressed
        /// </summary>
        public event ButtonPressed RBPressed
        {
            add => RBPressedInternal += value;
            remove => RBPressedInternal -= value;
        }

        /// <summary>
        /// Called when the back button is pressed
        /// </summary>
        public event ButtonPressed BackPressed
        {
            add => BackPressedInternal += value;
            remove => BackPressedInternal -= value;
        }

        /// <summary>
        /// Called when the start button is pressed
        /// </summary>
        public event ButtonPressed StartPressed
        {
            add => StartPressedInternal += value;
            remove => StartPressedInternal -= value;
        }

        /// <summary>
        /// Called when dpad up is pressed
        /// </summary>
        public event ButtonPressed DPadUpPressed
        {
            add => DPadUpPressedInternal += value;
            remove => DPadUpPressedInternal -= value;
        }

        /// <summary>
        /// Called when dpad down is pressed
        /// </summary>
        public event ButtonPressed DPadDownPressed
        {
            add => DPadDownPressedInternal += value;
            remove => DPadDownPressedInternal -= value;
        }

        /// <summary>
        /// Called when dpad left is pressed
        /// </summary>
        public event ButtonPressed DPadLeftPressed
        {
            add => DPadLeftPressedInternal += value;
            remove => DPadLeftPressedInternal -= value;
        }

        /// <summary>
        /// Called when dpad right is pressed
        /// </summary>
        public event ButtonPressed DPadRightPressed
        {
            add => DPadRightPressedInternal += value;
            remove => DPadRightPressedInternal -= value;
        }

        /// <summary>
        /// Called when the left stick is pressed
        /// </summary>
        public event ButtonPressed LeftStickPressed
        {
            add => LeftStickPressedInternal += value;
            remove => LeftStickPressedInternal -= value;
        }

        /// <summary>
        /// Called when the right stick is pressed
        /// </summary>
        public event ButtonPressed RightStickPressed
        {
            add => RightStickPressedInternal += value;
            remove => RightStickPressedInternal -= value;
        }
        
        /// <summary>
        /// Called when the <see cref="XboxController"/> disconnects from the computer
        /// </summary>
        public event ControllerConnection Disconnected
        {
            add => DisconnectedInternal += value;
            remove => DisconnectedInternal -= value;
        }

        /// <summary>
        /// Called when the <see cref="XboxController"/> reconnects to the computer
        /// </summary>
        public event ControllerConnection Connected
        {
            add => ConnectedInternal += value;
            remove => ConnectedInternal -= value;
        }

        /// <summary>
        /// Private backing field for <see cref="APressed"/>
        /// </summary>
        private event ButtonPressed APressedInternal;

        /// <summary>
        /// Private backing field for <see cref="BPressed"/>
        /// </summary>
        private event ButtonPressed BPressedInternal;

        /// <summary>
        /// Private backing field for <see cref="XPressed"/>
        /// </summary>
        private event ButtonPressed XPressedInternal;

        /// <summary>
        /// Private backing field for <see cref="YPressed"/>
        /// </summary>
        private event ButtonPressed YPressedInternal;

        /// <summary>
        /// Private backing field for <see cref="LBPressed"/>
        /// </summary>
        private event ButtonPressed LBPressedInternal;

        /// <summary>
        /// Private backing field for <see cref="RBPressed"/>
        /// </summary>
        private event ButtonPressed RBPressedInternal;

        /// <summary>
        /// Private backing field for <see cref="BackPressed"/>
        /// </summary>
        private event ButtonPressed BackPressedInternal;

        /// <summary>
        /// Private backing field for <see cref="StartPressed"/>
        /// </summary>
        private event ButtonPressed StartPressedInternal;

        /// <summary>
        /// Private backing field for <see cref="DPadUpPressed"/>
        /// </summary>
        private event ButtonPressed DPadUpPressedInternal;

        /// <summary>
        /// Private backing field for <see cref="DPadDownPressed"/>
        /// </summary>
        private event ButtonPressed DPadDownPressedInternal;

        /// <summary>
        /// Private backing field for <see cref="DPadLeftPressed"/>
        /// </summary>
        private event ButtonPressed DPadLeftPressedInternal;

        /// <summary>
        /// Private backing field for <see cref="DPadRightPressed"/>
        /// </summary>
        private event ButtonPressed DPadRightPressedInternal;

        /// <summary>
        /// Private backing field for <see cref="LeftStickPressed"/>
        /// </summary>
        private event ButtonPressed LeftStickPressedInternal;

        /// <summary>
        /// Private backing field for <see cref="RightStickPressed"/>
        /// </summary>
        private event ButtonPressed RightStickPressedInternal;

        /// <summary>
        /// Private backing field for <see cref="Disconnected"/>
        /// </summary>
        private event ControllerConnection DisconnectedInternal;

        /// <summary>
        /// Private backing field for <see cref="Connected"/>
        /// </summary>
        private event ControllerConnection ConnectedInternal;

        /// <summary>
        /// Bitmask enum for <see cref="XboxController"/> buttons
        /// </summary>
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

        /// <summary>
        /// Gets the current state of the buttons on this <see cref="XboxController"/>
        /// </summary>
        public ButtonState Buttons { get; private set; }

        /// <summary>
        /// Gets the current state of the left stick on this <see cref="XboxController"/>
        /// </summary>
        public ControlStick LeftStick { get; private set; }

        /// <summary>
        /// Gets the current state of the right stick on this <see cref="XboxController"/>
        /// </summary>
        public ControlStick RightStick { get; private set; }
        
        /// <summary>
        /// Checks if any button from the given bitmask is pressed
        /// </summary>
        /// <param name="buttons">A bitmask of <see cref="ButtonType"/>s to check</param>
        /// <returns>A bool indicating if any of the given buttons are pressed</returns>
        public bool AnyIsPressed(ButtonType buttons)
        {
            foreach (ButtonType type in AllButtons)
            {
                if ((buttons & type) == type)
                {
                    if (GetButtonState(type).IsPressed)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if any button from the given bitmask was pressed
        /// </summary>
        /// <param name="buttons">A bitmask of <see cref="ButtonType"/>s to check</param>
        /// <returns>A bool indicating if any of the given buttons were pressed</returns>
        public bool AnyWasPressed(ButtonType buttons)
        {
            foreach (ButtonType type in AllButtons)
            {
                if ((buttons & type) == type)
                {
                    if (GetButtonState(type).WasPressed)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the state of the given button
        /// </summary>
        /// <param name="button">The button to check</param>
        /// <returns>A <see cref="Button"/> instance for the given button</returns>
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

        /// <summary>
        /// Called when the child <see cref="InputDevice"/> is disconnected from the computer
        /// </summary>
        protected override void OnDisconnect()
        {
            DisconnectedInternal?.Invoke();
        }

        /// <summary>
        /// Called when the child <see cref="InputDevice"/> is reconnected to the computer
        /// </summary>
        protected override void OnConnect()
        {
            // Pass the event upward
            ConnectedInternal?.Invoke();
        }

        /// <summary>
        /// Called every frame to update the button/stick states
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "False positive")]
        [SuppressMessage("Microsoft.StyleCop.CSharp.LayoutRules", "SA1503:CurlyBracketsMustNotBeOmitted", Justification = "Large amount of boilerplate code, would be much worse with 3 more lines each.")]
        protected override void UpdateController()
        {
            // Update the current/old controller state from the asynchronous reading
            oldButtonState = currentButtonState;
            currentButtonState = asyncButtonState;

            oldLStickState = currentLStickState;
            currentLStickState = asyncLStickState;

            oldRStickState = currentRStickState;
            currentRStickState = asyncRStickState;

            // Just a ton of boilerplate here
            Button a = new Button(currentButtonState.A.IsPressed, currentButtonState.A.IsPressed && !oldButtonState.A.IsPressed);
            Button b = new Button(currentButtonState.B.IsPressed, currentButtonState.B.IsPressed && !oldButtonState.B.IsPressed);
            Button x = new Button(currentButtonState.X.IsPressed, currentButtonState.X.IsPressed && !oldButtonState.X.IsPressed);
            Button y = new Button(currentButtonState.Y.IsPressed, currentButtonState.Y.IsPressed && !oldButtonState.Y.IsPressed);
            Button lb = new Button(currentButtonState.LB.IsPressed, currentButtonState.LB.IsPressed && !oldButtonState.LB.IsPressed);
            Button rb = new Button(currentButtonState.RB.IsPressed, currentButtonState.RB.IsPressed && !oldButtonState.RB.IsPressed);
            Button back = new Button(currentButtonState.Back.IsPressed, currentButtonState.Back.IsPressed && !oldButtonState.Back.IsPressed);
            Button start = new Button(currentButtonState.Start.IsPressed, currentButtonState.Start.IsPressed && !oldButtonState.Start.IsPressed);
            Button dpadUp = new Button(currentButtonState.DPadUp.IsPressed, currentButtonState.DPadUp.IsPressed && !oldButtonState.DPadUp.IsPressed);
            Button dpadDown = new Button(currentButtonState.DPadDown.IsPressed, currentButtonState.DPadDown.IsPressed && !oldButtonState.DPadDown.IsPressed);
            Button dpadLeft = new Button(currentButtonState.DPadLeft.IsPressed, currentButtonState.DPadLeft.IsPressed && !oldButtonState.DPadLeft.IsPressed);
            Button dpadRight = new Button(currentButtonState.DPadRight.IsPressed, currentButtonState.DPadRight.IsPressed && !oldButtonState.DPadRight.IsPressed);
            Button lStick = new Button(currentLStickState.Button.IsPressed, currentLStickState.Button.IsPressed && !oldLStickState.Button.IsPressed);
            Button rStick = new Button(currentRStickState.Button.IsPressed, currentRStickState.Button.IsPressed && !oldRStickState.Button.IsPressed);

            Buttons = new ButtonState(a, b, x, y, lb, rb, back, start, dpadLeft, dpadRight, dpadUp, dpadDown);
            LeftStick = new ControlStick(currentLStickState.State, lStick);
            RightStick = new ControlStick(currentRStickState.State, rStick);

            if (APressedInternal != null && Buttons.A.WasPressed) APressedInternal();
            if (BPressedInternal != null && Buttons.B.WasPressed) BPressedInternal();
            if (XPressedInternal != null && Buttons.X.WasPressed) XPressedInternal();
            if (YPressedInternal != null && Buttons.Y.WasPressed) YPressedInternal();
            if (LBPressedInternal != null && Buttons.LB.WasPressed) LBPressedInternal();
            if (RBPressedInternal != null && Buttons.RB.WasPressed) RBPressedInternal();
            if (BackPressedInternal != null && Buttons.Back.WasPressed) BackPressedInternal();
            if (StartPressedInternal != null && Buttons.Start.WasPressed) StartPressedInternal();
            if (DPadUpPressedInternal != null && Buttons.DPadUp.WasPressed) DPadUpPressedInternal();
            if (DPadDownPressedInternal != null && Buttons.DPadDown.WasPressed) DPadDownPressedInternal();
            if (DPadLeftPressedInternal != null && Buttons.DPadLeft.WasPressed) DPadLeftPressedInternal();
            if (DPadRightPressedInternal != null && Buttons.DPadRight.WasPressed) DPadRightPressedInternal();
            if (LeftStickPressedInternal != null && LeftStick.Button.WasPressed) LeftStickPressedInternal();
            if (RightStickPressedInternal != null && RightStick.Button.WasPressed) RightStickPressedInternal();
        }

        /// <summary>
        /// Converts the given <see cref="byte"/>[] into inputs
        /// </summary>
        /// <param name="bytes">The bytes received from the child <see cref="InputDevice"/></param>
        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "False positive")]
        protected override void ProcessDeviceBytes(byte[] bytes)
        {
            // Sticks are contained in bytes 1-8
            ushort leftStickX = BitConverter.ToUInt16(bytes, 1);
            ushort leftStickY = BitConverter.ToUInt16(bytes, 3);
            ushort rightStickX = BitConverter.ToUInt16(bytes, 5);
            ushort rightStickY = BitConverter.ToUInt16(bytes, 7);

            // These two bytes are definitely trigger info but I'm not sure how to get anything meaningful from them
            ushort triggers = BitConverter.ToUInt16(bytes, 9);

            // bytes[12] contains these in the first two bits
            bool lStickButton = (bytes[12] & 1) != 0;
            bool rStickButton = (bytes[12] & 2) != 0;

            // Despite this, it's not fully a bitmask so we have to clear those bits to get dpad info
            bytes[12] = (byte)(bytes[12] & ~3);

            // Create control stick objects
            asyncLStickState = new ControlStick(leftStickX, leftStickY, new Button(lStickButton));
            asyncRStickState = new ControlStick(rightStickX, rightStickY, new Button(rStickButton));

            bool dpadUp = false;
            bool dpadDown = false;
            bool dpadLeft = false;
            bool dpadRight = false;

            // bytes[12] appears to go clockwise around the dpad in increments of 4
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

            // bytes[11] contains a bitmask of all the buttons
            bool a = (bytes[11] & 1) != 0;
            bool b = (bytes[11] & 2) != 0;
            bool x = (bytes[11] & 4) != 0;
            bool y = (bytes[11] & 8) != 0;
            bool lb = (bytes[11] & 16) != 0;
            bool rb = (bytes[11] & 32) != 0;
            bool back = (bytes[11] & 64) != 0;
            bool start = (bytes[11] & 128) != 0;

            asyncButtonState = new ButtonState(a, b, x, y, lb, rb, back, start, dpadLeft, dpadRight, dpadUp, dpadDown);
        }

        /// <summary>
        /// Contains all information for an <see cref="XboxController"/>s buttons
        /// </summary>
        public struct ButtonState
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ButtonState"/> struct with the given values
            /// </summary>
            /// <param name="a">Whether or not the A button is pressed</param>
            /// <param name="b">Whether or not the B button is pressed</param>
            /// <param name="x">Whether or not the X button is pressed</param>
            /// <param name="y">Whether or not the Y button is pressed</param>
            /// <param name="lb">Whether or not the left bumper is pressed</param>
            /// <param name="rb">Whether or not the right bumper is pressed</param>
            /// <param name="back">Whether or not the back button is pressed</param>
            /// <param name="start">Whether or not the start button is pressed</param>
            /// <param name="dpadLeft">Whether or not dpad left is pressed</param>
            /// <param name="dpadRight">Whether or not dpad right is pressed</param>
            /// <param name="dpadUp">Whether or not dpad up is pressed</param>
            /// <param name="dpadDown">Whether or not dpad down is pressed</param>
            internal ButtonState(bool a, bool b, bool x, bool y, bool lb, bool rb, bool back, bool start, bool dpadLeft, bool dpadRight, bool dpadUp, bool dpadDown)
            {
                A = new Button(a);
                B = new Button(b);
                X = new Button(x);
                Y = new Button(y);
                LB = new Button(lb);
                RB = new Button(rb);
                Back = new Button(back);
                Start = new Button(start);
                DPadLeft = new Button(dpadLeft);
                DPadRight = new Button(dpadRight);
                DPadUp = new Button(dpadUp);
                DPadDown = new Button(dpadDown);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ButtonState"/> struct with the given values
            /// </summary>
            /// <param name="a"><see cref="Button"/> containing information about A</param>
            /// <param name="b"><see cref="Button"/> containing information about B</param>
            /// <param name="x"><see cref="Button"/> containing information about X</param>
            /// <param name="y"><see cref="Button"/> containing information about Y</param>
            /// <param name="lb"><see cref="Button"/> containing information about LB</param>
            /// <param name="rb"><see cref="Button"/> containing information about RB</param>
            /// <param name="back"><see cref="Button"/> containing information about back</param>
            /// <param name="start"><see cref="Button"/> containing information about start</param>
            /// <param name="dpadLeft"><see cref="Button"/> containing information about dpad left</param>
            /// <param name="dpadRight"><see cref="Button"/> containing information about dpad right</param>
            /// <param name="dpadUp"><see cref="Button"/> containing information about dpad up</param>
            /// <param name="dpadDown"><see cref="Button"/> containing information about dpad down</param>
            internal ButtonState(Button a, Button b, Button x, Button y, Button lb, Button rb, Button back, Button start, Button dpadLeft, Button dpadRight, Button dpadUp, Button dpadDown)
            {
                A = a;
                B = b;
                X = x;
                Y = y;
                LB = lb;
                RB = rb;
                Back = back;
                Start = start;
                DPadLeft = dpadLeft;
                DPadRight = dpadRight;
                DPadUp = dpadUp;
                DPadDown = dpadDown;
            }

            /// <summary>
            /// Gets the state of the A button
            /// </summary>
            public Button A { get; private set; }

            /// <summary>
            /// Gets the state of the B button
            /// </summary>
            public Button B { get; private set; }

            /// <summary>
            /// Gets the state of the X button
            /// </summary>
            public Button X { get; private set; }

            /// <summary>
            /// Gets the state of the Y button
            /// </summary>
            public Button Y { get; private set; }

            /// <summary>
            /// Gets the state of the left bumper
            /// </summary>
            public Button LB { get; private set; }

            /// <summary>
            /// Gets the state of the right bumper
            /// </summary>
            public Button RB { get; private set; }

            /// <summary>
            /// Gets the state of the back button
            /// </summary>
            public Button Back { get; private set; }

            /// <summary>
            /// Gets the state of the start button
            /// </summary>
            public Button Start { get; private set; }

            /// <summary>
            /// Gets the state of the dpad left button
            /// </summary>
            public Button DPadLeft { get; private set; }

            /// <summary>
            /// Gets the state of the dpad right button
            /// </summary>
            public Button DPadRight { get; private set; }

            /// <summary>
            /// Gets the state of the dpad up button
            /// </summary>
            public Button DPadUp { get; private set; }

            /// <summary>
            /// Gets the state of the dpad down button
            /// </summary>
            public Button DPadDown { get; private set; }
        }
    }
}
