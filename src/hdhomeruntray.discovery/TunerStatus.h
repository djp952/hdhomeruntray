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

#ifndef __TUNERSTATUS_H_
#define __TUNERSTATUS_H_
#pragma once

#include "DeviceStatusColor.h"

#pragma warning(push, 4)

using namespace System;
using namespace System::Drawing;
using namespace System::Net;

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

	// Channel
	//
	// Gets the tuned channel string (modulation+frequency)
	property String^ Channel
	{
		String^ get(void);
	}

	// HasVirtualChannel
	//
	// Gets a flag indicating if the virtual channel is set or not
	property bool HasVirtualChannel
	{
		bool get(void);
	}

	// IsActive
	//
	// Gets a flag indicating if the tuner is active or not
	property bool IsActive
	{
		bool get(void);
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
	property Color SignalQualityColor
	{
		Color get(void);
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
	property Color SignalStrengthColor
	{
		Color get(void);
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
	property Color SymbolQualityColor
	{
		Color get(void);
	}

	// TargetIP
	//
	// Gets the target IP address of the tuner
	property IPAddress^ TargetIP
	{
		IPAddress^ get(void);
	}

	// VirtualChannelName
	//
	// Gets the tuned virtual channel name
	property String^ VirtualChannelName
	{
		String^ get(void);
	}

	// VirtualChannelNumber
	//
	// Gets the tuned virtual channel number
	property String^ VirtualChannelNumber
	{
		String^ get(void);
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
	// Internal Member Functions

	// Create
	//
	// Creates a new TunerStatus instance
	static TunerStatus^ Create(TunerDevice^ tunerdevice, int index);

private:

	// Instance Constructors
	//
	TunerStatus(void);
	TunerStatus(struct hdhomerun_tuner_status_t const* status, IPAddress^ targetip);
	TunerStatus(struct hdhomerun_tuner_status_t const* status, struct hdhomerun_tuner_vstatus_t const* vstatus, IPAddress^ targetip);

	//-----------------------------------------------------------------------
	// Private Member Functions

	// ConvertHDHomeRunColor
	//
	// Converts the color code from libhdhomerun into the color I want to use
	static Color ConvertHDHomeRunColor(uint32_t color);

	// GetVirtualChannelName
	//
	// Retrieves the virtual channel name from a tuner vstatus string
	static String^ GetVirtualChannelName(String^ vstatus);

	//-----------------------------------------------------------------------
	// Member Variables

	String^				m_channel = gcnew String("none");
	int					m_signalquality = 0;
	Color				m_signalqualitycolor = DeviceStatusColor::Gray;
	int					m_signalstrength = 0;
	Color				m_signalstrengthcolor = DeviceStatusColor::Gray;
	int					m_symbolquality = 0;
	Color				m_symbolqualitycolor = DeviceStatusColor::Gray;
	bool				m_hasvirtualchannel = false;
	String^				m_virtualchannelnum = String::Empty;
	String^				m_virtualchannelname = String::Empty;
	int					m_bitrate = 0;
	IPAddress^			m_targetip = IPAddress::None;
};

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)

#endif	// __TUNERSTATUS_H_
