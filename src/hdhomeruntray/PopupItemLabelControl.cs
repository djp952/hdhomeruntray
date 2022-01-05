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
using System.Drawing;
using System.Windows.Forms;

namespace zuki.hdhomeruntray
{
	//--------------------------------------------------------------------------
	// Class PopupItemLabelControl
	//
	// Implements a static label popup item control

	internal class PopupItemLabelControl : PopupItemControl
	{
		// Instance Constructor
		//
		public PopupItemLabelControl(string text, SizeF scalefactor) : base(PopupItemControlType.Static, scalefactor)
		{
			if(text == null) throw new ArgumentNullException(nameof(text));

			// Create the label control for the text
			PassthroughLabelControl label = new PassthroughLabelControl
			{
				AutoSize = true,
				Size = new Size(1, 1),
				Text = text,
				TextAlign = ContentAlignment.BottomCenter,
				Dock = DockStyle.Left,
				Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
				Visible = true
			};

			// Windows 11 - Change label typeface to Segoe UI Variable Display Semib
			//
			if(VersionHelper.IsWindows11OrGreater())
				label.Font = new Font("Segoe UI Variable Display Semib", label.Font.Size, label.Font.Style);

			// Add the label to the layout panel
			base.LayoutPanel.Controls.Add(label);
		}
	}
}
