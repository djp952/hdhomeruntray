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
	// Class TunerDeviceFooterControl (internal)
	//
	// User control that implements the footer for a tuner device in the DeviceForm

	partial class TunerDeviceFooterControl : UserControl
	{
		// Instance Constructor
		//
		private TunerDeviceFooterControl()
		{
			InitializeComponent();

			this.Padding = this.Padding.ScaleDPI(this.Handle);
			m_layoutpanel.Margin = m_layoutpanel.Margin.ScaleDPI(this.Handle);
			m_layoutpanel.Padding = m_layoutpanel.Padding.ScaleDPI(this.Handle);

			// WINDOWS 11
			//
			if(VersionHelper.IsWindows11OrGreater())
			{
				this.m_firmwareversion.Font = new Font("Segoe UI Variable Small", this.m_firmwareversion.Font.Size, this.m_firmwareversion.Font.Style);
				this.m_unused.Font = new Font("Segoe UI Variable Small", this.m_unused.Font.Size, this.m_unused.Font.Style);
			}
		}

		// Instance Constructor
		//
		public TunerDeviceFooterControl(TunerDevice device) : this()
		{
			if(device == null) throw new ArgumentNullException(nameof(device));

			// Just copy the data from the device instance into the appropriate controls
			m_firmwareversion.Text = device.FirmwareVersion;
			m_unused.Text = String.Empty;
		}
	}
}
