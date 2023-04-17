<p align="center">
  <img width="100" height="100" src="https://github.com/1y0n/AV_Evasion_Tool/blob/master/images/2021.ico">
</p>

<p align="center">
  <img width="374" height="50" src="https://github.com/1y0n/AV_Evasion_Tool/blob/master/images/xred.team.png">
</p>


<h1 align="center">掩日 - 适用于红队的综合免杀工具</h1>


## 声明

1. 仅限用于技术研究和获得正式授权的测试活动！
2. 工作繁忙、水平低下、精力有限、时间仓促，代码未经过大量测试，如发现问题请提交 issue。
3. 主程序已被部分杀软标记，请添加到杀软白名单。
4. issue很少看，想聊可以加（备注github）
<p>
  <img width="281" height="381" src="https://github.com/1y0n/AV_Evasion_Tool/blob/master/images/6885f8e2548b84f0def32b8bf3d412d.jpg">
</p>

## 下载

[Github 下载](https://github.com/1y0n/AV_Evasion_Tool/releases/download/20230417/20230417.zip)

## 依赖
如果使用工具的全部功能，请确保满足以下全部条件：
1. 64位 Windows 7 或以上操作系统
2. .net framework 4.5 或更高版本
3. 安装 tdm-gcc
4. 安装 [64位 Go 语言环境](https://go.dev/dl/go1.17.8.windows-amd64.msi)，并添加到系统环境变量

tdm-gcc[下载地址](https://github.com/jmeubank/tdm-gcc/releases/download/v9.2.0-tdm64-1/tdm64-gcc-9.2.0.exe)，双击运行，选择 CREATE，然后一直“下一步”即可。
下载并安装，完成后，新建一个cmd窗口，输入 `gcc --version` ，能够正常显示版本号即说明成功。如果安装成功还是提示出错，需要在环境变量里把tdm-gcc移动到最前面。

## 使用

**生成路径中不要包含中文和空格，并且生成过程中尽量关闭所有的杀毒软件，否则会生成失败！**

64位免杀效果远好于32位，能用64尽量用64（注意对应的 shellcode 也需要是64位）。

**针对Cobalt Strike，不要选择生成Windows分阶段木马、Windows无阶段木马，而是生成payload，最终是一个payload.c文件。**

<p align="center">
  <img src="https://github.com/1y0n/AV_Evasion_Tool/blob/master/images/step-1.jpg">
</p>

<p align="center">
  <img src="https://github.com/1y0n/AV_Evasion_Tool/blob/master/images/step-2.jpg">
</p>

## 更新
v20230417 2023年4月17日
  1. 效果优化

v20230329 2023年3月29日
  1. 效果优化
  2. 修复识别杀软时卡死问题

v20230303 2023年3月3日
  1. 效果优化

v20221105 2022年11月5日
  1. 一点儿更新

v20220801 2022年8月1日
  1. 效果优化

v20220629 2022年6月29日
  1. 优化某杀软误报问题

v20220419 2022年4月19日
  1. 免杀效果优化

v20220329 2022年3月29日
  1. bug修复（感谢@1191569886反馈）

v20220325 2022年3月25日
  1. bug修复（感谢@Tas9er反馈）

v20220311 2022年3月11日
  1. bug修复
  2. 使用C语言生成的程序体积减小90%（可以使用UPX进一步压缩体积，最终可以压缩到11KB左右）

v20220204 2022年3月7日
  1. 全新的界面
  2. 支持 Go 语言
  3. 支持本地分离
  4. 支持网络分离
  5. 杀软对比功能
  6. 更新检测功能

## 引用
感谢以下优秀项目/文章/网站，本工具修改和使用了其中的代码、资源或思路：
  1. Dount (https://github.com/TheWover/donut)
  2. avList (https://github.com/gh0stkey/avList)
  3. goShellCodeByPassVT (https://github.com/fcre1938/goShellCodeByPassVT)
  4. HandyControls (https://github.com/HandyOrg/HandyControl)
  5. flaticon (https://www.flaticon.com)
