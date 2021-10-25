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

using System.Drawing;
using System.Windows.Forms;
using zuki.hdhomeruntray.interop;

namespace hdhomeruntray
{
	public partial class MainForm : Form
	{
		#region Win32 API Declarations
		/// <summary>
		/// Win32 API Declarations
		/// </summary>
		private static class NativeMethods
		{
			public const int SM_CXSMICON = 49;

			[System.Runtime.InteropServices.DllImport("user32.dll")]
			public static extern int GetSystemMetrics(int smIndex);
		}
		#endregion

		public MainForm()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			// remove me; testing DPI support
			int iconsize = NativeMethods.GetSystemMetrics(NativeMethods.SM_CXSMICON);

			// remove me; exercising the collection
			DeviceCollection devices = DeviceCollection.Create(DiscoveryMethod.Broadcast);
		}
	}
}
