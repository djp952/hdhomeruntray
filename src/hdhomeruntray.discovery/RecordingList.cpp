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

#include "JsonWebRequest.h"
#include "RecordingList.h"
#include "ReadOnlyListEnumerator.h"

using namespace Newtonsoft::Json;
using namespace Newtonsoft::Json::Linq;

#pragma warning(push, 4)

namespace zuki::hdhomeruntray::discovery {

//---------------------------------------------------------------------------
// RecordingList Constructor (private)
//
// Arguments:
//
//	recordings		- List<> containing the recordings

RecordingList::RecordingList(List<Recording^>^ recordings) : m_recordings(recordings)
{
	if(Object::ReferenceEquals(recordings, nullptr)) throw gcnew ArgumentNullException("recordings");
}

//---------------------------------------------------------------------------
// RecordingList::default[int]::get
//
// Gets the element at the specified index in the read-only list

Recording^ RecordingList::default::get(int index)
{
	return m_recordings[index];
}

//---------------------------------------------------------------------------
// RecordingList::Count::get
//
// Gets the number of elements in the collection

int RecordingList::Count::get(void)
{
	return m_recordings->Count;
}

//---------------------------------------------------------------------------
// RecordingList::Create (static)
//
// Creates a new RecordingList instance by executing a discovery
//
// Arguments:
//
//  statusurl		- URL of the status JSON for the tuner device

RecordingList^ RecordingList::Create(String^ statusurl)
{
	if(Object::ReferenceEquals(statusurl, nullptr)) throw gcnew ArgumentNullException("statusurl");

	List<Recording^>^ discovered = gcnew List<Recording^>();			// Collection of discovered recordings

	// The status JSON from the RECORD engine contains two types of objects; objects
	// that represent active recordings has "Resource":"record".  The other type is
	// a Live TV buffer, this is "Resource":"live"
	JArray^ recordings = JsonWebRequest::GetArray(statusurl);
	if(!Object::ReferenceEquals(recordings, nullptr)) {

		for each(JObject^ recording in recordings) {

			JToken^ resource = recording->GetValue("Resource", StringComparison::OrdinalIgnoreCase);
			if((!Object::ReferenceEquals(resource, nullptr)) && (String::Compare(resource->ToObject<String^>(), "record",
				StringComparison::OrdinalIgnoreCase) == 0)) discovered->Add(Recording::Create(recording));
		}
	}

	// Return the generated List<> as a new RecordingList instance
	return gcnew RecordingList(discovered);
}

//---------------------------------------------------------------------------
// RecordingList::GetEnumerator
//
// Returns a generic IEnumerator<T> for the member collection
//
// Arguments:
//
//	NONE

IEnumerator<Recording^>^ RecordingList::GetEnumerator(void)
{
	return gcnew ReadOnlyListEnumerator<Recording^>(this);
}

//---------------------------------------------------------------------------
// RecordingList::IEnumerable_GetEnumerator
//
// Returns a non-generic IEnumerator for the member collection
//
// Arguments:
//
//	NONE

System::Collections::IEnumerator^ RecordingList::IEnumerable_GetEnumerator(void)
{
	return GetEnumerator();
}

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)
