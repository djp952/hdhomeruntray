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

			m_layoutpanel.EnableDoubleBuferring();

			Padding = Padding.ScaleDPI(Handle);
			m_layoutpanel.Padding = m_layoutpanel.Padding.ScaleDPI(Handle);
			m_layoutpanel.Radii = m_layoutpanel.Radii.ScaleDPI(Handle);

			// WINDOWS 11
			//
			if(VersionHelper.IsWindows11OrGreater())
			{
				m_version.Font = new Font("Segoe UI Variable Small", m_version.Font.Size, m_version.Font.Style);
				m_space.Font = new Font("Segoe UI Variable Small", m_space.Font.Size, m_space.Font.Style);
			}
		}

		// Instance Constructor
		//
		public StorageDeviceFooterControl(StorageDevice device) : this()
		{
			if(device == null) throw new ArgumentNullException(nameof(device));

			// Just copy the data from the device instance into the appropriate controls
			m_version.Text = "Version " + device.Version;
			m_space.Text = FormatDiskSpace(device.FreeSpace);
		}

		//-------------------------------------------------------------------------
		// Private Member Functions
		//-------------------------------------------------------------------------

		// FormatDiskSpace
		//
		// Formats a disk space number
		private static string FormatDiskSpace(long space)
		{
			const double KB = 1024;             // Kilobytes
			const double MB = KB * KB;			// Megabytes
			const double GB = MB * KB;			// Gigabytes
			const double TB = GB * KB;			// Terabytes

			double value = space;

			if(value >= TB) return String.Format("{0:N2} TB", value / TB);
			else if(value >= GB) return String.Format("{0:N2} GB", value / GB);
			else if(value >= MB) return String.Format("{0:N2} MB", value / MB);
			else if(value >= KB) return String.Format("{0:N2} KB", value / KB);

			return space.ToString();
		}
	}
}
