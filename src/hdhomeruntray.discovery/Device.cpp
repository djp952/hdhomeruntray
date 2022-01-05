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

#include "Device.h"

#pragma warning(push, 4)

namespace zuki::hdhomeruntray::discovery {

//---------------------------------------------------------------------------
// Device Constructor (protected)
//
// Arguments:
//
//	type		- Type of device being constructed

Device::Device(_DeviceType type) : m_devicetype(type), m_localip(IPAddress::None), m_baseurl(String::Empty)
{
}

//---------------------------------------------------------------------------
// Device Constructor (protected)
//
// Arguments:
//
//	device		- Reference to the JSON discovery data for the device
//	localip		- The IP address of the device
//	type		- Type of device being constructed

Device::Device(JObject^ device, IPAddress^ localip, _DeviceType type) : m_devicetype(type), m_localip(localip)
{
	if(CLRISNULL(device)) throw gcnew ArgumentNullException("device");
	if(CLRISNULL(localip)) throw gcnew ArgumentNullException("localip");

	JToken^ baseurl = device->GetValue("BaseURL", StringComparison::OrdinalIgnoreCase);

	if(CLRISNOTNULL(baseurl)) m_baseurl = baseurl->ToString();
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
// Device::LocalIP::get
//
// Gets the device local IP address

IPAddress^ Device::LocalIP::get(void)
{
	return m_localip;
}

//---------------------------------------------------------------------------
// Device::Type::get
//
// Gets the device type identifier

_DeviceType Device::Type::get(void)
{
	return m_devicetype;
}

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)
