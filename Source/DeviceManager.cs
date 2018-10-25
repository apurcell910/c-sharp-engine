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
                    InputDevice device = new InputDevice(desc, this);
                    if (device.VendorID == 0x45E && device.ProductID == 0x28E && !controllers.Any(controller => controller.Path == device.DevicePath))
                    {
                        Xbox360Controller newController = new Xbox360Controller(device);
                        controllers.Add(newController);
                        _controllerAdded?.Invoke(newController);
                    }
                    else
                    {
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

            Guid hidClass = WindowsNative.HidGuid;
            IntPtr infoSet = WindowsNative.SetupDiGetClassDevs(ref hidClass, null, 0, WindowsNative.DIGCF_PRESENT | WindowsNative.DIGCF_DEVICEINTERFACE);

            //-1 = INVALID_HANDLE_VALUE
            if (infoSet.ToInt64() != -1)
            {
                WindowsNative.SP_DEVINFO_DATA infoData = new WindowsNative.SP_DEVINFO_DATA();
                infoData.SetupDefaults();

                int deviceIndex = 0;

                while (WindowsNative.SetupDiEnumDeviceInfo(infoSet, deviceIndex, ref infoData))
                {
                    deviceIndex++;

                    WindowsNative.SP_DEVICE_INTERFACE_DATA deviceInterfaceData = new WindowsNative.SP_DEVICE_INTERFACE_DATA();
                    deviceInterfaceData.cbSize = Marshal.SizeOf(deviceInterfaceData);

                    int deviceInterfaceIndex = 0;

                    while (WindowsNative.SetupDiEnumDeviceInterfaces(infoSet, ref infoData, ref hidClass, deviceInterfaceIndex, ref deviceInterfaceData))
                    {
                        deviceInterfaceIndex++;

                        int bufferSize = 0;
                        WindowsNative.SP_DEVICE_INTERFACE_DETAIL_DATA interfaceDetail = new WindowsNative.SP_DEVICE_INTERFACE_DETAIL_DATA();
                        interfaceDetail.Size = IntPtr.Size == 4 ? 4 + Marshal.SystemDefaultCharSize : 8;

                        WindowsNative.SetupDiGetDeviceInterfaceDetail(infoSet, ref deviceInterfaceData, IntPtr.Zero, 0, ref bufferSize, IntPtr.Zero);

                        string path = WindowsNative.SetupDiGetDeviceInterfaceDetail(infoSet, ref deviceInterfaceData, ref interfaceDetail, bufferSize, ref bufferSize, IntPtr.Zero) ? interfaceDetail.DevicePath : null;
                        
                        byte[] descriptionBuffer = new byte[1024];
                        ulong propertyType = 0;
                        int requiredSize = 0;

                        WindowsNative.DEVPROPKEY key = WindowsNative.DEVPKEY_Device_BusReportedDeviceDesc;

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

                            WindowsNative.SetupDiGetDeviceRegistryProperty(infoSet, ref infoData, 0, ref regType, descriptionBuffer, descriptionBuffer.Length, ref requiredSize);

                            string desc = Encoding.UTF8.GetString(descriptionBuffer);
                            desc = desc.Remove(desc.IndexOf((char)0));

                            devices.Add(new DeviceDescriptor(path, desc));
                        }
                    }
                }

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
