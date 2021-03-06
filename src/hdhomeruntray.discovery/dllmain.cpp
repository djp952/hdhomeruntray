//---------------------------------------------------------------------------
// Copyright (c) 2021-2022 Michael G. Brehm
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//---------------------------------------------------------------------------

#include <WinSock2.h>
#include <Windows.h>

#ifdef __CLR_VER
#error dllmain.cpp cannot be compiled with Common Language Runtime (CLR) support
#endif

#pragma warning(push, 4)

//---------------------------------------------------------------------------
// DllMain
//
// Dynamic link library entry point
//
// Arguments:
//
//	instance		- Module base address
//	reason			- Reason DllMain is being invoked
//	context			- Load context information

extern "C" int APIENTRY DllMain(HINSTANCE /*instance*/, DWORD reason, LPVOID /*context*/)
{
	static int wsaresult = 0;			// Result from WSAStartup
	WSADATA wsadata = {};				// Windows sockets startup structure

	switch(reason) {

		case DLL_PROCESS_ATTACH:

			wsaresult = WSAStartup(MAKEWORD(2, 2), &wsadata);
			break;

		case DLL_PROCESS_DETACH:

			if(wsaresult == 0) WSACleanup();
			break;
	}

	return TRUE;
}

//---------------------------------------------------------------------------

#pragma warning(pop)

