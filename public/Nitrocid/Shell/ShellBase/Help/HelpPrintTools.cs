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

using System;
using System.Collections.Generic;
using System.Linq;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Languages;
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
        internal static void ShowCommandList(string commandType, bool showGeneral = true, bool showMod = false, bool showAlias = false, bool showUnified = false, bool showAddon = false)
        {
            // Get general commands
            var shellInfo = ShellManager.GetShellInfo(commandType);
            var commands = CommandManager.GetCommands(commandType);
            var commandList = shellInfo.Commands;

            // Add every command from each mod, addon, and alias
            var ModCommandList = shellInfo.ModCommands;
            var AddonCommandList = shellInfo.addonCommands;
            var unifiedCommandList = ShellManager.unifiedCommandDict;
            var AliasedCommandList = AliasManager.GetEntireAliasListFromType(commandType)
                .ToDictionary((ai) => ai, (ai) => ai.TargetCommand);
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_BASE_HELP_CMDLISTING") + (Config.MainConfig.ShowCommandsCount ? " [{0}]" : ""), true, KernelColorType.ListTitle, commands.Length);

            // The built-in commands
            if (showGeneral)
            {
                TextWriters.Write(CharManager.NewLine + LanguageTools.GetLocalized("NKS_SHELL_BASE_HELP_GENERALLISTING") + (Config.MainConfig.ShowCommandsCount & Config.MainConfig.ShowShellCommandsCount ? " [{0}]" : ""), true, KernelColorType.ListTitle, commandList.Count);
                if (commandList.Count == 0)
                    TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_SHELL_BASE_HELP_NOLISTING"), true, KernelColorType.Warning);
                foreach (var cmd in commandList)
                {
                    if ((!cmd.Flags.HasFlag(CommandFlags.Strict) | cmd.Flags.HasFlag(CommandFlags.Strict) & UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator)) & (KernelEntry.Maintenance & !cmd.Flags.HasFlag(CommandFlags.NoMaintenance) | !KernelEntry.Maintenance))
                    {
                        string[] usages = [.. cmd.CommandArgumentInfo.Select((cai) => cai.RenderedUsage).Where((usage) => !string.IsNullOrEmpty(usage))];
                        TextWriters.Write("  - {0}{1}: ", false, KernelColorType.ListEntry, cmd.Command, usages.Length > 0 ? $" {string.Join(" | ", usages)}" : "");
                        TextWriters.Write("{0}", true, KernelColorType.ListValue, LanguageTools.GetLocalized(cmd.HelpDefinition));
                    }
                }
            }

            // The addon commands
            if (showAddon)
            {
                TextWriters.Write(CharManager.NewLine + LanguageTools.GetLocalized("NKS_SHELL_BASE_HELP_ADDONLISTING") + (Config.MainConfig.ShowCommandsCount & Config.MainConfig.ShowAddonCommandsCount ? " [{0}]" : ""), true, KernelColorType.ListTitle, AddonCommandList.Count);
                if (AddonCommandList.Count == 0)
                    TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_SHELL_BASE_HELP_NOADDONLISTING"), true, KernelColorType.Warning);
                foreach (var cmd in AddonCommandList)
                {
                    if ((!cmd.Flags.HasFlag(CommandFlags.Strict) | cmd.Flags.HasFlag(CommandFlags.Strict) & UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator)) & (KernelEntry.Maintenance & !cmd.Flags.HasFlag(CommandFlags.NoMaintenance) | !KernelEntry.Maintenance))
                    {
                        string[] usages = [.. cmd.CommandArgumentInfo.Select((cai) => cai.RenderedUsage).Where((usage) => !string.IsNullOrEmpty(usage))];
                        TextWriters.Write("  - {0}{1}: ", false, KernelColorType.ListEntry, cmd.Command, usages.Length > 0 ? $" {string.Join(" | ", usages)}" : "");
                        TextWriters.Write("{0}", true, KernelColorType.ListValue, LanguageTools.GetLocalized(cmd.HelpDefinition));
                    }
                }
            }

            // The mod commands
            if (showMod)
            {
                var modSettingsInstance = Config.baseConfigurations["ModsConfig"];
                var modEnableCountKey = ConfigTools.GetSettingsKey(modSettingsInstance, "ShowModCommandsCount");
                bool showModCommandsCount = (bool)(ConfigTools.GetValueFromEntry(modEnableCountKey, modSettingsInstance) ?? false);
                TextWriters.Write(CharManager.NewLine + LanguageTools.GetLocalized("NKS_SHELL_BASE_HELP_MODLISTING") + (Config.MainConfig.ShowCommandsCount & showModCommandsCount ? " [{0}]" : ""), true, KernelColorType.ListTitle, ModCommandList.Count);
                if (ModCommandList.Count == 0)
                    TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_SHELL_BASE_HELP_NOMODLISTING"), true, KernelColorType.Warning);
                foreach (var cmd in ModCommandList)
                {
                    if ((!cmd.Flags.HasFlag(CommandFlags.Strict) | cmd.Flags.HasFlag(CommandFlags.Strict) & UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator)) & (KernelEntry.Maintenance & !cmd.Flags.HasFlag(CommandFlags.NoMaintenance) | !KernelEntry.Maintenance))
                    {
                        string[] usages = [.. cmd.CommandArgumentInfo.Select((cai) => cai.RenderedUsage).Where((usage) => !string.IsNullOrEmpty(usage))];
                        TextWriters.Write("  - {0}{1}: ", false, KernelColorType.ListEntry, cmd.Command, usages.Length > 0 ? $" {string.Join(" | ", usages)}" : "");
                        TextWriters.Write("{0}", true, KernelColorType.ListValue, LanguageTools.GetLocalized(cmd.HelpDefinition));
                    }
                }
            }

            // The alias commands
            if (showAlias)
            {
                TextWriters.Write(CharManager.NewLine + LanguageTools.GetLocalized("NKS_SHELL_BASE_HELP_ALIASLISTING") + (Config.MainConfig.ShowCommandsCount & Config.MainConfig.ShowShellAliasesCount ? " [{0}]" : ""), true, KernelColorType.ListTitle, AliasedCommandList.Count);
                if (AliasedCommandList.Count == 0)
                    TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_SHELL_BASE_HELP_NOALIASLISTING"), true, KernelColorType.Warning);
                foreach (var cmd in AliasedCommandList)
                {
                    if ((!cmd.Value.Flags.HasFlag(CommandFlags.Strict) | cmd.Value.Flags.HasFlag(CommandFlags.Strict) & UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator)) & (KernelEntry.Maintenance & !cmd.Value.Flags.HasFlag(CommandFlags.NoMaintenance) | !KernelEntry.Maintenance))
                    {
                        string[] usages = [.. cmd.Value.CommandArgumentInfo.Select((cai) => cai.RenderedUsage).Where((usage) => !string.IsNullOrEmpty(usage))];
                        TextWriters.Write("  - {0} -> {1}{2}: ", false, KernelColorType.ListEntry, cmd.Key.Alias, cmd.Value.Command, usages.Length > 0 ? $" {string.Join(" | ", usages)}" : "");
                        TextWriters.Write("{0}", true, KernelColorType.ListValue, LanguageTools.GetLocalized(cmd.Value.HelpDefinition));
                    }
                }
            }

            // The unified commands
            if (showUnified)
            {
                TextWriters.Write(CharManager.NewLine + LanguageTools.GetLocalized("NKS_SHELL_BASE_HELP_UNIFIEDLISTING") + (Config.MainConfig.ShowCommandsCount & Config.MainConfig.ShowUnifiedCommandsCount ? " [{0}]" : ""), true, KernelColorType.ListTitle, unifiedCommandList.Count);
                if (unifiedCommandList.Count == 0)
                    TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_SHELL_BASE_HELP_NOUNIFIEDLISTING"), true, KernelColorType.Warning);
                foreach (var cmd in unifiedCommandList)
                {
                    if ((!cmd.Flags.HasFlag(CommandFlags.Strict) | cmd.Flags.HasFlag(CommandFlags.Strict) & UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator)) & (KernelEntry.Maintenance & !cmd.Flags.HasFlag(CommandFlags.NoMaintenance) | !KernelEntry.Maintenance))
                    {
                        string[] usages = [.. cmd.CommandArgumentInfo.Select((cai) => cai.RenderedUsage).Where((usage) => !string.IsNullOrEmpty(usage))];
                        TextWriters.Write("  - {0}{1}: ", false, KernelColorType.ListEntry, cmd.Command, usages.Length > 0 ? $" {string.Join(" | ", usages)}" : "");
                        TextWriters.Write("{0}", true, KernelColorType.ListValue, LanguageTools.GetLocalized(cmd.HelpDefinition));
                    }
                }
            }
        }

        internal static void ShowCommandListSimplified(string commandType)
        {
            // Get visible commands
            var commands = CommandManager.GetCommandNames(commandType);
            List<string> finalCommand = [];
            foreach (string cmd in commands)
            {
                // Get the necessary flags
                var command = CommandManager.GetCommand(cmd, commandType);
                bool hasAdmin = UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator);
                bool isStrict = command.Flags.HasFlag(CommandFlags.Strict);
                bool isNoMaintenance = command.Flags.HasFlag(CommandFlags.NoMaintenance);

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
            var shellInfo = ShellManager.GetShellInfo(commandType);
            var CommandList = shellInfo.Commands;

            // Add every command from each mod, addon, and alias
            var ModCommandList = shellInfo.ModCommands;
            var AddonCommandList = shellInfo.addonCommands;
            var unifiedCommandList = ShellManager.unifiedCommandDict;
            var AliasedCommandList = AliasManager.GetEntireAliasListFromType(commandType)
                .ToDictionary((ai) => ai, (ai) => ai.TargetCommand);

            // Check to see if command exists
            if (!string.IsNullOrWhiteSpace(command) &&
                (CommandList.Any((ci) => ci.Command == command) ||
                 AliasedCommandList.Any((info) => info.Key.Alias == command) ||
                 ModCommandList.Any((ci) => ci.Command == command) ||
                 AddonCommandList.Any((ci) => ci.Command == command) ||
                 unifiedCommandList.Any((ci) => ci.Command == command)))
            {
                // Found!
                bool IsMod = ModCommandList.Any((ci) => ci.Command == command);
                bool IsAlias = AliasedCommandList.Any((info) => info.Key.Alias == command);
                bool IsAddon = AddonCommandList.Any((ci) => ci.Command == command);
                bool IsUnified = unifiedCommandList.Any((ci) => ci.Command == command);
                var FinalCommandList =
                    IsMod ? ModCommandList.ToDictionary((info) => info.Command, (info) => info) :
                    IsAddon ? AddonCommandList.ToDictionary((info) => info.Command, (info) => info) :
                    IsAlias ? AliasedCommandList.ToDictionary((info) => info.Key.Command, (info) => info.Key.TargetCommand) :
                    IsUnified ? unifiedCommandList.ToDictionary((info) => info.Command, (info) => info) :
                    CommandList.ToDictionary((info) => info.Command, (info) => info);
                string FinalCommand =
                    IsMod || IsAddon || IsUnified ? command :
                    IsAlias ? AliasManager.GetAlias(command, commandType).Command :
                    command;
                string HelpDefinition = LanguageTools.GetLocalized(FinalCommandList[FinalCommand].HelpDefinition);

                // Write the description now
                if (string.IsNullOrEmpty(HelpDefinition))
                    HelpDefinition = LanguageTools.GetLocalized("NKS_SHELL_BASE_HELP_DEFINEDBY") + command;
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_BASE_HELP_COMMAND"), false, KernelColorType.ListEntry);
                TextWriters.Write($" {FinalCommand}", KernelColorType.ListValue);
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_BASE_HELP_DESCRIPTION"), false, KernelColorType.ListEntry);
                TextWriters.Write($" {HelpDefinition}", KernelColorType.ListValue);

                // Iterate through command argument information instances
                var argumentInfos = FinalCommandList[FinalCommand].CommandArgumentInfo ?? [];
                foreach (var argumentInfo in argumentInfos)
                {
                    var Arguments = Array.Empty<CommandArgumentPart>();
                    var Switches = Array.Empty<SwitchInfo>();
                    string renderedUsage = "";

                    // Populate help usages
                    if (argumentInfo is not null)
                    {
                        Arguments = argumentInfo.Arguments;
                        Switches = argumentInfo.Switches;
                        renderedUsage = argumentInfo.RenderedUsage;
                    }

                    // Print usage information
                    TextWriterRaw.Write();
                    TextWriters.Write($"{FinalCommand} {renderedUsage}", KernelColorType.ListEntry);

                    // If we have arguments, print their descriptions
                    if (Arguments.Length != 0)
                    {
                        TextWriters.Write("* " + LanguageTools.GetLocalized("NKS_SHELL_BASE_HELP_CMDARGLISTING"), KernelColorType.NeutralText);
                        foreach (var argument in Arguments)
                        {
                            string argumentDescUnlocalized = argument.Options.ArgumentDescription;
                            if (string.IsNullOrWhiteSpace(argument.Options.ArgumentDescription))
                                argumentDescUnlocalized = LanguageTools.GetLocalized("NKS_SHELL_BASE_HELP_CMDARGNODESC");
                            string argumentName = argument.ArgumentExpression;
                            string argumentDesc = LanguageTools.GetLocalized(argumentDescUnlocalized);
                            TextWriters.Write($"    {argumentName}: ", false, KernelColorType.ListEntry);
                            TextWriters.Write(argumentDesc, KernelColorType.ListValue);
                        }
                    }

                    // If we have switches, print their descriptions
                    if (Switches.Length != 0)
                    {
                        TextWriters.Write("* " + LanguageTools.GetLocalized("NKS_SHELL_BASE_HELP_SWITCHLISTING"), KernelColorType.NeutralText);
                        foreach (var Switch in Switches)
                        {
                            string switchDescUnlocalized = Switch.HelpDefinition;
                            if (string.IsNullOrWhiteSpace(Switch.HelpDefinition))
                                switchDescUnlocalized = LanguageTools.GetLocalized("NKS_SHELL_BASE_HELP_CMDSWITCHNODESC");
                            string switchName = Switch.SwitchName;
                            string switchDesc = LanguageTools.GetLocalized(switchDescUnlocalized);
                            TextWriters.Write($"    -{switchName}: ", false, KernelColorType.ListEntry);
                            TextWriters.Write(switchDesc, KernelColorType.ListValue);
                        }
                    }
                }

                // Extra help action for some commands
                FinalCommandList[FinalCommand].CommandBase?.HelpHelper();
            }
            else
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_BASE_HELP_NOHELP"), true, KernelColorType.Error, command);
        }
    }
}
