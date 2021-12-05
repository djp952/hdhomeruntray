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

#ifndef __LIVEBUFFERLIST_H_
#define __LIVEBUFFERLIST_H_
#pragma once

#include "LiveBuffer.h"

#pragma warning(push, 4)

using namespace System;
using namespace System::Collections::Generic;

namespace zuki::hdhomeruntray::discovery {

//---------------------------------------------------------------------------
// Class LiveBufferList
//
// Implements an IReadOnlyList<> based enumerable collection of live buffers
//---------------------------------------------------------------------------

public ref class LiveBufferList : public IReadOnlyList<LiveBuffer^>
{
public:

	//-----------------------------------------------------------------------
	// Member Functions

	// GetEnumerator
	//
	// Returns a generic IEnumerator<T> for the member collection
	virtual IEnumerator<LiveBuffer^>^ GetEnumerator(void);

	//-----------------------------------------------------------------------
	// Properties

	// default[int]
	//
	// Gets the element at the specified index in the read-only list
	property LiveBuffer^ default[int]
	{
		virtual LiveBuffer^ get(int index);
	}

	// Count
	//
	// Gets the number of elements in the collection
	property int Count
	{
		virtual int get();
	}

internal:

	//-----------------------------------------------------------------------
	// Internal Member Functions

	// Create (static)
	//
	// Creates a new LiveBufferList instance
	static LiveBufferList^ Create(List<LiveBuffer^>^ livebuffers);

	//-----------------------------------------------------------------------
	// Internal Fields

	// Empty (static)
	//
	// Returns an empty device collection instance
	static initonly LiveBufferList^ Empty = gcnew LiveBufferList(gcnew List<LiveBuffer^>());

private:

	// Instance Constructor
	//
	LiveBufferList(List<LiveBuffer^>^ livebuffers);

	//-----------------------------------------------------------------------
	// Private Member Functions

	// GetEnumerator (IEnumerable)
	//
	// Returns a non-generic IEnumerator for the member collection
	virtual System::Collections::IEnumerator^ IEnumerable_GetEnumerator(void) sealed = System::Collections::IEnumerable::GetEnumerator;

	//-----------------------------------------------------------------------
	// Member Variables

	List<LiveBuffer^>^		m_livebuffers;		// Underlying collection
};

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)

#endif	// __LIVEBUFFERLIST_H_