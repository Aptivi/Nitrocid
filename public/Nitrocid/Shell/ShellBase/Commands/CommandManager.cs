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

using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Splash;
using Nitrocid.Misc.Text.Probers.Regexp;
using Nitrocid.Modifications;
using Nitrocid.Shell.ShellBase.Aliases;
using Nitrocid.Shell.ShellBase.Shells;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        /// <param name="includeAliases">Whether to include aliases or not</param>
        /// <returns>True if found; False if not found or shell type is invalid.</returns>
        public static bool IsCommandFound(string Command, ShellType ShellType, bool includeAliases = true) =>
            IsCommandFound(Command, ShellManager.GetShellTypeName(ShellType), includeAliases);

        /// <summary>
        /// Checks to see if the command is found in selected shell command type
        /// </summary>
        /// <param name="Command">A command</param>
        /// <param name="ShellType">The shell type name</param>
        /// <param name="includeAliases">Whether to include aliases or not</param>
        /// <returns>True if found; False if not found or shell type is invalid.</returns>
        public static bool IsCommandFound(string Command, string ShellType, bool includeAliases = true)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Command: {0}, ShellType: {1}", Command, ShellType);
            return GetCommands(ShellType, includeAliases).ContainsKey(Command);
        }

        /// <summary>
        /// Checks to see if the command is found in all the shells
        /// </summary>
        /// <param name="Command">A command</param>
        /// <param name="includeAliases">Whether to include aliases or not</param>
        /// <returns>True if found; False if not found.</returns>
        public static bool IsCommandFound(string Command, bool includeAliases = true)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Command: {0}", Command);
            bool found = false;
            foreach (var ShellType in ShellManager.AvailableShells.Keys)
            {
                found = GetCommands(ShellType, includeAliases).ContainsKey(Command);
                if (found)
                    break;
            }
            return found;
        }

        /// <summary>
        /// Gets the command dictionary according to the shell type
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        /// <param name="includeAliases">Whether to include aliases or not</param>
        public static Dictionary<string, CommandInfo> GetCommands(ShellType ShellType, bool includeAliases = true) =>
            GetCommands(ShellManager.GetShellTypeName(ShellType), includeAliases);

        /// <summary>
        /// Gets the command dictionary according to the shell type
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        /// <param name="includeAliases">Whether to include aliases or not</param>
        public static Dictionary<string, CommandInfo> GetCommands(string ShellType, bool includeAliases = true)
        {
            // Individual shells
            var shellInfo = ShellManager.GetShellInfo(ShellType);
            var addonCommands = ShellManager.GetShellInfo(ShellType).addonCommands;
            var modCommands = ModManager.ListModCommands(ShellType);
            var aliasCommands = AliasManager.GetEntireAliasListFromType(ShellType);
            Dictionary<string, CommandInfo> FinalCommands = shellInfo.Commands;

            // Unified commands
            foreach (string UnifiedCommand in ShellManager.UnifiedCommands.Keys)
            {
                if (!FinalCommands.ContainsKey(UnifiedCommand))
                    FinalCommands.Add(UnifiedCommand, ShellManager.UnifiedCommands[UnifiedCommand]);
            }

            // Addon commands
            foreach (string AddonCommand in addonCommands.Keys)
            {
                if (!FinalCommands.ContainsKey(AddonCommand))
                    FinalCommands.Add(AddonCommand, addonCommands[AddonCommand]);
            }

            // Mod commands
            foreach (string ModCommand in modCommands.Keys)
            {
                if (!FinalCommands.ContainsKey(ModCommand))
                    FinalCommands.Add(ModCommand, modCommands[ModCommand]);
            }

            // Aliased commands
            foreach (var aliasInfo in aliasCommands)
            {
                if (!includeAliases)
                    break;
                string alias = aliasInfo.Alias;
                var resolved = aliasInfo.TargetCommand;
                FinalCommands.TryAdd(alias, resolved);
            }

            return FinalCommands;
        }

        /// <summary>
        /// Gets the command dictionary according to the shell type by searching for the partial command name
        /// </summary>
        /// <param name="namePattern">A valid regex pattern for command name</param>
        /// <param name="ShellType">The shell type</param>
        /// <param name="includeAliases">Whether to include aliases or not</param>
        public static Dictionary<string, CommandInfo> FindCommands([StringSyntax(StringSyntaxAttribute.Regex)] string namePattern, ShellType ShellType, bool includeAliases = true) =>
            FindCommands(namePattern, ShellManager.GetShellTypeName(ShellType), includeAliases);

        /// <summary>
        /// Gets the command dictionary according to the shell type by searching for the partial command name
        /// </summary>
        /// <param name="namePattern">A valid regex pattern for command name</param>
        /// <param name="ShellType">The shell type</param>
        /// <param name="includeAliases">Whether to include aliases or not</param>
        public static Dictionary<string, CommandInfo> FindCommands([StringSyntax(StringSyntaxAttribute.Regex)] string namePattern, string ShellType, bool includeAliases = true)
        {
            // Verify that the provided regex is valid
            if (!RegexpTools.IsValidRegex(namePattern))
                throw new KernelException(KernelExceptionType.CommandManager, Translate.DoTranslation("Invalid command pattern provided."));

            // Get all the commands first
            var allCommands = GetCommands(ShellType, includeAliases);

            // Now, find the commands that match the specified regex pattern.
            var foundCommands = allCommands
                .Where((kvp) => RegexpTools.IsMatch(kvp.Key, namePattern))
                .ToDictionary((kvp) => kvp.Key, (kvp) => kvp.Value);
            return foundCommands;
        }

        /// <summary>
        /// Gets a command, specified by the shell type
        /// </summary>
        /// <param name="Command">A command</param>
        /// <param name="ShellType">The shell type</param>
        /// <param name="includeAliases">Whether to include aliases or not</param>
        /// <returns>A <see cref="CommandInfo"/> instance of a specified command</returns>
        public static CommandInfo GetCommand(string Command, ShellType ShellType, bool includeAliases = true) =>
            GetCommand(Command, ShellManager.GetShellTypeName(ShellType), includeAliases);

        /// <summary>
        /// Gets a command, specified by the shell type
        /// </summary>
        /// <param name="Command">A command</param>
        /// <param name="ShellType">The shell type name</param>
        /// <param name="includeAliases">Whether to include aliases or not</param>
        /// <returns>True if found; False if not found or shell type is invalid.</returns>
        public static CommandInfo GetCommand(string Command, string ShellType, bool includeAliases = true)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Command: {0}, ShellType: {1}", Command, ShellType);
            var commandList = GetCommands(ShellType, includeAliases);
            if (!IsCommandFound(Command, ShellType, includeAliases))
                throw new KernelException(KernelExceptionType.CommandManager, Translate.DoTranslation("Command not found."));
            return commandList[Command];
        }

        /// <summary>
        /// Registers a command to the mod command list
        /// </summary>
        /// <param name="ShellType">Type of Nitrocid's built-in shell</param>
        /// <param name="commandBase">Custom command base to register</param>
        public static void RegisterCustomCommand(ShellType ShellType, CommandInfo commandBase) =>
            RegisterCustomCommand(ShellManager.GetShellTypeName(ShellType), commandBase);

        /// <summary>
        /// Registers a command to the mod command list
        /// </summary>
        /// <param name="ShellType">Type of a shell, including your custom type and other mod's custom type to extend it</param>
        /// <param name="commandBase">Custom command base to register</param>
        public static void RegisterCustomCommand(string ShellType, CommandInfo commandBase)
        {
            // First, check the values
            if (!ShellManager.ShellTypeExists(ShellType))
                throw new KernelException(KernelExceptionType.CommandManager, Translate.DoTranslation("Shell type {0} doesn't exist."), ShellType);
            if (commandBase is null)
                throw new KernelException(KernelExceptionType.CommandManager, Translate.DoTranslation("You must provide the command base."));
            string command = commandBase.Command;
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to register {0}, ShellType: {1}", command, ShellType);

            // Check the command name
            if (string.IsNullOrEmpty(command))
                throw new KernelException(KernelExceptionType.CommandManager, Translate.DoTranslation("You must provide the command."));

            // Check to see if the command conflicts with pre-existing shell commands
            if (IsCommandFound(command, ShellType))
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Command {0} conflicts with available shell commands or mod commands.", command);
                throw new KernelException(KernelExceptionType.CommandManager, Translate.DoTranslation("The command specified is already added! It's possible that you may have conflicting mods."));
            }

            // Check to see if the help definition is full
            if (string.IsNullOrEmpty(commandBase.HelpDefinition))
            {
                SplashReport.ReportProgress(Translate.DoTranslation("No definition for command {0}."), command);
                DebugWriter.WriteDebug(DebugLevel.W, "No definition, {0}.Def = \"Command not defined\"", command);
                commandBase.HelpDefinition = Translate.DoTranslation("Command not defined");
            }

            // Now, add the command to the mod list
            DebugWriter.WriteDebug(DebugLevel.I, "Adding command {0} for {1}...", command, ShellType);
            if (!ModManager.ListModCommands(ShellType).ContainsKey(command))
                ModManager.ListModCommands(ShellType).Add(command, commandBase);
            DebugWriter.WriteDebug(DebugLevel.I, "Registered {0}, ShellType: {1}", command, ShellType);
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
                throw new KernelException(KernelExceptionType.CommandManager, Translate.DoTranslation("Some of the custom commands can't be loaded.") + CharManager.NewLine + string.Join(CharManager.NewLine, failedCommands));
        }

        /// <summary>
        /// Unregisters a custom command
        /// </summary>
        /// <param name="ShellType">Type of Nitrocid's built-in shell</param>
        /// <param name="commandName">Custom command name to unregister</param>
        public static void UnregisterCustomCommand(ShellType ShellType, string commandName) =>
            UnregisterCustomCommand(ShellManager.GetShellTypeName(ShellType), commandName);

        /// <summary>
        /// Unregisters a custom command
        /// </summary>
        /// <param name="ShellType">Type of a shell, including your custom type and other mod's custom type to extend it</param>
        /// <param name="commandName">Custom command name to unregister</param>
        public static void UnregisterCustomCommand(string ShellType, string commandName)
        {
            // First, check the values
            if (!ShellManager.ShellTypeExists(ShellType))
                throw new KernelException(KernelExceptionType.CommandManager, Translate.DoTranslation("Shell type {0} doesn't exist."), ShellType);
            if (string.IsNullOrEmpty(commandName))
                throw new KernelException(KernelExceptionType.CommandManager, Translate.DoTranslation("You must provide the command."));

            // Check to see if we have this command
            if (!ModManager.ListModCommands(ShellType).ContainsKey(commandName))
                throw new KernelException(KernelExceptionType.CommandManager, Translate.DoTranslation("The custom command specified is not found."));
            else
                ModManager.ListModCommands(ShellType).Remove(commandName);
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
                throw new KernelException(KernelExceptionType.CommandManager, Translate.DoTranslation("Some of the custom commands can't be unloaded.") + CharManager.NewLine + string.Join(CharManager.NewLine, failedCommands));
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
                throw new KernelException(KernelExceptionType.CommandManager, Translate.DoTranslation("Shell type {0} doesn't exist."), ShellType);
            if (commandBase is null)
                throw new KernelException(KernelExceptionType.CommandManager, Translate.DoTranslation("You must provide the command base."));
            string command = commandBase.Command;
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to register {0}, ShellType: {1}", command, ShellType);

            // Check the command name
            if (string.IsNullOrEmpty(command))
                throw new KernelException(KernelExceptionType.CommandManager, Translate.DoTranslation("You must provide the command."));

            // Check to see if the command conflicts with pre-existing shell commands
            if (IsCommandFound(command, ShellType))
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Command {0} conflicts with available shell commands or addon commands.", command);
                throw new KernelException(KernelExceptionType.CommandManager, Translate.DoTranslation("The command specified is already added! It's possible that you may have conflicting addons."));
            }

            // Check to see if the help definition is full
            if (string.IsNullOrEmpty(commandBase.HelpDefinition))
            {
                SplashReport.ReportProgress(Translate.DoTranslation("No definition for command {0}."), command);
                DebugWriter.WriteDebug(DebugLevel.W, "No definition, {0}.Def = \"Command not defined\"", command);
                commandBase.HelpDefinition = Translate.DoTranslation("Command not defined");
            }

            // Now, add the command to the addon list
            DebugWriter.WriteDebug(DebugLevel.I, "Adding command {0} for {1}...", command, ShellType);
            ShellManager.AvailableShells[ShellType].addonCommands.TryAdd(command, commandBase);
            DebugWriter.WriteDebug(DebugLevel.I, "Registered {0}, ShellType: {1}", command, ShellType);
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
                throw new KernelException(KernelExceptionType.CommandManager, Translate.DoTranslation("Some of the addon commands can't be loaded.") + CharManager.NewLine + string.Join(CharManager.NewLine, failedCommands));
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
                throw new KernelException(KernelExceptionType.CommandManager, Translate.DoTranslation("Shell type {0} doesn't exist."), ShellType);
            if (string.IsNullOrEmpty(commandName))
                throw new KernelException(KernelExceptionType.CommandManager, Translate.DoTranslation("You must provide the command."));

            // Check to see if we have this command
            if (!ShellManager.AvailableShells[ShellType].addonCommands.ContainsKey(commandName))
                throw new KernelException(KernelExceptionType.CommandManager, Translate.DoTranslation("The addon command specified is not found."));
            else
                ShellManager.availableShells[ShellType].addonCommands.Remove(commandName);
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
                throw new KernelException(KernelExceptionType.CommandManager, Translate.DoTranslation("Some of the addon commands can't be unloaded.") + CharManager.NewLine + string.Join(CharManager.NewLine, failedCommands));
        }
    }
}
