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

#include "LiveBufferList.h"
#include "ReadOnlyListEnumerator.h"

#pragma warning(push, 4)

namespace zuki::hdhomeruntray::discovery {

//---------------------------------------------------------------------------
// LiveBufferList Constructor (private)
//
// Arguments:
//
//	livebuffers		- List<> containing the live buffers

LiveBufferList::LiveBufferList(List<LiveBuffer^>^ livebuffers) : m_livebuffers(livebuffers)
{
	if(CLRISNULL(livebuffers)) throw gcnew ArgumentNullException("livebuffers");
}

//---------------------------------------------------------------------------
// LiveBufferList::default[int]::get
//
// Gets the element at the specified index in the read-only list

LiveBuffer^ LiveBufferList::default::get(int index)
{
	return m_livebuffers[index];
}

//---------------------------------------------------------------------------
// LiveBufferList::Count::get
//
// Gets the number of elements in the collection

int LiveBufferList::Count::get(void)
{
	return m_livebuffers->Count;
}

//---------------------------------------------------------------------------
// LiveBufferList::Create (internal, static)
//
// Creates a new LiveBufferList instance by executing a discovery
//
// Arguments:
//
//  livebuffers	- List<> of LiveBuffer instances to back the collection

LiveBufferList^ LiveBufferList::Create(List<LiveBuffer^>^ livebuffers)
{
	if(CLRISNULL(livebuffers)) throw gcnew ArgumentNullException("livebuffers");

	return gcnew LiveBufferList(livebuffers);
}

//---------------------------------------------------------------------------
// LiveBufferList::GetEnumerator
//
// Returns a generic IEnumerator<T> for the member collection
//
// Arguments:
//
//	NONE

IEnumerator<LiveBuffer^>^ LiveBufferList::GetEnumerator(void)
{
	return gcnew ReadOnlyListEnumerator<LiveBuffer^>(this);
}

//---------------------------------------------------------------------------
// LiveBufferList::GetHashCode
//
// Serves as the default hash function
//
// Arguments:
//
//	NONE

int LiveBufferList::GetHashCode(void)
{
	// 32-bit FNV-1a primes (http://www.isthe.com/chongo/tech/comp/fnv/index.html#FNV-source) 
	const int fnv_offset_basis = 2166136261U;
	const int fnv_prime = 16777619U;

	int hash = fnv_offset_basis;

	// Start with the live buffers count
	hash ^= m_livebuffers->Count;
	hash *= fnv_prime;

	// Hash against each of the individual LiveBuffer instances
	for each(LiveBuffer^ livebuffer in m_livebuffers)
	{
		hash ^= livebuffer->GetHashCode();
		hash *= fnv_prime;
	}

	return hash;
}

//---------------------------------------------------------------------------
// LiveBufferList::IEnumerable_GetEnumerator
//
// Returns a non-generic IEnumerator for the member collection
//
// Arguments:
//
//	NONE

System::Collections::IEnumerator^ LiveBufferList::IEnumerable_GetEnumerator(void)
{
	return GetEnumerator();
}

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)
