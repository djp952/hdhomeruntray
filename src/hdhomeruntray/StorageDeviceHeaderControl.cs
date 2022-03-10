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
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using zuki.hdhomeruntray.discovery;

namespace zuki.hdhomeruntray
{
	//--------------------------------------------------------------------------
	// Class StorageDeviceHeaderControl (internal)
	//
	// User control that implements the header for a storage device in the DeviceForm

	internal partial class StorageDeviceHeaderControl : UserControl
	{
		// Instance Constructor
		//
		private StorageDeviceHeaderControl(SizeF scalefactor)
		{
			InitializeComponent();

			// THEME
			//
			m_appthemechanged = new EventHandler(OnApplicationThemeChanged);
			ApplicationTheme.Changed += m_appthemechanged;
			OnApplicationThemeChanged(this, EventArgs.Empty);

			m_layoutpanel.EnableDoubleBuffering();

			Padding = Padding.ScaleDPI(scalefactor);
			m_layoutpanel.Padding = m_layoutpanel.Padding.ScaleDPI(scalefactor);
			m_layoutpanel.Radii = m_layoutpanel.Radii.ScaleDPI(scalefactor);

			// WINDOWS 11
			//
			if(VersionHelper.IsWindows11OrGreater())
			{
				m_devicename.Font = new Font("Segoe UI Variable Display SemiB", m_devicename.Font.Size, m_devicename.Font.Style);
				m_storageid.Font = new Font("Segoe UI Variable Small", m_storageid.Font.Size, m_storageid.Font.Style);
				m_ipaddress.Font = new Font("Segoe UI Variable Small", m_ipaddress.Font.Size, m_ipaddress.Font.Style);
			}
		}

		// Instance Constructor
		//
		public StorageDeviceHeaderControl(StorageDevice device, SizeF scalefactor) : this(scalefactor)
		{
			if(device == null) throw new ArgumentNullException(nameof(device));

			// Just copy the data from the device instance into the appropriate controls
			m_devicename.Text = device.FriendlyName;
			m_storageid.Text = device.StorageID;
			m_ipaddress.Text = device.LocalIP.ToString();

			// Save the BaseURL for the device for the link target
			m_baseurl = device.BaseURL;
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

		//-------------------------------------------------------------------
		// Event Handlers
		//-------------------------------------------------------------------

		// OnApplicationThemeChanged
		//
		// Invoked when the application theme has changed
		private void OnApplicationThemeChanged(object sender, EventArgs args)
		{
			m_layoutpanel.BackColor = ApplicationTheme.PanelBackColor;
			m_layoutpanel.ForeColor = ApplicationTheme.PanelForeColor;
			m_ipaddress.ActiveLinkColor = m_ipaddress.LinkColor = m_ipaddress.VisitedLinkColor = ApplicationTheme.LinkColor;
		}

		// OnIPAddressClicked
		//
		// Invoked when the IP address link has been clicked
		private void OnIPAddressClicked(object sender, LinkLabelLinkClickedEventArgs args)
		{
			using(Process process = new Process())
			{
				process.StartInfo.FileName = m_baseurl;
				process.StartInfo.UseShellExecute = true;
				process.StartInfo.Verb = "open";
				process.Start();
			}
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private readonly string m_baseurl;
		private readonly EventHandler m_appthemechanged;
	}
}
