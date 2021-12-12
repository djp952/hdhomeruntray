
namespace zuki.hdhomeruntray
{
	partial class TunerDeviceStatusControl
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
			this.m_signallayoutpanel = new System.Windows.Forms.TableLayoutPanel();
			this.m_signalstrengthlabel = new System.Windows.Forms.Label();
			this.m_signalqualitylabel = new System.Windows.Forms.Label();
			this.m_symbolqualitylabel = new System.Windows.Forms.Label();
			this.m_signalstrengthbar = new zuki.hdhomeruntray.SignalStatusProcessBar();
			this.m_signalqualitybar = new zuki.hdhomeruntray.SignalStatusProcessBar();
			this.m_symbolqualitybar = new zuki.hdhomeruntray.SignalStatusProcessBar();
			this.m_signalstrengthpct = new System.Windows.Forms.Label();
			this.m_signalqualitypct = new System.Windows.Forms.Label();
			this.m_symbolqualitypct = new System.Windows.Forms.Label();
			this.m_headerlayoutpanel = new System.Windows.Forms.TableLayoutPanel();
			this.m_tunernumber = new System.Windows.Forms.Label();
			this.m_channelname = new System.Windows.Forms.Label();
			this.m_activedot = new System.Windows.Forms.Label();
			this.m_footerlayoutpanel = new System.Windows.Forms.TableLayoutPanel();
			this.m_targetip = new System.Windows.Forms.Label();
			this.m_bitrate = new System.Windows.Forms.Label();
			this.m_layoutpanel.SuspendLayout();
			this.m_signallayoutpanel.SuspendLayout();
			this.m_headerlayoutpanel.SuspendLayout();
			this.m_footerlayoutpanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_layoutpanel
			// 
			this.m_layoutpanel.AutoSize = true;
			this.m_layoutpanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_layoutpanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.m_layoutpanel.ColumnCount = 1;
			this.m_layoutpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.m_layoutpanel.Controls.Add(this.m_signallayoutpanel, 0, 1);
			this.m_layoutpanel.Controls.Add(this.m_headerlayoutpanel, 0, 0);
			this.m_layoutpanel.Controls.Add(this.m_footerlayoutpanel, 0, 2);
			this.m_layoutpanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_layoutpanel.Location = new System.Drawing.Point(0, 0);
			this.m_layoutpanel.Margin = new System.Windows.Forms.Padding(0);
			this.m_layoutpanel.Name = "m_layoutpanel";
			this.m_layoutpanel.Padding = new System.Windows.Forms.Padding(4);
			this.m_layoutpanel.RowCount = 3;
			this.m_layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_layoutpanel.Size = new System.Drawing.Size(299, 127);
			this.m_layoutpanel.TabIndex = 2;
			// 
			// m_signallayoutpanel
			// 
			this.m_signallayoutpanel.AutoSize = true;
			this.m_signallayoutpanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_signallayoutpanel.ColumnCount = 3;
			this.m_signallayoutpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.m_signallayoutpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
			this.m_signallayoutpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.m_signallayoutpanel.Controls.Add(this.m_signalstrengthlabel, 0, 0);
			this.m_signallayoutpanel.Controls.Add(this.m_signalqualitylabel, 0, 1);
			this.m_signallayoutpanel.Controls.Add(this.m_symbolqualitylabel, 0, 2);
			this.m_signallayoutpanel.Controls.Add(this.m_signalstrengthbar, 1, 0);
			this.m_signallayoutpanel.Controls.Add(this.m_signalqualitybar, 1, 1);
			this.m_signallayoutpanel.Controls.Add(this.m_symbolqualitybar, 1, 2);
			this.m_signallayoutpanel.Controls.Add(this.m_signalstrengthpct, 2, 0);
			this.m_signallayoutpanel.Controls.Add(this.m_signalqualitypct, 2, 1);
			this.m_signallayoutpanel.Controls.Add(this.m_symbolqualitypct, 2, 2);
			this.m_signallayoutpanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_signallayoutpanel.Location = new System.Drawing.Point(7, 28);
			this.m_signallayoutpanel.Name = "m_signallayoutpanel";
			this.m_signallayoutpanel.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
			this.m_signallayoutpanel.RowCount = 3;
			this.m_signallayoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_signallayoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_signallayoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_signallayoutpanel.Size = new System.Drawing.Size(285, 71);
			this.m_signallayoutpanel.TabIndex = 1;
			// 
			// m_signalstrengthlabel
			// 
			this.m_signalstrengthlabel.AutoSize = true;
			this.m_signalstrengthlabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_signalstrengthlabel.Location = new System.Drawing.Point(3, 4);
			this.m_signalstrengthlabel.Name = "m_signalstrengthlabel";
			this.m_signalstrengthlabel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.m_signalstrengthlabel.Size = new System.Drawing.Size(88, 21);
			this.m_signalstrengthlabel.TabIndex = 0;
			this.m_signalstrengthlabel.Text = "Signal Strength";
			this.m_signalstrengthlabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_signalqualitylabel
			// 
			this.m_signalqualitylabel.AutoSize = true;
			this.m_signalqualitylabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_signalqualitylabel.Location = new System.Drawing.Point(3, 25);
			this.m_signalqualitylabel.Name = "m_signalqualitylabel";
			this.m_signalqualitylabel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.m_signalqualitylabel.Size = new System.Drawing.Size(88, 21);
			this.m_signalqualitylabel.TabIndex = 1;
			this.m_signalqualitylabel.Text = "Signal Quality";
			this.m_signalqualitylabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_symbolqualitylabel
			// 
			this.m_symbolqualitylabel.AutoSize = true;
			this.m_symbolqualitylabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_symbolqualitylabel.Location = new System.Drawing.Point(3, 46);
			this.m_symbolqualitylabel.Name = "m_symbolqualitylabel";
			this.m_symbolqualitylabel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.m_symbolqualitylabel.Size = new System.Drawing.Size(88, 21);
			this.m_symbolqualitylabel.TabIndex = 2;
			this.m_symbolqualitylabel.Text = "Symbol Quality";
			this.m_symbolqualitylabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_signalstrengthbar
			// 
			this.m_signalstrengthbar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_signalstrengthbar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_signalstrengthbar.Location = new System.Drawing.Point(97, 7);
			this.m_signalstrengthbar.Maximum = 100;
			this.m_signalstrengthbar.Minimum = 0;
			this.m_signalstrengthbar.Name = "m_signalstrengthbar";
			this.m_signalstrengthbar.Padding = new System.Windows.Forms.Padding(0, 2, 0, 1);
			this.m_signalstrengthbar.ProgressBarColor = System.Drawing.SystemColors.ControlLight;
			this.m_signalstrengthbar.Size = new System.Drawing.Size(144, 15);
			this.m_signalstrengthbar.TabIndex = 3;
			this.m_signalstrengthbar.Value = 0;
			// 
			// m_signalqualitybar
			// 
			this.m_signalqualitybar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_signalqualitybar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_signalqualitybar.Location = new System.Drawing.Point(97, 28);
			this.m_signalqualitybar.Maximum = 100;
			this.m_signalqualitybar.Minimum = 0;
			this.m_signalqualitybar.Name = "m_signalqualitybar";
			this.m_signalqualitybar.Padding = new System.Windows.Forms.Padding(0, 2, 0, 1);
			this.m_signalqualitybar.ProgressBarColor = System.Drawing.SystemColors.ControlLight;
			this.m_signalqualitybar.Size = new System.Drawing.Size(144, 15);
			this.m_signalqualitybar.TabIndex = 4;
			this.m_signalqualitybar.Value = 0;
			// 
			// m_symbolqualitybar
			// 
			this.m_symbolqualitybar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_symbolqualitybar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_symbolqualitybar.Location = new System.Drawing.Point(97, 49);
			this.m_symbolqualitybar.Maximum = 100;
			this.m_symbolqualitybar.Minimum = 0;
			this.m_symbolqualitybar.Name = "m_symbolqualitybar";
			this.m_symbolqualitybar.Padding = new System.Windows.Forms.Padding(0, 2, 0, 1);
			this.m_symbolqualitybar.ProgressBarColor = System.Drawing.SystemColors.ControlLight;
			this.m_symbolqualitybar.Size = new System.Drawing.Size(144, 15);
			this.m_symbolqualitybar.TabIndex = 5;
			this.m_symbolqualitybar.Value = 0;
			// 
			// m_signalstrengthpct
			// 
			this.m_signalstrengthpct.AutoSize = true;
			this.m_signalstrengthpct.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_signalstrengthpct.Location = new System.Drawing.Point(247, 4);
			this.m_signalstrengthpct.Name = "m_signalstrengthpct";
			this.m_signalstrengthpct.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.m_signalstrengthpct.Size = new System.Drawing.Size(35, 21);
			this.m_signalstrengthpct.TabIndex = 6;
			this.m_signalstrengthpct.Text = "100%";
			this.m_signalstrengthpct.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_signalqualitypct
			// 
			this.m_signalqualitypct.AutoSize = true;
			this.m_signalqualitypct.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_signalqualitypct.Location = new System.Drawing.Point(247, 25);
			this.m_signalqualitypct.Name = "m_signalqualitypct";
			this.m_signalqualitypct.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.m_signalqualitypct.Size = new System.Drawing.Size(35, 21);
			this.m_signalqualitypct.TabIndex = 7;
			this.m_signalqualitypct.Text = "100%";
			this.m_signalqualitypct.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_symbolqualitypct
			// 
			this.m_symbolqualitypct.AutoSize = true;
			this.m_symbolqualitypct.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_symbolqualitypct.Location = new System.Drawing.Point(247, 46);
			this.m_symbolqualitypct.Name = "m_symbolqualitypct";
			this.m_symbolqualitypct.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.m_symbolqualitypct.Size = new System.Drawing.Size(35, 21);
			this.m_symbolqualitypct.TabIndex = 8;
			this.m_symbolqualitypct.Text = "100%";
			this.m_symbolqualitypct.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_headerlayoutpanel
			// 
			this.m_headerlayoutpanel.AutoSize = true;
			this.m_headerlayoutpanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_headerlayoutpanel.ColumnCount = 3;
			this.m_headerlayoutpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.m_headerlayoutpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.m_headerlayoutpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.m_headerlayoutpanel.Controls.Add(this.m_tunernumber, 1, 0);
			this.m_headerlayoutpanel.Controls.Add(this.m_channelname, 2, 0);
			this.m_headerlayoutpanel.Controls.Add(this.m_activedot, 0, 0);
			this.m_headerlayoutpanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_headerlayoutpanel.Location = new System.Drawing.Point(7, 7);
			this.m_headerlayoutpanel.Name = "m_headerlayoutpanel";
			this.m_headerlayoutpanel.RowCount = 1;
			this.m_headerlayoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_headerlayoutpanel.Size = new System.Drawing.Size(285, 15);
			this.m_headerlayoutpanel.TabIndex = 3;
			// 
			// m_tunernumber
			// 
			this.m_tunernumber.AutoSize = true;
			this.m_tunernumber.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_tunernumber.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_tunernumber.Location = new System.Drawing.Point(26, 0);
			this.m_tunernumber.Name = "m_tunernumber";
			this.m_tunernumber.Size = new System.Drawing.Size(107, 15);
			this.m_tunernumber.TabIndex = 1;
			this.m_tunernumber.Text = "{ m_tunernumber }";
			this.m_tunernumber.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// m_channelname
			// 
			this.m_channelname.AutoSize = true;
			this.m_channelname.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_channelname.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_channelname.Location = new System.Drawing.Point(139, 0);
			this.m_channelname.Name = "m_channelname";
			this.m_channelname.Size = new System.Drawing.Size(143, 15);
			this.m_channelname.TabIndex = 2;
			this.m_channelname.Text = "{ m_channelname }";
			this.m_channelname.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// m_activedot
			// 
			this.m_activedot.AutoSize = true;
			this.m_activedot.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_activedot.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_activedot.ForeColor = System.Drawing.SystemColors.GrayText;
			this.m_activedot.Location = new System.Drawing.Point(3, 0);
			this.m_activedot.Name = "m_activedot";
			this.m_activedot.Size = new System.Drawing.Size(17, 15);
			this.m_activedot.TabIndex = 3;
			this.m_activedot.Text = "●";
			this.m_activedot.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// m_footerlayoutpanel
			// 
			this.m_footerlayoutpanel.AutoSize = true;
			this.m_footerlayoutpanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_footerlayoutpanel.ColumnCount = 2;
			this.m_footerlayoutpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.m_footerlayoutpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.m_footerlayoutpanel.Controls.Add(this.m_targetip, 0, 0);
			this.m_footerlayoutpanel.Controls.Add(this.m_bitrate, 1, 0);
			this.m_footerlayoutpanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_footerlayoutpanel.Location = new System.Drawing.Point(7, 105);
			this.m_footerlayoutpanel.Name = "m_footerlayoutpanel";
			this.m_footerlayoutpanel.RowCount = 1;
			this.m_footerlayoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_footerlayoutpanel.Size = new System.Drawing.Size(285, 15);
			this.m_footerlayoutpanel.TabIndex = 4;
			// 
			// m_targetip
			// 
			this.m_targetip.AutoSize = true;
			this.m_targetip.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_targetip.Location = new System.Drawing.Point(3, 0);
			this.m_targetip.Name = "m_targetip";
			this.m_targetip.Size = new System.Drawing.Size(78, 15);
			this.m_targetip.TabIndex = 4;
			this.m_targetip.Text = "{ m_targetip }";
			this.m_targetip.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// m_bitrate
			// 
			this.m_bitrate.AutoSize = true;
			this.m_bitrate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_bitrate.Location = new System.Drawing.Point(87, 0);
			this.m_bitrate.Name = "m_bitrate";
			this.m_bitrate.Size = new System.Drawing.Size(195, 15);
			this.m_bitrate.TabIndex = 3;
			this.m_bitrate.Text = "{ m_bitrate }";
			this.m_bitrate.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// TunerDeviceStatusControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.m_layoutpanel);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "TunerDeviceStatusControl";
			this.Size = new System.Drawing.Size(299, 127);
			this.m_layoutpanel.ResumeLayout(false);
			this.m_layoutpanel.PerformLayout();
			this.m_signallayoutpanel.ResumeLayout(false);
			this.m_signallayoutpanel.PerformLayout();
			this.m_headerlayoutpanel.ResumeLayout(false);
			this.m_headerlayoutpanel.PerformLayout();
			this.m_footerlayoutpanel.ResumeLayout(false);
			this.m_footerlayoutpanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label m_tunernumber;
		private System.Windows.Forms.Label m_channelname;
		private System.Windows.Forms.Label m_bitrate;
		private System.Windows.Forms.Label m_targetip;
		private zuki.hdhomeruntray.RoundedTableLayoutPanel m_layoutpanel;
		private System.Windows.Forms.TableLayoutPanel m_signallayoutpanel;
		private System.Windows.Forms.Label m_signalstrengthlabel;
		private System.Windows.Forms.Label m_signalqualitylabel;
		private System.Windows.Forms.Label m_symbolqualitylabel;
		private zuki.hdhomeruntray.SignalStatusProcessBar m_signalstrengthbar;
		private zuki.hdhomeruntray.SignalStatusProcessBar m_signalqualitybar;
		private zuki.hdhomeruntray.SignalStatusProcessBar m_symbolqualitybar;
		private System.Windows.Forms.Label m_signalstrengthpct;
		private System.Windows.Forms.Label m_signalqualitypct;
		private System.Windows.Forms.Label m_symbolqualitypct;
		private System.Windows.Forms.TableLayoutPanel m_headerlayoutpanel;
		private System.Windows.Forms.Label m_activedot;
		private System.Windows.Forms.TableLayoutPanel m_footerlayoutpanel;
	}
}
