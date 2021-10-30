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

#ifndef __STORAGEDEVICE_H_
#define __STORAGEDEVICE_H_
#pragma once

#include "Device.h"

#pragma warning(push, 4)

using namespace System;
using namespace System::Net;

using namespace Newtonsoft::Json;
using namespace Newtonsoft::Json::Linq;

namespace zuki::hdhomeruntray::discovery {

//---------------------------------------------------------------------------
// Class StorageDevice
//
// Describes a HDHomeRun storage (DVR) device
//---------------------------------------------------------------------------

public ref class StorageDevice : Device
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

	// StorageID
	//
	// Gets the storage device identifier
	property String^ StorageID
	{
		String^ get(void);
	}

	// StorageURL
	//
	// Gets the storage device data URL
	property String^ StorageURL
	{
		String^ get(void);
	}

internal:

	//-----------------------------------------------------------------------
	// Internal Member Functions

	// Create
	//
	// Creates a new StorageDevice instance
	static StorageDevice^ Create(JObject^ device);

private:

	// Instance Constructor
	//
	StorageDevice(JObject^ device);

	//-----------------------------------------------------------------------
	// Member Variables

	String^					m_friendlyname;		// Storage device friendly name
	String^					m_storageid;		// Storage device identifier
	String^					m_storageurl;		// Storage device data URL
};

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)

#endif	// __STORAGEDEVICE_H_
