﻿//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;

// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

namespace KS.Shell.ShellBase.Commands
{
    public class CommandArgumentInfo
    {

        /// <summary>
        /// The help usages of command.
        /// </summary>
        public string[] HelpUsages { get; private set; }
        /// <summary>
        /// Does the command require arguments?
        /// </summary>
        public bool ArgumentsRequired { get; private set; }
        /// <summary>
        /// User must specify at least this number of arguments
        /// </summary>
        public int MinimumArguments { get; private set; }
        /// <summary>
        /// Auto completion function delegate
        /// </summary>
        public Func<string[]> AutoCompleter { get; private set; }

        /// <summary>
        /// Installs a new instance of the command argument info class
        /// </summary>
        /// <param name="HelpUsages">Help usages</param>
        /// <param name="ArgumentsRequired">Arguments required</param>
        /// <param name="MinimumArguments">Minimum arguments</param>
        /// <param name="AutoCompleter">Auto completion function</param>
        public CommandArgumentInfo(string[] HelpUsages, bool ArgumentsRequired, int MinimumArguments, Func<string[]> AutoCompleter = null)
        {
            this.HelpUsages = HelpUsages;
            this.ArgumentsRequired = ArgumentsRequired;
            this.MinimumArguments = MinimumArguments;
            this.AutoCompleter = AutoCompleter;
        }

    }
}