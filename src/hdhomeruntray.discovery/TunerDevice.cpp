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

#include "TunerDevice.h"

#pragma warning(push, 4)

namespace zuki::hdhomeruntray::discovery {

//---------------------------------------------------------------------------
// TunerDevice Constructor (private)
//
// Arguments:
//
//	device		- Reference to the JSON discovery data for the device

TunerDevice::TunerDevice(JObject^ device) : Device(device, zuki::hdhomeruntray::discovery::DeviceType::Tuner)
{
	if(Object::ReferenceEquals(device, nullptr)) throw gcnew ArgumentNullException("device");

	JToken^ deviceid = device->GetValue("DeviceID", StringComparison::OrdinalIgnoreCase);
	JToken^ friendlyname = device->GetValue("FriendlyName", StringComparison::OrdinalIgnoreCase);
	JToken^ islegacy = device->GetValue("Legacy", StringComparison::OrdinalIgnoreCase);
	JToken^ tunercount = device->GetValue("TunerCount", StringComparison::OrdinalIgnoreCase);

	if(!Object::ReferenceEquals(deviceid, nullptr)) m_deviceid = deviceid->ToObject<String^>();
	if(!Object::ReferenceEquals(friendlyname, nullptr)) m_friendlyname = friendlyname->ToObject<String^>();
	m_islegacy = (!Object::ReferenceEquals(islegacy, nullptr) && (islegacy->ToObject<int>() == 1));
	m_tunercount = (!Object::ReferenceEquals(tunercount, nullptr)) ? tunercount->ToObject<int>() : 0;
}

//---------------------------------------------------------------------------
// StorageDevice::Create (internal)
//
// Creates a new StorageDevice instance
//
// Arguments:
//
//	device		- Reference to the JSON discovery data for the device

TunerDevice^ TunerDevice::Create(JObject^ device)
{
	if(Object::ReferenceEquals(device, nullptr)) throw gcnew ArgumentNullException();
	return gcnew TunerDevice(device);
}

//---------------------------------------------------------------------------
// TunerDevice::DeviceID::get
//
// Gets the tuner device identifier

String^ TunerDevice::DeviceID::get(void)
{
	return m_deviceid;
}

//---------------------------------------------------------------------------
// TunerDevice::FriendlyName::get
//
// Gets the tuner device friendly name

String^ TunerDevice::FriendlyName::get(void)
{
	return m_friendlyname;
}

//---------------------------------------------------------------------------
// TunerDevice::Name::get
//
// Gets the tuner device name

String^ TunerDevice::Name::get(void)
{
	return m_deviceid;
}

//---------------------------------------------------------------------------
// TunerDevice::IsLegacy::get
//
// Get a flag indicating if the tuner device is a legacy device

bool TunerDevice::IsLegacy::get(void)
{
	return m_islegacy;
}

//---------------------------------------------------------------------------
// TunerDevice::TunerCount::get
//
// Gets the number of individual device tuners on the device

int TunerDevice::TunerCount::get(void)
{
	return m_tunercount;
}

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)
