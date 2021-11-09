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
using System.Runtime.InteropServices;

namespace zuki.hdhomeruntray
{
	//-----------------------------------------------------------------------
	// Class VersionHelper (internal)
	//
	// Class used to implement the Win32 API <versionhelper.h> macros
	//
	// Source: https://stackoverflow.com/questions/31550444/c-sharp-how-to-use-the-new-version-helper-api

	static class VersionHelper
    {
		#region Win32 API Declarations
		private static class NativeMethods
		{
			public const byte VER_EQUAL = 1;
			public const byte VER_GREATER = 2;
			public const byte VER_GREATER_EQUAL = 3;
			public const byte VER_LESS = 4;
			public const byte VER_LESS_EQUAL = 5;
			public const byte VER_AND = 6;
			public const byte VER_OR = 7;

			public const byte VER_CONDITION_MASK = 7;
			public const byte VER_NUM_BITS_PER_CONDITION_MASK = 3;

			public const uint VER_MINORVERSION = 0x0000001;
			public const uint VER_MAJORVERSION = 0x0000002;
			public const uint VER_BUILDNUMBER = 0x0000004;
			public const uint VER_PLATFORMID = 0x0000008;
			public const uint VER_SERVICEPACKMINOR = 0x0000010;
			public const uint VER_SERVICEPACKMAJOR = 0x0000020;
			public const uint VER_SUITENAME = 0x0000040;
			public const uint VER_PRODUCT_TYPE = 0x0000080;

			public const byte VER_NT_DOMAIN_CONTROLLER = 0x0000002;
			public const byte VER_NT_SERVER = 0x0000003;
			public const byte VER_NT_WORKSTATION = 0x0000001;

			public const ushort _WIN32_WINNT_NT4 = 0x0400;
			public const ushort _WIN32_WINNT_WIN2K = 0x0500;
			public const ushort _WIN32_WINNT_WINXP = 0x0501;
			public const ushort _WIN32_WINNT_WS03 = 0x0502;
			public const ushort _WIN32_WINNT_WIN6 = 0x0600;
			public const ushort _WIN32_WINNT_VISTA = 0x0600;
			public const ushort _WIN32_WINNT_WS08 = 0x0600;
			public const ushort _WIN32_WINNT_LONGHORN = 0x0600;
			public const ushort _WIN32_WINNT_WIN7 = 0x0601;
			public const ushort _WIN32_WINNT_WIN8 = 0x0602;
			public const ushort _WIN32_WINNT_WINBLUE = 0x0603;
			public const ushort _WIN32_WINNT_WINTHRESHOLD = 0x0A00;
			public const ushort _WIN32_WINNT_WIN10 = 0x0A00;

			public const bool FALSE = false;

			public static byte LOBYTE(ushort w)
			{
				return ((byte)(w & 0xff));
			}

			public static byte HIBYTE(ushort w)
			{
				return ((byte)(w >> 8 & 0xff));
			}

			[DllImport("kernel32.dll")]
			public static extern ulong VerSetConditionMask(ulong ConditionMask, uint TypeMask, byte Condition);

			[DllImport("kernel32.dll")]
			public static extern bool VerifyVersionInfoW(ref OSVERSIONINFOEXW lpVersionInformation, uint dwTypeMask, ulong dwlConditionMask);

			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			public struct OSVERSIONINFOEXW
			{
				public int dwOSVersionInfoSize;
				public int dwMajorVersion;
				public int dwMinorVersion;
				public int dwBuildNumber;
				public int dwPlatformId;
				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
				public string szCSDVersion;
				public UInt16 wServicePackMajor;
				public UInt16 wServicePackMinor;
				public UInt16 wSuiteMask;
				public byte wProductType;
				public byte wReserved;
			}
		}
		#endregion

		// TODO: comments

		public static bool IsWindowsVersionOrGreater(ushort wMajorVersion, ushort wMinorVersion, ushort wServicePackMajor)
		{
			var osvi = new NativeMethods.OSVERSIONINFOEXW
			{
				dwOSVersionInfoSize = Marshal.SizeOf(typeof(NativeMethods.OSVERSIONINFOEXW))
			};

			var dwlConditionMask = NativeMethods.VerSetConditionMask(
				NativeMethods.VerSetConditionMask(
				NativeMethods.VerSetConditionMask(
					0, NativeMethods.VER_MAJORVERSION, NativeMethods.VER_GREATER_EQUAL),
					   NativeMethods.VER_MINORVERSION, NativeMethods.VER_GREATER_EQUAL),
					   NativeMethods.VER_SERVICEPACKMAJOR, NativeMethods.VER_GREATER_EQUAL);

			osvi.dwMajorVersion = wMajorVersion;
			osvi.dwMinorVersion = wMinorVersion;
			osvi.wServicePackMajor = wServicePackMajor;

			return NativeMethods.VerifyVersionInfoW(ref osvi, NativeMethods.VER_MAJORVERSION | NativeMethods.VER_MINORVERSION | NativeMethods.VER_SERVICEPACKMAJOR, dwlConditionMask) != NativeMethods.FALSE;
		}

		public static bool IsWindowsXPOrGreater()
		{
			return IsWindowsVersionOrGreater(NativeMethods.HIBYTE(NativeMethods._WIN32_WINNT_WINXP), NativeMethods.LOBYTE(NativeMethods._WIN32_WINNT_WINXP), 0);
		}

		public static bool IsWindowsXPSP1OrGreater()
		{
			return IsWindowsVersionOrGreater(NativeMethods.HIBYTE(NativeMethods._WIN32_WINNT_WINXP), NativeMethods.LOBYTE(NativeMethods._WIN32_WINNT_WINXP), 1);
		}

