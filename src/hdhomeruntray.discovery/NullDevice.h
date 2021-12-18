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

#ifndef __NULLDEVICE_H_
#define __NULLDEVICE_H_
#pragma once

#include "Device.h"

#pragma warning(push, 4)

using namespace System;

namespace zuki::hdhomeruntray::discovery {

//---------------------------------------------------------------------------
// Class NullDevice
//
// Describes a null device (DeviceType::None), used by the GUI application
// in places where a device instance is needed but is not available
//---------------------------------------------------------------------------

public ref class NullDevice : Device
{
public:

	//-----------------------------------------------------------------------
	// Properties

	// FriendlyName
	//
	// Gets the device friendly name
	property String^ FriendlyName
	{
		virtual String^ get(void) override;
	}

	// Name
	//
	// Gets the device name (device/storage id)
	property String^ Name
	{
		virtual String^ get(void) override;
	}

internal:

	//-----------------------------------------------------------------------
	// Internal Member Functions

	// Create
	//
	// Creates a new NullDevice instance
	static NullDevice^ Create(void);

private:

	// Instance Constructors
	//
	NullDevice(void);

	//-----------------------------------------------------------------------
	// Member Variables

	String^				m_name;					// Device name
	String^				m_friendlyname;			// Device friendly name
};

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)

#endif	// __NULLDEVICE_H_
