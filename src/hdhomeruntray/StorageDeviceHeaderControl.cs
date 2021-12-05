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
using System.Windows.Forms;

using zuki.hdhomeruntray.discovery;

namespace zuki.hdhomeruntray
{
	//--------------------------------------------------------------------------
	// Class StorageDeviceHeaderControl (internal)
	//
	// User control that implements the header for a storage device in the DeviceForm

	partial class StorageDeviceHeaderControl : UserControl
	{
		// Instance Constructor
		//
		private StorageDeviceHeaderControl()
		{
			InitializeComponent();

			this.Padding = this.Padding.ScaleDPI(this.Handle);
			m_layoutPanel.Padding = m_layoutPanel.Padding.ScaleDPI(this.Handle);

			// WINDOWS 11
			//
			if(VersionHelper.IsWindows11OrGreater())
			{
				this.m_devicename.Font = new Font("Segoe UI Variable Display SemiB", this.m_devicename.Font.Size, this.m_devicename.Font.Style);
				this.m_storageid.Font = new Font("Segoe UI Variable Small", this.m_storageid.Font.Size, this.m_storageid.Font.Style);
				this.m_ipaddress.Font = new Font("Segoe UI Variable Small", this.m_ipaddress.Font.Size, this.m_ipaddress.Font.Style);
			}
		}

		// Instance Constructor
		//
		public StorageDeviceHeaderControl(StorageDevice device) : this()
		{
			if(device == null) throw new ArgumentNullException(nameof(device));

			// Just copy the data from the device instance into the appropriate controls
			m_devicename.Text = device.FriendlyName;
			m_storageid.Text = device.StorageID;
			m_ipaddress.Text = device.LocalIP.ToString();
		}
	}
}
