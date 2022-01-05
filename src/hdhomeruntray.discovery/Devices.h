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

#ifndef __DEVICES_H_
#define __DEVICES_H_
#pragma once

#include "DeviceList.h"
#include "DiscoveryCompletedEventArgs.h"
#include "DiscoveryMethod.h"

#pragma warning(push, 4)

using namespace System;
using namespace System::Collections::Specialized;
using namespace System::Runtime::InteropServices;
using namespace System::Threading;

namespace zuki::hdhomeruntray::discovery {

//-----------------------------------------------------------------------
// Delegate Types
//---------------------------------------------------------------------------

// DiscoveryCompletedEventHandler
//
// Delegate signature for the Devices::DiscoveryCompleted event
public delegate void DiscoveryCompletedEventHandler(Object^ sender, DiscoveryCompletedEventArgs^ args);

//---------------------------------------------------------------------------
// Class Devices
//
// Implements the top-level HDHomeRun device discovery class

public ref class Devices
{
public:

	// Instance Constructor
	//
	Devices();

	//-----------------------------------------------------------------------
	// Events

	// DiscoveryCompleted
	//
	// Invoked upon completion of a successful device discovery
	event DiscoveryCompletedEventHandler^ DiscoveryCompleted
	{
		public:		void add(DiscoveryCompletedEventHandler^ handler);
		internal:	void raise(Object^ sender, DiscoveryCompletedEventArgs^ args);
		public:		void remove(DiscoveryCompletedEventHandler^ handler);
	}

	//-----------------------------------------------------------------------
	// Member Functions

	// CancelAsync
	//
	// Cancels an asynchronous operation
	void CancelAsync(Object^ taskid);

	// Discover
	//
	// Executes a synchronous device discovery operation
	DeviceList^ Discover(DiscoveryMethod method);
	
	// DiscoverAsync
	//
	// Executes an asynchronous device discovery operation
	void DiscoverAsync(DiscoveryMethod method, Object^ taskid);

	// IsIPv4NetworkAvailable (static)
	//
	// Helper function used to determine if the IPv4 network is available
	static bool IsIPv4NetworkAvailable(void);

	// TryDiscover
	//
	// Executes a synchronous device discovery operation
	bool TryDiscover(DiscoveryMethod method, [OutAttribute] DeviceList^% devices);

private:

	//-----------------------------------------------------------------------
	// Private Type Declarations

	// DiscoverAsyncWorkerWorkerEventHandler
	//
	// Delegate signature for the DiscoveryAsyncWorker method
	delegate void DiscoverAsyncWorkerWorkerEventHandler(DiscoveryMethod method, AsyncOperation^ async);

	//-----------------------------------------------------------------------
	// Private Member Functions

	// DiscoverAsyncCompletionMethod
	//
	// DiscoverAsync completion method
	void DiscoverAsyncCompletionMethod(DiscoveryMethod method, DeviceList^ devices, Exception^ exception, bool cancelled, AsyncOperation^ async);

	// DiscoverAsyncWorker
	//
	// Internal implmentation of DiscoverAsync
	void DiscoverAsyncWorker(DiscoveryMethod method, AsyncOperation^ async);

	// DiscoveryCompletedAsync
	//
	// Callback invoked when the asynchronous operation completed
	void DiscoveryCompletedAsync(Object^ state);

	// TaskCancelled
	//
	// Helper function used to determine if a task was cancelled
	bool TaskCancelled(Object^ taskid);

	//-----------------------------------------------------------------------
	// Member Variables

	SendOrPostCallback^				m_asynccompleted;	// DiscoveryCompletedAsync delegate
	DiscoveryCompletedEventHandler^ m_completed;		// DiscoveryCompleted delegate
	HybridDictionary^				m_statetolifetime;	// User state to lifetime tracking
};

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)

#endif	// __DEVICES_H_
