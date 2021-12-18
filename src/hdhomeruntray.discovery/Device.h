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

using namespace Newtonsoft::Json;
using namespace Newtonsoft::Json::Linq;

using _DeviceType = zuki::hdhomeruntray::discovery::DeviceType;

namespace zuki::hdhomeruntray::discovery {

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

	// FriendlyName
	//
	// Gets the device friendly name
	property String^ FriendlyName
	{
		virtual String^ get(void) abstract;
	}

	// LocalIP
	//
	// Gets the device IP address
	property IPAddress^ LocalIP
	{
		IPAddress^ get(void);
	}

	// Name
	//
	// Gets the device name (device/storage id)
	property String^ Name
	{
		virtual String^ get(void) abstract;
	}

	// DeviceType
	//
	// Gets the device type identifier
	property _DeviceType Type
	{
		_DeviceType get(void);
	}

protected:

	// Instance Constructor
	//
	Device(_DeviceType type);
	Device(JObject^ device, IPAddress^ localip, _DeviceType type);

	//-----------------------------------------------------------------------
	// Member Variables

	String^					m_baseurl;			// Device base URL string
	IPAddress^				m_localip;			// Device IP address
	_DeviceType				m_devicetype;		// Device type flag
};

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)

#endif	// __DEVICE_H_
