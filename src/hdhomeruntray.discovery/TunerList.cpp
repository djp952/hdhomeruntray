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
#include "TunerList.h"
#include "ReadOnlyListEnumerator.h"

using namespace Newtonsoft::Json;
using namespace Newtonsoft::Json::Linq;

#pragma warning(push, 4)

namespace zuki::hdhomeruntray::discovery {

//---------------------------------------------------------------------------
// TunerList Constructor (private)
//
// Arguments:
//
//	tuners		- List<> containing the tuners

TunerList::TunerList(List<Tuner^>^ tuners) : m_tuners(tuners)
{
	if(CLRISNULL(tuners)) throw gcnew ArgumentNullException("tuners");
}

//---------------------------------------------------------------------------
// TunerList::default[int]::get
//
// Gets the element at the specified index in the read-only list

Tuner^ TunerList::default::get(int index)
{
	return m_tuners[index];
}

//---------------------------------------------------------------------------
// TunerList::Count::get
//
// Gets the number of elements in the collection

int TunerList::Count::get(void)
{
	return m_tuners->Count;
}

//---------------------------------------------------------------------------
// TunerList::Create (static)
//
// Creates a new TunerList instance by executing a discovery
//
// Arguments:
//
//  statusurl		- URL of the status JSON for the tuner device

TunerList^ TunerList::Create(String^ statusurl)
{
	if(CLRISNULL(statusurl)) throw gcnew ArgumentNullException("statusurl");

	List<Tuner^>^ discovered = gcnew List<Tuner^>();			// Collection of discovered tuners

	JArray^ tuners = JsonWebRequest::GetArray(statusurl);
	if(CLRISNOTNULL(tuners)) {

		// Create a Tuner object for each individual array object
		for each(JObject^ tuner in tuners) discovered->Add(Tuner::Create(tuner));
	}

	// Return the generated List<> as a new TunerList instance
	return gcnew TunerList(discovered);
}

//---------------------------------------------------------------------------
// TunerList::GetEnumerator
//
// Returns a generic IEnumerator<T> for the member collection
//
// Arguments:
//
//	NONE

IEnumerator<Tuner^>^ TunerList::GetEnumerator(void)
{
	return gcnew ReadOnlyListEnumerator<Tuner^>(this);
}

//---------------------------------------------------------------------------
// TunerList::IEnumerable_GetEnumerator
//
// Returns a non-generic IEnumerator for the member collection
//
// Arguments:
//
//	NONE

System::Collections::IEnumerator^ TunerList::IEnumerable_GetEnumerator(void)
{
	return GetEnumerator();
}

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)
