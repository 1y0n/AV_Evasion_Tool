using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 掩日___免杀执行器生成工具
{
    enum RuntimeVersion
    {
        None,
        v2,
        v4,
        Auto,
    }

    interface IScriptGenerator
    {
        string GenerateScript(byte[] serialized_object,
                              string entry_class_name,
                              string additional_script,
                              RuntimeVersion version,
                              bool enable_debug);
        bool SupportsScriptlet { get; }
        string ScriptName { get; }
    }
}
