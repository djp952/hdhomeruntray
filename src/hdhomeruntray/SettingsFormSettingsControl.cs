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
using System.Drawing;
using System.Windows.Forms;

using zuki.hdhomeruntray.discovery;
using zuki.hdhomeruntray.Properties;

namespace zuki.hdhomeruntray
{
	//--------------------------------------------------------------------------
	// Class SettingsFormSettingsControl (internal)
	//
	// User control that implements the settings panel for the settngs form

	internal partial class SettingsFormSettingsControl : UserControl
	{
		// Instance Constructor
		//
		public SettingsFormSettingsControl()
		{
			InitializeComponent();

			m_layoutpanel.EnableDoubleBuferring();

			using(Graphics graphics = CreateGraphics())
			{
				Padding = Padding.ScaleDPI(graphics);
				m_layoutpanel.Margin = m_layoutpanel.Margin.ScaleDPI(graphics);
				m_layoutpanel.Padding = m_layoutpanel.Padding.ScaleDPI(graphics);
				m_layoutpanel.Radii = m_layoutpanel.Radii.ScaleDPI(graphics);

				// Scaling the margins of the comboboxes will adjust the row height
				m_autostart.Margin = m_autostart.Margin.ScaleDPI(graphics);
				m_discoveryinterval.Margin = m_discoveryinterval.Margin.ScaleDPI(graphics);
				m_discoverymethod.Margin = m_discoverymethod.Margin.ScaleDPI(graphics);
				m_trayiconhover.Margin = m_trayiconhover.Margin.ScaleDPI(graphics);
				m_trayiconhoverdelay.Margin = m_trayiconhoverdelay.Margin.ScaleDPI(graphics);
				m_tunerstatuscolorsource.Margin = m_tunerstatuscolorsource.Margin.ScaleDPI(graphics);
				m_unpinautomatically.Margin = m_unpinautomatically.Margin.ScaleDPI(graphics);
			}

			// WINDOWS 11
			//
			if(VersionHelper.IsWindows11OrGreater())
			{
				m_autostartlabel.Font = new Font("Segoe UI Variable Text Semibold", m_autostartlabel.Font.Size, m_autostartlabel.Font.Style);
				m_discoveryintervallabel.Font = new Font("Segoe UI Variable Text Semibold", m_discoveryintervallabel.Font.Size, m_discoveryintervallabel.Font.Style);
				m_discoverymethodlabel.Font = new Font("Segoe UI Variable Text Semibold", m_discoverymethodlabel.Font.Size, m_discoverymethodlabel.Font.Style);
				m_trayiconhoverlabel.Font = new Font("Segoe UI Variable Text Semibold", m_trayiconhoverlabel.Font.Size, m_trayiconhoverlabel.Font.Style);
				m_trayiconhoverdelaylabel.Font = new Font("Segoe UI Variable Text Semibold", m_trayiconhoverdelaylabel.Font.Size, m_trayiconhoverdelaylabel.Font.Style);
				m_tunerstatuscolorsourcelabel.Font = new Font("Segoe UI Variable Text Semibold", m_tunerstatuscolorsourcelabel.Font.Size, m_tunerstatuscolorsourcelabel.Font.Style);
				m_unpinautomaticallylabel.Font = new Font("Segoe UI Variable Text Semibold", m_unpinautomaticallylabel.Font.Size, m_unpinautomaticallylabel.Font.Style);

				m_autostart.Font = new Font("Segoe UI Variable Text", m_autostart.Font.Size, m_autostart.Font.Style);
				m_discoveryinterval.Font = new Font("Segoe UI Variable Text", m_discoveryinterval.Font.Size, m_discoveryinterval.Font.Style);
				m_discoverymethod.Font = new Font("Segoe UI Variable Text", m_discoverymethod.Font.Size, m_discoverymethod.Font.Style);
				m_trayiconhover.Font = new Font("Segoe UI Variable Text", m_trayiconhover.Font.Size, m_trayiconhover.Font.Style);
				m_trayiconhoverdelay.Font = new Font("Segoe UI Variable Text", m_trayiconhoverdelay.Font.Size, m_trayiconhoverdelay.Font.Style);
				m_tunerstatuscolorsource.Font = new Font("Segoe UI Variable Text", m_tunerstatuscolorsource.Font.Size, m_tunerstatuscolorsource.Font.Style);
				m_unpinautomatically.Font = new Font("Segoe UI Variable Text", m_unpinautomatically.Font.Size, m_unpinautomatically.Font.Style);
			}

			// Bind each of the ComboBox drop-downs to their enum class
			m_autostart.BindEnum(Settings.Default.AutoStart);
			m_discoveryinterval.BindEnum(Settings.Default.DiscoveryInterval);
			m_discoverymethod.BindEnum(Settings.Default.DiscoveryMethod);
			m_trayiconhover.BindEnum(Settings.Default.TrayIconHover);
			m_trayiconhoverdelay.BindEnum(Settings.Default.TrayIconHoverDelay);
			m_tunerstatuscolorsource.BindEnum(Settings.Default.TunerStatusColorSource);
			m_unpinautomatically.BindEnum(Settings.Default.AutoUnpin);
		}

