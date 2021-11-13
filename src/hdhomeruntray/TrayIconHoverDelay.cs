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

using System.ComponentModel;

namespace zuki.hdhomeruntray
{
	//-----------------------------------------------------------------------
	// Enum TrayIconHoverDelay
	//
	// Defines constants that control how quickly the pop-up form will show
	// when the user hovers the mouse over the tray icon.  Values are in ms

	public enum TrayIconHoverDelay
	{
		[Description("System Default")]
		SystemDefault = 0,

		[Description("None")]
		None = 1,

		[Description("250 Milliseconds")]
		TwoHundredFiftyMilliseconds = 250,

		[Description("500 Milliseconds")]
		FiveHundredMilliseconds = 500,

		[Description("1 Second")]
		OneSecond = 1000,

		[Description("2 Seconds")]
		TwoSeconds = 2000,
	}
}
