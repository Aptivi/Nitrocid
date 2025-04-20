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
using Nitrocid.Shell.ShellBase.Shells;

namespace Nitrocid.Shell.ShellBase.Help
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
            ShowHelpExtended("", ShellManager.CurrentShellType, Config.MainConfig.SimHelp, showGeneral, showMod, showAlias, showUnified, showAddon);

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
            ShowHelpExtended("", ShellManager.GetShellTypeName(commandType), Config.MainConfig.SimHelp, showGeneral, showMod, showAlias, showUnified, showAddon);

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
            ShowHelpExtended(command, ShellManager.CurrentShellType, Config.MainConfig.SimHelp, showGeneral, showMod, showAlias, showUnified, showAddon);

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
            ShowHelpExtended(command, ShellManager.GetShellTypeName(commandType), Config.MainConfig.SimHelp, showGeneral, showMod, showAlias, showUnified, showAddon);

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
        public static void ShowHelp(string command, string commandType, bool showGeneral = true, bool showMod = false, bool showAlias = false, bool showUnified = false, bool showAddon = false) =>
            ShowHelpExtended(command, commandType, Config.MainConfig.SimHelp, showGeneral, showMod, showAlias, showUnified, showAddon);

        /// <summary>
        /// Shows the list of commands under the current shell type
        /// </summary>
        /// <param name="simplified">Uses simplified help</param>
        /// <param name="showGeneral">Shows all general commands</param>
        /// <param name="showMod">Shows all mod commands</param>
        /// <param name="showAlias">Shows all aliased commands</param>
        /// <param name="showUnified">Shows all unified commands</param>
        /// <param name="showAddon">Shows all kernel addon commands</param>
        public static void ShowHelpExtended(bool simplified = false, bool showGeneral = true, bool showMod = false, bool showAlias = false, bool showUnified = false, bool showAddon = false) =>
            ShowHelpExtended("", ShellManager.CurrentShellType, simplified, showGeneral, showMod, showAlias, showUnified, showAddon);

        /// <summary>
        /// Shows the list of commands under the specified shell type
        /// </summary>
        /// <param name="simplified">Uses simplified help</param>
        /// <param name="commandType">A specified shell type</param>
        /// <param name="showGeneral">Shows all general commands</param>
        /// <param name="showMod">Shows all mod commands</param>
        /// <param name="showAlias">Shows all aliased commands</param>
        /// <param name="showUnified">Shows all unified commands</param>
        /// <param name="showAddon">Shows all kernel addon commands</param>
        public static void ShowHelpExtended(ShellType commandType, bool simplified = false, bool showGeneral = true, bool showMod = false, bool showAlias = false, bool showUnified = false, bool showAddon = false) =>
            ShowHelpExtended("", ShellManager.GetShellTypeName(commandType), simplified, showGeneral, showMod, showAlias, showUnified, showAddon);

        /// <summary>
        /// Shows the help of a command, or command list under the current shell type if nothing is specified
        /// </summary>
        /// <param name="simplified">Uses simplified help</param>
        /// <param name="command">A specified command</param>
        /// <param name="showGeneral">Shows all general commands</param>
        /// <param name="showMod">Shows all mod commands</param>
        /// <param name="showAlias">Shows all aliased commands</param>
        /// <param name="showUnified">Shows all unified commands</param>
        /// <param name="showAddon">Shows all kernel addon commands</param>
        public static void ShowHelpExtended(string command, bool simplified = false, bool showGeneral = true, bool showMod = false, bool showAlias = false, bool showUnified = false, bool showAddon = false) =>
            ShowHelpExtended(command, ShellManager.CurrentShellType, simplified, showGeneral, showMod, showAlias, showUnified, showAddon);

        /// <summary>
        /// Shows the help of a command, or command list under the specified shell type if nothing is specified
        /// </summary>
        /// <param name="simplified">Uses simplified help</param>
        /// <param name="command">A specified command</param>
        /// <param name="commandType">A specified shell type</param>
        /// <param name="showGeneral">Shows all general commands</param>
        /// <param name="showMod">Shows all mod commands</param>
        /// <param name="showAlias">Shows all aliased commands</param>
        /// <param name="showUnified">Shows all unified commands</param>
        /// <param name="showAddon">Shows all kernel addon commands</param>
        public static void ShowHelpExtended(string command, ShellType commandType, bool simplified = false, bool showGeneral = true, bool showMod = false, bool showAlias = false, bool showUnified = false, bool showAddon = false) =>
            ShowHelpExtended(command, ShellManager.GetShellTypeName(commandType), simplified, showGeneral, showMod, showAlias, showUnified, showAddon);

        /// <summary>
        /// Shows the help of a command, or command list under the specified shell type if nothing is specified
        /// </summary>
        /// <param name="simplified">Uses simplified help</param>
        /// <param name="command">A specified command</param>
        /// <param name="commandType">A specified shell type</param>
        /// <param name="showGeneral">Shows all general commands</param>
        /// <param name="showMod">Shows all mod commands</param>
        /// <param name="showAlias">Shows all aliased commands</param>
        /// <param name="showUnified">Shows all unified commands</param>
        /// <param name="showAddon">Shows all kernel addon commands</param>
        public static void ShowHelpExtended(string command, string commandType, bool simplified = false, bool showGeneral = true, bool showMod = false, bool showAlias = false, bool showUnified = false, bool showAddon = false)
        {
            // Check to see if command exists
            if (!string.IsNullOrWhiteSpace(command))
                HelpPrintTools.ShowHelpUsage(command, commandType);
            else if (string.IsNullOrWhiteSpace(command))
            {
                // List the available commands
                if (simplified)
                    HelpPrintTools.ShowCommandListSimplified(commandType);
                else
                    HelpPrintTools.ShowCommandList(commandType, showGeneral, showMod, showAlias, showUnified, showAddon);
            }
        }
    }
}
