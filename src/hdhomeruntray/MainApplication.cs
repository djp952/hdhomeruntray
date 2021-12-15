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
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
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

		// OnMenuItemExit
		//
		// Invoked via the "Exit" menu item
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
				m_timer.Enabled = false;            // Stop the timer

				// Reset the timer interval to the new value and force a new discovery
				m_timer.Interval = (int)Settings.Default.DiscoveryInterval;
				ExecuteDiscovery();

				m_timer.Enabled = true;             // Restart the timer
			}

			// TrayIconHoverDelay
			//
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

		// EnableDisableAutoStart (static)
		//
		// Enables or disables auto-start for the application
		private static void EnableDisableAutoStart(bool enable)
		{
			// Use hdhomeruntray_TRAYICONGUID as the registry value name
			//
			// TODO: GUID needs to be different for the x64 build
			string appname = "hdhomeruntray_" + s_guid.ToString("N").ToUpper();

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
			var container = new Container();

			var exititem = new ToolStripMenuItem("Exit");
			exititem.Click += new EventHandler(OnMenuItemExit);

			// Create the ContextMenuStrip for the tray icon
			var contextmenu = new ContextMenuStrip(container)
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
			m_notifyicon = new ShellNotifyIcon(s_guid, container);
			m_notifyicon.ClosePopup += new EventHandler(OnNotifyIconClosePopup);
			m_notifyicon.OpenPopup += new EventHandler(OnNotifyIconOpenPopup);
			m_notifyicon.Selected += new EventHandler(OnNotifyIconSelected);
			m_notifyicon.ContextMenuStrip = contextmenu;
			m_notifyicon.Icon = StatusIcons.Get(StatusIconType.Idle);
			m_notifyicon.HoverInterval = GetHoverInterval(Settings.Default.TrayIconHoverDelay);
			m_notifyicon.ToolTip = "HDHomeRun Status Monitor";

			// Create the periodic timer object
			m_timer = new System.Timers.Timer
			{
				AutoReset = true,
				Interval = (double)Settings.Default.DiscoveryInterval,
			};
			m_timer.Elapsed += new ElapsedEventHandler(OnTimerElapsed);

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
		private static bool IsAutoStartEnabled()
		{
			// Use hdhomeruntray_TRAYICONGUID as the registry value name
			//
			// TODO: GUID needs to be different for the x64 build
			string appname = "hdhomeruntray_" + s_guid.ToString("N").ToUpper();

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
			string text = new string((char)glyph, 1);	// Convert to string
			Bitmap image = null;						// Image to return

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
					numactive += status.LiveBuffers.Count;
					numactive += status.Playbacks.Count;
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
		//
		// TODO: The x64 version of the application needs a different GUID, the tray settings
		// cache both the GUID and the path to the executable
		private static readonly Guid s_guid = Guid.Parse("{E7E66A47-F253-4D59-BCE1-60193EE55B7C}");
	}
}