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
using System.Net;
using System.Windows.Forms;

using zuki.hdhomeruntray.discovery;

namespace zuki.hdhomeruntray
{
	//--------------------------------------------------------------------------
	// Class TunerDeviceStatusControl (internal)
	//
	// User control that implements the status for a tuner device in the DeviceForm

	internal partial class TunerDeviceStatusControl : UserControl
	{
		// Instance Constructor
		//
		private TunerDeviceStatusControl(SizeF scalefactor)
		{
			InitializeComponent();

			m_layoutpanel.EnableDoubleBuffering();

			// THEME
			//
			m_appthemechanged = new EventHandler(OnApplicationThemeChanged);
			ApplicationTheme.Changed += m_appthemechanged;
			OnApplicationThemeChanged(this, EventArgs.Empty);

			// COLOR FILTER
			//
			m_statuscolorschanged = new EventHandler(OnStatusColorsChanged);
			StatusColor.Changed += m_statuscolorschanged;
			OnStatusColorsChanged(this, EventArgs.Empty);

			m_layoutpanel.SuspendLayout();
			m_signallayoutpanel.SuspendLayout();
			m_headerlayoutpanel.SuspendLayout();
			m_footerlayoutpanel.SuspendLayout();
			SuspendLayout();

			// The scaling factor needs to be saved as it's used dynamically below
			m_scalefactor = scalefactor;

			try
			{
				Padding = Padding.ScaleDPI(m_scalefactor);

				m_layoutpanel.Margin = m_layoutpanel.Margin.ScaleDPI(m_scalefactor);
				m_layoutpanel.Padding = m_layoutpanel.Padding.ScaleDPI(m_scalefactor);
				m_headerlayoutpanel.Margin = m_headerlayoutpanel.Margin.ScaleDPI(m_scalefactor);
				m_headerlayoutpanel.Padding = m_headerlayoutpanel.Padding.ScaleDPI(m_scalefactor);
				m_signallayoutpanel.Margin = m_signallayoutpanel.Margin.ScaleDPI(m_scalefactor);
				m_signallayoutpanel.Padding = m_signallayoutpanel.Padding.ScaleDPI(m_scalefactor);
				m_footerlayoutpanel.Margin = m_footerlayoutpanel.Margin.ScaleDPI(m_scalefactor);
				m_footerlayoutpanel.Padding = m_footerlayoutpanel.Padding.ScaleDPI(m_scalefactor);

				m_signalstrengthlabel.Padding = m_signalstrengthlabel.Padding.ScaleDPI(m_scalefactor);
				m_signalstrengthbar.Padding = m_signalstrengthbar.Padding.ScaleDPI(m_scalefactor);
				m_signalstrengthpct.Padding = m_signalstrengthpct.Padding.ScaleDPI(m_scalefactor);

				m_signalqualitylabel.Padding = m_signalqualitylabel.Padding.ScaleDPI(m_scalefactor);
				m_signalqualitybar.Padding = m_signalqualitybar.Padding.ScaleDPI(m_scalefactor);
				m_signalqualitypct.Padding = m_signalqualitypct.Padding.ScaleDPI(m_scalefactor);

				m_symbolqualitylabel.Padding = m_symbolqualitylabel.Padding.ScaleDPI(m_scalefactor);
				m_symbolqualitybar.Padding = m_symbolqualitybar.Padding.ScaleDPI(m_scalefactor);
				m_symbolqualitypct.Padding = m_symbolqualitypct.Padding.ScaleDPI(m_scalefactor);

				// Enforce a minimum size for the signal strength progress bars
				m_signalqualitybar.MinimumSize = m_signalqualitybar.MinimumSize = m_symbolqualitybar.MinimumSize =
					new Size((int)(m_signalqualitybar.MinimumSize.Width * m_scalefactor.Width), 
					(int)(m_signalqualitybar.MinimumSize.Height * m_scalefactor.Height));

				// These panels start out as invisible
				m_signallayoutpanel.Visible = false;
				m_footerlayoutpanel.Visible = false;

				// WINDOWS 11
				//
				if(VersionHelper.IsWindows11OrGreater())
				{
					m_tunernumber.Font = new Font("Segoe UI Variable Text Semibold", m_tunernumber.Font.Size, m_tunernumber.Font.Style);
					m_channelname.Font = new Font("Segoe UI Variable Text Semibold", m_channelname.Font.Size, m_channelname.Font.Style);
					m_signalstrengthlabel.Font = new Font("Segoe UI Variable Text", m_signalstrengthlabel.Font.Size, m_signalstrengthlabel.Font.Style);
					m_signalqualitylabel.Font = new Font("Segoe UI Variable Text", m_signalqualitylabel.Font.Size, m_signalqualitylabel.Font.Style);
					m_symbolqualitylabel.Font = new Font("Segoe UI Variable Text", m_symbolqualitylabel.Font.Size, m_symbolqualitylabel.Font.Style);
					m_signalstrengthpct.Font = new Font("Segoe UI Variable Text", m_signalstrengthpct.Font.Size, m_signalstrengthpct.Font.Style);
					m_signalqualitypct.Font = new Font("Segoe UI Variable Text", m_signalqualitypct.Font.Size, m_signalqualitypct.Font.Style);
					m_symbolqualitypct.Font = new Font("Segoe UI Variable Text", m_symbolqualitypct.Font.Size, m_symbolqualitypct.Font.Style);
					m_targetip.Font = new Font("Segoe UI Variable Text", m_targetip.Font.Size, m_targetip.Font.Style);
					m_bitrate.Font = new Font("Segoe UI Variable Text", m_bitrate.Font.Size, m_bitrate.Font.Style);
				}
			}

			finally
			{
				m_layoutpanel.ResumeLayout(false);
				m_layoutpanel.PerformLayout();
				m_signallayoutpanel.ResumeLayout(false);
				m_signallayoutpanel.PerformLayout();
				m_headerlayoutpanel.ResumeLayout(false);
				m_headerlayoutpanel.PerformLayout();
				m_footerlayoutpanel.ResumeLayout(false);
				m_footerlayoutpanel.PerformLayout();
				ResumeLayout(false);
				PerformLayout();
			}

			// Lock in the width of the percentage text controls to fit "100%"
			int pctwidth = m_signalstrengthpct.Width;
			m_signalstrengthpct.MinimumSize = new Size(pctwidth, 0);
			m_signalqualitypct.MinimumSize = new Size(pctwidth, 0);
			m_symbolqualitypct.MinimumSize = new Size(pctwidth, 0);
		}

