//---------------------------------------------------------------------------
// Copyright (c) 2021 Michael G. Brehm
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

#ifndef __STORAGESTATUS_H_
#define __STORAGESTATUS_H_
#pragma once

#include "Recording.h"

#pragma warning(push, 4)

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Drawing;

namespace zuki::hdhomeruntray::discovery {

// FORWARD DECLARATIONS
//
ref class StorageDevice;

//---------------------------------------------------------------------------
// Class StorageStatus
//
// Describes the status of an individual HDHomeRun RECORD engine
//---------------------------------------------------------------------------

public ref class StorageStatus
{
public:

	//-----------------------------------------------------------------------
	// Properties

	// StatusColor
	//
	// Gets the color code for the overall status
	property Color StatusColor
	{
		Color get(void);
	}

internal:

	//-----------------------------------------------------------------------
	// Internal Member Functions

	// Create
	//
	// Creates a new StorageStatus instance
	static StorageStatus^ Create(StorageDevice^ storagedevice);

private:

	// Instance Constructor
	//
	StorageStatus(List<String^>^ livebuffers, List<Recording^>^ recordings);

	//-----------------------------------------------------------------------
	// Private Constants

	// COLOR_XXXX
	//
	// Replacement colors for the default values from libhdhomerun
	literal uint32_t COLOR_GREEN = 0xFF1EE500;
	literal uint32_t COLOR_RED = 0xFFE50000;
	literal uint32_t COLOR_GRAY = 0xFFC0C0C0;

	//-----------------------------------------------------------------------
	// Member Variables

	uint32_t			m_statuscolor;			// Overall status color
};

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)

#endif	// __STORAGESTATUS_H_
