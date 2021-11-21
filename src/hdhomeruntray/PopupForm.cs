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
	// Class PopupForm
	//
	// Form displayed as the popup from the notify icon, provides a display
	// of each discovered device and the status of the tuners/recordings

	internal partial class PopupForm : Form
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
		public PopupForm(DeviceList devices) : this()
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
		}

		//-------------------------------------------------------------------
		// Member Functions
		//-------------------------------------------------------------------

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

			// This should work acceptably well given that the screen/monitor that will
			// display this form is the same one with the taskbar, but there are better ways
			// in .NET 4.7 and/or Windows 10/11 to figure out how to scale this value
			float scalefactor = ((float)SystemInformation.SmallIconSize.Height / 16.0F);

			// Move the form to the desired position before showing it; it should be aligned
			// to the lower-right corner of the work area
			var top = screen.WorkingArea.Height - this.Size.Height - (int)(12.0F * scalefactor);
			var left = screen.WorkingArea.Width - this.Size.Width - (int)(12.0F * scalefactor);
			this.Location = new Point(left, top);

			this.Show();                    // Show the form at the calculated position
			m_timer.Enabled = true;			// Enable the refresh timer
		}

		//-------------------------------------------------------------------
		// Event Handlers
		//-------------------------------------------------------------------

		// OnTimerTick
		//
		// Invoked when the timer comes due
		private void OnTimerTick(object sender, EventArgs args)
		{
			foreach(Control control in m_layoutpanel.Controls)
			{
				if(control is PopupItemControl) control.Refresh();
			}
		}

		private void OnFormClosing(object sender, FormClosingEventArgs args)
		{
			m_timer.Enabled = false;
		}
	}
}
