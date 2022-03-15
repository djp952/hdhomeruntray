
namespace zuki.hdhomeruntray
{
	partial class TunerDeviceFooterControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.m_outerpanel = new System.Windows.Forms.TableLayoutPanel();
			this.m_layoutpanel = new zuki.hdhomeruntray.RoundedTableLayoutPanel();
			this.m_firmwareversion = new System.Windows.Forms.Label();
			this.m_updateavailable = new System.Windows.Forms.LinkLabel();
			this.m_outerpanel.SuspendLayout();
			this.m_layoutpanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_outerpanel
			// 
			this.m_outerpanel.AutoSize = true;
			this.m_outerpanel.ColumnCount = 1;
			this.m_outerpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.m_outerpanel.Controls.Add(this.m_layoutpanel, 0, 0);
			this.m_outerpanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_outerpanel.Location = new System.Drawing.Point(0, 0);
			this.m_outerpanel.Margin = new System.Windows.Forms.Padding(0);
			this.m_outerpanel.Name = "m_outerpanel";
			this.m_outerpanel.RowCount = 1;
			this.m_outerpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.m_outerpanel.Size = new System.Drawing.Size(250, 21);
			this.m_outerpanel.TabIndex = 2;
			// 
			// m_layoutpanel
			// 
			this.m_layoutpanel.AutoSize = true;
			this.m_layoutpanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_layoutpanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.m_layoutpanel.ColumnCount = 2;
			this.m_layoutpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.m_layoutpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.m_layoutpanel.Controls.Add(this.m_firmwareversion, 0, 0);
			this.m_layoutpanel.Controls.Add(this.m_updateavailable, 1, 0);
			this.m_layoutpanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_layoutpanel.Location = new System.Drawing.Point(0, 0);
			this.m_layoutpanel.Margin = new System.Windows.Forms.Padding(0);
			this.m_layoutpanel.Name = "m_layoutpanel";
			this.m_layoutpanel.Padding = new System.Windows.Forms.Padding(4);
			this.m_layoutpanel.Radii = new zuki.hdhomeruntray.Radii(0, 0, 4, 4);
			this.m_layoutpanel.RowCount = 1;
			this.m_layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.m_layoutpanel.Size = new System.Drawing.Size(250, 21);
			this.m_layoutpanel.TabIndex = 1;
			// 
			// m_firmwareversion
			// 
			this.m_firmwareversion.AutoSize = true;
			this.m_firmwareversion.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_firmwareversion.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_firmwareversion.Location = new System.Drawing.Point(7, 4);
			this.m_firmwareversion.Name = "m_firmwareversion";
			this.m_firmwareversion.Size = new System.Drawing.Size(115, 13);
			this.m_firmwareversion.TabIndex = 3;
			this.m_firmwareversion.Text = "{ m_firmwareversion }";
			this.m_firmwareversion.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// m_updateavailable
			// 
			this.m_updateavailable.ActiveLinkColor = System.Drawing.SystemColors.HotTrack;
			this.m_updateavailable.AutoSize = true;
			this.m_updateavailable.Dock = System.Windows.Forms.DockStyle.Right;
			this.m_updateavailable.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_updateavailable.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.m_updateavailable.LinkColor = System.Drawing.SystemColors.HotTrack;
			this.m_updateavailable.Location = new System.Drawing.Point(128, 4);
			this.m_updateavailable.Name = "m_updateavailable";
			this.m_updateavailable.Size = new System.Drawing.Size(115, 13);
			this.m_updateavailable.TabIndex = 4;
			this.m_updateavailable.TabStop = true;
			this.m_updateavailable.Text = "{ m_updateavailable }";
			this.m_updateavailable.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			this.m_updateavailable.VisitedLinkColor = System.Drawing.SystemColors.HotTrack;
			this.m_updateavailable.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnUpdateClicked);
			// 
			// TunerDeviceFooterControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.m_outerpanel);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "TunerDeviceFooterControl";
			this.Size = new System.Drawing.Size(250, 21);
			this.m_outerpanel.ResumeLayout(false);
			this.m_outerpanel.PerformLayout();
			this.m_layoutpanel.ResumeLayout(false);
			this.m_layoutpanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private RoundedTableLayoutPanel m_layoutpanel;
		private System.Windows.Forms.Label m_firmwareversion;
		private System.Windows.Forms.LinkLabel m_updateavailable;
		private System.Windows.Forms.TableLayoutPanel m_outerpanel;
	}
}
