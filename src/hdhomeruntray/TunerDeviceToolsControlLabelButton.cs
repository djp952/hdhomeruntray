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
	// Class TunerDeviceToolsControlLabelButton (internal)
	//
	// Implements a button for the tuner device tools control strip

	internal partial class TunerDeviceToolsControlLabelButton : RoundedTableLayoutPanel
	{
		// Instance Constructor (protected)
		//
		public TunerDeviceToolsControlLabelButton(string text, SizeF scalefactor)
		{
			// There is no Forms Designer for this component, set the required
			// properties manually during construction
			AutoSize = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			BackColor = SystemColors.ControlLightLight;
			Dock = DockStyle.Fill;
			Padding = new Padding(4).ScaleDPI(scalefactor);

			// Create the label control for the glyph
			PassthroughLabelControl label = new PassthroughLabelControl
			{
				AutoSize = true,
				Size = new Size(1, 1),
				Text = text,
				TextAlign = ContentAlignment.MiddleCenter,
				Dock = DockStyle.Fill,
				Font = new Font("Segoe UI Semibold", 8.25F, FontStyle.Bold),
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

			// Windows 11 - Change label typeface to Segoe UI Variable Display Semib
			//
			if(VersionHelper.IsWindows11OrGreater())
				label.Font = new Font("Segoe UI Variable Display Semib", label.Font.Size, label.Font.Style);
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

		// Selected
		//
		// Invoked when a button-type PopupItemControl has been selected
		public event EventHandler Selected;

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
			Selected?.Invoke(this, EventArgs.Empty);
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
			ForeColor = ApplicationTheme.PanelForeColor;
			BackColor = ApplicationTheme.PanelBackColor;
		}

		//-------------------------------------------------------------------
		// Private Member Variables
		//-------------------------------------------------------------------

		private readonly EventHandler m_appthemechanged;
	}
}
