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
	// Class TunerDeviceStatusControl (internal)
	//
	// User control that implements the status for a tuner device in the DeviceForm

	partial class TunerDeviceStatusControl : UserControl
	{
		// Instance Constructor
		//
		private TunerDeviceStatusControl()
		{
			InitializeComponent();

			Padding = Padding.ScaleDPI(Handle);

			m_layoutpanel.Margin = m_layoutpanel.Margin.ScaleDPI(Handle);
			m_layoutpanel.Padding = m_layoutpanel.Padding.ScaleDPI(Handle);
			m_headerlayoutpanel.Margin = m_headerlayoutpanel.Margin.ScaleDPI(Handle);
			m_headerlayoutpanel.Padding = m_headerlayoutpanel.Padding.ScaleDPI(Handle);
			m_signallayoutpanel.Margin = m_signallayoutpanel.Margin.ScaleDPI(Handle);
			m_signallayoutpanel.Padding = m_signallayoutpanel.Padding.ScaleDPI(Handle);
			m_footerlayoutpanel.Margin = m_footerlayoutpanel.Margin.ScaleDPI(Handle);
			m_footerlayoutpanel.Padding = m_footerlayoutpanel.Padding.ScaleDPI(Handle);

			// WINDOWS 11
			//
			if(VersionHelper.IsWindows11OrGreater())
			{
				m_tunernumber.Font = new Font("Segoe UI Variable Text Semibold", m_tunernumber.Font.Size, m_tunernumber.Font.Style);
				m_channel.Font = new Font("Segoe UI Variable Text Semibold", m_channel.Font.Size, m_channel.Font.Style);
				m_signalstrengthlabel.Font = new Font("Segoe UI Variable Text", m_signalstrengthlabel.Font.Size, m_signalstrengthlabel.Font.Style);
				m_signalqualitylabel.Font = new Font("Segoe UI Variable Small", m_signalqualitylabel.Font.Size, m_signalqualitylabel.Font.Style);
				m_symbolqualitylabel.Font = new Font("Segoe UI Variable Text", m_symbolqualitylabel.Font.Size, m_symbolqualitylabel.Font.Style);
				m_signalstrengthpct.Font = new Font("Segoe UI Variable Text", m_signalstrengthpct.Font.Size, m_signalstrengthpct.Font.Style);
				m_signalqualitypct.Font = new Font("Segoe UI Variable Text", m_signalqualitypct.Font.Size, m_signalqualitypct.Font.Style);
				m_symbolqualitypct.Font = new Font("Segoe UI Variable Text", m_symbolqualitypct.Font.Size, m_symbolqualitypct.Font.Style);
				m_targetip.Font = new Font("Segoe UI Variable Text", m_targetip.Font.Size, m_targetip.Font.Style);
				m_bitrate.Font = new Font("Segoe UI Variable Text", m_bitrate.Font.Size, m_bitrate.Font.Style);
			}
		}

		// Instance Constructor
		//
		public TunerDeviceStatusControl(TunerDevice device, Tuner tuner) : this()
		{
			m_device = device ?? throw new ArgumentNullException(nameof(device));
			m_tuner = tuner ?? throw new ArgumentNullException(nameof(tuner));

			// Tuner identifier is static once created
			m_tunernumber.Text = "Tuner " + m_tuner.Index.ToString(); 
			
			UpdateStatus();				// Initial status load
		}

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

			m_layoutpanel.SuspendLayout();

			try
			{
				// Header Controls
				//
				Color forecolor = (status.IsActive) ? DeviceStatusColor.Green : DeviceStatusColor.Gray;
				switch(Settings.Default.TunerStatusColorSource)
				{
					case TunerStatusColorSource.SignalStrength:
						forecolor = status.SignalStrengthColor;
						break;

					case TunerStatusColorSource.SignalQuality:
						forecolor = status.SignalQualityColor;
						break;

					case TunerStatusColorSource.SymbolQuality:
						forecolor = status.SymbolQualityColor;
						break;
				}
				m_activedot.ForeColor = forecolor;
				
				if(status.IsActive)
				{
					if(status.HasVirtualChannel) m_channel.Text = status.VirtualChannelNumber + " " + status.VirtualChannelName;
					else m_channel.Text = status.Channel;
				}
				else m_channel.Text = "Idle";

				if(status.IsActive)
				{
					// Signal Meter Controls
					//
					m_signalstrengthbar.ProgressBarColor = status.SignalStrengthColor;
					m_signalstrengthbar.Value = (status.IsActive) ? status.SignalStrength : 100;
					m_signalstrengthpct.Text = String.Format("{0}%", status.SignalStrength);

					m_signalqualitybar.ProgressBarColor = status.SignalQualityColor;
					m_signalqualitybar.Value = (status.IsActive) ? status.SignalQuality : 100;
					m_signalqualitypct.Text = String.Format("{0}%", status.SignalQuality);

					m_symbolqualitybar.ProgressBarColor = status.SymbolQualityColor;
					m_symbolqualitybar.Value = (status.IsActive) ? status.SymbolQuality : 100;
					m_symbolqualitypct.Text = String.Format("{0}%", status.SymbolQuality);

					// Footer Controls
					//
					m_targetip.Text = String.Empty;     // TODO: Not sure I want to include this
					m_bitrate.Text = FormatBitRate(status.BitRate);

					// Ensure the signal meter and footer are visible for an active tuner
					if(!m_signallayoutpanel.Visible) m_signallayoutpanel.Visible = true;
					if(!m_footerlayoutpanel.Visible) m_footerlayoutpanel.Visible = true;
				}

				else
				{
					// Hide the signal meter and footer panels for inactive devices
					if(m_signallayoutpanel.Visible) m_signallayoutpanel.Visible = false;
					if(m_footerlayoutpanel.Visible) m_footerlayoutpanel.Visible = false;
				}
			}

			finally { m_layoutpanel.ResumeLayout(); }
		}

		//-------------------------------------------------------------------------
		// Private Member Functions
		//-------------------------------------------------------------------------

		// FormatBitRate
		//
		// Formats a bit rate number
		private static string FormatBitRate(long bps)
		{
			const double Kbps = 1024;				// Kilobits/s
			const double Mbps = Kbps * Kbps;		// Megabits/s
			const double Gbps = Mbps * Kbps;		// Gigabits/s

			double value = bps;

			if(value >= Gbps) return String.Format("{0:N2} Gb/s", value / Gbps);
			else if(value >= Mbps) return String.Format("{0:N2} Mb/s", value / Mbps);
			else if(value >= Kbps) return String.Format("{0:N2} Kb/s", value / Kbps);

			return String.Format("{0} bps", bps);
		}

		//-------------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------------

		private readonly TunerDevice m_device;
		private readonly Tuner m_tuner;
		private int m_lasthash = 0;
	}
}
