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

#include "LiveBuffer.h"

#pragma warning(push, 4)

namespace zuki::hdhomeruntray::discovery {

//---------------------------------------------------------------------------
// LiveBuffer Constructor (private)
//
// Arguments:
//
//	livebuffer		- Reference to the JSON status data for the live buffer

LiveBuffer::LiveBuffer(JObject^ livebuffer) : m_targetip(IPAddress::None)
{
	if(CLRISNULL(livebuffer)) throw gcnew ArgumentNullException("livebuffer");

	// The only things in a live buffer are the name and target IP address
	JToken^ name = livebuffer->GetValue("Name", StringComparison::OrdinalIgnoreCase);
	JToken^ targetip = livebuffer->GetValue("TargetIP", StringComparison::OrdinalIgnoreCase);

	m_name = CLRISNOTNULL(name) ? name->ToObject<String^>() : String::Empty;
	if(CLRISNOTNULL(targetip)) IPAddress::TryParse(targetip->ToObject<String^>(), m_targetip);
}

//---------------------------------------------------------------------------
// LiveBuffer::Create (internal)
//
// Creates a new LiveBuffer instance
//
// Arguments:
//
//	livebuffer		- Reference to the JSON status data for the live buffer

LiveBuffer^ LiveBuffer::Create(JObject^ livebuffer)
{
	if(CLRISNULL(livebuffer)) throw gcnew ArgumentNullException("livebuffer");
	return gcnew LiveBuffer(livebuffer);
}

//---------------------------------------------------------------------------
// LiveBuffer::Name::get
//
// Gets the name of the live buffer

String^ LiveBuffer::Name::get(void)
{
	return m_name;
}

//---------------------------------------------------------------------------
// LiveBuffer::TargetIP::get
//
// Gets the target IP address of the live buffer

IPAddress^ LiveBuffer::TargetIP::get(void)
{
	return m_targetip;
}

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)
