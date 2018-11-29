using System;

namespace SharpSlugsEngine.Input
{
    /// <summary>
    /// Indicates the type of a <see cref="GameController"/>
    /// </summary>
    public enum ControllerType
    {
        Unknown,
        Xbox,
        Xbox360,
        XboxOne,
        XboxOneS,
        Playstation3,
        Playstation4
    }

    /// <summary>
    /// Stores the state of a button on a <see cref="GameController"/>
    /// </summary>
    public struct Button
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> struct with the given values
        /// </summary>
        /// <param name="pressed">Whether or not the <see cref="Button"/> is currently pressed</param>
        /// <param name="wasPressed">Whether or not the <see cref="Button"/> was pressed on the last frame</param>
        internal Button(bool pressed, bool wasPressed = false)
        {
            IsPressed = pressed;
            WasPressed = wasPressed;
        }

        /// <summary>
        /// Gets a value indicating whether or not the <see cref="Button"/> is pressed
        /// </summary>
        public bool IsPressed { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not the <see cref="Button"/> was pressed
        /// </summary>
        public bool WasPressed { get; private set; }
    }

    /// <summary>
    /// Stores the state of a stick on a <see cref="GameController"/>
    /// </summary>
    public struct ControlStick
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControlStick"/> struct with the given values
        /// </summary>
        /// <param name="x">X value in the range 0-65535</param>
        /// <param name="y">Y value in the range 0-65535</param>
        /// <param name="buttonState">The current state of the stick <see cref="Button"/></param>
        internal ControlStick(ushort x, ushort y, Button buttonState)
        {
            // Convert left and right to floats in the range 0-1
            float leftFloat = x / (float)ushort.MaxValue;
            float rightFloat = y / (float)ushort.MaxValue;

            // Scale these floats to -1 - 1
            State = new Vector2((leftFloat * 2) - 1, (rightFloat * 2) - 1);

            Button = buttonState;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlStick"/> struct with the given values
        /// </summary>
        /// <param name="pos">Current position of the <see cref="ControlStick"/></param>
        /// <param name="buttonState">The current state of the stick <see cref="Button"/></param>
        internal ControlStick(Vector2 pos, Button buttonState)
        {
            State = pos;
            Button = buttonState;
        }

        /// <summary>
        /// Gets the position of the <see cref="ControlStick"/>
        /// </summary>
        public Vector2 State { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ControlStick"/> is pressed
        /// </summary>
        public Button Button { get; private set; }
    }

    /// <summary>
    /// Generic backend for all controller classes
    /// </summary>
    public abstract class GameController
    {
        private InputDevice device;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameController"/> class with the specified <see cref="InputDevice"/>
        /// </summary>
        /// <param name="device">The <see cref="InputDevice"/> to pull inputs from</param>
        internal GameController(InputDevice device)
        {
            // Everything is gonna break anyway if device is null, might as well throw an obvious exception
            this.device = device ?? throw new ArgumentNullException();

            Type = ControllerType.Unknown;

            switch (device.VendorID)
            {
                case DeviceManager.VIDMicrosoft:
                    switch (device.ProductID)
                    {
                        case DeviceManager.PIDXbox:
                            Type = ControllerType.Xbox;
                            break;
                        case DeviceManager.PIDXbox360:
                            Type = ControllerType.Xbox360;
                            break;
                        case DeviceManager.PIDXboxOne:
                            Type = ControllerType.XboxOne;
                            break;
                        case DeviceManager.PIDXboxOneS:
                        case DeviceManager.PIDXboxOneSAlt:
                            Type = ControllerType.XboxOneS;
                            break;
                    }

                    break;
                case DeviceManager.VIDSony:
                    switch (device.ProductID)
                    {
                        case DeviceManager.PIDPlaystation3:
                            Type = ControllerType.Playstation3;
                            break;
                        case DeviceManager.PIDPlaystation4:
                            Type = ControllerType.Playstation4;
                            break;
                    }

                    break;
            }

            // Just hooking this to be able to pass the event up further. Could implement better
            device.OnDisconnect += OnDisconnect;

            // Begin reading data from the controller
            device.ReadAsync(ReadDeviceBytes);
        }

        /// <summary>
        /// Gets the <see cref="ControllerType"/> of this <see cref="GameController"/>
        /// </summary>
        public ControllerType Type { get; private set; }

        /// <summary>
        /// Gets a value indicating whether <see cref="device"/> is connected
        /// </summary>
        public bool IsConnected => device.Connected;

        /// <summary>
        /// Gets the device path of <see cref="device"/>
        /// </summary>
        internal string Path => device.DevicePath;

        /// <summary>
        /// Called every frame to update the state of this <see cref="GameController"/>
        /// </summary>
        internal void Update()
        {
            if (device == null)
            {
                throw new NullReferenceException("_device cannot be null");
            }

            // Try to reconnect if applicable
            if (!device.Connected)
            {
                if (device.TryReconnect())
                {
                    device.ReadAsync(ReadDeviceBytes);
                    OnConnect();
                }

                return;
            }

            UpdateController();
        }

        /// <summary>
        /// Called when <see cref="device"/> connects to the computer
        /// </summary>
        protected abstract void OnConnect();

        /// <summary>
        /// Called when <see cref="device"/> disconnects from the computer
        /// </summary>
        protected abstract void OnDisconnect();

        /// <summary>
        /// Called every frame to update the <see cref="GameController"/>
        /// </summary>
        protected abstract void UpdateController();

        /// <summary>
        /// Called when new <see cref="byte"/>s are received from <see cref="device"/>
        /// </summary>
        /// <param name="bytes">The received <see cref="byte"/>s</param>
        protected abstract void ProcessDeviceBytes(byte[] bytes);

        /// <summary>
        /// Called from <see cref="device"/> when new <see cref="byte"/>s are read
        /// </summary>
        /// <param name="bytes">The read <see cref="byte"/>s</param>
        private void ReadDeviceBytes(byte[] bytes)
        {
            if (device == null)
            {
                throw new NullReferenceException("_device cannot be null");
            }

            if (!device.Connected)
            {
                return;
            }

            ProcessDeviceBytes(bytes);

            // Can't forget to begin the next read
            device.ReadAsync(ReadDeviceBytes);
        }
    }
}
