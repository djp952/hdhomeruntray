//---------------------------------------------------------------------------
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
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using zuki.hdhomeruntray.discovery;

namespace zuki.hdhomeruntray
{
	//--------------------------------------------------------------------------
	// Class TunerDeviceToolsControl (internal)
	//
	// User control that implements the header for a tuner device in the DeviceForm

	internal partial class TunerDeviceToolsControl : UserControl
	{
		// Instance Constructor
		//
		private TunerDeviceToolsControl(SizeF scalefactor)
		{
			InitializeComponent();

			m_layoutpanel.EnableDoubleBuffering();

			Padding = Padding.ScaleDPI(scalefactor);
			m_layoutpanel.Margin = m_layoutpanel.Margin.ScaleDPI(scalefactor);
			m_layoutpanel.Padding = m_layoutpanel.Padding.ScaleDPI(scalefactor);
		}

		// Instance Constructor
		//
		public TunerDeviceToolsControl(TunerDevice device, SizeF scalefactor) : this(scalefactor)
		{
			m_device = device ?? throw new ArgumentNullException(nameof(device));

			int numtuners = device.Tuners.Count;
			float columnpercent = 100.0F / (numtuners + 1);		// + "Restart"

			m_layoutpanel.ColumnCount = (numtuners + 1);        // + "Restart"

			// The layout panel has one column at design time, adjust that to be percentage based
			// and add enough columns to support each tuner instance
			m_layoutpanel.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, columnpercent);
			for(int index = 0; index < (numtuners - 1); index++)
				m_layoutpanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, columnpercent));

			// The "Reboot" column will be set to AutoSize to take up any remaining space cleanly
			m_layoutpanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

			// Add the "Stop" buttons for each tuner
			for(int index = 0; index < numtuners; index++)
			{
				var stopbutton = new TunerDeviceToolsControlLabelButton(String.Format("Stop Tuner {0}", index), scalefactor)
				{
					Tag = m_device.Tuners[index]
				};
				stopbutton.Selected += new EventHandler(OnStopTunerSelected);
				m_layoutpanel.Controls.Add(stopbutton, index, 0);
			}

			// Add the "Restart" button
			var restartbutton = new TunerDeviceToolsControlLabelButton("Restart Device", scalefactor);
			restartbutton.Selected += new EventHandler(OnRestartSelected);
			m_layoutpanel.Controls.Add(restartbutton, numtuners, 0);
		}

		//-------------------------------------------------------------------
		// Event Handlers
		//-------------------------------------------------------------------

		// OnRestartSelected
		//
		// Invoked when the "Restart Device" button has been selected
		private void OnRestartSelected(object sender, EventArgs args)
		{
			try { m_device.Restart(); }
			catch { /* DON'T CARE */ }
		}

		// OnStopTunerSelected
		//
		// Invoked when a "Stop Tuner" button has been selected
		private void OnStopTunerSelected(object sender, EventArgs args)
		{
			if((sender is Control control) && (control.Tag is Tuner tuner)) m_device.StopTuner(tuner.Index);
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private readonly TunerDevice m_device;
	}
}
