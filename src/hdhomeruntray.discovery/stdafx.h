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

#ifndef __STDAFX_H_
#define __STDAFX_H_
#pragma once

//---------------------------------------------------------------------------
// CHECK_DISPOSED
//
// Used throughout to make object disposed exceptions easier to read and not
// require hard-coding a class name into the statement.  This will throw the
// function name, but that's actually better in my opinion

#define CHECK_DISPOSED(__flag) \
	if(__flag) throw gcnew ObjectDisposedException(gcnew String(__FUNCTION__));

//-----------------------------------------------------------------------------
// CLRASSERT
//
// Used in place of directly calling Debug::Assert() in the code to ensure that
// it won't fire for RELEASE builds.  C++/CLI does not define the necessary DEBUG
// ConditionalAttribute to suppress it

#ifdef _DEBUG
#define CLRASSERT(condition, ...) System::Diagnostics::Debug::Assert(condition, ##__VA_ARGS__)
#else
#define CLRASSERT(condition, ...)
#endif

//---------------------------------------------------------------------------
// CLRISNULL / CLRISNOTNULL
//
// Shorthand macros for the often-used Object::ReferenceEquals(o, nullptr)

#define CLRISNULL(__object) (Object::ReferenceEquals(__object, nullptr))
#define CLRISNOTNULL(__object) (!Object::ReferenceEquals(__object, nullptr))

//---------------------------------------------------------------------------
// Win32 Declarations

#define	WINVER				_WIN32_WINNT_WIN7
#define	_WIN32_WINNT		_WIN32_WINNT_WIN7

#include <WinSock2.h>				// Include Windows Sockets declarations
#include <Windows.h>				// Include main Windows declarations
#include <netlistmgr.h>				// Include network list manager decls
#include <msclr\auto_handle.h>		// Include msclr::auto_handle<>
#include <msclr\lock.h>				// Include msclr::lock
#include <msclr\marshal.h>			// Include msclr::interop::marshal_as<>

//---------------------------------------------------------------------------
// libhdhomerun

#include <hdhomerun.h>				// Include HDHomeRun declarations

struct hdhomerun_device_t {};		// LNK4248 : Unresolved typeref token
struct hdhomerun_debug_t {};		// LNK4248 : Unresolved typeref token
struct hdhomerun_channel_list_t {};	// LNK4248 : Unresolved typeref token

//---------------------------------------------------------------------------

#endif	// __STDAFX_H_
