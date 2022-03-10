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

			// THEME
			//
			m_appthemechanged = new EventHandler(OnApplicationThemeChanged);
			ApplicationTheme.Changed += m_appthemechanged;
			OnApplicationThemeChanged(this, EventArgs.Empty);

			m_layoutpanel.EnableDoubleBuffering();

			using(Graphics graphics = CreateGraphics())
			{
				Padding = Padding.ScaleDPI(graphics);
				m_layoutpanel.Margin = m_layoutpanel.Margin.ScaleDPI(graphics);
				m_layoutpanel.Padding = m_layoutpanel.Padding.ScaleDPI(graphics);
				m_layoutpanel.Radii = m_layoutpanel.Radii.ScaleDPI(graphics);

				// Scaling the margins of the comboboxes will adjust the row height
				m_startautomatically.Margin = m_startautomatically.Margin.ScaleDPI(graphics);
				m_discoveryinterval.Margin = m_discoveryinterval.Margin.ScaleDPI(graphics);
				m_discoverymethod.Margin = m_discoverymethod.Margin.ScaleDPI(graphics);
				m_virtualledcolorset.Margin = m_virtualledcolorset.Margin.ScaleDPI(graphics);
				m_theme.Margin = m_theme.Margin.ScaleDPI(graphics);
				m_hoveractions.Margin = m_hoveractions.Margin.ScaleDPI(graphics);
				m_hoveractionsdelay.Margin = m_hoveractionsdelay.Margin.ScaleDPI(graphics);
				m_unpinautomatically.Margin = m_unpinautomatically.Margin.ScaleDPI(graphics);
			}

			// WINDOWS 11
			//
			if(VersionHelper.IsWindows11OrGreater())
			{
				m_startautomaticallylabel.Font = new Font("Segoe UI Variable Text Semibold", m_startautomaticallylabel.Font.Size, m_startautomaticallylabel.Font.Style);
				m_discoveryintervallabel.Font = new Font("Segoe UI Variable Text Semibold", m_discoveryintervallabel.Font.Size, m_discoveryintervallabel.Font.Style);
				m_discoverymethodlabel.Font = new Font("Segoe UI Variable Text Semibold", m_discoverymethodlabel.Font.Size, m_discoverymethodlabel.Font.Style);
				m_virtualledcolorsetlabel.Font = new Font("Segoe UI Variable Text Semibold", m_virtualledcolorsetlabel.Font.Size, m_virtualledcolorsetlabel.Font.Style);
				m_themelabel.Font = new Font("Segoe UI Variable Text Semibold", m_themelabel.Font.Size, m_themelabel.Font.Style);
				m_hoveractionslabel.Font = new Font("Segoe UI Variable Text Semibold", m_hoveractionslabel.Font.Size, m_hoveractionslabel.Font.Style);
				m_hoveractionsdelaylabel.Font = new Font("Segoe UI Variable Text Semibold", m_hoveractionsdelaylabel.Font.Size, m_hoveractionsdelaylabel.Font.Style);
				m_unpinautomaticallylabel.Font = new Font("Segoe UI Variable Text Semibold", m_unpinautomaticallylabel.Font.Size, m_unpinautomaticallylabel.Font.Style);

				m_startautomatically.Font = new Font("Segoe UI Variable Text", m_startautomatically.Font.Size, m_startautomatically.Font.Style);
				m_discoveryinterval.Font = new Font("Segoe UI Variable Text", m_discoveryinterval.Font.Size, m_discoveryinterval.Font.Style);
				m_discoverymethod.Font = new Font("Segoe UI Variable Text", m_discoverymethod.Font.Size, m_discoverymethod.Font.Style);
				m_virtualledcolorset.Font = new Font("Segoe UI Variable Text", m_virtualledcolorset.Font.Size, m_virtualledcolorset.Font.Style);
				m_theme.Font = new Font("Segoe UI Variable Text", m_theme.Font.Size, m_theme.Font.Style);
				m_hoveractions.Font = new Font("Segoe UI Variable Text", m_hoveractions.Font.Size, m_hoveractions.Font.Style);
				m_hoveractionsdelay.Font = new Font("Segoe UI Variable Text", m_hoveractionsdelay.Font.Size, m_hoveractionsdelay.Font.Style);
				m_unpinautomatically.Font = new Font("Segoe UI Variable Text", m_unpinautomatically.Font.Size, m_unpinautomatically.Font.Style);
			}

			// Bind each of the ComboBox drop-downs to their enum class
			m_startautomatically.BindEnum(Settings.Default.StartAutomatically);
			m_discoveryinterval.BindEnum(Settings.Default.DiscoveryInterval);
			m_discoverymethod.BindEnum(Settings.Default.DiscoveryMethod);
			m_virtualledcolorset.BindEnum(Settings.Default.VirtualLEDColorSet);
			m_theme.BindEnum(Settings.Default.Theme);
			m_hoveractions.BindEnum(Settings.Default.HoverActions);
			m_hoveractionsdelay.BindEnum(Settings.Default.HoverActionsDelay);
			m_unpinautomatically.BindEnum(Settings.Default.UnpinAutomatically);
		}

		// Dispose
		//
		// Releases unmanaged resources and optionally releases managed resources
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				// Dispose managed state
				if(m_appthemechanged != null) ApplicationTheme.Changed -= m_appthemechanged;
				if(components != null) components.Dispose();
			}

			base.Dispose(disposing);
		}

		//-------------------------------------------------------------------
		// Event Handlers
		//-------------------------------------------------------------------

		// OnApplicationThemeChanged
		//
		// Invoked when the application theme has changed
		private void OnApplicationThemeChanged(object sender, EventArgs args)
		{
			m_layoutpanel.BackColor = ApplicationTheme.PanelBackColor;
			m_layoutpanel.ForeColor = ApplicationTheme.PanelForeColor;
		}

		// OnAutoStartCommitted
		//
		// Invoked when a change to the combobox is committed
		private void OnAutoStartCommitted(object sender, EventArgs args)
		{
			// If the value of the combobox changed, update and save the settings
			EnabledDisabled autostart = (EnabledDisabled)m_startautomatically.SelectedValue;
			if(autostart != Settings.Default.StartAutomatically)
			{
				Settings.Default.StartAutomatically = autostart;
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

		// OnStatusColorSetCommitted
		//
		// Invoked when a change to the combobox is committed
		private void OnStatusColorSetCommitted(object sender, EventArgs args)
		{
			// If the value of the combobox changed, update and save the settings
			VirtualLEDColorSet colorset = (VirtualLEDColorSet)m_virtualledcolorset.SelectedValue;
			if(colorset != Settings.Default.VirtualLEDColorSet)
			{
				Settings.Default.VirtualLEDColorSet = colorset;
				Settings.Default.Save();
			}
		}

		// OnTrayIconHoverCommitted
		//
		// Invoked when a change to the combobox is committed
		private void OnTrayIconHoverCommitted(object sender, EventArgs args)
		{
			// If the value of the combobox changed, update and save the settings
			EnabledDisabled trayiconhover = (EnabledDisabled)m_hoveractions.SelectedValue;
			if(trayiconhover != Settings.Default.HoverActions)
			{
				Settings.Default.HoverActions = trayiconhover;
				Settings.Default.Save();
			}
		}

		// OnThemeCommitted
		//
		// Invoked when a change to the combobox is committed
		private void OnThemeCommitted(object sender, EventArgs args)
		{
			// If the value of the combobox changed, update and save the settings
			Theme theme = (Theme)m_theme.SelectedValue;
			if(theme != Settings.Default.Theme)
			{
				Settings.Default.Theme = theme;
				Settings.Default.Save();
			}
		}

		// OnTrayIconHoverDelayCommitted
		//
		// Invoked when a change to the combobox is committed
		private void OnTrayIconHoverDelayCommitted(object sender, EventArgs args)
		{
			// If the value of the combobox changed, update and save the settings
			HoverActionsDelay trayiconhoverdelay = (HoverActionsDelay)m_hoveractionsdelay.SelectedValue;
			if(trayiconhoverdelay != Settings.Default.HoverActionsDelay)
			{
				Settings.Default.HoverActionsDelay = trayiconhoverdelay;
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
			if(unpinautomatically != Settings.Default.UnpinAutomatically)
			{
				Settings.Default.UnpinAutomatically = unpinautomatically;
				Settings.Default.Save();
			}
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private readonly EventHandler m_appthemechanged;
	}
}
