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

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace zuki.hdhomeruntray
{
	//-----------------------------------------------------------------------
	// Class RoundedTableLayoutPanel (internal)
	//
	// Customization of the TableLayoutPanel control to provide rounded corners

	internal class RoundedTableLayoutPanel : TableLayoutPanel
	{
		// Instance Constructor
		//
		public RoundedTableLayoutPanel() : base()
		{
		}

		//-------------------------------------------------------------------
		// Properties
		//-------------------------------------------------------------------

		[Category("Layout")]
		public Radii Radii
		{
			get => m_radii;
			set
			{
				m_radii = value;
				Invalidate();
			}
		}

		//-------------------------------------------------------------------
		// TableLayoutPanel Overrides
		//-------------------------------------------------------------------

		// OnPaint
		//
		// Invoked when the control is being painted
		protected override void OnPaint(PaintEventArgs args)
		{
			// Paint the background using the back color of the parent
			using(Brush background = new SolidBrush(Parent.BackColor))
			{
				// Paint the rounded
				using(Brush brush = new SolidBrush(BackColor))
				{
					// Don't use anti-aliasing on the background
					args.Graphics.FillRectangle(background, ClientRectangle);

					// Use anti-aliasing on the rounded rectangle
					args.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
					args.Graphics.FillRoundedRectangle(brush, ClientRectangle, m_radii.TopLeft, m_radii.TopRight, m_radii.BottomRight, m_radii.BottomLeft);
				}
			}
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private Radii m_radii = new Radii(0);
	}
}
