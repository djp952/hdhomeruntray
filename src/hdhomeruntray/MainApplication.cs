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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Security.Cryptography;
using System.Text;
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

	internal class MainApplication : ApplicationContext
	{
		// Instance Constructor
		//
		public MainApplication()
		{
			// Create a WindowsFormsSynchronizationContext on which event handlers
			// can be invoked without causing weird threading issues
			m_context = new WindowsFormsSynchronizationContext();

			// Create a pseudo GUID to use as the GUID for the ShellNotifyIcon instance, Windows
			// stores this and the path to the application in it's Tray Icon cache and if the application
			// is run from a different path with a different GUID, Shell_NotifyIconW (NIM_ADD) will fail.
			//
			// This method has a drawback in that each time the application is run from a different place,
			// Windows will treat it as a completely separate application and the user will end up with
			// multiple instances in their Taskbar Settings, but it's better than having no icon at all
			//
			byte[] buffer = Encoding.Unicode.GetBytes(Application.ExecutablePath.ToUpper());
			byte[] hash = MD5.Create().ComputeHash(buffer);
			m_guid = new Guid(hash);

			Application.ApplicationExit += new EventHandler(OnApplicationExit);
			InitializeComponent();

			// Upgrade the settings if necessary
			if(Settings.Default.UpgradeRequired)
			{
				Settings.Default.Upgrade();
				Settings.Default.UpgradeRequired = false;
				Settings.Default.Save();
			}

			// Check the actual autostart setting in the registry and change if necessary
			EnabledDisabled autostart = (IsAutoStartEnabled()) ? EnabledDisabled.Enabled : EnabledDisabled.Disabled;
			if(autostart != Settings.Default.AutoStart)
			{
				Settings.Default.AutoStart = autostart;
				Settings.Default.Save();
			}

			// On Windows 10 and above, set up a system theme registry monitor
			if(VersionHelper.IsWindows10OrGreater())
			{
				m_thememonitor = new RegistryKeyValueChangeMonitor(Registry.CurrentUser, @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
				m_thememonitor.ValueChanged += new EventHandler(OnTaskbarThemeChanged);

				try { m_thememonitor.Start(); }
				catch(Exception) { /* DON'T CARE FOR NOW */ }
			}

			// Wire up a handler to watch for property changes
			Settings.Default.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);

			// Wire up a handler for watch for power events
			m_powerchanged = new PowerModeChangedEventHandler(OnPowerModeChanged);
			SystemEvents.PowerModeChanged += m_powerchanged;

			// Create and wire up the device discovery object
			m_devices = new Devices();
			m_devices.DiscoveryCompleted += new DiscoveryCompletedEventHandler(OnDiscoveryCompleted);

			// Show the tray icon after initialization
			m_notifyicon.Visible = true;

			// Wait for the network to become available before executing initial discovery
			m_networktimer.Start();
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

			// Unsubscribe from the power mode event handler
			SystemEvents.PowerModeChanged -= m_powerchanged;

			// Stop and dispose of the theme registry monitor if created
			if(m_thememonitor != null)
			{
				m_thememonitor.Stop();
				m_thememonitor.Dispose();
			}

			m_networktimer.Enabled = false;     // Disable the timer
			m_discoverytimer.Enabled = false;   // Disable the timer
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

		// OnDiscoveryTimerElapsed
		//
		// Invoked when the discovery timer object has come due
		private void OnDiscoveryTimerElapsed(object sender, ElapsedEventArgs args)
		{
			ExecuteDiscovery();
		}

		// OnMenuItemExit
		//
		// Invoked via the "Exit" menu item or the tray icon receives a WM_CLOSE
		private void OnMenuItemExit(object sender, EventArgs args)
		{
			m_context.Post(new SendOrPostCallback((o) =>
			{
				if(m_popupform != null)
				{
					m_popupform.Close();
					m_popupform.Dispose();
					m_popupform = null;
				}

				Application.Exit();

			}), null);
		}

		// OnNetworkTimerElapsed
		//
		// Invoked when the network timer object has come due
		private void OnNetworkTimerElapsed(object sender, ElapsedEventArgs args)
		{
			bool hasnetwork = true;     // Assume the network is available

			// Check for IPv4 network connectivity; in the unlikely event of an
			// exception, assume that the network is up and roll the dice
			try { hasnetwork = Devices.IsIPv4NetworkAvailable(); }
			catch(Exception) { /* DO NOTHING */ }

			// If the network is available switch to device discovery
			if(hasnetwork)
			{
				m_networktimer.Stop();          // Stop this timer
				ExecuteDiscovery();             // Execute discovery
				m_discoverytimer.Start();       // Start periodic discovery
			}
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

		// OnNotifyIconOpenPopup
		//
		// Invoked when the hover popup window should be opened
		private void OnNotifyIconOpenPopup(object sender, EventArgs args)
		{
			m_context.Post(new SendOrPostCallback((o) =>
			{
				// Show the popup window if it's not already shown, and this
				// function hasn't been disabled by the user
				if((Settings.Default.TrayIconHover == EnabledDisabled.Enabled) && (m_popupform == null))
				{
					m_popupform = new PopupForm(m_devicelist);
					m_popupform.DeviceStatusChanged += new DeviceStatusChangedEventHandler(OnPopupDeviceStatusChanged);
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
						m_popupform.Pin(m_notifyicon);
					}
				}

				// Otherwise create a new popup form in a pinned state
				else
				{
					m_popupform = new PopupForm(m_devicelist, true, m_notifyicon);
					m_popupform.DeviceStatusChanged += new DeviceStatusChangedEventHandler(OnPopupDeviceStatusChanged);
					m_popupform.Unpinned += new EventHandler(OnPopupUnpinned);
					m_popupform.ShowFromNotifyIcon(m_notifyicon);
				}

			}), null);
		}

		// OnPopupDeviceStatusChaged
		//
		// Invoked when the overall device status has changed
		private void OnPopupDeviceStatusChanged(object sender, DeviceStatusChangedEventArgs args)
		{
			// The timer granularity of the popup form is much higher than
			// the discovery timer; just go with what it's telling us
			if(args.DeviceStatus != m_status)
			{
				m_status = args.DeviceStatus;
				m_notifyicon.Icon = StatusIcons.Get(m_status);
			}
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

		// OnPowerModeChanged
		//
		// Invoked when the system power mode has changed
		private void OnPowerModeChanged(object sender, PowerModeChangedEventArgs args)
		{
			// Stop timers and close the popup form if it's open on either suspend or resume
			if((args.Mode == PowerModes.Suspend) || (args.Mode == PowerModes.Resume))
			{
				m_discoverytimer.Enabled = false;
				m_networktimer.Enabled = false;

				m_context.Post(new SendOrPostCallback((o) =>
				{
					if(m_popupform != null)
					{
						m_popupform.Close();
						m_popupform.Dispose();
						m_popupform = null;
					}

				}), null);
			}

			// On resume, use the network timer to wait for IPv4 to come back up prior
			// to executing a new discovery operation
			if(args.Mode == PowerModes.Resume) m_networktimer.Start();
		}

		// OnPropertyChanged
		//
		// Invoked when a settings property has been changed
		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			// AutoStart
			//
			if(args.PropertyName == nameof(Settings.Default.AutoStart))
			{
				// Enable or disable the autostart function in the registry
				EnableDisableAutoStart(Settings.Default.AutoStart == EnabledDisabled.Enabled);
			}

			// DiscoveryInterval
			// DiscoveryMethod
			//
			if((args.PropertyName == nameof(Settings.Default.DiscoveryInterval)) ||
				(args.PropertyName == nameof(Settings.Default.DiscoveryMethod)))
			{
				bool wasenabled = m_discoverytimer.Enabled;         // Check if enabled first
				if(wasenabled) m_discoverytimer.Enabled = false;    // Stop the timer

				// Reset the timer interval to the new value and force a new discovery
				m_discoverytimer.Interval = (int)Settings.Default.DiscoveryInterval;
				if(wasenabled) ExecuteDiscovery();

				if(wasenabled) m_discoverytimer.Enabled = true;     // Restart the timer
			}

			// TrayIconHoverDelay
			//
			if(args.PropertyName == nameof(Settings.Default.TrayIconHoverDelay))
			{
				m_notifyicon.HoverInterval = GetHoverInterval(Settings.Default.TrayIconHoverDelay);
			}
		}

		// OnTaskbarThemeChanged
		//
		// Invoked when the system taskbar theme has changed
		private void OnTaskbarThemeChanged(object sender, EventArgs args)
		{
			// Refresh the status icon if would be different than it already is
			Icon statusicon = StatusIcons.Get(m_status);
			if(!m_notifyicon.Icon.Equals(statusicon)) m_notifyicon.Icon = statusicon;
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

		// EnableDisableAutoStart (static)
		//
		// Enables or disables auto-start for the application
		private void EnableDisableAutoStart(bool enable)
		{
			Debug.Assert(m_guid != Guid.Empty);

			// Use hdhomeruntray_TRAYICONGUID as the registry value name
			string appname = "hdhomeruntray_" + m_guid.ToString("N").ToUpper();

			// Open the HKEY_CURRENT_USER "Run" key
			RegistryKey startupkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
			if(startupkey != null)
			{
				if(enable) startupkey.SetValue(appname, Application.ExecutablePath);
				else
				{
					// RegistryKey will throw if the value doesn't exist ...
					try { startupkey.DeleteValue(appname); }
					catch(Exception) { /* DO NOTHING */ }
				}
			}
		}

		// ExecuteDiscovery
		//
		// Executes the asynchronous discovery operation
		private void ExecuteDiscovery()
		{
			m_devices.CancelAsync(this);
			m_devices.DiscoverAsync(Settings.Default.DiscoveryMethod, this);
		}

		// InitializeComponent
		//
		// Initializes all of the Windows Forms components and object
		private void InitializeComponent()
		{
			// Create the Container for the controls
			Container container = new Container();

			ToolStripMenuItem exititem = new ToolStripMenuItem("Exit");
			exititem.Click += new EventHandler(OnMenuItemExit);

			// Create the ContextMenuStrip for the tray icon
			ContextMenuStrip contextmenu = new ContextMenuStrip(container)
			{
				BackColor = SystemColors.ControlLightLight,
				Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
				RenderMode = ToolStripRenderMode.Professional
			};

			// Create the icon for the exit menu item
			Image exiticon = null;

			// Windows 11 - Segoe Fluent Icons
			//
			if(VersionHelper.IsWindows11OrGreater())
				exiticon = MenuImageFromSymbolGlyph(SymbolGlyph.Exit, contextmenu.ForeColor, "Segoe Fluent Icons", 11.25F, FontStyle.Regular);

			// Windows 10 - Segoe MDL2 Assets
			//
			else if(VersionHelper.IsWindows10OrGreater())
				exiticon = MenuImageFromSymbolGlyph(SymbolGlyph.Exit, contextmenu.ForeColor, "Segoe MDL2 Assets", 11.25F, FontStyle.Regular);

			// Windows 7 - Symbols
			else
				exiticon = MenuImageFromSymbolGlyph(SymbolGlyph.Exit, contextmenu.ForeColor, "Symbols", 11.25F, FontStyle.Regular);

			if(exiticon != null) exititem.Image = exiticon;

			// Add the completed menu item to the context menu control
			contextmenu.Items.Add(exititem);

			// Create and initialize the ShellNotifyIcon instance
			m_notifyicon = new ShellNotifyIcon(m_guid, container);
			m_notifyicon.CloseApplicaftion += new EventHandler(OnMenuItemExit);
			m_notifyicon.ClosePopup += new EventHandler(OnNotifyIconClosePopup);
			m_notifyicon.OpenPopup += new EventHandler(OnNotifyIconOpenPopup);
			m_notifyicon.Selected += new EventHandler(OnNotifyIconSelected);
			m_notifyicon.ContextMenuStrip = contextmenu;
			m_notifyicon.Icon = StatusIcons.Get(DeviceStatus.Idle);
			m_notifyicon.HoverInterval = GetHoverInterval(Settings.Default.TrayIconHoverDelay);
			m_notifyicon.ToolTip = "HDHomeRun Status Monitor";

			// Create the network timer object
			m_networktimer = new System.Timers.Timer
			{
				AutoReset = true,
				Interval = 100,
			};
			m_networktimer.Elapsed += new ElapsedEventHandler(OnNetworkTimerElapsed);

			// Create the discovery timer object
			m_discoverytimer = new System.Timers.Timer
			{
				AutoReset = true,
				Interval = (double)Settings.Default.DiscoveryInterval,
			};
			m_discoverytimer.Elapsed += new ElapsedEventHandler(OnDiscoveryTimerElapsed);

			// WINDOWS 11
			//
			if(VersionHelper.IsWindows11OrGreater())
			{
				contextmenu.Font = new Font("Segoe UI Variable Text Semibold", contextmenu.Font.Size, contextmenu.Font.Style);
			}
		}

		// IsAutoStartEnabled (static)
		//
		// Checks if auto-start is currently enabled or disabled
		private bool IsAutoStartEnabled()
		{
			Debug.Assert(m_guid != Guid.Empty);

			// Use hdhomeruntray_TRAYICONGUID as the registry value name
			string appname = "hdhomeruntray_" + m_guid.ToString("N").ToUpper();

			// Open the HKEY_CURRENT_USER "Run" key
			RegistryKey startupkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
			if(startupkey == null) return false;

			// Try to get the value for the constructed application name
			object obj = startupkey.GetValue(appname);
			if(obj == null) return false;

			// If the value returned is a string and matches our executable path; it's set for startup
			return (obj is string @string) && (@string == Application.ExecutablePath);
		}

		// MenuImageFromSymbolGlyph (static)
		//
		// Helper function used to create a menu image from a SymbolGlyph
		private static Image MenuImageFromSymbolGlyph(SymbolGlyph glyph, Color forecolor,
			string familyname, float emsize, FontStyle style)
		{
			string text = new string((char)glyph, 1);   // Convert to string
			Bitmap image = null;                        // Image to return

			using(Font font = new Font(familyname, emsize, style))
			{
				// Measure the size required to draw the glyph character
				Size textsize = TextRenderer.MeasureText(text, font);
				Size size = textsize;

				// Square off the size
				if(size.Width > size.Height) size.Height = size.Width;
				if(size.Height > size.Width) size.Width = size.Height;

				// Determine the position within the bitmap to draw
				int left = (size.Width - textsize.Width) / 2;
				int top = (size.Height - textsize.Height) / 2;

				// Create a 32bpp transparent bitmap and render the text
				image = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
				using(Graphics drawing = Graphics.FromImage(image))
				{
					drawing.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
					TextRenderer.DrawText(drawing, text, font, new Point(left, top), forecolor);
				}
			}

			return image;
		}

		// UpdateNotifyIcon
		//
		// Updates the state of the notify icon after a discovery
		private void UpdateNotifyIcon(DeviceList devices)
		{
			if(devices == null) throw new ArgumentNullException(nameof(devices));

			DeviceStatus overallstatus = DeviceStatus.Idle;

			// Iterate over all the devices to determine what status should be shown
			foreach(Device device in devices)
			{
				// For tuners, we can stop looking after the first one found to be active
				if(device is TunerDevice tunerdevice)
				{
					foreach(Tuner tuner in tunerdevice.Tuners)
					{
						DeviceStatus status = tunerdevice.GetTunerStatus(tuner).DeviceStatus;
						if(status > overallstatus)
						{
							overallstatus = status;
							break;
						}
					}
				}

				// For storage, we can stop looking after the first one found to be recording
				else if(device is StorageDevice storagedevice)
				{
					DeviceStatus status = storagedevice.GetStorageStatus().DeviceStatus;
					if(status > overallstatus) overallstatus = status;
					if(overallstatus >= DeviceStatus.ActiveAndRecording) break;
				}
			}

			// If the overall status has changed, update the tray icon accordingly
			if(overallstatus != m_status)
			{
				m_status = overallstatus;
				m_notifyicon.Icon = StatusIcons.Get(m_status);
			}
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private readonly WindowsFormsSynchronizationContext m_context;
		private ShellNotifyIcon m_notifyicon;
		private System.Timers.Timer m_networktimer;
		private System.Timers.Timer m_discoverytimer;
		private PopupForm m_popupform = null;
		private readonly Devices m_devices;
		private DeviceList m_devicelist = DeviceList.Empty;
		private DeviceStatus m_status = DeviceStatus.Idle;
		private readonly RegistryKeyValueChangeMonitor m_thememonitor;
		private readonly PowerModeChangedEventHandler m_powerchanged;
		private readonly Guid m_guid = Guid.Empty;
	}
}