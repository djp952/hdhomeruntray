
namespace zuki.hdhomeruntray
{
	partial class SettingsForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.m_layoutpanel = new System.Windows.Forms.FlowLayoutPanel();
			this.m_header = new zuki.hdhomeruntray.SettingsFormHeaderControl();
			this.m_settings = new zuki.hdhomeruntray.SettingsFormSettingsControl();
			this.m_footer = new zuki.hdhomeruntray.SettingsFormFooterControl();
			this.m_layoutpanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_layoutpanel
			// 
			this.m_layoutpanel.AutoSize = true;
			this.m_layoutpanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_layoutpanel.Controls.Add(this.m_header);
			this.m_layoutpanel.Controls.Add(this.m_settings);
			this.m_layoutpanel.Controls.Add(this.m_footer);
			this.m_layoutpanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_layoutpanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.m_layoutpanel.Location = new System.Drawing.Point(0, 0);
			this.m_layoutpanel.Name = "m_layoutpanel";
			this.m_layoutpanel.Padding = new System.Windows.Forms.Padding(4);
			this.m_layoutpanel.Size = new System.Drawing.Size(387, 330);
			this.m_layoutpanel.TabIndex = 0;
			this.m_layoutpanel.WrapContents = false;
			// 
			// m_header
			// 
			this.m_header.AutoSize = true;
			this.m_header.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_header.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_header.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_header.Location = new System.Drawing.Point(7, 7);
			this.m_header.Name = "m_header";
			this.m_header.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
			this.m_header.Size = new System.Drawing.Size(285, 47);
			this.m_header.TabIndex = 0;
			// 
			// m_settings
			// 
			this.m_settings.AutoSize = true;
			this.m_settings.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_settings.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_settings.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_settings.Location = new System.Drawing.Point(7, 60);
			this.m_settings.Name = "m_settings";
			this.m_settings.Padding = new System.Windows.Forms.Padding(0, 1, 0, 1);
			this.m_settings.Size = new System.Drawing.Size(285, 153);
			this.m_settings.TabIndex = 1;
			// 
			// m_footer
			// 
			this.m_footer.AutoSize = true;
			this.m_footer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_footer.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_footer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_footer.Location = new System.Drawing.Point(7, 219);
			this.m_footer.Name = "m_footer";
			this.m_footer.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this.m_footer.Size = new System.Drawing.Size(285, 25);
			this.m_footer.TabIndex = 2;
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(387, 330);
			this.ControlBox = false;
			this.Controls.Add(this.m_layoutpanel);
			this.DoubleBuffered = true;
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

		private System.Windows.Forms.FlowLayoutPanel m_layoutpanel;
		private SettingsFormHeaderControl m_header;
		private SettingsFormSettingsControl m_settings;
		private SettingsFormFooterControl m_footer;
	}
}