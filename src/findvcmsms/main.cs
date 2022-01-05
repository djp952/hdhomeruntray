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

//-------------------------------------------------------------------------------
// findvcmsms
//
// Build Tool used to locate the Visual C++ merge modules and
// generate a WiX fragment that references the detected locations
//
// USAGE:
//
//	findvcmsms {basepath} {outfile}
//
//      basepath    - Base path to begin the merge module search
//      outfile		- Path to the output .wxs file
//-------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;

namespace zuki.build.tools
{
	internal class main
	{
		/// <summary>
		/// Shows application usage information
		/// </summary>
		private static void ShowUsage()
		{
			Console.WriteLine();
			Console.WriteLine(AppDomain.CurrentDomain.FriendlyName.ToUpper() + " basedir [-ver:redistver] [-filter:suffix;suffix;suffix] [-out:outfile]");
			Console.WriteLine();
			Console.WriteLine("Searches for Visual C++ Runtime Merge Modules");
			Console.WriteLine();
			Console.WriteLine("  basedir    : Base directory from which to search");
			Console.WriteLine("  -ver       : Visual C++ redist version to use");
			Console.WriteLine("  -filter    : Merge module suffix filter(s); for example CRT_x86;MFC_x86");
			Console.WriteLine("  -out       : Specifies the output .wxs WiX fragment file");
		}

		/// <summary>
		/// Application entry point
		/// </summary>
		/// <param name="arguments">Array of comamnd line arguments</param>
		private static int Main(string[] arguments)
		{
			// Dictionary<> of all detected merge modules
			Dictionary<string, string> modules = new Dictionary<string, string>();

			// banner
			Console.WriteLine();
			Console.WriteLine("findvcmsms - Searches for Visual C++ Runtime Merge Modules");
			Console.WriteLine();

			try
			{
				// Parse the command line arguments and switches
				CommandLine commandline = new CommandLine(arguments);

				// If insufficient arguments specified or help was requested, display the usage and exit
				if((commandline.Arguments.Count == 0) || (commandline.Switches.ContainsKey("?")))
				{
					ShowUsage();
					return 0;
				}

				// There has to be exactly one command line argument to indicate the base directory
				if(commandline.Arguments.Count != 1) throw new ArgumentException("Base directory must be specified");
				string basedir = commandline.Arguments[0];

				// The specified base directory must exist
				if(!Directory.Exists(basedir)) throw new ArgumentException("Specified base directory [" + basedir + "] does not exist");

				// -ver - Limit the directories to search based on the VCRedistVersion value
				string redistver = (commandline.Switches.ContainsKey("ver")) ? commandline.Switches["ver"] : string.Empty;

				// -filter - Specify merge module filter(s)
				List<string> filters = new List<string>();
				if(commandline.Switches.ContainsKey("filter"))
					filters.AddRange(commandline.Switches["filter"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));

				// Default to just CRT_X86 if no filters were added
				if(filters.Count == 0) filters.Add("CRT_x86");

				// -out - Specify output file name
				string outputfile = (commandline.Switches.ContainsKey("out")) ? commandline.Switches["out"] :
					Path.Combine(Environment.CurrentDirectory, "vcmergemods.wxs");

				// Attempt to create the output directory if it does not exist
				string outdir = Path.GetDirectoryName(outputfile);
				if(!Directory.Exists(outdir)) Directory.CreateDirectory(outdir);

				// Search the base directory recursively for all .msm files
				foreach(string msm in Directory.GetFiles(basedir, "*.msm", SearchOption.AllDirectories))
				{
					// If only looking for a specific version, make sure that version string is part of the path
					if(!string.IsNullOrEmpty(redistver) && !msm.Contains(redistver)) continue;

					string module = Path.GetFileNameWithoutExtension(msm);
					foreach(string filter in filters)
					{
						if(module.EndsWith("_" + filter, StringComparison.OrdinalIgnoreCase))
						{
							Console.WriteLine("Found merge module: " + msm);

							// Overwrite any existing key with the new path, it can be assumed (perhaps improperly)
							// that the last merge module found will be the newest, at least that's holding true
							// so far for Visual C++ 2017 ...
							modules[module] = msm;
						}
					}
				}

				// Create the runtime text template and generate the output file
				File.WriteAllText(outputfile, new vcmergemods(modules).TransformText());

				Console.WriteLine();
				Console.WriteLine("> " + modules.Count.ToString() + " merge modules written to " + outputfile);
				Console.WriteLine();

				return 0;
			}

			catch(Exception ex)
			{
				Console.WriteLine();
				Console.WriteLine("ERROR: " + ex.Message);
				Console.WriteLine();

				return unchecked((int)0x80004005);      // <-- E_FAIL
			}
		}
	}
}