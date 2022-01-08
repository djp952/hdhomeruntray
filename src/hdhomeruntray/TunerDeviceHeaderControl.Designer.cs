
namespace zuki.hdhomeruntray
{
	partial class TunerDeviceHeaderControl
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
			this.m_layoutpanel = new zuki.hdhomeruntray.RoundedTableLayoutPanel();
			this.m_devicename = new System.Windows.Forms.Label();
			this.m_modelname = new System.Windows.Forms.Label();
			this.m_deviceid = new System.Windows.Forms.Label();
			this.m_ipaddress = new System.Windows.Forms.LinkLabel();
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
			this.m_layoutpanel.Controls.Add(this.m_devicename, 0, 0);
			this.m_layoutpanel.Controls.Add(this.m_modelname, 1, 0);
			this.m_layoutpanel.Controls.Add(this.m_deviceid, 0, 1);
			this.m_layoutpanel.Controls.Add(this.m_ipaddress, 1, 1);
			this.m_layoutpanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_layoutpanel.Location = new System.Drawing.Point(0, 0);
			this.m_layoutpanel.Name = "m_layoutpanel";
			this.m_layoutpanel.Padding = new System.Windows.Forms.Padding(4);
			this.m_layoutpanel.Radii = new zuki.hdhomeruntray.Radii(4, 4, 0, 0);
			this.m_layoutpanel.RowCount = 2;
			this.m_layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.m_layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.m_layoutpanel.Size = new System.Drawing.Size(246, 42);
			this.m_layoutpanel.TabIndex = 1;
			// 
			// m_devicename
			// 
			this.m_devicename.AutoSize = true;
			this.m_devicename.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_devicename.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_devicename.Location = new System.Drawing.Point(7, 4);
			this.m_devicename.Name = "m_devicename";
			this.m_devicename.Size = new System.Drawing.Size(113, 17);
			this.m_devicename.TabIndex = 1;
			this.m_devicename.Text = "{ m_devicename }";
			this.m_devicename.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// m_modelname
			// 
			this.m_modelname.AutoSize = true;
			this.m_modelname.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_modelname.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_modelname.Location = new System.Drawing.Point(126, 4);
			this.m_modelname.Name = "m_modelname";
			this.m_modelname.Size = new System.Drawing.Size(113, 17);
			this.m_modelname.TabIndex = 2;
			this.m_modelname.Text = "{ m_modelname }";
			this.m_modelname.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// m_deviceid
			// 
			this.m_deviceid.AutoSize = true;
			this.m_deviceid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_deviceid.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_deviceid.Location = new System.Drawing.Point(7, 21);
			this.m_deviceid.Name = "m_deviceid";
			this.m_deviceid.Size = new System.Drawing.Size(113, 17);
			this.m_deviceid.TabIndex = 3;
			this.m_deviceid.Text = "{ m_deviceid }";
			this.m_deviceid.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// m_ipaddress
			// 
			this.m_ipaddress.ActiveLinkColor = System.Drawing.SystemColors.HotTrack;
			this.m_ipaddress.AutoSize = true;
			this.m_ipaddress.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ipaddress.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_ipaddress.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.m_ipaddress.LinkColor = System.Drawing.SystemColors.HotTrack;
			this.m_ipaddress.Location = new System.Drawing.Point(126, 21);
			this.m_ipaddress.Name = "m_ipaddress";
			this.m_ipaddress.Size = new System.Drawing.Size(113, 17);
			this.m_ipaddress.TabIndex = 4;
			this.m_ipaddress.TabStop = true;
			this.m_ipaddress.Text = "{ m_ipaddress }";
			this.m_ipaddress.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			this.m_ipaddress.VisitedLinkColor = System.Drawing.SystemColors.HotTrack;
			this.m_ipaddress.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnIPAddressClicked);
			// 
			// TunerDeviceHeaderControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.m_layoutpanel);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "TunerDeviceHeaderControl";
			this.Size = new System.Drawing.Size(246, 42);
			this.m_layoutpanel.ResumeLayout(false);
			this.m_layoutpanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private RoundedTableLayoutPanel m_layoutpanel;
		private System.Windows.Forms.Label m_devicename;
		private System.Windows.Forms.Label m_modelname;
		private System.Windows.Forms.Label m_deviceid;
		private System.Windows.Forms.LinkLabel m_ipaddress;
	}
}
