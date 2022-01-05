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

#ifndef __DEVICESTATUSCOLOR_H_
#define __DEVICESTATUSCOLOR_H_
#pragma once

#pragma warning(push, 4)

#include "DeviceStatus.h"

using namespace System;
using namespace System::Drawing;

using _DeviceStatus = zuki::hdhomeruntray::discovery::DeviceStatus;

namespace zuki::hdhomeruntray::discovery {

//---------------------------------------------------------------------------
// Class DeviceStatusColor
//
// Defines constant Color declarations for device functions
//---------------------------------------------------------------------------

public ref class DeviceStatusColor abstract sealed
{
public:

	//-----------------------------------------------------------------------
	// Member Functions
	//-----------------------------------------------------------------------

	// FromDeviceStatus (static)
	//
	// Determines the color from a DeviceStatus enumeration value
	static Color FromDeviceStatus(_DeviceStatus devicestatus);

	//-----------------------------------------------------------------------
	// Fields
	//-----------------------------------------------------------------------

	// Gray
	//
	static initonly Color Gray = Color::FromArgb(0xFFC0C0C0);

	// Green
	//
	static initonly Color Green = Color::FromArgb(0xFF1EE500);

	// Red
	//
	static initonly Color Red = Color::FromArgb(0xFFE50000);

	// Yellow
	//
	static initonly Color Yellow = Color::FromArgb(0xFFFFE900);
};

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)

#endif	// __DEVICESTATUSCOLOR_H_
