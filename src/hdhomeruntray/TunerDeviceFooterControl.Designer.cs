
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
			this.m_layoutpanel = new zuki.hdhomeruntray.RoundedTableLayoutPanel();
			this.m_firmwareversion = new System.Windows.Forms.Label();
			this.m_unused = new System.Windows.Forms.Label();
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
			this.m_layoutpanel.Controls.Add(this.m_firmwareversion, 0, 0);
			this.m_layoutpanel.Controls.Add(this.m_unused, 1, 0);
			this.m_layoutpanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_layoutpanel.Location = new System.Drawing.Point(0, 0);
			this.m_layoutpanel.Name = "m_layoutpanel";
			this.m_layoutpanel.Padding = new System.Windows.Forms.Padding(4);
			this.m_layoutpanel.Radii = new zuki.hdhomeruntray.Radii(0, 0, 4, 4);
			this.m_layoutpanel.RowCount = 1;
			this.m_layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.m_layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
			this.m_layoutpanel.Size = new System.Drawing.Size(207, 21);
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
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.m_layoutpanel);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "TunerDeviceFooterControl";
			this.Size = new System.Drawing.Size(207, 21);
			this.m_layoutpanel.ResumeLayout(false);
			this.m_layoutpanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private RoundedTableLayoutPanel m_layoutpanel;
		private System.Windows.Forms.Label m_firmwareversion;
		private System.Windows.Forms.Label m_unused;
	}
}
