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

using System.Drawing;
using System.Windows.Forms;

using zuki.hdhomeruntray.discovery;

namespace zuki.hdhomeruntray
{
	//-----------------------------------------------------------------------
	// Class PopupItemControl
	//
	// Implements the user control that displays the status of an individual
	// HDHomeRun device in the pop-up status form

	internal partial class PopupItemControl : UserControl
	{
		// Instance Constructor
		//
		private PopupItemControl()
		{
			InitializeComponent();
		}

		// Instance Constructor
		//
		public PopupItemControl(Device device) : this()
		{
			m_device = device;

			// Create the panel control for the device
			var devicepanel = new RoundedFlowLayoutPanel
			{
				AutoSize = true,
				AutoSizeMode = AutoSizeMode.GrowAndShrink,
				FlowDirection = FlowDirection.LeftToRight,
				WrapContents = false,
				BackColor = SystemColors.ControlLightLight,
				Padding = new Padding(8),
				Radius = 16
			};

			// Create the name label for the control
			m_name = new Label
			{
				AutoSize = true,
				Size = new Size(1, 1),
				Text = device.FriendlyName,
				TextAlign = ContentAlignment.BottomCenter,
				Dock = DockStyle.Left,
				Font = VersionHelper.IsWindows11OrGreater() ? new Font("Segoe UI Variable Display Semib", 9F, FontStyle.Regular) : 
					new Font("Segoe UI Semibold", 9F, FontStyle.Regular),
				Visible = true
			};

			// Add the device name label to the device panel
			devicepanel.Controls.Add(m_name);

			// Determine the number of dots to display; for tuners this will be the
			// number of tuners within the device; otherwise just use one dot
			int numdots = 1;
			if(m_device is TunerDevice tunerdevice) numdots = tunerdevice.Tuners.Count;

			// Create the dot labels
			m_dots = new Label[numdots];
			for(int index = 0; index < numdots; index++)
			{
				m_dots[index] = new Label
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

				// Add the dot label to the device panel
				devicepanel.Controls.Add(m_dots[index]);
			}

			// Add the completed panel to the layout panel
			m_layoutpanel.Controls.Add(devicepanel);

			this.Refresh();				// Perform an initial refresh to update the colors
		}

		//-------------------------------------------------------------------------
		// Member Functions
		//-------------------------------------------------------------------------

		// NoDevices
		//
		// Returns a special instance of PopupItemControl that indicates there are
		// no detected HDHomeRun devices
		public static PopupItemControl NoDevices()
		{
			PopupItemControl nodevices = new PopupItemControl();

			// Create the panel control for the static label
			var devicepanel = new RoundedFlowLayoutPanel
			{
				AutoSize = true,
				AutoSizeMode = AutoSizeMode.GrowAndShrink,
				FlowDirection = FlowDirection.LeftToRight,
				WrapContents = false,
				BackColor = SystemColors.ControlLightLight,
				Padding = new Padding(8),
				Radius = 16
			};

			// Create the static label
			var name = new Label
			{
				AutoSize = true,
				Size = new Size(1, 1),
				Text = "No HDHomeRun devices detected",
				TextAlign = ContentAlignment.BottomCenter,
				Dock = DockStyle.Left,
				Font = VersionHelper.IsWindows11OrGreater() ? new Font("Segoe UI Variable Display Semib", 9F, FontStyle.Regular) :
					new Font("Segoe UI Semibold", 9F, FontStyle.Regular),
				Visible = true
			};

			// Add the static label to the panel and the panel to the layout panel
			devicepanel.Controls.Add(name);
			nodevices.m_layoutpanel.Controls.Add(devicepanel);

			return nodevices;
		}

		//-------------------------------------------------------------------------
		// Control Overrides
		//-------------------------------------------------------------------------

		// Refresh
		//
		// Overrides Control::Refresh
		public override void Refresh()
		{
			// No device; this is a static "NoDevices" instance
			if(m_device == null) return;

			// TunerDevice
			//
			if(m_device is TunerDevice tunerdevice)
			{
				for(int index = 0; index < tunerdevice.Tuners.Count; index++)
				{
					TunerStatus status = tunerdevice.GetTunerStatus(index);
					m_dots[index].ForeColor = status.SignalQualityColor;
				}
			}

			// StorageDevice
			//
			else if(m_device is StorageDevice storagedevice)
			{
				// TODO: Get the color from "StorageStatus" when it's available;
				// green for live TV, red for recording, gray if idle
				if(storagedevice.Recordings.Count > 0) m_dots[0].ForeColor = Color.FromArgb(0xE50000);
				else m_dots[0].ForeColor = Color.FromArgb(0xC0C0C0);
			}

			base.Refresh();
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private readonly Device		m_device = null;	// Referenced device object
		private readonly Label		m_name;				// Device name label
		private readonly Label[]	m_dots;				// Status dot labels
	}
}
