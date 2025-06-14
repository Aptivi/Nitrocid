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

extern alias TextifyDep;

using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Splash;
using Nitrocid.Misc.Text.Probers.Regexp;
using Nitrocid.Shell.ShellBase.Shells;
using System;
using System.Collections.Generic;
using TextifyDep::System.Diagnostics.CodeAnalysis;
using System.Linq;
using Textify.General;

namespace Nitrocid.Shell.ShellBase.Commands
{
    /// <summary>
    /// Command management class
    /// </summary>
    public static class CommandManager
    {
        /// <summary>
        /// Checks to see if the command is found in selected shell command type
        /// </summary>
        /// <param name="Command">A command</param>
        /// <param name="ShellType">The shell type</param>
        /// <returns>True if found; False if not found or shell type is invalid.</returns>
        public static bool IsCommandFound(string Command, ShellType ShellType) =>
            IsCommandFound(Command, ShellManager.GetShellTypeName(ShellType));

        /// <summary>
        /// Checks to see if the command is found in selected shell command type
        /// </summary>
        /// <param name="Command">A command</param>
        /// <param name="ShellType">The shell type name</param>
        /// <returns>True if found; False if not found or shell type is invalid.</returns>
        public static bool IsCommandFound(string Command, string ShellType)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Command: {0}, ShellType: {1}", vars: [Command, ShellType]);
            return GetCommands(ShellType).Any((ci) => ci.Command == Command || ci.Aliases.Any((ai) => ai.Alias == Command));
        }

