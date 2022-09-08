
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
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Writers.MiscWriters;
using KS.Modifications;
using KS.Shell.ShellBase.Aliases;
using KS.Shell.ShellBase.Shells;
using KS.Users.Groups;

namespace KS.Shell.ShellBase.Commands
{
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
        public static void ShowHelp(ShellType CommandType) => ShowHelp("", CommandType);

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
        public static void ShowHelp(string command, ShellType CommandType, StreamWriter DebugDeviceSocket = null)
        {
            // Determine command type
            var CommandList = GetCommand.GetCommands(CommandType);
            Dictionary<string, CommandInfo> ModCommandList;
            var AliasedCommandList = AliasManager.Aliases;

            // Add every command from each mod
            ModCommandList = ModManager.ListModCommands(CommandType);

            // Select which list to use according to the shell type
            switch (CommandType)
            {
                case ShellType.Shell:
                    {
                        AliasedCommandList = AliasManager.Aliases;
                        break;
                    }
                case ShellType.FTPShell:
                    {
                        AliasedCommandList = AliasManager.FTPShellAliases;
                        break;
                    }
                case ShellType.MailShell:
                    {
                        AliasedCommandList = AliasManager.MailShellAliases;
                        break;
                    }
                case ShellType.RSSShell:
                    {
                        AliasedCommandList = AliasManager.RSSShellAliases;
                        break;
                    }
                case ShellType.SFTPShell:
                    {
                        AliasedCommandList = AliasManager.SFTPShellAliases;
                        break;
                    }
                case ShellType.TestShell:
                    {
                        AliasedCommandList = AliasManager.TestShellAliases;
                        break;
                    }
                case ShellType.TextShell:
                    {
                        AliasedCommandList = AliasManager.TextShellAliases;
                        break;
                    }
                case ShellType.RemoteDebugShell:
                    {
                        AliasedCommandList = AliasManager.RemoteDebugAliases;
                        break;
                    }
                case ShellType.JsonShell:
                    {
                        AliasedCommandList = AliasManager.JsonShellAliases;
                        break;
                    }
                case ShellType.HTTPShell:
                    {
                        AliasedCommandList = AliasManager.HTTPShellAliases;
                        break;
                    }
                case ShellType.HexShell:
                    {
                        AliasedCommandList = AliasManager.HexShellAliases;
                        break;
                    }
                case ShellType.ArchiveShell:
                    {
                        AliasedCommandList = AliasManager.ArchiveShellAliases;
                        break;
                    }
            }

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
                    Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, Translate.DoTranslation("Usage:"), false, ColorTools.ColTypes.ListEntry);

                    // If remote debug, set the command to be prepended by the slash
                    if (CommandType == ShellType.RemoteDebugShell)
                        FinalCommand = $"/{FinalCommand}";

