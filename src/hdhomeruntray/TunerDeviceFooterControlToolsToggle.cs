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
using System.Drawing;
using System.Windows.Forms;

namespace zuki.hdhomeruntray
{
	//-----------------------------------------------------------------------
	// Class TunerDeviceFooterControlToolsToggle (internal)
	//
	// Implements the tools toggle button for a tuner device footer control

	internal partial class TunerDeviceFooterControlToolsToggle : RoundedTableLayoutPanel
	{
		// Instance Constructor (protected)
		//
		public TunerDeviceFooterControlToolsToggle(SizeF scalefactor)
		{
			// There is no Forms Designer for this component, set the required
			// properties manually during construction
			AutoSize = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			BackColor = SystemColors.ControlLightLight;
			Dock = DockStyle.Fill;
			Margin = new Padding(4, 0, 0, 0).ScaleDPI(scalefactor);
			Padding = new Padding(4).ScaleDPI(scalefactor);
			Radii = new Radii(0, 0, 4, 0).ScaleDPI(scalefactor);

			// Create the label control for the glyph
			PassthroughLabelControl label = new PassthroughLabelControl
			{
				AutoSize = true,
				Size = new Size(1, 1),
				Text = new string((char)SymbolGlyph.Tools, 1),
				TextAlign = ContentAlignment.MiddleCenter,
				Dock = DockStyle.Fill,
				Font = new Font("Symbols", 8.25F, FontStyle.Regular),
				Visible = true
			};
			Controls.Add(label);

			// THEME
			//
			m_appthemechanged = new EventHandler(OnApplicationThemeChanged);
			ApplicationTheme.Changed += m_appthemechanged;
			OnApplicationThemeChanged(this, EventArgs.Empty);

			// Wire up the event handlers
			MouseClick += OnMouseClick;
			MouseEnter += OnMouseEnter;
			MouseLeave += OnMouseLeave;

			// Windows 11 - Change glyph typeface to Segoe Fluent Icons
			//
			if(VersionHelper.IsWindows11OrGreater())
				label.Font = new Font("Segoe Fluent Icons", label.Font.Size, FontStyle.Regular);

			// Windows 10 - Change glyph typeface to Segoe MDL2 Assets
			//
			else if(VersionHelper.IsWindows10OrGreater())
				label.Font = new Font("Segoe MDL2 Assets", label.Font.Size, FontStyle.Regular);
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
			}

			base.Dispose(disposing);
		}

		//-------------------------------------------------------------------------
		// Events
		//-------------------------------------------------------------------------

		// Toggled
		//
		// Invoked when the control has been toggled
		public event ToggledEventHandler Toggled;

		//-------------------------------------------------------------------
		// Properties
		//-------------------------------------------------------------------

		// IsToggled
		//
		// Exposes the toggled state of the control
		public bool IsToggled => m_toggled;

		//-------------------------------------------------------------------
		// Event Handlers
		//-------------------------------------------------------------------

		// OnApplicationThemeChanged
		//
		// Invoked when the application theme has changed
		private void OnApplicationThemeChanged(object sender, EventArgs args)
		{
			BackColor = ApplicationTheme.PanelBackColor;
			ForeColor = ApplicationTheme.PanelForeColor;
		}

		// OnMouseClick
		//
		// Handles the MouseClick event
		private void OnMouseClick(object sender, EventArgs args)
		{
			m_toggled = !m_toggled;             // Invert the toggle state

			// Update the toggle state if the mouse isn't in the control
			if(!ClientRectangle.Contains(PointToClient(Cursor.Position)))
				OnMouseLeave(sender, args);

			Toggled?.Invoke(this, new ToggledEventArgs(m_toggled));
		}

		// OnMouseEnter
		//
		// Handles the MouseEnter event
		private void OnMouseEnter(object sender, EventArgs args)
		{
			ForeColor = ApplicationTheme.InvertedPanelForeColor;
			BackColor = ApplicationTheme.InvertedPanelBackColor;
		}

		// OnMouseLeave
		//
		// Handles the MouseLeave event
		private void OnMouseLeave(object sender, EventArgs args)
		{
			if(!m_toggled)
			{
				ForeColor = ApplicationTheme.PanelForeColor;
				BackColor = ApplicationTheme.PanelBackColor;
			}
			else
			{
				ForeColor = ApplicationTheme.InvertedPanelForeColor;
				BackColor = ApplicationTheme.InvertedPanelBackColor;
			}
		}

		//-------------------------------------------------------------------
		// Private Member Variables
		//-------------------------------------------------------------------

		private readonly EventHandler m_appthemechanged;
		private bool m_toggled = false;
	}
}
