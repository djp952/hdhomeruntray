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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;

namespace zuki.hdhomeruntray
{
	//-----------------------------------------------------------------------
	// Class ShellNotifyIcon (internal)
	//
	// Component that creates an icon in the Windows System Tray
	//
	// Based on .NET Foundation .NET Windows Forms "NotifyIcon":
	// https://github.com/dotnet/winforms
	//
	// Licensed to the .NET Foundation under one or more agreements.
	// The .NET Foundation licenses this file to you under the MIT license.
	// See the LICENSE file in the project root for more information.

	internal class ShellNotifyIcon : Component, IWin32Window
	{
		#region Win32 API Declarations
		/// <summary>
		/// Win32 API Declarations
		/// </summary>
		private static class NativeMethods
		{
			public const uint NIF_MESSAGE = 0x00000001;
			public const uint NIF_ICON = 0x00000002;
			public const uint NIF_TIP = 0x00000004;
			public const uint NIF_STATE = 0x00000008;
			public const uint NIF_INFO = 0x00000010;
			public const uint NIF_GUID = 0x00000020;
			public const uint NIF_REALTIME = 0x00000040;
			public const uint NIF_SHOWTIP = 0x00000080;

			public const uint NIIF_NONE = 0x00000000;
			public const uint NIIF_INFO = 0x00000001;
			public const uint NIIF_WARNING = 0x00000002;
			public const uint NIIF_ERROR = 0x00000003;
			public const uint NIIF_USER = 0x00000004;
			public const uint NIIF_ICON_MASK = 0x0000000F;
			public const uint NIIF_NOSOUND = 0x00000010;
			public const uint NIIF_LARGE_ICON = 0x00000020;
			public const uint NIIF_RESPECT_QUIET_TIME = 0x00000080;

			public const uint NIM_ADD = 0x00000000;
			public const uint NIM_MODIFY = 0x00000001;
			public const uint NIM_DELETE = 0x00000002;
			public const uint NIM_SETFOCUS = 0x00000003;
			public const uint NIM_SETVERSION = 0x00000004;

			public const uint NIN_SELECT = WM_USER + 0;
			public const uint NIN_KEYSELECT = (NIN_SELECT | NINF_KEY);
			public const uint NIN_BALLOONSHOW = WM_USER + 2;
			public const uint NIN_BALLOONHIDE = WM_USER + 3;
			public const uint NIN_BALLOONTIMEOUT = WM_USER + 4;
			public const uint NIN_BALLOONUSERCLICK = WM_USER + 5;
			public const uint NIN_POPUPOPEN = WM_USER + 6;
			public const uint NIN_POPUPCLOSE = WM_USER + 7;

			public const uint NINF_KEY = 0x1;

			public const uint NOTIFYICON_VERSION_4 = 0x4;

			public const uint WM_CLOSE = 0x0010;
			public const uint WM_CONTEXTMENU = 0x007B;
			public const uint WM_DESTROY = 0x0002;
			public const uint WM_INITMENUPOPUP = 0x0117;
			public const uint WM_MOUSEMOVE = 0x0200;
			public const uint WM_USER = 0x0400;

			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			public struct NOTIFYICONDATAW
			{
				public uint cbSize;
				public IntPtr hWnd;
				public uint uID;
				public uint uFlags;
				public uint uCallbackMessage;
				public IntPtr hIcon;
				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
				public string szTip;
				public uint dwState;
				public uint dwStateMask;
				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
				public string szInfo;
				public uint uVersion;
				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
				public string szInfoTitle;
				public uint dwInfoFlags;
				public Guid guidItem;
				public IntPtr hBalloonIcon;
			}

			[StructLayout(LayoutKind.Sequential)]
			public struct NOTIFYICONIDENTIFIER
			{
				public uint cbSize;
				public IntPtr hWnd;
				public uint uID;
				public Guid guidItem;
			}

			[StructLayout(LayoutKind.Sequential)]
			public struct RECT
			{
				public int left;
				public int top;
				public int right;
				public int bottom;
			}

			public interface IHandle
			{
				IntPtr Handle { get; }
			}

			public static short GET_X_LPARAM(IntPtr value)
			{
				return unchecked((short)(long)value);
			}

			public static short GET_Y_LPARAM(IntPtr value)
			{
				return unchecked((short)((long)value >> 16));
			}

