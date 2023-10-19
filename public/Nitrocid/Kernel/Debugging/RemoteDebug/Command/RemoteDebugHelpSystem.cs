
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
using KS.Languages;
using KS.Shell.ShellBase.Help;

namespace KS.Kernel.Debugging.RemoteDebug.Command
{
    /// <summary>
    /// Help system for shells module
    /// </summary>
    public static class RemoteDebugHelpSystem
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
            // Determine command type
            var CommandList = RemoteDebugCommandExecutor.RemoteDebugCommands
                              .OrderBy((CommandValuePair) => CommandValuePair.Key)
                              .ToDictionary((CommandValuePair) => CommandValuePair.Key, (CommandValuePair) => CommandValuePair.Value);

            // Check to see if command exists
            if (!string.IsNullOrWhiteSpace(command) & CommandList.ContainsKey(command))
            {
                // Found!
                var FinalCommandList = CommandList;
                string FinalCommand = command;
                string RenderedCommand = $"/{command}";
                string HelpDefinition = FinalCommandList[FinalCommand].GetTranslatedHelpEntry();
                int UsageLength = Translate.DoTranslation("Usage:").Length;
                var HelpUsages = Array.Empty<string>();

                // Populate help usages
                if (FinalCommandList[FinalCommand].CommandArgumentInfo is not null)
                    HelpUsages = FinalCommandList[FinalCommand].CommandArgumentInfo.HelpUsages;

                // Print usage information
                foreach (string HelpUsage in HelpUsages)
                    DebugWriter.WriteDebugDeviceOnly(DebugLevel.I, Translate.DoTranslation("Usage:") + $" {FinalCommand} {HelpUsage}", true, device);

                // Write the description now
                if (string.IsNullOrEmpty(HelpDefinition))
                    HelpDefinition = Translate.DoTranslation("Command defined by ") + command;
                DebugWriter.WriteDebugDeviceOnly(DebugLevel.I, Translate.DoTranslation("Description:") + $" {HelpDefinition}", true, device);

                // Extra help action for some commands
                FinalCommandList[FinalCommand].CommandBase?.HelpHelper();
            }
            else if (string.IsNullOrWhiteSpace(command))
            {
                // List the available commands
                if (!HelpPrintTools.SimHelp)
                {
                    // The built-in commands
                    DebugWriter.WriteDebugDeviceOnly(DebugLevel.I, Translate.DoTranslation("General commands:") + (HelpPrintTools.ShowCommandsCount & HelpPrintTools.ShowShellCommandsCount ? " [{0}]" : ""), true, device, CommandList.Count);

                    // Check the command list count and print not implemented. This is an extremely rare situation.
                    if (CommandList.Count == 0)
                        DebugWriter.WriteDebugDeviceOnly(DebugLevel.I, "- " + Translate.DoTranslation("Shell commands not implemented!!!"), true, device);
                    foreach (string cmd in CommandList.Keys)
                        DebugWriter.WriteDebugDeviceOnly(DebugLevel.I, "- {0}: {1}", true, device, cmd, CommandList[cmd].GetTranslatedHelpEntry());
                }
                else
                {
                    // The built-in commands
                    foreach (string cmd in CommandList.Keys)
                        DebugWriter.WriteDebugDeviceOnly(DebugLevel.I, "{0}, ", true, device, cmd);
                }
            }
            else
            {
                DebugWriter.WriteDebugDeviceOnly(DebugLevel.I, Translate.DoTranslation("No help for command \"{0}\"."), true, device, command);
            }
        }

    }
}
