
namespace zuki.hdhomeruntray
{
	partial class SettingsForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.Label m_tunerstatuscolorsourcelabel;
			System.Windows.Forms.Label m_hoverintervallabel;
			System.Windows.Forms.Label m_discoverymethodlabel;
			System.Windows.Forms.Label m_discoverintervallabel;
			this.m_layoutpanel = new System.Windows.Forms.TableLayoutPanel();
			this.m_tunerstatuscolorsource = new zuki.hdhomeruntray.AutoSizeComboBox();
			this.m_discoverymethod = new zuki.hdhomeruntray.AutoSizeComboBox();
			this.m_discoveryinterval = new zuki.hdhomeruntray.AutoSizeComboBox();
			this.m_trayiconhoverdelay = new zuki.hdhomeruntray.AutoSizeComboBox();
			m_tunerstatuscolorsourcelabel = new System.Windows.Forms.Label();
			m_hoverintervallabel = new System.Windows.Forms.Label();
			m_discoverymethodlabel = new System.Windows.Forms.Label();
			m_discoverintervallabel = new System.Windows.Forms.Label();
			this.m_layoutpanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_tunerstatuscolorsourcelabel
			// 
			m_tunerstatuscolorsourcelabel.AutoSize = true;
			m_tunerstatuscolorsourcelabel.Dock = System.Windows.Forms.DockStyle.Left;
			m_tunerstatuscolorsourcelabel.Location = new System.Drawing.Point(7, 91);
			m_tunerstatuscolorsourcelabel.Name = "m_tunerstatuscolorsourcelabel";
			m_tunerstatuscolorsourcelabel.Size = new System.Drawing.Size(144, 78);
			m_tunerstatuscolorsourcelabel.TabIndex = 8;
			m_tunerstatuscolorsourcelabel.Text = "Tuner Status Color Source";
			m_tunerstatuscolorsourcelabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_hoverintervallabel
			// 
			m_hoverintervallabel.AutoSize = true;
			m_hoverintervallabel.Dock = System.Windows.Forms.DockStyle.Left;
			m_hoverintervallabel.Location = new System.Drawing.Point(7, 62);
			m_hoverintervallabel.Name = "m_hoverintervallabel";
			m_hoverintervallabel.Size = new System.Drawing.Size(124, 29);
			m_hoverintervallabel.TabIndex = 6;
			m_hoverintervallabel.Text = "Tray Icon Hover Delay";
			m_hoverintervallabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_discoverymethodlabel
			// 
			m_discoverymethodlabel.AutoSize = true;
			m_discoverymethodlabel.Dock = System.Windows.Forms.DockStyle.Left;
			m_discoverymethodlabel.Location = new System.Drawing.Point(7, 33);
			m_discoverymethodlabel.Name = "m_discoverymethodlabel";
			m_discoverymethodlabel.Size = new System.Drawing.Size(104, 29);
			m_discoverymethodlabel.TabIndex = 2;
			m_discoverymethodlabel.Text = "Discovery Method";
			m_discoverymethodlabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_discoverintervallabel
			// 
			m_discoverintervallabel.AutoSize = true;
			m_discoverintervallabel.Dock = System.Windows.Forms.DockStyle.Left;
			m_discoverintervallabel.Location = new System.Drawing.Point(7, 4);
			m_discoverintervallabel.Name = "m_discoverintervallabel";
			m_discoverintervallabel.Size = new System.Drawing.Size(102, 29);
			m_discoverintervallabel.TabIndex = 0;
			m_discoverintervallabel.Text = "Discovery Interval";
			m_discoverintervallabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_layoutpanel
			// 
			this.m_layoutpanel.AutoSize = true;
			this.m_layoutpanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_layoutpanel.ColumnCount = 2;
			this.m_layoutpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.m_layoutpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.m_layoutpanel.Controls.Add(m_tunerstatuscolorsourcelabel, 0, 3);
			this.m_layoutpanel.Controls.Add(this.m_tunerstatuscolorsource, 1, 3);
			this.m_layoutpanel.Controls.Add(m_hoverintervallabel, 0, 2);
			this.m_layoutpanel.Controls.Add(this.m_discoverymethod, 1, 1);
			this.m_layoutpanel.Controls.Add(m_discoverymethodlabel, 0, 1);
			this.m_layoutpanel.Controls.Add(m_discoverintervallabel, 0, 0);
			this.m_layoutpanel.Controls.Add(this.m_discoveryinterval, 1, 0);
			this.m_layoutpanel.Controls.Add(this.m_trayiconhoverdelay, 1, 2);
			this.m_layoutpanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_layoutpanel.Location = new System.Drawing.Point(0, 0);
			this.m_layoutpanel.Name = "m_layoutpanel";
			this.m_layoutpanel.Padding = new System.Windows.Forms.Padding(4);
			this.m_layoutpanel.RowCount = 4;
			this.m_layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_layoutpanel.Size = new System.Drawing.Size(362, 173);
			this.m_layoutpanel.TabIndex = 1;
			// 
			// m_tunerstatuscolorsource
			// 
			this.m_tunerstatuscolorsource.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_tunerstatuscolorsource.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.m_tunerstatuscolorsource.Location = new System.Drawing.Point(157, 94);
			this.m_tunerstatuscolorsource.Name = "m_tunerstatuscolorsource";
			this.m_tunerstatuscolorsource.Size = new System.Drawing.Size(198, 23);
			this.m_tunerstatuscolorsource.TabIndex = 8;
			this.m_tunerstatuscolorsource.SelectionChangeCommitted += new System.EventHandler(this.OnTunerStatusColorSourceCommitted);
			// 
			// m_discoverymethod
			// 
			this.m_discoverymethod.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_discoverymethod.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.m_discoverymethod.Location = new System.Drawing.Point(157, 36);
			this.m_discoverymethod.Name = "m_discoverymethod";
			this.m_discoverymethod.Size = new System.Drawing.Size(198, 23);
			this.m_discoverymethod.TabIndex = 3;
			this.m_discoverymethod.SelectionChangeCommitted += new System.EventHandler(this.OnDiscoveryMethodCommitted);
			// 
			// m_discoveryinterval
			// 
			this.m_discoveryinterval.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_discoveryinterval.Location = new System.Drawing.Point(157, 7);
			this.m_discoveryinterval.Name = "m_discoveryinterval";
			this.m_discoveryinterval.Size = new System.Drawing.Size(198, 23);
			this.m_discoveryinterval.TabIndex = 1;
			this.m_discoveryinterval.SelectionChangeCommitted += new System.EventHandler(this.OnDiscoveryIntervalCommitted);
			// 
			// m_trayiconhoverdelay
			// 
			this.m_trayiconhoverdelay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_trayiconhoverdelay.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.m_trayiconhoverdelay.Location = new System.Drawing.Point(157, 65);
			this.m_trayiconhoverdelay.Name = "m_trayiconhoverdelay";
			this.m_trayiconhoverdelay.Size = new System.Drawing.Size(198, 23);
			this.m_trayiconhoverdelay.TabIndex = 7;
			this.m_trayiconhoverdelay.SelectionChangeCommitted += new System.EventHandler(this.OnTrayIconHoverDelayCommitted);
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(362, 173);
			this.ControlBox = false;
			this.Controls.Add(this.m_layoutpanel);
			this.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SettingsForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.TopMost = true;
			this.m_layoutpanel.ResumeLayout(false);
			this.m_layoutpanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel m_layoutpanel;
		private zuki.hdhomeruntray.AutoSizeComboBox m_tunerstatuscolorsource;
		private zuki.hdhomeruntray.AutoSizeComboBox m_discoverymethod;
		private zuki.hdhomeruntray.AutoSizeComboBox m_discoveryinterval;
		private zuki.hdhomeruntray.AutoSizeComboBox m_trayiconhoverdelay;
	}
}