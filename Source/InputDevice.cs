using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace SharpSlugsEngine
{
    internal class InputDevice : IDisposable
    {
        private DeviceManager _manager;

        private DeviceDescriptor _descriptor;
        private WindowsNative.HIDP_CAPS _capabilities;
        private bool _open;
        internal bool _connected;

        private IntPtr _hid;

        public string DevicePath => _descriptor.path;
        public string DeviceDescription => _descriptor.desc;
        public int VendorID => _descriptor.vid;
        public int ProductID => _descriptor.pid;

        private delegate byte[] ReadDelegate();
        public delegate void ReadCallback(byte[] bytes);

        public delegate void ControllerDisconnected();
        public event ControllerDisconnected OnDisconnect;

        public InputDevice(DeviceDescriptor descriptor, DeviceManager manager)
        {
            _descriptor = descriptor;
            _manager = manager;

            Open();

            if (_open)
            {
                _connected = true;

                WindowsNative.HIDD_ATTRIBUTES attr = new WindowsNative.HIDD_ATTRIBUTES();
                attr.Size = Marshal.SizeOf(attr);
                WindowsNative.HidD_GetAttributes(_hid, ref attr);

                _capabilities = default(WindowsNative.HIDP_CAPS);
                IntPtr ptr = default(IntPtr);
                if (WindowsNative.HidD_GetPreparsedData(_hid, ref ptr))
                {
                    WindowsNative.HidP_GetCaps(ptr, ref _capabilities);
                    WindowsNative.HidD_FreePreparsedData(ptr);
                }

                Close();

                _descriptor.vid = attr.VendorID;
                _descriptor.pid = attr.ProductID;
            }
        }

        public void ReadAsync(ReadCallback callback)
        {
            ReadDelegate del = new ReadDelegate(Read);
            del.BeginInvoke(AsyncReadEnd, new object[2] { del, callback });
        }

        public void AsyncReadEnd(IAsyncResult res)
        {
            object[] objects = res.AsyncState as object[];

            ReadDelegate del = objects[0] as ReadDelegate;
            ReadCallback callback = objects[1] as ReadCallback;
            callback.Invoke(del.EndInvoke(res));
        }

        public byte[] Read()
        {
            if (!_open) Open();

            byte[] bytes = new byte[] { };
            
            if (_capabilities.InputReportByteLength <= 0)
            {
                return bytes;
            }
            
            bytes = new byte[_capabilities.InputReportByteLength];
            IntPtr buffer = Marshal.AllocHGlobal(bytes.Length);

            try
            {
                NativeOverlapped overlapped = new NativeOverlapped();

                if (!WindowsNative.ReadFile(_hid, buffer, (uint)bytes.Length, out uint read, ref overlapped))
                {
                    int err = Marshal.GetLastWin32Error();
                    if (err == 1167)
                    {
                        Close();
                        _connected = false;
                        _manager.RemoveDevice(this);
                        OnDisconnect?.Invoke();
                    }
                    else
                    {
                        throw new Exception("Error #" + err + ": " + new Win32Exception(err).Message);
                    }
                }
                Marshal.Copy(buffer, bytes, 0, (int)read);
            }
            finally
            {
                //Using finally to guarantee this is freed to prevent memory leaks
                Marshal.FreeHGlobal(buffer);
            }

            return bytes;
        }

        public bool TryReconnect()
        {
            if (_manager.Devices.Any(device => device.path == _descriptor.path))
            {
                _connected = true;
                return true;
            }

            return false;
        }

        public void Open()
        {
            if (_open) return;

            WindowsNative.SECURITY_ATTRIBUTES sec = new WindowsNative.SECURITY_ATTRIBUTES();
            sec.SetupDefaults();
            sec.bInheritHandle = true;

            //3 = FILE_SHARE_READ | FILE_SHARE_WRITE, OPEN_EXISTING
            //2147483648 = GENERIC_READ
            _hid = WindowsNative.CreateFile(_descriptor.path, 2147483648, 3, ref sec, 3, 0, 0);

            if (_hid.ToInt32() != -1) _open = true;
        }

        public void Close()
        {
            WindowsNative.CancelIoEx(_hid, IntPtr.Zero);
            WindowsNative.CloseHandle(_hid);

            _hid = new IntPtr(-1);
            _open = false;
        }

        public void Dispose()
        {
            (this as IDisposable).Dispose();
        }

        void IDisposable.Dispose()
        {
            if (_open) Close();
        }
    }
}
