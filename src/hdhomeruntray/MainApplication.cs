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
	// Class MainApplication (internal)
	//
	// Provides the main application context object, which is used as the 
	// parameter to Application.Run() instead of providing a main form object
	
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
			// Create and initialize the ShellNotifyIcon instance
			m_notifyicon = new ShellNotifyIcon();
			m_notifyicon.ClosePopup += new EventHandler(this.OnNotifyIconClosePopup);
			m_notifyicon.OpenPopup += new EventHandler(this.OnNotifyIconOpenPopup);
			m_notifyicon.Selected += new EventHandler(this.OnNotifyIconSelected);
			m_notifyicon.Icon = StatusIcons.Get(StatusIconType.Idle);

			// Create the context menu
			m_contextmenu = new ContextMenuStrip
			{
				Name = "m_contextmenu"
			};
			m_contextmenu.SuspendLayout();

			// Exit
			var menuitem_exit = new ToolStripMenuItem
			{
				Name = "menuitem_exit",
				Text = "Exit"
			};
			menuitem_exit.Click += new EventHandler(this.OnMenuItemExit);

			// Complete the context menu and associate it with the ShellNotifyIcon
			m_contextmenu.Items.AddRange(new ToolStripItem[] { menuitem_exit });
			m_contextmenu.ResumeLayout(false);
			m_notifyicon.ContextMenuStrip = m_contextmenu;
		}

		//-------------------------------------------------------------------
		// Event Handlers
		//-------------------------------------------------------------------

		// OnApplicationExit
		//
		// Invoked when the application is exiting
		private void OnApplicationExit(object sender, EventArgs args)
		{
			m_notifyicon.Visible = false;		// Remove the tray icon
		}

		// OnMenuItemExit
		//
		// Invoked via the "Exit" menu item
		private void OnMenuItemExit(object sender, EventArgs args)
		{
			Application.Exit();
		}

		// OnNotifyIconClosePopup
		//
		// Invoked when the hover popup window should be closed
		private void OnNotifyIconClosePopup(object sender, EventArgs args)
		{
		}

		// OnNotifyIconOpenPopup
		//
		// Invoked when the hover popup window should be opened
		private void OnNotifyIconOpenPopup(object sender, EventArgs args)
		{
		}

		// OnNotifyIconSelected
		//
		// Invoked when the notify icon has been selected (clicked on)
		private void OnNotifyIconSelected(object sender, EventArgs args)
		{
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private ShellNotifyIcon m_notifyicon;
		private ContextMenuStrip m_contextmenu;
	}
}