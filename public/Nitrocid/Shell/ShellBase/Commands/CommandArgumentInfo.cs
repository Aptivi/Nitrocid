
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

using KS.Languages;
using System;
using System.Linq;

namespace KS.Shell.ShellBase.Commands
{
    /// <summary>
    /// Command argument info class
    /// </summary>
    public class CommandArgumentInfo
    {

        /// <summary>
        /// Does the command require arguments?
        /// </summary>
        public bool ArgumentsRequired { get; private set; }
        /// <summary>
        /// User must specify at least this number of arguments
        /// </summary>
        public int MinimumArguments { get; private set; }
        /// <summary>
        /// Command arguments
        /// </summary>
        public string[] Arguments { get; private set; }
        /// <summary>
        /// Command switches
        /// </summary>
        public SwitchInfo[] Switches { get; private set; } = new[] {
            new SwitchInfo("set", /* Localizable */ "Sets the value of the output to the selected UESH variable", false, true)
        };
        /// <summary>
        /// Auto completion function delegate
        /// </summary>
        public Func<string, int, char[], string[]> AutoCompleter { get; private set; }

        /// <summary>
        /// Installs a new instance of the command argument info class
        /// </summary>
        public CommandArgumentInfo()
            : this(Array.Empty<string>(), Array.Empty<SwitchInfo>(), false, 0) { }

        /// <summary>
        /// Installs a new instance of the command argument info class
        /// </summary>
        /// <param name="Arguments">Command arguments</param>
        /// <param name="Switches">Command switches</param>
        public CommandArgumentInfo(string[] Arguments, SwitchInfo[] Switches)
            : this(Arguments, Switches, false, 0) { }

        /// <summary>
        /// Installs a new instance of the command argument info class
        /// </summary>
        /// <param name="Arguments">Command arguments</param>
        /// <param name="Switches">Command switches</param>
        /// <param name="ArgumentsRequired">Arguments required</param>
        /// <param name="MinimumArguments">Minimum arguments</param>
        /// <param name="AutoCompleter">Auto completion function</param>
        public CommandArgumentInfo(string[] Arguments, SwitchInfo[] Switches, bool ArgumentsRequired, int MinimumArguments, Func<string, int, char[], string[]> AutoCompleter = null)
        {
            this.Arguments = Arguments;
            this.Switches = this.Switches.Union(Switches).ToArray();
            this.ArgumentsRequired = ArgumentsRequired;
            this.MinimumArguments = MinimumArguments;
            this.AutoCompleter = AutoCompleter;
        }

    }
}
