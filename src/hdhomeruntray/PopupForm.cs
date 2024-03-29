﻿//---------------------------------------------------------------------------
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
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using zuki.hdhomeruntray.discovery;

namespace zuki.hdhomeruntray
{
	//-----------------------------------------------------------------------
	// Class PopupForm (internal)
	//
	// Form displayed as the popup from the notify icon, provides a display
	// of each discovered device and the status of the tuners/recordings

	internal partial class PopupForm : Form
	{
		#region Win32 API Declarations
		private static class NativeMethods
		{
			public const uint ABM_GETTASKBARPOS = 0x00000005;
			public const int WS_EX_COMPOSITED = 0x02000000;

			public enum DWMWINDOWATTRIBUTE
			{
				DWMWA_WINDOW_CORNER_PREFERENCE = 33
			}

			public enum DWM_WINDOW_CORNER_PREFERENCE
			{
				DWMWCP_DEFAULT = 0,
				DWMWCP_DONOTROUND = 1,
				DWMWCP_ROUND = 2,
				DWMWCP_ROUNDSMALL = 3
			}

			[StructLayout(LayoutKind.Sequential)]
			public struct RECT
			{
				public int left;
				public int top;
				public int right;
				public int bottom;
			}

			[StructLayout(LayoutKind.Sequential)]
			public struct APPBARDATA
			{
				public uint cbSize;
				public IntPtr hWnd;
				public uint uCallbackMessage;
				public uint uEdge;
				public RECT rc;
				public IntPtr lParam;
			}

			[DllImport("dwmapi.dll")]
			public static extern long DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attribute, ref DWM_WINDOW_CORNER_PREFERENCE pvAttribute, uint cbAttribute);

			[DllImport("shell32.dll")]
			public static extern IntPtr SHAppBarMessage(uint dwMessage, ref APPBARDATA pData);
		}
		#endregion

		// Instance Constructor (private)
		//
		private PopupForm()
		{
			InitializeComponent();

			// THEME
			//
			m_appthemechanged = new EventHandler(OnApplicationThemeChanged);
			ApplicationTheme.Changed += m_appthemechanged;
			OnApplicationThemeChanged(this, EventArgs.Empty);

			m_layoutpanel.EnableDoubleBuffering();

			// WINDOWS 11
			//
			if(VersionHelper.IsWindows11OrGreater())
			{
				// Change the padding to 4 (will scale below)
				Padding = new Padding(4);

				// Apply rounded corners to the form
				NativeMethods.DWMWINDOWATTRIBUTE attribute = NativeMethods.DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
				NativeMethods.DWM_WINDOW_CORNER_PREFERENCE preference = NativeMethods.DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
				NativeMethods.DwmSetWindowAttribute(Handle, attribute, ref preference, sizeof(uint));
			}

			// WINDOWS 10
			//
			else if(VersionHelper.IsWindows10OrGreater())
			{
				// Switch to a single border and change the padding to 2 (will scale below)
				FormBorderStyle = FormBorderStyle.FixedToolWindow;
				Padding = new Padding(2);
			}

			// Using CreateGraphics() in every form/control that autoscales was
			// causing a performance concern; calculate the factor only once
			using(Graphics graphics = CreateGraphics())
			{
				m_scalefactor = new SizeF(graphics.DpiX / 96.0F, graphics.DpiY / 96.0F);
			}

			Padding = Padding.ScaleDPI(m_scalefactor);
			m_layoutpanel.Margin = m_layoutpanel.Margin.ScaleDPI(m_scalefactor);
			m_layoutpanel.Padding = m_layoutpanel.Padding.ScaleDPI(m_scalefactor);
		}

		// Instance Constructor
		//
		public PopupForm(DeviceList devices) : this(devices, false, null)
		{
		}

