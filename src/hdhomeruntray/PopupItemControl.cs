//---------------------------------------------------------------------------
// Copyright (c) 2021-2022 Michael G. Brehm
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
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace zuki.hdhomeruntray
{
	//-----------------------------------------------------------------------
	// Class PopupItemControl (internal)
	//
	// Implements the base class for each popup form item control

	internal abstract partial class PopupItemControl : UserControl
	{
		// Instance Constructor (protected)
		//
		protected PopupItemControl(PopupItemControlType type, SizeF scalefactor)
		{
			InitializeComponent();

			// THEME
			//
			m_appthemechanged = new EventHandler(OnApplicationThemeChanged);
			ApplicationTheme.Changed += m_appthemechanged;
			OnApplicationThemeChanged(this, EventArgs.Empty);

			m_layoutpanel.EnableDoubleBuferring();

			Padding = Padding.ScaleDPI(scalefactor);
			m_layoutpanel.Margin = m_layoutpanel.Margin.ScaleDPI(scalefactor);
			m_layoutpanel.Padding = m_layoutpanel.Padding.ScaleDPI(scalefactor);
			m_layoutpanel.Radii = m_layoutpanel.Radii.ScaleDPI(scalefactor);

			// Save the type of control being implemented
			m_type = type;

			// Button type event handlers
			if(m_type == PopupItemControlType.Button)
			{
				m_layoutpanel.MouseClick += OnMouseClickButton;
				m_layoutpanel.MouseEnter += OnMouseEnterButton;
				m_layoutpanel.MouseLeave += OnMouseLeaveButton;
			}

			// Toggle type event handlers
			else if(m_type == PopupItemControlType.Toggle)
			{
				m_layoutpanel.MouseClick += OnMouseClickToggle;
				m_layoutpanel.MouseEnter += OnMouseEnterToggle;
				m_layoutpanel.MouseLeave += OnMouseLeaveToggle;
			}
		}

		// Dispose
		//
		// Releases unmanaged resources and optionally releases managed resources
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				// Dispose managed state
				if(m_appthemechanged != null) ApplicationTheme.Changed -= m_appthemechanged;
				if(components != null) components.Dispose();
			}

			base.Dispose(disposing);
		}

		//-------------------------------------------------------------------------
		// Events
		//-------------------------------------------------------------------------

		// Selected
		//
		// Invoked when a button-type PopupItemControl has been selected
		public event EventHandler Selected;

		// Toggled
		//
		// Invoked when a toggle-type PopupItemControl has been toggled
		public event PopupItemToggledEventHandler Toggled;

		//-------------------------------------------------------------------
		// Member Functions
		//-------------------------------------------------------------------

		// Toggle
		//
		// Forces the state of a toggle-type control
		public void Toggle(bool toggled)
		{
			// If the desired state does not match the current state, force
			// a change by invoking the mouse click handler for the layout panel
			// (The mouse events are pass-through to that control)
			if((m_type == PopupItemControlType.Toggle) && (toggled != m_toggled))
			{
				OnMouseClickToggle(LayoutPanel, EventArgs.Empty);
			}
		}

		//-------------------------------------------------------------------
		// Properties
		//-------------------------------------------------------------------

		// ControlType
		//
		// Exposes the type of popup item control
		public PopupItemControlType ControlType => m_type;

		// IsToggled
		//
		// Exposes the toggled state of a toggle-type control
		public bool IsToggled => m_toggled;

		//-------------------------------------------------------------------
		// Protected Properties
		//-------------------------------------------------------------------

		// LayoutPanel
		//
		// Exposes the reference to the layout panel
		protected FlowLayoutPanel LayoutPanel => m_layoutpanel;

		//-------------------------------------------------------------------
		// Event Handlers
		//-------------------------------------------------------------------

		// OnApplicationThemeChanged
		//
		// Invoked when the application theme has changed
		private void OnApplicationThemeChanged(object sender, EventArgs args)
		{
			m_layoutpanel.BackColor = ApplicationTheme.PanelBackColor;
			m_layoutpanel.ForeColor = ApplicationTheme.PanelForeColor;
		}

		// OnMouseClickButton
		//
		// Handles the MouseClick event for button-type controls
		private void OnMouseClickButton(object sender, EventArgs args)
		{
			Selected?.Invoke(this, EventArgs.Empty);
		}

		// OnMouseEnterButton
		//
		// Handles the MouseEnter event for button-type controls
		private void OnMouseEnterButton(object sender, EventArgs args)
		{
			Debug.Assert(sender is RoundedFlowLayoutPanel);
			RoundedFlowLayoutPanel panel = (RoundedFlowLayoutPanel)sender;

			panel.ForeColor = ApplicationTheme.InvertedPanelForeColor;
			panel.BackColor = ApplicationTheme.InvertedPanelBackColor;
		}

		// OnMouseLeaveButton
		//
		// Handles the MouseLeave event for button-type controls
		private void OnMouseLeaveButton(object sender, EventArgs args)
		{
			Debug.Assert(sender is RoundedFlowLayoutPanel);
			RoundedFlowLayoutPanel panel = (RoundedFlowLayoutPanel)sender;

			panel.ForeColor = ApplicationTheme.PanelForeColor;
			panel.BackColor = ApplicationTheme.PanelBackColor;
		}

		// OnMouseClickToggle
		//
		// Handles the MouseClick event for toggle-type controls
		private void OnMouseClickToggle(object sender, EventArgs args)
		{
			m_toggled = !m_toggled;             // Invert the toggle state

			// Update the toggle state if the mouse isn't in the control
			if(!ClientRectangle.Contains(PointToClient(Cursor.Position)))
				OnMouseLeaveToggle(sender, args);

			Toggled?.Invoke(this, new PopupItemToggledEventArgs(m_toggled));
		}

		// OnMouseEnterToggle
		//
		// Handles the MouseEnter event for toggle-type controls
		private void OnMouseEnterToggle(object sender, EventArgs args)
		{
			Debug.Assert(sender is RoundedFlowLayoutPanel);
			RoundedFlowLayoutPanel panel = (RoundedFlowLayoutPanel)sender;

			panel.ForeColor = ApplicationTheme.InvertedPanelForeColor;
			panel.BackColor = ApplicationTheme.InvertedPanelBackColor;
		}

		// OnMouseLeaveToggle
		//
		// Handles the MouseLeave event for toggle-type controls
		private void OnMouseLeaveToggle(object sender, EventArgs args)
		{
			Debug.Assert(sender is RoundedFlowLayoutPanel);
			RoundedFlowLayoutPanel panel = (RoundedFlowLayoutPanel)sender;

			if(!m_toggled)
			{
				panel.ForeColor = ApplicationTheme.PanelForeColor;
				panel.BackColor = ApplicationTheme.PanelBackColor;
			}
			else
			{
				panel.ForeColor = ApplicationTheme.InvertedPanelForeColor;
				panel.BackColor = ApplicationTheme.InvertedPanelBackColor;
			}
		}

		//-------------------------------------------------------------------
		// Private Member Variables
		//-------------------------------------------------------------------

		private readonly PopupItemControlType m_type;
		private bool m_toggled = false;
		private readonly EventHandler m_appthemechanged;
	}
}
