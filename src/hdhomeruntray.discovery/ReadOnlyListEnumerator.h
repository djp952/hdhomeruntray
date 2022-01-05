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

#ifndef __READONLYLISTENUMERATOR_H_
#define __READONLYLISTENUMERATOR_H_
#pragma once

#pragma warning(push, 4)

using namespace System;
using namespace System::Collections::Generic;

//---------------------------------------------------------------------------
// Class ReadOnlyListEnumerator (internal)
//
// Implements a generic enumerator for IReadOnlyList<>-based collections
//---------------------------------------------------------------------------

generic<typename T>
ref class ReadOnlyListEnumerator : public IEnumerator<T>
{
public:

	// Instance Constructor
	//
	ReadOnlyListEnumerator(IReadOnlyList<T>^ collection);

	//-----------------------------------------------------------------------
	// Member Functions

	// MoveNext
	//
	// Advances the enumerator to the next element of the collection
	virtual bool MoveNext(void);

	// Reset
	//
	// Sets the enumerator to its initial position 
	virtual void Reset(void);

	//-----------------------------------------------------------------------
	// Properties

	// Current
	//
	// Gets the current element in the collection
	property T Current
	{
		virtual T get(void);
	}

private:

	// Destructor
	//
	~ReadOnlyListEnumerator();

	//-----------------------------------------------------------------------
	// Private Properties

	// IEnumerator_Current (IEnumerator)
	//
	// Gets the current element in the collection as an untyped Object^
	property Object^ IEnumerator_Current
	{
		virtual Object^ get(void) sealed = System::Collections::IEnumerator::Current::get;
	}

	//-----------------------------------------------------------------------
	// Member Variables

	IReadOnlyList<T>^		m_collection;	// Referenced collection
	bool					m_disposed;		// Object disposal flag
	int						m_index = -1;	// Current index into collection
};

//---------------------------------------------------------------------------

#pragma warning(pop)

#endif	// __READONLYLISTENUMERATOR_H_
