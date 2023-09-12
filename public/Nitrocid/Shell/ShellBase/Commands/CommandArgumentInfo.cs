
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
        public bool ArgumentsRequired =>
            Arguments.Any((part) => part.ArgumentRequired);
        /// <summary>
        /// User must specify at least this number of arguments
        /// </summary>
        public int MinimumArguments =>
            Arguments.Where((part) => part.ArgumentRequired).Count();
        /// <summary>
        /// Command arguments
        /// </summary>
        public CommandArgumentPart[] Arguments { get; private set; }
        /// <summary>
        /// Command switches
        /// </summary>
        public SwitchInfo[] Switches { get; private set; } = new[] {
            new SwitchInfo("set", /* Localizable */ "Sets the value of the output to the selected UESH variable", false, true)
        };
        /// <summary>
        /// Whether to accept the -set switch to set the UESH variable value
        /// </summary>
        public bool AcceptsSet { get; private set; }

        /// <summary>
        /// Installs a new instance of the command argument info class
        /// </summary>
        public CommandArgumentInfo()
            : this(Array.Empty<CommandArgumentPart>(), Array.Empty<SwitchInfo>(), false)
        { }

        /// <summary>
        /// Installs a new instance of the command argument info class
        /// </summary>
        /// <param name="Arguments">Command arguments</param>
        /// <param name="Switches">Command switches</param>
        public CommandArgumentInfo(CommandArgumentPart[] Arguments, SwitchInfo[] Switches)
            : this(Arguments, Switches, false)
        { }

        /// <summary>
        /// Installs a new instance of the command argument info class
        /// </summary>
        /// <param name="Arguments">Command arguments</param>
        /// <param name="Switches">Command switches</param>
        /// <param name="AcceptsSet">Whether to accept the -set switch or not</param>
        public CommandArgumentInfo(CommandArgumentPart[] Arguments, SwitchInfo[] Switches, bool AcceptsSet = false)
        {
            this.Arguments = Arguments;
            if (AcceptsSet)
                this.Switches = this.Switches.Union(Switches).ToArray();
            else
                this.Switches = Switches;
            this.AcceptsSet = AcceptsSet;
        }

    }
}
