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

			// Add the "Reset" buttons for each tuner
			for(int index = 0; index < numtuners; index++)
			{
				var resetbutton = new TunerDeviceToolsControlLabelButton(String.Format("Reset Tuner {0}", index), scalefactor)
				{
					Tag = m_device.Tuners[index]
				};
				resetbutton.Selected += new EventHandler(OnResetTunerSelected);
				m_layoutpanel.Controls.Add(resetbutton, index, 0);
			}

			// Add the "Reboot" button
			var rebootbutton = new TunerDeviceToolsControlLabelButton("Reboot", scalefactor);
			rebootbutton.Selected += new EventHandler(OnRebootSelected);
			m_layoutpanel.Controls.Add(rebootbutton, numtuners, 0);
		}

		//-------------------------------------------------------------------
		// Event Handlers
		//-------------------------------------------------------------------

		// OnRebootSelected
		//
		// Invoked when the "Reboot" button has been selected
		private void OnRebootSelected(object sender, EventArgs args)
		{
			try { m_device.Reboot(); }
			catch { /* DON'T CARE */ }
		}

		// OnResetTunerSelected
		//
		// Invoked when a "Reset Tuner" button has been selected
		private void OnResetTunerSelected(object sender, EventArgs args)
		{
			if((sender is Control control) && (control.Tag is Tuner tuner)) m_device.ResetTuner(tuner.Index);
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private readonly TunerDevice m_device;
	}
}
