
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

using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Splash;
using KS.Misc.Text;
using KS.Modifications;
using KS.Shell.ShellBase.Shells;
using System;
using System.Collections.Generic;

namespace KS.Shell.ShellBase.Commands
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
            DebugWriter.WriteDebug(DebugLevel.I, "Command: {0}, ShellType: {1}", Command, ShellType);
            return GetCommands(ShellType).ContainsKey(Command);
        }

        /// <summary>
        /// Checks to see if the command is found in all the shells
        /// </summary>
        /// <param name="Command">A command</param>
        /// <returns>True if found; False if not found.</returns>
        public static bool IsCommandFound(string Command)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Command: {0}", Command);
            bool found = false;
            foreach (var ShellType in ShellManager.AvailableShells.Keys)
            {
                found = GetCommands(ShellType).ContainsKey(Command);
                if (found)
                    break;
            }
            return found;
        }

        /// <summary>
        /// Gets the command dictionary according to the shell type
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static Dictionary<string, CommandInfo> GetCommands(ShellType ShellType) =>
            GetCommands(ShellManager.GetShellTypeName(ShellType));

        /// <summary>
        /// Gets the command dictionary according to the shell type
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static Dictionary<string, CommandInfo> GetCommands(string ShellType)
        {
            // Individual shells
            var shellInfo = ShellManager.GetShellInfo(ShellType);
            var modCommands = ModManager.ListModCommands(ShellType);
            Dictionary<string, CommandInfo> FinalCommands = shellInfo.Commands;

            // Unified commands
            foreach (string UnifiedCommand in ShellManager.UnifiedCommandDict.Keys)
            {
                if (!FinalCommands.ContainsKey(UnifiedCommand))
                    FinalCommands.Add(UnifiedCommand, ShellManager.UnifiedCommandDict[UnifiedCommand]);
            }

            // Mod commands
            foreach (string ModCommand in modCommands.Keys)
            {
                if (!FinalCommands.ContainsKey(ModCommand))
                    FinalCommands.Add(ModCommand, modCommands[ModCommand]);
            }

            // Aliased commands
            foreach (string Alias in shellInfo.Aliases.Keys)
            {
                string resolved = shellInfo.Aliases[Alias];
                if (!FinalCommands.ContainsKey(Alias))
                    FinalCommands.Add(Alias, FinalCommands[resolved]);
            }

            return FinalCommands;
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
            DebugWriter.WriteDebug(DebugLevel.I, "Command: {0}, ShellType: {1}", Command, ShellType);
            var commandList = GetCommands(ShellType);
            if (!IsCommandFound(Command, ShellType))
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
            if (!ShellTypeManager.ShellTypeExists(ShellType))
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
                SplashReport.ReportProgress(Translate.DoTranslation("No definition for command {0}."), 0, command);
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
            List<string> failedCommands = new();
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
                    failedCommands.Add($"  - {(commandBase is not null ? commandBase.Command : "???")}: {ex.Message}");
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
            if (!ShellTypeManager.ShellTypeExists(ShellType))
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
            List<string> failedCommands = new();
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
                    failedCommands.Add($"  - {(!string.IsNullOrEmpty(commandBase) ? commandBase : "???")}: {ex.Message}");
                }
            }
            if (failedCommands.Count > 0)
                throw new KernelException(KernelExceptionType.CommandManager, Translate.DoTranslation("Some of the custom commands can't be unloaded.") + CharManager.NewLine + string.Join(CharManager.NewLine, failedCommands));
        }
    }
}
