
namespace zuki.hdhomeruntray
{
	partial class TunerDeviceToolsControl
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
			this.m_layoutpanel = new System.Windows.Forms.TableLayoutPanel();
			this.SuspendLayout();
			// 
			// m_layoutpanel
			// 
			this.m_layoutpanel.AutoSize = true;
			this.m_layoutpanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_layoutpanel.ColumnCount = 1;
			this.m_layoutpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.m_layoutpanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_layoutpanel.Location = new System.Drawing.Point(0, 0);
			this.m_layoutpanel.Margin = new System.Windows.Forms.Padding(0);
			this.m_layoutpanel.Name = "m_layoutpanel";
			this.m_layoutpanel.RowCount = 1;
			this.m_layoutpanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_layoutpanel.Size = new System.Drawing.Size(0, 0);
			this.m_layoutpanel.TabIndex = 1;
			// 
			// TunerDeviceToolsControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.m_layoutpanel);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "TunerDeviceToolsControl";
			this.Size = new System.Drawing.Size(0, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TableLayoutPanel m_layoutpanel;
	}
}
