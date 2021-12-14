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
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using zuki.hdhomeruntray.discovery;

namespace zuki.hdhomeruntray
{
	//-----------------------------------------------------------------------
	// Class StorageDeviceForm (internal)
	//
	// Specializes DeviceForm for use with storage devices

	class StorageDeviceForm : DeviceForm
	{
		// Instance Constructor
		//
		public StorageDeviceForm(StorageDevice device, PopupForm form, PopupItemControl item) : base(form, item)
		{
			m_device = device ?? throw new ArgumentNullException(nameof(device));

			m_layoutpanel.SuspendLayout();

			try
			{
				// Add the header user control for the device
				m_header = new StorageDeviceHeaderControl(device)
				{
					Dock = DockStyle.Top,
					Padding = new Padding(0, 0, 0, 1).ScaleDPI(Handle)
				};
				m_layoutpanel.Controls.Add(m_header);

				// Add the footer user control for the device
				m_footer = new StorageDeviceFooterControl(device)
				{
					Dock = DockStyle.Top,
					Padding = new Padding(0, 1, 0, 0).ScaleDPI(Handle)
				};
				m_layoutpanel.Controls.Add(m_footer);

				// Manually invoke the timer callback to populate the live
				// buffers and recordings that are active on the device
				OnTimerTick(this, EventArgs.Empty);
			}

			finally { m_layoutpanel.ResumeLayout(); }

			// Set the timer for periodic refresh of the storage status
			m_timer.Interval = 2000;
			m_timer.Tick += new EventHandler(OnTimerTick);
			m_timer.Enabled = true; 
		}

		// OnTimerTick
		//
		// Invoked when the timer comes due
		private void OnTimerTick(object sender, EventArgs args)
		{
			m_layoutpanel.SuspendLayout();

			try
			{
				// Get updated status for the device and check against the last known
				// hash, this collection changes relatively infrequently compared to tuners
				StorageStatus status = m_device.GetStorageStatus();
				if(status.GetHashCode() == m_lasthash) return;

				// Save the updated hash code
				m_lasthash = status.GetHashCode();

				// Check all of the active items in the layout panel to make sure they are still valid
				List<Control> toremove = new List<Control>();
				foreach(Control control in m_layoutpanel.Controls)
				{
					if((control is StorageDeviceLiveBufferControl) && 
						(!status.LiveBuffers.Any(livebuffer => livebuffer.GetHashCode() == (int)control.Tag))) toremove.Add(control);

					else if((control is StorageDeviceRecordingControl) &&
						(!status.Recordings.Any(recording => recording.GetHashCode() == (int)control.Tag))) toremove.Add(control);
				}

				foreach(Control control in toremove) m_layoutpanel.Controls.Remove(control);

				// Cast the layout panel controls collection as IEnumerable<T>
				IEnumerable<Control> controls = m_layoutpanel.Controls.Cast<Control>();

				// Add LiveBuffer items for each one that isn't already represented
				foreach(LiveBuffer livebuffer in status.LiveBuffers)
				{
					if(!controls.Any(control => (control is StorageDeviceLiveBufferControl) && ((int)control.Tag == livebuffer.GetHashCode())))
					{
						StorageDeviceLiveBufferControl livebuffercontrol = new StorageDeviceLiveBufferControl(livebuffer)
						{
							Dock = DockStyle.Top,
							Tag = livebuffer.GetHashCode(),
							Padding = new Padding(0, 1, 0, 1).ScaleDPI(Handle)
						};

						// Insert LiveBuffer items after the header
						m_layoutpanel.Controls.Add(livebuffercontrol);
						m_layoutpanel.Controls.SetChildIndex(livebuffercontrol, m_layoutpanel.Controls.GetChildIndex(m_header) + 1);
					}
				}

				// Add Playback items for each one that isn't already represented
				foreach(Playback playback in status.Playbacks)
				{
					if(!controls.Any(control => (control is StorageDevicePlaybackControl) && ((int)control.Tag == playback.GetHashCode())))
					{
						StorageDevicePlaybackControl playbackcontrol = new StorageDevicePlaybackControl(playback)
						{
							Dock = DockStyle.Top,
							Tag = playback.GetHashCode(),
							Padding = new Padding(0, 1, 0, 1).ScaleDPI(Handle)
						};

						// Insert LiveBuffer items after the header
						m_layoutpanel.Controls.Add(playbackcontrol);
						m_layoutpanel.Controls.SetChildIndex(playbackcontrol, m_layoutpanel.Controls.GetChildIndex(m_header) + 1);
					}
				}

				// Add Recording items for each one that isn't already represented
				foreach(Recording recording in status.Recordings)
				{
					if(!controls.Any(control => (control is StorageDeviceRecordingControl) && ((int)control.Tag == recording.GetHashCode())))
					{
						StorageDeviceRecordingControl recordingcontrol = new StorageDeviceRecordingControl(recording)
						{
							Dock = DockStyle.Top,
							Tag = recording.GetHashCode(),
							Padding = new Padding(0, 1, 0, 1).ScaleDPI(Handle)
						};

						// Insert Recording items above the footer
						m_layoutpanel.Controls.Add(recordingcontrol);
						m_layoutpanel.Controls.SetChildIndex(recordingcontrol, m_layoutpanel.Controls.GetChildIndex(m_footer));
					}
				}
			}

			finally { m_layoutpanel.ResumeLayout(); }
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private readonly StorageDevice m_device;
		private readonly StorageDeviceHeaderControl m_header;
		private readonly StorageDeviceFooterControl m_footer;
		private int m_lasthash = 0;
	}
}
