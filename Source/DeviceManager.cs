using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpSlugsEngine
{
    public class DeviceManager
    {
        private Game _game;
        private TimeSpan _lastCheck = TimeSpan.FromTicks(0);

        internal List<DeviceDescriptor> Devices { get; private set; }
        public Xbox360Controller[] Xbox360Controllers { get; private set; }

        public delegate void NewController(Xbox360Controller controller);
        private event NewController _controllerAdded;
        public event NewController ControllerAdded
        {
            add => _controllerAdded += value;
            remove => _controllerAdded -= value;
        }

        internal DeviceManager(Game game)
        {
            _game = game;
            Xbox360Controllers = new Xbox360Controller[0];
        }

        public void Update(GameTime time)
        {
            //Check for controllers infrequently, takes too much CPU power
            if ((time.totalTime - _lastCheck).TotalSeconds >= 1f)
            {
                Devices = GetDevices();
                List<Xbox360Controller> controllers = new List<Xbox360Controller>();
                controllers.AddRange(Xbox360Controllers);

                foreach (DeviceDescriptor desc in Devices)
                {
                    //Only have a class for 360 controllers so far, ignore anything else
                    InputDevice device = new InputDevice(desc, this);
                    if (device.VendorID == 0x45E && device.ProductID == 0x28E && !controllers.Any(controller => controller.Path == device.DevicePath))
                    {
                        Xbox360Controller newController = new Xbox360Controller(device);
                        controllers.Add(newController);

                        //Tell the game there's a new controller if it's subscribed to the event
                        _controllerAdded?.Invoke(newController);
                    }
                    else
                    {
                        //Make sure the InputDevice closes its connection
                        device.Dispose();
                    }
                }

                Xbox360Controllers = controllers.ToArray();
                _lastCheck = time.totalTime;
            }

            foreach (Xbox360Controller controller in Xbox360Controllers)
            {
                controller.Update();
            }
        }

        internal void RemoveDevice(InputDevice device)
        {
            Devices.RemoveAll(desc => desc.path == device.DevicePath);
        }

        private List<DeviceDescriptor> GetDevices()
        {
            List<DeviceDescriptor> devices = new List<DeviceDescriptor>();

            //Get the device information set for connected HID devices
            Guid hidClass = WindowsNative.HidGuid;
            IntPtr infoSet = WindowsNative.SetupDiGetClassDevs(ref hidClass, null, 0, WindowsNative.DIGCF_PRESENT | WindowsNative.DIGCF_DEVICEINTERFACE);

            //-1 = INVALID_HANDLE_VALUE
            if (infoSet.ToInt64() != -1)
            {
                WindowsNative.SP_DEVINFO_DATA infoData = new WindowsNative.SP_DEVINFO_DATA();
                infoData.SetupDefaults();

                //Loop through all the elements in the info set
                int deviceIndex = 0;
                while (WindowsNative.SetupDiEnumDeviceInfo(infoSet, deviceIndex, ref infoData))
                {
                    deviceIndex++;

                    WindowsNative.SP_DEVICE_INTERFACE_DATA deviceInterfaceData = new WindowsNative.SP_DEVICE_INTERFACE_DATA();
                    deviceInterfaceData.cbSize = Marshal.SizeOf(deviceInterfaceData);

                    //Loop through all the devices
                    int deviceInterfaceIndex = 0;
                    while (WindowsNative.SetupDiEnumDeviceInterfaces(infoSet, ref infoData, ref hidClass, deviceInterfaceIndex, ref deviceInterfaceData))
                    {
                        deviceInterfaceIndex++;

                        int bufferSize = 0;

                        //If IntPtr.Size == 4, we're on 32 bit Windows
                        //This check is untested because nobody uses 32 bit Windows in 2018
                        WindowsNative.SP_DEVICE_INTERFACE_DETAIL_DATA interfaceDetail = new WindowsNative.SP_DEVICE_INTERFACE_DETAIL_DATA();
                        interfaceDetail.cbSize = IntPtr.Size == 4 ? 4 + Marshal.SystemDefaultCharSize : 8;

                        //Call this once just to get the right value for bufferSize
                        WindowsNative.SetupDiGetDeviceInterfaceDetail(infoSet, ref deviceInterfaceData, IntPtr.Zero, 0, ref bufferSize, IntPtr.Zero);

                        //Actually fill the detail struct and get device path
                        string path = WindowsNative.SetupDiGetDeviceInterfaceDetail(infoSet, ref deviceInterfaceData, ref interfaceDetail, bufferSize, ref bufferSize, IntPtr.Zero) ? interfaceDetail.DevicePath : null;
                        
                        byte[] descriptionBuffer = new byte[1024];
                        ulong propertyType = 0;
                        int requiredSize = 0;

                        WindowsNative.DEVPROPKEY key = WindowsNative.DEVPKEY_Device_BusReportedDeviceDesc;

                        //Attempt to get description using SetupDiGetDevicePropertyW
                        //This only works on Vista+
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

                            //If this attempt at getting the description fails, no big deal
                            //Device path is all that really matters
                            WindowsNative.SetupDiGetDeviceRegistryProperty(infoSet, ref infoData, 0, ref regType, descriptionBuffer, descriptionBuffer.Length, ref requiredSize);

                            string desc = Encoding.UTF8.GetString(descriptionBuffer);
                            desc = desc.Remove(desc.IndexOf((char)0));

                            devices.Add(new DeviceDescriptor(path, desc));
                        }
                    }
                }

                //Free the allocated memory to prevent a leak
                WindowsNative.SetupDiDestroyDeviceInfoList(infoSet);
            }

            return devices;
        }
    }

    internal struct DeviceDescriptor
    {
        public DeviceDescriptor(string p, string d)
        {
            path = p;
            desc = d;

            vid = 0;
            pid = 0;
        }

        public string path;
        public string desc;

        public int vid;
        public int pid;
    }
}
