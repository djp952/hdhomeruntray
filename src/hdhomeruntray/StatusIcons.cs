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
using System.Drawing;
using System.Windows.Forms;

using Microsoft.Win32;
using zuki.hdhomeruntray.discovery;

namespace zuki.hdhomeruntray
{
	//-----------------------------------------------------------------------
	// Class StatusIcons (internal)
	//
	// Helper class used to load the proper system tray icons for the system

	internal static class StatusIcons
	{
		//-------------------------------------------------------------------
		// Member Functions
		//-------------------------------------------------------------------

		// Get (static)
		//
		// Retrieves the specified icon
		public static Icon Get(DeviceStatus devicestatus)
		{
			bool lighticon = false;

			// On Windows 10 and above the icon should be appropriate for the system theme
			if(VersionHelper.IsWindows10OrGreater())
			{
				// NOTE: Assume zero (dark taskbar / light icon) here if the value is missing
				object value = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "SystemUsesLightTheme", 0);
				if((value is int @int) && (@int != 1)) lighticon = true;
			}

			switch(devicestatus)
			{
				case DeviceStatus.Idle: return (lighticon) ? s_idle_light : s_idle_dark;
				case DeviceStatus.Active: return (lighticon) ? s_active_light : s_active_dark;
				case DeviceStatus.ActiveAndRecording: return (lighticon) ? s_recording_light : s_recording_dark;
				default: throw new ArgumentOutOfRangeException(nameof(devicestatus));
			}
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private static readonly Icon s_active_dark = new Icon(Properties.Resources.TrayIconGreenDark, SystemInformation.SmallIconSize);
		private static readonly Icon s_active_light = new Icon(Properties.Resources.TrayIconGreenLight, SystemInformation.SmallIconSize);
		private static readonly Icon s_idle_dark = new Icon(Properties.Resources.TrayIconGrayDark, SystemInformation.SmallIconSize);
		private static readonly Icon s_idle_light = new Icon(Properties.Resources.TrayIconGrayLight, SystemInformation.SmallIconSize);
		private static readonly Icon s_recording_dark = new Icon(Properties.Resources.TrayIconRedDark, SystemInformation.SmallIconSize);
		private static readonly Icon s_recording_light = new Icon(Properties.Resources.TrayIconRedLight, SystemInformation.SmallIconSize);
	}
}
