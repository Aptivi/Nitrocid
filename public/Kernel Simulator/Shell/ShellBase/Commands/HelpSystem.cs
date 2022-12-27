
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Writers.MiscWriters;
using KS.Modifications;
using KS.Shell.ShellBase.Aliases;
using KS.Shell.ShellBase.Shells;
using KS.Users.Groups;
using KS.Users.Login;

namespace KS.Shell.ShellBase.Commands
{
    /// <summary>
    /// Help system for shells module
    /// </summary>
    public static class HelpSystem
    {

        /// <summary>
        /// Shows the list of commands under the current shell type
        /// </summary>
        public static void ShowHelp() => ShowHelp("", Shell.CurrentShellType);

        /// <summary>
        /// Shows the list of commands under the specified shell type
        /// </summary>
        /// <param name="CommandType">A specified shell type</param>
        public static void ShowHelp(ShellType CommandType) => ShowHelp("", Shell.GetShellTypeName(CommandType));

        /// <summary>
        /// Shows the help of a command, or command list under the current shell type if nothing is specified
        /// </summary>
        /// <param name="command">A specified command</param>
        public static void ShowHelp(string command) => ShowHelp(command, Shell.CurrentShellType);

        /// <summary>
        /// Shows the help of a command, or command list under the specified shell type if nothing is specified
        /// </summary>
        /// <param name="command">A specified command</param>
        /// <param name="CommandType">A specified shell type</param>
        /// <param name="DebugDeviceSocket">Only for remote debug shell. Specifies the debug device socket.</param>
        public static void ShowHelp(string command, ShellType CommandType, StreamWriter DebugDeviceSocket = null) => ShowHelp(command, Shell.GetShellTypeName(CommandType), DebugDeviceSocket);

        /// <summary>
        /// Shows the help of a command, or command list under the specified shell type if nothing is specified
        /// </summary>
        /// <param name="command">A specified command</param>
        /// <param name="CommandType">A specified shell type</param>
        /// <param name="DebugDeviceSocket">Only for remote debug shell. Specifies the debug device socket.</param>
        public static void ShowHelp(string command, string CommandType, StreamWriter DebugDeviceSocket = null)
        {
            // Determine command type
            var CommandList = CommandManager.GetCommands(CommandType)
                                            .Where((CommandValuePair) => !CommandValuePair.Value.Flags.HasFlag(CommandFlags.Hidden))
                                            .OrderBy((CommandValuePair) => CommandValuePair.Key)
                                            .ToDictionary((CommandValuePair) => CommandValuePair.Key, (CommandValuePair) => CommandValuePair.Value);
            Dictionary<string, CommandInfo> ModCommandList;
            var AliasedCommandList = AliasManager.GetAliasesListFromType(CommandType);

            // Add every command from each mod
            ModCommandList = ModManager.ListModCommands(CommandType);

            // Check to see if command exists
            if (!string.IsNullOrWhiteSpace(command) & (CommandList.ContainsKey(command) | AliasedCommandList.ContainsKey(command) | ModCommandList.ContainsKey(command)))
            {
                // Found!
                bool IsMod = ModCommandList.ContainsKey(command);
                bool IsAlias = AliasedCommandList.ContainsKey(command);
                var FinalCommandList = IsMod ? ModCommandList : CommandList;
                string FinalCommand = IsMod ? command : IsAlias ? AliasedCommandList[command] : command;
                string HelpDefinition = IsMod ? FinalCommandList[FinalCommand].HelpDefinition : FinalCommandList[FinalCommand].GetTranslatedHelpEntry();
                int UsageLength = Translate.DoTranslation("Usage:").Length;
                var HelpUsages = Array.Empty<string>();

                // Populate help usages
                if (FinalCommandList[FinalCommand].CommandArgumentInfo is not null)
                {
                    HelpUsages = FinalCommandList[FinalCommand].CommandArgumentInfo.HelpUsages;
                }

                // Print usage information
                if (HelpUsages.Length != 0)
                {
                    // Print the usage information holder
                    var Indent = default(bool);
                    Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, Translate.DoTranslation("Usage:"), false, KernelColorType.ListEntry);

                    // If remote debug, set the command to be prepended by the slash
                    if (CommandType == "RemoteDebugShell")
                        FinalCommand = $"/{FinalCommand}";

                    // Enumerate through the available help usages
                    foreach (string HelpUsage in HelpUsages)
                    {
                        // Indent, if necessary
                        if (Indent)
                            Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, " ".Repeat(UsageLength), false, KernelColorType.ListEntry);
                        Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, $" {FinalCommand} {HelpUsage}", true, KernelColorType.ListEntry);
                        Indent = true;
                    }
                }

