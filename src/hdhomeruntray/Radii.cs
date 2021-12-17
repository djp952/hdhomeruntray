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

namespace zuki.hdhomeruntray
{
	//-----------------------------------------------------------------------
	// Struct Radii (internal)
	//
	// Value type for use with the "Rounded" panel user controls.  Does not
	// implement all of the stuff publically consumable value types should
	// (GetHashCode, Equals, etc.)  TODO: Go back and implement those
	//
	// Based on .NET Foundation .NET Windows Forms "Padding":
	// https://github.com/dotnet/winforms
	//
	// Licensed to the .NET Foundation under one or more agreements.
	// The .NET Foundation licenses this file to you under the MIT license.
	// See the LICENSE file in the project root for more information.

	[TypeConverter(typeof(RadiiConverter))]
	[Serializable]
	internal struct Radii
	{
		// Instance Constructor
		//
		public Radii(int all)
		{
			m_all = true;
			m_topleft = m_topright = m_bottomright = m_bottomleft = all;
		}

		// Instance Constructor
		//
		public Radii(int topleft, int topright, int bottomright, int bottomleft)
		{
			m_topleft = topleft;
			m_topright = topright;
			m_bottomright = bottomright;
			m_bottomleft = bottomleft;
			m_all = m_topleft == m_topright && m_topleft == m_bottomright && m_topleft == m_bottomleft;
		}

		//-------------------------------------------------------------------
		// Properties
		//-------------------------------------------------------------------

		// All
		//
		// Sets all of the radii to the same value
		[RefreshProperties(RefreshProperties.All)]
		public int All
		{
			get => m_all ? m_topleft : -1;
			set
			{
				if(m_all != true || m_topleft != value)
				{
					m_all = true;
					m_topleft = m_topright = m_bottomright = m_bottomleft = value;
				}
			}
		}

		// BottomLeft
		//
		// Sets the radius of the bottom left corner
		[RefreshProperties(RefreshProperties.All)]
		public int BottomLeft
		{
			get => m_all ? m_topleft : m_bottomleft;
			set
			{
				if(m_all || m_bottomleft != value)
				{
					m_all = false;
					m_bottomleft = value;
				}
			}
		}

		// BottomRight
		//
		// Sets the radius of the bottom right corner
		[RefreshProperties(RefreshProperties.All)]
		public int BottomRight
		{
			get => m_all ? m_topleft : m_bottomright;
			set
			{
				if(m_all || m_bottomright != value)
				{
					m_all = false;
					m_bottomright = value;
				}
			}
		}

		// TopLeft
		//
		// Sets the radius of the top left corner
		[RefreshProperties(RefreshProperties.All)]
		public int TopLeft
		{
			get => m_topleft;
			set
			{
				if(m_all || m_topleft != value)
				{
					m_all = false;
					m_topleft = value;
				}
			}
		}

		// TopRight
		//
		// Sets the radius of the top right corner
		[RefreshProperties(RefreshProperties.All)]
		public int TopRight
		{
			get => m_all ? m_topleft : m_topright;
			set
			{
				if(m_all || m_topright != value)
				{
					m_all = false;
					m_topright = value;
				}
			}
		}

		//-------------------------------------------------------------------
		// Fields
		//-------------------------------------------------------------------

		public static readonly Radii Empty = new Radii(0);

		//-------------------------------------------------------------------
		// Internal Member Functions
		//-------------------------------------------------------------------

		// ShouldSerializeAll
		//
		// Indicates to RadiiConverter that m_all should be serialized
		internal bool ShouldSerializeAll()
		{
			return m_all;
		}

		//-------------------------------------------------------------------
		// Member Variables
		//-------------------------------------------------------------------

		private bool m_all;
		private int m_topleft;
		private int m_topright;
		private int m_bottomright;
		private int m_bottomleft;
	}
}
