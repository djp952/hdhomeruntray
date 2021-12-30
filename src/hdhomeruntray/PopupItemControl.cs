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
using Microsoft.Win32;

using zuki.hdhomeruntray.Properties;

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
		protected PopupItemControl(PopupItemControlType type)
		{
			InitializeComponent();

			m_layoutpanel.EnableDoubleBuferring();

			using(Graphics graphics = CreateGraphics())
			{
				Padding = Padding.ScaleDPI(graphics);
				m_layoutpanel.Margin = m_layoutpanel.Margin.ScaleDPI(graphics);
				m_layoutpanel.Padding = m_layoutpanel.Padding.ScaleDPI(graphics);
				m_layoutpanel.Radii = m_layoutpanel.Radii.ScaleDPI(graphics);
			}

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

			// Create the hover timer
			m_hovertimer = new Timer();
			m_hovertimer.Tick += new EventHandler(OnHoverTimerTick);
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

		// HoverToClick
		//
		// Enables/disables "hover to click" for this control
		public bool HoverToClick
		{
			get => m_hovertoclick;
			set => m_hovertoclick = value;
		}

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

		// OnHoverTimerTick
		//
		// Invoked when the hover timer object has come due
		private void OnHoverTimerTick(object sender, EventArgs args)
		{
			// Always stop the timer, this is a one-shot
			if(m_hovertimer.Enabled) m_hovertimer.Stop();

			//If the mouse cursor is still in the client rectangle invoke a click
			if(ClientRectangle.Contains(PointToClient(Cursor.Position)))
			{
				if(m_type == PopupItemControlType.Toggle) OnMouseClickToggle(this, EventArgs.Empty);
				else if(m_type == PopupItemControlType.Button) OnMouseClickButton(this, EventArgs.Empty);
			}
		}

		// OnMouseClickButton
		//
		// Handles the MouseClick event for button-type controls
		private void OnMouseClickButton(object sender, EventArgs args)
		{
			// Stop any pending hover operation for this control
			if(m_hovertimer.Enabled) m_hovertimer.Stop();

			Selected?.Invoke(this, EventArgs.Empty);
		}

		// OnMouseEnterButton
		//
		// Handles the MouseEnter event for button-type controls
		private void OnMouseEnterButton(object sender, EventArgs args)
		{
			// Start the hover timer for this control if it is allowed and not already enabled
			if((m_hovertoclick) && (Settings.Default.HoverToClick == EnabledDisabled.Enabled) && (!m_hovertimer.Enabled))
			{
				m_hovertimer.Interval = GetHoverInterval(Settings.Default.HoverToClickDelay);
				m_hovertimer.Start();
			}

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
			// Stop any pending hover operation for this control
			if(m_hovertimer.Enabled) m_hovertimer.Stop();

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
			// Stop any pending hover operation for this control
			if(m_hovertimer.Enabled) m_hovertimer.Stop();

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
			// Start the hover timer for this control if it is allowed and not already enabled
			if((m_hovertoclick) && (Settings.Default.HoverToClick == EnabledDisabled.Enabled) && (!m_hovertimer.Enabled))
			{
				m_hovertimer.Interval = GetHoverInterval(Settings.Default.HoverToClickDelay);
				m_hovertimer.Start();
			}

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
			// Stop any pending hover operation for this control
			if(m_hovertimer.Enabled) m_hovertimer.Stop();

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
		// Private Member Functions
		//-------------------------------------------------------------------

		// GetHoverInterval (static)
		//
		// Converts a HoverToClickDelay into milliseconds taking into consideration
		// the running operation system limitations
		private static int GetHoverInterval(HoverToClickDelay delay)
		{
			// No coersion is necessary for a non-default value or a default one outside of Windows 11
			if((delay != HoverToClickDelay.SystemDefault) || (!VersionHelper.IsWindows11OrGreater())) return (int)delay;

			int mousehovertimeout = 400;            // Default value to use on Windows 11 (ms)

			// Use the default hover interval specified in HKEY_CURRENT_USER
			object value = Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseHoverTime", null);
			if((value != null) && (value is string @string)) _ = int.TryParse(@string, out mousehovertimeout);

			return mousehovertimeout;
		}

		//-------------------------------------------------------------------
		// Private Member Variables
		//-------------------------------------------------------------------

		private readonly PopupItemControlType m_type;
		private bool m_toggled = false;
		private bool m_hovertoclick = false;
		private readonly Timer m_hovertimer;
	}
}
