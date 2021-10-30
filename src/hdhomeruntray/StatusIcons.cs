﻿//---------------------------------------------------------------------------
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
	// Class StatusIcons (internal)
	//
	// Helper class used to load the proper system tray icons for the system
	
	class StatusIcons
	{
		// Static Constructor
		//
		static StatusIcons()
		{
			// Create icons of the proper size for the system from the embedded resources
			s_active_dark = new Icon(Properties.Resources.trayicon_green_dark, SystemInformation.SmallIconSize);
			s_active_light = new Icon(Properties.Resources.trayicon_green_light, SystemInformation.SmallIconSize);
			s_idle_dark = new Icon(Properties.Resources.trayicon_gray_dark, SystemInformation.SmallIconSize);
			s_idle_light = new Icon(Properties.Resources.trayicon_gray_light, SystemInformation.SmallIconSize);
			s_recording_dark = new Icon(Properties.Resources.trayicon_red_dark, SystemInformation.SmallIconSize);
			s_recording_light = new Icon(Properties.Resources.trayicon_red_light, SystemInformation.SmallIconSize);
		}

		//-------------------------------------------------------------------
		// Member Functions
		//-------------------------------------------------------------------

		// Get (static)
		//
		// Retrieves the specified icon
		public static Icon Get(StatusIconType type)
		{
			// TODO: Theme support
			switch(type)
			{
				case StatusIconType.Active: return s_active_dark;
				case StatusIconType.Idle: return s_idle_dark;
				case StatusIconType.Recording: return s_recording_dark;
				default: throw new ArgumentOutOfRangeException("type");
			}
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private static readonly Icon s_active_dark;			// Active (dark)
		private static readonly Icon s_active_light;		// Active (light)
		private static readonly Icon s_idle_dark;			// Idle (dark)
		private static readonly Icon s_idle_light;			// Idle (light)
		private static readonly Icon s_recording_dark;		// Recording (dark)
		private static readonly Icon s_recording_light;		// Recording (light)
	}
}