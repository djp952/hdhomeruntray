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
using zuki.hdhomeruntray.Properties;

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
			VirtualLEDColorSet colorset = Settings.Default.VirtualLEDColorSet;

			// The "System" colorset depends on if the user has a color filter enabled or not
			if(colorset == VirtualLEDColorSet.System)
			{
				colorset = VirtualLEDColorSet.GreenRed;				// Default to green/red

				// If on Windows 10 / Windows 11, change the color set to blue/orange if the user
				// has any color filtering setting applied at the operating system level
				if(VersionHelper.IsWindows10OrGreater())
				{
					object value = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering", "Active", 0);
					if((value is int @int) && (@int != 0)) colorset = VirtualLEDColorSet.BlueOrange;
				}
			}

			// High contrast mode requires checking what the color of the taskbar will be
			if(SystemInformation.HighContrast)
			{
				// It appears the taskbar will follow SystemColors.Control
				int brightness = Brightness(SystemColors.Control);
				if(brightness < 130) lighticon = true;
			}

			// On Windows 10 and above the icon should be appropriate for the system theme
			else if(VersionHelper.IsWindows10OrGreater())
			{
				// NOTE: Assume zero (dark taskbar / light icon) here if the value is missing
				object value = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "SystemUsesLightTheme", 0);
				if((value is int @int) && (@int != 1)) lighticon = true;
			}

			switch(devicestatus)
			{
				// Idle is always gray
				case DeviceStatus.Idle: 
					return (lighticon) ? s_graylight : s_graydark;

				// Active is either green or blue
				case DeviceStatus.Active: 
					if(colorset == VirtualLEDColorSet.BlueOrange) return (lighticon) ? s_bluelight : s_bluedark;
					else return (lighticon) ? s_greenlight : s_greendark;

				// ActiveAndRecording is either red or orange
				case DeviceStatus.ActiveAndRecording:
					if(colorset == VirtualLEDColorSet.BlueOrange) return (lighticon) ? s_orangelight : s_orangedark;
					else return (lighticon) ? s_redlight : s_reddark;

				default: throw new ArgumentOutOfRangeException(nameof(devicestatus));
			}
		}

		//-------------------------------------------------------------------
		// Private Member Functions
		//-------------------------------------------------------------------

		// Brightness
		//
		// Determines the perceived brightness of a color
		// https://www.nbdtech.com/Blog/archive/2008/04/27/calculating-the-perceived-brightness-of-a-color.aspx
		private static int Brightness(Color color)
		{
			return (int)Math.Sqrt(color.R * color.R * .241 + color.G * color.G * .691 + color.B * color.B * .068);
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private static readonly Icon s_bluedark = new Icon(Properties.Resources.TrayIconBlueDark, SystemInformation.SmallIconSize);
		private static readonly Icon s_bluelight = new Icon(Properties.Resources.TrayIconBlueLight, SystemInformation.SmallIconSize);
		private static readonly Icon s_graydark = new Icon(Properties.Resources.TrayIconGrayDark, SystemInformation.SmallIconSize);
		private static readonly Icon s_graylight = new Icon(Properties.Resources.TrayIconGrayLight, SystemInformation.SmallIconSize);
		private static readonly Icon s_greendark = new Icon(Properties.Resources.TrayIconGreenDark, SystemInformation.SmallIconSize);
		private static readonly Icon s_greenlight = new Icon(Properties.Resources.TrayIconGreenLight, SystemInformation.SmallIconSize);
		private static readonly Icon s_orangedark = new Icon(Properties.Resources.TrayIconOrangeDark, SystemInformation.SmallIconSize);
		private static readonly Icon s_orangelight = new Icon(Properties.Resources.TrayIconOrangeLight, SystemInformation.SmallIconSize);
		private static readonly Icon s_reddark = new Icon(Properties.Resources.TrayIconRedDark, SystemInformation.SmallIconSize);
		private static readonly Icon s_redlight = new Icon(Properties.Resources.TrayIconRedLight, SystemInformation.SmallIconSize);
	}
}
