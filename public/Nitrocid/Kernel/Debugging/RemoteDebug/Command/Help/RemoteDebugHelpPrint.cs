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

using Nitrocid.Kernel.Configuration;

namespace Nitrocid.Kernel.Debugging.RemoteDebug.Command.Help
{
    /// <summary>
    /// Help system for shells module
    /// </summary>
    public static class RemoteDebugHelpPrint
    {

        /// <summary>
        /// Shows the list of commands under the current shell type
        /// </summary>
        /// <param name="device">Device to contact</param>
        public static void ShowHelp(RemoteDebugDevice device) =>
            ShowHelp("", device);

        /// <summary>
        /// Shows the help of a command, or command list under the current shell type if nothing is specified
        /// </summary>
        /// <param name="command">A specified command</param>
        /// <param name="device">Device to contact</param>
        public static void ShowHelp(string command, RemoteDebugDevice device)
        {
            // Check to see if command exists
            if (!string.IsNullOrWhiteSpace(command))
                RemoteDebugHelpPrintTools.ShowHelpUsage(command, device);
            else
            {
                // List the available commands
                if (!Config.MainConfig.SimHelp)
                    RemoteDebugHelpPrintTools.ShowCommandList(device);
                else
                    RemoteDebugHelpPrintTools.ShowCommandListSimple(device);
            }
        }

    }
}
