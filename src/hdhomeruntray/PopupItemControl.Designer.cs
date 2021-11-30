
namespace zuki.hdhomeruntray
{
	partial class PopupItemControl
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
			this.m_controlspanel = new System.Windows.Forms.FlowLayoutPanel();
			this.SuspendLayout();
			// 
			// m_controlspanel
			// 
			this.m_controlspanel.AutoSize = true;
			this.m_controlspanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_controlspanel.Location = new System.Drawing.Point(0, 0);
			this.m_controlspanel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.m_controlspanel.Name = "m_controlspanel";
			this.m_controlspanel.Size = new System.Drawing.Size(0, 0);
			this.m_controlspanel.TabIndex = 0;
			this.m_controlspanel.WrapContents = false;
			// 
			// PopupItemControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.m_controlspanel);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "PopupItemControl";
			this.Size = new System.Drawing.Size(0, 0);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel m_controlspanel;
	}
}
