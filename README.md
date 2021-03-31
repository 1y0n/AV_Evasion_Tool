<p align="center">
  <img width="100" height="100" src="https://sec-note.oss-cn-beijing.aliyuncs.com/trojan.ico">
</p>



<h1 align="center"> 掩日 - Advanced AV Evasion Tool For Red Team Ops</h1>

用于快速生成免杀的 EXE 可执行文件。

## 声明
![#f03c15](https://via.placeholder.com/15/f03c15/000000?text=+) 仅限用于技术研究和获得正式授权的测试活动。

![#f03c15](https://via.placeholder.com/15/f03c15/000000?text=+) legal disclaimer: Usage of this tool for attacking targets without prior mutual consent is illegal. It is the end user's responsibility to obey all applicable local, state and federal laws. Developers assume no liability and are not responsible for any misuse or damage caused by this program.

## 下载

**[3.1.2下载](https://github.com/1y0n/AV_Evasion_Tool/releases/download/3.1.2/3.1.2.zip)**

## 依赖
3.x 依赖：
1. 64位 Windows 7、8、10 操作系统
2. .net framework 4.0 或更高版本 (Windows 自带)
3. tdm-gcc

tdm-gcc[下载地址](https://github.com/jmeubank/tdm-gcc/releases/download/v9.2.0-tdm64-1/tdm64-gcc-9.2.0.exe)，双击运行，选择 CREATE，然后一直“下一步”即可。
下载并安装，完成后，新建一个cmd窗口，输入 `gcc --version` ，出现以下效果即说明成功：

![](https://sec-note.oss-cn-beijing.aliyuncs.com/img/20200604232603.png)

## 使用

生成过程中请关闭所有的杀毒软件（包括 Windows Defender），否则很可能生成失败！！！（生成完成后正常开启即可）

64位免杀效果远好于32位，能用64尽量用64。

**生成路径中尽量不要包含中文，否则可能生成失败**

![](https://github.com/1y0n/AV_Evasion_Tool/blob/master/images/yr3.gif)

处理 exe 文件也是一样的：
![](https://github.com/1y0n/AV_Evasion_Tool/blob/master/images/yr3_exe.gif)

如果你添加了自定义密码，那么就执行的时候在后面加密码就可以了，比如： xxx.exe 1234

## 更新

**在一些特殊情况下，新版本的免杀效果不一定好，此时可尝试在 release 页面下载旧版本**

v3.1.2 2021年2月6日
  1. 免杀效果优化
  2. 现在用户可以自行决定要不要隐藏窗口
  3. 由于算法原因，免杀的exe运行时，会进入一个很长很长的解密阶段，大概需要几分钟，解密后可以正常运行

v3.0RC 2020年12月29日
  1. 新年快乐！
  2. 修复了一个严重bug
  3. 免杀效果优化
  4. 支持自定义密码

v3.0beta2 2020年11月29日
  1. 修复了找不到 dll 的问题

v3.0 2020年11月14日
  1. 更易于使用
  2. 理论上更好的免杀效果😂
  3. 支持对 exe 文件进行二次处理实现免杀（基于Donut）

v2.1 2020年9月1日
  1. 现在每次异或的key都会随机生成了；
  2. 部分细节更新。

## 引用
1.0版本参考了 Avitor 的结构。使用了 DotNetToJscript 的代码。部分代码参考了网络资料。

2.0版本修改使用了很多网络代码，因为各种转载，来源已不可考，在此表示感谢❤。

3.0版本基于TheWover,Odzhan 的 Donut 项目，他们的技术水平及开源精神令人敬佩。3.0只是在他们的基础上做了一点微小的图形化工作。

## 赞助
如果这个工具对你有用，就请我喝杯奶茶吧。赞助请留言你的微信号，可以更早获得更新（但是新版本一般不稳定，bug很多）😉
![](https://github.com/1y0n/AV_Evasion_Tool/blob/master/images/donate.png)
