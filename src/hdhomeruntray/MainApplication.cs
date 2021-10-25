//---------------------------------------------------------------------------
// Copyright (c) 2021 Michael G. Brehm
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//---------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Windows.Forms;

namespace hdhomeruntray
{
	//-----------------------------------------------------------------------
	// MainApplication (internal)
	//
	// Provides the main application context object, which is used as the 
	// parameter to Application.Run() instead of providing a main form object
	//-----------------------------------------------------------------------

	class MainApplication : ApplicationContext
	{
		// Instance Constructor
		//
		public MainApplication()
		{
			Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
			InitializeComponent();

			// Show the tray icon after initialization
			m_notifyicon.Visible = true;
		}

		//-------------------------------------------------------------------
		// Private Member Functions
		//-------------------------------------------------------------------

		// InitializeComponent
		//
		// Initializes all of the Windows Forms components and object
		private void InitializeComponent()
		{
			// Create the tray icon instance and set a default icon
			m_notifyicon = new NotifyIcon();
			m_notifyicon.Icon = StatusIcons.Get(StatusIconType.Idle);

			// Work in progress, the standard meno looks like garbage


			m_contextmenu = new ContextMenuStrip();
			CloseMenuItem = new ToolStripMenuItem();
			m_contextmenu.SuspendLayout();

			// 
			// TrayIconContextMenu
			// 
			this.m_contextmenu.Items.AddRange(new ToolStripItem[] {
			this.CloseMenuItem});
			this.m_contextmenu.Name = "TrayIconContextMenu";
			this.m_contextmenu.Size = new Size(153, 70);
			// 
			// CloseMenuItem
			// 
			this.CloseMenuItem.Name = "CloseMenuItem";
			this.CloseMenuItem.Size = new Size(152, 22);
			this.CloseMenuItem.Text = "Exit";
			this.CloseMenuItem.Click += new EventHandler(this.CloseMenuItem_Click);

			m_contextmenu.ResumeLayout(false);
			m_notifyicon.ContextMenuStrip = m_contextmenu;
		}

		private void OnApplicationExit(object sender, EventArgs args)
		{
			// Hide the tray icon on exit
			m_notifyicon.Visible = false;
		}

		private void CloseMenuItem_Click(object sender, EventArgs args)
		{
			Application.Exit();
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private NotifyIcon m_notifyicon;
		private ContextMenuStrip m_contextmenu;
		private ToolStripMenuItem CloseMenuItem;
	}
}