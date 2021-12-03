
namespace zuki.hdhomeruntray
{
	partial class DeviceForm
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
			this.m_layoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.SuspendLayout();
			// 
			// m_layoutPanel
			// 
			this.m_layoutPanel.AutoSize = true;
			this.m_layoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_layoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_layoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.m_layoutPanel.Location = new System.Drawing.Point(4, 4);
			this.m_layoutPanel.Name = "m_layoutPanel";
			this.m_layoutPanel.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.m_layoutPanel.Size = new System.Drawing.Size(276, 111);
			this.m_layoutPanel.TabIndex = 0;
			this.m_layoutPanel.WrapContents = false;
			// 
			// DeviceForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(284, 119);
			this.ControlBox = false;
			this.Controls.Add(this.m_layoutPanel);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DeviceForm";
			this.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "DeviceForm";
			this.TopMost = true;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel m_layoutPanel;
	}
}