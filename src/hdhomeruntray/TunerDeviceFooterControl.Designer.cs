
namespace zuki.hdhomeruntray
{
	partial class TunerDeviceFooterControl
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
			this.m_firmwareversion = new System.Windows.Forms.Label();
			this.m_unused = new System.Windows.Forms.Label();
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
			this.m_layoutPanel.Controls.Add(this.m_firmwareversion, 0, 0);
			this.m_layoutPanel.Controls.Add(this.m_unused, 1, 0);
			this.m_layoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_layoutPanel.ForeColor = System.Drawing.Color.Black;
			this.m_layoutPanel.Location = new System.Drawing.Point(0, 0);
			this.m_layoutPanel.Name = "m_layoutPanel";
			this.m_layoutPanel.Padding = new System.Windows.Forms.Padding(4);
			this.m_layoutPanel.Radius = 16;
			this.m_layoutPanel.RowCount = 1;
			this.m_layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.m_layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
			this.m_layoutPanel.Size = new System.Drawing.Size(207, 21);
			this.m_layoutPanel.TabIndex = 1;
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
			// m_unused
			// 
			this.m_unused.AutoSize = true;
			this.m_unused.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_unused.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_unused.Location = new System.Drawing.Point(128, 4);
			this.m_unused.Name = "m_unused";
			this.m_unused.Size = new System.Drawing.Size(72, 13);
			this.m_unused.TabIndex = 4;
			this.m_unused.Text = "{ m_unused }";
			this.m_unused.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// TunerDeviceFooterControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.m_layoutPanel);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "TunerDeviceFooterControl";
			this.Size = new System.Drawing.Size(207, 21);
			this.m_layoutPanel.ResumeLayout(false);
			this.m_layoutPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private RoundedTableLayoutPanel m_layoutPanel;
		private System.Windows.Forms.Label m_firmwareversion;
		private System.Windows.Forms.Label m_unused;
	}
}
