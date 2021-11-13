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

using System;
using System.Windows.Forms;

using zuki.hdhomeruntray.discovery;
using zuki.hdhomeruntray.Properties;

namespace zuki.hdhomeruntray
{
	//-----------------------------------------------------------------------
	// Class MainFormSettingsPage
	//
	// Implements the user control that provides the means to control the
	// application settings

	internal partial class MainFormSettingsPage : UserControl
	{
		// Instance Constructor
		//
		public MainFormSettingsPage()
		{
			InitializeComponent();

			// Bind each of the ComboBox drop-downs to their enum class
			m_discoverymethod.BindEnum(Settings.Default.DiscoveryMethod);
			m_discoveryinterval.BindEnum(Settings.Default.DiscoveryInterval);
			m_trayiconhoverdelay.BindEnum(Settings.Default.TrayIconHoverDelay);
		}

		//-------------------------------------------------------------------
		// UserControl Overrides
		//-------------------------------------------------------------------

		// OnHandleDestroyed
		//
		// Invoked when the handle for the UserControl has been destroyed
		protected override void OnHandleDestroyed(EventArgs args)
		{
			bool save = false;				// Flag to save the settings changes

			// Pull out all of the values specified by the user while the control was active
			DiscoveryInterval discoveryinterval = (DiscoveryInterval)m_discoveryinterval.SelectedValue;
			DiscoveryMethod discoverymethod = (DiscoveryMethod)m_discoverymethod.SelectedValue;
			TrayIconHoverDelay trayiconhoverdelay = (TrayIconHoverDelay)m_trayiconhoverdelay.SelectedValue;

			// DiscoveryInterval
			if(discoveryinterval != Settings.Default.DiscoveryInterval)
			{
				Settings.Default.DiscoveryInterval = discoveryinterval;
				save = true;
			}

			// DiscoveryMethod
			if(discoverymethod != Settings.Default.DiscoveryMethod)
			{
				Settings.Default.DiscoveryMethod = discoverymethod;
				save = true;
			}

			// TrayIconHoverDelay
			if(trayiconhoverdelay != Settings.Default.TrayIconHoverDelay)
			{
				Settings.Default.TrayIconHoverDelay = trayiconhoverdelay;
				save = true;
			}

			if(save) Settings.Default.Save();		// Save any changes

			base.OnHandleDestroyed(args);
		}
	}
}
