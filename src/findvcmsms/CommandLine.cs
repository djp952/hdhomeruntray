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

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace zuki.build.tools
{
	/// <summary>
	/// Command-line argument/switch helper
	/// </summary>
	internal class CommandLine
	{
		/// <summary>
		/// Instance constructor
		/// </summary>
		/// <param name="args">Array of command-line argument strings</param>
		public CommandLine(string[] args)
		{
			// Create a List<> of all arguments and an empty switch dictionary
			List<string> arguments = new List<string>(args);
			Dictionary<string, string> switches = new Dictionary<string, string>();

			// Separate out all switched arguments from the argument list
			int index = 0;
			while(index < arguments.Count)
			{
				// If the argument starts with a forward slash or a minus sign, this is a switch
				if(arguments[index].StartsWith("/") || arguments[index].StartsWith("-"))
				{
					if(arguments[index].Length > 1)
					{
						// Pull out the entire switch argument, sans the / or -
						string switcharg = arguments[index].Substring(1);

						// Switches can optionally include data after a colon
						int colon = switcharg.IndexOf(":");
						if(colon != -1) switches.Add(switcharg.Substring(0, colon).ToLower(), switcharg.Substring(colon + 1));
						else switches.Add(switcharg.ToLower(), string.Empty);

						arguments.RemoveAt(index);          // Remove switch from the argument list
					}
				}

				else index++;                               // Move to the next argument string
			}

			// Convert the argument and switch collections into ReadOnly<> fields
			Arguments = new ReadOnlyCollection<string>(arguments);
			Switches = new ReadOnlyDictionary<string, string>(switches);
		}

		/// <summary>
		/// Collection of non-switched arguments
		/// </summary>
		public readonly ReadOnlyCollection<string> Arguments;

		/// <summary>
		/// Collection of switches and optional values
		/// </summary>
		public readonly ReadOnlyDictionary<string, string> Switches;
	}
}
