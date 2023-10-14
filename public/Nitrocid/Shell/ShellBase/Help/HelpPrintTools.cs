
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
using System.Collections.Generic;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Configuration;
using KS.Languages;
using KS.Misc.Text;
using KS.Modifications;
using KS.Shell.ShellBase.Aliases;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.ShellBase.Switches;
using KS.Users;

namespace KS.Shell.ShellBase.Help
{
    internal static class HelpPrintTools
    {
        internal static Dictionary<string, CommandInfo> GetCommandsNoHidden(string commandType)
        {
            // Determine command type
            var CommandList = ShellManager.GetShellInfo(commandType).Commands
                .Where((CommandValuePair) => !CommandValuePair.Value.Flags.HasFlag(CommandFlags.Hidden))
                .OrderBy((CommandValuePair) => CommandValuePair.Key)
                .ToDictionary((CommandValuePair) => CommandValuePair.Key, (CommandValuePair) => CommandValuePair.Value);
            return CommandList;
        }

        internal static void ShowCommandList(string commandType, bool showGeneral = true, bool showMod = false, bool showAlias = false, bool showUnified = false, bool showAddon = false)
        {
            // Get general commands
            var commands = CommandManager.GetCommands(commandType);
            var commandList = GetCommandsNoHidden(commandType);

            // Add every command from each mod, addon, and alias
            var ModCommandList = ModManager.ListModCommands(commandType);
            var AddonCommandList = ShellManager.GetShellInfo(commandType).addonCommands;
            var unifiedCommandList = ShellManager.unifiedCommandDict;
            var AliasedCommandList = AliasManager.GetAliasesListFromType(commandType);
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Available commands:") + (KernelFlags.ShowCommandsCount ? " [{0}]" : ""), true, KernelColorType.ListTitle, commands.Count);

            // The built-in commands
            if (showGeneral)
            {
                TextWriterColor.WriteKernelColor(CharManager.NewLine + Translate.DoTranslation("General commands:") + (KernelFlags.ShowCommandsCount & KernelFlags.ShowShellCommandsCount ? " [{0}]" : ""), true, KernelColorType.ListTitle, commandList.Count);
                if (commandList.Count == 0)
                    TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Shell commands not implemented!!!"), true, KernelColorType.Warning);
                foreach (string cmd in commandList.Keys)
                {
                    if ((!commandList[cmd].Flags.HasFlag(CommandFlags.Strict) | commandList[cmd].Flags.HasFlag(CommandFlags.Strict) & UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator)) & (KernelFlags.Maintenance & !commandList[cmd].Flags.HasFlag(CommandFlags.NoMaintenance) | !KernelFlags.Maintenance))
                    {
                        TextWriterColor.WriteKernelColor("  - {0}: ", false, KernelColorType.ListEntry, cmd);
                        TextWriterColor.WriteKernelColor("{0}", true, KernelColorType.ListValue, commandList[cmd].GetTranslatedHelpEntry());
                    }
                }
            }

