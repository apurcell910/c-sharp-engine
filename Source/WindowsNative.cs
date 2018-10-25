using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace SharpSlugsEngine
{
    internal static class WindowsNative
    {
        private static Guid _hidGuid = Guid.Empty;

        /// <summary>
        /// Returns the GUID for HIDClass devices
        /// </summary>
        internal static Guid HidGuid
        {
            get
            {
                if (_hidGuid == Guid.Empty) HidD_GetHidGuid(ref _hidGuid);
                return _hidGuid;
            }
        }

        //ftp://81.20.17.7/Langs/vc2015/include/10.0.10586.0/shared/devpkey.h
        internal static readonly DEVPROPKEY DEVPKEY_Device_BusReportedDeviceDesc = new DEVPROPKEY()
        {
            fmtid = new Guid(0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2),
            pid = 4
        };

        #region Windows Structs
        //https://docs.microsoft.com/en-us/windows/desktop/api/setupapi/ns-setupapi-_sp_devinfo_data
        [StructLayout(LayoutKind.Sequential)]
        internal struct SP_DEVINFO_DATA
        {
            public void SetupDefaults()
            {
                cbSize = Marshal.SizeOf(this);
                DevInst = 0;
                ClassGuid = Guid.Empty;
                Reserved = IntPtr.Zero;
            }

            public int cbSize;
            public Guid ClassGuid;
            public int DevInst;
            public IntPtr Reserved;
        }

        //https://docs.microsoft.com/en-us/windows/desktop/api/setupapi/ns-setupapi-_sp_device_interface_data
        [StructLayout(LayoutKind.Sequential)]
        internal struct SP_DEVICE_INTERFACE_DATA
        {
            public int cbSize;
            public Guid InterfaceClassGuid;
            public int Flags;
            public IntPtr Reserved;
        }

        //https://docs.microsoft.com/en-us/windows/desktop/api/setupapi/ns-setupapi-_sp_device_interface_detail_data_a
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public int Size;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string DevicePath;
        }

        //https://docs.microsoft.com/en-us/windows-hardware/drivers/install/devpropkey
        [StructLayout(LayoutKind.Sequential)]
        internal struct DEVPROPKEY
        {
            public Guid fmtid;
            public ulong pid;
        }

        //https://msdn.microsoft.com/en-us/library/windows/desktop/aa379560%28v=vs.85%29.aspx?f=255&MSPPError=-2147217396
        [StructLayout(LayoutKind.Sequential)]
        internal struct SECURITY_ATTRIBUTES
        {
            public void SetupDefaults()
            {
                lpSecurityDescriptor = IntPtr.Zero;
                bInheritHandle = false;
                nLength = Marshal.SizeOf(this);
            }

            public int nLength;
            public IntPtr lpSecurityDescriptor;
            public bool bInheritHandle;
        }

        //https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/content/hidsdi/ns-hidsdi-_hidd_attributes
        //Those docs say Size is a ulong, but that seems incorrect as far as I can tell
        [StructLayout(LayoutKind.Sequential)]
        internal struct HIDD_ATTRIBUTES
        {
            public int Size;
            public ushort VendorID;
            public ushort ProductID;
            public ushort VersionNumber;
        }

        //https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/content/hidpi/ns-hidpi-_hidp_caps
        [StructLayout(LayoutKind.Sequential)]
        internal struct HIDP_CAPS
        {
            internal short UsagePage;
            internal short Usage;
            internal ushort InputReportByteLength;
            internal ushort OutputReportByteLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
            internal ushort[] Reserved;
            internal ushort NumberLinkCollectionNodes;
            internal ushort NumberInputButtonCaps;
            internal ushort NumberInputValueCaps;
            internal ushort NumberInputDataIndices;
            internal ushort NumberOutputButtonCaps;
            internal ushort NumberOutputValueCaps;
            internal ushort NumberOutputDataIndices;
            internal ushort NumberFeatureButtonCaps;
            internal ushort NumberFeatureValueCaps;
            internal ushort NumberFeatureDataIndices;
        }
        #endregion
        
        //https://docs.microsoft.com/en-us/windows/desktop/api/setupapi/nf-setupapi-setupdigetclassdevsw
        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, string Enumerator, int hwndParent, int flags);

        internal const short DIGCF_DEFAULT = 1;
        internal const short DIGCF_PRESENT = 2;
        internal const short DIGCF_ALLCLASSES = 4;
        internal const short DIGCF_PROFILE = 8;
        internal const short DIGCF_DEVICEINTERFACE = 16;

        //https://docs.microsoft.com/en-us/windows/desktop/api/setupapi/nf-setupapi-setupdienumdeviceinfo
        [DllImport("setupapi.dll")]
        internal static extern bool SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, int MemberIndex, ref SP_DEVINFO_DATA DeviceInfoData);

        //https://docs.microsoft.com/en-us/windows/desktop/api/setupapi/nf-setupapi-setupdienumdeviceinterfaces
        [DllImport("setupapi.dll")]
        internal static extern bool SetupDiEnumDeviceInterfaces(IntPtr DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, ref Guid InterfaceClassGuid, int MemberIndex, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData);
        
        //https://docs.microsoft.com/en-us/windows/desktop/api/setupapi/nf-setupapi-setupdigetdeviceinterfacedetaila
        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        internal static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr DeviceInfoSet, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData, ref SP_DEVICE_INTERFACE_DETAIL_DATA DeviceInterfaceDetailData, int DeviceInterfaceDetailDataSize, ref int RequiredSize, IntPtr DeviceInfoData);

        //Same as above call, but with IntPtr for DeviceInterfaceDetailData so that it may be NULL
        //Copy pasted /// comment for convenience
        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        internal static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr DeviceInfoSet, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData, IntPtr DeviceInterfaceDetailData, int DeviceInterfaceDetailDataSize, ref int RequiredSize, IntPtr DeviceInfoData);

        //https://docs.microsoft.com/en-us/windows/desktop/api/setupapi/nf-setupapi-setupdidestroydeviceinfolist
        [DllImport("setupapi.dll")]
        internal static extern int SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

        //https://docs.microsoft.com/en-us/windows/desktop/api/setupapi/nf-setupapi-setupdigetdevicepropertyw
        [DllImport("setupapi.dll")]
        internal static extern bool SetupDiGetDevicePropertyW(IntPtr deviceInfo, ref SP_DEVINFO_DATA deviceInfoData, ref DEVPROPKEY propKey, ref ulong propertyDataType, byte[] propertyBuffer, int propertyBufferSize, ref int requiredSize, uint flags);

        //https://docs.microsoft.com/en-us/windows/desktop/api/setupapi/nf-setupapi-setupdigetdeviceregistrypropertya
        [DllImport("setupapi.dll")]
        internal static extern bool SetupDiGetDeviceRegistryProperty(IntPtr deviceInfoSet, ref SP_DEVINFO_DATA deviceInfoData, int propertyVal, ref int propertyRegDataType, byte[] propertyBuffer, int propertyBufferSize, ref int requiredSize);

        //https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/content/hidsdi/nf-hidsdi-hidd_gethidguid
        [DllImport("hid.dll")]
        internal static extern void HidD_GetHidGuid(ref Guid hidGuid);

        //https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/content/hidsdi/nf-hidsdi-hidd_getattributes
        [DllImport("hid.dll")]
        internal static extern bool HidD_GetAttributes(IntPtr HidDeviceObject, ref HIDD_ATTRIBUTES Attributes);

        //https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/content/hidsdi/nf-hidsdi-hidd_getpreparseddata
        [DllImport("hid.dll")]
        internal static extern bool HidD_GetPreparsedData(IntPtr HidDeviceObject, ref IntPtr PreparsedData);

        //https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/content/hidpi/nf-hidpi-hidp_getcaps
        [DllImport("hid.dll")]
        internal static extern int HidP_GetCaps(IntPtr PreparsedData, ref HIDP_CAPS Capabilities);

        //https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/content/hidsdi/nf-hidsdi-hidd_freepreparseddata
        [DllImport("hid.dll")]
        internal static extern bool HidD_FreePreparsedData(IntPtr PreparsedData);

        //https://docs.microsoft.com/en-us/windows/desktop/api/fileapi/nf-fileapi-createfilea
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, int dwShareMode, ref SECURITY_ATTRIBUTES lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, int hTemplateFile);

        //https://docs.microsoft.com/en-us/windows/desktop/fileio/cancelioex-func
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        internal static extern bool CancelIoEx(IntPtr hFile, IntPtr lpOverlapped);

        //https://msdn.microsoft.com/en-us/library/windows/desktop/ms724211(v=vs.85).aspx
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        internal static extern bool CloseHandle(IntPtr hObject);

        //https://docs.microsoft.com/en-us/windows/desktop/api/fileapi/nf-fileapi-readfile
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool ReadFile(IntPtr hFile, IntPtr lpBuffer, uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, ref NativeOverlapped lpOverlapped);
    }
}
