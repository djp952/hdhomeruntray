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
			Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
			InitializeComponent();

			// Wire up a handler to watch for property changes
			Settings.Default.PropertyChanged += OnPropertyChanged;

			// Create the wire up the device discovery object
			m_devices = new Devices();
			m_devices.DiscoveryCompleted += new DiscoveryCompletedEventHandler(this.OnDiscoveryCompleted);

			// Show the tray icon after initialization
			m_notifyicon.Visible = true;

			// Invoke an initial refresh of the device discovery data
			OnTimerTick(this, EventArgs.Empty);

			// Start the periodic timer
			m_timer.Enabled = true;
		}

		//-------------------------------------------------------------------
		// Private Member Functions
		//-------------------------------------------------------------------

		// InitializeComponent
		//
		// Initializes all of the Windows Forms components and object
		private void InitializeComponent()
		{
			// Create and initialize the ShellNotifyIcon instance
			m_notifyicon = new ShellNotifyIcon();
			m_notifyicon.ClosePopup += new EventHandler(this.OnNotifyIconClosePopup);
			m_notifyicon.OpenPopup += new EventHandler(this.OnNotifyIconOpenPopup);
			m_notifyicon.Selected += new EventHandler(this.OnNotifyIconSelected);
			m_notifyicon.Icon = StatusIcons.Get(StatusIconType.Idle);

			// Create the context menu
			m_contextmenu = new ContextMenuStrip
			{
				Name = "m_contextmenu"
			};
			m_contextmenu.SuspendLayout();

			// Exit
			var menuitem_exit = new ToolStripMenuItem
			{
				Name = "menuitem_exit",
				Text = "Exit"
			};
			menuitem_exit.Click += new EventHandler(this.OnMenuItemExit);

			// Complete the context menu and associate it with the ShellNotifyIcon
			m_contextmenu.Items.AddRange(new ToolStripItem[] { menuitem_exit });
			m_contextmenu.ResumeLayout(false);
			m_notifyicon.ContextMenuStrip = m_contextmenu;

			// Create the periodic timer object
			m_timer = new Timer
			{
				Interval = Settings.Default.DiscoveryInterval,
			};
			m_timer.Tick += new EventHandler(this.OnTimerTick);
		}

		//-------------------------------------------------------------------
		// Event Handlers
		//-------------------------------------------------------------------

		// OnApplicationExit
		//
		// Invoked when the application is exiting
		private void OnApplicationExit(object sender, EventArgs args)
		{
			// Ensure all windows are closed
			if(m_popupform != null) m_popupform.Close();
			if(m_mainform != null) m_mainform.Close();

			m_timer.Enabled = false;            // Stop the timer
			m_devices.CancelAsync(this);		// Cancel any operations
			m_notifyicon.Visible = false;		// Remove the tray icon
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

		// OnMainFormClosed
		//
		// Invoked when the main form has been closed
		private new void OnMainFormClosed(object sender, EventArgs args)
		{
			m_mainform.Dispose();
			m_mainform = null;
		}

		// OnMenuItemExit
		//
		// Invoked via the "Exit" menu item
		private void OnMenuItemExit(object sender, EventArgs args)
		{
			Application.Exit();
		}

		// OnNotifyIconClosePopup
		//
		// Invoked when the hover popup window should be closed
		private void OnNotifyIconClosePopup(object sender, EventArgs args)
		{
			if(m_popupform != null)
			{
				m_popupform.Close();
				m_popupform.Dispose();
				m_popupform = null;
			}
		}

		// OnNotifyIconOpenPopup
		//
		// Invoked when the hover popup window should be opened
		private void OnNotifyIconOpenPopup(object sender, EventArgs args)
		{
			// Do not show the popup window if the main window is open
			if((m_mainform == null) && (m_popupform == null))
			{
				m_popupform = new PopupForm(m_devicelist);
				m_popupform.ShowFromNotifyIcon(m_notifyicon);
			}
		}

		// OnNotifyIconSelected
		//
		// Invoked when the notify icon has been selected (clicked on)
		private void OnNotifyIconSelected(object sender, EventArgs args)
		{
			// Close the popup window if it's open
			OnNotifyIconClosePopup(this, EventArgs.Empty);

			// This action can be used to toggle the state of the main form
			if(m_mainform == null)
			{
				m_mainform = new MainForm();
				m_mainform.FormClosed += new FormClosedEventHandler(this.OnMainFormClosed);
				m_mainform.ShowFromNotifyIcon(m_notifyicon);
			}

			else m_mainform.Close();
		}

		// OnPropertyChanged
		//
		// Invoked when a settings property has been changed
		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			// DiscoveryInterval / DiscoveryMethod
			if((args.PropertyName == nameof(Settings.Default.DiscoveryInterval)) ||
				(args.PropertyName == nameof(Settings.Default.DiscoveryMethod)))
			{
				m_timer.Enabled = false;			// Stop the timer

				// Reset the timer interval to the new value and force a new discovery
				m_timer.Interval = Settings.Default.DiscoveryInterval;
				OnTimerTick(this, EventArgs.Empty);

				m_timer.Enabled = true;				// Restart the timer
			}
		}

		// OnTimerTick
		//
		// Invoked when the timer object has come due
		private void OnTimerTick(object sender, EventArgs args)
		{
			DiscoveryMethod discoverymethod = DiscoveryMethod.Broadcast;

			// The setting for discovery method is stored as an integer; make sure it's valid before using it
			int method = Settings.Default.DiscoveryMethod;
			if(Enum.IsDefined(typeof(DiscoveryMethod), method)) discoverymethod = (DiscoveryMethod)method;

			m_devices.CancelAsync(this);
			m_devices.DiscoverAsync(discoverymethod, this);
		}

		//-------------------------------------------------------------------
		// Private Member Functions
		//-------------------------------------------------------------------

		// UpdateNotifyIcon
		//
		// Updates the state of the notify icon after a discovery
		private void UpdateNotifyIcon(DeviceList devices)
		{
			if(devices == null) throw new ArgumentNullException("devices");

			int numactive = 0;              // Count of active tuners
			int numrecording = 0;			// Count of active recordings

			// Iterate over all the devices to determine what status should be shown
			foreach(Device device in devices)
			{
				if(device is TunerDevice tunerdevice)
				{
					foreach(Tuner tuner in tunerdevice.Tuners)
					{
						if(tuner.IsActive) numactive++;
					}
				}

				else if(device is StorageDevice storagedevice)
				{
					numrecording += storagedevice.Recordings.Count;
				}
			}

			// Windows 11 doesn't support NIN_POPUPOPEN/NIN_POPUPCLOSE, at least not yet, so the custom
			// hover implementation must always be used on that platform
			if(VersionHelper.IsWindows11OrGreater())
			{
				int mousehovertimeout = 400;			// Default value to use on Windows 11 (ms)

				// Use the default hover interval specified in HKEY_CURRENT_USER
				object value = Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseHoverTime", null);
				if((value != null) && (value is string @string)) int.TryParse(@string, out mousehovertimeout);

				// TODO: use the configured custom timeout if not set to default (0)

				m_notifyicon.ToolTip = String.Empty;
				m_notifyicon.HoverInterval = mousehovertimeout;
			}

			// For NIN_POPUPOPEN/NIN_POPUPCLOSE to be fired the tool tip text must be set to something
			else m_notifyicon.ToolTip = "HDHomeRun System Tray";

			// Update the icon image based on the overall status
			if(numrecording > 0) m_notifyicon.Icon = StatusIcons.Get(StatusIconType.Recording);
			else if(numactive > 0) m_notifyicon.Icon = StatusIcons.Get(StatusIconType.Active);
			else m_notifyicon.Icon = StatusIcons.Get(StatusIconType.Idle);
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private ShellNotifyIcon	m_notifyicon;
		private ContextMenuStrip m_contextmenu;
		private Timer m_timer;
		private PopupForm m_popupform;
		private MainForm m_mainform;
		private Devices m_devices;
		private DeviceList m_devicelist = DeviceList.Empty;
	}
}