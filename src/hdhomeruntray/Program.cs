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
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace zuki.hdhomeruntray
{
	internal static class Program
	{
		#region Win32 API Declarations
		private static class NativeMethods
		{
			[DllImport("user32.dll")]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool SetProcessDPIAware();
		}
		#endregion

		// Main
		//
		// Application entry point
		[STAThread]
		private static void Main()
		{
			// Prevent multiple instances of the application from running at the same
			// time; the tray icon is registered with a GUID, it will only show up once
			s_mutex = new Mutex(true, Application.ProductName, out bool creatednew);
			if(!creatednew) return;

			if(Environment.OSVersion.Version.Major >= 6) NativeMethods.SetProcessDPIAware();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainApplication());

			s_mutex.Close();
		}


		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private static Mutex s_mutex = null;        // Single instance mutex
	}
}
