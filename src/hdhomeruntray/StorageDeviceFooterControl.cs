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
using System.Windows.Forms;

using zuki.hdhomeruntray.discovery;

namespace zuki.hdhomeruntray
{
	//--------------------------------------------------------------------------
	// Class StorageDeviceFooterControl (internal)
	//
	// User control that implements the footer for a storage device in the DeviceForm

	partial class StorageDeviceFooterControl : UserControl
	{
		// Instance Constructor
		//
		private StorageDeviceFooterControl()
		{
			InitializeComponent();

			this.Padding = this.Padding.ScaleDPI(this.Handle);
			m_layoutPanel.Padding = m_layoutPanel.Padding.ScaleDPI(this.Handle);
		}

		// Instance Constructor
		//
		public StorageDeviceFooterControl(StorageDevice device) : this()
		{
			if(device == null) throw new ArgumentNullException(nameof(device));

			// Just copy the data from the device instance into the appropriate controls
			m_version.Text = "Version: ";                   // TODO: doesn't exist yet
			m_space.Text = "";								// TODO: doesn't exist yet
		}
	}
}