                    // Enumerate through the available help usages
                    foreach (string HelpUsage in HelpUsages)
                    {
                        // Indent, if necessary
                        if (Indent)
                            Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, " ".Repeat(UsageLength), false, ColorTools.ColTypes.ListEntry);
                        Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, $" {FinalCommand} {HelpUsage}", true, ColorTools.ColTypes.ListEntry);
                        Indent = true;
                    }
                }

                // Write the description now
                if (string.IsNullOrEmpty(HelpDefinition))
                    HelpDefinition = Translate.DoTranslation("Command defined by ") + command;
                Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, Translate.DoTranslation("Description:") + $" {HelpDefinition}", true, ColorTools.ColTypes.ListValue);

                // Extra help action for some commands
                FinalCommandList[FinalCommand].CommandBase?.HelpHelper();
            }
            else if (string.IsNullOrWhiteSpace(command))
            {
                // List the available commands
                if (!Flags.SimHelp)
                {
                    // The built-in commands
                    Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, Translate.DoTranslation("General commands:") + (Flags.ShowCommandsCount & Flags.ShowShellCommandsCount ? " [{0}]" : ""), true, ColorTools.ColTypes.ListTitle, CommandList.Count);

                    // Check the command list count and print not implemented. This is an extremely rare situation.
                    if (CommandList.Count == 0)
                        Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, "- " + Translate.DoTranslation("Shell commands not implemented!!!"), true, ColorTools.ColTypes.Warning);
                    foreach (string cmd in CommandList.Keys)
                    {
                        if ((!CommandList[cmd].Flags.HasFlag(CommandFlags.Strict) | CommandList[cmd].Flags.HasFlag(CommandFlags.Strict) & GroupManagement.HasGroup(Login.Login.CurrentUser?.Username, GroupManagement.GroupType.Administrator)) & (Flags.Maintenance & !CommandList[cmd].Flags.HasFlag(CommandFlags.NoMaintenance) | !Flags.Maintenance))
                        {
                            Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, "- {0}: ", false, Shell.UnifiedCommandDict.ContainsKey(cmd) ? ColorTools.ColTypes.Success : ColorTools.ColTypes.ListEntry, cmd);
                            Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, "{0}", true, ColorTools.ColTypes.ListValue, CommandList[cmd].GetTranslatedHelpEntry());
                        }
                    }

                    // The mod commands
                    Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, Kernel.Kernel.NewLine + Translate.DoTranslation("Mod commands:") + (Flags.ShowCommandsCount & Flags.ShowModCommandsCount ? " [{0}]" : ""), true, ColorTools.ColTypes.ListTitle, ModCommandList.Count);
                    if (ModCommandList.Count == 0)
                        Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, "- " + Translate.DoTranslation("No mod commands."), true, ColorTools.ColTypes.Warning);
                    foreach (string cmd in ModCommandList.Keys)
                    {
                        Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, "- {0}: ", false, ColorTools.ColTypes.ListEntry, cmd);
                        Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, "{0}", true, ColorTools.ColTypes.ListValue, ModCommandList[cmd].HelpDefinition);
                    }

                    // The alias commands
                    Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, Kernel.Kernel.NewLine + Translate.DoTranslation("Alias commands:") + (Flags.ShowCommandsCount & Flags.ShowShellAliasesCount ? " [{0}]" : ""), true, ColorTools.ColTypes.ListTitle, AliasedCommandList.Count);
                    if (AliasedCommandList.Count == 0)
                        Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, "- " + Translate.DoTranslation("No alias commands."), true, ColorTools.ColTypes.Warning);
                    foreach (string cmd in AliasedCommandList.Keys)
                    {
                        Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, "- {0}: ", false, ColorTools.ColTypes.ListEntry, cmd);
                        Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, "{0}", true, ColorTools.ColTypes.ListValue, CommandList[AliasedCommandList[cmd]].GetTranslatedHelpEntry());
                    }

                    // A tip for you all
                    Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, Kernel.Kernel.NewLine + Translate.DoTranslation("* You can use multiple commands using the colon between commands."), true, ColorTools.ColTypes.Tip);
                    Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, "* " + Translate.DoTranslation("Commands highlighted in another color are unified commands and are available in every shell."), true, ColorTools.ColTypes.Tip);
                }
                else
                {
                    // The built-in commands
                    foreach (string cmd in CommandList.Keys)
                    {
                        if ((!CommandList[cmd].Flags.HasFlag(CommandFlags.Strict) | CommandList[cmd].Flags.HasFlag(CommandFlags.Strict) & GroupManagement.HasGroup(Login.Login.CurrentUser?.Username, GroupManagement.GroupType.Administrator)) & (Flags.Maintenance & !CommandList[cmd].Flags.HasFlag(CommandFlags.NoMaintenance) | !Flags.Maintenance))
                        {
                            Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, "{0}, ", false, ColorTools.ColTypes.ListEntry, cmd);
                        }
                    }

                    // The mod commands
                    foreach (string cmd in ModCommandList.Keys)
                        Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, "{0}, ", false, ColorTools.ColTypes.ListEntry, cmd);

                    // The alias commands
                    Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, string.Join(", ", AliasedCommandList.Keys), true, ColorTools.ColTypes.ListEntry);
                }
            }
            else
            {
                Decisive.DecisiveWrite(CommandType, DebugDeviceSocket, Translate.DoTranslation("No help for command \"{0}\"."), true, ColorTools.ColTypes.Error, command);
            }
        }

    }
}
