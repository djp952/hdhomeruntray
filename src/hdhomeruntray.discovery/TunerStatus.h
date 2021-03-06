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

#ifndef __TUNERSTATUS_H_
#define __TUNERSTATUS_H_
#pragma once

#include "DeviceStatus.h"
#include "DeviceStatusColor.h"

#pragma warning(push, 4)

using namespace System;
using namespace System::Net;

using _DeviceStatus = zuki::hdhomeruntray::discovery::DeviceStatus;

namespace zuki::hdhomeruntray::discovery {

// FORWARD DECLARATIONS
//
ref class TunerDevice;

//---------------------------------------------------------------------------
// Class TunerStatus
//
// Describes the status of an individual HDHomeRun device tuner
//---------------------------------------------------------------------------

public ref class TunerStatus
{
public:

	//-----------------------------------------------------------------------
	// Properties

	// BitRate
	//
	// Gets the current tuner output bitrate
	property int BitRate
	{
		int get(void);
	}

	// ChannelName
	//
	// Gets the tuned channel name
	property String^ ChannelName
	{
		String^ get(void);
	}

	// DeviceStatus
	//
	// Gets the overall device status
	property _DeviceStatus DeviceStatus
	{
		_DeviceStatus get(void);
	}

	// SignalQuality
	//
	// Gets the signal quality of the tuned channel
	property int SignalQuality
	{
		int get(void);
	}

	// SignalQualityColor
	//
	// Gets the color code for the signal quality
	property DeviceStatusColor SignalQualityColor
	{
		DeviceStatusColor get(void);
	}

	// SignalStrength
	//
	// Gets the signal strength of the tuned channel
	property int SignalStrength
	{
		int get(void);
	}

	// SignalStrengthColor
	//
	// Gets the color code for the signal strength
	property DeviceStatusColor SignalStrengthColor
	{
		DeviceStatusColor get(void);
	}

	// SymbolQuality
	//
	// Gets the symbol qualityh of the tuned channel
	property int SymbolQuality
	{
		int get(void);
	}

	// SymbolQualityColor
	//
	// Gets the color code for the symbol quality
	property DeviceStatusColor SymbolQualityColor
	{
		DeviceStatusColor get(void);
	}

	// TargetIP
	//
	// Gets the target IP address of the tuner
	property IPAddress^ TargetIP
	{
		IPAddress^ get(void);
	}

	//-----------------------------------------------------------------------
	// Fields

	// Empty
	//
	// Provides an empty TunerStatus instance
	static initonly TunerStatus^ Empty = gcnew TunerStatus();

	//-----------------------------------------------------------------------
	// Object Overrides

	// GetHashCode
	//
	// Serves as the default hash function
	virtual int GetHashCode(void) override;

internal:

	//-----------------------------------------------------------------------
	// Internal Fields

	// NoChannel
	//
	// String used by HDHomeRun tuners to indicate there is no tuned channel
	static initonly String^ NoChannel = gcnew String("none");

	//-----------------------------------------------------------------------
	// Internal Member Functions

	// Create
	//
	// Creates a new TunerStatus instance
	static TunerStatus^ Create(TunerDevice^ tunerdevice, int index);

private:

	// Instance Constructors
	//
	TunerStatus(void);
	TunerStatus(String^ channelname, struct hdhomerun_tuner_status_t const* status, IPAddress^ targetip);

	//-----------------------------------------------------------------------
	// Private Member Functions

	// StringFromUTF8 (static)
	//
	// Creates a System::String^ from an input UTF8 encoded char* string
	static String^ StringFromUTF8(char const* input);

	// GetVirtualChannelName
	//
	// Retrieves the virtual channel name from a tuner program and streaminfo
	static String^ GetVirtualChannelName(String^ program, String^ streaminfo);

	//-----------------------------------------------------------------------
	// Member Variables

	String^				m_channelname = NoChannel;
	int					m_signalquality = 0;
	DeviceStatusColor	m_signalqualitycolor = DeviceStatusColor::Neutral;
	int					m_signalstrength = 0;
	DeviceStatusColor	m_signalstrengthcolor = DeviceStatusColor::Neutral;
	int					m_symbolquality = 0;
	DeviceStatusColor	m_symbolqualitycolor = DeviceStatusColor::Neutral;
	int					m_bitrate = 0;
	IPAddress^			m_targetip = IPAddress::None;
	_DeviceStatus		m_devicestatus = _DeviceStatus::Idle;
};

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)

#endif	// __TUNERSTATUS_H_
