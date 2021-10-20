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

#ifndef __TUNERDEVICE_H_
#define __TUNERDEVICE_H_
#pragma once

#include "Device.h"

#pragma warning(push, 4)

using namespace System;
using namespace System::Net;

namespace zuki::hdhomeruntray::interop {

//---------------------------------------------------------------------------
// Class TunerDevice
//
// Describes a HDHomeRun tuner device
//---------------------------------------------------------------------------

public ref class TunerDevice : Device
{
public:

	//-----------------------------------------------------------------------
	// Properties

	// DeviceID
	//
	// Gets the tuner device identifier
	property String^ DeviceID
	{
		String^ get(void);
	}

	// IsLegacy
	//
	// Get a flag indicating if the tuner device is a legacy device
	property bool IsLegacy
	{
		bool get(void);
	}

	// TunerCount
	//
	// Gets the number of individual device tuners on the device
	property int TunerCount
	{
		int get(void);
	}

internal:

	//-----------------------------------------------------------------------
	// Internal Member Functions

	// Create
	//
	// Creates a new TunerDevice instance
	static TunerDevice^ Create(struct hdhomerun_discover_device_v3_t const* device);

private:

	// Instance Constructor
	//
	TunerDevice(struct hdhomerun_discover_device_v3_t const* device);

	//-----------------------------------------------------------------------
	// Member Variables

	String^					m_deviceid;			// Tuner device identifier
	bool					m_islegacy;			// Legacy tuner device flag
	int						m_tunercount;		// Number of device tuners
};

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::interop

#pragma warning(pop)

#endif	// __TUNERDEVICE_H_
