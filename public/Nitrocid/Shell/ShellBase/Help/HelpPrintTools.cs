//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using System;
using System.Collections.Generic;
using System.Linq;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.ConsoleBase.Writers.ConsoleWriters;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Languages;
using Nitrocid.Modifications;
using Nitrocid.Shell.ShellBase.Aliases;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Users;
using Textify.General;

namespace Nitrocid.Shell.ShellBase.Help
{
    internal static class HelpPrintTools
    {
        /// <summary>
        /// Simplified Help Command
        /// </summary>
        public static bool SimHelp =>
            Config.MainConfig.SimHelp;

        /// <summary>
        /// Shows how many commands available in help for shells
        /// </summary>
        public static bool ShowCommandsCount =>
            Config.MainConfig.ShowCommandsCount;

        /// <summary>
        /// Shows how many shell commands available in help for shells
        /// </summary>
        public static bool ShowShellCommandsCount =>
            Config.MainConfig.ShowShellCommandsCount;

        /// <summary>
        /// Shows how many mod commands available in help for shells
        /// </summary>
        public static bool ShowModCommandsCount =>
            Config.MainConfig.ShowModCommandsCount;

        /// <summary>
        /// Shows how many aliases available in help for shells
        /// </summary>
        public static bool ShowShellAliasesCount =>
            Config.MainConfig.ShowShellAliasesCount;

        /// <summary>
        /// Shows how many unified commands available in help for shells
        /// </summary>
        public static bool ShowUnifiedCommandsCount =>
            Config.MainConfig.ShowUnifiedCommandsCount;

        /// <summary>
        /// Shows how many addon commands available in help for shells
        /// </summary>
        public static bool ShowAddonCommandsCount =>
            Config.MainConfig.ShowAddonCommandsCount;

        internal static void ShowCommandList(string commandType, bool showGeneral = true, bool showMod = false, bool showAlias = false, bool showUnified = false, bool showAddon = false)
        {
            // Get general commands
            var commands = CommandManager.GetCommands(commandType);
            var commandList = ShellManager.GetShellInfo(commandType).Commands;

            // Add every command from each mod, addon, and alias
            var ModCommandList = ModManager.ListModCommands(commandType);
            var AddonCommandList = ShellManager.GetShellInfo(commandType).addonCommands;
            var unifiedCommandList = ShellManager.unifiedCommandDict;
            var AliasedCommandList = AliasManager.GetEntireAliasListFromType(commandType)
                .ToDictionary((ai) => ai, (ai) => ai.TargetCommand);
            TextWriters.Write(Translate.DoTranslation("Available commands:") + (ShowCommandsCount ? " [{0}]" : ""), true, KernelColorType.ListTitle, commands.Count);

            // The built-in commands
            if (showGeneral)
            {
                TextWriters.Write(CharManager.NewLine + Translate.DoTranslation("General commands:") + (ShowCommandsCount & ShowShellCommandsCount ? " [{0}]" : ""), true, KernelColorType.ListTitle, commandList.Count);
                if (commandList.Count == 0)
                    TextWriters.Write("- " + Translate.DoTranslation("Shell commands not implemented!!!"), true, KernelColorType.Warning);
                foreach (string cmd in commandList.Keys)
                {
                    if ((!commandList[cmd].Flags.HasFlag(CommandFlags.Strict) | commandList[cmd].Flags.HasFlag(CommandFlags.Strict) & UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator)) & (KernelEntry.Maintenance & !commandList[cmd].Flags.HasFlag(CommandFlags.NoMaintenance) | !KernelEntry.Maintenance))
                    {
                        TextWriters.Write("  - {0}: ", false, KernelColorType.ListEntry, cmd);
                        TextWriters.Write("{0}", true, KernelColorType.ListValue, commandList[cmd].GetTranslatedHelpEntry());
                    }
                }
            }

