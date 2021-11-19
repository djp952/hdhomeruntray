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

#include "ReadOnlyListEnumerator.h"

#pragma warning(push, 4)

//---------------------------------------------------------------------------
// ReadOnlyListEnumerator Constructor
//
// Arguments:
//
//	collection		- IReadOnlyList<> based collection to be enumerated

generic<typename T>
ReadOnlyListEnumerator<T>::ReadOnlyListEnumerator(IReadOnlyList<T>^ collection) : m_collection(collection)
{
	if(CLRISNULL(collection)) throw gcnew ArgumentNullException("collection");
}

//---------------------------------------------------------------------------
// ReadOnlyListEnumerator Destructor

generic<typename T>
ReadOnlyListEnumerator<T>::~ReadOnlyListEnumerator()
{
	if(m_disposed) return;

	m_collection = nullptr;			// Release our reference to the collection
	m_disposed = true;				// Object is now in a disposed state
}

//---------------------------------------------------------------------------
// ReadOnlyListEnumerator::Current::get
//
// Gets the current element in the collection

generic<typename T>
T ReadOnlyListEnumerator<T>::Current::get(void)
{
	CHECK_DISPOSED(m_disposed);

	if((m_index < 0) || (m_index >= m_collection->Count)) throw gcnew InvalidOperationException();
	return m_collection[m_index];
}

//---------------------------------------------------------------------------
// ReadOnlyListEnumerator::IEnumerator_Current::get
//
// Gets the current element in the collection as an untyped Object^

generic<typename T>
Object^ ReadOnlyListEnumerator<T>::IEnumerator_Current::get(void)
{
	CHECK_DISPOSED(m_disposed);
	return Current::get();
}

//---------------------------------------------------------------------------
// ReadOnlyListEnumerator::MoveNext
//
// Advances the enumerator to the next element of the collection
//
// Arguments:
//
//	NONE

generic<typename T>
bool ReadOnlyListEnumerator<T>::MoveNext(void)
{
	CHECK_DISPOSED(m_disposed);
	return (++m_index < m_collection->Count);
}

//---------------------------------------------------------------------------
// ReadOnlyListEnumerator::Reset
//
// Sets the enumerator to its initial position
//
// Arguments:
//
//	NONE

generic<typename T>
void ReadOnlyListEnumerator<T>::Reset(void)
{
	CHECK_DISPOSED(m_disposed);
	m_index = -1;
}

//---------------------------------------------------------------------------

#pragma warning(pop)
