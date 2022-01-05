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

using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace zuki.hdhomeruntray
{
	//--------------------------------------------------------------------------
	// Class SettingsFormFooterControl (internal)
	//
	// User control that implements the footer for the settngs form

	internal partial class SettingsFormFooterControl : UserControl
	{
		// Instance Constructor
		//
		public SettingsFormFooterControl()
		{
			InitializeComponent();

			m_layoutpanel.EnableDoubleBuferring();

			using(Graphics graphics = CreateGraphics())
			{
				Padding = Padding.ScaleDPI(graphics);
				m_layoutpanel.Margin = m_layoutpanel.Margin.ScaleDPI(graphics);
				m_layoutpanel.Padding = m_layoutpanel.Padding.ScaleDPI(graphics);
				m_layoutpanel.Radii = m_layoutpanel.Radii.ScaleDPI(graphics);
			}

			// WINDOWS 11
			//
			if(VersionHelper.IsWindows11OrGreater())
			{
				m_link1.Font = new Font("Segoe UI Variable Text", m_link1.Font.Size, m_link1.Font.Style);
				m_link2.Font = new Font("Segoe UI Variable Text", m_link1.Font.Size, m_link1.Font.Style);
			}

			// Set the link label text based on whatever I ultimately decide they should do
			m_link1.Text = "";
			m_link2.Text = "LICENSE";
		}

		//-------------------------------------------------------------------
		// Event Handlers
		//-------------------------------------------------------------------

		// OnLink1Clicked
		//
		// Invoked when "link 1" has been clicked
		private void OnLink1Clicked(object sender, LinkLabelLinkClickedEventArgs args)
		{
		}

		// OnLink2Clicked
		//
		// Invoked when "link 2" has been clicked
		private void OnLink2Clicked(object sender, LinkLabelLinkClickedEventArgs args)
		{
			// Link 2 current points to the project license
			using(Process process = new Process())
			{
				process.StartInfo.FileName = Properties.Resources.LicenseURL;
				process.StartInfo.UseShellExecute = true;
				process.StartInfo.Verb = "open";
				process.Start();
			}
		}
	}
}
