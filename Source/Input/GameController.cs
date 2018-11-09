using System;

namespace SharpSlugsEngine.Input
{
    /// <summary>
    /// Generic backend for all controller classes
    /// </summary>
    public abstract class GameController
    {
        private InputDevice _device;

        public ControllerType Type { get; private set; }
        internal string Path => _device.DevicePath;

        public bool IsConnected => _device._connected;

        internal GameController(InputDevice device)
        {
            //Everything is gonna break anyway if device is null, might as well throw an obvious exception
            _device = device ?? throw new ArgumentNullException();

            Type = ControllerType.Unknown;

            switch (device.VendorID)
            {
                case DeviceManager.VID_MICROSOFT:
                    switch (device.ProductID)
                    {
                        case DeviceManager.PID_XBOX:
                            Type = ControllerType.Xbox;
                            break;
                        case DeviceManager.PID_XBOX_360:
                            Type = ControllerType.Xbox360;
                            break;
                        case DeviceManager.PID_XBOX_ONE:
                            Type = ControllerType.XboxOne;
                            break;
                        case DeviceManager.PID_XBOX_ONE_S:
                            Type = ControllerType.XboxOneS;
                            break;
                    }
                    break;
                case DeviceManager.VID_SONY:
                    switch (device.ProductID)
                    {
                        case DeviceManager.PID_PLAYSTATION_3:
                            Type = ControllerType.Playstation3;
                            break;
                        case DeviceManager.PID_PLAYSTATION_4:
                            Type = ControllerType.Playstation4;
                            break;
                    }
                    break;
            }

            //Just hooking this to be able to pass the event up further. Could implement better
            device.OnDisconnect += OnDisconnect;

            //Begin reading data from the controller
            device.ReadAsync(ReadDeviceBytes);
        }

        internal void Update()
        {
            if (_device == null) throw new NullReferenceException("_device cannot be null");

            //Try to reconnect if applicable
            if (!_device._connected)
            {
                if (_device.TryReconnect())
                {
                    _device.ReadAsync(ReadDeviceBytes);
                    OnConnect();
                }
                return;
            }

            UpdateController();
        }

        private void ReadDeviceBytes(byte[] bytes)
        {
            if (_device == null) throw new NullReferenceException("_device cannot be null");
            if (!_device._connected) return;

            ProcessDeviceBytes(bytes);

            //Can't forget to begin the next read
            _device.ReadAsync(ReadDeviceBytes);
        }

        protected abstract void OnConnect();
        protected abstract void OnDisconnect();

        protected abstract void UpdateController();

        protected abstract void ProcessDeviceBytes(byte[] bytes);
    }

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
}
