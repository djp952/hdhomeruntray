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
//	status		- Pointer to the unmanaged hdhomerun_tuner_status_t struct

TunerStatus::TunerStatus(struct hdhomerun_tuner_status_t const* status) : m_hasvirtualchannel(false)
{
	if(status == nullptr) throw gcnew ArgumentNullException("status");

	m_channel = gcnew String(status->channel);

	m_signalstrength = static_cast<int>(status->signal_strength);
	m_signalstrengthcolor = hdhomerun_device_get_tuner_status_ss_color(const_cast<hdhomerun_tuner_status_t*>(status));

	m_signalquality = static_cast<int>(status->signal_to_noise_quality);
	m_signalqualitycolor = hdhomerun_device_get_tuner_status_snq_color(const_cast<hdhomerun_tuner_status_t*>(status));

	m_symbolquality = static_cast<int>(status->symbol_error_quality);
	m_symbolqualitycolor = hdhomerun_device_get_tuner_status_seq_color(const_cast<hdhomerun_tuner_status_t*>(status));
}
		
//---------------------------------------------------------------------------
// TunerStatus Constructor (private)
//
// Arguments:
//
//	status		- Pointer to the unmanaged hdhomerun_tuner_status_t struct
//	vstatus		- Pointer to the unmanaged hdhomerun_tuner_vstatus_t struct

TunerStatus::TunerStatus(struct hdhomerun_tuner_status_t const* status, struct hdhomerun_tuner_vstatus_t const* vstatus) : TunerStatus(status)
{
	if(status == nullptr) throw gcnew ArgumentNullException("status");
	if(vstatus == nullptr) throw gcnew ArgumentNullException("vstatus");

	m_virtualchannelnum = gcnew String(vstatus->vchannel);
	m_virtualchannelname = gcnew String(vstatus->name);

	m_hasvirtualchannel = true;			// Virtual channel info is present
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
	if(color == HDHOMERUN_STATUS_COLOR_GREEN) return Color::FromArgb(COLOR_GREEN);
	else if(color == HDHOMERUN_STATUS_COLOR_YELLOW) return Color::FromArgb(COLOR_YELLOW);
	else if(color == HDHOMERUN_STATUS_COLOR_RED) return Color::FromArgb(COLOR_RED);

	// HDHOMERUN_STATUS_COLOR_NEUTRAL is only used for devices that don't 
	// support tuner locking; not sure what that's about so default to green
	return Color::FromArgb(COLOR_GREEN);
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
	if(device == nullptr) throw gcnew Exception("Failed to create hdhomerun_device_t instance for the tuner");

	try {

		struct hdhomerun_tuner_status_t status = {};			// Legacy status
		struct hdhomerun_tuner_vstatus_t vstatus = {};			// Virtual channel status

		int result = hdhomerun_device_get_tuner_status(device, nullptr, &status);
		if((result == 1) && (!tunerdevice->IsLegacy)) result = hdhomerun_device_get_tuner_vstatus(device, nullptr, &vstatus);
		
		// 1 == success; 0 == rejected; -1 = communication failure
		if(result == 1) return (tunerdevice->IsLegacy) ? gcnew TunerStatus(&status) : gcnew TunerStatus(&status, &vstatus);
		else if(result == 0) throw gcnew Exception("Tuner device rejected the request");
		else if(result == -1) throw gcnew Exception("There was a communication failure accessing the tuner device");
		else throw gcnew Exception("An unknown error occurred accessing the tuner device");
	}

	// Ensure destruction of the hdhomerun_device_t instance
	finally { hdhomerun_device_destroy(device); }
}

//---------------------------------------------------------------------------
// TunerStatus::HasVirtualChannel::get
//
// Gets a flag indicating if the virtual channel data is available

bool TunerStatus::HasVirtualChannel::get(void)
{
	return m_hasvirtualchannel;
}

//---------------------------------------------------------------------------
// TunerStatus::IsActive::get
//
// Gets a flag indicating if the tuner is active or not

bool TunerStatus::IsActive::get(void)
{
	return (String::Compare(m_channel, "none", StringComparison::OrdinalIgnoreCase) != 0);
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
	return this->IsActive ? ConvertHDHomeRunColor(m_signalqualitycolor) : Color::FromArgb(COLOR_GRAY);
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
	return this->IsActive ? ConvertHDHomeRunColor(m_signalstrengthcolor) : Color::FromArgb(COLOR_GRAY);
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
	return this->IsActive ? ConvertHDHomeRunColor(m_symbolqualitycolor) : Color::FromArgb(COLOR_GRAY);
}

//---------------------------------------------------------------------------
// TunerStatus::VirtualChannelName::get
//
// Gets the tuned virtual channel name

String^ TunerStatus::VirtualChannelName::get(void)
{
	return m_virtualchannelname;
}

//---------------------------------------------------------------------------
// TunerStatus::VirtualChannelNumber::get
//
// Gets the tuned virtual channel number

String^ TunerStatus::VirtualChannelNumber::get(void)
{
	return m_virtualchannelnum;
}

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)
