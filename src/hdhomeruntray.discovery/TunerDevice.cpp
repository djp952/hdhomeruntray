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

using namespace System::Globalization;

namespace zuki::hdhomeruntray::discovery {

//---------------------------------------------------------------------------
// TunerDevice Constructor (private)
//
// Arguments:
//
//	device		- Reference to the JSON discovery data for the device
//	localip		- The IP address of the device

TunerDevice::TunerDevice(JObject^ device, IPAddress^ localip) : Device(device, localip, zuki::hdhomeruntray::discovery::DeviceType::Tuner)
{
	if(CLRISNULL(device)) throw gcnew ArgumentNullException("device");

	JToken^ deviceid = device->GetValue("DeviceID", StringComparison::OrdinalIgnoreCase);
	JToken^ friendlyname = device->GetValue("FriendlyName", StringComparison::OrdinalIgnoreCase);
	JToken^ modelnumber = device->GetValue("ModelNumber", StringComparison::OrdinalIgnoreCase);
	JToken^ firmwarename = device->GetValue("FirmwareName", StringComparison::OrdinalIgnoreCase);
	JToken^ firmwareversion = device->GetValue("FirmwareVersion", StringComparison::OrdinalIgnoreCase);
	JToken^ islegacy = device->GetValue("Legacy", StringComparison::OrdinalIgnoreCase);
	JToken^ tunercount = device->GetValue("TunerCount", StringComparison::OrdinalIgnoreCase);

	m_deviceid = CLRISNOTNULL(deviceid) ? deviceid->ToObject<String^>() : String::Empty;
	m_friendlyname = CLRISNOTNULL(friendlyname) ? friendlyname->ToObject<String^>() : String::Empty;
	m_modelnumber = CLRISNOTNULL(modelnumber) ? modelnumber->ToObject<String^>() : String::Empty;
	m_firmwarename = CLRISNOTNULL(firmwarename) ? firmwarename->ToObject<String^>() : String::Empty;
	m_firmwareversion = CLRISNOTNULL(firmwareversion) ? firmwareversion->ToObject<String^>() : String::Empty;
	m_islegacy = (CLRISNOTNULL(islegacy) && (islegacy->ToObject<int>() == 1));
	m_tunercount = CLRISNOTNULL(tunercount) ? tunercount->ToObject<int>() : 0;

	// Create a collection from which the individual tuner objects can be accessed
	m_tuners = TunerList::Create(m_tunercount);

	// Create an array that can be used to cache the last channel name and
	// target IP address 
	// for a tuner's status to allow it to optimize a refresh
	m_statuscache = gcnew array<Tuple<String^, String^, IPAddress^>^>(m_tunercount);
	for(int index = 0; index < m_tunercount; index++)
		m_statuscache[index] = gcnew Tuple<String^, String^, IPAddress^>(TunerStatus::NoChannel, TunerStatus::NoChannel, IPAddress::None);
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

	// Convert the device ID into an unsigned 32-bit integer
	uint32_t deviceid = 0;
	if(!uint32_t::TryParse(DeviceID, NumberStyles::HexNumber, CultureInfo::InvariantCulture, deviceid))
		throw gcnew ArgumentOutOfRangeException("deviceid");

	// Convert the IP address into a byte array; convert for little endian as necessary
	array<Byte>^ ipbytes = LocalIP->GetAddressBytes();
	if(BitConverter::IsLittleEndian) Array::Reverse(ipbytes);

	// Attempt to create a hdhomerun_device_t instance for the tuner instance
	hdhomerun_device_t* device = hdhomerun_device_create(deviceid, BitConverter::ToUInt32(ipbytes, 0), index, nullptr);
	if(device == nullptr) return TunerStatus::Empty;

	try {

		// Get the general status for the tuner device
		struct hdhomerun_tuner_status_t status = {};
		int result = hdhomerun_device_get_tuner_status(device, nullptr, &status);
		if(result != 1) return TunerStatus::Empty;

		// Create a marshaling context to convert from String^ to unmanaged strings
		msclr::auto_handle<msclr::interop::marshal_context> context(gcnew msclr::interop::marshal_context());

		//
		// CACHED METADATA
		//

		// Compare the status (modulation::frequency) channel name against the cache
		String^ channel = gcnew String(status.channel);
		if(String::Compare(channel, m_statuscache[index]->Item1, StringComparison::OrdinalIgnoreCase) == 0) {

			// Use the cached channel name string and target IP address to bypass re-figuring all that out
			strncpy_s(status.channel, std::extent<decltype(status.channel)>::value, context->marshal_as<char const*>(m_statuscache[index]->Item2), _TRUNCATE);
			return TunerStatus::Create(&status, m_statuscache[index]->Item3);
		}

		//
		// RESET METADATA
		//

		// If the channel name is "none", reset the cache item and return an empty tuner status
		if(String::Compare(channel, TunerStatus::NoChannel, StringComparison::OrdinalIgnoreCase) == 0) {

			m_statuscache[index] = gcnew Tuple<String^, String^, IPAddress^>(TunerStatus::NoChannel, TunerStatus::NoChannel, IPAddress::None);
			return TunerStatus::Empty;
		}

		//
		// CREATE NEW METADATA
		//

		String^ channelname = String::Empty;			// Channel name to use/cache
		IPAddress^ targetip = IPAddress::None;			// Target IP address to use/cache

		// Get the stream information for the tuned frequency
		char* streaminfo_str = nullptr;
		result = hdhomerun_device_get_tuner_streaminfo(device, &streaminfo_str);
		if(result == 1) {

			// Convert the volatile unmanaged string pointer before it's overwritten
			String^ streaminfo = gcnew String(streaminfo_str);

			// Try to get the program number string from the tuner
			char* program_str = nullptr;
			result = hdhomerun_device_get_tuner_program(device, &program_str);
			if(result == 1) {

				// Try to use the stream info and program number to get the channel name
				channelname = GetVirtualChannelName(gcnew String(program_str), streaminfo);
			}
		}

		// If the stream/program mapping didn't work out, try the channel map instead
		if(String::IsNullOrEmpty(channelname)) {

			// Get the channel mapping string from the tuner
			char* channelmap_str = nullptr;
			result = hdhomerun_device_get_tuner_channelmap(device, &channelmap_str);
			if(result == 1) {

				// Create the hdhomerun_channel_list_t for the specified channel mapping
				struct hdhomerun_channel_list_t* channellist = hdhomerun_channel_list_create(channelmap_str);
				if(channellist != nullptr) {

					// The raw channel name should be modulation:frequency, we only care about the frequency
					int colon = channel->IndexOf(':');
					String^ frequencystring = (colon > 0) ? channel->Substring(colon + 1) : channel;

					// Try to concvert the frequency string into a 32-bit integer for libhdhomerun to work with
					uint32_t frequency = 0;
					if(uint32_t::TryParse(frequencystring, frequency)) {

						// Get the channel number for the specified frequency in the channel listing
						uint16_t number = hdhomerun_channel_frequency_to_number(channellist, frequency);
						if(number > 0) channelname = String::Format("{0} ({1}MHz)", number, frequency / 1000000);
					}

					// Destroy the allocated channel list
					hdhomerun_channel_list_destroy(channellist);
				}
			}
		}

		// If nothing better worked out, just use the raw channel name as the display channel name
		if(String::IsNullOrEmpty(channelname)) channelname = channel;

		// Try to get the tuner target device to convert into an IP address
		char* target_str = nullptr;
		if(hdhomerun_device_get_tuner_target(device, &target_str) == 1) {

			// The target string will be in the form of a URI ([http|rtp]://192.168.0.1:12345),
			// extract just the IPv4 address portion of the string using System::Uri
			Uri^ uri = nullptr;
			if(Uri::TryCreate(gcnew String(target_str), UriKind::RelativeOrAbsolute, uri))
			{
				// HostNameType can actually throw, this will happen if target is "none", for example
				try { if(uri->HostNameType == UriHostNameType::IPv4) IPAddress::TryParse(uri->Host, targetip); }
				catch(Exception^) { /* DO NOTHING */ }
			}
		}

		// Update the status cache with the original channel name, new channel name, and new IP address
		m_statuscache[index] = gcnew Tuple<String^, String^, IPAddress^>(channel, channelname, targetip);

		// Update the hdhomerun_tuner_status_t structure with the channel name we want
		strncpy_s(status.channel, std::extent<decltype(status.channel)>::value, context->marshal_as<char const*>(channelname), _TRUNCATE);

		// Create the new TunerStatus instance
		return TunerStatus::Create(&status, targetip);
	}

	// Ensure destruction of the hdhomerun_device_t instance
	finally { hdhomerun_device_destroy(device); }
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
// TunerDevice::GetVirtualChannelName (static, private)
//
// Retrieves the virtual channel name from a tuner program and streaminfo
//
// Arguments:
//
//	program		- Tuner program string
//	streaminfo	- Tuner stream information string

String^ TunerDevice::GetVirtualChannelName(String^ program, String^ streaminfo)
{
	if(CLRISNULL(program)) throw gcnew ArgumentNullException("program");
	if(CLRISNULL(streaminfo)) throw gcnew ArgumentNullException("streaminfo");

	// The program string has to be set to something
	if(String::IsNullOrEmpty(program)) return String::Empty;

	// The program can have additional information after the number, like on the
	// HDHomeRun EXTEND there is a transcode=.  Just grab the first bit
	int spaceindex = program->IndexOf(' ');
	if(spaceindex > 0) program = program->Substring(0, spaceindex);

	// Don't use a program number set to zero
	if(String::Compare(program, "0", StringComparison::OrdinalIgnoreCase) == 0) return String::Empty;

	// The stream information comes in on multiple lines of text split by \n
	array<String^>^ streams = streaminfo->Split(gcnew array<wchar_t> {'\n'}, StringSplitOptions::RemoveEmptyEntries);
	for each(String ^ stream in streams) {

		// If the line starts with the program number, grab the virtual channel number/name
		if(stream->StartsWith(program)) {

			int colonspace = stream->IndexOf(": ");
			if(colonspace > 0) return stream->Substring(colonspace + 2);
		}
	}

	return String::Empty;
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