                // Write the description now
                if (string.IsNullOrEmpty(HelpDefinition))
                    HelpDefinition = Translate.DoTranslation("Command defined by ") + command;
                Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, Translate.DoTranslation("Description:") + $" {HelpDefinition}", true, KernelColorType.ListValue);

                // Extra help action for some commands
                FinalCommandList[FinalCommand].CommandBase?.HelpHelper();
            }
            else if (string.IsNullOrWhiteSpace(command))
            {
                // List the available commands
                if (!Flags.SimHelp)
                {
                    // The built-in commands
                    Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, Translate.DoTranslation("General commands:") + (Flags.ShowCommandsCount & Flags.ShowShellCommandsCount ? " [{0}]" : ""), true, KernelColorType.ListTitle, CommandList.Count);

                    // Check the command list count and print not implemented. This is an extremely rare situation.
                    if (CommandList.Count == 0)
                        Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, "- " + Translate.DoTranslation("Shell commands not implemented!!!"), true, KernelColorType.Warning);
                    foreach (string cmd in CommandList.Keys)
                    {
                        if ((!CommandList[cmd].Flags.HasFlag(CommandFlags.Strict) | CommandList[cmd].Flags.HasFlag(CommandFlags.Strict) & GroupManagement.HasGroup(Login.CurrentUser?.Username, GroupManagement.GroupType.Administrator)) & (Flags.Maintenance & !CommandList[cmd].Flags.HasFlag(CommandFlags.NoMaintenance) | !Flags.Maintenance))
                        {
                            Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, "- {0}: ", false, Shell.UnifiedCommandDict.ContainsKey(cmd) ? KernelColorType.Success : KernelColorType.ListEntry, cmd);
                            Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, "{0}", true, KernelColorType.ListValue, CommandList[cmd].GetTranslatedHelpEntry());
                        }
                    }

                    // The mod commands
                    Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, CharManager.NewLine + Translate.DoTranslation("Mod commands:") + (Flags.ShowCommandsCount & Flags.ShowModCommandsCount ? " [{0}]" : ""), true, KernelColorType.ListTitle, ModCommandList.Count);
                    if (ModCommandList.Count == 0)
                        Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, "- " + Translate.DoTranslation("No mod commands."), true, KernelColorType.Warning);
                    foreach (string cmd in ModCommandList.Keys)
                    {
                        Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, "- {0}: ", false, KernelColorType.ListEntry, cmd);
                        Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, "{0}", true, KernelColorType.ListValue, ModCommandList[cmd].HelpDefinition);
                    }

                    // The alias commands
                    Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, CharManager.NewLine + Translate.DoTranslation("Alias commands:") + (Flags.ShowCommandsCount & Flags.ShowShellAliasesCount ? " [{0}]" : ""), true, KernelColorType.ListTitle, AliasedCommandList.Count);
                    if (AliasedCommandList.Count == 0)
                        Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, "- " + Translate.DoTranslation("No alias commands."), true, KernelColorType.Warning);
                    foreach (string cmd in AliasedCommandList.Keys)
                    {
                        Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, "- {0}: ", false, KernelColorType.ListEntry, cmd);
                        Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, "{0}", true, KernelColorType.ListValue, CommandList[AliasedCommandList[cmd]].GetTranslatedHelpEntry());
                    }

                    // A tip for you all
                    Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, CharManager.NewLine + Translate.DoTranslation("* You can use multiple commands using the semicolon between commands."), true, KernelColorType.Tip);
                    Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, "* " + Translate.DoTranslation("Commands highlighted in another color are unified commands and are available in every shell."), true, KernelColorType.Tip);
                }
                else
                {
                    // The built-in commands
                    foreach (string cmd in CommandList.Keys)
                    {
                        if ((!CommandList[cmd].Flags.HasFlag(CommandFlags.Strict) | CommandList[cmd].Flags.HasFlag(CommandFlags.Strict) & GroupManagement.HasGroup(Login.CurrentUser?.Username, GroupManagement.GroupType.Administrator)) & (Flags.Maintenance & !CommandList[cmd].Flags.HasFlag(CommandFlags.NoMaintenance) | !Flags.Maintenance))
                        {
                            Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, "{0}, ", false, KernelColorType.ListEntry, cmd);
                        }
                    }

                    // The mod commands
                    foreach (string cmd in ModCommandList.Keys)
                        Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, "{0}, ", false, KernelColorType.ListEntry, cmd);

                    // The alias commands
                    Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, string.Join(", ", AliasedCommandList.Keys), true, KernelColorType.ListEntry);
                }
            }
            else
            {
                Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, Translate.DoTranslation("No help for command \"{0}\"."), true, KernelColorType.Error, command);
            }
        }

    }
}
