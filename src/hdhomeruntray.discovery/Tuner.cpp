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

#include "Tuner.h"

#pragma warning(push, 4)

namespace zuki::hdhomeruntray::discovery {

//---------------------------------------------------------------------------
// Tuner Constructor (private)
//
// Arguments:
//
//	tuner		- Reference to the JSON status data for the tuner

Tuner::Tuner(JObject^ tuner)
{
	if(Object::ReferenceEquals(tuner, nullptr)) throw gcnew ArgumentNullException("tuner");

	// TODO: All of these tokens will need to be available at some point; need to check an ATSC tuner
	// to make sure they will be available for every device ...
	JToken^ resource = tuner->GetValue("Resource", StringComparison::OrdinalIgnoreCase);
	//JToken^ vctnumber = tuner->GetValue("VctNumber", StringComparison::OrdinalIgnoreCase);
	//JToken^ vctname = tuner->GetValue("VctName", StringComparison::OrdinalIgnoreCase);
	JToken^ frequency = tuner->GetValue("Frequency", StringComparison::OrdinalIgnoreCase);
	//JToken^ signalstrengthpercent = tuner->GetValue("SignalStrengthPercent", StringComparison::OrdinalIgnoreCase);
	//JToken^ signalqualitypercent = tuner->GetValue("SignalQualityPercent", StringComparison::OrdinalIgnoreCase);
	//JToken^ symbolqualitypercent = tuner->GetValue("SymbolQualityPercent", StringComparison::OrdinalIgnoreCase);
	JToken^ targetip = tuner->GetValue("TargetIP", StringComparison::OrdinalIgnoreCase);

	// The tuner index is presented in the JSON as "Resource":"tunerX", but the "X" is the value we want here,
	// treat the token as a string that can be parsed into an int if the "tuner" portion is removed
	if(!Object::ReferenceEquals(resource, nullptr)) int::TryParse(resource->ToObject<String^>()->Replace("tuner", ""), m_index);

	m_frequency = (!Object::ReferenceEquals(frequency, nullptr)) ? frequency->ToObject<__int64>() : -1;
	m_targetip = (!Object::ReferenceEquals(targetip, nullptr)) ? targetip->ToObject<String^>() : String::Empty;
}

//---------------------------------------------------------------------------
// Tuner::Create (internal)
//
// Creates a new Tuner instance
//
// Arguments:
//
//	tuner		- Reference to the JSON status data for the tuner

Tuner^ Tuner::Create(JObject^ tuner)
{
	if(Object::ReferenceEquals(tuner, nullptr)) throw gcnew ArgumentNullException("tuner");
	return gcnew Tuner(tuner);
}

//---------------------------------------------------------------------------
// Tuner::Index::get
//
// Gets the resource index of the tuner instance

int Tuner::Index::get(void)
{
	return m_index;
}

//---------------------------------------------------------------------------
// Tuner::IsActive::get
//
// Gets a flag indicating if the tuner is active or not

bool Tuner::IsActive::get(void)
{
	// For now, assume that the presence of a tuned frequency and/or a target
	// IP address are sufficient to deem the device as "active"
	return ((m_frequency >= 0) || (!String::IsNullOrEmpty(m_targetip)));
}

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)
