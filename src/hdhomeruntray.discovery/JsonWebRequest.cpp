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

#include "JsonWebRequest.h"

using namespace System::IO;
using namespace System::Text;
using namespace System::Threading;
using namespace System::Threading::Tasks;

#pragma warning(push, 4)

//---------------------------------------------------------------------------
// JsonWebRequest Constructor (static, private)

static JsonWebRequest::JsonWebRequest()
{
	s_httpclient = gcnew HttpClient();

	// Nothing this application does should take more than 2.5 seconds to complete
	s_httpclient->Timeout = TimeSpan::FromMilliseconds(2500);
}

//---------------------------------------------------------------------------
// JsonWebRequest::GetArray (static)
//
// Retrieves a JSON array from the provided URI
//
// Arguments:
//
//	uri		- URI from which to retrieve the JSON data

JArray^ JsonWebRequest::GetArray(String^ uri)
{
	// Use a dummy cancellation source to provide a cancellation token
	msclr::auto_handle<CancellationTokenSource> source(gcnew CancellationTokenSource());
	return Get<JArray^>(uri, source->Token);
}

//---------------------------------------------------------------------------
// JsonWebRequest::GetArray
//
// Retrieves a JSON array from the provided URI
//
// Arguments:
//
//	uri		- URI from which to retrieve the JSON data
//	cancel	- Cancellation token to cancel the operation 

JArray^ JsonWebRequest::GetArray(String^ uri, CancellationToken cancel)
{
	return Get<JArray^>(uri, cancel);
}

//---------------------------------------------------------------------------
// JsonWebRequest::GetObject (static)
//
// Retrieves a JSON object from the provided URI
//
// Arguments:
//
//	uri		- URI from which to retrieve the JSON data

JObject^ JsonWebRequest::GetObject(String^ uri)
{
	// Use a dummy cancellation source to provide a cancellation token
	msclr::auto_handle<CancellationTokenSource> source(gcnew CancellationTokenSource());
	return Get<JObject^>(uri, source->Token);
}

//---------------------------------------------------------------------------
// JsonWebRequest::GetObject (static)
//
// Retrieves a JSON object from the provided URI
//
// Arguments:
//
//	uri		- URI from which to retrieve the JSON data
//	cancel	- Cancellation token to cancel the operation

JObject^ JsonWebRequest::GetObject(String^ uri, CancellationToken cancel)
{
	return Get<JObject^>(uri, cancel);
}

//---------------------------------------------------------------------------
// JsonWebRequest::Get (private)
//
// Generic implementation of the GetX functions
//
// Arguments:
//
//	uri		- URI from which to retrieve the JSON data
//	cancel	- Cancellation token to cancel the operation

generic<class T> T JsonWebRequest::Get(String^ uri, CancellationToken cancel)
{
	if(String::IsNullOrEmpty(uri)) throw gcnew ArgumentNullException("uri");

	// Pass the cancellation token onto the HttpClient and watch for TaskCanceledException (via AggregateException)
	Task<HttpResponseMessage^>^ gettask = s_httpclient->GetAsync(uri, HttpCompletionOption::ResponseHeadersRead, cancel);
	
	try {

		gettask->Result->EnsureSuccessStatusCode();
		gettask->Wait();
	}

	catch(AggregateException^) { return T(); }
	catch(TaskCanceledException^) { return T(); }

	// If the returned data was of type application/json, access the stream data and deserialize it
	if(CLRISNOTNULL(gettask->Result->Content) && gettask->Result->Content->Headers->ContentType->MediaType == "application/json")
	{
		// Watch out for TaskCanceledException (via AggregateException) here as well
		try {

			Task<Stream^>^ streamtask = gettask->Result->Content->ReadAsStreamAsync();
			streamtask->Wait();

			msclr::auto_handle<TextReader> textreader(gcnew StreamReader(streamtask->Result));
			msclr::auto_handle<JsonTextReader> jsonreader(gcnew JsonTextReader(textreader.get()));

			JsonSerializer^ serializer = gcnew JsonSerializer();
			return serializer->Deserialize<T>(jsonreader.get());
		}

		catch(AggregateException^) { return T(); }
		catch(TaskCanceledException^) { return T(); }
	}

	return T();						// default(T)
}

//---------------------------------------------------------------------------

#pragma warning(pop)
