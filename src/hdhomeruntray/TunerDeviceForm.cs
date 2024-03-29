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
		public TunerDeviceForm(TunerDevice device, PopupForm form, PopupItemControl item, DockStyle dockstyle) : base(form, item, dockstyle)
		{
			m_device = device ?? throw new ArgumentNullException(nameof(device));

			m_layoutpanel.SuspendLayout();

			try
			{
				// Add the header user control for the device
				m_header = new TunerDeviceHeaderControl(m_device, m_scalefactor)
				{
					Dock = DockStyle.Top,
					Padding = new Padding(0, 0, 0, 1).ScaleDPI(m_scalefactor)
				};
				m_layoutpanel.Controls.Add(m_header);

				// Add the tuner user controls for the device
				foreach(Tuner tuner in m_device.Tuners)
				{
					TunerDeviceStatusControl status = new TunerDeviceStatusControl(m_device, tuner, m_scalefactor)
					{
						Dock = DockStyle.Top,
						Padding = new Padding(0, 1, 0, 1).ScaleDPI(m_scalefactor)
					};
					status.DeviceStatusChanged += new DeviceStatusChangedEventHandler(OnDeviceStatusChanged);
					m_layoutpanel.Controls.Add(status);
				}

				// Add the footer user control for the device
				m_footer = new TunerDeviceFooterControl(m_device, m_scalefactor)
				{
					Dock = DockStyle.Top,
					Padding = new Padding(0, 1, 0, 0).ScaleDPI(m_scalefactor)
				};
				m_footer.ShowToolsToggled += new ToggledEventHandler(OnShowToolsToggled);
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

		// OnShowToolsToggled
		//
		// Invoked when the show tools option has been toggled
		private void OnShowToolsToggled(object sender, ToggledEventArgs args)
		{
			if(args.Toggled == false)
			{
				// When turned off, cycle through and remove any TunerDeviceToolsControl objects
				foreach(Control control in m_layoutpanel.Controls)
				{
					if(control is TunerDeviceToolsControl) m_layoutpanel.Controls.Remove(control);
				}
			}
			else
			{
				// When turned on, insert a TunerDeviceToolsControl above the footer
				TunerDeviceToolsControl toolscontrol = new TunerDeviceToolsControl(m_device, m_scalefactor)
				{
					Dock = DockStyle.Top,
					Padding = new Padding(0, 1, 0, 1).ScaleDPI(m_scalefactor)
				};

				// Insert the control above the footer
				m_layoutpanel.Controls.Add(toolscontrol);
				m_layoutpanel.Controls.SetChildIndex(toolscontrol, m_layoutpanel.Controls.GetChildIndex(m_footer));
			}
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

		private readonly TunerDevice m_device;
		private readonly TunerDeviceHeaderControl m_header;
		private readonly TunerDeviceFooterControl m_footer;
	}
}
