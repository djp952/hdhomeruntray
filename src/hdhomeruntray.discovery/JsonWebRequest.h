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

#ifndef __JSONWEBREQUEST_H_
#define __JSONWEBREQUEST_H_
#pragma once

#pragma warning(push, 4)

using namespace System;
using namespace System::Net::Http;
using namespace System::Threading;

using namespace Newtonsoft::Json;
using namespace Newtonsoft::Json::Linq;

//---------------------------------------------------------------------------
// Class JsonWebRequest (internal)
//
// Helper class to read JSON data over HTTP
//---------------------------------------------------------------------------

ref class JsonWebRequest
{
public:

	//-----------------------------------------------------------------------
	// Member Functions

	// GetArray (static)
	//
	// Retrieves a JSON array from the provided URI
	static JArray^ GetArray(String^ uri);
	static JArray^ GetArray(String^ uri, CancellationToken cancel);

	// GetObject (static)
	//
	// Retrieves a JSON object from the provided URI
	static JObject^ GetObject(String^ uri);
	static JObject^ GetObject(String^ uri, CancellationToken cancel);

private:

	// Static Constructor
	//
	static JsonWebRequest::JsonWebRequest();

	//-----------------------------------------------------------------------
	// Private Member Functions

	// Get
	//
	// Generic implementation of the GetX functions
	generic<class T> static T Get(String^ uri, CancellationToken cancel);

	//-----------------------------------------------------------------------
	// Member Variables

	static HttpClient^	s_httpclient;		// Underlying HttpClient instance
};

//---------------------------------------------------------------------------

#pragma warning(pop)

#endif	// __JSONWEBREQUEST_H_
