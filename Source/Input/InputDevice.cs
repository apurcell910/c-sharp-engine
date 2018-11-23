using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace SharpSlugsEngine.Input
{
    /// <summary>
    /// Manages the reading of data from a HID compliant device
    /// </summary>
    internal class InputDevice : IDisposable
    {
        private DeviceManager manager;

        private DeviceDescriptor descriptor;
        private WindowsNative.HIDP_CAPS capabilities;
        private bool open;
        private IntPtr hid;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputDevice"/> class with the specified <see cref="DeviceDescriptor"/> and parent <see cref="DeviceManager"/>
        /// </summary>
        /// <param name="descriptor">Describes the HID compliant device to open a connection to</param>
        /// <param name="manager">The parent <see cref="DeviceManager"/> of this <see cref="InputDevice"/></param>
        public InputDevice(DeviceDescriptor descriptor, DeviceManager manager)
        {
            this.descriptor = descriptor;
            this.manager = manager;

            Open();

            // If the device succeeded in opening, get the attributes and capabilities
            if (open)
            {
                Connected = true;

                WindowsNative.HIDD_ATTRIBUTES attr = new WindowsNative.HIDD_ATTRIBUTES();
                attr.Size = Marshal.SizeOf(attr);
                WindowsNative.HidD_GetAttributes(hid, ref attr);

                capabilities = default(WindowsNative.HIDP_CAPS);
                IntPtr ptr = default(IntPtr);

                // This shouldn't fail, but no big deal if it does
                // The device will just end up getting garbage collected if it has invalid vid/pid
                if (WindowsNative.HidD_GetPreparsedData(hid, ref ptr))
                {
                    WindowsNative.HidP_GetCaps(ptr, ref capabilities);
                    WindowsNative.HidD_FreePreparsedData(ptr);
                }

                Close();

                this.descriptor.SetIDSFromAttributes(attr);
            }
        }

        /// <summary>
        /// Used for passing read bytes to other classes
        /// </summary>
        /// <param name="bytes">The bytes that have been read</param>
        public delegate void ReadCallback(byte[] bytes);

        /// <summary>
        /// Used for controller disconnection events
        /// </summary>
        public delegate void ControllerDisconnected();

        /// <summary>
        /// Used for device reads
        /// </summary>
        /// <returns>The bytes read from the device</returns>
        private delegate byte[] ReadDelegate();

        /// <summary>
        /// Called when the device disconnects from the system
        /// </summary>
        public event ControllerDisconnected OnDisconnect;

        /// <summary>
        /// Gets a value indicating whether this <see cref="InputDevice"/> is connected
        /// </summary>
        public bool Connected { get; private set; }

        /// <summary>
        /// Gets the device path
        /// </summary>
        public string DevicePath => descriptor.Path;

        /// <summary>
        /// Gets the device description
        /// </summary>
        public string DeviceDescription => descriptor.Description;

        /// <summary>
        /// Gets the <see cref="InputDevice"/>'s vendor ID
        /// </summary>
        public int VendorID => descriptor.VendorID;

        /// <summary>
        /// Gets the <see cref="InputDevice"/>'s product ID
        /// </summary>
        public int ProductID => descriptor.ProductID;

        /// <summary>
        /// Gets the amount of <see cref="byte"/>s the <see cref="InputDevice"/> is capable of sending
        /// </summary>
        public ushort ReadSize => capabilities.InputReportByteLength;

        /// <summary>
        /// Begin asynchronous reading of data
        /// </summary>
        /// <param name="callback">The function to be called when data is done being read</param>
        public void ReadAsync(ReadCallback callback)
        {
            ReadDelegate del = new ReadDelegate(Read);
            del.BeginInvoke(AsyncReadEnd, new object[2] { del, callback });
        }

        /// <summary>
        /// Attempt to reconnect after disconnecting from the device
        /// </summary>
        /// <returns>A value indicating whether the reconnection was successful</returns>
        public bool TryReconnect()
        {
            if (Connected)
            {
                return true;
            }

            // Kinda inefficient check, should probably add a timeout to this so it doesn't just retry indefinitely
            if (manager.Devices.Any(device => device.Path == descriptor.Path))
            {
                Connected = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Closes the connection to the device
        /// </summary>
        public void Dispose()
        {
            if (open)
            {
                Close();
            }
        }

        /// <summary>
        /// Closes the connection to the device
        /// </summary>
        void IDisposable.Dispose()
        {
            Dispose();
        }

        /// <summary>
        /// Opens the connection to the device
        /// </summary>
        private void Open()
        {
            if (open)
            {
                return;
            }

            WindowsNative.SECURITY_ATTRIBUTES sec = new WindowsNative.SECURITY_ATTRIBUTES();
            sec.SetupDefaults();
            sec.bInheritHandle = true;

            // 3 = FILE_SHARE_READ | FILE_SHARE_WRITE, OPEN_EXISTING
            // 2147483648 = GENERIC_READ
            hid = WindowsNative.CreateFile(descriptor.Path, 2147483648, 3, ref sec, 3, 0, 0);

            // Check that it actually succeeded before setting open
            if (hid.ToInt32() != -1)
            {
                open = true;
            }
        }

        /// <summary>
        /// Closes the connection to the device
        /// </summary>
        private void Close()
        {
            // I think this first call is only for Vista+, but do we really have to care about XP?
            WindowsNative.CancelIoEx(hid, IntPtr.Zero);
            WindowsNative.CloseHandle(hid);

            hid = new IntPtr(-1);
            open = false;
        }

        /// <summary>
        /// Called when bytes finish being read
        /// </summary>
        /// <param name="res">Contains the delegates involved in the read</param>
        private void AsyncReadEnd(IAsyncResult res)
        {
            // Pull the read function and callback out of the IAsyncResult
            object[] objects = res.AsyncState as object[];
            ReadDelegate del = objects[0] as ReadDelegate;
            ReadCallback callback = objects[1] as ReadCallback;

            // Send the data up to the controller class
            callback.Invoke(del.EndInvoke(res));
        }

        /// <summary>
        /// Reads bytes from the device
        /// </summary>
        /// <returns>The read bytes</returns>
        private byte[] Read()
        {
            if (!open)
            {
                Open();
            }

            byte[] bytes = new byte[] { };

            // I'm not sure why a controller wouldn't be able to send anything, but better safe than sorry
            if (capabilities.InputReportByteLength <= 0)
            {
                return bytes;
            }

            // Create return array and allocate unmanaged array for native calls
            bytes = new byte[capabilities.InputReportByteLength];
            IntPtr buffer = Marshal.AllocHGlobal(bytes.Length);

            try
            {
                // Just a windows native struct
                NativeOverlapped overlapped = new NativeOverlapped();

                if (!WindowsNative.ReadFile(hid, buffer, (uint)bytes.Length, out uint read, ref overlapped))
                {
                    // If the I/O read failed, get the error
                    int err = Marshal.GetLastWin32Error();

                    // Error 1167 means the device isn't connected
                    if (err == 1167)
                    {
                        Close();
                        Connected = false;
                        manager.RemoveDevice(this);
                        OnDisconnect?.Invoke();
                    }
                    else
                    {
                        // If it's an unknown error, throw it
                        throw new Exception("Error #" + err + ": " + new Win32Exception(err).Message);
                    }
                }

                Marshal.Copy(buffer, bytes, 0, (int)read);
            }
            finally
            {
                // Using finally to guarantee this is freed to prevent memory leaks
                Marshal.FreeHGlobal(buffer);
            }

            return bytes;
        }
    }
}
