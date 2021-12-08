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
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace zuki.hdhomeruntray
{
	//-----------------------------------------------------------------------
	// Class PopupItemControl (internal)
	//
	// Implements the base class for each popup form item control

	abstract partial class PopupItemControl : UserControl
	{
		// Instance Constructor (protected)
		//
		protected PopupItemControl(PopupItemControlType type)
		{
			InitializeComponent();

			// Adjust the padding after initialization
			Padding = Padding.ScaleDPI(Handle);
			m_controlspanel.Margin = m_controlspanel.Margin.ScaleDPI(Handle);

			// Save the type of control being implemented
			m_type = type;

			// Create the inner RoundedFlowLayoutPanel control
			m_layoutpanel = new RoundedFlowLayoutPanel
			{
				AutoSize = true,
				AutoSizeMode = AutoSizeMode.GrowAndShrink,
				FlowDirection = FlowDirection.LeftToRight,
				WrapContents = false,
				ForeColor = SystemColors.ControlText,
				BackColor = SystemColors.ControlLightLight,
				Padding = new Padding(6).ScaleDPI(Handle),
				Radii = new Radii(4).ScaleDPI(Handle),
			};

			// This needs to be double-buffered to prevent flickering when
			// the background color changes
			m_layoutpanel.EnableDoubleBuferring();

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

			// Add the inner RoundedFlowLayoutPanel to the output FlowLayoutPanel
			m_controlspanel.Controls.Add(m_layoutpanel);
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
		public PopupItemControlType ControlType
		{
			get { return m_type; }
		}

		// IsToggled
		//
		// Exposes the toggled state of a toggle-type control
		public bool IsToggled
		{
			get { return m_toggled; }
		}

		//-------------------------------------------------------------------
		// Protected Properties
		//-------------------------------------------------------------------

		// LayoutPanel
		//
		// Exposes the reference to the layout panel
		protected FlowLayoutPanel LayoutPanel
		{
			get { return m_layoutpanel; }
		}

		//-------------------------------------------------------------------
		// Event Handlers
		//-------------------------------------------------------------------

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

			panel.ForeColor = SystemColors.ControlLightLight;
			panel.BackColor = SystemColors.ControlDark;
		}

		// OnMouseLeaveButton
		//
		// Handles the MouseLeave event for button-type controls
		private void OnMouseLeaveButton(object sender, EventArgs args)
		{
			Debug.Assert(sender is RoundedFlowLayoutPanel);
			RoundedFlowLayoutPanel panel = (RoundedFlowLayoutPanel)sender;

			panel.ForeColor = SystemColors.ControlText;
			panel.BackColor = SystemColors.ControlLightLight;
		}

		// OnMouseClickToggle
		//
		// Handles the MouseClick event for toggle-type controls
		private void OnMouseClickToggle(object sender, EventArgs args)
		{
			m_toggled = !m_toggled;             // Invert the toggle state
			OnMouseLeaveToggle(sender, args);   // Update the toggle state
			
			Toggled?.Invoke(this, new PopupItemToggledEventArgs(m_toggled));
		}

		// OnMouseEnterToggle
		//
		// Handles the MouseEnter event for toggle-type controls
		private void OnMouseEnterToggle(object sender, EventArgs args)
		{
			Debug.Assert(sender is RoundedFlowLayoutPanel);
			RoundedFlowLayoutPanel panel = (RoundedFlowLayoutPanel)sender;

			panel.ForeColor = SystemColors.ControlLightLight;
			panel.BackColor = SystemColors.ControlDark;
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
				panel.ForeColor = SystemColors.ControlText;
				panel.BackColor = SystemColors.ControlLightLight;
			}
			else
			{
				panel.ForeColor = SystemColors.ControlLightLight;
				panel.BackColor = SystemColors.ControlDark;
			}
		}

		//-------------------------------------------------------------------
		// Private Member Variables
		//-------------------------------------------------------------------

		private readonly PopupItemControlType m_type;
		private readonly RoundedFlowLayoutPanel m_layoutpanel;
		private bool m_toggled = false;
	}
}
