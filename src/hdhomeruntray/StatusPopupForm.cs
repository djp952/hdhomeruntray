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

namespace hdhomeruntray
{
	//-----------------------------------------------------------------------
	// Class StatusPopupForm (internal)
	//
	// Implements the "popup" form that appears when the user hovers over the
	// system tray icon; provides read-only device status information
	
	partial class StatusPopupForm : Form
	{
		#region Win32 API Declarations
		/// <summary>
		/// Win32 API Declarations
		/// </summary>
		private static class NativeMethods
		{
			[System.Runtime.InteropServices.DllImport("gdi32.dll")]
			public static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int w, int h);

			[System.Runtime.InteropServices.DllImport("gdi32.dll")]
			[return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
			public static extern bool DeleteObject(IntPtr hObject);
		}
		#endregion
		
		// Instance Constructor
		//
		public StatusPopupForm()
		{
			InitializeComponent();
		}

		// Dispose
		//
		// Releases unmanaged resources and optionally releases managed resources
		protected override void Dispose(bool disposing)
		{
			// Dispose managed state
			if(disposing && (components != null))
			{
				components.Dispose();
			}

			// Dispose unmanaged state
			if(m_hrgn != IntPtr.Zero) NativeMethods.DeleteObject(m_hrgn);
		
			base.Dispose(disposing);
		}

		//-------------------------------------------------------------------
		// Event Handlers
		//-------------------------------------------------------------------

		// OnSizeChanged
		//
		// Invoked when the size of the form has changed
		private void OnSizeChanged(object sender, EventArgs args)
		{
			IntPtr hrgnprev = m_hrgn;			// Save previous HRGN

			// Create a new region for the form based on the new width and height
			m_hrgn = NativeMethods.CreateRoundRectRgn(0, 0, Width, Height, 20, 20);
			Region = Region.FromHrgn(m_hrgn);

			// Release the previously set HRGN if it wasn't NULL
			if(hrgnprev != IntPtr.Zero) NativeMethods.DeleteObject(hrgnprev);
		}

		private IntPtr m_hrgn;
	}
}
