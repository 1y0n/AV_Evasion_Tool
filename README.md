# 掩日 - 免杀执行器生成工具
用于生成免杀的EXE可执行文件/AV evasion

[点此下载](https://github.com/1y0n/AV_Evasion_Tool/releases/)编译好的最新版本。

## 更新
计划更新2.0版本，不再需要手动粘贴shellcode，直接输入IP:端口即可使用，转用C语言生成免杀文件，免杀效果更好，同时，去除了不实用的一键上线功能，专注免杀，只为实用。

## 前置条件

需要系统已经安装了 .net framework 4.0 或更高版本。

## 思路

随机 KEY → 使用此 KEY 对 shellcode 进行加密 → 将加密后的 shellcode 放入对应模板代码中 → 调用 CSC.exe 对模板代码进行加密 → 生成完成

## 使用说明

![](https://sec-note.oss-cn-beijing.aliyuncs.com/img/20200424091844.png)

第一步： 通过 msfvenom 或 cs 生成 c 或 c# 格式的shellcode，将shellcode复制到文本框中。

第二步：根据生成shellcode是32位还是64位，选择对应的“目标架构”。

第三步：选择执行方式，因为已经公开了工具，所以部分执行方式已经不免杀，可以自己换一种测试。

第四步：按需选择添加图标。

第五步：按需选择是否延时执行。

第六步：建议勾选隐藏执行界面。

第七步：点击“生成免杀执行器”：

![](https://sec-note.oss-cn-beijing.aliyuncs.com/img/20200424092253.png)

提示生成成功，现在这个 exe 就可以免杀执行你的 shellcode 了。

**不要将生成的程序上传到在线杀毒网站，同时在自己测试的时候不要使用杀软的“云查杀”功能**

一键上线可以自己探索，原理是白名单执行。目前测试 win10 是没问题的。

## 引用
1.0版本参考了 Avitor 的结构。使用了 DotNetToJscript 的代码。部分代码参考了网络资料。

## 后记
因为是 C# 编写的程序，同时公开了源码，所以免杀会很快失效。

