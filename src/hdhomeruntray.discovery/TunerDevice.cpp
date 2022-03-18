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

#include "TunerDevice.h"

#pragma warning(push, 4)

using namespace System::Globalization;

namespace zuki::hdhomeruntray::discovery {

//---------------------------------------------------------------------------
// TunerDevice Constructor (private)
//
// Arguments:
//
//	device		- Reference to the JSON discovery data for the device
//	localip		- The IP address of the device

TunerDevice::TunerDevice(JObject^ device, IPAddress^ localip) : Device(device, localip, _DeviceType::Tuner)
{
	if(CLRISNULL(device)) throw gcnew ArgumentNullException("device");

	JToken^ deviceid = device->GetValue("DeviceID", StringComparison::OrdinalIgnoreCase);
	JToken^ friendlyname = device->GetValue("FriendlyName", StringComparison::OrdinalIgnoreCase);
	JToken^ modelnumber = device->GetValue("ModelNumber", StringComparison::OrdinalIgnoreCase);
	JToken^ firmwarename = device->GetValue("FirmwareName", StringComparison::OrdinalIgnoreCase);
	JToken^ firmwareversion = device->GetValue("FirmwareVersion", StringComparison::OrdinalIgnoreCase);
	JToken^ islegacy = device->GetValue("Legacy", StringComparison::OrdinalIgnoreCase);
	JToken^ tunercount = device->GetValue("TunerCount", StringComparison::OrdinalIgnoreCase);
	JToken^ upgradeavailable = device->GetValue("UpgradeAvailable", StringComparison::OrdinalIgnoreCase);

	m_deviceid = CLRISNOTNULL(deviceid) ? deviceid->ToObject<String^>() : String::Empty;
	m_friendlyname = CLRISNOTNULL(friendlyname) ? friendlyname->ToObject<String^>() : String::Empty;
	m_modelnumber = CLRISNOTNULL(modelnumber) ? modelnumber->ToObject<String^>() : String::Empty;
	m_firmwarename = CLRISNOTNULL(firmwarename) ? firmwarename->ToObject<String^>() : String::Empty;
	m_firmwareversion = CLRISNOTNULL(firmwareversion) ? firmwareversion->ToObject<String^>() : String::Empty;
	m_islegacy = (CLRISNOTNULL(islegacy) && (islegacy->ToObject<int>() == 1));
	m_tunercount = CLRISNOTNULL(tunercount) ? tunercount->ToObject<int>() : 0;
	m_upgradeavailable = CLRISNOTNULL(upgradeavailable);

	// Create a collection from which the individual tuner objects can be accessed
	m_tuners = TunerList::Create(m_tunercount);
}

//---------------------------------------------------------------------------
// TunerDevice::Create (internal)
//
// Creates a new TunerDevice instance
//
// Arguments:
//
//	device		- Reference to the JSON discovery data for the device
//	localip		- The IP address of the device

TunerDevice^ TunerDevice::Create(JObject^ device, IPAddress^ localip)
{
	if(CLRISNULL(device)) throw gcnew ArgumentNullException();
	return gcnew TunerDevice(device, localip);
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
// TunerDevice::FirmwareName::get
//
// Gets the tuner device firmware name

String^ TunerDevice::FirmwareName::get(void)
{
	return m_firmwarename;
}

//---------------------------------------------------------------------------
// TunerDevice::FirmwareUpdateAvailable
//
// Flag if there is a firmware update available for the device

bool TunerDevice::FirmwareUpdateAvailable::get(void)
{
	return m_upgradeavailable;
}

//---------------------------------------------------------------------------
// TunerDevice::FirmwareVersion::get
//
// Gets the tuner device firmware version

String^ TunerDevice::FirmwareVersion::get(void)
{
	return m_firmwareversion;
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
// TunerDevice::ModelNumber::get
//
// Gets the tuner device model number

String^ TunerDevice::ModelNumber::get(void)
{
	return m_modelnumber;
}

//---------------------------------------------------------------------------
// TunerDevice::GetTunerStatus
//
// Gets the detailed status information for a tuner
//
// Arguments:
//
//	index		- Index of the tuner within the device

TunerStatus^ TunerDevice::GetTunerStatus(int index)
{
	if((index < 0) || (index >= m_tunercount)) throw gcnew ArgumentOutOfRangeException("index");

	return TunerStatus::Create(this, index);
}

//---------------------------------------------------------------------------
// TunerDevice::GetTunerStatus
//
// Gets the detailed status information for a tuner
//
// Arguments:
//
//	tuner		- Tuner instance to get the index from

TunerStatus^ TunerDevice::GetTunerStatus(Tuner^ tuner)
{
	if(CLRISNULL(tuner)) throw gcnew ArgumentNullException("tuner");
	
	return GetTunerStatus(tuner->Index);
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
// TunerDevice::Reboot
//
// Reboots the tuner device
//
// Arguments:
//
//	NONE

void TunerDevice::Reboot(void)
{
	// Convert the device ID into an unsigned 32-bit integer
	uint32_t deviceid = 0;
	if(uint32_t::TryParse(DeviceID, NumberStyles::HexNumber, CultureInfo::InvariantCulture, deviceid)) {

		// Convert the IP address into a byte array; convert for little endian as necessary
		array<Byte>^ ipbytes = LocalIP->GetAddressBytes();
		if(BitConverter::IsLittleEndian) Array::Reverse(ipbytes);

		// Attempt to create a hdhomerun_device_t instance for tuner instance 0
		hdhomerun_device_t* device = hdhomerun_device_create(deviceid, BitConverter::ToUInt32(ipbytes, 0), 0, nullptr);
		if(device != nullptr) {

			// Attempt to reboot the device by sending the "sys/restart self" command to it
			hdhomerun_device_set_var(device, "/sys/restart", "self", nullptr, nullptr);

			// Release the hdhomerun_device_t instance
			hdhomerun_device_destroy(device);
		}
	}
}

//---------------------------------------------------------------------------
// TunerDevice::ResetTuner
//
// Resets a tuner
//
// Arguments:
//
//	index		- Index of the tuner to reset

void TunerDevice::ResetTuner(int index)
{
	// Convert the device ID into an unsigned 32-bit integer
	uint32_t deviceid = 0;
	if(uint32_t::TryParse(DeviceID, NumberStyles::HexNumber, CultureInfo::InvariantCulture, deviceid)) {

		// Convert the IP address into a byte array; convert for little endian as necessary
		array<Byte>^ ipbytes = LocalIP->GetAddressBytes();
		if(BitConverter::IsLittleEndian) Array::Reverse(ipbytes);

		// Attempt to create a hdhomerun_device_t instance for tuner instance 0
		hdhomerun_device_t* device = hdhomerun_device_create(deviceid, BitConverter::ToUInt32(ipbytes, 0), 0, nullptr);
		if(device != nullptr) {

			char name[256] = {};			// Variable name

			// Unlock the specified tuner
			sprintf_s(name, std::extent<decltype(name)>::value, "/tuner%d/lockkey", index);
			hdhomerun_device_set_var(device, name, "force", nullptr, nullptr);

			// Reset the specified tuner
			sprintf_s(name, std::extent<decltype(name)>::value, "/tuner%d/channel", index);
			hdhomerun_device_set_var(device, name, "none", nullptr, nullptr);

			// Release the hdhomerun_device_t instance
			hdhomerun_device_destroy(device);
		}
	}
}

//---------------------------------------------------------------------------
// TunerDevice::Tuners::get
//
// Accesses the collection of tuner objects

TunerList^ TunerDevice::Tuners::get(void)
{
	CLRASSERT(CLRISNOTNULL(m_tuners));

	return m_tuners;
}

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)
