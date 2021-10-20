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

#include "StorageDevice.h"

#pragma warning(push, 4)

namespace zuki::hdhomeruntray::interop {

//---------------------------------------------------------------------------
// StorageDevice Constructor (private)
//
// Arguments:
//
//	device			- Pointer to the unmanaged device information

StorageDevice::StorageDevice(struct hdhomerun_discover_device_v3_t const* device) : Device(device)
{
	CLRASSERT(device != nullptr);

	// TODO: Assign local properties here
}

//---------------------------------------------------------------------------
// StorageDevice::Create (internal)
//
// Creates a new StorageDevice instance
//
// Arguments:
//
//	device			- Pointer to the unmanaged device information

StorageDevice^ StorageDevice::Create(struct hdhomerun_discover_device_v3_t const* device)
{
	return gcnew StorageDevice(device);
}

//---------------------------------------------------------------------------
// StorageDevice::StorageID::get
//
// Gets the storage device identifier

String^ StorageDevice::StorageID::get(void)
{
	return m_storageid;
}

//---------------------------------------------------------------------------
// StorageDevice::StorageURL::get
//
// Gets the storage device data URL

String^ StorageDevice::StorageURL::get(void)
{
	return m_storageurl;
}

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::interop

#pragma warning(pop)