		//-------------------------------------------------------------------
		// Event Handlers
		//-------------------------------------------------------------------

		// OnAutoStartCommitted
		//
		// Invoked when a change to the combobox is committed
		private void OnAutoStartCommitted(object sender, EventArgs args)
		{
			// If the value of the combobox changed, update and save the settings
			EnabledDisabled autostart = (EnabledDisabled)m_autostart.SelectedValue;
			if(autostart != Settings.Default.AutoStart)
			{
				Settings.Default.AutoStart = autostart;
				Settings.Default.Save();
			}
		}

		// OnDiscoveryIntervalCommitted
		//
		// Invoked when a change to the combobox is committed
		private void OnDiscoveryIntervalCommitted(object sender, EventArgs args)
		{
			// If the value of the combobox changed, update and save the settings
			DiscoveryInterval discoveryinterval = (DiscoveryInterval)m_discoveryinterval.SelectedValue;
			if(discoveryinterval != Settings.Default.DiscoveryInterval)
			{
				Settings.Default.DiscoveryInterval = discoveryinterval;
				Settings.Default.Save();
			}
		}

		// OnDiscoveryMethodCommitted
		//
		// Invoked when a change to the combobox is committed
		private void OnDiscoveryMethodCommitted(object sender, EventArgs args)
		{
			// If the value of the combobox changed, update and save the settings
			DiscoveryMethod discoverymethod = (DiscoveryMethod)m_discoverymethod.SelectedValue;
			if(discoverymethod != Settings.Default.DiscoveryMethod)
			{
				Settings.Default.DiscoveryMethod = discoverymethod;
				Settings.Default.Save();
			}
		}

		// OnTrayIconHoverCommitted
		//
		// Invoked when a change to the combobox is committed
		private void OnTrayIconHoverCommitted(object sender, EventArgs args)
		{
			// If the value of the combobox changed, update and save the settings
			EnabledDisabled trayiconhover = (EnabledDisabled)m_trayiconhover.SelectedValue;
			if(trayiconhover != Settings.Default.TrayIconHover)
			{
				Settings.Default.TrayIconHover = trayiconhover;
				Settings.Default.Save();
			}
		}

		// OnTrayIconHoverDelayCommitted
		//
		// Invoked when a change to the combobox is committed
		private void OnTrayIconHoverDelayCommitted(object sender, EventArgs args)
		{
			// If the value of the combobox changed, update and save the settings
			TrayIconHoverDelay trayiconhoverdelay = (TrayIconHoverDelay)m_trayiconhoverdelay.SelectedValue;
			if(trayiconhoverdelay != Settings.Default.TrayIconHoverDelay)
			{
				Settings.Default.TrayIconHoverDelay = trayiconhoverdelay;
				Settings.Default.Save();
			}
		}

		// OnTunerStatusColorSourceCommitted
		//
		// Invoked when a change to the combobox is committed
		private void OnTunerStatusColorSourceCommitted(object sender, EventArgs args)
		{
			// If the value of the combobox changed, update and save the settings
			TunerStatusColorSource tunerstatuscolorsource = (TunerStatusColorSource)m_tunerstatuscolorsource.SelectedValue;
			if(tunerstatuscolorsource != Settings.Default.TunerStatusColorSource)
			{
				Settings.Default.TunerStatusColorSource = tunerstatuscolorsource;
				Settings.Default.Save();
			}
		}

		// OnUnpinAutomaticallyCommitted
		//
		// Invoked when a change to the combobox is committed
		private void OnUnpinAutomaticallyCommitted(object sender, EventArgs args)
		{
			// If the value of the combobox changed, update and save the settings
			EnabledDisabled unpinautomatically = (EnabledDisabled)m_unpinautomatically.SelectedValue;
			if(unpinautomatically != Settings.Default.AutoUnpin)
			{
				Settings.Default.AutoUnpin = unpinautomatically;
				Settings.Default.Save();
			}
		}

	}
}
