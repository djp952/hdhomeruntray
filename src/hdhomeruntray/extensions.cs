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
		// Size.ScaleDPI
		//
		// Scales a Size value based on the DPI of the system

		public static Size ScaleDPI(this Size size, IntPtr handle)
		{
			float factorx = 1.0F;           // Assume no horizontal scaling
			float factory = 1.0F;           // Assume no vertical scaling

			// Use a Graphics instance to determine the control's DPI factor
			using(Graphics gr = Graphics.FromHwnd(handle))
			{
				factorx = gr.DpiX / 96.0F;
				factory = gr.DpiY / 96.0F;
			}

			// Create a new Size instance with the scaling factors applied
			return new Size((int)(size.Width * factorx), (int)(size.Height * factory));
		}
	}
}
