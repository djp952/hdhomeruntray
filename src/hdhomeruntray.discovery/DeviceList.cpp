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

#include <memory>

#include "DeviceList.h"
#include "JsonWebRequest.h"
#include "ReadOnlyListEnumerator.h"
#include "StorageDevice.h"
#include "TunerDevice.h"

using namespace Newtonsoft::Json;
using namespace Newtonsoft::Json::Linq;

#pragma warning(push, 4)

namespace zuki::hdhomeruntray::discovery {

// Class CompareDeviceById
//
// Helper class used to sort a List<Device^> by DeviceID
ref class CompareDeviceById : Comparer<Device^>
{
public:

	virtual int Compare(Device^ lhs, Device^ rhs) override
	{
		if(CLRISNULL(lhs) && CLRISNULL(rhs)) return 0;

		// safe_cast<> the objects into either of the underlying types
		TunerDevice^ lhstuner = dynamic_cast<TunerDevice^>(lhs);
		TunerDevice^ rhstuner = dynamic_cast<TunerDevice^>(rhs);
		StorageDevice^ lhsstorage = dynamic_cast<StorageDevice^>(lhs);
		StorageDevice^ rhsstorage = dynamic_cast<StorageDevice^>(rhs);

		// Make sure that particular assumption was valid
		CLRASSERT(CLRISNOTNULL(lhstuner) || CLRISNOTNULL(lhsstorage));
		CLRASSERT(CLRISNOTNULL(rhstuner) || CLRISNOTNULL(rhsstorage));

		// If both Devices are TunerDevices, compare the DeviceID
		if(CLRISNOTNULL(lhstuner) && CLRISNOTNULL(rhstuner))
			return String::Compare(lhstuner->DeviceID, rhstuner->DeviceID, StringComparison::OrdinalIgnoreCase);

		// If both Devices are StorageDevices, compare the StorageID
		if(CLRISNOTNULL(lhsstorage) && CLRISNOTNULL(rhsstorage))
			return String::Compare(lhsstorage->StorageID, rhsstorage->StorageID, StringComparison::OrdinalIgnoreCase);

		if(CLRISNOTNULL(lhstuner)) return -1;			// Tuner < Storage
		else return 1;									// Storage > Tuner
	}
};

//---------------------------------------------------------------------------
// DeviceList Constructor (private)
//
// Arguments:
//
//	devices		- List<> containing the devices

DeviceList::DeviceList(List<Device^>^ devices) : m_devices(devices)
{
	if(CLRISNULL(devices)) throw gcnew ArgumentNullException("devices");
}

//---------------------------------------------------------------------------
// DeviceList::default[int]::get
//
// Gets the element at the specified index in the read-only list

Device^ DeviceList::default::get(int index)
{
	return m_devices[index];
}

//---------------------------------------------------------------------------
// DeviceList::Count::get
//
// Gets the number of elements in the collection

int DeviceList::Count::get(void)
{
	return m_devices->Count;
}

//---------------------------------------------------------------------------
// DeviceList::Create (static, internal)
//
// Creates a new DeviceList instance by executing a discovery
//
// Arguments:
//
//  method		- Discovery method to be used

DeviceList^ DeviceList::Create(DiscoveryMethod method)
{
	if(method == DiscoveryMethod::Broadcast) return DiscoverBroadcast();
	else if(method == DiscoveryMethod::Http) return DiscoverHttp();
	else throw gcnew ArgumentOutOfRangeException("method");
}

//---------------------------------------------------------------------------
// DeviceList::DiscoverBroadcast (static, private)
//
// Executes a broadcast device discovery
//
// Arguments:
//
//	NONE

DeviceList^ DeviceList::DiscoverBroadcast(void)
{
	List<Device^>^ discovered = gcnew List<Device^>();				// Collection of discovered devices

	// Allocate enough heap storage to hold up to 64 enumerated devices on the network
	std::unique_ptr<struct hdhomerun_discover_device_v3_t[]> devices(new struct hdhomerun_discover_device_v3_t[64]);

	// Use the libhdhomerun broadcast discovery mechanism to find all devices on the local network
	int result = hdhomerun_discover_find_devices_custom_v3(0, HDHOMERUN_DEVICE_TYPE_WILDCARD,
		HDHOMERUN_DEVICE_ID_WILDCARD, &devices[0], 64);
	if(result == -1) throw gcnew Exception("hdhomerun_discover_find_devices_custom_v3 failed");

	for(int index = 0; index < result; index++) {

		// Use the discovery JSON reported by the device as opposed to the data returned from UDP
		String^ discoverurl = String::Concat(gcnew String(devices[index].base_url), "/discover.json");

		// The web request will fail if the URL doesn't exist or doesn't return JSON; skip this device
		// if that's the case for now; should probably have some way to indicate this to the user
		try {

			JObject^ discovery = JsonWebRequest::GetObject(discoverurl);
			if(CLRISNOTNULL(discovery)) {

				// Gather enough information from the discovery data to determine how to proceed
				JToken^ deviceid = discovery->GetValue("DeviceID", StringComparison::OrdinalIgnoreCase);
				JToken^ storageid = discovery->GetValue("StorageID", StringComparison::OrdinalIgnoreCase);

				// Convert the numeric IP address into an IPAddress instance
				array<Byte>^ ipbytes = BitConverter::GetBytes(devices[index].ip_addr);
				if(BitConverter::IsLittleEndian) Array::Reverse(ipbytes);
				IPAddress^ ipaddress = gcnew IPAddress(ipbytes);

				// A single device may report both tuners and storage so check for both types and
				// process them as distinct device instances
				if(CLRISNOTNULL(deviceid)) discovered->Add(TunerDevice::Create(discovery, ipaddress));
				if(CLRISNOTNULL(storageid)) discovered->Add(StorageDevice::Create(discovery, ipaddress));
			}
		}

		catch(Exception^) { /* DO NOTHING */ }
	}

	// Sort the list of discovered devices prior to generating the DeviceList instance
	discovered->Sort(gcnew CompareDeviceById());

	return gcnew DeviceList(discovered);
}

//---------------------------------------------------------------------------
// DeviceList::DiscoverHttp (static, private)
//
// Executes an HTTP device discovery
//
// Arguments:
//
//	NONE

DeviceList^ DeviceList::DiscoverHttp(void)
{
	List<Device^>^ discovered = gcnew List<Device^>();				// Collection of discovered devices

	// The discovery JSON is returned as an array consisting of individual device objects
	JArray^ devices = JsonWebRequest::GetArray("https://ipv4-api.hdhomerun.com/discover");
	if(CLRISNOTNULL(devices)) {

		for each(JObject^ device in devices) {

			// Gather enough information from the discovery data to determine how to proceed
			JToken^ deviceid = device->GetValue("DeviceID", StringComparison::OrdinalIgnoreCase);
			JToken^ storageid = device->GetValue("StorageID", StringComparison::OrdinalIgnoreCase);
			JToken^ discoverurl = device->GetValue("DiscoverURL", StringComparison::OrdinalIgnoreCase);
			JToken^ localip = device->GetValue("LocalIP", StringComparison::OrdinalIgnoreCase);

			// Retrieve the individual device discovery data
			if(CLRISNOTNULL(discoverurl) && CLRISNOTNULL(localip)) {

				// HTTP discovery can return stale devices for up to 24 hours after they went dark,
				// for tuner devices we can run a quick check to see if they are still alive
				if(CLRISNOTNULL(deviceid) && (!TunerExists(localip->ToObject<String^>()))) continue;

				// The web request will fail if the URL doesn't exist or doesn't return JSON; skip this device
				// if that's the case for now; should probably have some way to indicate this to the user
				try {

					JObject^ discovery = JsonWebRequest::GetObject(discoverurl->ToObject<String^>());
					if(CLRISNOTNULL(discovery)) {

						// Convert the LocalIP string into an IPAddress instance
						IPAddress^ ipaddress = IPAddress::None;
						if(CLRISNOTNULL(localip)) {

							// Storage devices come in as IP:PORT, we only want the IP address
							array<String^>^ parts = localip->ToObject<String^>()->Split(':');
							if(parts->Length >= 1) IPAddress::TryParse(parts[0], ipaddress);
						}

						// A single device may report both tuners and storage so check for both types and
						// process them as distinct device instances
						if(CLRISNOTNULL(deviceid)) discovered->Add(TunerDevice::Create(discovery, ipaddress));
						if(CLRISNOTNULL(storageid)) discovered->Add(StorageDevice::Create(discovery, ipaddress));
					}
				}

				catch(Exception^) { /* DO NOTHING */ }
			}
		}
	}

	// Sort the list of discovered devices prior to generating the DeviceList instance
	discovered->Sort(gcnew CompareDeviceById());

	return gcnew DeviceList(discovered);
}

//---------------------------------------------------------------------------
// DeviceList::GetEnumerator
//
// Returns a generic IEnumerator<T> for the member collection
//
// Arguments:
//
//	NONE

IEnumerator<Device^>^ DeviceList::GetEnumerator(void)
{
	return gcnew ReadOnlyListEnumerator<Device^>(this);
}

//---------------------------------------------------------------------------
// DeviceList::IEnumerable_GetEnumerator
//
// Returns a non-generic IEnumerator for the member collection
//
// Arguments:
//
//	NONE

System::Collections::IEnumerator^ DeviceList::IEnumerable_GetEnumerator(void)
{
	return GetEnumerator();
}

//---------------------------------------------------------------------------
// DeviceList::TunerExists (static, private)
//
// Determines if a tuner device exists on the local network
//
// Arguments:
//
//	localip		- The IP address of the device returned from discovery

bool DeviceList::TunerExists(String^ localip)
{
	char			devicestr[128] = {};	// Device string (IP address)
	uint32_t		deviceid = 0;			// Device ID returned from inquiry

	// Convert the managed String instance into a char array for libhdhomerun
	msclr::auto_handle<msclr::interop::marshal_context> context(gcnew msclr::interop::marshal_context());
	strncpy_s(devicestr, std::extent<decltype(devicestr)>::value, context->marshal_as<char const*>(localip), _TRUNCATE);

	// Attempt to create a hdhomerun_device_t instance for the tuner instance.  In the
	// event this fails for some reason, assume the device exists
	hdhomerun_device_t* device = hdhomerun_device_create_from_str(devicestr, nullptr);
	if(device == nullptr) return true;

	// Attempt to get the Device ID for the tuner and release the device
	deviceid = hdhomerun_device_get_device_id(device);
	hdhomerun_device_destroy(device);

	// If the Device ID was retrieved successfully, the tuner is alive
	return (deviceid != 0);
}

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)