        /// <summary>
        /// Checks to see if the command is found in all the shells
        /// </summary>
        /// <param name="Command">A command</param>
        /// <returns>True if found; False if not found.</returns>
        public static bool IsCommandFound(string Command)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Command: {0}", vars: [Command]);
            bool found = false;
            foreach (var ShellType in ShellManager.AvailableShells.Keys)
            {
                found = GetCommands(ShellType).Any((ci) => ci.Command == Command || ci.Aliases.Any((ai) => ai.Alias == Command));
                if (found)
                    break;
            }
            return found;
        }

        /// <summary>
        /// Gets the command list according to the shell type
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static CommandInfo[] GetCommands(ShellType ShellType) =>
            GetCommands(ShellManager.GetShellTypeName(ShellType));

        /// <summary>
        /// Gets the command list according to the shell type
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static CommandInfo[] GetCommands(string ShellType)
        {
            // Individual shells
            var shellInfo = ShellManager.GetShellInfo(ShellType);
            var addonCommands = shellInfo.addonCommands;
            var modCommands = shellInfo.ModCommands;
            List<CommandInfo> FinalCommands = shellInfo.Commands;

            // Unified commands
            foreach (var UnifiedCommand in ShellManager.UnifiedCommands)
            {
                if (!FinalCommands.Contains(UnifiedCommand))
                    FinalCommands.Add(UnifiedCommand);
            }

            // Addon commands
            foreach (var AddonCommand in addonCommands)
            {
                if (!FinalCommands.Contains(AddonCommand))
                    FinalCommands.Add(AddonCommand);
            }

            // Mod commands
            foreach (var ModCommand in modCommands)
            {
                if (!FinalCommands.Contains(ModCommand))
                    FinalCommands.Add(ModCommand);
            }

            return [.. FinalCommands];
        }

        /// <summary>
        /// Gets the command names according to the shell type
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static string[] GetCommandNames(ShellType ShellType) =>
            GetCommandNames(ShellManager.GetShellTypeName(ShellType));

        /// <summary>
        /// Gets the command names according to the shell type
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static string[] GetCommandNames(string ShellType)
        {
            // Return command names
            string[] FinalCommands = GetCommands(ShellType).Select((ci) => ci.Command).ToArray();
            return FinalCommands;
        }

        /// <summary>
        /// Gets the command list according to the shell type by searching for the partial command name
        /// </summary>
        /// <param name="namePattern">A valid regex pattern for command name</param>
        /// <param name="ShellType">The shell type</param>
        public static CommandInfo[] FindCommands([StringSyntax(StringSyntaxAttribute.Regex)] string namePattern, ShellType ShellType) =>
            FindCommands(namePattern, ShellManager.GetShellTypeName(ShellType));

        /// <summary>
        /// Gets the command list according to the shell type by searching for the partial command name
        /// </summary>
        /// <param name="namePattern">A valid regex pattern for command name</param>
        /// <param name="ShellType">The shell type</param>
        public static CommandInfo[] FindCommands([StringSyntax(StringSyntaxAttribute.Regex)] string namePattern, string ShellType)
        {
            // Verify that the provided regex is valid
            if (!RegexpTools.IsValidRegex(namePattern))
                throw new KernelException(KernelExceptionType.CommandManager, LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_EXCEPTION_CMDPATTERNINVALID"));

            // Get all the commands first
            var allCommands = GetCommands(ShellType);

            // Now, find the commands that match the specified regex pattern.
            var foundCommands = allCommands
                .Where((info) => RegexpTools.IsMatch(info.Command, namePattern))
                .ToArray();
            return foundCommands;
        }

        /// <summary>
        /// Gets a command, specified by the shell type
        /// </summary>
        /// <param name="Command">A command</param>
        /// <param name="ShellType">The shell type</param>
        /// <returns>A <see cref="CommandInfo"/> instance of a specified command</returns>
        public static CommandInfo GetCommand(string Command, ShellType ShellType) =>
            GetCommand(Command, ShellManager.GetShellTypeName(ShellType));

        /// <summary>
        /// Gets a command, specified by the shell type
        /// </summary>
        /// <param name="Command">A command</param>
        /// <param name="ShellType">The shell type name</param>
        /// <returns>True if found; False if not found or shell type is invalid.</returns>
        public static CommandInfo GetCommand(string Command, string ShellType)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Command: {0}, ShellType: {1}", vars: [Command, ShellType]);
            var commandList = GetCommands(ShellType);
            if (!IsCommandFound(Command, ShellType))
                throw new KernelException(KernelExceptionType.CommandManager, LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_EXCEPTION_NOTFOUND"));
            return commandList.Single((ci) => ci.Command == Command || ci.Aliases.Any((ai) => ai.Alias == Command));
        }

        /// <summary>
        /// Registers a command to the mod command list
        /// </summary>
        /// <param name="ShellType">Type of Nitrocid's built-in shell</param>
        /// <param name="commandBase">Custom command base to register</param>
        public static void RegisterCustomCommand(ShellType ShellType, CommandInfo? commandBase) =>
            RegisterCustomCommand(ShellManager.GetShellTypeName(ShellType), commandBase);

        /// <summary>
        /// Registers a command to the mod command list
        /// </summary>
        /// <param name="ShellType">Type of a shell, including your custom type and other mod's custom type to extend it</param>
        /// <param name="commandBase">Custom command base to register</param>
        public static void RegisterCustomCommand(string ShellType, CommandInfo? commandBase)
        {
            // First, check the values
            if (!ShellManager.ShellTypeExists(ShellType))
                throw new KernelException(KernelExceptionType.CommandManager, LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_EXCEPTION_TYPENOTFOUND"), ShellType);
            if (commandBase is null)
                throw new KernelException(KernelExceptionType.CommandManager, LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_EXCEPTION_BASENEEDED"));
            string command = commandBase.Command;
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to register {0}, ShellType: {1}", vars: [command, ShellType]);

            // Check the command name
            if (string.IsNullOrEmpty(command))
                throw new KernelException(KernelExceptionType.CommandManager, LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_EXCEPTION_CMDNEEDED"));

            // Check to see if the command conflicts with pre-existing shell commands
            if (IsCommandFound(command, ShellType))
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Command {0} conflicts with available shell commands or mod commands.", vars: [command]);
                throw new KernelException(KernelExceptionType.CommandManager, LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_EXCEPTION_CMDALREADYEXISTS"));
            }

            // Check to see if the help definition is full
            if (string.IsNullOrEmpty(commandBase.HelpDefinition))
            {
                SplashReport.ReportProgress(LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_NODEFINITION"), command);
                DebugWriter.WriteDebug(DebugLevel.W, "No definition, {0}.Def = \"Command not defined\"", vars: [command]);
                commandBase.HelpDefinition = LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_UNDEFINED");
            }

            // Now, add the command to the mod list
            DebugWriter.WriteDebug(DebugLevel.I, "Adding command {0} for {1}...", vars: [command, ShellType]);
            var shellInfo = ShellManager.GetShellInfo(ShellType);
            if (!shellInfo.ModCommands.Contains(commandBase))
                shellInfo.ModCommands.Add(commandBase);
            DebugWriter.WriteDebug(DebugLevel.I, "Registered {0}, ShellType: {1}", vars: [command, ShellType]);
        }

        /// <summary>
        /// Registers a group of custom commands
        /// </summary>
        /// <param name="ShellType">Type of Nitrocid's built-in shell</param>
        /// <param name="commandBases">Custom command bases to register</param>
        public static void RegisterCustomCommands(ShellType ShellType, CommandInfo[] commandBases) =>
            RegisterCustomCommands(ShellManager.GetShellTypeName(ShellType), commandBases);

        /// <summary>
        /// Registers a group of custom commands
        /// </summary>
        /// <param name="ShellType">Type of a shell, including your custom type and other mod's custom type to extend it</param>
        /// <param name="commandBases">Custom command bases to register</param>
        public static void RegisterCustomCommands(string ShellType, CommandInfo[] commandBases)
        {
            List<string> failedCommands = [];
            foreach (var commandBase in commandBases)
            {
                try
                {
                    RegisterCustomCommand(ShellType, commandBase);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, $"Can't register custom command: {ex.Message}");
                    DebugWriter.WriteDebugStackTrace(ex);
                    failedCommands.Add($"  - {(commandBase is not null ? commandBase.Command : "???")}: {(ex is KernelException kex ? kex.OriginalExceptionMessage : ex.Message)}");
                }
            }
            if (failedCommands.Count > 0)
                throw new KernelException(KernelExceptionType.CommandManager, LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_EXCEPTION_CANTLOAD_CUSTOM") + CharManager.NewLine + string.Join(CharManager.NewLine, failedCommands));
        }

        /// <summary>
        /// Unregisters a custom command
        /// </summary>
        /// <param name="ShellType">Type of Nitrocid's built-in shell</param>
        /// <param name="commandName">Custom command name to unregister</param>
        public static void UnregisterCustomCommand(ShellType ShellType, string? commandName) =>
            UnregisterCustomCommand(ShellManager.GetShellTypeName(ShellType), commandName);

        /// <summary>
        /// Unregisters a custom command
        /// </summary>
        /// <param name="ShellType">Type of a shell, including your custom type and other mod's custom type to extend it</param>
        /// <param name="commandName">Custom command name to unregister</param>
        public static void UnregisterCustomCommand(string ShellType, string? commandName)
        {
            // First, check the values
            if (!ShellManager.ShellTypeExists(ShellType))
                throw new KernelException(KernelExceptionType.CommandManager, LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_EXCEPTION_TYPENOTFOUND"), ShellType);
            if (string.IsNullOrEmpty(commandName))
                throw new KernelException(KernelExceptionType.CommandManager, LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_EXCEPTION_CMDNEEDED"));

            // Check to see if we have this command
            if (!GetCommandNames(ShellType).Contains(commandName))
                throw new KernelException(KernelExceptionType.CommandManager, LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_EXCEPTION_NOTFOUND_CUSTOM"));
            else
            {
                // We have the command. Remove it.
                var cmd = GetCommand(commandName, ShellType);
                ShellManager.GetShellInfo(ShellType).ModCommands.Remove(cmd);
            }
        }

        /// <summary>
        /// Unregisters a group of custom commands
        /// </summary>
        /// <param name="ShellType">Type of Nitrocid's built-in shell</param>
        /// <param name="commandNames">Custom command names to unregister</param>
        public static void UnregisterCustomCommands(ShellType ShellType, string[] commandNames) =>
            UnregisterCustomCommands(ShellManager.GetShellTypeName(ShellType), commandNames);

        /// <summary>
        /// Unregisters a group of custom commands
        /// </summary>
        /// <param name="ShellType">Type of a shell, including your custom type and other mod's custom type to extend it</param>
        /// <param name="commandNames">Custom command names to unregister</param>
        public static void UnregisterCustomCommands(string ShellType, string[] commandNames)
        {
            List<string> failedCommands = [];
            foreach (string commandBase in commandNames)
            {
                try
                {
                    UnregisterCustomCommand(ShellType, commandBase);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, $"Can't unregister custom command: {ex.Message}");
                    DebugWriter.WriteDebugStackTrace(ex);
                    failedCommands.Add($"  - {(!string.IsNullOrEmpty(commandBase) ? commandBase : "???")}: {(ex is KernelException kex ? kex.OriginalExceptionMessage : ex.Message)}");
                }
            }
            if (failedCommands.Count > 0)
                throw new KernelException(KernelExceptionType.CommandManager, LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_EXCEPTION_CANTUNLOAD_CUSTOM") + CharManager.NewLine + string.Join(CharManager.NewLine, failedCommands));
        }

        /// <summary>
        /// Registers a command to the addon command list
        /// </summary>
        /// <param name="ShellType">Type of Nitrocid's built-in shell</param>
        /// <param name="commandBase">Custom command base to register</param>
        internal static void RegisterAddonCommand(ShellType ShellType, CommandInfo commandBase) =>
            RegisterAddonCommand(ShellManager.GetShellTypeName(ShellType), commandBase);

        /// <summary>
        /// Registers a command to the addon command list
        /// </summary>
        /// <param name="ShellType">Type of a shell, including your custom type and other addon's custom type to extend it</param>
        /// <param name="commandBase">Custom command base to register</param>
        internal static void RegisterAddonCommand(string ShellType, CommandInfo commandBase)
        {
            // First, check the values
            if (!ShellManager.ShellTypeExists(ShellType))
                throw new KernelException(KernelExceptionType.CommandManager, LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_EXCEPTION_TYPENOTFOUND"), ShellType);
            if (commandBase is null)
                throw new KernelException(KernelExceptionType.CommandManager, LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_EXCEPTION_BASENEEDED"));
            string command = commandBase.Command;
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to register {0}, ShellType: {1}", vars: [command, ShellType]);

            // Check the command name
            if (string.IsNullOrEmpty(command))
                throw new KernelException(KernelExceptionType.CommandManager, LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_EXCEPTION_CMDNEEDED"));

            // Check to see if the command conflicts with pre-existing shell commands
            if (IsCommandFound(command, ShellType))
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Command {0} conflicts with available shell commands or addon commands.", vars: [command]);
                throw new KernelException(KernelExceptionType.CommandManager, LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_EXCEPTION_CMDALREADYEXISTS_CUSTOM"));
            }

            // Check to see if the help definition is full
            if (string.IsNullOrEmpty(commandBase.HelpDefinition))
            {
                SplashReport.ReportProgress(LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_NODEFINITION"), command);
                DebugWriter.WriteDebug(DebugLevel.W, "No definition, {0}.Def = \"Command not defined\"", vars: [command]);
                commandBase.HelpDefinition = LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_UNDEFINED");
            }

            // Now, add the command to the addon list
            DebugWriter.WriteDebug(DebugLevel.I, "Adding command {0} for {1}...", vars: [command, ShellType]);
            if (!ShellManager.AvailableShells[ShellType].addonCommands.Contains(commandBase))
                ShellManager.AvailableShells[ShellType].addonCommands.Add(commandBase);
            DebugWriter.WriteDebug(DebugLevel.I, "Registered {0}, ShellType: {1}", vars: [command, ShellType]);
        }

        /// <summary>
        /// Registers a group of addon commands
        /// </summary>
        /// <param name="ShellType">Type of Nitrocid's built-in shell</param>
        /// <param name="commandBases">Addon command bases to register</param>
        internal static void RegisterAddonCommands(ShellType ShellType, CommandInfo[] commandBases) =>
            RegisterAddonCommands(ShellManager.GetShellTypeName(ShellType), commandBases);

        /// <summary>
        /// Registers a group of addon commands
        /// </summary>
        /// <param name="ShellType">Type of a shell, including your custom type and other addon's custom type to extend it</param>
        /// <param name="commandBases">Addon command bases to register</param>
        internal static void RegisterAddonCommands(string ShellType, CommandInfo[] commandBases)
        {
            List<string> failedCommands = [];
            foreach (var commandBase in commandBases)
            {
                try
                {
                    RegisterAddonCommand(ShellType, commandBase);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, $"Can't register addon command: {ex.Message}");
                    DebugWriter.WriteDebugStackTrace(ex);
                    failedCommands.Add($"  - {(commandBase is not null ? commandBase.Command : "???")}: {(ex is KernelException kex ? kex.OriginalExceptionMessage : ex.Message)}");
                }
            }
            if (failedCommands.Count > 0)
                throw new KernelException(KernelExceptionType.CommandManager, LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_EXCEPTION_CANTLOAD_ADDON") + CharManager.NewLine + string.Join(CharManager.NewLine, failedCommands));
        }

        /// <summary>
        /// Unregisters a addon command
        /// </summary>
        /// <param name="ShellType">Type of Nitrocid's built-in shell</param>
        /// <param name="commandName">Addon command name to unregister</param>
        internal static void UnregisterAddonCommand(ShellType ShellType, string commandName) =>
            UnregisterAddonCommand(ShellManager.GetShellTypeName(ShellType), commandName);

        /// <summary>
        /// Unregisters a addon command
        /// </summary>
        /// <param name="ShellType">Type of a shell, including your custom type and other addon's custom type to extend it</param>
        /// <param name="commandName">Addon command name to unregister</param>
        internal static void UnregisterAddonCommand(string ShellType, string commandName)
        {
            // First, check the values
            if (!ShellManager.ShellTypeExists(ShellType))
                throw new KernelException(KernelExceptionType.CommandManager, LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_EXCEPTION_TYPENOTFOUND"), ShellType);
            if (string.IsNullOrEmpty(commandName))
                throw new KernelException(KernelExceptionType.CommandManager, LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_EXCEPTION_CMDNEEDED"));

            // Check to see if we have this command
            if (!GetCommandNames(ShellType).Contains(commandName))
                throw new KernelException(KernelExceptionType.CommandManager, LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_EXCEPTION_NOTFOUND_ADDON"));
            else
            {
                // We have the command. Remove it.
                var cmd = GetCommand(commandName, ShellType);
                ShellManager.availableShells[ShellType].addonCommands.Remove(cmd);
            }
        }

        /// <summary>
        /// Unregisters a group of addon commands
        /// </summary>
        /// <param name="ShellType">Type of Nitrocid's built-in shell</param>
        /// <param name="commandNames">Addon command names to unregister</param>
        internal static void UnregisterAddonCommands(ShellType ShellType, string[] commandNames) =>
            UnregisterAddonCommands(ShellManager.GetShellTypeName(ShellType), commandNames);

        /// <summary>
        /// Unregisters a group of addon commands
        /// </summary>
        /// <param name="ShellType">Type of a shell, including your custom type and other addon's custom type to extend it</param>
        /// <param name="commandNames">Addon command names to unregister</param>
        internal static void UnregisterAddonCommands(string ShellType, string[] commandNames)
        {
            List<string> failedCommands = [];
            foreach (string commandBase in commandNames)
            {
                try
                {
                    UnregisterAddonCommand(ShellType, commandBase);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, $"Can't unregister addon command: {ex.Message}");
                    DebugWriter.WriteDebugStackTrace(ex);
                    failedCommands.Add($"  - {(!string.IsNullOrEmpty(commandBase) ? commandBase : "???")}: {(ex is KernelException kex ? kex.OriginalExceptionMessage : ex.Message)}");
                }
            }
            if (failedCommands.Count > 0)
                throw new KernelException(KernelExceptionType.CommandManager, LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_EXCEPTION_CANTUNLOAD_ADDON") + CharManager.NewLine + string.Join(CharManager.NewLine, failedCommands));
        }
    }
}
