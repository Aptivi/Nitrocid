
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
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Text;
using KS.Modifications;
using KS.Shell.ShellBase.Aliases;
using KS.Shell.ShellBase.Shells;
using KS.Users;

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
        public static void ShowHelp() => ShowHelp("", ShellManager.CurrentShellType);

        /// <summary>
        /// Shows the list of commands under the specified shell type
        /// </summary>
        /// <param name="CommandType">A specified shell type</param>
        public static void ShowHelp(ShellType CommandType) => ShowHelp("", ShellManager.GetShellTypeName(CommandType));

        /// <summary>
        /// Shows the help of a command, or command list under the current shell type if nothing is specified
        /// </summary>
        /// <param name="command">A specified command</param>
        public static void ShowHelp(string command) => ShowHelp(command, ShellManager.CurrentShellType);

        /// <summary>
        /// Shows the help of a command, or command list under the specified shell type if nothing is specified
        /// </summary>
        /// <param name="command">A specified command</param>
        /// <param name="CommandType">A specified shell type</param>
        public static void ShowHelp(string command, ShellType CommandType) => ShowHelp(command, ShellManager.GetShellTypeName(CommandType));

        /// <summary>
        /// Shows the help of a command, or command list under the specified shell type if nothing is specified
        /// </summary>
        /// <param name="command">A specified command</param>
        /// <param name="CommandType">A specified shell type</param>
        public static void ShowHelp(string command, string CommandType)
        {
            // Determine command type
            var CommandList = ShellManager.GetShellInfo(CommandType).Commands
                .Union(ShellManager.UnifiedCommandDict)
                .Where((CommandValuePair) => !CommandValuePair.Value.Flags.HasFlag(CommandFlags.Hidden))
                .OrderBy((CommandValuePair) => CommandValuePair.Key)
                .ToDictionary((CommandValuePair) => CommandValuePair.Key, (CommandValuePair) => CommandValuePair.Value);
            var AliasedCommandList = AliasManager.GetAliasesListFromType(CommandType);

            // Add every command from each mod
            var ModCommandList = ModManager.ListModCommands(CommandType);

            // Add every command from each addon
            var AddonCommandList = ShellManager.GetShellInfo(CommandType).addonCommands;

            // Check to see if command exists
            if (!string.IsNullOrWhiteSpace(command) & (CommandList.ContainsKey(command) | AliasedCommandList.ContainsKey(command) | ModCommandList.ContainsKey(command) | AddonCommandList.ContainsKey(command)))
            {
                // Found!
                bool IsMod = ModCommandList.ContainsKey(command);
                bool IsAlias = AliasedCommandList.ContainsKey(command);
                bool IsAddon = AddonCommandList.ContainsKey(command);
                var FinalCommandList = IsMod ? ModCommandList : IsAddon ? AddonCommandList : CommandList;
                string FinalCommand = (IsMod || IsAddon) ? command : IsAlias ? AliasedCommandList[command] : command;
                string HelpDefinition = IsMod ? FinalCommandList[FinalCommand].HelpDefinition : FinalCommandList[FinalCommand].GetTranslatedHelpEntry();

                // Iterate through command argument information instances
                var argumentInfos = FinalCommandList[FinalCommand].CommandArgumentInfo ?? Array.Empty<CommandArgumentInfo>();
                foreach (var argumentInfo in argumentInfos)
                {
                    var Arguments = Array.Empty<string>();
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
                        TextWriterColor.Write(Translate.DoTranslation("Usage:") + $" {FinalCommand}", false, KernelColorType.ListEntry);

                        // Enumerate through the available switches first
                        foreach (var Switch in Switches)
                        {
                            bool required = Switch.IsRequired;
                            bool argRequired = Switch.ArgumentsRequired;
                            bool acceptsValue = Switch.AcceptsValues;
                            string switchName = Switch.SwitchName;
                            string renderedSwitchValue = argRequired ? $"=value" : acceptsValue ? $"[=value]" : "";
                            string renderedSwitch = required ? $" <-{switchName}{renderedSwitchValue}>" : $" [-{switchName}{renderedSwitchValue}]";
                            TextWriterColor.Write(renderedSwitch, false, KernelColorType.ListEntry);
                        }

                        // Enumerate through the available arguments
                        int howManyRequired = argumentInfo.MinimumArguments;
                        int queriedArgs = 1;
                        foreach (string Argument in Arguments)
                        {
                            bool required = argumentInfo.ArgumentsRequired && queriedArgs <= howManyRequired;
                            string renderedArgument = required ? $" <{Argument}>" : $" [{Argument}]";
                            TextWriterColor.Write(renderedArgument, false, KernelColorType.ListEntry);
                            queriedArgs++;
                        }
                        TextWriterColor.Write();
                    }
                    else
                        TextWriterColor.Write(Translate.DoTranslation("Usage:") + $" {FinalCommand}", true, KernelColorType.ListEntry);

                    // If we have switches, print their descriptions
                    if (Switches.Length != 0)
                    {
                        TextWriterColor.Write(Translate.DoTranslation("This command has the below switches that change how it works:"));
                        foreach (var Switch in Switches)
                        {
                            string switchName = Switch.SwitchName;
                            string switchDesc = IsMod ? Switch.HelpDefinition : Switch.GetTranslatedHelpEntry();
                            TextWriterColor.Write($"  -{switchName}: ", false, KernelColorType.ListEntry);
                            TextWriterColor.Write(switchDesc, true, KernelColorType.ListValue);
                        }
                    }
                }

                // Write the description now
                if (string.IsNullOrEmpty(HelpDefinition))
                    HelpDefinition = Translate.DoTranslation("Command defined by ") + command;
                TextWriterColor.Write(Translate.DoTranslation("Description:") + $" {HelpDefinition}", true, KernelColorType.ListValue);

                // Extra help action for some commands
                FinalCommandList[FinalCommand].CommandBase?.HelpHelper();
            }
            else if (string.IsNullOrWhiteSpace(command))
            {
                // List the available commands
                if (!Flags.SimHelp)
                {
                    // The built-in commands
                    TextWriterColor.Write(Translate.DoTranslation("General commands:") + (Flags.ShowCommandsCount & Flags.ShowShellCommandsCount ? " [{0}]" : ""), true, KernelColorType.ListTitle, CommandList.Count);

                    // Check the command list count and print not implemented. This is an extremely rare situation.
                    if (CommandList.Count == 0)
                        TextWriterColor.Write("- " + Translate.DoTranslation("Shell commands not implemented!!!"), true, KernelColorType.Warning);
                    foreach (string cmd in CommandList.Keys)
                    {
                        if ((!CommandList[cmd].Flags.HasFlag(CommandFlags.Strict) | CommandList[cmd].Flags.HasFlag(CommandFlags.Strict) & UserManagement.CurrentUser.Admin) & (Flags.Maintenance & !CommandList[cmd].Flags.HasFlag(CommandFlags.NoMaintenance) | !Flags.Maintenance))
                        {
                            TextWriterColor.Write("- {0}: ", false, ShellManager.UnifiedCommandDict.ContainsKey(cmd) ? KernelColorType.Success : KernelColorType.ListEntry, cmd);
                            TextWriterColor.Write("{0}", true, KernelColorType.ListValue, CommandList[cmd].GetTranslatedHelpEntry());
                        }
                    }

                    // The addon commands
                    TextWriterColor.Write(CharManager.NewLine + Translate.DoTranslation("Kernel addon commands:") + (Flags.ShowCommandsCount & Flags.ShowModCommandsCount ? " [{0}]" : ""), true, KernelColorType.ListTitle, ModCommandList.Count);
                    if (AddonCommandList.Count == 0)
                        TextWriterColor.Write("- " + Translate.DoTranslation("No kernel addon commands."), true, KernelColorType.Warning);
                    foreach (string cmd in AddonCommandList.Keys)
                    {
                        TextWriterColor.Write("- {0}: ", false, KernelColorType.ListEntry, cmd);
                        TextWriterColor.Write("{0}", true, KernelColorType.ListValue, AddonCommandList[cmd].GetTranslatedHelpEntry());
                    }

                    // The mod commands
                    TextWriterColor.Write(CharManager.NewLine + Translate.DoTranslation("Mod commands:") + (Flags.ShowCommandsCount & Flags.ShowModCommandsCount ? " [{0}]" : ""), true, KernelColorType.ListTitle, ModCommandList.Count);
                    if (ModCommandList.Count == 0)
                        TextWriterColor.Write("- " + Translate.DoTranslation("No mod commands."), true, KernelColorType.Warning);
                    foreach (string cmd in ModCommandList.Keys)
                    {
                        TextWriterColor.Write("- {0}: ", false, KernelColorType.ListEntry, cmd);
                        TextWriterColor.Write("{0}", true, KernelColorType.ListValue, ModCommandList[cmd].HelpDefinition);
                    }

                    // The alias commands
                    TextWriterColor.Write(CharManager.NewLine + Translate.DoTranslation("Alias commands:") + (Flags.ShowCommandsCount & Flags.ShowShellAliasesCount ? " [{0}]" : ""), true, KernelColorType.ListTitle, AliasedCommandList.Count);
                    if (AliasedCommandList.Count == 0)
                        TextWriterColor.Write("- " + Translate.DoTranslation("No alias commands."), true, KernelColorType.Warning);
                    foreach (string cmd in AliasedCommandList.Keys)
                    {
                        TextWriterColor.Write("- {0}: ", false, KernelColorType.ListEntry, cmd);
                        TextWriterColor.Write("{0}", true, KernelColorType.ListValue, CommandList[AliasedCommandList[cmd]].GetTranslatedHelpEntry());
                    }

                    // A tip for you all
                    TextWriterColor.Write(CharManager.NewLine + Translate.DoTranslation("* You can use multiple commands using the semicolon between commands."), true, KernelColorType.Tip);
                    TextWriterColor.Write("* " + Translate.DoTranslation("Commands highlighted in another color are unified commands and are available in every shell."), true, KernelColorType.Tip);
                }
                else
                {
                    // The built-in commands
                    foreach (string cmd in CommandList.Keys)
                        if ((!CommandList[cmd].Flags.HasFlag(CommandFlags.Strict) | CommandList[cmd].Flags.HasFlag(CommandFlags.Strict) & UserManagement.CurrentUser.Admin) & (Flags.Maintenance & !CommandList[cmd].Flags.HasFlag(CommandFlags.NoMaintenance) | !Flags.Maintenance))
                            TextWriterColor.Write("{0}, ", false, KernelColorType.ListEntry, cmd);

                    // The mod commands
                    foreach (string cmd in ModCommandList.Keys)
                        TextWriterColor.Write("{0}, ", false, KernelColorType.ListEntry, cmd);

                    // The alias commands
                    TextWriterColor.Write(string.Join(", ", AliasedCommandList.Keys), true, KernelColorType.ListEntry);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("No help for command \"{0}\"."), true, KernelColorType.Error, command);
            }
        }

    }
}