		// Instance Constructor
		//
		public PopupForm(DeviceList devices, bool pinned, ShellNotifyIcon icon) : this()
		{
			if(devices == null) throw new ArgumentNullException(nameof(devices));

			// If no devices were detected, place a dummy item in the list
			if(devices.Count == 0)
			{
				m_layoutpanel.Controls.Add(new PopupItemLabelControl("No HDHomeRun devices detected", m_scalefactor));
				return;
			}

			// Add each device as a PopupItemControl into the layout panel
			for(int index = 0; index < devices.Count; index++)
			{
				PopupItemDeviceControl devicecontrol = new PopupItemDeviceControl(devices[index], m_scalefactor);

				// If there is more than one device to display
				if(devices.Count > 1)
				{
					// The first device only gets right padding
					if(index == 0) devicecontrol.Padding = new Padding(0, 0, 1, 0).ScaleDPI(m_scalefactor);

					// The last device only gets left padding
					else if(index == (devices.Count - 1)) devicecontrol.Padding = new Padding(1, 0, 0, 0).ScaleDPI(m_scalefactor);

					// Middle devices get both left and right padding
					else devicecontrol.Padding = new Padding(1, 0, 1, 0).ScaleDPI(m_scalefactor);
				}

				devicecontrol.Toggled += new ToggledEventHandler(OnDeviceToggled);
				m_layoutpanel.Controls.Add(devicecontrol);
			}

			// If the window is supposed to be pinned, pin it
			if((pinned) && (icon != null)) Pin(icon);
		}

