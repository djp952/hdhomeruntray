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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace zuki.hdhomeruntray
{
    //-----------------------------------------------------------------------
    // Class RoundedFlowLayoutPanel (internal)
    //
    // Customization of the FlowLayoutPanel control to provide rounded corners
    //
    // Based on:
    // https://stackoverflow.com/questions/11540117/panel-with-rounded-edges
    //
    // TODO: Borders, the reference implementation above didn't work well at all,
    // what I want is a nice Windows-11-esque faint drop shadow here

    class RoundedFlowLayoutPanel : FlowLayoutPanel
    {
        // Instance Constructor
        //
        public RoundedFlowLayoutPanel() : base()
        {
            // Ensure the control is being double-buffered
            DoubleBuffered = true;
        }

        //-------------------------------------------------------------------
        // Properties
        //-------------------------------------------------------------------

        // Radius
        //
        // Gets/sets the corner radius to be applied to the control during paint
        [DefaultValue(0)]
		public int Radius
		{
			get
			{
				return m_radius;
			}
			set
			{
                if(value < 0) throw new ArgumentOutOfRangeException("value");
                else if(value != m_radius)
                {
                    m_radius = value;
                    Invalidate();
                }
			}
		}

        //-------------------------------------------------------------------
        // FlowLayoutPanel Overrides
        //-------------------------------------------------------------------

        // OnPaint
        //
        // Invoked when the control is being painted
        protected override void OnPaint(PaintEventArgs args)
        {
            // Invoke the base implementation first
            base.OnPaint(args);

            // This only works if a non-zero radius has been specified
            if(m_radius > 0)
            {
                // Scale the radius based on the DPI of the control
                float factorx = args.Graphics.DpiX / 96.0F;
                float factory = args.Graphics.DpiY / 96.0F;
                int radius = (int)(m_radius * ((factorx + factory) / 2.0F));

                // Set an anti-alised smoothing mode
                args.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // Create a GraphicsPath that will generate the necessary information
                // to effectively round the corners of the control
                GraphicsPath path = new GraphicsPath();
                path.StartFigure();
                path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
                path.AddLine(radius, 0, Width - radius, 0);
                path.AddArc(new Rectangle(Width - radius, 0, radius, radius), 270, 90);
                path.AddLine(Width, radius, Width, Height - radius);
                path.AddArc(new Rectangle(Width - radius, Height - radius, radius, radius), 0, 90);
                path.AddLine(Width - radius, Height, radius, Height);
                path.AddArc(new Rectangle(0, Height - radius, radius, radius), 90, 90);
                path.AddLine(0, Height - radius, 0, radius);
                path.CloseFigure();

                // Assign the generated GraphicsPath to the control
                Region = new Region(path);
            }
        }

        //-------------------------------------------------------------------
        // Member Variables
        //-------------------------------------------------------------------

        private int m_radius = 0;               // Corner radius
    }
}
