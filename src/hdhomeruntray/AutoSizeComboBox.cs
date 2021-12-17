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
using System.Reflection;
using System.Windows.Forms;

namespace zuki.hdhomeruntray
{
	//-----------------------------------------------------------------------
	// Class AutoSizeComboBox
	//
	// ComboBox derivative that auto-sizes horizontally based on the drop-down
	// contents; the normal ComboBox only scales vertically
	//
	// Based partially on:
	// https://stackoverflow.com/questions/4448509/how-to-make-the-combobox-drop-down-list-resize-itself-to-fit-the-largest-item

	public class AutoSizeComboBox : ComboBox
	{
		// Instance Constructor
		//
		public AutoSizeComboBox()
		{
		}

		//-------------------------------------------------------------------
		// ComboBox Overrides
		//-------------------------------------------------------------------

		// OnDisplayMemberChanged
		//
		// Invoked when the DisplayMember property has been changed
		protected override void OnDisplayMemberChanged(EventArgs args)
		{
			// Invoke the base version first
			base.OnDisplayMemberChanged(args);

			// Determine what the widest entry in the drop-down will be
			int maxwidth = 0;
			foreach(object item in Items)
			{
				// Get the text that will appear in the control
				string text;
				if(string.IsNullOrEmpty(DisplayMember)) text = item.ToString();
				else
				{
					PropertyInfo pinfo = item.GetType().GetProperty(DisplayMember);
					text = pinfo.GetValue(item, null).ToString();
				}

				// If the text is wider than any previously seen, adjust the maximum
				if(TextRenderer.MeasureText(text, Font).Width > maxwidth)
					maxwidth = TextRenderer.MeasureText(text, Font).Width;
			}

			// Change the control width to accomodate the longest string
			Width = maxwidth + SystemInformation.VerticalScrollBarWidth;
		}
	}
}
