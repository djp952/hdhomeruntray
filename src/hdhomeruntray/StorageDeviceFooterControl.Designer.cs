
namespace zuki.hdhomeruntray
{
	partial class StorageDeviceFooterControl
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
			this.m_layoutPanel = new zuki.hdhomeruntray.RoundedTableLayoutPanel();
			this.m_version = new System.Windows.Forms.Label();
			this.m_space = new System.Windows.Forms.Label();
			this.m_layoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_layoutPanel
			// 
			this.m_layoutPanel.AutoSize = true;
			this.m_layoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_layoutPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.m_layoutPanel.ColumnCount = 2;
			this.m_layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.m_layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.m_layoutPanel.Controls.Add(this.m_version, 0, 0);
			this.m_layoutPanel.Controls.Add(this.m_space, 1, 0);
			this.m_layoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_layoutPanel.ForeColor = System.Drawing.Color.Black;
			this.m_layoutPanel.Location = new System.Drawing.Point(0, 0);
			this.m_layoutPanel.Name = "m_layoutPanel";
			this.m_layoutPanel.Padding = new System.Windows.Forms.Padding(4);
			this.m_layoutPanel.Radius = 16;
			this.m_layoutPanel.RowCount = 1;
			this.m_layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.m_layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
			this.m_layoutPanel.Size = new System.Drawing.Size(152, 21);
			this.m_layoutPanel.TabIndex = 1;
			// 
			// m_version
			// 
			this.m_version.AutoSize = true;
			this.m_version.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_version.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_version.Location = new System.Drawing.Point(7, 4);
			this.m_version.Name = "m_version";
			this.m_version.Size = new System.Drawing.Size(70, 13);
			this.m_version.TabIndex = 3;
			this.m_version.Text = "{ m_version }";
			this.m_version.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// m_space
			// 
			this.m_space.AutoSize = true;
			this.m_space.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_space.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_space.Location = new System.Drawing.Point(83, 4);
			this.m_space.Name = "m_space";
			this.m_space.Size = new System.Drawing.Size(62, 13);
			this.m_space.TabIndex = 4;
			this.m_space.Text = "{ m_space }";
			this.m_space.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// StorageDeviceFooterControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.m_layoutPanel);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "StorageDeviceFooterControl";
			this.Size = new System.Drawing.Size(152, 21);
			this.m_layoutPanel.ResumeLayout(false);
			this.m_layoutPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private RoundedTableLayoutPanel m_layoutPanel;
		private System.Windows.Forms.Label m_version;
		private System.Windows.Forms.Label m_space;
	}
}
