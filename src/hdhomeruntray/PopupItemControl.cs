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

using zuki.hdhomeruntray.discovery;
using zuki.hdhomeruntray.Properties;

namespace zuki.hdhomeruntray
{
	//-----------------------------------------------------------------------
	// Class PopupItemControl (internal)
	//
	// Implements the user control that displays the status of an individual
	// HDHomeRun device in the pop-up status form
	//
	// TODO: clean up
	// TODO: refactor into two separate classes (PopupItemDeviceControl / PopupItemGlyphControl)

	partial class PopupItemControl : UserControl
	{
		public PopupItemControl(Device device) : this(device, PopupItemControlType.Static)
		{
		}

		// Instance Constructor (Device)
		//
		public PopupItemControl(Device device, PopupItemControlType type) : this(type)
		{
			m_device = device;

			// Create the name label for the control
			var name = new PassthroughLabelControl
			{
				AutoSize = true,
				Size = new Size(1, 1),
				Text = device.FriendlyName,
				TextAlign = ContentAlignment.BottomCenter,
				Dock = DockStyle.Left,
				Font = VersionHelper.IsWindows11OrGreater() ? new Font("Segoe UI Variable Display Semib", 9F, FontStyle.Regular) : 
					new Font("Segoe UI Semibold", 9F, FontStyle.Regular),
				Visible = true
			};

			// Add the device name label to the device panel
			m_layoutpanel.Controls.Add(name);

			// Determine the number of dots to display; for tuners this will be the
			// number of tuners within the device; otherwise just use one dot
			int numdots = 1;
			if(m_device is TunerDevice tunerdevice) numdots = tunerdevice.Tuners.Count;

			// Create the dot labels
			m_dots = new Label[numdots];
			for(int index = 0; index < numdots; index++)
			{
				m_dots[index] = new PassthroughLabelControl
				{
					AutoSize = true,
					Size = new Size(1, 1),
					Text = "●",                                 // U+25CF
					TextAlign = ContentAlignment.BottomCenter,
					ForeColor = SystemColors.GrayText,
					Dock = DockStyle.Left,
					Font = new Font("Segoe UI Symbol", 9F, FontStyle.Regular),
					Visible = true
				};

				// Add the dot label to the device panel
				m_layoutpanel.Controls.Add(m_dots[index]);
			}

			this.Refresh();				// Perform an initial refresh to update the colors
		}

		public PopupItemControl(SymbolGlyph glyph) : this(glyph, PopupItemControlType.Static)
		{
		}

		// Instance Constructor (SymbolGlyph)
		//
		public PopupItemControl(SymbolGlyph glyph, PopupItemControlType type) : this(type)
		{
			var label = new PassthroughLabelControl
			{
				AutoSize = true,
				Size = new Size(1, 1),
				Text = new string((char)glyph, 1),
				TextAlign = ContentAlignment.BottomCenter,
				Dock = DockStyle.Left,
				Font = new Font("Symbols", 11.25F, FontStyle.Regular),
				Visible = true
			};

			// TODO: Figure out how to do semibold, probably need an unmanaged HFONT here.
			//       These "Regular" weights are too thin compared to the device instances
			//       (http://www.pinvoke.net/default.aspx/gdi32.createfont)
			
			// Windows 11 - Change glyph typeface to Segoe Fluent Icons
			//
			if(VersionHelper.IsWindows11OrGreater())
				label.Font = new Font("Segoe Fluent Icons", label.Font.Size, FontStyle.Regular);

			// Windows 10 - Change glyph typeface to Segoe MDL2 Assets
			//
			else if(VersionHelper.IsWindows10OrGreater())
				label.Font = new Font("Segoe MDL2 Assets", label.Font.Size, FontStyle.Regular);

			// Add the device name label to the device panel
			m_layoutpanel.Controls.Add(label);

			this.Refresh();             // Perform an initial refresh to update the colors
		}

		//-------------------------------------------------------------------------
		// Events
		//-------------------------------------------------------------------------

		// Selected
		//
		// Invoked when a non-static PopupItemControl has been selected
		public event PopupItemSelectedEventHandler Selected;

		//-------------------------------------------------------------------------
		// Member Functions
		//-------------------------------------------------------------------------

		// NoDevices
		//
		// Returns a special instance of PopupItemControl that indicates there are
		// no detected HDHomeRun devices
		public static PopupItemControl NoDevices()
		{
			PopupItemControl nodevices = new PopupItemControl();

			// Create the panel control for the static label
			var devicepanel = new RoundedFlowLayoutPanel
			{
				AutoSize = true,
				AutoSizeMode = AutoSizeMode.GrowAndShrink,
				FlowDirection = FlowDirection.LeftToRight,
				WrapContents = false,
				BackColor = SystemColors.ControlLightLight,
				Padding = new Padding(8),
				Radius = 16
			};

			// Create the static label
			var name = new Label
			{
				AutoSize = true,
				Size = new Size(1, 1),
				Text = "No HDHomeRun devices detected",
				TextAlign = ContentAlignment.BottomCenter,
				Dock = DockStyle.Left,
				Font = VersionHelper.IsWindows11OrGreater() ? new Font("Segoe UI Variable Display Semib", 9F, FontStyle.Regular) :
					new Font("Segoe UI Semibold", 9F, FontStyle.Regular),
				Visible = true
			};

			// Add the static label to the panel and the panel to the layout panel
			devicepanel.Controls.Add(name);
			nodevices.m_controlspanel.Controls.Add(devicepanel);

			return nodevices;
		}

		//-------------------------------------------------------------------------
		// Control Overrides
		//-------------------------------------------------------------------------

