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
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace zuki.hdhomeruntray
{
	//-----------------------------------------------------------------------
	// Class SignalStatusProcessBar (internal)
	//
	// Replacement for a standard ProgressBar that behaves more like I want it to
	// for use with the tuner signal statistics control
	//
	// Based on:
	// https://docs.microsoft.com/en-US/troubleshoot/dotnet/csharp/create-smooth-progress-bar

	internal partial class SignalStatusProcessBar : UserControl
	{
		// Instance Constructor
		//
		public SignalStatusProcessBar()
		{
			InitializeComponent();

			Padding = Padding.ScaleDPI(Handle);
		}

		//-------------------------------------------------------------------
		// Properties
		//-------------------------------------------------------------------

		// Maximum
		//
		// Get/sets the maximum value of the progress bar
		public int Maximum
		{
			get => m_maximum;
			set
			{
				if((value < 0) || (value < m_minimum)) throw new ArgumentOutOfRangeException(nameof(value));

				m_maximum = value;
				Invalidate();
			}
		}

		// Minimum
		//
		// Get/sets the minimum value of the progress bar
		public int Minimum
		{
			get => m_minimum;
			set
			{
				if((value < m_minimum) || (value > m_maximum)) throw new ArgumentOutOfRangeException(nameof(value));

				m_value = value;
				Invalidate();
			}
		}

		// ProgressBarColor
		//
		// Gets/sets the color of the progress bar
		public Color ProgressBarColor
		{
			get => m_color;
			set
			{
				m_color = value;
				Invalidate();
			}
		}

		// Value
		//
		// Gets/sets the current value of the progress bar
		public int Value
		{
			get => m_value;
			set
			{
				if((value < m_minimum) || (value > m_maximum)) throw new ArgumentOutOfRangeException(nameof(value));

				if(value == m_value) return;

				int previous = m_value;
				m_value = value;

				// Invalidate only the changed area of the control
				Rectangle newValueRect = GetPaddedClientRectangle();
				Rectangle oldValueRect = newValueRect;

				// Use a new value to calculate the rectangle for progress
				float percent = (m_value - m_minimum) / (float)(m_maximum - m_minimum);
				newValueRect.Width = (int)(newValueRect.Width * percent);

				// Use an old value to calculate the rectangle for progress.
				percent = (previous - m_minimum) / (float)(m_maximum - m_minimum);
				oldValueRect.Width = (int)(oldValueRect.Width * percent);

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

				updateRect.Height = Height;

				Invalidate(updateRect);
			}
		}

		//-------------------------------------------------------------------
		// UserControl Overrides
		//-------------------------------------------------------------------

		// OnPaint
		//
		// Invoked to paint the control surface
		protected override void OnPaint(PaintEventArgs args)
		{
			float percent = (m_value - m_minimum) / (float)(m_maximum - m_minimum);

			Rectangle rect = GetPaddedClientRectangle();
			rect.Width = (int)(rect.Width * percent);

			using(SolidBrush brush = new SolidBrush(m_color))
			{
				// Fill with slighly rounded corners
				args.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
				args.Graphics.FillRoundedRectangle(brush, rect, Math.Min(rect.Width, 2.ScaleDPI(Handle)));
			}
		}

		// OnResize
		//
		// Invoked when the control is resized
		protected override void OnResize(EventArgs args)
		{
			Invalidate();
		}

		//-------------------------------------------------------------------
		// Private Member Functions
		//-------------------------------------------------------------------

		// GetPaddedClientRectangle
		//
		// Gets the client rectangle, taking padding into account
		private Rectangle GetPaddedClientRectangle()
		{
			return new Rectangle(ClientRectangle.Left + Padding.Left,
				ClientRectangle.Top + Padding.Top, ClientRectangle.Width - (Padding.Left + Padding.Right),
				ClientRectangle.Height - (Padding.Top + Padding.Bottom));
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private readonly int m_minimum = 0;
		private int m_maximum = 100;
		private int m_value = 0;
		private Color m_color = Color.Blue;
	}
}