		public static bool IsWindowsXPSP2OrGreater()
		{
			return IsWindowsVersionOrGreater(NativeMethods.HIBYTE(NativeMethods._WIN32_WINNT_WINXP), NativeMethods.LOBYTE(NativeMethods._WIN32_WINNT_WINXP), 2);
		}

		public static bool IsWindowsXPSP3OrGreater()
		{
			return IsWindowsVersionOrGreater(NativeMethods.HIBYTE(NativeMethods._WIN32_WINNT_WINXP), NativeMethods.LOBYTE(NativeMethods._WIN32_WINNT_WINXP), 3);
		}

		public static bool IsWindowsVistaOrGreater()
		{
			return IsWindowsVersionOrGreater(NativeMethods.HIBYTE(NativeMethods._WIN32_WINNT_VISTA), NativeMethods.LOBYTE(NativeMethods._WIN32_WINNT_VISTA), 0);
		}

		public static bool IsWindowsVistaSP1OrGreater()
		{
			return IsWindowsVersionOrGreater(NativeMethods.HIBYTE(NativeMethods._WIN32_WINNT_VISTA), NativeMethods.LOBYTE(NativeMethods._WIN32_WINNT_VISTA), 1);
		}

		public static bool IsWindowsVistaSP2OrGreater()
		{
			return IsWindowsVersionOrGreater(NativeMethods.HIBYTE(NativeMethods._WIN32_WINNT_VISTA), NativeMethods.LOBYTE(NativeMethods._WIN32_WINNT_VISTA), 2);
		}

		public static bool IsWindows7OrGreater()
		{
			return IsWindowsVersionOrGreater(NativeMethods.HIBYTE(NativeMethods._WIN32_WINNT_WIN7), NativeMethods.LOBYTE(NativeMethods._WIN32_WINNT_WIN7), 0);
		}

		public static bool IsWindows7SP1OrGreater()
		{
			return IsWindowsVersionOrGreater(NativeMethods.HIBYTE(NativeMethods._WIN32_WINNT_WIN7), NativeMethods.LOBYTE(NativeMethods._WIN32_WINNT_WIN7), 1);
		}

		public static bool IsWindows8OrGreater()
		{
			return IsWindowsVersionOrGreater(NativeMethods.HIBYTE(NativeMethods._WIN32_WINNT_WIN8), NativeMethods.LOBYTE(NativeMethods._WIN32_WINNT_WIN8), 0);
		}

		public static bool IsWindows8Point1OrGreater()
		{
			return IsWindowsVersionOrGreater(NativeMethods.HIBYTE(NativeMethods._WIN32_WINNT_WINBLUE), NativeMethods.LOBYTE(NativeMethods._WIN32_WINNT_WINBLUE), 0);
		}

		public static bool IsWindowsThresholdOrGreater()
		{
			return IsWindowsVersionOrGreater(NativeMethods.HIBYTE(NativeMethods._WIN32_WINNT_WINTHRESHOLD), NativeMethods.LOBYTE(NativeMethods._WIN32_WINNT_WINTHRESHOLD), 0);
		}

		public static bool IsWindows10OrGreater()
		{
			return IsWindowsVersionOrGreater(NativeMethods.HIBYTE(NativeMethods._WIN32_WINNT_WINTHRESHOLD), NativeMethods.LOBYTE(NativeMethods._WIN32_WINNT_WINTHRESHOLD), 0);
		}

		// TODO: Revisit this when <versionhelpers.h> has this macro implemented
		public static bool IsWindows11OrGreater()
		{
			var osvi = new NativeMethods.OSVERSIONINFOEXW
			{
				dwOSVersionInfoSize = Marshal.SizeOf(typeof(NativeMethods.OSVERSIONINFOEXW))
			};

			var dwlConditionMask = NativeMethods.VerSetConditionMask(
				NativeMethods.VerSetConditionMask(
				NativeMethods.VerSetConditionMask(
					0, NativeMethods.VER_MAJORVERSION, NativeMethods.VER_GREATER_EQUAL),
					   NativeMethods.VER_MINORVERSION, NativeMethods.VER_GREATER_EQUAL),
					   NativeMethods.VER_BUILDNUMBER, NativeMethods.VER_GREATER_EQUAL);

			osvi.dwMajorVersion = NativeMethods.HIBYTE(NativeMethods._WIN32_WINNT_WINTHRESHOLD);
			osvi.dwMinorVersion = NativeMethods.LOBYTE(NativeMethods._WIN32_WINNT_WINTHRESHOLD);
			osvi.dwBuildNumber = 22000;

			return NativeMethods.VerifyVersionInfoW(ref osvi, NativeMethods.VER_MAJORVERSION | NativeMethods.VER_MINORVERSION | NativeMethods.VER_BUILDNUMBER, dwlConditionMask) != NativeMethods.FALSE;
		}

		public static bool IsWindowsServer()
		{
			var osvi = new NativeMethods.OSVERSIONINFOEXW
			{
				dwOSVersionInfoSize = Marshal.SizeOf(typeof(NativeMethods.OSVERSIONINFOEXW)),
				wProductType = NativeMethods.VER_NT_WORKSTATION
			};

			var dwlConditionMask = NativeMethods.VerSetConditionMask(0, NativeMethods.VER_PRODUCT_TYPE, NativeMethods.VER_EQUAL);

			return !NativeMethods.VerifyVersionInfoW(ref osvi, NativeMethods.VER_PRODUCT_TYPE, dwlConditionMask);
		}
	}
}