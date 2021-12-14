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

#include "StorageStatus.h"

#include "DeviceStatusColor.h"
#include "JsonWebRequest.h"
#include "StorageDevice.h"

#pragma warning(push, 4)

using namespace Newtonsoft::Json;
using namespace Newtonsoft::Json::Linq;

namespace zuki::hdhomeruntray::discovery {

//---------------------------------------------------------------------------
// StorageStatus Constructor (private)
//
// Arguments:
//
//	livebuffers		- List<> of live buffers
//	playbacks		- List<> of active playbacks
//	recordings		- List<> of active recordings

StorageStatus::StorageStatus(LiveBufferList^ livebuffers, PlaybackList^ playbacks, RecordingList^ recordings) : 
	m_statuscolor(DeviceStatusColor::Gray), m_livebuffers(livebuffers), m_playbacks(playbacks),  m_recordings(recordings)
{
	if(CLRISNULL(livebuffers)) throw gcnew ArgumentNullException("livebuffers");
	if(CLRISNULL(playbacks)) throw gcnew ArgumentNullException("playbacks");
	if(CLRISNULL(recordings)) throw gcnew ArgumentNullException("recordings");

	// If there are live TV streams being buffered or recording playbacks,
	// report the status as green
	if((m_livebuffers->Count > 0) || (m_playbacks->Count > 0)) m_statuscolor = DeviceStatusColor::Green;

	// If there are active recordings in progress, report the status as red
	if(m_recordings->Count > 0) m_statuscolor = DeviceStatusColor::Red;
}

//---------------------------------------------------------------------------
// StorageStatus::Create (internal)
//
// Creates a new StorageStatus instance
//
// Arguments:
//
//	storagedevice	- Reference to the parent StorageDevice object

StorageStatus^ StorageStatus::Create(StorageDevice^ storagedevice)
{
	if(CLRISNULL(storagedevice)) throw gcnew ArgumentNullException("storagedevice");

	// Generate the URL to the device status JSON data
	String^ statusurl = String::Concat(storagedevice->BaseURL, "/status.json");

	List<LiveBuffer^>^ livebuffers = gcnew List<LiveBuffer^>();		// Collection of Live TV buffers
	List<Playback^>^ playbacks = gcnew List<Playback^>();			// Collection of active playbacks
	List<Recording^>^ recordings = gcnew List<Recording^>();		// Collection of active recordings

	// The status JSON from the RECORD engine contains three types of objects; "Resource":"record", "Resource":"playback" and "Resource":"live"
	JArray^ resources = JsonWebRequest::GetArray(statusurl);
	if(CLRISNOTNULL(resources)) {

		for each(JObject^ resource in resources) {

			JToken^ resourcetoken = resource->GetValue("Resource", StringComparison::OrdinalIgnoreCase);
			if(CLRISNOTNULL(resourcetoken)) {

				String^ resourcetype = resourcetoken->ToObject<String^>();
				if(CLRISNOTNULL(resourcetype)) {

					if(String::Compare(resourcetype, "record", StringComparison::OrdinalIgnoreCase) == 0) recordings->Add(Recording::Create(resource));
					else if(String::Compare(resourcetype, "playback", StringComparison::OrdinalIgnoreCase) == 0) playbacks->Add(Playback::Create(resource));
					else if(String::Compare(resourcetype, "live", StringComparison::OrdinalIgnoreCase) == 0) livebuffers->Add(LiveBuffer::Create(resource));
				}
			}
		}
	}

	return gcnew StorageStatus(LiveBufferList::Create(livebuffers), PlaybackList::Create(playbacks), RecordingList::Create(recordings));
}

//---------------------------------------------------------------------------
// StorageStatus::GetHashCode
//
// Serves as the default hash function
//
// Arguments:
//
//	NONE

int StorageStatus::GetHashCode(void)
{
	// 32-bit FNV-1a primes (http://www.isthe.com/chongo/tech/comp/fnv/index.html#FNV-source) 
	const int fnv_offset_basis = 2166136261U;
	const int fnv_prime = 16777619U;

	int hash = fnv_offset_basis;

	// FNV hash each member variable
	hash ^= m_statuscolor.GetHashCode();
	hash *= fnv_prime;
	hash ^= CLRISNULL(m_livebuffers) ? 0 : m_livebuffers->GetHashCode();
	hash *= fnv_prime;
	hash ^= CLRISNULL(m_recordings) ? 0 : m_recordings->GetHashCode();
	hash *= fnv_prime;

	return hash;
}

//---------------------------------------------------------------------------
// StorageStatus::LiveBuffers::get
//
// Gets the collection of active live buffers

LiveBufferList^ StorageStatus::LiveBuffers::get(void)
{
	CLRASSERT(CLRISNOTNULL(m_livebuffers));
	return m_livebuffers;
}

//---------------------------------------------------------------------------
// StorageStatus::Playbacks::get
//
// Gets the collection of active recording playbacks

PlaybackList^ StorageStatus::Playbacks::get(void)
{
	CLRASSERT(CLRISNOTNULL(m_playbacks));
	return m_playbacks;
}

//---------------------------------------------------------------------------
// StorageStatus::Recordings::get
//
// Gets the collection of active recordings

RecordingList^ StorageStatus::Recordings::get(void)
{
	CLRASSERT(CLRISNOTNULL(m_recordings));
	return m_recordings;
}

//---------------------------------------------------------------------------
// StorageStatus::StatusColor::get
//
// Gets the color code for the overall status

Color StorageStatus::StatusColor::get(void)
{
	return m_statuscolor;
}

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)
