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

	internal abstract partial class DeviceForm : Form
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

			// THEME
			//
			m_appthemechanged = new EventHandler(OnApplicationThemeChanged);
			ApplicationTheme.Changed += m_appthemechanged;
			OnApplicationThemeChanged(this, EventArgs.Empty);

			m_layoutpanel.EnableDoubleBuferring();

			// WINDOWS 11
			//
			if(VersionHelper.IsWindows11OrGreater())
			{
				// Remove the border and change the padding to 4 (will scale below)
				FormBorderStyle = FormBorderStyle.None;
				Padding = new Padding(4);

				// Apply rounded corners to the form
				NativeMethods.DWMWINDOWATTRIBUTE attribute = NativeMethods.DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
				NativeMethods.DWM_WINDOW_CORNER_PREFERENCE preference = NativeMethods.DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
				NativeMethods.DwmSetWindowAttribute(Handle, attribute, ref preference, sizeof(uint));
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

		// Instance Constructor (protected)
		//
		protected DeviceForm(PopupForm form, PopupItemControl item) : this()
		{
			if(form == null) throw new ArgumentNullException(nameof(form));
			if(item == null) throw new ArgumentNullException(nameof(item));

			m_popupformbounds = form.Bounds;
			m_popupitembounds = item.Bounds;

			// Avoid repeated calls to OnSizeChanged() by adding the event handler
			// after the form has been initialized and call it manually the first time
			SizeChanged += new EventHandler(OnSizeChanged);
			OnSizeChanged(this, EventArgs.Empty);
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
		// Invoked when the status of a tuner has changed
		public event DeviceStatusChangedEventHandler DeviceStatusChanged;

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
		// Event Handlers
		//-------------------------------------------------------------------

		// OnApplicationThemeChanged
		//
		// Invoked when the application theme has changed
		private void OnApplicationThemeChanged(object sender, EventArgs args)
		{
			BackColor = ApplicationTheme.FormBackColor;
		}

		// OnSizeChanged
		//
		// Invoked when the size of the form has changed
		private void OnSizeChanged(object sender, EventArgs args)
		{
			// This should work acceptably well given that the screen/monitor that will
			// display this form is the same one with the taskbar, but there are better ways
			// in .NET 4.7 and/or Windows 10/11 to figure out how to scale this value
			float scalefactor = (SystemInformation.SmallIconSize.Height / 16.0F);

			// The item's coordinates will be relative to the parent form
			int itemleft = m_popupformbounds.Left + m_popupitembounds.Left;

			// Move the form so that it's centered above the item that was used to open it
			int top = m_popupformbounds.Top - Size.Height - (int)(4.0F * scalefactor);
			int left = (itemleft + (m_popupitembounds.Width / 2)) - (Width / 2);

			// Adjust the left margin of the form if necessary
			if(left < m_popupformbounds.Left) left = m_popupformbounds.Left;

			// Adjust the right margin of the form if necessary
			int right = left + Width;
			if(right > m_popupformbounds.Right) left -= (right - m_popupformbounds.Right);

			// Set the location of the form
			Location = new Point(left, top);
		}

		//-------------------------------------------------------------------
		// Protected Member Functions
		//-------------------------------------------------------------------

		// RaiseDeviceStatusChanged
		//
		// Allows derived classes to invoke the DeviceStatusChanged event
		protected void RaiseDeviceStatusChanged(object sender, DeviceStatusChangedEventArgs args)
		{
			DeviceStatusChanged?.Invoke(sender, args);
		}

		//-------------------------------------------------------------------
		// Protected Member Variables
		//-------------------------------------------------------------------

		protected readonly SizeF m_scalefactor = SizeF.Empty;

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private readonly Rectangle m_popupformbounds;
		private readonly Rectangle m_popupitembounds;
		private readonly EventHandler m_appthemechanged;
	}
}
