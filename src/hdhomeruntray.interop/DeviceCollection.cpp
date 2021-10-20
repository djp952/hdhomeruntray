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

#include "stdafx.h"

#include "DeviceCollection.h"
#include "ReadOnlyListEnumerator.h"

#pragma warning(push, 4)

namespace zuki::hdhomeruntray::interop {

//---------------------------------------------------------------------------
// DeviceCollection Constructor (private)
//
// Arguments:
//
//	devices		- List<> containing the devices

DeviceCollection::DeviceCollection(List<Device^>^ devices) : m_devices(devices)
{
	if(Object::ReferenceEquals(devices, nullptr)) throw gcnew ArgumentNullException("profiles");
}

//---------------------------------------------------------------------------
// DeviceCollection::default[int]::get
//
// Gets the element at the specified index in the read-only list

Device^ DeviceCollection::default::get(int index)
{
	return m_devices[index];
}

//---------------------------------------------------------------------------
// DeviceCollection::Count::get
//
// Gets the number of elements in the collection

int DeviceCollection::Count::get(void)
{
	return m_devices->Count;
}

//---------------------------------------------------------------------------
// DeviceCollection::Discover (static, internal)
//
// Creates a new DeviceCollection instance by executing a discovery
//
// Arguments:
//
//  NONE

DeviceCollection^ DeviceCollection::Discover(void)
{
	return Discover(DiscoveryMethod::Broadcast);
}

//---------------------------------------------------------------------------
// DeviceCollection::Discover (static, internal)
//
// Creates a new DeviceCollection instance by executing a discovery
//
// Arguments:
//
//  method		- Discovery method to be used

DeviceCollection^ DeviceCollection::Discover(DiscoveryMethod method)
{
	return nullptr;
}

//---------------------------------------------------------------------------
// DeviceCollection::GetEnumerator
//
// Returns a generic IEnumerator<T> for the member collection
//
// Arguments:
//
//	NONE

IEnumerator<Device^>^ DeviceCollection::GetEnumerator(void)
{
	return gcnew ReadOnlyListEnumerator<Device^>(this);
}

//---------------------------------------------------------------------------
// DeviceCollection::IEnumerable_GetEnumerator
//
// Returns a non-generic IEnumerator for the member collection
//
// Arguments:
//
//	NONE

System::Collections::IEnumerator^ DeviceCollection::IEnumerable_GetEnumerator(void)
{
	return GetEnumerator();
}

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::interop

#pragma warning(pop)
