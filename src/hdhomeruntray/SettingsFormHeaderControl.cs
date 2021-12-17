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

using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace zuki.hdhomeruntray
{
	//--------------------------------------------------------------------------
	// Class SettingsFormHeaderControl (internal)
	//
	// User control that implements the header for the settngs form

	internal partial class SettingsFormHeaderControl : UserControl
	{
		// Instance Constructor
		//
		public SettingsFormHeaderControl()
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
				m_appname.Font = new Font("Segoe UI Variable Display SemiB", m_appname.Font.Size, m_appname.Font.Style);
				m_version.Font = new Font("Segoe UI Variable Small", m_version.Font.Size, m_version.Font.Style);
			}

			// Use the applicaton icon to generate an appropriately sized image
			Icon icon = new Icon(Properties.Resources.ApplicationIcon, SystemInformation.IconSize);
			if(icon != null) m_icon.Image = icon.ToBitmap();

			// Get the information for the header from the file version information
			FileVersionInfo fileverinfo = FileVersionInfo.GetVersionInfo(typeof(SettingsFormHeaderControl).Assembly.Location);
			m_appname.Text = fileverinfo.ProductName;
			m_version.Text = fileverinfo.FileVersion;
		}
	}
}
