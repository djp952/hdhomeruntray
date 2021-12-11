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

	// Assign channel name
	m_channelname = gcnew String(status->channel);

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

TunerStatus^ TunerStatus::Create(struct hdhomerun_tuner_status_t const* status, IPAddress^ targetip)
{
	if(status == nullptr) throw gcnew ArgumentNullException("status");
	if(CLRISNULL(targetip)) throw gcnew ArgumentNullException("targetip");

	return gcnew TunerStatus(status, targetip);
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
// TunerStatus::IsActive::get
//
// Gets a flag indicating if the tuner is active or not

bool TunerStatus::IsActive::get(void)
{
	if(String::IsNullOrEmpty(m_channelname)) return false;
	else return (String::Compare(m_channelname, NoChannel, StringComparison::OrdinalIgnoreCase) != 0);
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
