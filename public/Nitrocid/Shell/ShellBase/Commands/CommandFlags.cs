//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

namespace Nitrocid.Shell.ShellBase.Commands
{
    /// <summary>
    /// Command flags
    /// </summary>
    public enum CommandFlags
    {
        /// <summary>
        /// No flags
        /// </summary>
        None = 0,
        /// <summary>
        /// The command is strict, meaning that it's only available for administrators.
        /// </summary>
        Strict = 1,
        /// <summary>
        /// This command can't run in maintenance mode.
        /// </summary>
        NoMaintenance = 2,
        /// <summary>
        /// The command is obsolete.
        /// </summary>
        Obsolete = 4,
        /// <summary>
        /// Redirection is supported, meaning that all the output to the commands can be redirected to a file.
        /// </summary>
        RedirectionSupported = 8,
        /// <summary>
        /// This command is wrappable to pages.
        /// </summary>
        Wrappable = 16,
    }
}
