<p align="center">
  <img width="100" height="100" src="https://sec-note.oss-cn-beijing.aliyuncs.com/trojan.ico">
</p>



<h1 align="center"> 掩日 2.0 - Advanced AV Evasion Tool For Red Team.</h1>

**2.0 版本开发完成，正在测试~**

用于生成免杀的EXE可执行文件

## 环境安装
2.0 依赖：
1. 64位 Windows 操作系统
2. .net framework 4.0+(Windows 自带)
3. tdm-gcc

tdm-gcc[下载地址](https://github.com/jmeubank/tdm-gcc/releases/download/v9.2.0-tdm64-1/tdm64-gcc-9.2.0.exe)，
下载并安装，完成后，新建一个cmd窗口，输入 `gcc -version` ，出现以下效果即说明成功：

![](https://sec-note.oss-cn-beijing.aliyuncs.com/img/20200604232603.png)

## 使用说明

**生成路径中不要包含中文，否则很可能生成失败**

### 极简模式
双击运行

![](https://sec-note.oss-cn-beijing.aliyuncs.com/img/20200604232812.png)

输入 IP:端口 ，例如 127.0.0.1:4444 ，选择目标系统是 32位还是 64位。然后点击生成即可。

### 进阶模式
点击生成按钮旁边的 进阶 ，即可使用进阶模式。

![](https://sec-note.oss-cn-beijing.aliyuncs.com/img/20200604233042.png)

首先选择目标系统是 32位还是 64位，然后选择使用的语言（推荐C），接着选择执行方式，如果选择注入到现有进程，需要提供进程 PID，如果选择注入新进程，需要提供启动的进程名。虚拟机/沙箱、图标这里按心情设置。最后粘贴你的 shellcode 到最下方的输入框。点击生成按钮。

## 更新
暂无计划。

## 引用
1.0版本参考了 Avitor 的结构。使用了 DotNetToJscript 的代码。部分代码参考了网络资料。

2.0版本修改使用了很多网络代码，因为各种转载，来源已不可考，在此表示感谢❤。

## 提示
因为是 C# 编写的程序，同时公开了源码，所以免杀会很快失效，且用且珍惜🤪。

