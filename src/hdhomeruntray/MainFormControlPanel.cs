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

namespace zuki.hdhomeruntray
{
	//-----------------------------------------------------------------------
	// Class MainFormControlPanel
	//
	// Implements the user control the provides the 'control panel' at the 
	// bottom of the main form

	public partial class MainFormControlPanel : UserControl
	{
		// Instance Constructor
		//
		public MainFormControlPanel()
		{
			InitializeComponent();

			// Windows 11 - Change glyph typeface to Segoe Fluent Icons
			//
			if(VersionHelper.IsWindows11OrGreater())
				m_pinunpin.Font = m_devicelist.Font = m_options.Font = new Font("Segoe Fluent Icons", m_pinunpin.Font.Size, FontStyle.Bold);

			// Windows 10 - Change glyph typeface to Segoe MDL2 Assets
			//
			else if(VersionHelper.IsWindows10OrGreater())
				m_pinunpin.Font = m_devicelist.Font = m_options.Font = new Font("Segoe MDL2 Assets", m_pinunpin.Font.Size, FontStyle.Bold);
		}
	}
}
