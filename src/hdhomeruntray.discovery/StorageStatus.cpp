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
//	livebuffers		- List<> if live buffers
//	recordings		- List<> of active recordings

StorageStatus::StorageStatus(List<String^>^ livebuffers, List<Recording^>^ recordings) : m_statuscolor(COLOR_GRAY)
{
	// If there are live TV streams being buffered, report the status as green
	if(livebuffers->Count > 0) m_statuscolor = COLOR_GREEN;

	// If there are active recordings in progress, report the status as red
	if(recordings->Count > 0) m_statuscolor = COLOR_RED;
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

	List<String^>^ livebuffers = gcnew List<String^>();			// Collection of Live TV buffers
	List<Recording^>^ recordings = gcnew List<Recording^>();	// Collection of discovered recordings

	// The status JSON from the RECORD engine contains two types of objects; objects that represent 
	// active recordings has "Resource":"record".  The other type is a Live TV buffer, this is "Resource":"live"
	JArray^ resources = JsonWebRequest::GetArray(statusurl);
	if(CLRISNOTNULL(resources)) {

		for each(JObject^ resource in resources) {

			JToken^ resourcetoken = resource->GetValue("Resource", StringComparison::OrdinalIgnoreCase);
			if(CLRISNOTNULL(resourcetoken)) {

				String^ resourcetype = resourcetoken->ToObject<String^>();
				if(CLRISNOTNULL(resourcetype)) {

					if(String::Compare(resourcetype, "record", StringComparison::OrdinalIgnoreCase) == 0) recordings->Add(Recording::Create(resource));
					else if(String::Compare(resourcetype, "live", StringComparison::OrdinalIgnoreCase) == 0) livebuffers->Add("TODO");
				}
			}
		}
	}

	return gcnew StorageStatus(livebuffers, recordings);
}

//---------------------------------------------------------------------------
// StorageStatus::StatusColor::get
//
// Gets the color code for the overall status

Color StorageStatus::StatusColor::get(void)
{
	return Color::FromArgb(m_statuscolor);
}

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)
