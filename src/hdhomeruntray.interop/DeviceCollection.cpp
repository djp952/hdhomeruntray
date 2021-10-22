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

#include <memory>

#include "DeviceCollection.h"
#include "JsonWebRequest.h"
#include "ReadOnlyListEnumerator.h"
#include "StorageDevice.h"
#include "TunerDevice.h"

using namespace Newtonsoft::Json;
using namespace Newtonsoft::Json::Linq;

#pragma warning(push, 4)

namespace zuki::hdhomeruntray::interop {

//---------------------------------------------------------------------------
// DeviceCollection Constructor (private)
//
// Arguments:
//
//	devices		- List<> containing the devices

DeviceCollection::DeviceCollection(List<Device^>^ devices) : m_devices(devices)
{
	if(Object::ReferenceEquals(devices, nullptr)) throw gcnew ArgumentNullException("devices");
}

//---------------------------------------------------------------------------
// DeviceCollection::default[int]::get
//
// Gets the element at the specified index in the read-only list

Device^ DeviceCollection::default::get(int index)
{
	return m_devices[index];
}

//---------------------------------------------------------------------------
// DeviceCollection::Count::get
//
// Gets the number of elements in the collection

int DeviceCollection::Count::get(void)
{
	return m_devices->Count;
}

//---------------------------------------------------------------------------
// DeviceCollection::Create (static)
//
// Creates a new DeviceCollection instance by executing a discovery
//
// Arguments:
//
//  NONE

DeviceCollection^ DeviceCollection::Create(void)
{
	return Create(DiscoveryMethod::Broadcast);
}

//---------------------------------------------------------------------------
// DeviceCollection::Create (static)
//
// Creates a new DeviceCollection instance by executing a discovery
//
// Arguments:
//
//  method		- Discovery method to be used

DeviceCollection^ DeviceCollection::Create(DiscoveryMethod method)
{
	if(method == DiscoveryMethod::Broadcast) return DiscoverBroadcast();
	else if(method == DiscoveryMethod::Http) return DiscoverHttp();
	else throw gcnew ArgumentOutOfRangeException("method");
}

//---------------------------------------------------------------------------
// DeviceCollection::DiscoverBroadcast (static, private)
//
// Executes a broadcast device discovery
//
// Arguments:
//
//	NONE

DeviceCollection^ DeviceCollection::DiscoverBroadcast(void)
{
	List<Device^>^ discovered = gcnew List<Device^>();				// Collection of discovered devices

	// Allocate enough heap storage to hold up to 64 enumerated devices on the network
	std::unique_ptr<struct hdhomerun_discover_device_v3_t[]> devices(new struct hdhomerun_discover_device_v3_t[64]);

	// Use the libhdhomerun broadcast discovery mechanism to find all devices on the local network
	int result = hdhomerun_discover_find_devices_custom_v3(0, HDHOMERUN_DEVICE_TYPE_WILDCARD,
		HDHOMERUN_DEVICE_ID_WILDCARD, &devices[0], 64);
	if(result == -1) throw gcnew Exception("hdhomerun_discover_find_devices_custom_v3 failed");

	for(int index = 0; index < result; index++) {

		// Create the appropriate device object type based on the reported device type
		if(devices[index].device_type == HDHOMERUN_DEVICE_TYPE_TUNER) discovered->Add(TunerDevice::Create(devices[index]));
		else if(devices[index].device_type == HDHOMERUN_DEVICE_TYPE_STORAGE) discovered->Add(StorageDevice::Create(devices[index]));
	}

	return gcnew DeviceCollection(discovered);
}

//---------------------------------------------------------------------------
// DeviceCollection::DiscoverHttp (static, private)
//
// Executes an HTTP device discovery
//
// Arguments:
//
//	NONE

DeviceCollection^ DeviceCollection::DiscoverHttp(void)
{
	List<Device^>^ discovered = gcnew List<Device^>();				// Collection of discovered devices

	// TODO: CancellationSource/token

	// The discovery JSON is returned as an array consisting of individual device objects
	JArray^ devices = JsonWebRequest::GetArray("https://ipv4-api.hdhomerun.com/discover");
	if(!Object::ReferenceEquals(devices, nullptr)) {

		for each(JObject^ device in devices) {

			// Gather enough information from the discovery data to determine how to proceed
			JToken^ deviceid = device->GetValue("DeviceID", StringComparison::OrdinalIgnoreCase);
			JToken^ storageid = device->GetValue("StorageID", StringComparison::OrdinalIgnoreCase);
			JToken^ discoverurl = device->GetValue("DiscoverURL", StringComparison::OrdinalIgnoreCase);

			// Retrieve the individual device discovery data
			// TODO: Cancellation / Exceptions
			if(!Object::ReferenceEquals(discoverurl, nullptr)) {

				JObject^ discovery = JsonWebRequest::GetObject(discoverurl->ToObject<String^>());
				if(!Object::ReferenceEquals(discovery, nullptr)) {

					// A single device may report both tuners and storage so check for both types and
					// process them as distinct device instances
					if(!Object::ReferenceEquals(nullptr, deviceid)) discovered->Add(TunerDevice::Create(discovery));
					if(!Object::ReferenceEquals(nullptr, storageid)) discovered->Add(StorageDevice::Create(discovery));
				}
			}
		}
	}

	// Return the generated List<> as a new DeviceCollection instance
	return gcnew DeviceCollection(discovered);
}

//---------------------------------------------------------------------------
// DeviceCollection::GetEnumerator
//
// Returns a generic IEnumerator<T> for the member collection
//
// Arguments:
//
//	NONE

IEnumerator<Device^>^ DeviceCollection::GetEnumerator(void)
{
	return gcnew ReadOnlyListEnumerator<Device^>(this);
}

//---------------------------------------------------------------------------
// DeviceCollection::IEnumerable_GetEnumerator
//
// Returns a non-generic IEnumerator for the member collection
//
// Arguments:
//
//	NONE

System::Collections::IEnumerator^ DeviceCollection::IEnumerable_GetEnumerator(void)
{
	return GetEnumerator();
}

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::interop

#pragma warning(pop)
