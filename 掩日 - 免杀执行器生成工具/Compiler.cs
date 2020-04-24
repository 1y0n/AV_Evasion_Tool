using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;


namespace 掩日___免杀执行器生成工具
{
    class Compiler
    {
        CSharpCodeProvider provider = new CSharpCodeProvider();
        CompilerParameters parameters = new CompilerParameters();

        public Compiler()
        {
            parameters.ReferencedAssemblies.Add("System.Core.dll");
            parameters.GenerateInMemory = false;
            parameters.GenerateExecutable = true;
            parameters.IncludeDebugInformation = false;
            parameters.ReferencedAssemblies.Add("mscorlib.dll");
            parameters.ReferencedAssemblies.Add("System.dll");
        }
        
        public void compileToExe(String code, String decKey, String filePath, String Arch, string type="exe")
        {

            parameters.OutputAssembly = filePath;
            parameters.CompilerOptions = "/target:" + type + Arch;
            if (type != "exe") 
            {
                parameters.GenerateExecutable = false;
            }

            CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);

            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();

                foreach (CompilerError error in results.Errors)
                {
                    sb.AppendLine(String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                }

                throw new InvalidOperationException(sb.ToString());
            }
        }
    }
}
