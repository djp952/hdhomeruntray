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

#ifndef __DEVICE_H_
#define __DEVICE_H_
#pragma once

#include "DeviceType.h"

#pragma warning(push, 4)

using namespace System;
using namespace System::Net;

namespace zuki::hdhomeruntray::interop {

//---------------------------------------------------------------------------
// Class Device (abstract)
//
// Describes an individual HDHomeRun device
//---------------------------------------------------------------------------

public ref class Device abstract
{
public:

	//-----------------------------------------------------------------------
	// Properties

	// BaseURL
	//
	// Gets the device web interface base URL
	property String^ BaseURL
	{
		String^ get(void);
	}

	// DeviceType
	//
	// Gets the device type identifier
	property zuki::hdhomeruntray::interop::DeviceType DeviceType
	{
		zuki::hdhomeruntray::interop::DeviceType get(void);
	}

	// IPAddress
	//
	// Get the IPv4 address of the device
	property System::Net::IPAddress^ IPAddress
	{
		System::Net::IPAddress^ get(void);
	}

protected:

	// Instance Constructor
	//
	Device(struct hdhomerun_discover_device_v3_t const* device);

	//-----------------------------------------------------------------------
	// Member Variables

	String^										m_baseurl;		// Device base URL string
	zuki::hdhomeruntray::interop::DeviceType	m_devicetype;	// Device type flag
	System::Net::IPAddress^						m_ipaddress;	// Device IPv4 address
};

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::interop

#pragma warning(pop)

#endif	// __DEVICE_H_
