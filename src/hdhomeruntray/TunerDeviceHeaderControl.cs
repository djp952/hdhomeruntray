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
using System.Windows.Forms;

using zuki.hdhomeruntray.discovery;

namespace zuki.hdhomeruntray
{
	//--------------------------------------------------------------------------
	// Class TunerDeviceHeaderControl (internal)
	//
	// User control that implements the header for a tuner device in the DeviceForm

	internal partial class TunerDeviceHeaderControl : UserControl
	{
		// Instance Constructor
		//
		private TunerDeviceHeaderControl()
		{
			InitializeComponent();

			m_layoutpanel.EnableDoubleBuferring();

			using(Graphics graphics = CreateGraphics())
			{
				Padding = Padding.ScaleDPI(graphics);
				m_layoutpanel.Margin = m_layoutpanel.Margin.ScaleDPI(graphics);
				m_layoutpanel.Padding = m_layoutpanel.Padding.ScaleDPI(graphics);
				m_layoutpanel.Radii = m_layoutpanel.Radii.ScaleDPI(graphics);
			}

			// WINDOWS 11
			//
			if(VersionHelper.IsWindows11OrGreater())
			{
				m_devicename.Font = new Font("Segoe UI Variable Display SemiB", m_devicename.Font.Size, m_devicename.Font.Style);
				m_modelname.Font = new Font("Segoe UI Variable Display SemiB", m_modelname.Font.Size, m_modelname.Font.Style);
				m_deviceid.Font = new Font("Segoe UI Variable Small", m_deviceid.Font.Size, m_deviceid.Font.Style);
				m_ipaddress.Font = new Font("Segoe UI Variable Small", m_ipaddress.Font.Size, m_ipaddress.Font.Style);
			}
		}

		// Instance Constructor
		//
		public TunerDeviceHeaderControl(TunerDevice device) : this()
		{
			if(device == null) throw new ArgumentNullException(nameof(device));

			// Just copy the data from the device instance into the appropriate controls
			m_devicename.Text = device.FriendlyName;
			m_modelname.Text = device.ModelNumber;
			m_deviceid.Text = device.DeviceID;
			m_ipaddress.Text = device.LocalIP.ToString();

			// Save the BaseURL for the device for the link target
			m_baseurl = device.BaseURL;
		}

		//-------------------------------------------------------------------
		// Event Handlers
		//-------------------------------------------------------------------

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
		// Event Handlers
		//-------------------------------------------------------------------

		private readonly string m_baseurl;
	}
}
