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

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using Microsoft.Win32;

using zuki.hdhomeruntray.discovery;
using zuki.hdhomeruntray.Properties;

namespace zuki.hdhomeruntray
{
	//-----------------------------------------------------------------------
	// Class MainApplication (internal)
	//
	// Provides the main application context object, which is used as the 
	// parameter to Application.Run() instead of providing a main form object
	
	class MainApplication : ApplicationContext
	{
		// Instance Constructor
		//
		public MainApplication()
		{
			// Create a WindowsFormsSynchronizationContext on which event handlers
			// can be invoked without causing weird threading issues
			m_context = new WindowsFormsSynchronizationContext();

			Application.ApplicationExit += new EventHandler(OnApplicationExit);
			InitializeComponent();

			// Wire up a handler to watch for property changes
			Settings.Default.PropertyChanged += OnPropertyChanged;

			// Create and wire up the device discovery object
			m_devices = new Devices();
			m_devices.DiscoveryCompleted += new DiscoveryCompletedEventHandler(OnDiscoveryCompleted);

			// Show the tray icon after initialization
			m_notifyicon.Visible = true;

			// Invoke an initial refresh of the device discovery data
			ExecuteDiscovery();

			// Start the periodic timer
			m_timer.Start();
		}

		//-------------------------------------------------------------------
		// Private Member Functions
		//-------------------------------------------------------------------

		// InitializeComponent
		//
		// Initializes all of the Windows Forms components and object
		private void InitializeComponent()
		{
			Guid iconguid = s_guid;

			// Change the GUID from the default to the replacement if necessary
			if(!Guid.Empty.Equals(Settings.Default.TrayIconGuid))
			{
				iconguid = Settings.Default.TrayIconGuid;
			}

			// Create and initialize the ShellNotifyIcon instance
			m_notifyicon = new ShellNotifyIcon(iconguid);
			m_notifyicon.ClosePopup += new EventHandler(OnNotifyIconClosePopup);
			m_notifyicon.OpenPopup += new EventHandler(OnNotifyIconOpenPopup);
			m_notifyicon.Selected += new EventHandler(OnNotifyIconSelected);
			m_notifyicon.GuidChanged += new ShellNotifyIconGuidChangedEventHandler(OnNotifyIconGuidChanged);
			m_notifyicon.Icon = StatusIcons.Get(StatusIconType.Idle);
			m_notifyicon.HoverInterval = GetHoverInterval(Settings.Default.TrayIconHoverDelay);
			m_notifyicon.ToolTip = "HDHomeRun System Tray";

			// Create the periodic timer object
			m_timer = new System.Timers.Timer
			{
				AutoReset = true,
				Interval = (double)Settings.Default.DiscoveryInterval,
			};
			m_timer.Elapsed += new ElapsedEventHandler(OnTimerElapsed);
		}

		//-------------------------------------------------------------------
		// Event Handlers
		//-------------------------------------------------------------------

		// OnApplicationExit
		//
		// Invoked when the application is exiting
		private void OnApplicationExit(object sender, EventArgs args)
		{
			m_context.Post(new SendOrPostCallback((o) =>
			{
				// Ensure all windows are closed
				if(m_popupform != null)
				{
					m_popupform.Close();
					m_popupform.Dispose();
					m_popupform = null;
				}

			}), null);

			m_timer.Enabled = false;            // Stop the timer
			m_devices.CancelAsync(this);        // Cancel any operations
			m_notifyicon.Visible = false;       // Remove the tray icon
		}

		// OnDiscoveryCompleted
		//
		// Invoked when a discovery operation has completed
		private void OnDiscoveryCompleted(object sender, DiscoveryCompletedEventArgs args)
		{
			// If the operation was cancelled, don't do anything
			if(args.Cancelled) return;

			// If there was an exception during discovery, handle it
			// TODO

			// Swap the current device list with the updated one and refresh the icon
			m_devicelist = args.Devices;
			UpdateNotifyIcon(m_devicelist);
		}

		// OnNotifyIconClosePopup
		//
		// Invoked when the hover popup window should be closed
		private void OnNotifyIconClosePopup(object sender, EventArgs args)
		{
			m_context.Post(new SendOrPostCallback((o) =>
			{
				if(m_popupform == null) return;

				// Only close the popup window if it did not become pinned
				if(!m_popupform.Pinned)
				{
					m_popupform.Close();
					m_popupform.Dispose();
					m_popupform = null;
				}

			}), null);
		}

		// OnNotifyIconGuidChanged
		//
		// Invoked when the Guid of the icon needed to be changed
		private void OnNotifyIconGuidChanged(object sender, ShellNotifyIconGuidChangedEventArgs args)
		{
			Settings.Default.TrayIconGuid = args.NewGuid;
			Settings.Default.Save();
		}

		// OnNotifyIconOpenPopup
		//
		// Invoked when the hover popup window should be opened
		private void OnNotifyIconOpenPopup(object sender, EventArgs args)
		{
			m_context.Post(new SendOrPostCallback((o) =>
			{
				// Show the popup window if it's not already shown
				if(m_popupform == null)
				{
					m_popupform = new PopupForm(m_devicelist);
					m_popupform.ShowFromNotifyIcon(m_notifyicon);
				}

			}), null);
		}

