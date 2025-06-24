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

using Nitrocid.Base.Kernel.Debugging.RemoteDebug;

namespace Nitrocid.Base.Kernel.Debugging.RemoteDebug.Command
{
    /// <summary>
    /// Base command executor
    /// </summary>
    public interface IRemoteDebugCommand
    {

        /// <summary>
        /// Executes the command with the given argument
        /// </summary>
        /// <param name="parameters">Command parameters including passed arguments and switches information</param>
        /// <param name="device">Device that executed the command</param>
        void Execute(RemoteDebugCommandParameters parameters, RemoteDebugDevice device);

        /// <summary>
        /// Shows additional information for the command when "help command" is invoked
        /// </summary>
        void HelpHelper();

    }
}
