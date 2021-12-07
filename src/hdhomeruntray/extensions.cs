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
using System.Linq;
using System.Windows.Forms;

namespace zuki.hdhomeruntray
{
    //-----------------------------------------------------------------------
    // Class Extensions
    //
    // Implements extension methods

    static class Extensions
    {
		//-------------------------------------------------------------------
		// ComboBox.BindEnum
		//
		// Binds an enumeration type to a ComboBox using DescriptionAttribute
		//
		// Based on BindEnumToComboBox<T>:
		// https://stackoverflow.com/questions/35935961/bind-combobox-with-enum-description/35936210
		//
		// Modified to sort by the integer value instead of the string representation of that value

		public static void BindEnum<T>(this ComboBox combobox, T @default)
		{
			var list = Enum.GetValues(typeof(T))
				.Cast<T>()
				.Select(value => new
				{
					Description = (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute)) as DescriptionAttribute)?.Description ?? value.ToString(),
					Value = value
				})
				.OrderBy(item => item.Value)		// MB: was item.Value.ToString()
				.ToList();

			combobox.DataSource = list;
			combobox.DisplayMember = "Description";
			combobox.ValueMember = "Value";

			foreach(var opts in list)
			{
				if(opts.Value.ToString() == @default.ToString())
				{
					combobox.SelectedItem = opts;
				}
			}
		}

		//-------------------------------------------------------------------
		// float.ScaleDPI
		//
		// Scales a float value based on the DPI of the system

		public static float ScaleDPI(this float value, IntPtr handle)
		{
			float factorx = 1.0F;           // Assume no horizontal scaling
			float factory = 1.0F;           // Assume no vertical scaling

			// Use a Graphics instance to determine the control's DPI factor
			using(Graphics gr = Graphics.FromHwnd(handle))
			{
				factorx = gr.DpiX / 96.0F;
				factory = gr.DpiY / 96.0F;
			}

			return (float)(value * ((factorx + factory) / 2.0));
		}

		//-------------------------------------------------------------------
		// Graphics.DrawRoundedRectangle
		//
		// Draws a rounded rectangle specified by a bounding Rectangle and four corner radius values
		//
		// KygSoft.Drawing
		// https://github.com/koszeggy/KGySoft.Drawing/blob/master/KGySoft.Drawing/Drawing/_Extensions/GraphicsExtensions.cs

		public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, Rectangle bounds, int radiusTopLeft, int radiusTopRight, int radiusBottomRight, int radiusBottomLeft)
		{
			if(graphics == null) throw new ArgumentNullException(nameof(graphics));
			if(pen == null) throw new ArgumentNullException(nameof(pen));

			using(GraphicsPath path = CreateRoundedRectangle(bounds, radiusTopLeft, radiusTopRight, radiusBottomRight, radiusBottomLeft))
			{
				graphics.DrawPath(pen, path);
			}
		}

		//-------------------------------------------------------------------
		// Graphics.DrawRoundedRectangle
		//
		// Draws a rounded rectangle specified by a bounding Rectangle and a common corner radius value for each corners
		//
		// KygSoft.Drawing
		// https://github.com/koszeggy/KGySoft.Drawing/blob/master/KGySoft.Drawing/Drawing/_Extensions/GraphicsExtensions.cs

		public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, Rectangle bounds, int cornerRadius)
		{
			if(graphics == null) throw new ArgumentNullException(nameof(graphics));
			if(pen == null) throw new ArgumentNullException(nameof(pen));

			using(GraphicsPath path = CreateRoundedRectangle(bounds, cornerRadius))
			{
				graphics.DrawPath(pen, path);
			}
		}

		//-------------------------------------------------------------------
		// Graphics.FillRoundedRectangle
		//
		// Fills a rounded rectangle specified by a bounding Rectangle and four custom corner radius values
		//
		// KygSoft.Drawing
		// https://github.com/koszeggy/KGySoft.Drawing/blob/master/KGySoft.Drawing/Drawing/_Extensions/GraphicsExtensions.cs

		public static void FillRoundedRectangle(this Graphics graphics, Brush brush, Rectangle bounds, int radiusTopLeft, int radiusTopRight, int radiusBottomRight, int radiusBottomLeft)
		{
			if(graphics == null) throw new ArgumentNullException(nameof(graphics));
			if(brush == null) throw new ArgumentNullException(nameof(brush));

			using(GraphicsPath path = CreateRoundedRectangle(bounds, radiusTopLeft, radiusTopRight, radiusBottomRight, radiusBottomLeft))
			{
				graphics.FillPath(brush, path);
			}
		}

		//-------------------------------------------------------------------
		// Graphics.FillRoundedRectangle
		//
		// Fills a rounded rectangle specified by a bounding Rectangle and a common corner radius value for each corners
		//
		// KygSoft.Drawing
		// https://github.com/koszeggy/KGySoft.Drawing/blob/master/KGySoft.Drawing/Drawing/_Extensions/GraphicsExtensions.cs

		public static void FillRoundedRectangle(this Graphics graphics, Brush brush, Rectangle bounds, int cornerRadius)
		{
			if(graphics == null) throw new ArgumentNullException(nameof(graphics));
			if(brush == null) throw new ArgumentNullException(nameof(brush));

			using(GraphicsPath path = CreateRoundedRectangle(bounds, cornerRadius))
			{
				graphics.FillPath(brush, path);
			}
		}

		//-------------------------------------------------------------------
		// int.ScaleDPI
		//
		// Scales an integer value based on the DPI of the system

		public static int ScaleDPI(this int value, IntPtr handle)
		{
			float factor = 1.0F;            // Assume no scaling

			// Use a Graphics instance to determine the control's DPI factor
			using(Graphics gr = Graphics.FromHwnd(handle))
			{
				factor = (gr.DpiX / 96.0F) + (gr.DpiY / 96.0F) / 2.0F;
			}

			return (int)(value * factor);
		}

		//-------------------------------------------------------------------
		// Padding.ScaleDPI
		//
		// Scales a Padding value based on the DPI of the system

		public static Padding ScaleDPI(this Padding padding, IntPtr handle)
		{
			float factorx = 1.0F;			// Assume no horizontal scaling
			float factory = 1.0F;			// Assume no vertical scaling

			// Use a Graphics instance to determine the control's DPI factor
			using(Graphics gr = Graphics.FromHwnd(handle))
			{
				factorx = gr.DpiX / 96.0F;
				factory = gr.DpiY / 96.0F;
			}

			// Create a new Padding instance with the scaling factors applied
			return new Padding((int)(padding.Left * factorx), (int)(padding.Top * factory),
				(int)(padding.Right * factorx), (int)(padding.Bottom * factory));
		}

		//-------------------------------------------------------------------
		// Radii.ScaleDPI
		//
		// Scales a Radii value based on the DPI of the system

		public static Radii ScaleDPI(this Radii radii, IntPtr handle)
		{
			float factor = 1.0F;			// Assume no scaling

			// Use a Graphics instance to determine the control's DPI factor
			using(Graphics gr = Graphics.FromHwnd(handle))
			{
				factor = (gr.DpiX / 96.0F) + (gr.DpiY / 96.0F) / 2.0F;
			}

			// Create a new Radii instance with the scaling factor applied
			return new Radii((int)(radii.TopLeft * factor), (int)(radii.TopRight * factor),
				(int)(radii.BottomRight * factor), (int)(radii.BottomLeft * factor));
		}

		//-------------------------------------------------------------------
		// Private Member Functions
		//-------------------------------------------------------------------

		//-------------------------------------------------------------------
		// CreateRoundedRectangle
		//
		// Returns the path for a rounded rectangle specified by a bounding <see cref="Rectangle"/> structure and four corner radius values
		//
		// KygSoft.Drawing
		// https://github.com/koszeggy/KGySoft.Drawing/blob/master/KGySoft.Drawing/Drawing/_Extensions/GraphicsExtensions.cs

		private static GraphicsPath CreateRoundedRectangle(Rectangle bounds, int radiusTopLeft, int radiusTopRight, int radiusBottomRight, int radiusBottomLeft)
		{
			var size = new Size(radiusTopLeft << 1, radiusTopLeft << 1);
			var arc = new Rectangle(bounds.Location, size);
			var path = new GraphicsPath();

			// top left arc
			if(radiusTopLeft == 0) path.AddLine(arc.Location, arc.Location);
			else path.AddArc(arc, 180, 90);

			// top right arc
			if(radiusTopRight != radiusTopLeft)
			{
				size = new Size(radiusTopRight << 1, radiusTopRight << 1);
				arc.Size = size;
			}

			arc.X = bounds.Right - size.Width;
			if(radiusTopRight == 0) path.AddLine(arc.Location, arc.Location);
			else path.AddArc(arc, 270, 90);

			// bottom right arc
			if(radiusTopRight != radiusBottomRight)
			{
				size = new Size(radiusBottomRight << 1, radiusBottomRight << 1);
				arc.X = bounds.Right - size.Width;
				arc.Size = size;
			}

			arc.Y = bounds.Bottom - size.Height;
			if(radiusBottomRight == 0) path.AddLine(arc.Location, arc.Location);
			else path.AddArc(arc, 0, 90);

			// bottom left arc
			if(radiusBottomRight != radiusBottomLeft)
			{
				arc.Size = new Size(radiusBottomLeft << 1, radiusBottomLeft << 1);
				arc.Y = bounds.Bottom - arc.Height;
			}

			arc.X = bounds.Left;
			if(radiusBottomLeft == 0) path.AddLine(arc.Location, arc.Location);
			else path.AddArc(arc, 90, 90);

			path.CloseFigure();
			return path;
		}

		//-------------------------------------------------------------------
		// CreateRoundedRectangle
		//
		// Returns the path for a rounded rectangle specified by a bounding structure and a common corner radius value for each corners
		//
		// KygSoft.Drawing
		// https://github.com/koszeggy/KGySoft.Drawing/blob/master/KGySoft.Drawing/Drawing/_Extensions/GraphicsExtensions.cs

		private static GraphicsPath CreateRoundedRectangle(Rectangle bounds, int radius)
		{
			var path = new GraphicsPath();
			if(radius == 0)
			{
				path.AddRectangle(bounds);
				return path;
			}

			int diameter = radius * 2;
			var size = new Size(diameter, diameter);
			var arc = new Rectangle(bounds.Location, size);

			// top left arc
			path.AddArc(arc, 180, 90);

			// top right arc
			arc.X = bounds.Right - diameter;
			path.AddArc(arc, 270, 90);

			// bottom right arc
			arc.Y = bounds.Bottom - diameter;
			path.AddArc(arc, 0, 90);

			// bottom left arc
			arc.X = bounds.Left;
			path.AddArc(arc, 90, 90);

			path.CloseFigure();
			return path;
		}
	}
}
