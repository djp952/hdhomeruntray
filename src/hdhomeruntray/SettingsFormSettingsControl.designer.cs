
namespace zuki.hdhomeruntray
{
	partial class SettingsFormSettingsControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.m_layoutpanel = new zuki.hdhomeruntray.RoundedTableLayoutPanel();
			this.m_autostartlabel = new System.Windows.Forms.Label();
			this.m_discoveryintervallabel = new System.Windows.Forms.Label();
			this.m_discoverymethodlabel = new System.Windows.Forms.Label();
			this.m_trayiconhoverdelaylabel = new System.Windows.Forms.Label();
			this.m_tunerstatuscolorsourcelabel = new System.Windows.Forms.Label();
			this.m_autostart = new zuki.hdhomeruntray.AutoSizeComboBox();
			this.m_discoveryinterval = new zuki.hdhomeruntray.AutoSizeComboBox();
			this.m_discoverymethod = new zuki.hdhomeruntray.AutoSizeComboBox();
			this.m_trayiconhoverdelay = new zuki.hdhomeruntray.AutoSizeComboBox();
			this.m_tunerstatuscolorsource = new zuki.hdhomeruntray.AutoSizeComboBox();
			this.m_layoutpanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_layoutpanel
			// 
			this.m_layoutpanel.AutoSize = true;
			this.m_layoutpanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_layoutpanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.m_layoutpanel.ColumnCount = 2;
			this.m_layoutpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.m_layoutpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.m_layoutpanel.Controls.Add(this.m_autostartlabel, 0, 0);
			this.m_layoutpanel.Controls.Add(this.m_discoveryintervallabel, 0, 1);
			this.m_layoutpanel.Controls.Add(this.m_discoverymethodlabel, 0, 2);
			this.m_layoutpanel.Controls.Add(this.m_trayiconhoverdelaylabel, 0, 3);
			this.m_layoutpanel.Controls.Add(this.m_tunerstatuscolorsourcelabel, 0, 4);
			this.m_layoutpanel.Controls.Add(this.m_autostart, 1, 0);
			this.m_layoutpanel.Controls.Add(this.m_discoveryinterval, 1, 1);
			this.m_layoutpanel.Controls.Add(this.m_discoverymethod, 1, 2);
			this.m_layoutpanel.Controls.Add(this.m_trayiconhoverdelay, 1, 3);
			this.m_layoutpanel.Controls.Add(this.m_tunerstatuscolorsource, 1, 4);
			this.m_layoutpanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_layoutpanel.Location = new System.Drawing.Point(0, 0);
			this.m_layoutpanel.Name = "m_layoutpanel";
			this.m_layoutpanel.Padding = new System.Windows.Forms.Padding(4);
			this.m_layoutpanel.RowCount = 5;
			this.m_layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_layoutpanel.Size = new System.Drawing.Size(297, 177);
			this.m_layoutpanel.TabIndex = 0;
			// 
			// m_autostartlabel
			// 
			this.m_autostartlabel.AutoSize = true;
			this.m_autostartlabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_autostartlabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_autostartlabel.Location = new System.Drawing.Point(7, 4);
			this.m_autostartlabel.Name = "m_autostartlabel";
			this.m_autostartlabel.Size = new System.Drawing.Size(144, 33);
			this.m_autostartlabel.TabIndex = 0;
			this.m_autostartlabel.Text = "Automatically Start";
			this.m_autostartlabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_discoveryintervallabel
			// 
			this.m_discoveryintervallabel.AutoSize = true;
			this.m_discoveryintervallabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_discoveryintervallabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_discoveryintervallabel.Location = new System.Drawing.Point(7, 37);
			this.m_discoveryintervallabel.Name = "m_discoveryintervallabel";
			this.m_discoveryintervallabel.Size = new System.Drawing.Size(144, 34);
			this.m_discoveryintervallabel.TabIndex = 1;
			this.m_discoveryintervallabel.Text = "Device Discovery Interval";
			this.m_discoveryintervallabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_discoverymethodlabel
			// 
			this.m_discoverymethodlabel.AutoSize = true;
			this.m_discoverymethodlabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_discoverymethodlabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_discoverymethodlabel.Location = new System.Drawing.Point(7, 71);
			this.m_discoverymethodlabel.Name = "m_discoverymethodlabel";
			this.m_discoverymethodlabel.Size = new System.Drawing.Size(144, 34);
			this.m_discoverymethodlabel.TabIndex = 2;
			this.m_discoverymethodlabel.Text = "Device Discovery Method";
			this.m_discoverymethodlabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_trayiconhoverdelaylabel
			// 
			this.m_trayiconhoverdelaylabel.AutoSize = true;
			this.m_trayiconhoverdelaylabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_trayiconhoverdelaylabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_trayiconhoverdelaylabel.Location = new System.Drawing.Point(7, 105);
			this.m_trayiconhoverdelaylabel.Name = "m_trayiconhoverdelaylabel";
			this.m_trayiconhoverdelaylabel.Size = new System.Drawing.Size(144, 34);
			this.m_trayiconhoverdelaylabel.TabIndex = 3;
			this.m_trayiconhoverdelaylabel.Text = "Tray Icon Hover Delay";
			this.m_trayiconhoverdelaylabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_tunerstatuscolorsourcelabel
			// 
			this.m_tunerstatuscolorsourcelabel.AutoSize = true;
			this.m_tunerstatuscolorsourcelabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_tunerstatuscolorsourcelabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_tunerstatuscolorsourcelabel.Location = new System.Drawing.Point(7, 139);
			this.m_tunerstatuscolorsourcelabel.Name = "m_tunerstatuscolorsourcelabel";
			this.m_tunerstatuscolorsourcelabel.Size = new System.Drawing.Size(144, 34);
			this.m_tunerstatuscolorsourcelabel.TabIndex = 4;
			this.m_tunerstatuscolorsourcelabel.Text = "Tuner Status Color Source";
			this.m_tunerstatuscolorsourcelabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_autostart
			// 
			this.m_autostart.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_autostart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_autostart.FormattingEnabled = true;
			this.m_autostart.Location = new System.Drawing.Point(158, 7);
			this.m_autostart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 7);
			this.m_autostart.Name = "m_autostart";
			this.m_autostart.Size = new System.Drawing.Size(131, 23);
			this.m_autostart.TabIndex = 5;
			this.m_autostart.SelectionChangeCommitted += new System.EventHandler(this.OnAutoStartCommitted);
			// 
			// m_discoveryinterval
			// 
			this.m_discoveryinterval.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_discoveryinterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_discoveryinterval.FormattingEnabled = true;
			this.m_discoveryinterval.Location = new System.Drawing.Point(158, 41);
			this.m_discoveryinterval.Margin = new System.Windows.Forms.Padding(4, 4, 4, 7);
			this.m_discoveryinterval.Name = "m_discoveryinterval";
			this.m_discoveryinterval.Size = new System.Drawing.Size(131, 23);
			this.m_discoveryinterval.TabIndex = 6;
			this.m_discoveryinterval.SelectionChangeCommitted += new System.EventHandler(this.OnDiscoveryIntervalCommitted);
			// 
			// m_discoverymethod
			// 
			this.m_discoverymethod.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_discoverymethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_discoverymethod.FormattingEnabled = true;
			this.m_discoverymethod.Location = new System.Drawing.Point(158, 75);
			this.m_discoverymethod.Margin = new System.Windows.Forms.Padding(4, 4, 4, 7);
			this.m_discoverymethod.Name = "m_discoverymethod";
			this.m_discoverymethod.Size = new System.Drawing.Size(131, 23);
			this.m_discoverymethod.TabIndex = 7;
			this.m_discoverymethod.SelectionChangeCommitted += new System.EventHandler(this.OnDiscoveryMethodCommitted);
			// 
			// m_trayiconhoverdelay
			// 
			this.m_trayiconhoverdelay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_trayiconhoverdelay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_trayiconhoverdelay.FormattingEnabled = true;
			this.m_trayiconhoverdelay.Location = new System.Drawing.Point(158, 109);
			this.m_trayiconhoverdelay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 7);
			this.m_trayiconhoverdelay.Name = "m_trayiconhoverdelay";
			this.m_trayiconhoverdelay.Size = new System.Drawing.Size(131, 23);
			this.m_trayiconhoverdelay.TabIndex = 8;
			this.m_trayiconhoverdelay.SelectionChangeCommitted += new System.EventHandler(this.OnTrayIconHoverDelayCommitted);
			// 
			// m_tunerstatuscolorsource
			// 
			this.m_tunerstatuscolorsource.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_tunerstatuscolorsource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_tunerstatuscolorsource.FormattingEnabled = true;
			this.m_tunerstatuscolorsource.Location = new System.Drawing.Point(158, 143);
			this.m_tunerstatuscolorsource.Margin = new System.Windows.Forms.Padding(4, 4, 4, 7);
			this.m_tunerstatuscolorsource.Name = "m_tunerstatuscolorsource";
			this.m_tunerstatuscolorsource.Size = new System.Drawing.Size(131, 23);
			this.m_tunerstatuscolorsource.TabIndex = 9;
			this.m_tunerstatuscolorsource.SelectionChangeCommitted += new System.EventHandler(this.OnTunerStatusColorSourceCommitted);
			// 
			// SettingsFormSettingsControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.m_layoutpanel);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "SettingsFormSettingsControl";
			this.Size = new System.Drawing.Size(297, 177);
			this.m_layoutpanel.ResumeLayout(false);
			this.m_layoutpanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private RoundedTableLayoutPanel m_layoutpanel;
		private System.Windows.Forms.Label m_autostartlabel;
		private System.Windows.Forms.Label m_discoveryintervallabel;
		private System.Windows.Forms.Label m_discoverymethodlabel;
		private System.Windows.Forms.Label m_trayiconhoverdelaylabel;
		private System.Windows.Forms.Label m_tunerstatuscolorsourcelabel;
		private AutoSizeComboBox m_autostart;
		private AutoSizeComboBox m_discoveryinterval;
		private AutoSizeComboBox m_discoverymethod;
		private AutoSizeComboBox m_trayiconhoverdelay;
		private AutoSizeComboBox m_tunerstatuscolorsource;
	}
}
