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

			[DllImport("gdi32.dll", ExactSpelling = true)]
			[return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
			public static extern bool DeleteObject(IntPtr hObject);

			[DllImport("dwmapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern long DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attribute, ref DWM_WINDOW_CORNER_PREFERENCE pvAttribute, uint cbAttribute);
		}
		#endregion

		// Instance Constructor
		//
		public PopupForm()
		{
			InitializeComponent();

			// For some reason the padding for the form doesn't scale the way I expected
			// and the ScaleControl() method always sends in a ratio of 1:1.  Use a DPI-based
			// manual scaling of the padding to clean this up for now ...
			float factorx = 1.0F;
			float factory = 1.0F;
			using(Graphics gr = Graphics.FromHwnd(this.Handle))
			{
				factorx = gr.DpiX / 96.0F;
				factory = gr.DpiY / 96.0F;
			}

			// TODO: there has to be a better way
			this.Padding = new Padding((int)(this.Padding.Left * factorx), (int)(this.Padding.Top * factory),
				(int)(this.Padding.Right * factorx), (int)(this.Padding.Bottom * factory));

			// WINDOWS 11
			//
			if(VersionHelper.IsWindows11OrGreater())
			{
				// Apply rounded corners to the application
				var attribute = NativeMethods.DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
				var preference = NativeMethods.DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
				NativeMethods.DwmSetWindowAttribute(this.Handle, attribute, ref preference, sizeof(uint));
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
			if(devices == null) throw new ArgumentNullException();

			// If no devices were detected, place a dummy item in the list
			if(devices.Count == 0)
			{
				m_layoutpanel.Controls.Add(PopupItemControl.NoDevices());
				return;
			}

			// Add each device as a PopupItemControl into the layout panel
			foreach(Device device in devices) m_layoutpanel.Controls.Add(new PopupItemControl(device));

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
			var settings = new PopupItemControl(SymbolGlyph.Settings, PopupItemControlType.Toggle);
			settings.Selected += new PopupItemSelectedEventHandler(this.OnSettingsSelected);

			// Crate the unpin button
			var unpin = new PopupItemControl(SymbolGlyph.Unpin, PopupItemControlType.Button);
			unpin.Selected += new PopupItemSelectedEventHandler(this.OnUnpinSelected);

			var exit = new PopupItemControl(SymbolGlyph.Exit, PopupItemControlType.Button);
			exit.Selected += new PopupItemSelectedEventHandler(this.OnExitSelected);

			// Add the glyph items to the outer layout panel
			m_layoutpanel.Controls.Add(settings);
			m_layoutpanel.Controls.Add(unpin);
			m_layoutpanel.Controls.Add(exit);

			// If the form is already visible, it needs to be moved to adjust for the new width
			if(this.Visible) SetWindowPosition(Screen.FromPoint(new Point(this.Left, this.Top)));

			// Prevent adding the controls multiple times by tracking this
			m_pinned = true;
		}

		// ShowFromNotifyIcon
		//
		// Shows the form at a position based on the working area and the
		// bounding rectangle of the notify icon instance
		public void ShowFromNotifyIcon(ShellNotifyIcon icon)
		{
			if(icon == null) throw new ArgumentNullException("icon");

			// Get the boundaries of the notify icon and the associated Screen
			Rectangle iconbounds = icon.GetBounds();
			Screen screen = Screen.FromPoint(iconbounds.Location);

			SetWindowPosition(screen);		// Set the window position
			this.Show();                    // Show the form
			m_timer.Enabled = true;			// Enable the refresh timer
		}

		//-------------------------------------------------------------------
		// Event Handlers
		//-------------------------------------------------------------------

		// OnExitSelected
		//
		// Invoked when the exit button has been clicked
		private void OnExitSelected(object sender, PopupItemSelectedEventArgs args)
		{
			Application.Exit();				// Exit the application
		}

		// OnFormClosing
		//
		// Invoked when the form is closing
		private void OnFormClosing(object sender, FormClosingEventArgs args)
		{
			m_timer.Enabled = false;		// Kill the timer
		}

		// OnSettingsSelected
		//
		// Invoked when the settings toggle has been activated
		private void OnSettingsSelected(object sender, PopupItemSelectedEventArgs args)
		{
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
					if(control is PopupItemControl) control.Refresh();
				}
			}
		}

		// OnUnpinSelected
		//
		// Invoked when the unpin button has been selected
		private void OnUnpinSelected(object sender, PopupItemSelectedEventArgs args)
		{
			this.Close();				// Close this form
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
			float scalefactor = ((float)SystemInformation.SmallIconSize.Height / 16.0F);

			// Move the form to the desired position before showing it; it should be aligned
			// to the lower-right corner of the work area
			var top = screen.WorkingArea.Height - this.Size.Height - (int)(12.0F * scalefactor);
			var left = screen.WorkingArea.Width - this.Size.Width - (int)(12.0F * scalefactor);
			this.Location = new Point(left, top);
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private bool m_pinned = false;
	}
}