		// OnNotifyIconSelected
		//
		// Invoked when the notify icon has been selected (clicked on)
		private void OnNotifyIconSelected(object sender, EventArgs args)
		{
			m_context.Post(new SendOrPostCallback((o) =>
			{
				// If the popup form is already open, pin or close it
				if(m_popupform != null)
				{
					if(m_popupform.Pinned)
					{
						m_popupform.Close();
						m_popupform.Dispose();
						m_popupform = null;
					}
					else
					{
						m_popupform.Unpinned += new EventHandler(OnPopupUnpinned);
						m_popupform.Pin();
					}
				}

				// Otherwise create a new popup form in a pinned state
				else
				{
					m_popupform = new PopupForm(m_devicelist, true);
					m_popupform.Unpinned += new EventHandler(OnPopupUnpinned);
					m_popupform.ShowFromNotifyIcon(m_notifyicon);
				}

			}), null);
		}

		// OnPopupUnpinned
		//
		// Invoked when the popup form has been unpinned
		private void OnPopupUnpinned(object sender, EventArgs args)
		{
			m_context.Post(new SendOrPostCallback((o) =>
			{
				Debug.Assert(m_popupform != null);      // Should be impossible

				m_popupform.Close();
				m_popupform.Dispose();
				m_popupform = null;
			
			}), null);
		}

		// OnPropertyChanged
		//
		// Invoked when a settings property has been changed
		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			// DiscoveryInterval
			// DiscoveryMethod
			if((args.PropertyName == nameof(Settings.Default.DiscoveryInterval)) ||
				(args.PropertyName == nameof(Settings.Default.DiscoveryMethod)))
			{
				m_timer.Enabled = false;            // Stop the timer

				// Reset the timer interval to the new value and force a new discovery
				m_timer.Interval = (int)Settings.Default.DiscoveryInterval;
				ExecuteDiscovery();

				m_timer.Enabled = true;             // Restart the timer
			}

			// TrayIconHoverDelay
			if(args.PropertyName == nameof(Settings.Default.TrayIconHoverDelay))
			{
				m_notifyicon.HoverInterval = GetHoverInterval(Settings.Default.TrayIconHoverDelay);
			}
		}

		// OnTimerElapsed
		//
		// Invoked when the timer object has come due
		private void OnTimerElapsed(object sender, ElapsedEventArgs args)
		{
			ExecuteDiscovery();
		}

		//-------------------------------------------------------------------
		// Private Member Functions
		//-------------------------------------------------------------------

		// GetHoverInterval (static)
		//
		// Converts a TrayIconHoverDelay into milliseconds taking into consideration
		// the running operation system limitations
		private static int GetHoverInterval(TrayIconHoverDelay delay)
		{
			// No coersion is necessary for a non-default value or a default one outside of Windows 11
			if((delay != TrayIconHoverDelay.SystemDefault) || (!VersionHelper.IsWindows11OrGreater())) return (int)delay;

			int mousehovertimeout = 400;            // Default value to use on Windows 11 (ms)

			// Use the default hover interval specified in HKEY_CURRENT_USER
			object value = Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseHoverTime", null);
			if((value != null) && (value is string @string)) _ = int.TryParse(@string, out mousehovertimeout);

			return mousehovertimeout;
		}

		// ExecuteDiscovery
		//
		// Executes the asynchronous discovery operation
		private void ExecuteDiscovery()
		{
			m_devices.CancelAsync(this);
			m_devices.DiscoverAsync(Settings.Default.DiscoveryMethod, this);
		}

		// UpdateNotifyIcon
		//
		// Updates the state of the notify icon after a discovery
		private void UpdateNotifyIcon(DeviceList devices)
		{
			if(devices == null) throw new ArgumentNullException(nameof(devices));

			int numactive = 0;              // Count of active tuners
			int numrecording = 0;			// Count of active recordings

			// Iterate over all the devices to determine what status should be shown
			foreach(Device device in devices)
			{
				if(device is TunerDevice tunerdevice)
				{
					foreach(Tuner tuner in tunerdevice.Tuners)
					{
						TunerStatus status = tunerdevice.GetTunerStatus(tuner);
						if(status.IsActive) numactive++;
					}
				}

				else if(device is StorageDevice storagedevice)
				{
					StorageStatus status = storagedevice.GetStorageStatus();
					numrecording += status.Recordings.Count;
				}
			}

			// Update the icon image based on the overall status
			if(numrecording > 0) m_notifyicon.Icon = StatusIcons.Get(StatusIconType.Recording);
			else if(numactive > 0) m_notifyicon.Icon = StatusIcons.Get(StatusIconType.Active);
			else m_notifyicon.Icon = StatusIcons.Get(StatusIconType.Idle);
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private readonly WindowsFormsSynchronizationContext m_context;
		private ShellNotifyIcon m_notifyicon;
		private System.Timers.Timer	m_timer;
		private PopupForm m_popupform = null;
		private readonly Devices m_devices;
		private DeviceList m_devicelist = DeviceList.Empty;
		
		// Do not change this GUID; it has to remain the same to prevent Windows from creating
		// custom tray icon settings for each GUID that it sees
		private static readonly Guid s_guid = Guid.Parse("{E9DD6790-E032-4CAE-9140-CC0FB55FC210}");
	}
}