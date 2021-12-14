
namespace zuki.hdhomeruntray
{
	partial class SettingsFormHeaderControl
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
			this.m_icon = new System.Windows.Forms.PictureBox();
			this.m_version = new System.Windows.Forms.Label();
			this.m_appname = new System.Windows.Forms.Label();
			this.m_layoutpanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_icon)).BeginInit();
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
			this.m_layoutpanel.Controls.Add(this.m_icon, 0, 0);
			this.m_layoutpanel.Controls.Add(this.m_version, 1, 1);
			this.m_layoutpanel.Controls.Add(this.m_appname, 1, 0);
			this.m_layoutpanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_layoutpanel.Location = new System.Drawing.Point(0, 0);
			this.m_layoutpanel.Name = "m_layoutpanel";
			this.m_layoutpanel.Padding = new System.Windows.Forms.Padding(4);
			this.m_layoutpanel.Radii = new zuki.hdhomeruntray.Radii(4, 4, 0, 0);
			this.m_layoutpanel.RowCount = 2;
			this.m_layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_layoutpanel.Size = new System.Drawing.Size(150, 46);
			this.m_layoutpanel.TabIndex = 0;
			// 
			// m_icon
			// 
			this.m_icon.Location = new System.Drawing.Point(7, 7);
			this.m_icon.MinimumSize = new System.Drawing.Size(32, 32);
			this.m_icon.Name = "m_icon";
			this.m_layoutpanel.SetRowSpan(this.m_icon, 2);
			this.m_icon.Size = new System.Drawing.Size(32, 32);
			this.m_icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.m_icon.TabIndex = 0;
			this.m_icon.TabStop = false;
			// 
			// m_version
			// 
			this.m_version.AutoSize = true;
			this.m_version.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_version.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_version.Location = new System.Drawing.Point(45, 21);
			this.m_version.Name = "m_version";
			this.m_version.Size = new System.Drawing.Size(98, 21);
			this.m_version.TabIndex = 2;
			this.m_version.Text = "{ m_version }";
			this.m_version.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_appname
			// 
			this.m_appname.AutoSize = true;
			this.m_appname.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_appname.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_appname.Location = new System.Drawing.Point(45, 4);
			this.m_appname.Name = "m_appname";
			this.m_appname.Size = new System.Drawing.Size(98, 17);
			this.m_appname.TabIndex = 1;
			this.m_appname.Text = "{ m_appname }";
			this.m_appname.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SettingsFormHeaderControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.m_layoutpanel);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "SettingsFormHeaderControl";
			this.Size = new System.Drawing.Size(150, 46);
			this.m_layoutpanel.ResumeLayout(false);
			this.m_layoutpanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_icon)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private RoundedTableLayoutPanel m_layoutpanel;
		private System.Windows.Forms.PictureBox m_icon;
		private System.Windows.Forms.Label m_appname;
		private System.Windows.Forms.Label m_version;
	}
}
