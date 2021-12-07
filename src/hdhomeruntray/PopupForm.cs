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

	partial class PopupForm : Form
	{
		#region Win32 API Declarations
		private static class NativeMethods
		{
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

			[DllImport("dwmapi.dll")]
			public static extern long DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attribute, ref DWM_WINDOW_CORNER_PREFERENCE pvAttribute, uint cbAttribute);
		}
		#endregion

		// Instance Constructor (private)
		//
		private PopupForm()
		{
			InitializeComponent();

			// Scale the padding based on the form DPI
			Padding = Padding.ScaleDPI(Handle);
			m_layoutpanel.Margin = m_layoutpanel.Margin.ScaleDPI(Handle);
			m_layoutpanel.Padding = m_layoutpanel.Padding.ScaleDPI(Handle);

			// WINDOWS 11
			//
			if(VersionHelper.IsWindows11OrGreater())
			{
				// Apply rounded corners to the application
				var attribute = NativeMethods.DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
				var preference = NativeMethods.DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
				NativeMethods.DwmSetWindowAttribute(Handle, attribute, ref preference, sizeof(uint));
			}
		}

		// Instance Constructor
		//
		public PopupForm(DeviceList devices) : this(devices, false)
		{
		}

		// Instance Constructor
		//
		public PopupForm(DeviceList devices, bool pinned) : this()
		{
			if(devices == null) throw new ArgumentNullException(nameof(devices));

			// If no devices were detected, place a dummy item in the list
			if(devices.Count == 0)
			{
				m_layoutpanel.Controls.Add(new PopupItemLabelControl("No HDHomeRun devices detected"));
				return;
			}

			// Add each device as a PopupItemControl into the layout panel
			foreach(Device device in devices)
			{
				PopupItemDeviceControl devicecontrol = new PopupItemDeviceControl(device);
				devicecontrol.Toggled += new PopupItemToggledEventHandler(OnDeviceToggled);
				m_layoutpanel.Controls.Add(devicecontrol);
			}

			// If the window is supposed to be pinned, pin it
			if(pinned) Pin();
		}

		//-------------------------------------------------------------------
		// Member Functions
		//-------------------------------------------------------------------

		public bool Pinned
		{
			get { return m_pinned; }
		}

		// Pin
		//
		// "Pins" the popup form by adding additional controls
		public void Pin()
		{
			if(m_pinned) return;

			// Create the settings toggle
			var settings = new PopupItemGlyphControl(SymbolGlyph.Settings, PopupItemControlType.Toggle);
			settings.Toggled += new PopupItemToggledEventHandler(OnSettingsToggled);

			// Crate the unpin button
			var unpin = new PopupItemGlyphControl(SymbolGlyph.Unpin, PopupItemControlType.Button);
			unpin.Selected += new EventHandler(OnUnpinSelected);

			// Create the exit button
			var exit = new PopupItemGlyphControl(SymbolGlyph.Exit, PopupItemControlType.Button);
			exit.Selected += new EventHandler(OnExitSelected);

			// Add the glyph items to the outer layout panel
			m_layoutpanel.SuspendLayout();
			m_layoutpanel.Controls.Add(settings);
			m_layoutpanel.Controls.Add(unpin);
			m_layoutpanel.Controls.Add(exit);
			m_layoutpanel.ResumeLayout();

			// If the form is already visible, it needs to be moved to adjust for the new width
			if(Visible) SetWindowPosition(Screen.FromPoint(new Point(Left, Top)));

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

			SetWindowPosition(screen);		// Set the window position
			Show();                    // Show the form
			m_timer.Enabled = true;			// Enable the refresh timer
		}

		//-------------------------------------------------------------------
		// Event Handlers
		//-------------------------------------------------------------------

		// OnDeviceToggled
		//
		// Invoked when a device toggle state has been changed
		private void OnDeviceToggled(object sender, PopupItemToggledEventArgs args)
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
						m_deviceform = new TunerDeviceForm(tunerdevice, this, devicecontrol);
					}

					// STORAGE DEVICE
					//
					else if(devicecontrol.Device.Type == DeviceType.Storage)
					{
						Debug.Assert(devicecontrol.Device is StorageDevice);
						StorageDevice storagedevice = (StorageDevice)devicecontrol.Device;
						m_deviceform = new StorageDeviceForm(storagedevice, this, devicecontrol);
					}

					if(m_deviceform != null) m_deviceform.Show();
				}
			}

			else if(m_deviceform != null)
			{
				m_deviceform.Close();
				m_deviceform.Dispose();
				m_deviceform = null;
			}
		}

		// OnExitSelected
		//
		// Invoked when the exit button has been clicked
		private void OnExitSelected(object sender, EventArgs args)
		{
			Close();					// Close this form
			Application.Exit();				// Exit the application
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

			m_timer.Enabled = false;		// Kill the timer
		}

		// OnSettingsToggled
		//
		// Invoked when the settings toggle state has been changed
		private void OnSettingsToggled(object sender, PopupItemToggledEventArgs args)
		{
			if(args.Toggled)
			{
				UntoggleOthers(sender);

				if(m_settingsform == null)
				{
					m_settingsform = new SettingsForm();
					m_settingsform.ShowFromPopupItem(this, (PopupItemControl)sender);
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
			// Probably can't happen, but make sure the timer is still
			// enabled before executing the refresh
			if(m_timer.Enabled)
			{
				foreach(Control control in m_layoutpanel.Controls)
				{
					if(control is PopupItemDeviceControl devicecontrol) devicecontrol.Refresh();
				}
			}
		}

		// OnUnpinSelected
		//
		// Invoked when the unpin button has been selected
		private void OnUnpinSelected(object sender, EventArgs args)
		{
			Close();				// Close this form
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

		// SetWindowPosition
		//
		// Sets the window position so that it's in the lower right of the screen
		private void SetWindowPosition(Screen screen)
		{
			// This should work acceptably well given that the screen/monitor that will
			// display this form is the same one with the taskbar, but there are better ways
			// in .NET 4.7 and/or Windows 10/11 to figure out how to scale this value
			float scalefactor = (SystemInformation.SmallIconSize.Height / 16.0F);

			// Move the form to the desired position before showing it; it should be aligned
			// to the lower-right corner of the work area
			var top = screen.WorkingArea.Height - Size.Height - (int)(12.0F * scalefactor);
			var left = screen.WorkingArea.Width - Size.Width - (int)(12.0F * scalefactor);
			Location = new Point(left, top);
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
						//if(!Object.ReferenceEquals(popupitemcontrol, toggled))
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
		private DeviceForm m_deviceform = null;
		private SettingsForm m_settingsform = null;
	}
}
