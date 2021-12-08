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
	// Class StorageDeviceRecordingControl (internal)
	//
	// User control that implements the status for a Live Buffer

	partial class StorageDeviceRecordingControl : UserControl
	{
		// Instance Constructor
		//
		private StorageDeviceRecordingControl()
		{
			InitializeComponent();

			m_layoutpanel.EnableDoubleBuferring();

			Padding = Padding.ScaleDPI(Handle);
			m_layoutpanel.Margin = m_layoutpanel.Margin.ScaleDPI(Handle);
			m_layoutpanel.Padding = m_layoutpanel.Padding.ScaleDPI(Handle);

			// WINDOWS 11
			//
			if(VersionHelper.IsWindows11OrGreater())
			{
				m_recordinglabel.Font = new Font("Segoe UI Variable Text Semibold", m_recordinglabel.Font.Size, m_recordinglabel.Font.Style);
				m_name.Font = new Font("Segoe UI Variable Text", m_name.Font.Size, m_name.Font.Style);
			}
		}

		// Instance Constructor
		//
		public StorageDeviceRecordingControl(Recording recording) : this()
		{
			if(recording == null) throw new ArgumentNullException(nameof(recording));

			// This is static information, just assign from the recording instance
			m_activedot.ForeColor = DeviceStatusColor.Red;
			m_name.Text = recording.Name;
		}
	}
}