		// Instance Constructor
		//
		public TunerDeviceStatusControl(TunerDevice device, Tuner tuner, SizeF scalefactor) : this(scalefactor)
		{
			m_device = device ?? throw new ArgumentNullException(nameof(device));
			m_tuner = tuner ?? throw new ArgumentNullException(nameof(tuner));

			// Tuner identifier is static once created
			m_tunernumber.Text = "Tuner " + m_tuner.Index.ToString();

			UpdateStatus();             // Initial status load
		}

		// Dispose
		//
		// Releases unmanaged resources and optionally releases managed resources
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				// Dispose managed state
				if(m_statuscolorschanged != null) StatusColor.Changed -= m_statuscolorschanged;
				if(m_appthemechanged != null) ApplicationTheme.Changed -= m_appthemechanged;
				if(components != null) components.Dispose();
			}

			base.Dispose(disposing);
		}

		//-------------------------------------------------------------------------
		// Events
		//-------------------------------------------------------------------------

		// DeviceStatusChanged
		//
		// Invoked when the status has changed
		public event DeviceStatusChangedEventHandler DeviceStatusChanged;

		//-------------------------------------------------------------------------
		// Member Functions
		//-------------------------------------------------------------------------

		// UpdateStatus
		//
		// Called to update the contents of the control
		public void UpdateStatus()
		{
			// Get updated status for the device and check against the last known hash
			TunerStatus status = m_device.GetTunerStatus(m_tuner);
			if(status.GetHashCode() == m_lasthash) return;

			// Save the updated hash code
			m_lasthash = status.GetHashCode();

			// Save the current "dot" color
			Color lastcolor = m_activedot.ForeColor;

			m_layoutpanel.SuspendLayout();

			try
			{
				// Header Controls
				//
				m_activedot.ForeColor = StatusColor.FromDeviceStatus(status.DeviceStatus);
				m_channelname.Text = (status.DeviceStatus >= DeviceStatus.Active) ? status.ChannelName.Replace("&", "&&") : "Idle";

				if(status.DeviceStatus >= DeviceStatus.Active)
				{
					// Signal Meter Controls
					//
					m_signalstrengthbar.ProgressBarColor = StatusColor.FromDeviceStatusColor(status.SignalStrengthColor);
					m_signalstrengthbar.Value = status.SignalStrength;
					m_signalstrengthpct.Text = string.Format("{0}%", status.SignalStrength);

					m_signalqualitybar.ProgressBarColor = StatusColor.FromDeviceStatusColor(status.SignalQualityColor);
					m_signalqualitybar.Value = status.SignalQuality;
					m_signalqualitypct.Text = string.Format("{0}%", status.SignalQuality);

					m_symbolqualitybar.ProgressBarColor = StatusColor.FromDeviceStatusColor(status.SymbolQualityColor);
					m_symbolqualitybar.Value = status.SymbolQuality;
					m_symbolqualitypct.Text = string.Format("{0}%", status.SymbolQuality);

					// Ensure the signal meter is visible for an active tuner
					if(!m_signallayoutpanel.Visible) m_signallayoutpanel.Visible = true;

					// Footer Controls
					//
					if(!status.TargetIP.Equals(IPAddress.None))
					{
						m_targetip.Text = status.TargetIP.ToString();
						m_bitrate.Text = FormatBitRate(status.BitRate);
						if(!m_footerlayoutpanel.Visible) m_footerlayoutpanel.Visible = true;
					}
					else if(m_footerlayoutpanel.Visible) m_footerlayoutpanel.Visible = false;

					// The padding of the signal layout pane may need to change based on footer visibility
					Padding newpadding = new Padding(0, 4, 0, (m_footerlayoutpanel.Visible) ? 4 : 0).ScaleDPI(m_scalefactor);
					if(!m_signallayoutpanel.Padding.Equals(newpadding)) m_signallayoutpanel.Padding = newpadding;
				}

				else
				{
					// Hide the signal meter and footer panels for inactive devices
					if(m_signallayoutpanel.Visible) m_signallayoutpanel.Visible = false;
					if(m_footerlayoutpanel.Visible) m_footerlayoutpanel.Visible = false;
				}
			}

			finally { m_layoutpanel.ResumeLayout(); }

			// Invoke the status color changed event if the color of the dot changed
			if(lastcolor != m_activedot.ForeColor)
			{
				DeviceStatusChanged?.Invoke(this, new DeviceStatusChangedEventArgs(status.DeviceStatus, m_device, m_tuner.Index));
			}
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

		// OnStatusColorsChanged
		//
		// Invoked when the application status colors have changed
		private void OnStatusColorsChanged(object sender, EventArgs args)
		{
			// Rebase the colors for the new color set
			m_activedot.ForeColor = StatusColor.Rebase(m_activedot.ForeColor);
			m_signalstrengthbar.ProgressBarColor = StatusColor.Rebase(m_signalstrengthbar.ProgressBarColor);
			m_signalqualitybar.ProgressBarColor = StatusColor.Rebase(m_signalqualitybar.ProgressBarColor);
			m_symbolqualitybar.ProgressBarColor = StatusColor.Rebase(m_symbolqualitybar.ProgressBarColor);
		}

		//-------------------------------------------------------------------------
		// Private Member Functions
		//-------------------------------------------------------------------------

		// FormatBitRate
		//
		// Formats a bit rate number
		private static string FormatBitRate(long bps)
		{
			const double Kbps = 1024;               // Kilobits/s
			const double Mbps = Kbps * Kbps;        // Megabits/s
			const double Gbps = Mbps * Kbps;        // Gigabits/s

			double value = bps;

			if(value >= Gbps) return string.Format("{0:N2} Gb/s", value / Gbps);
			else if(value >= Mbps) return string.Format("{0:N2} Mb/s", value / Mbps);
			else if(value >= Kbps) return string.Format("{0:N2} Kb/s", value / Kbps);

			return string.Format("{0} bps", bps);
		}

		//-------------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------------

		private readonly TunerDevice m_device;
		private readonly Tuner m_tuner;
		private int m_lasthash = 0;
		private readonly SizeF m_scalefactor = SizeF.Empty;
		private readonly EventHandler m_appthemechanged;
		private readonly EventHandler m_statuscolorschanged;
	}
}