            // The addon commands
            if (showAddon)
            {
                TextWriterColor.WriteKernelColor(CharManager.NewLine + Translate.DoTranslation("Kernel addon commands:") + (KernelFlags.ShowCommandsCount & KernelFlags.ShowAddonCommandsCount ? " [{0}]" : ""), true, KernelColorType.ListTitle, AddonCommandList.Count);
                if (AddonCommandList.Count == 0)
                    TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("No kernel addon commands."), true, KernelColorType.Warning);
                foreach (string cmd in AddonCommandList.Keys)
                {
                    if ((!AddonCommandList[cmd].Flags.HasFlag(CommandFlags.Strict) | AddonCommandList[cmd].Flags.HasFlag(CommandFlags.Strict) & UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator)) & (KernelFlags.Maintenance & !AddonCommandList[cmd].Flags.HasFlag(CommandFlags.NoMaintenance) | !KernelFlags.Maintenance))
                    {
                        TextWriterColor.WriteKernelColor("  - {0}: ", false, KernelColorType.ListEntry, cmd);
                        TextWriterColor.WriteKernelColor("{0}", true, KernelColorType.ListValue, AddonCommandList[cmd].GetTranslatedHelpEntry());
                    }
                }
            }

            // The mod commands
            if (showMod)
            {
                TextWriterColor.WriteKernelColor(CharManager.NewLine + Translate.DoTranslation("Mod commands:") + (KernelFlags.ShowCommandsCount & KernelFlags.ShowModCommandsCount ? " [{0}]" : ""), true, KernelColorType.ListTitle, ModCommandList.Count);
                if (ModCommandList.Count == 0)
                    TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("No mod commands."), true, KernelColorType.Warning);
                foreach (string cmd in ModCommandList.Keys)
                {
                    if ((!ModCommandList[cmd].Flags.HasFlag(CommandFlags.Strict) | ModCommandList[cmd].Flags.HasFlag(CommandFlags.Strict) & UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator)) & (KernelFlags.Maintenance & !ModCommandList[cmd].Flags.HasFlag(CommandFlags.NoMaintenance) | !KernelFlags.Maintenance))
                    {
                        TextWriterColor.WriteKernelColor("  - {0}: ", false, KernelColorType.ListEntry, cmd);
                        TextWriterColor.WriteKernelColor("{0}", true, KernelColorType.ListValue, ModCommandList[cmd].HelpDefinition);
                    }
                }
            }

            // The alias commands
            if (showAlias)
            {
                TextWriterColor.WriteKernelColor(CharManager.NewLine + Translate.DoTranslation("Alias commands:") + (KernelFlags.ShowCommandsCount & KernelFlags.ShowShellAliasesCount ? " [{0}]" : ""), true, KernelColorType.ListTitle, AliasedCommandList.Count);
                if (AliasedCommandList.Count == 0)
                    TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("No alias commands."), true, KernelColorType.Warning);
                foreach (var cmd in AliasedCommandList)
                {
                    if ((!cmd.TargetCommand.Flags.HasFlag(CommandFlags.Strict) | cmd.TargetCommand.Flags.HasFlag(CommandFlags.Strict) & UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator)) & (KernelFlags.Maintenance & !cmd.TargetCommand.Flags.HasFlag(CommandFlags.NoMaintenance) | !KernelFlags.Maintenance))
                    {
                        TextWriterColor.WriteKernelColor("  - {0} -> {1}: ", false, KernelColorType.ListEntry, cmd.Alias, cmd.Command);
                        TextWriterColor.WriteKernelColor("{0}", true, KernelColorType.ListValue, cmd.TargetCommand.GetTranslatedHelpEntry());
                    }
                }
            }

            // The unified commands
            if (showUnified)
            {
                TextWriterColor.WriteKernelColor(CharManager.NewLine + Translate.DoTranslation("Unified commands:") + (KernelFlags.ShowCommandsCount & KernelFlags.ShowUnifiedCommandsCount ? " [{0}]" : ""), true, KernelColorType.ListTitle, unifiedCommandList.Count);
                if (unifiedCommandList.Count == 0)
                    TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Unified commands not implemented!!!"), true, KernelColorType.Warning);
                foreach (string cmd in unifiedCommandList.Keys)
                {
                    if ((!unifiedCommandList[cmd].Flags.HasFlag(CommandFlags.Strict) | unifiedCommandList[cmd].Flags.HasFlag(CommandFlags.Strict) & UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator)) & (KernelFlags.Maintenance & !unifiedCommandList[cmd].Flags.HasFlag(CommandFlags.NoMaintenance) | !KernelFlags.Maintenance))
                    {
                        TextWriterColor.WriteKernelColor("  - {0}: ", false, KernelColorType.ListEntry, cmd);
                        TextWriterColor.WriteKernelColor("{0}", true, KernelColorType.ListValue, unifiedCommandList[cmd].GetTranslatedHelpEntry());
                    }
                }
            }
        }

        internal static void ShowCommandListSimplified(string commandType)
        {
            // Get visible commands
            var commands = CommandManager.GetCommands(commandType);
            List<string> finalCommand = new();
            foreach (string cmd in commands.Keys)
            {
                // Get the necessary flags
                bool hasAdmin = UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator);
                bool isStrict = commands[cmd].Flags.HasFlag(CommandFlags.Strict);
                bool isNoMaintenance = commands[cmd].Flags.HasFlag(CommandFlags.NoMaintenance);

                // Now, populate the command list
                if ((!isStrict | isStrict & hasAdmin) &
                    (KernelFlags.Maintenance & !isNoMaintenance | !KernelFlags.Maintenance))
                    finalCommand.Add(cmd);
            }
            TextWriterColor.Write(string.Join(", ", finalCommand));
        }

        internal static void ShowHelpUsage(string command, string commandType)
        {
            // Determine command type
            var CommandList = ShellManager.GetShellInfo(commandType).Commands
                .Where((CommandValuePair) => !CommandValuePair.Value.Flags.HasFlag(CommandFlags.Hidden))
                .OrderBy((CommandValuePair) => CommandValuePair.Key)
                .ToDictionary((CommandValuePair) => CommandValuePair.Key, (CommandValuePair) => CommandValuePair.Value);

            // Add every command from each mod, addon, and alias
            var ModCommandList = ModManager.ListModCommands(commandType);
            var AddonCommandList = ShellManager.GetShellInfo(commandType).addonCommands;
            var unifiedCommandList = ShellManager.unifiedCommandDict;
            var AliasedCommandList = AliasManager.GetAliasesListFromType(commandType);
            var totalCommandList = CommandManager.GetCommands(commandType);

            // Check to see if command exists
            if (!string.IsNullOrWhiteSpace(command) &&
                (CommandList.ContainsKey(command) ||
                AliasedCommandList.Any((info) => info.Alias == command) ||
                ModCommandList.ContainsKey(command) ||
                AddonCommandList.ContainsKey(command) ||
                unifiedCommandList.ContainsKey(command)))
            {
                // Found!
                bool IsMod = ModCommandList.ContainsKey(command);
                bool IsAlias = AliasedCommandList.Any((info) => info.Alias == command);
                bool IsAddon = AddonCommandList.ContainsKey(command);
                bool IsUnified = unifiedCommandList.ContainsKey(command);
                var FinalCommandList =
                    IsMod ? ModCommandList :
                    IsAddon ? AddonCommandList :
                    IsAlias ? AliasedCommandList.ToDictionary((info) => info.Command, (info) => info.TargetCommand) :
                    IsUnified ? unifiedCommandList :
                    CommandList;
                string FinalCommand =
                    IsMod || IsAddon ? command :
                    IsAlias ? AliasManager.GetAlias(command, commandType).Command :
                    IsUnified ? unifiedCommandList[command].Command :
                    command;
                string HelpDefinition = FinalCommandList[FinalCommand].GetTranslatedHelpEntry();

                // Iterate through command argument information instances
                var argumentInfos = FinalCommandList[FinalCommand].CommandArgumentInfo ??
                    Array.Empty<CommandArgumentInfo>();
                foreach (var argumentInfo in argumentInfos)
                {
                    var Arguments = Array.Empty<CommandArgumentPart>();
                    var Switches = Array.Empty<SwitchInfo>();

                    // Populate help usages
                    if (argumentInfo is not null)
                    {
                        Arguments = argumentInfo.Arguments;
                        Switches = argumentInfo.Switches;
                    }

                    // Print usage information
                    if (Arguments.Length != 0 || Switches.Length != 0)
                    {
                        // Print the usage information holder
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("Usage:") + $" {FinalCommand}", false, KernelColorType.ListEntry);

                        // Enumerate through the available switches first
                        foreach (var Switch in Switches)
                        {
                            bool required = Switch.IsRequired;
                            bool argRequired = Switch.ArgumentsRequired;
                            bool acceptsValue = Switch.AcceptsValues;
                            string switchName = Switch.SwitchName;
                            string renderedSwitchValue = argRequired ? $"=value" : acceptsValue ? $"[=value]" : "";
                            string renderedSwitch =
                                required ?
                                $" <-{switchName}{renderedSwitchValue}>" :
                                $" [-{switchName}{renderedSwitchValue}]";
                            TextWriterColor.WriteKernelColor(renderedSwitch, false, KernelColorType.ListEntry);
                        }

                        // Enumerate through the available arguments
                        int howManyRequired = argumentInfo.MinimumArguments;
                        int queriedArgs = 1;
                        foreach (var Argument in Arguments)
                        {
                            bool required = argumentInfo.ArgumentsRequired && queriedArgs <= howManyRequired;
                            bool justNumeric = Argument.IsNumeric;
                            string renderedArgument =
                                required ?
                                $" <{Argument.ArgumentExpression}{(justNumeric ? ":int" : "")}>" :
                                $" [{Argument.ArgumentExpression}{(justNumeric ? ":int" : "")}]";
                            TextWriterColor.WriteKernelColor(renderedArgument, false, KernelColorType.ListEntry);
                            queriedArgs++;
                        }
                        TextWriterColor.Write();
                    }
                    else
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("Usage:") + $" {FinalCommand}", true, KernelColorType.ListEntry);

                    // If we have switches, print their descriptions
                    if (Switches.Length != 0)
                    {
                        TextWriterColor.Write(Translate.DoTranslation("This command has the below switches that change how it works:"));
                        foreach (var Switch in Switches)
                        {
                            string switchName = Switch.SwitchName;
                            string switchDesc = IsMod ? Switch.HelpDefinition : Switch.GetTranslatedHelpEntry();
                            TextWriterColor.WriteKernelColor($"  -{switchName}: ", false, KernelColorType.ListEntry);
                            TextWriterColor.WriteKernelColor(switchDesc, true, KernelColorType.ListValue);
                        }
                    }
                }

                // Write the description now
                if (string.IsNullOrEmpty(HelpDefinition))
                    HelpDefinition = Translate.DoTranslation("Command defined by ") + command;
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Description:") + $" {HelpDefinition}", true, KernelColorType.ListValue);

                // Extra help action for some commands
                FinalCommandList[FinalCommand].CommandBase?.HelpHelper();
            }
            else
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("No help for command \"{0}\"."), true, KernelColorType.Error, command);
        }
    }
}
