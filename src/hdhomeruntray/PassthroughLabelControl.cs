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
using System.Windows.Forms;

namespace zuki.hdhomeruntray
{
	//-----------------------------------------------------------------------
	// Class PassthroughLabelControl (internal)
	//
	// Customization of the Label control to have mouse messages pass through it
	//
	// Based on:
	// https://stackoverflow.com/questions/547172/pass-through-mouse-events-to-parent-control

	internal class PassthroughLabelControl : Label
	{
		#region Win32 API Declarations
		/// <summary>
		/// Win32 API Declarations
		/// </summary>
		private static class NativeMethods
		{
			public const int HTTRANSPARENT = -1;
			public const uint WM_NCHITTEST = 0x0084;
		}
		#endregion

		// Instance Constructor
		//
		public PassthroughLabelControl() : base()
		{
		}

		//-------------------------------------------------------------------
		// Label Overrides
		//-------------------------------------------------------------------

		// WndProc
		//
		// Processes window messages
		protected override void WndProc(ref Message message)
		{
			// WM_NCHITTEST - Send the message to the underlying window(s) in the
			// same thread until one does not return HTTRANSPARENT
			if((uint)message.Msg == NativeMethods.WM_NCHITTEST)
				message.Result = (IntPtr)NativeMethods.HTTRANSPARENT;
			else
				base.WndProc(ref message);
		}
	}
}
