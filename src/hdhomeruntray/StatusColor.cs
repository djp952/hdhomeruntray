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
using System.ComponentModel;
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
		// Static Constructor
		//
		static StatusColor()
		{
			if(Settings.Default.StatusColorSet == StatusColorSet.System) s_filteractive = GetColorFilterActive();
			else if(Settings.Default.StatusColorSet == StatusColorSet.GreenRed) s_filteractive = false;
			else if(Settings.Default.StatusColorSet == StatusColorSet.BlueOrange) s_filteractive = true;

			// Wire up a handler to watch for property changes
			Settings.Default.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
		}

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

		//-------------------------------------------------------------------------
		// Events
		//-------------------------------------------------------------------------

		// Changed
		//
		// Invoked when the status color set has changed
		public static event EventHandler Changed;

		//-------------------------------------------------------------------
		// Member Functions
		//-------------------------------------------------------------------

		// ColorFilterChanged (static)
		//
		// Invoked when the color filter has been changed
		public static void ColorFilterChanged(object sender, EventArgs args)
		{
			// This is only applicable if the setting is set to System
			if(Settings.Default.StatusColorSet == StatusColorSet.System)
			{
				// Get a new color filter flag for the system, and if it changed inform listeners
				bool filteractive = GetColorFilterActive();
				if(filteractive != s_filteractive)
				{
					s_filteractive = filteractive;
					Changed?.Invoke(typeof(ApplicationTheme), EventArgs.Empty);
				}
			}
		}

		// Rebase (static)
		//
		// Resets a color due to a color set switch
		public static Color Rebase(Color color)
		{
			if(s_filteractive)
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
			Color result = Gray;			// Default to gray

			if(status == DeviceStatus.Active) result = s_filteractive ? Blue : Green;
			else if(status == DeviceStatus.ActiveAndRecording) result = s_filteractive ? Orange : Red;

			return result;
		}

		// FromDeviceStatusColor (static)
		//
		// Retrieves the correct GUI color based on a DeviceStatusColor
		public static Color FromDeviceStatusColor(DeviceStatusColor devicestatuscolor)
		{
			Color result = Gray;			// Default to gray

			if(devicestatuscolor == DeviceStatusColor.Green) result = s_filteractive ? Blue : Green;
			else if(devicestatuscolor == DeviceStatusColor.Red) result = s_filteractive ? Orange : Red;
			else if(devicestatuscolor == DeviceStatusColor.Yellow) result = s_filteractive ? Blue : Yellow;

			return result;
		}

		//-------------------------------------------------------------------
		// Event Handlers
		//-------------------------------------------------------------------

		// OnPropertyChanged
		//
		// Invoked when a settings property has been changed
		private static void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			// StatusColorSet
			//
			if(args.PropertyName == nameof(Settings.Default.StatusColorSet))
			{
				bool filteractive = GetColorFilterActive();

				// Override for specific settings
				if(Settings.Default.StatusColorSet == StatusColorSet.GreenRed) filteractive = false;
				else if(Settings.Default.StatusColorSet == StatusColorSet.BlueOrange) filteractive = true;

				// If the setting has changed, invoke the event to inform
				if(filteractive != s_filteractive)
				{
					s_filteractive = filteractive;
					Changed?.Invoke(typeof(StatusColor), EventArgs.Empty);
				}
			}
		}

		//-------------------------------------------------------------------
		// Private Member Functions
		//-------------------------------------------------------------------

		// GetColorFilterActive
		//
		// Gets the flag indicating if color filtering is active
		private static bool GetColorFilterActive()
		{
			// On Windows 10 / Windows 11, check if filtering is currently active
			if(VersionHelper.IsWindows10OrGreater())
			{
				object value = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering", "Active", 0);
				if((value is int @int) && (@int != 0)) return true;
			}

			return false;                   // Default to no filtering
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private static bool s_filteractive = false;
	}
}
