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

#include "RecordingList.h"
#include "ReadOnlyListEnumerator.h"

#pragma warning(push, 4)

using namespace System::Diagnostics;

namespace zuki::hdhomeruntray::discovery {

//---------------------------------------------------------------------------
// RecordingList Constructor (private)
//
// Arguments:
//
//	recordings		- List<> containing the recordings

RecordingList::RecordingList(List<Recording^>^ recordings) : m_recordings(recordings)
{
	if(CLRISNULL(recordings)) throw gcnew ArgumentNullException("recordings");
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
// RecordingList::Create (internal, static)
//
// Creates a new RecordingList instance by executing a discovery
//
// Arguments:
//
//  recordings	- List<> of Recording instances to back the collection

RecordingList^ RecordingList::Create(List<Recording^>^ recordings)
{
	if(CLRISNULL(recordings)) throw gcnew ArgumentNullException("recordings");

	return gcnew RecordingList(recordings);
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
// RecordingList::GetHashCode
//
// Serves as the default hash function
//
// Arguments:
//
//	NONE

int RecordingList::GetHashCode(void)
{
	Debug::Assert(m_recordings != nullptr);

	// 32-bit FNV-1a primes (http://www.isthe.com/chongo/tech/comp/fnv/index.html#FNV-source) 
	const int fnv_offset_basis = 2166136261U;
	const int fnv_prime = 16777619U;

	int hash = fnv_offset_basis;

	// Start with the recordings count
	hash ^= CLRISNULL(m_recordings) ? 0 : m_recordings->Count;
	hash *= fnv_prime;

	// Hash against each of the individual Recording instances
	for each(Recording^ recording in m_recordings)
	{
		hash ^= CLRISNULL(recording) ? 0 : recording->GetHashCode();
		hash *= fnv_prime;
	}

	return hash;
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
