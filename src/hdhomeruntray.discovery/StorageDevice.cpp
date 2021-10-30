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

#include "StorageDevice.h"

#pragma warning(push, 4)

namespace zuki::hdhomeruntray::discovery {

//---------------------------------------------------------------------------
// StorageDevice Constructor (private)
//
// Arguments:
//
//	device		- Reference to the JSON discovery data for the device

StorageDevice::StorageDevice(JObject^ device) : Device(device, zuki::hdhomeruntray::discovery::DeviceType::Storage)
{
	if(Object::ReferenceEquals(device, nullptr)) throw gcnew ArgumentNullException("device");

	JToken^ friendlyname = device->GetValue("FriendlyName", StringComparison::OrdinalIgnoreCase);
	JToken^ storageid = device->GetValue("StorageID", StringComparison::OrdinalIgnoreCase);
	JToken^ storageurl = device->GetValue("StorageURL", StringComparison::OrdinalIgnoreCase);

	if(!Object::ReferenceEquals(friendlyname, nullptr)) m_friendlyname = friendlyname->ToObject<String^>();
	if(!Object::ReferenceEquals(storageid, nullptr)) m_storageid = storageid->ToObject<String^>();
	if(!Object::ReferenceEquals(storageurl, nullptr)) m_storageurl = storageurl->ToObject<String^>();
}

//---------------------------------------------------------------------------
// StorageDevice::Create (internal)
//
// Creates a new StorageDevice instance
//
// Arguments:
//
//	device		- Reference to the JSON discovery data for the device

StorageDevice^ StorageDevice::Create(JObject^ device)
{
	if(Object::ReferenceEquals(device, nullptr)) throw gcnew ArgumentNullException();
	return gcnew StorageDevice(device);
}

//---------------------------------------------------------------------------
// StorageDevice::FriendlyName::get
//
// Gets the storage device friendly name

String^ StorageDevice::FriendlyName::get(void)
{
	return m_friendlyname;
}

//---------------------------------------------------------------------------
// StorageDevice::Name::get
//
// Gets the storage device name

String^ StorageDevice::Name::get(void)
{
	return m_storageid;
}

//---------------------------------------------------------------------------
// StorageDevice::StorageID::get
//
// Gets the storage device identifier

String^ StorageDevice::StorageID::get(void)
{
	return m_storageid;
}

//---------------------------------------------------------------------------
// StorageDevice::StorageURL::get
//
// Gets the storage device data URL

String^ StorageDevice::StorageURL::get(void)
{
	return m_storageurl;
}

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)
