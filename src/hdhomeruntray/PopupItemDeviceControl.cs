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
using zuki.hdhomeruntray.Properties;

namespace zuki.hdhomeruntray
{
	//--------------------------------------------------------------------------
	// Class PopupItemDeviceControl
	//
	// Implements a Device popup item control

	internal class PopupItemDeviceControl : PopupItemControl
	{
		// Instance Constructor
		//
		public PopupItemDeviceControl(Device device) : base(PopupItemControlType.Toggle)
		{
			m_device = device ?? throw new ArgumentNullException(nameof(device));

			// Create the name label for the control
			PassthroughLabelControl name = new PassthroughLabelControl
			{
				AutoSize = true,
				Size = new Size(1, 1),
				Text = m_device.FriendlyName,
				TextAlign = ContentAlignment.BottomCenter,
				Dock = DockStyle.Left,
				Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
				Visible = true
			};

			// Windows 11 - Change label typeface to Segoe UI Variable Display Semib
			//
			if(VersionHelper.IsWindows11OrGreater())
				name.Font = new Font("Segoe UI Variable Display Semib", name.Font.Size, name.Font.Style);

			// Add the device name label to the layout panel
			base.LayoutPanel.Controls.Add(name);

			// Determine the number of dots to display; for tuners this will be the
			// number of tuners within the device; otherwise just use one dot
			int numdots = 1;
			if(m_device is TunerDevice tunerdevice) numdots = tunerdevice.Tuners.Count;

			// Create the dot labels
			m_dots = new Label[numdots];
			for(int index = 0; index < numdots; index++)
			{
				m_dots[index] = new PassthroughLabelControl
				{
					AutoSize = true,
					Size = new Size(1, 1),
					Text = "●",                                 // U+25CF
					TextAlign = ContentAlignment.BottomCenter,
					ForeColor = SystemColors.GrayText,
					Dock = DockStyle.Left,
					Font = new Font("Segoe UI Symbol", 9F, FontStyle.Regular),
					Visible = true
				};

				// Add the dot label to the layout panel
				base.LayoutPanel.Controls.Add(m_dots[index]);
			}

			Refresh();                 // Perform the initial refresh
		}

		//-------------------------------------------------------------------------
		// Properties
		//-------------------------------------------------------------------------

		// Device
		//
		// Gets a reference to the device instance associated with this item
		public Device Device => m_device;

		// DeviceStatus
		//
		// Gets the overall status of the device
		public DeviceStatus DeviceStatus => m_status;

		//-------------------------------------------------------------------------
		// Control Overrides
		//-------------------------------------------------------------------------

		// Refresh
		//
		// Overrides Control::Refresh
		public override void Refresh()
		{
			// Default overall status to Idle
			m_status = DeviceStatus.Idle;

			// No device; this is a static or glyph instance
			if(m_device == null) return;

			// TunerDevice
			//
			if(m_device is TunerDevice tunerdevice)
			{
				for(int index = 0; index < tunerdevice.Tuners.Count; index++)
				{
					// Get the granular tuner status from the device
					TunerStatus status = tunerdevice.GetTunerStatus(index);

					// Update the overall device status indicator
					if(status.DeviceStatus > m_status) m_status = status.DeviceStatus;

					// Default to a standard color, but obey the setting
					Color forecolor = DeviceStatusColor.FromDeviceStatus(status.DeviceStatus);
					switch(Settings.Default.TunerStatusColorSource)
					{
						case TunerStatusColorSource.SignalStrength:
							forecolor = status.SignalStrengthColor;
							break;

						case TunerStatusColorSource.SignalQuality:
							forecolor = status.SignalQualityColor;
							break;

						case TunerStatusColorSource.SymbolQuality:
							forecolor = status.SymbolQualityColor;
							break;
					}

					m_dots[index].ForeColor = forecolor;
				}
			}

			// StorageDevice
			//
			else if(m_device is StorageDevice storagedevice)
			{
				// Get the granular storage status from the device
				StorageStatus status = storagedevice.GetStorageStatus();

				// Update the overall device status indicator
				if(status.DeviceStatus > m_status) m_status = status.DeviceStatus;

				// The storage device only gets one dot for the overall status
				m_dots[0].ForeColor = DeviceStatusColor.FromDeviceStatus(status.DeviceStatus);
			}

			base.Refresh();
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private readonly Device m_device = null;
		private readonly Label[] m_dots;
		private DeviceStatus m_status = DeviceStatus.Idle;
	}
}