			public static ushort HIWORD(IntPtr value)
			{
				return unchecked((ushort)((ulong)value >> 16));
			}

			public static ushort LOWORD(IntPtr value)
			{
				return unchecked((ushort)(ulong)value);
			}

			[DllImport("user32.dll")]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool GetCursorPos(out Point pt);

			[DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			private static extern bool PostMessageW(IntPtr hWnd, uint Msg, IntPtr wParam = default, IntPtr lParam = default);

			public static bool PostMessageW(IHandle hWnd, uint Msg, IntPtr wParam = default, IntPtr lParam = default)
			{
				bool result = PostMessageW(hWnd.Handle, Msg, wParam, lParam);
				GC.KeepAlive(hWnd);
				return result;
			}

			[DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
			public static extern uint RegisterWindowMessageW(string lpString);

			[DllImport("user32.dll")]
			[return: MarshalAs(UnmanagedType.Bool)]
			private static extern bool SetForegroundWindow(IntPtr hWnd);

			public static bool SetForegroundWindow(IHandle hWnd)
			{
				bool result = SetForegroundWindow(hWnd.Handle);
				GC.KeepAlive(hWnd);
				return result;
			}

			[DllImport("shell32.dll")]
			public static extern int Shell_NotifyIconGetRect(ref NOTIFYICONIDENTIFIER identifier, out RECT iconLocation);

			[DllImport("shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool Shell_NotifyIconW(uint dwMessage, ref NOTIFYICONDATAW lpData);
		}
		#endregion

		// Instance Constructor
		//
		private ShellNotifyIcon()
		{
		}

		// Instance Constructor
		//
		private ShellNotifyIcon(IContainer container)
		{
			if(container != null) container.Add(this);
		}

		// Instance Constructor
		//
		public ShellNotifyIcon(Guid guid) : this(guid, null)
		{
		}

		// Instance Constructor
		//
		public ShellNotifyIcon(Guid guid, IContainer container) : this(container)
		{
			m_backingwindow = new BackingWindow(this);
			m_guid = guid;

			// Create the one-shot hover start timer
			m_hoverstart = new System.Timers.Timer()
			{
				AutoReset = false,
			};
			m_hoverstart.Elapsed += new ElapsedEventHandler(OnHoverStart);

			// Create the repeating hover stop timer
			m_hoverstop = new System.Timers.Timer
			{
				AutoReset = true,
				Interval = 25,          // Milliseconds
			};
			m_hoverstop.Elapsed += new ElapsedEventHandler(OnHoverStop);

			// Create the repeating timer to unblock WM_MOUSEMOVE
			m_blockstop = new System.Timers.Timer
			{
				AutoReset = true,
				Interval = 25,          // Milliseconds
			};
			m_blockstop.Elapsed += new ElapsedEventHandler(OnBlockPopupStop);

			UpdateIcon(m_visible);
		}

		// Static Constructor
		//
		static ShellNotifyIcon()
		{
			// This implementation requires access to an internal method of ContextMenuStrip:
			//
			// -> internal void ShowInTaskbar(int x, int y)
			foreach(MethodInfo methodinfo in typeof(ContextMenuStrip).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
			{
				if(string.Compare(methodinfo.Name, "ShowInTaskbar", true) == 0)
				{
					s_contextmenustrip_showintaskbar = methodinfo;
					break;
				}
			}

			Debug.Assert(s_contextmenustrip_showintaskbar != null, "Reflection for method ContextMenuStrip.ShowInTaskBar failed");
		}

		// Dispose
		//
		// Releases unmanaged resources and optionally releases managed resources
		protected override void Dispose(bool disposing)
		{
			// Dispose managed state
			if(disposing)
			{
				if(m_backingwindow != null)
				{
					m_icon = null;
					m_tooltip = null;
					UpdateIcon(false);
					m_backingwindow.DestroyHandle();
					m_backingwindow = null;
					m_contextmenu = null;
				}
			}
			else
			{
				// This same post is done in ControlNativeWindow's finalize method, so if you change
				// it, change it there too.
				//
				if(m_backingwindow != null && m_backingwindow.Handle != IntPtr.Zero)
				{
					NativeMethods.PostMessageW(m_backingwindow, NativeMethods.WM_CLOSE);
					m_backingwindow.ReleaseHandle();
				}
			}

			base.Dispose(disposing);
		}

		//-------------------------------------------------------------------
		// Events
		//-------------------------------------------------------------------

		// BalloonTipClicked
		//
		// Invoked when the balloon tip has been clicked (NIN_BALLOONUSERCLICK)
		public event EventHandler BalloonTipClicked
		{
			add => Events.AddHandler(EVENT_BALLOONTIPCLICKED, value);
			remove => Events.RemoveHandler(EVENT_BALLOONTIPCLICKED, value);
		}

		// BalloonTipClosed
		//
		// Invoked when the balloon tip has been closed or timed out (NIN_BALLOONHIDE | NIN_BALLOONTIMEOUT)
		public event EventHandler BalloonTipClosed
		{
			add => Events.AddHandler(EVENT_BALLOONTIPCLOSED, value);
			remove => Events.RemoveHandler(EVENT_BALLOONTIPCLOSED, value);
		}

		// BalloonTipShown
		//
		// Invoked when the balloon tip has been shown (NIN_BALLOONSHOW)
		public event EventHandler BalloonTipShown
		{
			add => Events.AddHandler(EVENT_BALLOONTIPSHOWN, value);
			remove => Events.RemoveHandler(EVENT_BALLOONTIPSHOWN, value);
		}

		// ClosePopup
		//
		// Invoked when the popup window should be closed (NIN_POPUPCLOSE)
		public event EventHandler ClosePopup
		{
			add => Events.AddHandler(EVENT_CLOSEPOPUP, value);
			remove => Events.RemoveHandler(EVENT_CLOSEPOPUP, value);
		}

		// OpenPopup
		//
		// Invoked when the popup window should be opened (NIN_POPUPOPEN)
		public event EventHandler OpenPopup
		{
			add => Events.AddHandler(EVENT_OPENPOPUP, value);
			remove => Events.RemoveHandler(EVENT_OPENPOPUP, value);
		}

		// Selected
		//
		// Invoked when the notification icon is selected (NIN_SELECT)
		public event EventHandler Selected
		{
			add => Events.AddHandler(EVENT_SELECTED, value);
			remove => Events.RemoveHandler(EVENT_SELECTED, value);
		}

		//-------------------------------------------------------------------
		// Properties
		//-------------------------------------------------------------------

		// ContextMenuStrip
		//
		// Gets or sets the context menu strip for the tray icon
		[DefaultValue(null)]
		public ContextMenuStrip ContextMenuStrip
		{
			get => m_contextmenu;
			set => m_contextmenu = value;
		}

		// Handle (IWin32Window)
		//
		// Exposes the window handle of the NativeWindow
		public IntPtr Handle => ((IWin32Window)m_backingwindow).Handle;

		// Icon
		//
		// Gets or sets the icon to show in the tray
		[Localizable(true), DefaultValue(null)]
		public Icon Icon
		{
			get => m_icon;
			set
			{
				if(m_icon != value)
				{
					m_icon = value;
					UpdateIcon(m_visible);
				}
			}
		}

		// HoverInterval
		//
		// Gets or sets a custom hover interval (disables NIN_POPUPxxx messages)
		[DefaultValue(0)]
		public int HoverInterval
		{
			get => m_hoverinterval;
			set
			{
				if(m_hoverinterval != value)
				{
					m_hoverinterval = value;
					UpdateIcon(m_visible);
				}
			}
		}

		// Tag
		//
		// Gets or sets a user-defined tag object 
		[Localizable(false), Bindable(true), DefaultValue(null), TypeConverter(typeof(StringConverter))]
		public object Tag
		{
			get => m_tag;
			set => m_tag = value;
		}

		// ToolTip
		//
		// Gets or sets the ToolTip text displayed when the mouse hovers over the tray icon
		[Localizable(true), DefaultValue("")]
		public string ToolTip
		{
			get => m_tooltip;
			set
			{
				// Don't allow the value to become null
				if(value == null) value = string.Empty;

				if(value != m_tooltip)
				{
					m_tooltip = value;
					if(m_created) UpdateIcon(m_visible);
				}
			}
		}

		// Visible
		//
		// Gets or sets a value indicating whether the icon is visible in the Windows System Tray.
		[Localizable(true), DefaultValue(false)]
		public bool Visible
		{
			get => m_visible;
			set
			{
				if(m_visible != value)
				{
					UpdateIcon(value);
					m_visible = value;
				}
			}
		}

		//-------------------------------------------------------------------
		// Member Functions
		//-------------------------------------------------------------------

		// GetBounds
		//
		// Gets the bounding rectangle for the tray icon
		public Rectangle GetBounds()
		{
			// If the tray icon has not been created or we are in design mode, bail out
			if(!m_created || DesignMode) return Rectangle.Empty;

			// Create and initialize the required unmanaged structures
			NativeMethods.NOTIFYICONIDENTIFIER identifer = new NativeMethods.NOTIFYICONIDENTIFIER
			{
				cbSize = (uint)Marshal.SizeOf(typeof(NativeMethods.NOTIFYICONIDENTIFIER)),
				hWnd = m_backingwindow.Handle,
				guidItem = m_guid,
			};

			// Attempt to retrive the bounding rectangle for the notify icon
			if(NativeMethods.Shell_NotifyIconGetRect(ref identifer, out NativeMethods.RECT rect) == 0)     // S_OK
				return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);

			// The operation failed; return a default bounding rectangle
			return Rectangle.Empty;
		}

		// ShowNotification
		//
		// Shows a notification with the specified text
		public void ShowNotification(string text)
		{
			ShowNotification(text, null, ToolTipIcon.None, null);
		}

		// ShowNotification
		//
		// Shows a notification with the specified text and title
		public void ShowNotification(string text, string title)
		{
			ShowNotification(text, title, ToolTipIcon.None, null);
		}

		// ShowNotification
		//
		// Shows a notification with the specified text, title, and type
		public void ShowNotification(string text, string title, ToolTipIcon type)
		{
			ShowNotification(text, title, type, null);
		}

		// ShowNotification
		//
		// Shows a notification with the specified title, text, type, and icon
		public void ShowNotification(string text, string title, ToolTipIcon type, Icon icon)
		{
			// If the icon hasn't been created or we're in design mode, bail out
			if(!m_created || DesignMode) return;

			// Initialize the NOTIFYICONDATAW structure for this operation
			NativeMethods.NOTIFYICONDATAW data = new NativeMethods.NOTIFYICONDATAW
			{
				cbSize = (uint)Marshal.SizeOf(typeof(NativeMethods.NOTIFYICONDATAW)),
				uFlags = NativeMethods.NIF_INFO | NativeMethods.NIF_GUID,
				dwInfoFlags = (uint)type,
				guidItem = m_guid,
			};

			// Ensure the backing window object has been created
			if(m_backingwindow.Handle == IntPtr.Zero) m_backingwindow.CreateHandle(new CreateParams());
			data.hWnd = m_backingwindow.Handle;

			// There are upper boundaries on how long the text and title strings can be
			if(!string.IsNullOrEmpty(text)) data.szInfo = string.Concat(text.Take(MAX_INFO));
			if(!string.IsNullOrEmpty(title)) data.szInfoTitle = string.Concat(title.Take(MAX_INFOTITLE));

			// If an icon has been provided, set the proper flag and the handle to the icon
			if(icon != null)
			{
				data.hBalloonIcon = icon.Handle;

				// The icon needs to either be SM_CXICON/SM_CYICON (NIIF_LARGE_ICON) or SM_CXSMICON/SM_CYSMICON in size
				if(icon.Size == SystemInformation.IconSize) data.dwInfoFlags |= (NativeMethods.NIIF_USER | NativeMethods.NIIF_LARGE_ICON);
				else if(icon.Size == SystemInformation.SmallIconSize) data.dwInfoFlags |= (NativeMethods.NIIF_USER);
				else data.hBalloonIcon = IntPtr.Zero;
			}

			// Show the notification
			NativeMethods.Shell_NotifyIconW(NativeMethods.NIM_MODIFY, ref data);
		}

		//-------------------------------------------------------------------
		// Private Data Types
		//-------------------------------------------------------------------

		// Class ShellNotifyIconNativeWindow
		//
		// Placeholder window that the tray icon instance is attached to
		private class BackingWindow : NativeWindow, NativeMethods.IHandle
		{
			// Instance constructor
			//
			public BackingWindow(ShellNotifyIcon component)
			{
				m_component = component;
			}

			// Destructor
			//
			~BackingWindow()
			{
				// Release the handle from our window proc, re-routing it back to the system
				if(Handle != IntPtr.Zero) NativeMethods.PostMessageW(this, NativeMethods.WM_CLOSE);
			}

			// LockReference
			//
			// Locks the object in place to prevent garbage collection while it's in use
			public void LockReference(bool locked)
			{
				if(locked)
				{
					if(!m_gchandle.IsAllocated) m_gchandle = GCHandle.Alloc(m_component, GCHandleType.Normal);
				}
				else
				{
					if(m_gchandle.IsAllocated) m_gchandle.Free();
				}
			}

			// OnThreadException (NativeWindow)
			//
			// Manages an unhandled thread exception
			protected override void OnThreadException(Exception ex)
			{
				// Route the exception to the application
				Application.OnThreadException(ex);
			}

			// WndProc (NativeWindow)
			//
			// Processes window messages
			protected override void WndProc(ref Message message)
			{
				// Pass all messages onto the ShellNotifyIcon component
				m_component.WndProc(ref message);
			}

			private readonly ShellNotifyIcon m_component;   // Owning ShellNotifyIcon component
			private GCHandle m_gchandle;                    // Handle to prevent Garbage collection
		}

		//-------------------------------------------------------------------
		// Private Member Functions
		//-------------------------------------------------------------------

		// OnBalloonTipClicked
		//
		// Raises the BalloonTipClicked event
		private void OnBalloonTipClicked()
		{
			((EventHandler)Events[EVENT_BALLOONTIPCLICKED])?.Invoke(this, EventArgs.Empty);
		}

		// OnBalloonTipClosed
		//
		// Raises the BalloonTipClosed event
		private void OnBalloonTipClosed()
		{
			((EventHandler)Events[EVENT_BALLOONTIPCLOSED])?.Invoke(this, EventArgs.Empty);
		}

		// OnBallonTipShown
		//
		// Raises the BalloonTipShown event
		private void OnBalloonTipShown()
		{
			((EventHandler)Events[EVENT_BALLOONTIPSHOWN])?.Invoke(this, EventArgs.Empty);
		}

		// OnBlockPopupStop
		//
		// Invoked when the popup block stop timer object has come due
		private void OnBlockPopupStop(object sender, ElapsedEventArgs args)
		{
			// If the mouse is no longer in the icon boundaries, unblock the popups
			if(!GetBounds().Contains(Cursor.Position))
			{
				m_blockstop.Stop();
				m_blockpopup = false;
			}
		}

		// OnClosePopup
		//
		// Raises the ClosePopup event
		private void OnClosePopup()
		{
			((EventHandler)Events[EVENT_CLOSEPOPUP])?.Invoke(this, EventArgs.Empty);
		}

		// OnContextMenu
		//
		// Creates and shows the context menu associated with the tray icon
		private void OnContextMenu()
		{
			if(m_contextmenu == null || s_contextmenustrip_showintaskbar == null) return;

			// Find out where the cursor happens to be
			NativeMethods.GetCursorPos(out Point cursorpos);

			// The current window must be made the foreground window before calling TrackPopupMenuEx,
			// and a task switch must be forced after the call
			NativeMethods.SetForegroundWindow(m_backingwindow);

			// Set the context menu strip to be topmost, allowing it to overlap the system tray
			s_contextmenustrip_showintaskbar.Invoke(m_contextmenu, new object[] { cursorpos.X, cursorpos.Y });
		}

		// OnHoverStart
		//
		// Invoked when the hover start timer object has come due
		private void OnHoverStart(object sender, ElapsedEventArgs args)
		{
			// Check if the mouse cursor is still in the icon boundaries
			if(GetBounds().Contains(Cursor.Position))
			{
				// Start the repeating stop timer and open the popup window
				m_hoverstop.Start();
				OnOpenPopup();
			}
		}

		// OnHoverStop
		//
		// Invoked when the hover stop timer object has come due
		private void OnHoverStop(object sender, ElapsedEventArgs args)
		{
			// If the mouse is no longer in the icon boundaries, close the popup
			if(!GetBounds().Contains(Cursor.Position))
			{
				m_hoverstop.Stop();
				OnClosePopup();
			}
		}

		// OnOpenPopup
		//
		// Raises the OpenPopup event
		private void OnOpenPopup()
		{
			((EventHandler)Events[EVENT_OPENPOPUP])?.Invoke(this, EventArgs.Empty);
		}

		// OnSelected
		//
		// Raises the Selected event
		private void OnSelected()
		{
			((EventHandler)Events[EVENT_SELECTED])?.Invoke(this, EventArgs.Empty);
		}

		// OnTaskbarCreated
		//
		// Invoked when the custom TaskbarCreated window message is handled
		private void OnTaskbarCreated()
		{
			m_created = false;
			UpdateIcon(m_visible);
		}

		// UpdateIcon
		//
		// Updates the notify icon in the system tray
		private void UpdateIcon(bool visible)
		{
			lock(m_lock)
			{
				// Watch out for DesignMode here
				if(DesignMode) return;

				// Lock or unlock the NativeWindow in place (prevent GC)
				m_backingwindow.LockReference(visible);

				// If the icon will be visible and the NativeWindow isn't created, create it
				if(visible && (m_backingwindow.Handle == IntPtr.Zero))
					m_backingwindow.CreateHandle(new CreateParams());

				// Initialize the NOTIFYICONDATA structure
				NativeMethods.NOTIFYICONDATAW data = new NativeMethods.NOTIFYICONDATAW
				{
					cbSize = (uint)Marshal.SizeOf(typeof(NativeMethods.NOTIFYICONDATAW)),
					uFlags = NativeMethods.NIF_MESSAGE | NativeMethods.NIF_GUID,
					hWnd = m_backingwindow.Handle,
					uCallbackMessage = WM_TRAYICONMESSAGE,
					guidItem = m_guid,
				};

				// NIF_ICON
				if(m_icon != null)
				{
					data.uFlags |= NativeMethods.NIF_ICON;
					data.hIcon = m_icon.Handle;
				}

				// NIF_TIP (disabled with custom hover interval)
				if((m_tooltip != null) && (m_hoverinterval <= 0))
				{
					data.uFlags |= NativeMethods.NIF_TIP;
					data.szTip = string.Concat(m_tooltip.Take(MAX_TOOLTIP));
				}

				// Create or update the system tray icon
				if(visible && (m_icon != null))
				{
					if(!m_created)
					{
						// Create the tray icon instance
						m_created = NativeMethods.Shell_NotifyIconW(NativeMethods.NIM_ADD, ref data);

						if(m_created)
						{
							// Set the version of the tray icon to NOTIFY_VERSION_4 to enable modern functionality,
							// this only needs to be done once after the NIM_ADD operation
							data.uVersion = NativeMethods.NOTIFYICON_VERSION_4;
							NativeMethods.Shell_NotifyIconW(NativeMethods.NIM_SETVERSION, ref data);
						}
					}

					else NativeMethods.Shell_NotifyIconW(NativeMethods.NIM_MODIFY, ref data);
				}

				// Remove the tray icon if it's not going to be visible
				else if(m_created)
				{
					NativeMethods.Shell_NotifyIconW(NativeMethods.NIM_DELETE, ref data);
					m_created = false;
				}
			}
		}

		// WndProc
		//
		// Processes window messages
		private void WndProc(ref Message message)
		{
			// Handle the custom TaskbarCreated window message first
			if(message.Msg == s_taskbarcreated)
			{
				OnTaskbarCreated();
				return;
			}

			// Normal window message
			switch((uint)message.Msg)
			{
				// WM_DESTROY
				//
				// The application is being destroyed
				case NativeMethods.WM_DESTROY:
					UpdateIcon(false);
					break;

				// WM_TRAYICONMESSAGE
				//
				// Message from the shell directed at the tray icon
				case WM_TRAYICONMESSAGE:

					switch((uint)NativeMethods.LOWORD(message.LParam))
					{
						case NativeMethods.NIN_BALLOONUSERCLICK:
							OnBalloonTipClicked();
							break;

						case NativeMethods.NIN_BALLOONHIDE:
							OnBalloonTipClosed();
							break;

						case NativeMethods.NIN_BALLOONSHOW:
							OnBalloonTipShown();
							break;

						case NativeMethods.NIN_BALLOONTIMEOUT:
							OnBalloonTipClosed();
							break;

						case NativeMethods.NIN_POPUPCLOSE:

							// If this event comes in when a custom hover interval has been set, ignore it
							if(m_hoverinterval <= 0) OnClosePopup();
							break;

						case NativeMethods.NIN_POPUPOPEN:

							// If this event comes in when a custom hover interval has been set, ignore it
							if(m_hoverinterval <= 0) OnOpenPopup();
							break;

						case NativeMethods.NIN_SELECT:
						case NativeMethods.NIN_KEYSELECT:

							// When a custom hover interval is set, the popup events are
							// blocked until the cursor has left the tray icon
							if(m_hoverinterval > 0)
							{
								if(m_hoverstart.Enabled) m_hoverstart.Stop();
								m_blockstop.Start();
								m_blockpopup = true;
							}

							OnSelected();
							break;

						case NativeMethods.WM_CONTEXTMENU:

							// When a custom hover interval is set, the popup events are
							// blocked until the cursor has left the tray icon
							if(m_hoverinterval > 0)
							{
								if(m_hoverstart.Enabled) m_hoverstart.Stop();
								m_blockstop.Start();
								m_blockpopup = true;
							}

							OnContextMenu();
							break;

						case NativeMethods.WM_MOUSEMOVE:

							// This is only applicable if a custom hover interval has been set
							if(m_hoverinterval > 0)
							{
								// A NIN_SELECT or WM_CONTEXTMENU message blocks WM_MOUSEMOVE until the
								// cursor has left the boundaries of the control (WM_MOUSELEAVE is hard)
								if(!m_blockpopup)
								{
									// Check if the mouse cursor is within the boundaries of the tray icon or not
									if(GetBounds().Contains(NativeMethods.GET_X_LPARAM(message.WParam), NativeMethods.GET_Y_LPARAM(message.WParam)))
									{
										// In the icon; if the hover start timer is not already enabled, start it
										if(!m_hoverstart.Enabled)
										{
											m_hoverstart.Interval = m_hoverinterval;
											m_hoverstart.Start();
										}
									}
									else
									{
										// Not in the icon, if the hover start timer is enabled, stop it before 
										// it comes due.  The mouse has to stay within the icon boundaries
										if(m_hoverstart.Enabled) m_hoverstart.Stop();
									}
								}
							}

							break;
					}

					// Don't pass these onto the backing window for processing
					return;
			}

			// Pass the message on to the ShellNotifyIconNativeWindow default WndProc
			m_backingwindow.DefWndProc(ref message);
		}

		//-------------------------------------------------------------------
		// Private Constants
		//-------------------------------------------------------------------

		// MAX_INFO
		//
		// The maximum number of characters available for balloon tip text
		private const int MAX_INFO = 255;

		// MAX_INFOTITLE
		//
		// The maximum number of characters available for balloon tip titles
		private const int MAX_INFOTITLE = 63;

		// MAX_TOOLTIP
		//
		// The maximum number of characters available for tool tip text
		private const int MAX_TOOLTIP = 127;

		// WM_TRAYICONMESSAGE
		//
		// Custom window message used by the shell to sent tray icon messages
		private const uint WM_TRAYICONMESSAGE = NativeMethods.WM_USER + 1024;

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private readonly object m_lock = new object();      // Synchronziation object
		private BackingWindow m_backingwindow = null;       // Backing window
		private bool m_created = false;                     // Creation flag
		private readonly System.Timers.Timer m_hoverstart;  // Pseudo-hover start timer
		private readonly System.Timers.Timer m_hoverstop;   // Pesudo-hover stop time
		private bool m_blockpopup = false;                  // Flag to block popup events
		private readonly System.Timers.Timer m_blockstop;   // Timer to unblock popup events
		private int m_hoverinterval = 0;                    // Pesudo-hover interval
		private readonly Guid m_guid = Guid.NewGuid();      // Icon GUID

		// Property backing variables
		//
		private ContextMenuStrip m_contextmenu = null;      // .ContextMenu
		private Icon m_icon = null;                         // .Icon
		private object m_tag = null;                        // .Tag
		private string m_tooltip = string.Empty;            // .ToolTip
		private bool m_visible = false;                     // .Visible

		// Component Event objects
		//
		private static readonly object EVENT_BALLOONTIPSHOWN = new object();
		private static readonly object EVENT_BALLOONTIPCLICKED = new object();
		private static readonly object EVENT_BALLOONTIPCLOSED = new object();
		private static readonly object EVENT_CLOSEPOPUP = new object();
		private static readonly object EVENT_OPENPOPUP = new object();
		private static readonly object EVENT_SELECTED = new object();

		// Reflected member functions
		//
		private static readonly MethodInfo s_contextmenustrip_showintaskbar = null;

		// Custom window messages
		//
		private static readonly int s_taskbarcreated = (int)NativeMethods.RegisterWindowMessageW("TaskbarCreated");
	}
}
