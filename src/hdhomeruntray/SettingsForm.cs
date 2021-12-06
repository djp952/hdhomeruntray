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
using zuki.hdhomeruntray.Properties;

namespace zuki.hdhomeruntray
{
	//-----------------------------------------------------------------------
	// Class SettingsForm (internal)
	//
	// Implements the form that provides the means to manipulate the settings

	partial class SettingsForm : Form
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

		// Instance Constructor
		//
		public SettingsForm()
		{
			InitializeComponent();

			Padding = Padding.ScaleDPI(Handle);
			m_layoutPanel.Padding = m_layoutPanel.Padding.ScaleDPI(Handle);

			// WINDOWS 11
			//
			if(VersionHelper.IsWindows11OrGreater())
			{
				// Switch to Segoe UI Variable Display
				Font = new Font("Segoe UI Variable Display Semib", 9F, FontStyle.Bold);

				// Apply rounded corners to the application
				var attribute = NativeMethods.DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
				var preference = NativeMethods.DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
				NativeMethods.DwmSetWindowAttribute(Handle, attribute, ref preference, sizeof(uint));
			}

			// Bind each of the ComboBox drop-downs to their enum class
			m_discoverymethod.BindEnum(Settings.Default.DiscoveryMethod);
			m_discoveryinterval.BindEnum(Settings.Default.DiscoveryInterval);
			m_trayiconhoverdelay.BindEnum(Settings.Default.TrayIconHoverDelay);
			m_tunerstatuscolorsource.BindEnum(Settings.Default.TunerStatusColorSource);
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
			Show();
		}

		//-------------------------------------------------------------------
		// Event Handlers
		//-------------------------------------------------------------------

		// OnDiscoveryIntervalCommitted
		//
		// Invoked when a change to the combobox is committed
		private void OnDiscoveryIntervalCommitted(object sender, EventArgs args)
		{
			// If the value of the combobox changed, update and save the settings
			DiscoveryInterval discoveryinterval = (DiscoveryInterval)m_discoveryinterval.SelectedValue;
			if(discoveryinterval != Settings.Default.DiscoveryInterval)
			{
				Settings.Default.DiscoveryInterval = discoveryinterval;
				Settings.Default.Save();
			}
		}

		// OnDiscoveryMethodCommitted
		//
		// Invoked when a change to the combobox is committed
		private void OnDiscoveryMethodCommitted(object sender, EventArgs args)
		{
			// If the value of the combobox changed, update and save the settings
			DiscoveryMethod discoverymethod = (DiscoveryMethod)m_discoverymethod.SelectedValue;
			if(discoverymethod != Settings.Default.DiscoveryMethod)
			{
				Settings.Default.DiscoveryMethod = discoverymethod;
				Settings.Default.Save();
			}
		}

		// OnTrayIconHoverDelayCommitted
		//
		// Invoked when a change to the combobox is committed
		private void OnTrayIconHoverDelayCommitted(object sender, EventArgs args)
		{
			// If the value of the combobox changed, update and save the settings
			TrayIconHoverDelay trayiconhoverdelay = (TrayIconHoverDelay)m_trayiconhoverdelay.SelectedValue;
			if(trayiconhoverdelay != Settings.Default.TrayIconHoverDelay)
			{
				Settings.Default.TrayIconHoverDelay = trayiconhoverdelay;
				Settings.Default.Save();
			}
		}

		// OnTunerStatusColorSourceCommitted
		//
		// Invoked when a change to the combobox is committed
		private void OnTunerStatusColorSourceCommitted(object sender, EventArgs args)
		{
			// If the value of the combobox changed, update and save the settings
			TunerStatusColorSource tunerstatuscolorsource = (TunerStatusColorSource)m_tunerstatuscolorsource.SelectedValue;
			if(tunerstatuscolorsource != Settings.Default.TunerStatusColorSource)
			{
				Settings.Default.TunerStatusColorSource = tunerstatuscolorsource;
				Settings.Default.Save();
			}
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
			float scalefactor = (SystemInformation.SmallIconSize.Height / 16.0F);

			// The item's coordinates will be relative to the parent form
			var itemleft = formbounds.Left + itembounds.Left;

			// Move the form so that it's centered above the item that was used to open it
			var top = formbounds.Top - Size.Height - (int)(4.0F * scalefactor);
			var left = (itemleft + (itembounds.Width / 2)) - (Width / 2);

			// Adjust the left margin of the form if necessary
			if(left < formbounds.Left) left = formbounds.Left;

			// Adjust the right margin of the form if necessary
			var right = left + Width;
			if(right > formbounds.Right) left -= (right - formbounds.Right);

			// Set the location of the form
			Location = new Point(left, top);
		}
	}
}
