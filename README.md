<p align="center">
  <img width="100" height="100" src="https://sec-note.oss-cn-beijing.aliyuncs.com/trojan.ico">
</p>



<h1 align="center"> 掩日 - Advanced AV Evasion Tool For Red Team Ops</h1>

用于生成免杀的EXE可执行文件。有账号的可以考虑点个⭐

## 声明
![#f03c15](https://via.placeholder.com/15/f03c15/000000?text=+) 仅限用于技术研究和获得正式授权的测试活动。

![#f03c15](https://via.placeholder.com/15/f03c15/000000?text=+) legal disclaimer: Usage of this tool for attacking targets without prior mutual consent is illegal. It is the end user's responsibility to obey all applicable local, state and federal laws. Developers assume no liability and are not responsible for any misuse or damage caused by this program.

## 下载

你可以[在此下载](https://github.com/1y0n/AV_Evasion_Tool/releases/tag/2.1)编译好的exe。

## 依赖
2.0 依赖：
1. 64位 Windows 7、8、10 操作系统
2. .net framework 4.0或更高版本 (Windows 自带)
3. tdm-gcc

tdm-gcc[下载地址](https://github.com/jmeubank/tdm-gcc/releases/download/v9.2.0-tdm64-1/tdm64-gcc-9.2.0.exe)，
下载并安装，完成后，新建一个cmd窗口，输入 `gcc --version` ，出现以下效果即说明成功：

![](https://sec-note.oss-cn-beijing.aliyuncs.com/img/20200604232603.png)

## 使用

**生成路径中不要包含中文，否则很可能生成失败**

### 极简模式
双击运行

![](https://sec-note.oss-cn-beijing.aliyuncs.com/img/20200604232812.png)

输入 IP:端口 ，例如 127.0.0.1:4444 ，选择目标系统是 32位还是 64位。然后点击生成即可。

### 进阶模式
点击生成按钮旁边的 进阶 ，即可使用进阶模式。

![](https://sec-note.oss-cn-beijing.aliyuncs.com/img/20200604233042.png)

首先选择目标系统是 32位还是 64位，然后选择使用的语言（推荐C），接着选择执行方式，如果选择注入到现有进程，需要提供进程 PID，如果选择注入新进程，需要提供启动的进程名。虚拟机/沙箱、图标这里按心情设置。最后粘贴你的 shellcode 到最下方的输入框。点击生成按钮。

**在测试的时候发现使用语言为 C，执行方式为 执行1 时会出现一些莫名其妙的问题，所以更推荐 执行2**

### 效果
![](https://sec-note.oss-cn-beijing.aliyuncs.com/img/cce858a5c99f6909f32a839a0b02975.png)

## 更新
v2.1 2020年9月1日
  1. 现在每次异或的key都会随机生成了；
  2. 部分细节更新。


想关注更新的话可以点 Star 旁边的 Watch 。

## 引用
1.0版本参考了 Avitor 的结构。使用了 DotNetToJscript 的代码。部分代码参考了网络资料。

2.0版本修改使用了很多网络代码，因为各种转载，来源已不可考，在此表示感谢❤。

