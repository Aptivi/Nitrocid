
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

using KS.Shell.ShellBase.Shells;

namespace KS.Shell.ShellBase.Help
{
    /// <summary>
    /// Help system for shells module
    /// </summary>
    public static class HelpPrint
    {
        /// <summary>
        /// Shows the list of commands under the current shell type
        /// </summary>
        /// <param name="showGeneral">Shows all general commands</param>
        /// <param name="showMod">Shows all mod commands</param>
        /// <param name="showAlias">Shows all aliased commands</param>
        /// <param name="showUnified">Shows all unified commands</param>
        /// <param name="showAddon">Shows all kernel addon commands</param>
        public static void ShowHelp(bool showGeneral = true, bool showMod = false, bool showAlias = false, bool showUnified = false, bool showAddon = false) =>
            ShowHelp("", ShellManager.CurrentShellType, showGeneral, showMod, showAlias, showUnified, showAddon);

        /// <summary>
        /// Shows the list of commands under the specified shell type
        /// </summary>
        /// <param name="commandType">A specified shell type</param>
        /// <param name="showGeneral">Shows all general commands</param>
        /// <param name="showMod">Shows all mod commands</param>
        /// <param name="showAlias">Shows all aliased commands</param>
        /// <param name="showUnified">Shows all unified commands</param>
        /// <param name="showAddon">Shows all kernel addon commands</param>
        public static void ShowHelp(ShellType commandType, bool showGeneral = true, bool showMod = false, bool showAlias = false, bool showUnified = false, bool showAddon = false) =>
            ShowHelp("", ShellManager.GetShellTypeName(commandType), showGeneral, showMod, showAlias, showUnified, showAddon);

        /// <summary>
        /// Shows the help of a command, or command list under the current shell type if nothing is specified
        /// </summary>
        /// <param name="command">A specified command</param>
        /// <param name="showGeneral">Shows all general commands</param>
        /// <param name="showMod">Shows all mod commands</param>
        /// <param name="showAlias">Shows all aliased commands</param>
        /// <param name="showUnified">Shows all unified commands</param>
        /// <param name="showAddon">Shows all kernel addon commands</param>
        public static void ShowHelp(string command, bool showGeneral = true, bool showMod = false, bool showAlias = false, bool showUnified = false, bool showAddon = false) =>
            ShowHelp(command, ShellManager.CurrentShellType, showGeneral, showMod, showAlias, showUnified, showAddon);

        /// <summary>
        /// Shows the help of a command, or command list under the specified shell type if nothing is specified
        /// </summary>
        /// <param name="command">A specified command</param>
        /// <param name="commandType">A specified shell type</param>
        /// <param name="showGeneral">Shows all general commands</param>
        /// <param name="showMod">Shows all mod commands</param>
        /// <param name="showAlias">Shows all aliased commands</param>
        /// <param name="showUnified">Shows all unified commands</param>
        /// <param name="showAddon">Shows all kernel addon commands</param>
        public static void ShowHelp(string command, ShellType commandType, bool showGeneral = true, bool showMod = false, bool showAlias = false, bool showUnified = false, bool showAddon = false) =>
            ShowHelp(command, ShellManager.GetShellTypeName(commandType), showGeneral, showMod, showAlias, showUnified, showAddon);

        /// <summary>
        /// Shows the help of a command, or command list under the specified shell type if nothing is specified
        /// </summary>
        /// <param name="command">A specified command</param>
        /// <param name="commandType">A specified shell type</param>
        /// <param name="showGeneral">Shows all general commands</param>
        /// <param name="showMod">Shows all mod commands</param>
        /// <param name="showAlias">Shows all aliased commands</param>
        /// <param name="showUnified">Shows all unified commands</param>
        /// <param name="showAddon">Shows all kernel addon commands</param>
        public static void ShowHelp(string command, string commandType, bool showGeneral = true, bool showMod = false, bool showAlias = false, bool showUnified = false, bool showAddon = false)
        {
            // Check to see if command exists
            if (!string.IsNullOrWhiteSpace(command))
                HelpPrintTools.ShowHelpUsage(command, commandType);
            else if (string.IsNullOrWhiteSpace(command))
            {
                // List the available commands
                if (HelpPrintTools.SimHelp)
                    HelpPrintTools.ShowCommandListSimplified(commandType);
                else
                    HelpPrintTools.ShowCommandList(commandType, showGeneral, showMod, showAlias, showUnified, showAddon);
            }
        }
    }
}
