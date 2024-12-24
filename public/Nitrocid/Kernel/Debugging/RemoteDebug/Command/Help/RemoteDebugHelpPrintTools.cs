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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Languages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Kernel.Debugging.RemoteDebug.Command.Help
{
    internal static class RemoteDebugHelpPrintTools
    {
        internal static Dictionary<string, RemoteDebugCommandInfo> GetCommands()
        {
            var CommandList =
                RemoteDebugCommandExecutor.RemoteDebugCommands
                .OrderBy((CommandValuePair) => CommandValuePair.Key)
                .ToDictionary((CommandValuePair) => CommandValuePair.Key, (CommandValuePair) => CommandValuePair.Value);
            DebugWriter.WriteDebug(DebugLevel.I, "Remote Debug Commands: {0} [{1}]", CommandList.Count, string.Join(", ", CommandList.Keys));
            return CommandList;
        }

        internal static void ShowCommandList(RemoteDebugDevice device)
        {
            var commandList = GetCommands();

            // The built-in commands
            DebugWriter.WriteDebugDeviceOnly(DebugLevel.I, Translate.DoTranslation("General commands:") + (Config.MainConfig.ShowCommandsCount & Config.MainConfig.ShowShellCommandsCount ? " [{0}]" : ""), true, device, commandList.Count);

            // Check the command list count and print not implemented. This is an extremely rare situation.
            if (commandList.Count == 0)
                DebugWriter.WriteDebugDeviceOnly(DebugLevel.I, "- " + Translate.DoTranslation("Shell commands not implemented!!!"), true, device);
            foreach (string cmd in commandList.Keys)
                DebugWriter.WriteDebugDeviceOnly(DebugLevel.I, "- {0}: {1}", true, device, cmd, commandList[cmd].GetTranslatedHelpEntry());
        }

        internal static void ShowCommandListSimple(RemoteDebugDevice device)
        {
            var commandList = GetCommands();

            // The built-in commands
            DebugWriter.WriteDebugDeviceOnly(DebugLevel.I, string.Join(", ", commandList.Keys), true, device);
        }

        internal static void ShowHelpUsage(string command, RemoteDebugDevice device)
        {
            // Check to see if we have this command
            var commandList = GetCommands();
            if (!commandList.ContainsKey(command))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "We found no help! {0}", command);
                TextWriters.Write(Translate.DoTranslation("No help for command \"{0}\"."), true, KernelColorType.Error, command);
                return;
            }

            // Now, populate usages for each command
            string FinalCommand = command;
            string RenderedCommand = $"/{command}";
            string HelpDefinition = commandList[FinalCommand].GetTranslatedHelpEntry();
            var HelpUsages = Array.Empty<string>();

            // Populate help usages
            if (commandList[FinalCommand].CommandArgumentInfo is not null)
                HelpUsages = commandList[FinalCommand].CommandArgumentInfo.HelpUsages;

            // Print usage information
            foreach (string HelpUsage in HelpUsages)
                DebugWriter.WriteDebugDeviceOnly(DebugLevel.I, Translate.DoTranslation("Usage:") + $" {RenderedCommand} {HelpUsage}", true, device);

            // Write the description now
            if (string.IsNullOrEmpty(HelpDefinition))
                HelpDefinition = Translate.DoTranslation("No remote debug command description");
            DebugWriter.WriteDebugDeviceOnly(DebugLevel.I, Translate.DoTranslation("Description:") + $" {HelpDefinition}", true, device);

            // Extra help action for some commands
            commandList[FinalCommand].CommandBase?.HelpHelper();
        }

    }
}
