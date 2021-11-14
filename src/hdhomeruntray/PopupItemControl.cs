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
	//
	// TODO: Colors need to go into a ColorManager class (themes / OS differences)
	// TODO: Fonts need to go into a FontManager class (themes / OS differences)
	// TODO: "DOT" needs to be a constant somewhere, if Segoe UI Symbol isn't available
	// this character will change

	internal partial class PopupItemControl : UserControl
	{
		// Instance Constructor
		//
		public PopupItemControl()
		{
			InitializeComponent();
		}

		// Instance Constructor
		//
		public PopupItemControl(TunerDevice device) : this()
		{
			var name = new Label
			{
				AutoSize = true,
				Size = new Size(1, 1),
				Text = device.FriendlyName,
				TextAlign = ContentAlignment.MiddleCenter,
				Dock = DockStyle.Left,
				Font = new Font("Segoe UI Semibold", 9F, FontStyle.Regular),
				Visible = true
			};
			m_layoutpanel.Controls.Add(name);

			foreach(Tuner tuner in device.Tuners)
			{
				var dot = new Label
				{
					AutoSize = true,
					Size = new Size(1, 1),
					//dot.Text = "●";	// Segoe UI
					//dot.Text = "l";	// Wingdings
					Text = "●", // Segoe UI Symbol  U+25CF;
					TextAlign = ContentAlignment.BottomCenter,
					Dock = DockStyle.Left,
					Font = new Font("Segoe UI Symbol", 9F, FontStyle.Bold)
				};

				if(tuner.IsActive) dot.ForeColor = Color.FromArgb(0x1EE500);       // TODO: constant
				else dot.ForeColor = Color.FromArgb(0xC0C0C0);						// TODO: constant

				dot.Visible = true;
				m_layoutpanel.Controls.Add(dot);
			}
		}

		// Instance Constructor
		//
		public PopupItemControl(StorageDevice storage) : this()
		{
			var name = new Label
			{
				AutoSize = true,
				Size = new Size(1, 1),
				Text = storage.FriendlyName,
				TextAlign = ContentAlignment.MiddleCenter,
				Dock = DockStyle.Left,
				Font = new Font("Segoe UI Semibold", 9F, FontStyle.Regular),
				Visible = true
			};
			m_layoutpanel.Controls.Add(name);

			var dot = new Label
			{
				AutoSize = true,
				Size = new Size(1, 1),
				//dot.Text = "●";	// Segoe UI
				//dot.Text = "l";	// Wingdings
				Text = "●", // Segoe UI Symbol  (U+25CF);
				TextAlign = ContentAlignment.BottomCenter,
				Dock = DockStyle.Left,
				Font = new Font("Segoe UI Symbol", 9F, FontStyle.Bold)
			};

			if(storage.Recordings.Count > 0) dot.ForeColor = Color.FromArgb(0xE50000);
			else dot.ForeColor = Color.FromArgb(0xC0C0C0);

			dot.Visible = true;
			m_layoutpanel.Controls.Add(dot);
		}

		public static PopupItemControl NoDevices()
		{
			PopupItemControl nodevices = new PopupItemControl();
			var name = new Label
			{
				AutoSize = true,
				Size = new Size(1, 1),
				Text = "No HDHomeRun devices detected",
				TextAlign = ContentAlignment.MiddleCenter,
				Dock = DockStyle.Left,
				Font = new Font("Segoe UI Semibold", nodevices.Font.Size, FontStyle.Regular),
				Visible = true
			};
			nodevices.m_layoutpanel.Controls.Add(name);

			return nodevices;
		}
	}
}
