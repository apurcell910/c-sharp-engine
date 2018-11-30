using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpSlugsEngine.Input
{
    /// <summary>
    /// Stores information about a single HID controller
    /// </summary>
    internal struct DeviceDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceDescriptor"/> struct with a given path and description
        /// </summary>
        /// <param name="p">The path of the HID controller</param>
        /// <param name="d">The description of the HID controller</param>
        public DeviceDescriptor(string p, string d)
        {
            Path = p;
            Description = d;

            VendorID = 0;
            ProductID = 0;
        }

        /// <summary>
        /// Gets the device path for this HID controller
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Gets the device description for this HID controller
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the vendor ID for this HID controller
        /// </summary>
        public int VendorID { get; private set; }

        /// <summary>
        /// Gets the product ID for this HID controller
        /// </summary>
        public int ProductID { get; private set; }

        /// <summary>
        /// Populates <see cref="VendorID"/> and <see cref="ProductID"/> from a given <see cref="WindowsNative.HIDD_ATTRIBUTES"/>
        /// </summary>
        /// <param name="attr">The struct containing the product and vendor IDs to set</param>
        public void SetIDSFromAttributes(WindowsNative.HIDD_ATTRIBUTES attr)
        {
            VendorID = attr.VendorID;
            ProductID = attr.ProductID;
        }
    }

    /// <summary>
    /// Contains all recognized HID compliant game controllers
    /// </summary>
    public class DeviceManager
    {
        // Magic numbers assigned by manufacturers https://www.the-sz.com/products/usbid/index.php
        internal const int VIDMicrosoft = 0x45E;
        internal const int VIDSony = 0x54C;

        internal const int PIDXbox = 0x202;
        internal const int PIDXbox360 = 0x28E;
        internal const int PIDXboxOne = 0x2D1;
        internal const int PIDXboxOneS = 0x2EA;
        internal const int PIDXboxOneSAlt = 0x2FF;

        internal const int PIDPlaystation3 = 0x268;
        internal const int PIDPlaystation4 = 0x5C4;

        private GameController[] controllersInternal;
        private XboxController[] xboxControllersInternal;

        private Game game;
        private TimeSpan lastCheck = TimeSpan.FromTicks(0);

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceManager"/> class with the given parent <see cref="Game"/>
        /// </summary>
        /// <param name="game">The parent <see cref="Game"/> of the new <see cref="DeviceManager"/></param>
        internal DeviceManager(Game game)
        {
            this.game = game;
            xboxControllersInternal = new XboxController[0];
            controllersInternal = new GameController[0];
        }

        /// <summary>
        /// Used when a new <see cref="GameController"/> is connected to the system
        /// </summary>
        /// <param name="controller">The new <see cref="GameController"/></param>
        public delegate void NewController(GameController controller);

        /// <summary>
        /// Called when a new <see cref="GameController"/> is connected to the system
        /// </summary>
        public event NewController ControllerAdded
        {
            add => ControllerAddedInternal += value;
            remove => ControllerAddedInternal -= value;
        }

        /// <summary>
        /// Backing field for <see cref="ControllerAdded"/>
        /// </summary>
        private event NewController ControllerAddedInternal;

        /// <summary>
        /// Gets all currently valid <see cref="GameController"/>s
        /// </summary>
        public GameController[] Controllers => (GameController[])controllersInternal.Clone();

        /// <summary>
        /// Gets all currently valid <see cref="XboxController"/>s
        /// </summary>
        public XboxController[] XboxControllers => (XboxController[])xboxControllersInternal.Clone();

        /// <summary>
        /// Gets a list of connected <see cref="DeviceDescriptor"/>s. Note: This is the actual list, modify with caution
        /// </summary>
        internal List<DeviceDescriptor> Devices { get; private set; }

        /// <summary>
        /// Searches for new <see cref="GameController"/> connections, as well as updating existing ones
        /// </summary>
        /// <param name="time">Struct containing the elapsed time since the last <see cref="Update(GameTime)"/></param>
        internal void Update(GameTime time)
        {
            // Check for controllers infrequently, takes too much CPU power
            if ((time.totalTime - lastCheck).TotalSeconds >= 1f)
            {
                Devices = GetDevices();
                List<GameController> gameControllers = new List<GameController>();
                gameControllers.AddRange(controllersInternal);

                foreach (DeviceDescriptor desc in Devices)
                {
                    // Create an InputDevice in order to get the vendor and product ID
                    InputDevice device = new InputDevice(desc, this);

                    if (device.ReadSize == 0)
                    {
                        device.Dispose();
                        continue;
                    }

                    // Attempt to match the device to a known controller type
                    GameController newController = null;
                    if (!gameControllers.Any(controller => controller.Path == device.DevicePath))
                    {
                        switch (device.VendorID)
                        {
                            case VIDMicrosoft:
                                switch (device.ProductID)
                                {
                                    case PIDXbox:
                                    case PIDXbox360:
                                    case PIDXboxOne:
                                    case PIDXboxOneS:
                                    case PIDXboxOneSAlt:
                                        newController = new XboxController(device);
                                        break;
                                }

                                break;
                            case VIDSony:
                                switch (device.ProductID)
                                {
                                    case PIDPlaystation3:
                                        break;
                                    case PIDPlaystation4:
                                        break;
                                }

                                break;
                            default:
                                if (device.VendorID == 0xC12 && device.ProductID == 0xEF6)
                                {
                                    newController = new HitboxArcadeStick(device);
                                }

                                break;
                        }
                    }

                    if (newController != null)
                    {
                        gameControllers.Add(newController);

                        // Tell the game there's a new controller if it's subscribed to the event
                        ControllerAddedInternal?.Invoke(newController);
                    }
                    else
                    {
                        // Make sure the InputDevice closes its connection
                        device.Dispose();
                    }
                }

                controllersInternal = gameControllers.ToArray();
                xboxControllersInternal = controllersInternal.Where(controller => controller is XboxController).Select(controller => controller as XboxController).ToArray();
                lastCheck = time.totalTime;
            }

            foreach (GameController controller in controllersInternal)
            {
                controller.Update();
            }
        }

        /// <summary>
        /// Removes all <see cref="DeviceDescriptor"/>s matching this <see cref="InputDevice"/>
        /// </summary>
        /// <param name="device">The <see cref="InputDevice"/> containing VID and PID to remove</param>
        internal void RemoveDevice(InputDevice device)
        {
            Devices.RemoveAll(desc => desc.Path == device.DevicePath);
        }

        /// <summary>
        /// Gets a list of all connected HID compliant devices
        /// </summary>
        /// <returns>List of <see cref="DeviceDescriptor"/>s with path and description matching a HID compliant device</returns>
        private List<DeviceDescriptor> GetDevices()
        {
            List<DeviceDescriptor> devices = new List<DeviceDescriptor>();

            // Get the device information set for connected HID devices
            Guid hidClass = WindowsNative.HidGuid;
            IntPtr infoSet = WindowsNative.SetupDiGetClassDevs(ref hidClass, null, 0, WindowsNative.DIGCF_PRESENT | WindowsNative.DIGCF_DEVICEINTERFACE);

            // -1 = INVALID_HANDLE_VALUE
            if (infoSet.ToInt64() != -1)
            {
                WindowsNative.SP_DEVINFO_DATA infoData = new WindowsNative.SP_DEVINFO_DATA();
                infoData.SetupDefaults();

                // Loop through all the elements in the info set
                int deviceIndex = 0;
                while (WindowsNative.SetupDiEnumDeviceInfo(infoSet, deviceIndex, ref infoData))
                {
                    deviceIndex++;

                    WindowsNative.SP_DEVICE_INTERFACE_DATA deviceInterfaceData = new WindowsNative.SP_DEVICE_INTERFACE_DATA();
                    deviceInterfaceData.cbSize = Marshal.SizeOf(deviceInterfaceData);

                    // Loop through all the devices
                    int deviceInterfaceIndex = 0;
                    while (WindowsNative.SetupDiEnumDeviceInterfaces(infoSet, ref infoData, ref hidClass, deviceInterfaceIndex, ref deviceInterfaceData))
                    {
                        deviceInterfaceIndex++;

                        int bufferSize = 0;

                        // If IntPtr.Size == 4, we're on 32 bit Windows
                        // This check is untested because nobody uses 32 bit Windows in 2018
                        WindowsNative.SP_DEVICE_INTERFACE_DETAIL_DATA interfaceDetail = new WindowsNative.SP_DEVICE_INTERFACE_DETAIL_DATA();
                        interfaceDetail.cbSize = IntPtr.Size == 4 ? 4 + Marshal.SystemDefaultCharSize : 8;

                        // Call this once just to get the right value for bufferSize
                        WindowsNative.SetupDiGetDeviceInterfaceDetail(infoSet, ref deviceInterfaceData, IntPtr.Zero, 0, ref bufferSize, IntPtr.Zero);

                        // Actually fill the detail struct and get device path
                        string path = WindowsNative.SetupDiGetDeviceInterfaceDetail(infoSet, ref deviceInterfaceData, ref interfaceDetail, bufferSize, ref bufferSize, IntPtr.Zero) ? interfaceDetail.DevicePath : null;
                        
                        byte[] descriptionBuffer = new byte[1024];
                        ulong propertyType = 0;
                        int requiredSize = 0;

                        WindowsNative.DEVPROPKEY key = WindowsNative.DEVPKEY_Device_BusReportedDeviceDesc;

                        // Attempt to get description using SetupDiGetDevicePropertyW
                        // This only works on Vista+
                        if (WindowsNative.SetupDiGetDevicePropertyW(infoSet, ref infoData, ref key, ref propertyType, descriptionBuffer, descriptionBuffer.Length, ref requiredSize, 0))
                        {
                            string desc = Encoding.Unicode.GetString(descriptionBuffer);
                            desc = desc.Remove(desc.IndexOf((char)0));

                            devices.Add(new DeviceDescriptor(path, desc));
                        }
                        else
                        {
                            descriptionBuffer = new byte[1024];
                            requiredSize = 0;
                            int regType = 0;

                            // If this attempt at getting the description fails, no big deal
                            // Device path is all that really matters
                            WindowsNative.SetupDiGetDeviceRegistryProperty(infoSet, ref infoData, 0, ref regType, descriptionBuffer, descriptionBuffer.Length, ref requiredSize);

                            string desc = Encoding.UTF8.GetString(descriptionBuffer);
                            desc = desc.Remove(desc.IndexOf((char)0));

                            devices.Add(new DeviceDescriptor(path, desc));
                        }
                    }
                }

                // Free the allocated memory to prevent a leak
                WindowsNative.SetupDiDestroyDeviceInfoList(infoSet);
            }

            return devices;
        }
    }
}
