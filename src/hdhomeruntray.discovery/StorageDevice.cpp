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
//	device			- Reference to the unmanaged device information

StorageDevice::StorageDevice(struct hdhomerun_discover_device_v3_t const& device) : Device(device)
{
	m_storageid = gcnew String(device.storage_id);
	m_storageurl = gcnew String(device.storage_url);
}

//---------------------------------------------------------------------------
// StorageDevice Constructor (private)
//
// Arguments:
//
//	device		- Reference to the JSON discovery data for the device

StorageDevice::StorageDevice(JObject^ device) : Device(device, zuki::hdhomeruntray::discovery::DeviceType::Storage)
{
	if(Object::ReferenceEquals(device, nullptr)) throw gcnew ArgumentNullException("device");

	JToken^ storageid = device->GetValue("StorageID", StringComparison::OrdinalIgnoreCase);
	JToken^ storageurl = device->GetValue("StorageURL", StringComparison::OrdinalIgnoreCase);

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
//	device			- Reference to the unmanaged device information

StorageDevice^ StorageDevice::Create(struct hdhomerun_discover_device_v3_t const& device)
{
	return gcnew StorageDevice(device);
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
