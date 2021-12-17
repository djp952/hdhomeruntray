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

#include "DeviceStatus.h"
#include "LiveBufferList.h"
#include "PlaybackList.h"
#include "RecordingList.h"

#pragma warning(push, 4)

using namespace System;
using namespace System::Drawing;

using _DeviceStatus = zuki::hdhomeruntray::discovery::DeviceStatus;

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

	// DeviceStatus
	//
	// Gets the overall device status
	property _DeviceStatus DeviceStatus
	{
		_DeviceStatus get(void);
	}

	// LiveBuffers
	//
	// Gets the collection of active live buffers
	property LiveBufferList^ LiveBuffers
	{
		LiveBufferList^ get(void);
	}

	// Playbacks
	//
	// Gets the collection of active playbacks
	property PlaybackList^ Playbacks
	{
		PlaybackList^ get(void);
	}

	// Recordings
	//
	// Gets the collection of active recordings
	property RecordingList^ Recordings
	{
		RecordingList^ get(void);
	}

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
	// Creates a new StorageStatus instance
	static StorageStatus^ Create(StorageDevice^ storagedevice);

private:

	// Instance Constructor
	//
	StorageStatus(LiveBufferList^ livebuffers, PlaybackList^ playbacks, RecordingList^ recordings);

	//-----------------------------------------------------------------------
	// Member Variables

	LiveBufferList^		m_livebuffers = nullptr;
	PlaybackList^		m_playbacks = nullptr;
	RecordingList^		m_recordings = nullptr;
	_DeviceStatus		m_devicestatus = _DeviceStatus::Idle;
};

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)

#endif	// __STORAGESTATUS_H_
