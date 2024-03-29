﻿//---------------------------------------------------------------------------
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
using System.Windows.Forms;

using zuki.hdhomeruntray.discovery;

namespace zuki.hdhomeruntray
{
	//--------------------------------------------------------------------------
	// Class StorageDeviceFooterControl (internal)
	//
	// User control that implements the footer for a storage device in the DeviceForm

	internal partial class StorageDeviceFooterControl : UserControl
	{
		// Instance Constructor
		//
		private StorageDeviceFooterControl(SizeF scalefactor)
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
				m_version.Font = new Font("Segoe UI Variable Small", m_version.Font.Size, m_version.Font.Style);
				m_space.Font = new Font("Segoe UI Variable Small", m_space.Font.Size, m_space.Font.Style);
			}
		}

		// Instance Constructor
		//
		public StorageDeviceFooterControl(StorageDevice device, SizeF scalefactor) : this(scalefactor)
		{
			if(device == null) throw new ArgumentNullException(nameof(device));

			// Just copy the data from the device instance into the appropriate controls
			m_version.Text = "Version " + device.Version;
			m_space.Text = (device.FreeSpace > 0) ? FormatDiskSpace(device.FreeSpace) : String.Empty;
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
		}

		//-------------------------------------------------------------------
		// Member Functions
		//-------------------------------------------------------------------

		// UpdateStatus
		//
		// Updates the storage status data displayed in the footer
		public void UpdateStatus(StorageStatus status)
		{
			if(status == null) throw new ArgumentNullException("status");

			m_space.Text = (status.FreeSpace > 0) ? FormatDiskSpace(status.FreeSpace) : String.Empty;
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
			const double MB = KB * KB;          // Megabytes
			const double GB = MB * KB;          // Gigabytes
			const double TB = GB * KB;          // Terabytes

			double value = space;

			if(value >= TB) return string.Format("{0:N2} TB", value / TB);
			else if(value >= GB) return string.Format("{0:N2} GB", value / GB);
			else if(value >= MB) return string.Format("{0:N2} MB", value / MB);
			else if(value >= KB) return string.Format("{0:N2} KB", value / KB);

			return space.ToString();
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private readonly EventHandler m_appthemechanged;
	}
}
