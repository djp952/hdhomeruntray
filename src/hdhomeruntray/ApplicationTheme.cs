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

using zuki.hdhomeruntray.Properties;

namespace zuki.hdhomeruntray
{
	//-----------------------------------------------------------------------
	// Class ApplicationTheme (internal)
	//
	// Provides the theme color elements for the application

	internal static class ApplicationTheme
	{
		static ApplicationTheme()
		{
			if(Settings.Default.AppTheme == Theme.System) s_darkmode = (GetSystemTheme() == Theme.Dark);
			else if(Settings.Default.AppTheme == Theme.Light) s_darkmode = false;
			else if(Settings.Default.AppTheme == Theme.Dark) s_darkmode = true;

			// Wire up a handler to watch for property changes
			Settings.Default.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
		}

		//-------------------------------------------------------------------------
		// Events
		//-------------------------------------------------------------------------

		// Changed
		//
		// Invoked when the application theme has changed
		public static event EventHandler Changed;

		//-------------------------------------------------------------------
		// Member Functions
		//-------------------------------------------------------------------

		// SystemThemesChanged
		//
		// Invoked when the system theme(s) have changed
		public static void SystemThemesChanged(object sender, EventArgs args)
		{
			// This is only applicable if the setting is set to System
			if(Settings.Default.AppTheme == Theme.System)
			{
				// Get a new dark mode flag for the system, and if it has changed
				// inform any listeners on the Changed event to switch the theme
				bool dark = (GetSystemTheme() == Theme.Dark);
				if(dark != s_darkmode)
				{
					s_darkmode = dark;
					Changed?.Invoke(typeof(ApplicationTheme), EventArgs.Empty);
				}
			}
		}

		//-------------------------------------------------------------------
		// Properties
		//-------------------------------------------------------------------

		// FormBackColor
		//
		// The background color of a form
		public static Color FormBackColor => (s_darkmode) ? Color.FromArgb(0x20, 0x20, 0x20) : Color.FromArgb(0xF3, 0xF3, 0xF3);

		// InvertedPanelBackColor
		//
		// The background color of an interverted panel
		public static Color InvertedPanelBackColor => (s_darkmode) ? Color.FromArgb(0x58, 0x58, 0x58) : Color.FromArgb(0xB0, 0xB0, 0xB0);

		// InvertedPanelForeColor
		//
		// The text color of an inverted panel
		public static Color InvertedPanelForeColor => (s_darkmode) ? Color.White : Color.White;

		// LinkColor
		//
		// The text color of a hyperlink
		public static Color LinkColor => (s_darkmode) ? Color.LightSkyBlue : Color.SteelBlue;

		// PanelBackColor
		//
		// The background color of a panel
		public static Color PanelBackColor => (s_darkmode) ? Color.FromArgb(0x2B, 0x2B, 0x2B) : Color.White;

		// PanelForeColor
		//
		// The foreground color of a panel
		public static Color PanelForeColor => (s_darkmode) ? Color.White : Color.Black;

		//-------------------------------------------------------------------
		// Event Handlers
		//-------------------------------------------------------------------

		// OnPropertyChanged
		//
		// Invoked when a settings property has been changed
		private static void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			// AppTheme
			//
			if(args.PropertyName == nameof(Settings.Default.AppTheme))
			{
				bool dark = s_darkmode;             // Flag for dark mode

				if(Settings.Default.AppTheme == Theme.System) dark = (GetSystemTheme() == Theme.Dark);
				else if(Settings.Default.AppTheme == Theme.Light) dark = false;
				else if(Settings.Default.AppTheme == Theme.Dark) dark = true;

				// If the mode has changed, invoke the event to change it
				if(dark != s_darkmode)
				{
					s_darkmode = dark;
					Changed?.Invoke(typeof(ApplicationTheme), EventArgs.Empty);
				}
			}
		}

		//-------------------------------------------------------------------
		// Private Member Functions
		//-------------------------------------------------------------------

		// GetSystemTheme
		//
		// Gets the system-wide application theme
		private static Theme GetSystemTheme()
		{
			// On Windows 10 and above the icon should be appropriate for the system theme
			if(VersionHelper.IsWindows10OrGreater())
			{
				// NOTE: Assume one (light) here if the value is missing
				object value = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", 1);
				return ((value is int @int) && (@int == 0)) ? Theme.Dark : Theme.Light;
			}

			// Always default to light theme
			return Theme.Light;
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private static bool s_darkmode = false;
	}
}
