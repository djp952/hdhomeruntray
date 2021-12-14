
namespace zuki.hdhomeruntray
{
	partial class SettingsFormFooterControl
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
			this.m_link2 = new System.Windows.Forms.LinkLabel();
			this.m_link1 = new System.Windows.Forms.LinkLabel();
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
			this.m_layoutpanel.Controls.Add(this.m_link2, 1, 0);
			this.m_layoutpanel.Controls.Add(this.m_link1, 0, 0);
			this.m_layoutpanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_layoutpanel.Location = new System.Drawing.Point(0, 0);
			this.m_layoutpanel.Name = "m_layoutpanel";
			this.m_layoutpanel.Padding = new System.Windows.Forms.Padding(4);
			this.m_layoutpanel.Radii = new zuki.hdhomeruntray.Radii(0, 0, 4, 4);
			this.m_layoutpanel.RowCount = 1;
			this.m_layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_layoutpanel.Size = new System.Drawing.Size(144, 23);
			this.m_layoutpanel.TabIndex = 0;
			// 
			// m_link2
			// 
			this.m_link2.ActiveLinkColor = System.Drawing.SystemColors.HotTrack;
			this.m_link2.AutoSize = true;
			this.m_link2.Dock = System.Windows.Forms.DockStyle.Right;
			this.m_link2.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.m_link2.LinkColor = System.Drawing.SystemColors.HotTrack;
			this.m_link2.Location = new System.Drawing.Point(75, 4);
			this.m_link2.Name = "m_link2";
			this.m_link2.Size = new System.Drawing.Size(62, 15);
			this.m_link2.TabIndex = 1;
			this.m_link2.TabStop = true;
			this.m_link2.Text = "{ m_link2 }";
			this.m_link2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_link2.VisitedLinkColor = System.Drawing.SystemColors.HotTrack;
			this.m_link2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLink2Clicked);
			// 
			// m_link1
			// 
			this.m_link1.ActiveLinkColor = System.Drawing.SystemColors.HotTrack;
			this.m_link1.AutoSize = true;
			this.m_link1.Dock = System.Windows.Forms.DockStyle.Left;
			this.m_link1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.m_link1.LinkColor = System.Drawing.SystemColors.HotTrack;
			this.m_link1.Location = new System.Drawing.Point(7, 4);
			this.m_link1.Name = "m_link1";
			this.m_link1.Size = new System.Drawing.Size(62, 15);
			this.m_link1.TabIndex = 0;
			this.m_link1.TabStop = true;
			this.m_link1.Text = "{ m_link1 }";
			this.m_link1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_link1.VisitedLinkColor = System.Drawing.SystemColors.HotTrack;
			this.m_link1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLink1Clicked);
			// 
			// SettingsFormFooterControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.m_layoutpanel);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "SettingsFormFooterControl";
			this.Size = new System.Drawing.Size(144, 23);
			this.m_layoutpanel.ResumeLayout(false);
			this.m_layoutpanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private RoundedTableLayoutPanel m_layoutpanel;
		private System.Windows.Forms.LinkLabel m_link1;
		private System.Windows.Forms.LinkLabel m_link2;
	}
}
