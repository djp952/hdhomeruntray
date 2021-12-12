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
#include "Tuner.h"
#include "TunerList.h"
#include "TunerStatus.h"

#pragma warning(push, 4)

using namespace System;
using namespace System::Net;

using namespace Newtonsoft::Json;
using namespace Newtonsoft::Json::Linq;

namespace zuki::hdhomeruntray::discovery {

//---------------------------------------------------------------------------
// Class TunerDevice
//
// Describes a HDHomeRun tuner device
//---------------------------------------------------------------------------

public ref class TunerDevice : Device
{
public:

	//-----------------------------------------------------------------------
	// Member Functions

	// GetTunerStatus
	//
	// Gets the detailed status information for a tuner
	TunerStatus^ GetTunerStatus(int index);
	TunerStatus^ GetTunerStatus(Tuner^ tuner);

	//-----------------------------------------------------------------------
	// Properties

	// DeviceID
	//
	// Gets the tuner device identifier
	property String^ DeviceID
	{
		String^ get(void);
	}

	// FirmwareName
	//
	// Gets the tuner device firmware name
	property String^ FirmwareName
	{
		String^ get(void);
	}

	// FirmwareVersion
	//
	// Gets the tuner device firmware version
	property String^ FirmwareVersion
	{
		String^ get(void);
	}

	// FriendlyName
	//
	// Gets the device friendly name
	property String^ FriendlyName
	{
		virtual String^ get(void) override;
	}

	// ModelNumber
	//
	// Gets the device model number
	property String^ ModelNumber
	{
		String^ get(void);
	}

	// Name
	//
	// Gets the device name (device/storage id)
	property String^ Name
	{
		virtual String^ get(void) override;
	}

	// IsLegacy
	//
	// Get a flag indicating if the tuner device is a legacy device
	property bool IsLegacy
	{
		bool get(void);
	}

	// Tuners
	//
	// Accesses the collection of Tuner objects
	property TunerList^ Tuners
	{
		TunerList^ get(void);
	}

internal:

	//-----------------------------------------------------------------------
	// Internal Member Functions

	// Create
	//
	// Creates a new TunerDevice instance
	static TunerDevice^ Create(JObject^ device, IPAddress^ localip);

private:

	// Instance Constructors
	//
	TunerDevice(JObject^ device, IPAddress^ localip);

	//-----------------------------------------------------------------------
	// Member Variables

	String^					m_deviceid;			// Tuner device identifier
	String^					m_friendlyname;		// Tuner device friendly name
	bool					m_islegacy;			// Legacy tuner device flag
	String^					m_modelnumber;		// Model number string
	String^					m_firmwarename;		// Firmware name string
	String^					m_firmwareversion;	// Firmware version string
	int						m_tunercount;		// Number of tuner instances
	TunerList^				m_tuners;			// List<> of tuner instances
};

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)

#endif	// __TUNERDEVICE_H_
