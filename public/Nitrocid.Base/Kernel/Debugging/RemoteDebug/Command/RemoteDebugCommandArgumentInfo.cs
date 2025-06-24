//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

namespace Nitrocid.Base.Kernel.Debugging.RemoteDebug.Command
{
    /// <summary>
    /// Command argument info class
    /// </summary>
    public class RemoteDebugCommandArgumentInfo
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
        /// Installs a new instance of the command argument info class
        /// </summary>
        public RemoteDebugCommandArgumentInfo() :
            this([], false, 0)
        { }

        /// <summary>
        /// Installs a new instance of the command argument info class
        /// </summary>
        /// <param name="HelpUsages">Help usages</param>
        public RemoteDebugCommandArgumentInfo(string[] HelpUsages) :
            this(HelpUsages, false, 0)
        { }

        /// <summary>
        /// Installs a new instance of the command argument info class
        /// </summary>
        /// <param name="HelpUsages">Help usages</param>
        /// <param name="ArgumentsRequired">Arguments required</param>
        /// <param name="MinimumArguments">Minimum arguments</param>
        public RemoteDebugCommandArgumentInfo(string[] HelpUsages, bool ArgumentsRequired, int MinimumArguments)
        {
            this.HelpUsages = HelpUsages;
            this.ArgumentsRequired = ArgumentsRequired;
            this.MinimumArguments = MinimumArguments;
        }

    }
}
