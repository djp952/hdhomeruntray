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

#include "Device.h"

#pragma warning(push, 4)

namespace zuki::hdhomeruntray::discovery {

//---------------------------------------------------------------------------
// Device Constructor (protected)
//
// Arguments:
//
//	device			- Reference to the unmanaged device information

Device::Device(struct hdhomerun_discover_device_v3_t const& device)
{
	m_baseurl = gcnew String(device.base_url);
	m_devicetype = static_cast<zuki::hdhomeruntray::discovery::DeviceType>(device.device_type);
}

//---------------------------------------------------------------------------
// Device Constructor (protected)
//
// Arguments:
//
//	device		- Reference to the JSON discovery data for the device
//	type		- Type of device being constructed

Device::Device(JObject^ device, zuki::hdhomeruntray::discovery::DeviceType type) : m_devicetype(type)
{
	if(Object::ReferenceEquals(device, nullptr)) throw gcnew ArgumentNullException("device");

	JToken^ baseurl = device->GetValue("BaseURL", StringComparison::OrdinalIgnoreCase);
	if(!Object::ReferenceEquals(baseurl, nullptr)) m_baseurl = baseurl->ToString();
}

//---------------------------------------------------------------------------
// Device::BaseURL::get
//
// Gets the device web interface base URL

String^ Device::BaseURL::get(void)
{
	return m_baseurl;
}

//---------------------------------------------------------------------------
// Device::DeviceType::get
//
// Gets the device type identifier

zuki::hdhomeruntray::discovery::DeviceType Device::DeviceType::get(void)
{
	return m_devicetype;
}

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)
