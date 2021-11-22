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

#ifndef __LIVEBUFFER_H_
#define __LIVEBUFFER_H_
#pragma once

#pragma warning(push, 4)

using namespace System;
using namespace System::Net;

using namespace Newtonsoft::Json;
using namespace Newtonsoft::Json::Linq;

namespace zuki::hdhomeruntray::discovery {

//---------------------------------------------------------------------------
// Class LiveBuffer
//
// Describes an individual HDHomeRun live buffer
//---------------------------------------------------------------------------

public ref class LiveBuffer
{
public:

	//-----------------------------------------------------------------------
	// Properties

	// Name
	//
	// Gets the name of the live buffer
	property String^ Name
	{
		String^ get(void);
	}

	// TargetIP
	//
	// Gets the target IP address of the live buffer
	property IPAddress^ TargetIP
	{
		IPAddress^ get(void);
	}

internal:

	//-----------------------------------------------------------------------
	// Internal Member Functions

	// Create
	//
	// Creates a new LiveBuffer instance
	static LiveBuffer^ Create(JObject^ livebuffer);

private:

	// Instance Constructor
	//
	LiveBuffer(JObject^ livebuffer);

	//-----------------------------------------------------------------------
	// Member Variables

	String^				m_name;				// The live buffer name
	IPAddress^			m_targetip;			// The target IP address
};

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)

#endif	// __LIVEBUFFER_H_
