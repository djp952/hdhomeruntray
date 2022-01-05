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

using System;
using System.Runtime.InteropServices;
using System.Threading;

using Microsoft.Win32;

namespace zuki.hdhomeruntray
{
	//-----------------------------------------------------------------------
	// Class RegistryChangeMonitor (internal)
	//
	// Simple registry key value change monitor, will raise an event if
	// a value within the specified key has changed
	//
	// Loosely based on:
	// http://www.pinvoke.net/default.aspx/advapi32.regnotifychangekeyvalue

	internal class RegistryKeyValueChangeMonitor : IDisposable
	{
		#region Win32 API Declarations
		private static class NativeMethods
		{
			public const uint REG_NOTIFY_CHANGE_NAME = 0x00000001;
			public const uint REG_NOTIFY_CHANGE_ATTRIBUTES = 0x00000002;
			public const uint LAST_SET = 0x000000004;
			public const uint SECURITY = 0x00000008;
			public const uint REG_NOTIFY_THREAD_AGNOSTIC = 0x10000000;

			[DllImport("advapi32.dll")]
			public static extern int RegNotifyChangeKeyValue(IntPtr hKey, [MarshalAs(UnmanagedType.Bool)] bool watchSubtree,
				uint notifyFilter, IntPtr hEvent, [MarshalAs(UnmanagedType.Bool)] bool asynchronous);
		}
		#endregion

		// Instance Constructor
		//
		public RegistryKeyValueChangeMonitor(RegistryKey hive, string subkey)
		{
			m_hive = hive ?? throw new ArgumentNullException(nameof(hive));
			m_subkey = subkey ?? throw new ArgumentNullException(nameof(subkey));
		}

		#region IDisposable Implementation
		~RegistryKeyValueChangeMonitor()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if(m_disposed) return;

			Stop();
			m_disposed = true;
		}
		#endregion

		//-------------------------------------------------------------------
		// Events
		//-------------------------------------------------------------------

		// ValueChanged
		//
		// Invoked when a value in the specified registry key has changed
		public event EventHandler ValueChanged;

		//-------------------------------------------------------------------------
		// Properties
		//-------------------------------------------------------------------------

		// Monitoring
		//
		// Determines if the registry key is being monitored or not
		public bool Monitoring
		{
			get
			{
				lock(this)
				{
					return (m_monitoredkey != null);
				}
			}
		}

		//-------------------------------------------------------------------
		// Member Functions
		//-------------------------------------------------------------------

		// Start
		//
		// Begins monitoring the specified registry key for value changes
		public void Start()
		{
			lock(this)
			{
				// Already monitoring
				if(m_monitoredkey != null) return;

				// Create a new RegistryKey instance for the worker thread to monitor
				m_monitoredkey = m_hive.OpenSubKey(m_subkey);
				if(m_monitoredkey == null) throw new Exception("Unable to open the specified registry key for monitoring");

				// Create the background worker thread
				Thread thread = new Thread(new ThreadStart(ThreadProc))
				{
					IsBackground = true
				};

				// Start the background worker thread
				thread.Start();
			}
		}

		// Stop
		//
		// Stops monitoring the specified registry key for value changes
		public void Stop()
		{
			lock(this)
			{
				// Not monitoring
				if(m_monitoredkey == null) return;

				// Closing the registry key will trigger RegNotifyChangeKeyValue, setting the value
				// to null will let the thread know it's time to go away (don't dispose it)
				m_monitoredkey.Close();
				m_monitoredkey = null;
			}
		}

		//-------------------------------------------------------------------------
		// Private Member Functions
		//-------------------------------------------------------------------------

		// ThreadProc
		//
		// Implements the worker thread procedure
		private void ThreadProc()
		{
			try
			{
				// Grab a reference to the object since the member variable will go null on close
				RegistryKey key = m_monitoredkey;

				while(true)
				{
					// RegNotifyChangeKeyValue will block until a change is registered or the key is closed
					int result = NativeMethods.RegNotifyChangeKeyValue(key.Handle.DangerousGetHandle(), true, NativeMethods.LAST_SET, IntPtr.Zero, false);

					lock(this)
					{
						// A zero result and a valid reference in m_monitoredkey means to fire the event, otherwise die
						if((result == 0) && (m_monitoredkey != null)) ValueChanged?.Invoke(this, EventArgs.Empty);
						else return;
					}
				}
			}

			catch(Exception) { /* DO NOTHING */ }
		}

		//-------------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------------

		private bool m_disposed = false;
		private readonly RegistryKey m_hive;
		private readonly string m_subkey;
		private RegistryKey m_monitoredkey;
	}
}