		// Dispose
		//
		// Releases unmanaged resources and optionally releases managed resources
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				// Dispose managed state
				if(m_appthemechanged != null) ApplicationTheme.Changed -= m_appthemechanged;
				if(components != null) components.Dispose();
			}

			base.Dispose(disposing);
		}

		//-------------------------------------------------------------------------
		// Events
		//-------------------------------------------------------------------------

		// DeviceStatusChanged
		//
		// Invoked when the overall device status has changed
		public event DeviceStatusChangedEventHandler DeviceStatusChanged;

		// Unpinned
		//
		// Invoked when the popup form has been unpinned
		public event EventHandler Unpinned;

		//-------------------------------------------------------------------------
		// Properties
		//-------------------------------------------------------------------------

		// DockStyle
		//
		// Exposes the DockStyle of the PopupForm instance
		public DockStyle DockStyle => m_dockstyle;

		// Pinned
		//
		// Indicates if the popup form is currently pinned or not
		public bool Pinned => m_pinned;

		//-------------------------------------------------------------------
		// Member Functions
		//-------------------------------------------------------------------

		// Pin
		//
		// "Pins" the popup form by adding additional controls
		public void Pin(ShellNotifyIcon icon)
		{
			Activate();								// Activate the form

			if(m_pinned) return;

			// Create the settings toggle
			PopupItemGlyphControl settings = new PopupItemGlyphControl(SymbolGlyph.Settings, PopupItemControlType.Toggle, m_scalefactor)
			{
				Padding = new Padding(2, 0, 1, 0).ScaleDPI(m_scalefactor)
			};
			settings.Toggled += new ToggledEventHandler(OnSettingsToggled);

			// Crate the unpin button
			PopupItemGlyphControl unpin = new PopupItemGlyphControl(SymbolGlyph.Unpin, PopupItemControlType.Button, m_scalefactor)
			{
				Padding = new Padding(1, 0, 1, 0).ScaleDPI(m_scalefactor)
			};
			unpin.Selected += new EventHandler(OnUnpinSelected);

			// Add the glyph items to the outer layout panel
			m_layoutpanel.SuspendLayout();
			m_layoutpanel.Controls.AddRange(new Control[] { settings, unpin });
			m_layoutpanel.ResumeLayout();

			// For auto unpin, the location of the icon when the form was pinned is needed
			m_pinnediconrect = icon.GetBounds();

			// If the form is already visible, it needs to be moved to adjust for the new width
			if(Visible) SetWindowPosition(m_pinnediconrect, Screen.FromPoint(new Point(Left, Top)));

			// For auto unpin, the location of the icon when the form was pinned is needed
			m_pinnediconrect = icon.GetBounds();

			// Prevent adding the controls multiple times by tracking this
			m_pinned = true;
		}

		// ShowFromNotifyIcon
		//
		// Shows the form at a position based on the working area and the
		// bounding rectangle of the notify icon instance
		public void ShowFromNotifyIcon(ShellNotifyIcon icon)
		{
			if(icon == null) throw new ArgumentNullException(nameof(icon));

			// Get the boundaries of the notify icon and the associated Screen
			Rectangle iconbounds = icon.GetBounds();
			Screen screen = Screen.FromPoint(iconbounds.Location);

			// Get the dock style for the taskbar and use it for this and any
			// other windows that will be created
			m_dockstyle = GetTaskbarDockStyle(screen);

			SetWindowPosition(iconbounds, screen);      // Set the window position
			
			// Perform the initial refresh and start the timer
			OnTimerTick(this, EventArgs.Empty);
			m_timer.Enabled = true;

			Show();                         // Show the form
			Activate();						// Activate the form
		}

		//-------------------------------------------------------------------
		// Event Handlers
		//-------------------------------------------------------------------

		// OnApplicationThemeChanged
		//
		// Invoked when the application theme has changed
		private void OnApplicationThemeChanged(object sender, EventArgs args)
		{
			BackColor = ApplicationTheme.FormBackColor;
		}

		// OnDeactivate
		//
		// Invoked when the form has been deactivated
		private void OnDeactivate(object sender, EventArgs args)
		{
			// If the form is pinned and the setting to auto-unpin is set, this
			// event handler is enabled
			if((m_pinned) && (Properties.Settings.Default.UnpinAutomatically == EnabledDisabled.Enabled))
			{
				Point cursorpos = Cursor.Position;

				bool inpopup = ClientRectangle.Contains(PointToClient(cursorpos));
				bool inicon = m_pinnediconrect.Contains(cursorpos);

				// If the cursor is not in the popup form client area and not in the icon, unpin
				if(!inpopup && !inicon) OnUnpinSelected(this, EventArgs.Empty);
			}
		}

		// OnDeviceStatusChanged
		//
		// Invoked when the of a device has changed
		private void OnDeviceStatusChanged(object sender, DeviceStatusChangedEventArgs args)
		{
			DeviceStatus newstatus = args.DeviceStatus;

			// Iterate over all of the device controls to recheck the status in order to
			// fire our DeviceStatusChanged event and update the tray icon
			foreach(Control control in m_layoutpanel.Controls)
			{
				if(control is PopupItemDeviceControl devicecontrol)
				{
					// TODO: A proper Device.Equals() would be better
					if(devicecontrol.Device.BaseURL == args.Device.BaseURL) devicecontrol.SetDotColor(args.Index, StatusColor.FromDeviceStatus(args.DeviceStatus));
					if(devicecontrol.DeviceStatus > newstatus) newstatus = devicecontrol.DeviceStatus;
				}
			}

			// Check for an overall device status change and fire the event
			if(newstatus != m_status)
			{
				DeviceStatusChanged?.Invoke(this, new DeviceStatusChangedEventArgs(newstatus));
				m_status = newstatus;
			}
		}

		// OnDeviceToggled
		//
		// Invoked when a device toggle state has been changed
		private void OnDeviceToggled(object sender, ToggledEventArgs args)
		{
			if(args.Toggled)
			{
				UntoggleOthers(sender);     // Untoggle any other toggled control

				Debug.Assert(sender is PopupItemDeviceControl);
				PopupItemDeviceControl devicecontrol = (PopupItemDeviceControl)sender;

				if(m_deviceform == null)
				{
					// TUNER DEVICE
					//
					if(devicecontrol.Device.Type == DeviceType.Tuner)
					{
						Debug.Assert(devicecontrol.Device is TunerDevice);
						TunerDevice tunerdevice = (TunerDevice)devicecontrol.Device;
						m_deviceform = new TunerDeviceForm(tunerdevice, this, devicecontrol, m_dockstyle);
						m_deviceform.DeviceStatusChanged += new DeviceStatusChangedEventHandler(OnDeviceStatusChanged);
					}

					// STORAGE DEVICE
					//
					else if(devicecontrol.Device.Type == DeviceType.Storage)
					{
						Debug.Assert(devicecontrol.Device is StorageDevice);
						StorageDevice storagedevice = (StorageDevice)devicecontrol.Device;
						m_deviceform = new StorageDeviceForm(storagedevice, this, devicecontrol, m_dockstyle);
						m_deviceform.DeviceStatusChanged += new DeviceStatusChangedEventHandler(OnDeviceStatusChanged);
					}

					if(m_deviceform != null)
					{
						m_deviceform.Deactivate += new EventHandler(OnDeactivate);
						m_deviceform.Show(this);
					}
				}
			}

			else if(m_deviceform != null)
			{
				m_deviceform.Close();
				m_deviceform.Dispose();
				m_deviceform = null;
			}
		}

		// OnFormClosing
		//
		// Invoked when the form is closing
		private void OnFormClosing(object sender, FormClosingEventArgs args)
		{
			// Close and destory the device form if active
			if(m_deviceform != null)
			{
				m_deviceform.Close();
				m_deviceform.Dispose();
				m_deviceform = null;
			}

			// Close and destroy the settings form if active
			if(m_settingsform != null)
			{
				m_settingsform.Close();
				m_settingsform.Dispose();
				m_settingsform = null;
			}

			m_timer.Enabled = false;        // Kill the timer
			m_dockstyle = DockStyle.None;	// Reset the dock style
		}

		// OnSettingsToggled
		//
		// Invoked when the settings toggle state has been changed
		private void OnSettingsToggled(object sender, ToggledEventArgs args)
		{
			if(args.Toggled)
			{
				UntoggleOthers(sender);

				if(m_settingsform == null)
				{
					m_settingsform = new SettingsForm(this, (PopupItemControl)sender, m_dockstyle);
					m_settingsform.Deactivate += new EventHandler(OnDeactivate);
					m_settingsform.Show(this);
				}
			}

			else if(m_settingsform != null)
			{
				m_settingsform.Close();
				m_settingsform.Dispose();
				m_settingsform = null;
			}
		}

		// OnTimerTick
		//
		// Invoked when the timer comes due
		private void OnTimerTick(object sender, EventArgs args)
		{
			DeviceStatus newstatus = DeviceStatus.Idle;

			foreach(Control control in m_layoutpanel.Controls)
			{
				if(control is PopupItemDeviceControl devicecontrol)
				{
					devicecontrol.Refresh();
					if(devicecontrol.DeviceStatus > newstatus) newstatus = devicecontrol.DeviceStatus;
				}
			}

			// Check for an overall device status change and fire the event
			if(newstatus != m_status)
			{
				DeviceStatusChanged?.Invoke(this, new DeviceStatusChangedEventArgs(newstatus));
				m_status = newstatus;
			}
		}

		// OnUnpinSelected
		//
		// Invoked when the unpin button has been selected
		private void OnUnpinSelected(object sender, EventArgs args)
		{
			Unpinned?.Invoke(this, EventArgs.Empty);
			m_pinned = false;
			m_pinnediconrect = Rectangle.Empty;
		}

		//-------------------------------------------------------------------
		// Control Overrides
		//-------------------------------------------------------------------

		// CreateParams
		//
		// Gets the required creation parameters when the control handle is created
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= NativeMethods.WS_EX_COMPOSITED;
				return cp;
			}
		}

		//-------------------------------------------------------------------
		// Private Member Functions
		//-------------------------------------------------------------------

		// GetTaskbarDockStyle
		//
		// Gets the taskbar dock for the system
		private DockStyle GetTaskbarDockStyle(Screen screen)
		{
			NativeMethods.APPBARDATA appbardata = new NativeMethods.APPBARDATA()
			{
				cbSize = (uint)Marshal.SizeOf(typeof(NativeMethods.APPBARDATA))
			};

			// Try to get the taskbar position, this method accounts for the taskbar being collapsed
			if(NativeMethods.SHAppBarMessage(NativeMethods.ABM_GETTASKBARPOS, ref appbardata) != IntPtr.Zero)
			{
				int midx = (screen.Bounds.Left + screen.Bounds.Right) / 2;
				int midy = (screen.Bounds.Top + screen.Bounds.Bottom) / 2;

				if(appbardata.rc.right < midx) return DockStyle.Left;
				else if(appbardata.rc.bottom < midy) return DockStyle.Top;
				else if(appbardata.rc.left > midx) return DockStyle.Right;
			}

			return DockStyle.Bottom;
		}

		// SetWindowPosition
		//
		// Sets the window position so that it's in the lower right of the screen
		private void SetWindowPosition(Rectangle iconrect, Screen screen)
		{
			// This should work acceptably well given that the screen/monitor that will
			// display this form is the same one with the taskbar, but there are better ways
			// in .NET 4.7 and/or Windows 10/11 to figure out how to scale this value
			int padding = (int)(12.0F * (SystemInformation.SmallIconSize.Height / 16.0F));

			Rectangle workarea = screen.WorkingArea;    // Screen working area
			int top;									// Window top coordinate
			int left;									// Window left coordinate

			// TOP DOCK
			//
			if(m_dockstyle == DockStyle.Top)
			{
				// Adjust the working area if the tray icon isn't docked
				if(iconrect.Bottom > workarea.Top)
					workarea = new Rectangle(workarea.Left, iconrect.Bottom, workarea.Width, workarea.Height - (screen.WorkingArea.Bottom - iconrect.Bottom));

				// Position the window in the upper right corner of the (adjusted) working area
				top = workarea.Top + padding;
				left = workarea.Width - Size.Width - padding;
			}

			// LEFT DOCK
			//
			// TODO: REVERSE CONTROLS
			else if(m_dockstyle == DockStyle.Left)
			{
				// Adjust the working area if the tray icon isn't docked
				if(iconrect.Right > workarea.Left)
					workarea = new Rectangle(iconrect.Right, workarea.Top, workarea.Width - (screen.WorkingArea.Right - iconrect.Right), workarea.Height);

				// Position the window so it's centered horizontally with the icon and left-aigned in the work area
				top = (iconrect.Top + (iconrect.Height / 2)) - (Size.Height / 2);
				if((top + Height + padding) > (workarea.Bottom)) top = workarea.Bottom - Height - padding;
				left = workarea.Left + padding;
			}

			// RIGHT DOCK
			//
			else if(m_dockstyle == DockStyle.Right)
			{
				// Adjust the working area if the tray icon isn't docked
				if(iconrect.Left < workarea.Right)
					workarea = new Rectangle(workarea.Left, workarea.Top, workarea.Width - (screen.WorkingArea.Right - iconrect.Left), workarea.Height);

				// Position the window so it's centered horizontally with the icon and right-aigned in the work area
				top = (iconrect.Top + (iconrect.Height / 2)) - (Size.Height / 2);
				if((top + Height + padding) > (workarea.Bottom)) top = workarea.Bottom - Height - padding;
				left = workarea.Width - Size.Width - padding;
			}

			// BOTTOM DOCK
			//
			else
			{
				// Adjust the working area if the tray icon isn't docked
				if(iconrect.Top < workarea.Bottom)
					workarea = new Rectangle(workarea.Left, workarea.Top, workarea.Width, workarea.Height - (screen.WorkingArea.Bottom - iconrect.Top));

				// Position the window in the lower right corner of the (adjusted) working area
				top = workarea.Height - Size.Height - padding;
				left = workarea.Width - Size.Width - padding;
			}

			// Move the form to the desired position set it as topmost
			Location = new Point(left, top);
			TopMost = true;
		}

		// UntoggleOthers
		//
		// Untoggles any toggle-type popup items that aren't the new active one
		private void UntoggleOthers(object toggled)
		{
			foreach(Control control in m_layoutpanel.Controls)
			{
				if(control is PopupItemControl popupitemcontrol)
				{
					if(popupitemcontrol.ControlType == PopupItemControlType.Toggle)
					{
						if(popupitemcontrol != toggled && popupitemcontrol.IsToggled)
						{
							popupitemcontrol.Toggle(false);
						}
					}
				}
			}
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private bool m_pinned = false;
		private Rectangle m_pinnediconrect = Rectangle.Empty;
		private DeviceForm m_deviceform = null;
		private SettingsForm m_settingsform = null;
		private DeviceStatus m_status = DeviceStatus.Idle;
		private readonly SizeF m_scalefactor = SizeF.Empty;
		private readonly EventHandler m_appthemechanged;
		private DockStyle m_dockstyle = DockStyle.None;
	}
}
