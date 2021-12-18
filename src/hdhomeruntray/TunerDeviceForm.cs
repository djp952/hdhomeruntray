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
	//-----------------------------------------------------------------------
	// Class TunerDeviceForm (internal)
	//
	// Specializes DeviceForm for use with tuner devices

	internal class TunerDeviceForm : DeviceForm
	{
		// Instance Constructor
		//
		public TunerDeviceForm(TunerDevice device, PopupForm form, PopupItemControl item) : base(form, item)
		{
			if(device == null) throw new ArgumentNullException(nameof(device));

			m_layoutpanel.SuspendLayout();

			try
			{
				// Add the header user control for the device
				m_header = new TunerDeviceHeaderControl(device)
				{
					Dock = DockStyle.Top,
					Padding = new Padding(0, 0, 0, 1).ScaleDPI(Handle)
				};
				m_layoutpanel.Controls.Add(m_header);

				// Add the tuner user controls for the device
				foreach(Tuner tuner in device.Tuners)
				{
					TunerDeviceStatusControl status = new TunerDeviceStatusControl(device, tuner)
					{
						Dock = DockStyle.Top,
						Padding = new Padding(0, 1, 0, 1).ScaleDPI(Handle)
					};
					status.DeviceStatusChanged += new DeviceStatusChangedEventHandler(OnDeviceStatusChanged);
					m_layoutpanel.Controls.Add(status);
				}

				// Add the footer user control for the device
				m_footer = new TunerDeviceFooterControl(device)
				{
					Dock = DockStyle.Top,
					Padding = new Padding(0, 1, 0, 0).ScaleDPI(Handle)
				};
				m_layoutpanel.Controls.Add(m_footer);
			}

			finally { m_layoutpanel.ResumeLayout(); }

			// Set the timer for periodic refresh of the tuner status
			m_timer.Interval = 1000;
			m_timer.Tick += new EventHandler(OnTimerTick);
			m_timer.Enabled = true;
		}

		//-------------------------------------------------------------------------
		// Event Handlers
		//-------------------------------------------------------------------------

		// OnDeviceStatusChanged
		//
		// Invoked when the color of the settings "dot" has changed
		private void OnDeviceStatusChanged(object sender, DeviceStatusChangedEventArgs args)
		{
			RaiseDeviceStatusChanged(this, args);
		}

		// OnTimerTick
		//
		// Invoked when the timer comes due
		private void OnTimerTick(object sender, EventArgs args)
		{
			m_layoutpanel.SuspendLayout();

			try
			{
				// Only update the status controls each time the timer elapses
				foreach(Control control in m_layoutpanel.Controls)
				{
					if(control is TunerDeviceStatusControl statuscontrol)
					{
						statuscontrol.UpdateStatus();
					}
				}
			}

			finally { m_layoutpanel.ResumeLayout(); }
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private readonly TunerDeviceHeaderControl m_header;
		private readonly TunerDeviceFooterControl m_footer;
	}
}
