
namespace zuki.hdhomeruntray
{
	partial class StorageDeviceRecordingControl
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
			this.m_name = new System.Windows.Forms.Label();
			this.m_recordinglabel = new System.Windows.Forms.Label();
			this.m_activedot = new System.Windows.Forms.Label();
			this.m_layoutpanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_layoutpanel
			// 
			this.m_layoutpanel.AutoSize = true;
			this.m_layoutpanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_layoutpanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.m_layoutpanel.ColumnCount = 3;
			this.m_layoutpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.m_layoutpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.m_layoutpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.m_layoutpanel.Controls.Add(this.m_name, 0, 1);
			this.m_layoutpanel.Controls.Add(this.m_recordinglabel, 1, 0);
			this.m_layoutpanel.Controls.Add(this.m_activedot, 0, 0);
			this.m_layoutpanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_layoutpanel.Location = new System.Drawing.Point(0, 0);
			this.m_layoutpanel.Margin = new System.Windows.Forms.Padding(0);
			this.m_layoutpanel.Name = "m_layoutpanel";
			this.m_layoutpanel.Padding = new System.Windows.Forms.Padding(4);
			this.m_layoutpanel.RowCount = 2;
			this.m_layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_layoutpanel.Size = new System.Drawing.Size(98, 38);
			this.m_layoutpanel.TabIndex = 2;
			// 
			// m_name
			// 
			this.m_name.AutoSize = true;
			this.m_layoutpanel.SetColumnSpan(this.m_name, 2);
			this.m_name.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_name.Location = new System.Drawing.Point(7, 19);
			this.m_name.Name = "m_name";
			this.m_name.Size = new System.Drawing.Size(84, 15);
			this.m_name.TabIndex = 4;
			this.m_name.Text = "{ m_name }";
			this.m_name.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// m_recordinglabel
			// 
			this.m_recordinglabel.AutoSize = true;
			this.m_recordinglabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_recordinglabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_recordinglabel.Location = new System.Drawing.Point(30, 4);
			this.m_recordinglabel.Name = "m_recordinglabel";
			this.m_recordinglabel.Size = new System.Drawing.Size(61, 15);
			this.m_recordinglabel.TabIndex = 1;
			this.m_recordinglabel.Text = "Recording";
			this.m_recordinglabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// m_activedot
			// 
			this.m_activedot.AutoSize = true;
			this.m_activedot.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_activedot.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_activedot.ForeColor = System.Drawing.SystemColors.GrayText;
			this.m_activedot.Location = new System.Drawing.Point(7, 4);
			this.m_activedot.Name = "m_activedot";
			this.m_activedot.Size = new System.Drawing.Size(17, 15);
			this.m_activedot.TabIndex = 3;
			this.m_activedot.Text = "●";
			this.m_activedot.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// StorageDeviceRecordingControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.m_layoutpanel);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "StorageDeviceRecordingControl";
			this.Size = new System.Drawing.Size(98, 38);
			this.m_layoutpanel.ResumeLayout(false);
			this.m_layoutpanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label m_recordinglabel;
		private System.Windows.Forms.Label m_name;
		private zuki.hdhomeruntray.RoundedTableLayoutPanel m_layoutpanel;
		private System.Windows.Forms.Label m_activedot;
	}
}
