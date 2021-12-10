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

	m_channel = gcnew String(status->channel);

	// Only bother with setting the variables if the tuner is active
	if(IsActive) {

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
// TunerStatus::Channel::get
//
// Gets the tuned channel string (modulation+frequency)

String^ TunerStatus::Channel::get(void)
{
	return m_channel;
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

		// Get the status reported by all tuner devices first
		struct hdhomerun_tuner_status_t status = {};
		int result = hdhomerun_device_get_tuner_status(device, nullptr, &status);
		if(result != 1) return TunerStatus::Empty;

		// If the channel name is "none" we can get out early here
		if(strcmp(status.channel, "none") == 0) return TunerStatus::Empty;

		// Try to retrieve the virtual channel information from the program/stream.  Only CableCard
		// devices seem to support vstatus and to get bitrate the legacy status still needs to
		// be used so switching to JSON status isn't really a good option
		char* program_str = nullptr;
		result = hdhomerun_device_get_tuner_program(device, &program_str);
		if(result == 1) {
			
			// Volatile pointer, convert to String^ immediately
			String^ program = gcnew String(program_str);

			char* streaminfo_str = nullptr;
			result = hdhomerun_device_get_tuner_streaminfo(device, &streaminfo_str);
			if(result == 1) {

				// Volatile pointer, convert to String^ immediately
				String^ streaminfo = gcnew String(streaminfo_str);

				String^ vchannel = GetVirtualChannelName(program, streaminfo);
				if(!String::IsNullOrEmpty(vchannel)) {

					msclr::auto_handle<msclr::interop::marshal_context> context(gcnew msclr::interop::marshal_context());
					strncpy_s(status.channel, std::extent<decltype(status.channel)>::value, context->marshal_as<char const*>(vchannel), _TRUNCATE);
				}
			}
		}

		// And finally, try to get the tuner target device to convert into an IP address
		IPAddress^ targetip = IPAddress::None;
		char* target_str = nullptr;
		bool hastarget = (hdhomerun_device_get_tuner_target(device, &target_str) == 1);

		if(hastarget) {

			// Volatile pointer, convert to String^ immediately
			String^ target = gcnew String(target_str);

			// The target string will be in the form of a URI ([http|rtp]://192.168.0.1:12345),
			// extract just the IPv4 address portion of the string using System::Uri
			Uri^ uri = nullptr;
			if(Uri::TryCreate(target, UriKind::RelativeOrAbsolute, uri))
			{
				// HostNameType can actually throw, this will happen if target is "none", for example
				try { if(uri->HostNameType == UriHostNameType::IPv4) IPAddress::TryParse(uri->Host, targetip); }
				catch(Exception^) { /* DO NOTHING */ }
			}
		}

		return gcnew TunerStatus(&status, targetip);		// Create the status instance
	}

	// Ensure destruction of the hdhomerun_device_t instance
	finally { hdhomerun_device_destroy(device); }
}

//---------------------------------------------------------------------------
// TunerStatus::GetHashCode
//
// Serves as the default hash function
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
	hash ^= CLRISNULL(m_channel) ? 0 : m_channel->GetHashCode();
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
//	program		- Tuner program string
//	streaminfo	- Tuner stream information string

String^ TunerStatus::GetVirtualChannelName(String^ program, String^ streaminfo)
{
	if(CLRISNULL(program)) throw gcnew ArgumentNullException("program");
	if(CLRISNULL(streaminfo)) throw gcnew ArgumentNullException("streaminfo");

	// The program string has to be set to something
	if(String::IsNullOrEmpty(program)) return String::Empty;

	// The stream information comes in on multiple lines of text split by \n
	array<String^>^ streams = streaminfo->Split(gcnew array<wchar_t> {'\n'}, StringSplitOptions::RemoveEmptyEntries);
	for each(String^ stream in streams) {

		// If the line starts with the program number, grab the virtual channel number/name
		if(stream->StartsWith(program)) {

			int colonspace = stream->IndexOf(": ");
			if(colonspace > 0) return stream->Substring(colonspace + 2);
		}
	}

	return String::Empty;
}

//---------------------------------------------------------------------------
// TunerStatus::IsActive::get
//
// Gets a flag indicating if the tuner is active or not

bool TunerStatus::IsActive::get(void)
{
	if(String::IsNullOrEmpty(m_channel)) return false;
	else return (String::Compare(m_channel, "none", StringComparison::OrdinalIgnoreCase) != 0);
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