            // The addon commands
            if (showAddon)
            {
                TextWriters.Write(CharManager.NewLine + Translate.DoTranslation("Kernel addon commands:") + (ShowCommandsCount & ShowAddonCommandsCount ? " [{0}]" : ""), true, KernelColorType.ListTitle, AddonCommandList.Count);
                if (AddonCommandList.Count == 0)
                    TextWriters.Write("- " + Translate.DoTranslation("No kernel addon commands."), true, KernelColorType.Warning);
                foreach (string cmd in AddonCommandList.Keys)
                {
                    if ((!AddonCommandList[cmd].Flags.HasFlag(CommandFlags.Strict) | AddonCommandList[cmd].Flags.HasFlag(CommandFlags.Strict) & UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator)) & (KernelEntry.Maintenance & !AddonCommandList[cmd].Flags.HasFlag(CommandFlags.NoMaintenance) | !KernelEntry.Maintenance))
                    {
                        TextWriters.Write("  - {0}: ", false, KernelColorType.ListEntry, cmd);
                        TextWriters.Write("{0}", true, KernelColorType.ListValue, AddonCommandList[cmd].GetTranslatedHelpEntry());
                    }
                }
            }

            // The mod commands
            if (showMod)
            {
                TextWriters.Write(CharManager.NewLine + Translate.DoTranslation("Mod commands:") + (ShowCommandsCount & ShowModCommandsCount ? " [{0}]" : ""), true, KernelColorType.ListTitle, ModCommandList.Count);
                if (ModCommandList.Count == 0)
                    TextWriters.Write("- " + Translate.DoTranslation("No mod commands."), true, KernelColorType.Warning);
                foreach (string cmd in ModCommandList.Keys)
                {
                    if ((!ModCommandList[cmd].Flags.HasFlag(CommandFlags.Strict) | ModCommandList[cmd].Flags.HasFlag(CommandFlags.Strict) & UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator)) & (KernelEntry.Maintenance & !ModCommandList[cmd].Flags.HasFlag(CommandFlags.NoMaintenance) | !KernelEntry.Maintenance))
                    {
                        TextWriters.Write("  - {0}: ", false, KernelColorType.ListEntry, cmd);
                        TextWriters.Write("{0}", true, KernelColorType.ListValue, ModCommandList[cmd].GetTranslatedHelpEntry());
                    }
                }
            }

            // The alias commands
            if (showAlias)
            {
                TextWriters.Write(CharManager.NewLine + Translate.DoTranslation("Alias commands:") + (ShowCommandsCount & ShowShellAliasesCount ? " [{0}]" : ""), true, KernelColorType.ListTitle, AliasedCommandList.Count);
                if (AliasedCommandList.Count == 0)
                    TextWriters.Write("- " + Translate.DoTranslation("No alias commands."), true, KernelColorType.Warning);
                foreach (var cmd in AliasedCommandList.Keys)
                {
                    if ((!AliasedCommandList[cmd].Flags.HasFlag(CommandFlags.Strict) | AliasedCommandList[cmd].Flags.HasFlag(CommandFlags.Strict) & UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator)) & (KernelEntry.Maintenance & !AliasedCommandList[cmd].Flags.HasFlag(CommandFlags.NoMaintenance) | !KernelEntry.Maintenance))
                    {
                        TextWriters.Write("  - {0} -> {1}: ", false, KernelColorType.ListEntry, cmd.Alias, cmd.Command);
                        TextWriters.Write("{0}", true, KernelColorType.ListValue, AliasedCommandList[cmd].GetTranslatedHelpEntry());
                    }
                }
            }

            // The unified commands
            if (showUnified)
            {
                TextWriters.Write(CharManager.NewLine + Translate.DoTranslation("Unified commands:") + (ShowCommandsCount & ShowUnifiedCommandsCount ? " [{0}]" : ""), true, KernelColorType.ListTitle, unifiedCommandList.Count);
                if (unifiedCommandList.Count == 0)
                    TextWriters.Write("- " + Translate.DoTranslation("Unified commands not implemented!!!"), true, KernelColorType.Warning);
                foreach (string cmd in unifiedCommandList.Keys)
                {
                    if ((!unifiedCommandList[cmd].Flags.HasFlag(CommandFlags.Strict) | unifiedCommandList[cmd].Flags.HasFlag(CommandFlags.Strict) & UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator)) & (KernelEntry.Maintenance & !unifiedCommandList[cmd].Flags.HasFlag(CommandFlags.NoMaintenance) | !KernelEntry.Maintenance))
                    {
                        TextWriters.Write("  - {0}: ", false, KernelColorType.ListEntry, cmd);
                        TextWriters.Write("{0}", true, KernelColorType.ListValue, unifiedCommandList[cmd].GetTranslatedHelpEntry());
                    }
                }
            }
        }

        internal static void ShowCommandListSimplified(string commandType)
        {
            // Get visible commands
            var commands = CommandManager.GetCommands(commandType);
            List<string> finalCommand = [];
            foreach (string cmd in commands.Keys)
            {
                // Get the necessary flags
                bool hasAdmin = UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator);
                bool isStrict = commands[cmd].Flags.HasFlag(CommandFlags.Strict);
                bool isNoMaintenance = commands[cmd].Flags.HasFlag(CommandFlags.NoMaintenance);

                // Now, populate the command list
                if ((!isStrict | isStrict & hasAdmin) &
                    (KernelEntry.Maintenance & !isNoMaintenance | !KernelEntry.Maintenance))
                    finalCommand.Add(cmd);
            }
            TextWriterColor.Write(string.Join(", ", finalCommand));
        }

        internal static void ShowHelpUsage(string command, string commandType)
        {
            // Determine command type
            var CommandList = ShellManager.GetShellInfo(commandType).Commands;

            // Add every command from each mod, addon, and alias
            var ModCommandList = ModManager.ListModCommands(commandType);
            var AddonCommandList = ShellManager.GetShellInfo(commandType).addonCommands;
            var unifiedCommandList = ShellManager.unifiedCommandDict;
            var AliasedCommandList = AliasManager.GetEntireAliasListFromType(commandType)
                .ToDictionary((ai) => ai, (ai) => ai.TargetCommand);
            var totalCommandList = CommandManager.GetCommands(commandType);

            // Check to see if command exists
            if (!string.IsNullOrWhiteSpace(command) &&
                (CommandList.ContainsKey(command) ||
                AliasedCommandList.Any((info) => info.Key.Alias == command) ||
                ModCommandList.ContainsKey(command) ||
                AddonCommandList.ContainsKey(command) ||
                unifiedCommandList.ContainsKey(command)))
            {
                // Found!
                bool IsMod = ModCommandList.ContainsKey(command);
                bool IsAlias = AliasedCommandList.Any((info) => info.Key.Alias == command);
                bool IsAddon = AddonCommandList.ContainsKey(command);
                bool IsUnified = unifiedCommandList.ContainsKey(command);
                var FinalCommandList =
                    IsMod ? ModCommandList :
                    IsAddon ? AddonCommandList :
                    IsAlias ? AliasedCommandList.ToDictionary((info) => info.Key.Command, (info) => info.Key.TargetCommand) :
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
                    [];
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
                    TextWriters.Write(Translate.DoTranslation("Usage:") + $" {FinalCommand} {argumentInfo.RenderedUsage}", true, KernelColorType.ListEntry);

                    // If we have switches, print their descriptions
                    if (Switches.Length != 0)
                    {
                        TextWriterColor.Write(Translate.DoTranslation("This command has the below switches that change how it works:"));
                        foreach (var Switch in Switches)
                        {
                            string switchName = Switch.SwitchName;
                            string switchDesc = Switch.GetTranslatedHelpEntry();
                            TextWriters.Write($"  -{switchName}: ", false, KernelColorType.ListEntry);
                            TextWriters.Write(switchDesc, true, KernelColorType.ListValue);
                        }
                    }
                }

                // Write the description now
                if (string.IsNullOrEmpty(HelpDefinition))
                    HelpDefinition = Translate.DoTranslation("Command defined by ") + command;
                TextWriters.Write(Translate.DoTranslation("Description:") + $" {HelpDefinition}", true, KernelColorType.ListValue);

                // Extra help action for some commands
                FinalCommandList[FinalCommand].CommandBase?.HelpHelper();
            }
            else
                TextWriters.Write(Translate.DoTranslation("No help for command \"{0}\"."), true, KernelColorType.Error, command);
        }
    }
}
