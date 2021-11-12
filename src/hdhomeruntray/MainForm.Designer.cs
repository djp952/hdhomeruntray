
namespace zuki.hdhomeruntray
{
	partial class MainForm
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
			this.mainFormControlPanel1 = new zuki.hdhomeruntray.MainFormControlPanel();
			this.mainFormSettingsPage1 = new zuki.hdhomeruntray.MainFormSettingsPage();
			this.m_layoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_layoutPanel
			// 
			this.m_layoutPanel.AutoSize = true;
			this.m_layoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_layoutPanel.Controls.Add(this.mainFormControlPanel1);
			this.m_layoutPanel.Controls.Add(this.mainFormSettingsPage1);
			this.m_layoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_layoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.BottomUp;
			this.m_layoutPanel.Location = new System.Drawing.Point(10, 10);
			this.m_layoutPanel.Name = "m_layoutPanel";
			this.m_layoutPanel.Size = new System.Drawing.Size(294, 269);
			this.m_layoutPanel.TabIndex = 0;
			// 
			// mainFormControlPanel1
			// 
			this.mainFormControlPanel1.AutoSize = true;
			this.mainFormControlPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.mainFormControlPanel1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mainFormControlPanel1.Location = new System.Drawing.Point(3, 253);
			this.mainFormControlPanel1.Name = "mainFormControlPanel1";
			this.mainFormControlPanel1.Size = new System.Drawing.Size(132, 13);
			this.mainFormControlPanel1.TabIndex = 0;
			// 
			// mainFormSettingsPage1
			// 
			this.mainFormSettingsPage1.Location = new System.Drawing.Point(3, 77);
			this.mainFormSettingsPage1.Name = "mainFormSettingsPage1";
			this.mainFormSettingsPage1.Size = new System.Drawing.Size(195, 170);
			this.mainFormSettingsPage1.TabIndex = 1;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(314, 289);
			this.ControlBox = false;
			this.Controls.Add(this.m_layoutPanel);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.Padding = new System.Windows.Forms.Padding(10);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "MainForm";
			this.TopMost = true;
			this.m_layoutPanel.ResumeLayout(false);
			this.m_layoutPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel m_layoutPanel;
		private MainFormControlPanel mainFormControlPanel1;
		private MainFormSettingsPage mainFormSettingsPage1;
	}
}