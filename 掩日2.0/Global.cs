using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 掩日2._0
{
    class Global
    {
        public static string MINGWPATH;
        public static string ICONPATH = "";

        public static string[] C_Execute = {"执行1-VirtualAlloc",
                                            "执行2-GetProcAddress",
                                            "注入现有进程",
                                            "注入新进程"};

        public static string[] CS_Execute_x64 = {"执行1-GetProcAddress",
                                             "执行2-VirtualProtect",
                                             "注入现有进程",
                                             "注入新进程"};

        public static string[] CS_Execute_x86 = {"执行1-GetProcAddress",
                                                 "执行2-VirtualProtect"};

        public static string[] VM_Sandbox = {"不使用",
                                             "沙箱：延时约180秒",
                                             "虚拟机：简单反虚拟机"};

        //持久化，TODO
        public static string[] Persistence = { "不使用",
                                               "写入开机启动目录"};

        public static string[] Company_name = {"ApplePen", "SAVVYTECH", "TechyType", "TECHIEIE", "TechFLRRY", "TechCODER", "MicroSofter", "Gooogle",
                                               "Webuzz", "Tecazort", "Cyberry", "BigMi", "Facebowl", "Amazzon", "Hundred Poison", "BetaBet"};

    }
}
