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
	// Class DeviceForm (internal)
	//
	// Implements the form that shows detailed information about a device

	partial class DeviceForm : Form
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
		private DeviceForm()
		{
			InitializeComponent();

			this.Padding = this.Padding.ScaleDPI(this.Handle);
			this.m_layoutpanel.Padding = this.m_layoutpanel.Padding.ScaleDPI(this.Handle);
			this.m_layoutpanel.Margin = this.m_layoutpanel.Margin.ScaleDPI(this.Handle);

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
		public DeviceForm(Device device) : this()
		{
			if(device == null) throw new ArgumentNullException(nameof(device));

			// TUNER
			//
			if(device.Type == DeviceType.Tuner)
			{
				Debug.Assert(device is TunerDevice);
				TunerDevice tunerdevice = (TunerDevice)device;

				// Add the header user control for the device
				var header = new TunerDeviceHeaderControl(tunerdevice)
				{
					Dock = DockStyle.Top
				};
				m_layoutpanel.Controls.Add(header);

				// Add the tuner user controls for the device
				foreach(Tuner tuner in tunerdevice.Tuners)
				{
					var status = new TunerDeviceStatusControl(tunerdevice, tuner)
					{
						Dock = DockStyle.Top
					};
					m_layoutpanel.Controls.Add(status);
				}

				// Add the footer user control for the device if non-legacy
				var footer = new TunerDeviceFooterControl(tunerdevice)
				{
					Dock = DockStyle.Top
				};
				m_layoutpanel.Controls.Add(footer);
			}

			// STORAGE
			//
			else if(device.Type == DeviceType.Storage)
			{
				Debug.Assert(device is StorageDevice);
				StorageDevice storagedevice = (StorageDevice)device;

				// Add the header user control for the device
				var header = new StorageDeviceHeaderControl(storagedevice)
				{
					Dock = DockStyle.Top
				};
				m_layoutpanel.Controls.Add(header);

				// Add the recording user controls for the device
				// TODO

				// Add the footer user control for the device
				var footer = new StorageDeviceFooterControl(storagedevice)
				{
					Dock = DockStyle.Top
				};
				m_layoutpanel.Controls.Add(footer);
			}
		}

		//-------------------------------------------------------------------
		// Member Functions
		//-------------------------------------------------------------------

		// ShowFromPopupItem
		//
		// Shows the form at a position based on the popup form and the
		// location of the item that was used to open it
		// bounding rectangle of the notify icon instance
		public void ShowFromPopupItem(PopupForm form, PopupItemControl item)
		{
			if(form == null) throw new ArgumentNullException(nameof(form));
			if(item == null) throw new ArgumentNullException(nameof(item));

			// Set the window position based on the form and item
			SetWindowPosition(form.Bounds, item.Bounds);
			this.Show();
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
		// Sets the window position
		private void SetWindowPosition(Rectangle formbounds, Rectangle itembounds)
		{
			// This should work acceptably well given that the screen/monitor that will
			// display this form is the same one with the taskbar, but there are better ways
			// in .NET 4.7 and/or Windows 10/11 to figure out how to scale this value
			float scalefactor = ((float)SystemInformation.SmallIconSize.Height / 16.0F);

			// The item's coordinates will be relative to the parent form
			var itemleft = formbounds.Left + itembounds.Left;

			// Move the form so that it's centered above the item that was used to open it
			var top = formbounds.Top - this.Size.Height - (int)(4.0F * scalefactor);
			var left = (itemleft + (itembounds.Width / 2)) - (this.Width / 2);

			// Adjust the left margin of the form if necessary
			if(left < formbounds.Left) left = formbounds.Left;

			// Adjust the right margin of the form if necessary
			var right = left + this.Width;
			if(right > formbounds.Right) left -= (right - formbounds.Right);

			// Set the location of the form
			this.Location = new Point(left, top);
		}
	}
}
