
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Extensification.StringExts;
using System.Collections.Generic;

namespace KS.Shell.ShellBase.Commands
{
    /// <summary>
    /// Command help usage class
    /// </summary>
    public class HelpUsage
    {

        /// <summary>
        /// Command usage
        /// </summary>
        public string Usage { get; private set; }
        /// <summary>
        /// Command arguments
        /// </summary>
        public string[] Arguments { get; private set; }
        /// <summary>
        /// Command switches
        /// </summary>
        public string[] Switches { get; private set; }

        /// <summary>
        /// Installs a new instance of the help usage class
        /// </summary>
        /// <param name="Usage">Command usage</param>
        public HelpUsage(string Usage)
        {
            this.Usage = Usage;

            // Split the usage string with spaces and decide if said argument is a switch or an argument
            var usages = Usage.Split(' ');
            var args = new List<string>();
            var switches = new List<string>();
            foreach (string usage in usages)
            {
                // Check for beginnings and endings
                string finalUsage = usage.ReleaseDoubleQuotes();
                if ((finalUsage.StartsWith("<") && finalUsage.EndsWith(">")) ||
                    (finalUsage.StartsWith("[") && finalUsage.EndsWith("]")))
                {
                    // Check to see if the argument is required
                    bool isRequired = finalUsage.StartsWith("<") && finalUsage.EndsWith(">");
                    char startSyntax = isRequired ? '<' : '[';
                    char endSyntax = isRequired ? '>' : ']';

                    // Now, inspect the inside
                    string syntax = finalUsage.Substring(finalUsage.IndexOf(startSyntax) + 1, finalUsage.LastIndexOf(endSyntax) - 1);
                    bool isSwitch = syntax.StartsWith("-");

                    // Install the syntax to argument or switch list
                    if (isSwitch)
                    {
                        foreach (string switchStr in syntax.Split("|"))
                            switches.Add($"{startSyntax}{switchStr}{endSyntax}");
                    }
                    else
                        args.Add(usage);
                }
            }
            Arguments = args.ToArray();
            Switches = switches.ToArray();
        }

    }
}
