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

using System.Drawing;

using Microsoft.Win32;
using zuki.hdhomeruntray.discovery;
using zuki.hdhomeruntray.Properties;

namespace zuki.hdhomeruntray
{
	//-----------------------------------------------------------------------
	// Class StatusColor (internal)
	//
	// Helper class used to convert device status colors into GUI status colors

	internal static class StatusColor
	{
		//-------------------------------------------------------------------
		// Fields
		//-------------------------------------------------------------------

		// Blue
		//
		public static readonly Color Blue = Color.FromArgb(0x06, 0xA3, 0xFD);

		// Gray
		//
		public static readonly Color Gray = Color.FromArgb(0xC0, 0xC0, 0xC0);

		// Green
		//
		public static readonly Color Green = Color.FromArgb(0x1E, 0xE5, 0x00);

		// Orange
		//
		public static readonly Color Orange = Color.FromArgb(0xF8, 0x64, 0x01);

		// Red
		//
		public static readonly Color Red = Color.FromArgb(0xE5, 0x00, 0x00);

		// Yellow
		//
		public static readonly Color Yellow = Color.FromArgb(0xFF, 0xE9, 0x00);

		//-------------------------------------------------------------------
		// Member Functions
		//-------------------------------------------------------------------

		// Rebase (static)
		//
		// Resets a color due to a color set switch
		public static Color Rebase(Color color)
		{
			StatusColorSet colorset = Settings.Default.StatusColorSet;

			// The "System" colorset depends on if the user has a color filter enabled or not
			if(colorset == StatusColorSet.System)
			{
				colorset = StatusColorSet.GreenRed;             // Default to green/red

				// If on Windows 10 / Windows 11, change the color set to blue/orange if the user
				// has any color filtering setting applied at the operating system level
				if(VersionHelper.IsWindows10OrGreater())
				{
					object value = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering", "Active", 0);
					if((value is int @int) && (@int != 0)) colorset = StatusColorSet.BlueOrange;
				}
			}

			if(colorset == StatusColorSet.BlueOrange)
			{
				if(color == Green) return Blue;
				if(color == Red) return Orange;
			}
			else
			{
				if(color == Blue) return Green;
				if(color == Orange) return Red;
			}

			// Gray and Yellow don't get changed
			return color;
		}

		// FromDeviceStatus (static)
		//
		// Retrieves the correct GUI color based on a DeviceStatus
		public static Color FromDeviceStatus(DeviceStatus status)
		{
			Color result = Gray;
			StatusColorSet colorset = Settings.Default.StatusColorSet;

			// The "System" colorset depends on if the user has a color filter enabled or not
			if(colorset == StatusColorSet.System)
			{
				colorset = StatusColorSet.GreenRed;             // Default to green/red

				// If on Windows 10 / Windows 11, change the color set to blue/orange if the user
				// has any color filtering setting applied at the operating system level
				if(VersionHelper.IsWindows10OrGreater())
				{
					object value = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering", "Active", 0);
					if((value is int @int) && (@int != 0)) colorset = StatusColorSet.BlueOrange;
				}
			}

			// Active = Green/Blue
			//
			if(status == DeviceStatus.Active) result = (colorset == StatusColorSet.BlueOrange) ? Blue : Green;

			// ActiveAndRecording = Red/Orange
			//
			else if(status == DeviceStatus.ActiveAndRecording) result = (colorset == StatusColorSet.BlueOrange) ? Orange : Red;

			return result;
		}

		// FromDeviceStatusColor (static)
		//
		// Retrieves the correct GUI color based on a DeviceStatusColor
		public static Color FromDeviceStatusColor(DeviceStatusColor devicestatuscolor)
		{
			Color result = Gray;
			StatusColorSet colorset = Settings.Default.StatusColorSet;

			// The "System" colorset depends on if the user has a color filter enabled or not
			if(colorset == StatusColorSet.System)
			{
				colorset = StatusColorSet.GreenRed;             // Default to green/red

				// If on Windows 10 / Windows 11, change the color set to blue/orange if the user
				// has any color filtering setting applied at the operating system level
				if(VersionHelper.IsWindows10OrGreater())
				{
					object value = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering", "Active", 0);
					if((value is int @int) && (@int != 0)) colorset = StatusColorSet.BlueOrange;
				}
			}

			if(devicestatuscolor == DeviceStatusColor.Neutral) result = Gray;
			else if(devicestatuscolor == DeviceStatusColor.Green) result = (colorset == StatusColorSet.BlueOrange) ? Blue : Green;
			else if(devicestatuscolor == DeviceStatusColor.Red) result = (colorset == StatusColorSet.BlueOrange) ? Orange : Red;
			else if(devicestatuscolor == DeviceStatusColor.Yellow) result = (colorset == StatusColorSet.BlueOrange) ? Blue : Yellow;

			return result;
		}
	}
}
