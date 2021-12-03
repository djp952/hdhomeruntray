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
using System.Drawing;
using System.Windows.Forms;

namespace zuki.hdhomeruntray
{
    //-----------------------------------------------------------------------
    // Class SmoothProgressBar (internal)
    //
    // Replacement for a standard ProgressBar that behaves more like I want it to
    //
    // Based on:
    // https://docs.microsoft.com/en-US/troubleshoot/dotnet/csharp/create-smooth-progress-bar

    partial class SmoothProgressBar : UserControl
	{
		public SmoothProgressBar()
		{
			InitializeComponent();

            this.MinimumSize = this.MinimumSize.ScaleDPI(this.Handle);
		}

        // TODO: CLEAN THIS UP; THIS IS A COPY/PASTE JOB RIGHT NOW

        int min = 0;// Minimum value for progress range
        int max = 100;// Maximum value for progress range
        int val = 0;// Current progress
        Color BarColor = Color.Blue;// Color of progress meter

        protected override void OnResize(EventArgs e)
        {
            // Invalidate the control to get a repaint.
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            SolidBrush brush = new SolidBrush(BarColor);
            float percent = (float)(val - min) / (float)(max - min);
            Rectangle rect = this.ClientRectangle;

            // Calculate area for drawing the progress.
            rect.Width = (int)((float)rect.Width * percent);

            // Draw the progress meter.
            g.FillRectangle(brush, rect);

            // Draw a three-dimensional border around the control.
            Draw3DBorder(g);

            // Clean up.
            brush.Dispose();
            g.Dispose();
        }

        public int Minimum
        {
            get
            {
                return min;
            }

            set
            {
                // Prevent a negative value.
                if(value < 0)
                {
                    value = 0;
                }

                // Make sure that the minimum value is never set higher than the maximum value.
                if(value > max)
                {
                    max = value;
                }

                min = value;

                // Ensure value is still in range
                if(val < min)
                {
                    val = min;
                }

                // Invalidate the control to get a repaint.
                this.Invalidate();
            }
        }

        public int Maximum
        {
            get
            {
                return max;
            }

            set
            {
                // Make sure that the maximum value is never set lower than the minimum value.
                if(value < min)
                {
                    min = value;
                }

                max = value;

                // Make sure that value is still in range.
                if(val > max)
                {
                    val = max;
                }

                // Invalidate the control to get a repaint.
                this.Invalidate();
            }
        }

        public int Value
        {
            get
            {
                return val;
            }

            set
            {
                int oldValue = val;

                // Make sure that the value does not stray outside the valid range.
                if(value < min)
                {
                    val = min;
                }
                else if(value > max)
                {
                    val = max;
                }
                else
                {
                    val = value;
                }

                // Invalidate only the changed area.
                float percent;

                Rectangle newValueRect = this.ClientRectangle;
                Rectangle oldValueRect = this.ClientRectangle;

                // Use a new value to calculate the rectangle for progress.
                percent = (float)(val - min) / (float)(max - min);
                newValueRect.Width = (int)((float)newValueRect.Width * percent);

                // Use an old value to calculate the rectangle for progress.
                percent = (float)(oldValue - min) / (float)(max - min);
                oldValueRect.Width = (int)((float)oldValueRect.Width * percent);

                Rectangle updateRect = new Rectangle();

                // Find only the part of the screen that must be updated.
                if(newValueRect.Width > oldValueRect.Width)
                {
                    updateRect.X = oldValueRect.Size.Width;
                    updateRect.Width = newValueRect.Width - oldValueRect.Width;
                }
                else
                {
                    updateRect.X = newValueRect.Size.Width;
                    updateRect.Width = oldValueRect.Width - newValueRect.Width;
                }

                updateRect.Height = this.Height;

                // Invalidate the intersection region only.
                this.Invalidate(updateRect);
            }
        }

        public Color ProgressBarColor
        {
            get
            {
                return BarColor;
            }

            set
            {
                BarColor = value;

                // Invalidate the control to get a repaint.
                this.Invalidate();
            }
        }

        private void Draw3DBorder(Graphics g)
        {
            int PenWidth = (int)Pens.White.Width;

            g.DrawLine(Pens.DarkGray,
            new Point(this.ClientRectangle.Left, this.ClientRectangle.Top),
            new Point(this.ClientRectangle.Width - PenWidth, this.ClientRectangle.Top));
            g.DrawLine(Pens.DarkGray,
            new Point(this.ClientRectangle.Left, this.ClientRectangle.Top),
            new Point(this.ClientRectangle.Left, this.ClientRectangle.Height - PenWidth));
            g.DrawLine(Pens.White,
            new Point(this.ClientRectangle.Left, this.ClientRectangle.Height - PenWidth),
            new Point(this.ClientRectangle.Width - PenWidth, this.ClientRectangle.Height - PenWidth));
            g.DrawLine(Pens.White,
            new Point(this.ClientRectangle.Width - PenWidth, this.ClientRectangle.Top),
            new Point(this.ClientRectangle.Width - PenWidth, this.ClientRectangle.Height - PenWidth));
        }
    }
}