		// Refresh
		//
		// Overrides Control::Refresh
		public override void Refresh()
		{
			// No device; this is a static or glyph instance
			if(m_device == null) return;

			// TunerDevice
			//
			if(m_device is TunerDevice tunerdevice)
			{
				for(int index = 0; index < tunerdevice.Tuners.Count; index++)
				{
					// Get the granular tuner status from the device
					TunerStatus status = tunerdevice.GetTunerStatus(index);
					
					// Default to SignalQualityColor, but try to obey the setting
					Color forecolor = status.SignalQualityColor;
					switch(Settings.Default.TunerStatusColorSource)
					{
						case TunerStatusColorSource.SignalStrength:
							forecolor = status.SignalStrengthColor;
							break;

						case TunerStatusColorSource.SignalQuality:
							forecolor = status.SignalQualityColor;
							break;

						case TunerStatusColorSource.SymbolQuality:
							forecolor = status.SymbolQualityColor;
							break;							
					}

					m_dots[index].ForeColor = forecolor;
				}
			}

			// StorageDevice
			//
			else if(m_device is StorageDevice storagedevice)
			{
				// Get the granular storage status from the device
				StorageStatus status = storagedevice.GetStorageStatus();

				// The storage device only gets one dot for the overall status
				m_dots[0].ForeColor = status.StatusColor;
			}

			base.Refresh();
		}

		//-------------------------------------------------------------------
		// Event Handlers
		//-------------------------------------------------------------------

		// OnMouseClickButton
		//
		// Handles the MouseClick event for button-type controls
		private void OnMouseClickButton(object sender, EventArgs args)
		{
			Selected?.Invoke(this, new PopupItemSelectedEventArgs());
		}

		// OnMouseEnterButton
		//
		// Handles the MouseEnter event for button-type controls
		private void OnMouseEnterButton(object sender, EventArgs args)
		{
			Debug.Assert(sender is RoundedFlowLayoutPanel);
			RoundedFlowLayoutPanel panel = (RoundedFlowLayoutPanel)sender;

			panel.ForeColor = Color.White;
			panel.BackColor = SystemColors.ControlDark;
		}

		// OnMouseLeaveButton
		//
		// Handles the MouseLeave event for button-type controls
		private void OnMouseLeaveButton(object sender, EventArgs args)
		{
			Debug.Assert(sender is RoundedFlowLayoutPanel);
			RoundedFlowLayoutPanel panel = (RoundedFlowLayoutPanel)sender;

			panel.ForeColor = Color.Black;
			panel.BackColor = SystemColors.ControlLightLight;
		}

		// OnMouseClickToggle
		//
		// Handles the MouseClick event for toggle-type controls
		private void OnMouseClickToggle(object sender, EventArgs args)
		{
			m_toggled = !m_toggled;             // Invert the toggle state
			OnMouseLeaveToggle(sender, args);	// Update the toggle state
		}

		// OnMouseEnterToggle
		//
		// Handles the MouseEnter event for toggle-type controls
		private void OnMouseEnterToggle(object sender, EventArgs args)
		{
			Debug.Assert(sender is RoundedFlowLayoutPanel);
			RoundedFlowLayoutPanel panel = (RoundedFlowLayoutPanel)sender;

			panel.ForeColor = Color.White;
			panel.BackColor = SystemColors.ControlDark;
		}

		// OnMouseLeaveToggle
		//
		// Handles the MosueLeave event for toggle-type controls
		private void OnMouseLeaveToggle(object sender, EventArgs args)
		{
			Debug.Assert(sender is RoundedFlowLayoutPanel);
			RoundedFlowLayoutPanel panel = (RoundedFlowLayoutPanel)sender;

			if(!m_toggled)
			{
				panel.ForeColor = Color.Black;
				panel.BackColor = SystemColors.ControlLightLight;
			}
			else
			{
				panel.ForeColor = Color.White;
				panel.BackColor = SystemColors.ControlDark;
			}
		}

		//-------------------------------------------------------------------
		// Private Member Functions
		//-------------------------------------------------------------------

		// Instance Constructor (private)
		//
		private PopupItemControl()
		{
			InitializeComponent();
		}

		// Instance Constructor (private)
		//
		private PopupItemControl(PopupItemControlType type) : this()
		{
			m_type = type;			// Store the type of control being created

			// Create the inner RoundedFlowLayoutPanel control
			m_layoutpanel = new RoundedFlowLayoutPanel
			{
				AutoSize = true,
				AutoSizeMode = AutoSizeMode.GrowAndShrink,
				FlowDirection = FlowDirection.LeftToRight,
				WrapContents = false,
				ForeColor = Color.Black,
				BackColor = SystemColors.ControlLightLight,
				Padding = new Padding(8),
				Radius = 16,
			};

			// Button type
			if(m_type == PopupItemControlType.Button)
			{
				m_layoutpanel.MouseClick += OnMouseClickButton;
				m_layoutpanel.MouseEnter += OnMouseEnterButton;
				m_layoutpanel.MouseLeave += OnMouseLeaveButton;
			}

			// Toggle type
			else if(m_type == PopupItemControlType.Toggle)
			{
				m_layoutpanel.MouseClick += OnMouseClickToggle;
				m_layoutpanel.MouseEnter += OnMouseEnterToggle;
				m_layoutpanel.MouseLeave += OnMouseLeaveToggle;
			}

			// Add the inner RoundedFlowLayoutPanel to the output FlowLayoutPanel
			m_controlspanel.Controls.Add(m_layoutpanel);
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private readonly PopupItemControlType	m_type;				// Control type
		private readonly RoundedFlowLayoutPanel m_layoutpanel;		// Layout panel
		private bool							m_toggled = false;	// Toggled flag


		private readonly Device		m_device = null;	// Referenced device object
		private readonly Label[]	m_dots;				// Status dot labels
	}
}
