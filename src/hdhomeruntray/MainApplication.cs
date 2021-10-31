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
using System.Windows.Forms;

using zuki.hdhomeruntray.discovery;

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
			m_timer = new Timer();
			m_timer.Interval = 30000;       // TODO: Configurable
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
			// TODO: cancel any async operations

			m_timer.Enabled = false;			// Stop the timer
			m_notifyicon.Visible = false;		// Remove the tray icon
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
		}

		// OnNotifyIconOpenPopup
		//
		// Invoked when the hover popup window should be opened
		private void OnNotifyIconOpenPopup(object sender, EventArgs args)
		{
		}

		// OnNotifyIconSelected
		//
		// Invoked when the notify icon has been selected (clicked on)
		private void OnNotifyIconSelected(object sender, EventArgs args)
		{
		}

		// OnTimerTick
		//
		// Invoked when the timer object has come due
		private void OnTimerTick(object sender, EventArgs args)
		{
			// TODO: I want this to not lock up the entire process if it
			// doesn't work quickly; create an async version
			DeviceList devices = DeviceList.Create();

			// Update the tray icon
			UpdateNotifyIcon(devices);
		}

		// UpdateNotifyIcon
		//
		// Updates the state of the notify icon after a discovery
		private void UpdateNotifyIcon(DeviceList devices)
		{
			if(devices == null) throw new ArgumentNullException("devices");

			bool isactive = false;			// Active tuners flag
			bool isrecording = false;       // Active recordings flag

			// Iterate over all the devices to determine what status should be shown
			foreach(Device device in devices)
			{
				if(device is TunerDevice tunerdevice)
				{
					foreach(Tuner tuner in tunerdevice.Tuners)
					{
						if(tuner.IsActive) isactive = true;
					}
				}

				else if(device is StorageDevice storagedevice)
				{
					// TODO: get storage status when it exists
				}
			}

			// Update the tray icon accordingly
			if(isrecording) m_notifyicon.Icon = StatusIcons.Get(StatusIconType.Recording);
			else if(isactive) m_notifyicon.Icon = StatusIcons.Get(StatusIconType.Active);
			else m_notifyicon.Icon = StatusIcons.Get(StatusIconType.Idle);
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private ShellNotifyIcon m_notifyicon;
		private ContextMenuStrip m_contextmenu;
		private Timer m_timer;
	}
}