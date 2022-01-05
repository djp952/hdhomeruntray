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

#include "TunerStatus.h"

#include "TunerDevice.h"

#pragma warning(push, 4)

using namespace System::Globalization;

namespace zuki::hdhomeruntray::discovery {

//---------------------------------------------------------------------------
// TunerStatus Constructor (private)
//
// Arguments:
//
//	NONE

TunerStatus::TunerStatus(void)
{
}

//---------------------------------------------------------------------------
// TunerStatus Constructor (private)
//
// Arguments:
//
//	status		- Pointer to the unmanaged hdhomerun_tuner_status_t struct
//	targetip	- IPAddress of the target device

TunerStatus::TunerStatus(struct hdhomerun_tuner_status_t const* status, IPAddress^ targetip) : TunerStatus()
{
	if(status == nullptr) throw gcnew ArgumentNullException("status");
	if(CLRISNULL(targetip)) throw gcnew ArgumentNullException("targetip");

	// Assign the channel name and determine the overall device status
	m_channelname = gcnew String(status->channel);
	if(String::Compare(m_channelname, NoChannel, StringComparison::OrdinalIgnoreCase) != 0) m_devicestatus = _DeviceStatus::Active;

	// Only bother with setting the variables if the tuner is active
	if(m_devicestatus == _DeviceStatus::Active) {

		m_signalstrength = static_cast<int>(status->signal_strength);
		m_signalstrengthcolor = ConvertHDHomeRunColor(hdhomerun_device_get_tuner_status_ss_color(const_cast<hdhomerun_tuner_status_t*>(status)));

		m_signalquality = static_cast<int>(status->signal_to_noise_quality);
		m_signalqualitycolor = ConvertHDHomeRunColor(hdhomerun_device_get_tuner_status_snq_color(const_cast<hdhomerun_tuner_status_t*>(status)));

		m_symbolquality = static_cast<int>(status->symbol_error_quality);
		m_symbolqualitycolor = ConvertHDHomeRunColor(hdhomerun_device_get_tuner_status_seq_color(const_cast<hdhomerun_tuner_status_t*>(status)));

		m_bitrate = status->raw_bits_per_second;
		m_targetip = targetip;
	}
}
		
//---------------------------------------------------------------------------
// TunerStatus::BitRate::get
//
// Gets the current tuner output bitrate

int TunerStatus::BitRate::get(void)
{
	return m_bitrate;
}

//---------------------------------------------------------------------------
// TunerStatus::ChannelName::get
//
// Gets the name of the tuned channel

String^ TunerStatus::ChannelName::get(void)
{
	return m_channelname;
}

//---------------------------------------------------------------------------
// TunerStatus::ConvertHDHomeRunColor (private, static)
//
// Converts the color code from libhdhomerun into the color I want to use
//
// Arguments:
//
//	color		- The color code from libhdhomerun to convert

Color TunerStatus::ConvertHDHomeRunColor(uint32_t color)
{
	if(color == HDHOMERUN_STATUS_COLOR_GREEN) return DeviceStatusColor::Green;
	else if(color == HDHOMERUN_STATUS_COLOR_YELLOW) return DeviceStatusColor::Yellow;
	else if(color == HDHOMERUN_STATUS_COLOR_RED) return DeviceStatusColor::Red;

	// HDHOMERUN_STATUS_COLOR_NEUTRAL is only used for devices that don't 
	// support tuner locking; not sure what that's about so default to green
	return DeviceStatusColor::Green;
}

//---------------------------------------------------------------------------
// TunerStatus::Create (internal)
//
// Creates a new TunerStatus instance
//
// Arguments:
//
//	tunerdevice		- Reference to the parent TunerDevice object
//	index			- Index of the tuner within the TunerDevice

TunerStatus^ TunerStatus::Create(TunerDevice^ tunerdevice, int index)
{
	if(CLRISNULL(tunerdevice)) throw gcnew ArgumentNullException("tunerdevice");

	// Convert the device ID into an unsigned 32-bit integer
	uint32_t deviceid = 0;
	if(!uint32_t::TryParse(tunerdevice->DeviceID, NumberStyles::HexNumber, CultureInfo::InvariantCulture, deviceid))
		throw gcnew ArgumentOutOfRangeException("deviceid");

	// Convert the IP address into a byte array; convert for little endian as necessary
	array<Byte>^ ipbytes = tunerdevice->LocalIP->GetAddressBytes();
	if(BitConverter::IsLittleEndian) Array::Reverse(ipbytes);

	// Attempt to create a hdhomerun_device_t instance for the tuner instance
	hdhomerun_device_t* device = hdhomerun_device_create(deviceid, BitConverter::ToUInt32(ipbytes, 0), index, nullptr);
	if(device == nullptr) return TunerStatus::Empty;

	try {

		// Get the general status for the tuner device
		struct hdhomerun_tuner_status_t status = {};
		int result = hdhomerun_device_get_tuner_status(device, nullptr, &status);
		if(result != 1) return TunerStatus::Empty;

		String^ channelname = String::Empty;				// Updated channel name
		IPAddress^ targetip = IPAddress::None;				// Target IP address

		// Get the stream information for the tuned frequency
		char* streaminfo_str = nullptr;
		result = hdhomerun_device_get_tuner_streaminfo(device, &streaminfo_str);
		if((result == 1) && (streaminfo_str != nullptr)) {

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

					String^ rawchannel = gcnew String(status.channel);

					// The raw channel name should be modulation:frequency, we only care about the frequency
					int colon = rawchannel->IndexOf(':');
					String^ channelnumorfrequencystring = (colon > 0) ? rawchannel->Substring(colon + 1) : rawchannel;

					// Try to convert the channel/frequency string into a 32-bit integer for libhdhomerun to work with
					uint32_t channelnumorfrequency = 0;
					if(uint32_t::TryParse(channelnumorfrequencystring, channelnumorfrequency)) {

						// If the parsed value is less than 1000 (highest I see is 862 for eu-cable), assume channel number
						if(channelnumorfrequency < 1000) {

							uint16_t channelnum = static_cast<uint16_t>(channelnumorfrequency);
							uint32_t frequency = hdhomerun_channel_number_to_frequency(channellist, channelnum);
							if((channelnum > 0) && (frequency > 0)) channelname = String::Format("{0} ({1}MHz)", channelnum, frequency / 1000000);

						}

						else {

							uint32_t frequency = channelnumorfrequency;
							uint16_t channelnum = hdhomerun_channel_frequency_to_number(channellist, frequency);
							if((channelnum > 0) && (frequency > 0)) channelname = String::Format("{0} ({1}MHz)", channelnum, frequency / 1000000);
						}
					}

					// Destroy the allocated channel list
					hdhomerun_channel_list_destroy(channellist);
				}
			}
		}

