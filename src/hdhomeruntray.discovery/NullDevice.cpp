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

#include "NullDevice.h"

#pragma warning(push, 4)

namespace zuki::hdhomeruntray::discovery {

//---------------------------------------------------------------------------
// NullDevice Constructor (private)
//
// Arguments:
//
//	NONE

NullDevice::NullDevice() : Device(_DeviceType::None)
{
	m_name = String::Empty;
	m_friendlyname = String::Empty;
}

//---------------------------------------------------------------------------
// NullDevice::Create (internal)
//
// Creates a new NullDevice instance
//
// Arguments:
//
//	NONE

NullDevice^ NullDevice::Create(void)
{
	return gcnew NullDevice();
}

//---------------------------------------------------------------------------
// NullDevice::FriendlyName::get
//
// Gets the tuner device friendly name

String^ NullDevice::FriendlyName::get(void)
{
	return m_friendlyname;
}

//---------------------------------------------------------------------------
// NullDevice::Name::get
//
// Gets the tuner device name

String^ NullDevice::Name::get(void)
{
	return m_name;
}

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)
