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
//	localip		- IP address of the storage device

StorageDevice::StorageDevice(JObject^ device, IPAddress^ localip) : Device(device, localip, _DeviceType::Storage), 
	m_totalspace(0), m_freespace(0)
{
	if(CLRISNULL(device)) throw gcnew ArgumentNullException("device");

	JToken^ friendlyname = device->GetValue("FriendlyName", StringComparison::OrdinalIgnoreCase);
	JToken^ storageid = device->GetValue("StorageID", StringComparison::OrdinalIgnoreCase);
	JToken^ storageurl = device->GetValue("StorageURL", StringComparison::OrdinalIgnoreCase);
	JToken^ version = device->GetValue("Version", StringComparison::OrdinalIgnoreCase);
	JToken^ totalspace = device->GetValue("TotalSpace", StringComparison::OrdinalIgnoreCase);
	JToken^ freespace = device->GetValue("FreeSpace", StringComparison::OrdinalIgnoreCase);

	m_friendlyname = CLRISNOTNULL(friendlyname) ? friendlyname->ToObject<String^>() : String::Empty;
	m_storageid = CLRISNOTNULL(storageid) ? storageid->ToObject<String^>() : String::Empty;
	m_storageurl = CLRISNOTNULL(storageurl) ? storageurl->ToObject<String^>() : String::Empty;
	m_version = CLRISNOTNULL(version) ? version->ToObject<String^>() : String::Empty;

	if(CLRISNOTNULL(totalspace)) int64_t::TryParse(totalspace->ToObject<String^>(), m_totalspace);
	if(CLRISNOTNULL(freespace)) int64_t::TryParse(freespace->ToObject<String^>(), m_freespace);
}

//---------------------------------------------------------------------------
// StorageDevice::Create (internal)
//
// Creates a new StorageDevice instance
//
// Arguments:
//
//	device		- Reference to the JSON discovery data for the device
//	localip		- IP address of the storage device

StorageDevice^ StorageDevice::Create(JObject^ device, IPAddress^ localip)
{
	if(CLRISNULL(device)) throw gcnew ArgumentNullException("device");
	return gcnew StorageDevice(device, localip);
}

//---------------------------------------------------------------------------
// StorageDevice::FreeSpace::get
//
// Gets the free storage space

int64_t StorageDevice::FreeSpace::get(void)
{
	return m_freespace;
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
// StorageDevice::GetStorageStatus
//
// Gets the detailed status information for a storage device
//
// Arguments:
//
//	NONE

StorageStatus^ StorageDevice::GetStorageStatus(void)
{
	return StorageStatus::Create(this);
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
// StorageDevice::TotalSpace::get
//
// Gets the total storage space

int64_t StorageDevice::TotalSpace::get(void)
{
	return m_totalspace;
}

//---------------------------------------------------------------------------
// StorageDevice::Version::get
//
// Gets the storage device software version

String^ StorageDevice::Version::get(void)
{
	return m_version;
}

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)
