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
//	device			- Reference to the unmanaged device information

TunerDevice::TunerDevice(struct hdhomerun_discover_device_v3_t const& device) : Device(device)
{
	m_deviceid = device.device_id.ToString("00000000");
	m_islegacy = device.is_legacy;
	m_lineupurl = gcnew String(device.lineup_url);
	m_tunercount = device.tuner_count;
}

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
	JToken^ islegacy = device->GetValue("Legacy", StringComparison::OrdinalIgnoreCase);
	JToken^ lineupurl = device->GetValue("LineupURL", StringComparison::OrdinalIgnoreCase);
	JToken^ tunercount = device->GetValue("TunerCount", StringComparison::OrdinalIgnoreCase);

	if(!Object::ReferenceEquals(deviceid, nullptr)) m_deviceid = deviceid->ToObject<String^>();
	m_islegacy = (!Object::ReferenceEquals(islegacy, nullptr) && (islegacy->ToObject<int>() == 1));
	if(!Object::ReferenceEquals(lineupurl, nullptr)) m_lineupurl = lineupurl->ToObject<String^>();
	m_tunercount = (!Object::ReferenceEquals(tunercount, nullptr)) ? tunercount->ToObject<int>() : 0;
}

//---------------------------------------------------------------------------
// TunerDevice::Create (internal)
//
// Creates a new TunerDevice instance
//
// Arguments:
//
//	device			- Reference to the unmanaged device information

TunerDevice^ TunerDevice::Create(struct hdhomerun_discover_device_v3_t const& device)
{
	return gcnew TunerDevice(device);
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
// TunerDevice::IsLegacy::get
//
// Get a flag indicating if the tuner device is a legacy device

bool TunerDevice::IsLegacy::get(void)
{
	return m_islegacy;
}

//---------------------------------------------------------------------------
// TunerDevice::LineupURL
//
// Gets the tuner lineup discovery URL

String^ TunerDevice::LineupURL::get(void)
{
	return m_lineupurl;
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
