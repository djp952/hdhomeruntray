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

#include "PlaybackList.h"
#include "ReadOnlyListEnumerator.h"

#pragma warning(push, 4)

using namespace System::Diagnostics;

namespace zuki::hdhomeruntray::discovery {

//---------------------------------------------------------------------------
// PlaybackList Constructor (private)
//
// Arguments:
//
//	playbacks		- List<> containing the live buffers

PlaybackList::PlaybackList(List<Playback^>^ playbacks) : m_livebuffers(playbacks)
{
	if(CLRISNULL(playbacks)) throw gcnew ArgumentNullException("playbacks");
}

//---------------------------------------------------------------------------
// PlaybackList::default[int]::get
//
// Gets the element at the specified index in the read-only list

Playback^ PlaybackList::default::get(int index)
{
	return m_livebuffers[index];
}

//---------------------------------------------------------------------------
// PlaybackList::Count::get
//
// Gets the number of elements in the collection

int PlaybackList::Count::get(void)
{
	return m_livebuffers->Count;
}

//---------------------------------------------------------------------------
// PlaybackList::Create (internal, static)
//
// Creates a new PlaybackList instance by executing a discovery
//
// Arguments:
//
//  playbacks	- List<> of Playback instances to back the collection

PlaybackList^ PlaybackList::Create(List<Playback^>^ playbacks)
{
	if(CLRISNULL(playbacks)) throw gcnew ArgumentNullException("playbacks");

	return gcnew PlaybackList(playbacks);
}

//---------------------------------------------------------------------------
// PlaybackList::GetEnumerator
//
// Returns a generic IEnumerator<T> for the member collection
//
// Arguments:
//
//	NONE

IEnumerator<Playback^>^ PlaybackList::GetEnumerator(void)
{
	return gcnew ReadOnlyListEnumerator<Playback^>(this);
}

//---------------------------------------------------------------------------
// PlaybackList::GetHashCode
//
// Serves as the default hash function
//
// Arguments:
//
//	NONE

int PlaybackList::GetHashCode(void)
{
	Debug::Assert(m_livebuffers != nullptr);

	// 32-bit FNV-1a primes (http://www.isthe.com/chongo/tech/comp/fnv/index.html#FNV-source) 
	const int fnv_offset_basis = 2166136261U;
	const int fnv_prime = 16777619U;

	int hash = fnv_offset_basis;

	// Start with the live buffers count
	hash ^= CLRISNULL(m_livebuffers) ? 0 : m_livebuffers->Count;
	hash *= fnv_prime;

	// Hash against each of the individual Playback instances
	for each(Playback^ livebuffer in m_livebuffers)
	{
		hash ^= CLRISNULL(livebuffer) ? 0 : livebuffer->GetHashCode();
		hash *= fnv_prime;
	}

	return hash;
}

//---------------------------------------------------------------------------
// PlaybackList::IEnumerable_GetEnumerator
//
// Returns a non-generic IEnumerator for the member collection
//
// Arguments:
//
//	NONE

System::Collections::IEnumerator^ PlaybackList::IEnumerable_GetEnumerator(void)
{
	return GetEnumerator();
}

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)