		// If a better channel name was determined, overwrite it in the status structure
		if(!String::IsNullOrEmpty(channelname)) {

			msclr::auto_handle<msclr::interop::marshal_context> context(gcnew msclr::interop::marshal_context());
			strncpy_s(status.channel, std::extent<decltype(status.channel)>::value, context->marshal_as<char const*>(channelname), _TRUNCATE);
		}

		// Try to get the tuner target device to convert into an IP address
		char* target_str = nullptr;
		if(hdhomerun_device_get_tuner_target(device, &target_str) == 1) {

			// The target string will be in the form of a URI ([http|rtp]://192.168.0.1:12345),
			// extract just the IPv4 address portion of the string using System::Uri
			Uri^ uri = nullptr;
			if(Uri::TryCreate(gcnew String(target_str), UriKind::RelativeOrAbsolute, uri))
			{
				// HostNameType can actually throw, this will happen if target is "none", for example
				try { if(uri->IsAbsoluteUri && (uri->HostNameType == UriHostNameType::IPv4)) IPAddress::TryParse(uri->Host, targetip); }
				catch(Exception^) { /* DO NOTHING */ }
			}
		}

		return gcnew TunerStatus(&status, targetip);	// Create the TunerStatus instance
	}

	// Ensure destruction of the hdhomerun_device_t instance
	finally { hdhomerun_device_destroy(device); }
}

//---------------------------------------------------------------------------
// TunerStatus::DeviceStatus::get
//
// Gets the overall device status

_DeviceStatus TunerStatus::DeviceStatus::get(void)
{
	return m_devicestatus;
}

//---------------------------------------------------------------------------
// TunerStatus::GetHashCode
//
// Serves as the defult hash function
//
// Arguments:
//
//	NONE

int TunerStatus::GetHashCode(void)
{
	// 32-bit FNV-1a primes (http://www.isthe.com/chongo/tech/comp/fnv/index.html#FNV-source) 
	const int fnv_offset_basis = 2166136261U;
	const int fnv_prime = 16777619U;

	int hash = fnv_offset_basis;

	// FNV hash the relevant member variables; things like the color codes are tied
	// to the signal strength values, no need to include those here
	hash ^= CLRISNULL(m_channelname) ? 0 : m_channelname->GetHashCode();
	hash *= fnv_prime;
	hash ^= m_signalstrength.GetHashCode();
	hash *= fnv_prime;
	hash ^= m_signalquality.GetHashCode();
	hash *= fnv_prime;
	hash ^= m_symbolquality.GetHashCode();
	hash *= fnv_prime;
	hash *= fnv_prime;
	hash ^= m_bitrate.GetHashCode();
	hash *= fnv_prime;
	hash ^= CLRISNULL(m_targetip) ? 0 : m_targetip->GetHashCode();

	return hash;
}

//---------------------------------------------------------------------------
// TunerStatus::GetVirtualChannelName (static, private)
//
// Retrieves the virtual channel name from a tuner program and streaminfo
//
// Arguments:
//
//	program		- Tuner program number string
//	streaminfo	- Tuner stream information string

String^ TunerStatus::GetVirtualChannelName(String^ program, String^ streaminfo)
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
// TunerStatus::SignalQuality::get
//
// Gets the signal quality of the tuned channel

int TunerStatus::SignalQuality::get(void)
{
	return m_signalquality;
}

//---------------------------------------------------------------------------
// TunerStatus::SignalQualityColor::get
//
// Gets the color code for the signal quality

Color TunerStatus::SignalQualityColor::get(void)
{
	return m_signalqualitycolor;
}

//---------------------------------------------------------------------------
// TunerStatus::SignalStrength::get
//
// Gets the signal strength of the tuned channel

int TunerStatus::SignalStrength::get(void)
{
	return m_signalstrength;
}

//---------------------------------------------------------------------------
// TunerStatus::SignalStrengthColor::get
//
// Gets the color code for the signal strength

Color TunerStatus::SignalStrengthColor::get(void)
{
	return m_signalstrengthcolor;
}

//---------------------------------------------------------------------------
// TunerStatus::SymbolQuality::get
//
// Gets the symbol quality of the tuned channel

int TunerStatus::SymbolQuality::get(void)
{
	return m_symbolquality;
}

//---------------------------------------------------------------------------
// TunerStatus::SymbolQualityColor::get
//
// Gets the color code for the symbol quality

Color TunerStatus::SymbolQualityColor::get(void)
{
	return m_symbolqualitycolor;
}

//---------------------------------------------------------------------------
// TunerStatus::TargetIP::get
//
// Gets the target IP address of the tuner

IPAddress^ TunerStatus::TargetIP::get(void)
{
	return m_targetip;
}

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)
