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

#ifndef __DEVICELIST_H_
#define __DEVICELIST_H_
#pragma once

#include "Device.h"
#include "DiscoveryMethod.h"

#pragma warning(push, 4)

using namespace System;
using namespace System::Collections::Generic;

namespace zuki::hdhomeruntray::discovery {

//---------------------------------------------------------------------------
// Class DeviceList
//
// Implements an IReadOnlyList<> based enumerable list of devices
//---------------------------------------------------------------------------

public ref class DeviceList : public IReadOnlyList<Device^>
{
public:

	//-----------------------------------------------------------------------
	// Member Functions

	// GetEnumerator
	//
	// Returns a generic IEnumerator<T> for the member collection
	virtual IEnumerator<Device^>^ GetEnumerator(void);

	//-----------------------------------------------------------------------
	// Fields

	// Empty (static)
	//
	// Returns an empty device collection instance
	static initonly DeviceList^ Empty = gcnew DeviceList(gcnew List<Device^>());

	//-----------------------------------------------------------------------
	// Properties

	// default[int]
	//
	// Gets the element at the specified index in the read-only list
	property Device^ default[int]
	{
		virtual Device^ get(int index);
	}

	// Count
	//
	// Gets the number of elements in the collection
	property int Count
	{
		virtual int get();
	}

internal:

	// Create (static)
	//
	// Creates a new DeviceList instance
	static DeviceList^ Create(DiscoveryMethod method);

private:

	// Instance Constructor
	//
	DeviceList(List<Device^>^ devices);

	//-----------------------------------------------------------------------
	// Private Member Functions

	// DiscoverBroadcast (static)
	//
	// Executes a broadcast device discovery
	static DeviceList^ DiscoverBroadcast(void);

	// DiscoverHttp (static)
	//
	// Executes an HTTP device discovery
	static DeviceList^ DiscoverHttp(void);

	// GetEnumerator (IEnumerable)
	//
	// Returns a non-generic IEnumerator for the member collection
	virtual System::Collections::IEnumerator^ IEnumerable_GetEnumerator(void) sealed = System::Collections::IEnumerable::GetEnumerator;

	// TunerExists (static)
	//
	// Determines if a tuner device exists on the local network
	static bool TunerExists(String^ localip);
		
	//-----------------------------------------------------------------------
	// Member Variables

	List<Device^>^			m_devices;			// Underlying collection
};

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)

#endif	// __DEVICELIST_H_