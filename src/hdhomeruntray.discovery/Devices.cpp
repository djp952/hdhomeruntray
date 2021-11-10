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

#include "Devices.h"

#pragma warning(push, 4)

namespace zuki::hdhomeruntray::discovery {

//---------------------------------------------------------------------------
// Devices Constructor
//
// Arguments:
//
//	NONE

Devices::Devices(void)
{
	m_statetolifetime = gcnew HybridDictionary();
	m_asynccompleted = gcnew SendOrPostCallback(this, &Devices::DiscoveryCompletedAsync);
}

//---------------------------------------------------------------------------
// Devices::CancelAsync
//
// Cancels an asynchronous operation
//
// Arguments:
//
//  taskid      - taskid associated with the asynchrnous operation

void Devices::CancelAsync(Object^ taskid)
{
    msclr::lock lock(m_statetolifetime->SyncRoot);

    if(!m_statetolifetime->Contains(taskid)) return;
    AsyncOperation^ async = dynamic_cast<AsyncOperation^>(m_statetolifetime[taskid]);
    if(CLRISNOTNULL(async)) m_statetolifetime->Remove(taskid);
}

//---------------------------------------------------------------------------
// Devices::Discover
//
// Executes a synchronous device discovery operation
//
// Arguments:
//
//	method		- Discovery method to use to find the devices

DeviceList^ Devices::Discover(DiscoveryMethod method)
{
    return DeviceList::Create(method);
}

//---------------------------------------------------------------------------
// Devices::DiscoverAsync
//
// Executes an asynchronous device discovery operation
//
// Arguments:
//
//	method		- Discovery method to use to find the devices

void Devices::DiscoverAsync(DiscoveryMethod method, Object^ taskid)
{
    if(CLRISNULL(taskid)) throw gcnew ArgumentNullException("taskid");

    // Create an AsyncOperation for the taskid
    AsyncOperation^ async = AsyncOperationManager::CreateOperation(taskid);

    // Multiple threads will access the task dictionary, so it must be locked
    {
        msclr::lock lock(m_statetolifetime->SyncRoot);
        if(m_statetolifetime->Contains(taskid)) throw gcnew ArgumentException("taskid must be unique");
        m_statetolifetime[taskid] = async;
    }

    // Start the asynchronous operation
    DiscoverAsyncWorkerWorkerEventHandler^ worker = gcnew DiscoverAsyncWorkerWorkerEventHandler(this, &Devices::DiscoverAsyncWorker);
    worker->BeginInvoke(method, async, nullptr, nullptr);
}

//---------------------------------------------------------------------------
// Devices::DiscoverAsyncCompletionMethod
//
// DiscoverAsync completion method
//
// Arguments:
//
//	method		- Discovery method to use to find the devices
//	devices		- The list of discovered devices
//	exception	- Exception instance, if any
//	cancelled	- Flag indicating the operation was cancelled
//	async		- Tracks the lifetime of the asynchronous operation

void Devices::DiscoverAsyncCompletionMethod(DiscoveryMethod method, DeviceList^ devices, Exception^ exception, bool cancelled, AsyncOperation^ async)
{
    // If the task was not previously cancelled, remove it from the lifetime collection
    if(!cancelled) {

        msclr::lock lock(m_statetolifetime->SyncRoot);
        m_statetolifetime->Remove(async->UserSuppliedState);
    }

    // Package the results of the operation in a DiscoveryCompletedEventArgs instance
    DiscoveryCompletedEventArgs^ args = gcnew DiscoveryCompletedEventArgs(method, devices, exception, cancelled, async->UserSuppliedState);

	// End the task; the async object is responsible for marshalling the call
	async->PostOperationCompleted(m_asynccompleted, args);
}

//---------------------------------------------------------------------------
// Devices::DiscoverAsyncWorker (private)
//
// Internal implmentation of DiscoverAsync
//
// Arguments:
//
//	method		- Discovery method to use to find the devices
//	async		- Tracks the lifetime of the asynchronous operation

void Devices::DiscoverAsyncWorker(DiscoveryMethod method, AsyncOperation^ async)
{
    DeviceList^ devices = DeviceList::Empty;
    Exception^ exception = nullptr;

    // Check that the task is still active; the operation may have been cancelled
    // before the worker thread was scheduled
    if(!TaskCancelled(async->UserSuppliedState))
    {
        // Synchronously execute a discovery operation and store any thrown exceptions
        try { devices = Discover(method); }
        catch(Exception^ ex) { exception = ex; }
    }

    // Invoke the asynchronous completion method when the operation is complete
    DiscoverAsyncCompletionMethod(method, devices, exception, TaskCancelled(async->UserSuppliedState), async);
}

//---------------------------------------------------------------------------
// Devices::DiscoveryCompleted::add
//
// Adds a handler to the DiscoveryCompleted event

void Devices::DiscoveryCompleted::add(DiscoveryCompletedEventHandler^ handler)
{
	m_completed = safe_cast<DiscoveryCompletedEventHandler^>(Delegate::Combine(m_completed, handler));
}

//---------------------------------------------------------------------------
// Devices::DiscoveryCompleted::raise
//
// Raises the DiscoveryCompleted event
//
// Arguments:
//
//	sender		- Object raising the event
//	args		- Event arguments instance

void Devices::DiscoveryCompleted::raise(Object^ sender, DiscoveryCompletedEventArgs^ args)
{
	if(CLRISNOTNULL(m_completed)) m_completed(sender, args);
}

//---------------------------------------------------------------------------
// Devices::DiscoveryCompleted::remove
//
// Removes a handler from the DiscoveryCompleted event

void Devices::DiscoveryCompleted::remove(DiscoveryCompletedEventHandler^ handler)
{
	m_completed = safe_cast<DiscoveryCompletedEventHandler^>(Delegate::Remove(m_completed, handler));
}

//---------------------------------------------------------------------------
// Devices::DiscoveryCompletedAsync (private)
//
// Callback invoked when the asynchronous operation completed
//
// Arguments:
//
//	state		- SendOrPostCallback state object

void Devices::DiscoveryCompletedAsync(Object^ state)
{
	if(CLRISNULL(state)) throw gcnew ArgumentNullException("state");

	// The state object should be a DiscoveryCompletedEventArgs instance
	DiscoveryCompletedEventArgs^ args = dynamic_cast<DiscoveryCompletedEventArgs^>(state);
	if(CLRISNULL(args)) throw gcnew ArgumentNullException("args");

	DiscoveryCompleted(this, args);			// Raise the event
}

//---------------------------------------------------------------------------
// Devices::TaskCancelled (private)
//
// Helper function used to determine if a task was cancelled
//
// Arguments:
//
//	taskid		- Task identification object

bool Devices::TaskCancelled(Object^ taskid)
{
    msclr::lock lock(m_statetolifetime->SyncRoot);
	return m_statetolifetime[taskid] == nullptr;
}

//---------------------------------------------------------------------------
// Devices::TryDiscover
//
// Executes a synchronous device discovery operation
//
// Arguments:
//
//	method		- Discovery method to use to find the devices
//	devices		- Receives the generated DeviceList^ on success

bool Devices::TryDiscover(DiscoveryMethod method, [OutAttribute] DeviceList^% devices)
{
    try { devices = Discover(method); }
    catch(Exception^) { return false; }

    return true;
}

//---------------------------------------------------------------------------

} // zuki::hdhomeruntray::discovery

#pragma warning(pop)
