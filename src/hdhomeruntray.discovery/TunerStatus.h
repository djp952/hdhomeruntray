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

#pragma warning(push, 4)

using namespace System;
using namespace System::Drawing;

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

	// Channel
	//
	// Gets the tuned channel string (modulation+frequency)
	property String^ Channel
	{
		String^ get(void);
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

internal:

	//-----------------------------------------------------------------------
	// Internal Member Functions

	// Create
	//
	// Creates a new TunerStatus instance
	static TunerStatus^ Create(TunerDevice^ tunerdevice, int index);

private:

	// Instance Constructor
	//
	TunerStatus(struct hdhomerun_tuner_status_t const* status);

	//-----------------------------------------------------------------------
	// Member Variables

	String^				m_channel;				// Tuned channel
	int					m_signalquality;		// Signal quality
	Color				m_signalqualitycolor;	// Signal quality color
	int					m_signalstrength;		// Signal strength
	Color				m_signalstrengthcolor;	// Signal strength color
	int					m_symbolquality;		// Symbol quality
	Color				m_symbolqualitycolor;	// Symbol quality color
};

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)

#endif	// __TUNERSTATUS_H_
