
namespace zuki.hdhomeruntray
{
	partial class MainFormControlPanel
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
			this.m_layoutPanel = new System.Windows.Forms.Panel();
			this.m_pinunpin = new System.Windows.Forms.Label();
			this.m_devicelist = new System.Windows.Forms.Label();
			this.m_options = new System.Windows.Forms.Label();
			this.m_layoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_layoutPanel
			// 
			this.m_layoutPanel.AutoSize = true;
			this.m_layoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_layoutPanel.Controls.Add(this.m_pinunpin);
			this.m_layoutPanel.Controls.Add(this.m_devicelist);
			this.m_layoutPanel.Controls.Add(this.m_options);
			this.m_layoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_layoutPanel.Location = new System.Drawing.Point(10, 10);
			this.m_layoutPanel.Name = "m_layoutPanel";
			this.m_layoutPanel.Size = new System.Drawing.Size(60, 13);
			this.m_layoutPanel.TabIndex = 0;
			// 
			// m_pinunpin
			// 
			this.m_pinunpin.AutoSize = true;
			this.m_pinunpin.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_pinunpin.Font = new System.Drawing.Font("Symbols", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_pinunpin.Location = new System.Drawing.Point(0, 0);
			this.m_pinunpin.Name = "m_pinunpin";
			this.m_pinunpin.Size = new System.Drawing.Size(20, 13);
			this.m_pinunpin.TabIndex = 0;
			this.m_pinunpin.Text = "";
			this.m_pinunpin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_devicelist
			// 
			this.m_devicelist.AutoSize = true;
			this.m_devicelist.Dock = System.Windows.Forms.DockStyle.Right;
			this.m_devicelist.Font = new System.Drawing.Font("Symbols", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_devicelist.Location = new System.Drawing.Point(20, 0);
			this.m_devicelist.Name = "m_devicelist";
			this.m_devicelist.Size = new System.Drawing.Size(20, 13);
			this.m_devicelist.TabIndex = 1;
			this.m_devicelist.Text = "";
			this.m_devicelist.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// m_options
			// 
			this.m_options.AutoSize = true;
			this.m_options.Dock = System.Windows.Forms.DockStyle.Right;
			this.m_options.Font = new System.Drawing.Font("Symbols", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_options.Location = new System.Drawing.Point(40, 0);
			this.m_options.Name = "m_options";
			this.m_options.Size = new System.Drawing.Size(20, 13);
			this.m_options.TabIndex = 2;
			this.m_options.Text = "";
			this.m_options.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// MainFormControlPanel
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.m_layoutPanel);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "MainFormControlPanel";
			this.Padding = new System.Windows.Forms.Padding(10);
			this.Size = new System.Drawing.Size(80, 33);
			this.m_layoutPanel.ResumeLayout(false);
			this.m_layoutPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label m_pinunpin;
		private System.Windows.Forms.Label m_devicelist;
		private System.Windows.Forms.Label m_options;
		private System.Windows.Forms.Panel m_layoutPanel;
	}
}
