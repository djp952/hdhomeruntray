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
	//--------------------------------------------------------------------------
	// Class PopupItemGlyphControl
	//
	// Implements a SymbolGlyph popup item control

	class PopupItemGlyphControl : PopupItemControl
	{
		// Instance Constructor
		//
		public PopupItemGlyphControl(SymbolGlyph glyph) : this(glyph, PopupItemControlType.Button)
		{
		}

		// Instance Constructor
		//
		public PopupItemGlyphControl(SymbolGlyph glyph, PopupItemControlType type) : base(type)
		{
			// Create the label control for the symbol
			var label = new PassthroughLabelControl
			{
				AutoSize = true,
				Size = new Size(1, 1),
				Text = new string((char)glyph, 1),
				TextAlign = ContentAlignment.BottomCenter,
				Dock = DockStyle.Left,
				Font = new Font("Symbols", 11.25F, FontStyle.Regular),
				Visible = true
			};

			// Windows 11 - Change glyph typeface to Segoe Fluent Icons
			//
			if(VersionHelper.IsWindows11OrGreater())
				label.Font = new Font("Segoe Fluent Icons", label.Font.Size, FontStyle.Regular);

			// Windows 10 - Change glyph typeface to Segoe MDL2 Assets
			//
			else if(VersionHelper.IsWindows10OrGreater())
				label.Font = new Font("Segoe MDL2 Assets", label.Font.Size, FontStyle.Regular);

			// Add the glyph label to the layout panel
			base.LayoutPanel.Controls.Add(label);
		}
	}
}
