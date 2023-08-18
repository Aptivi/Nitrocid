
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
using KS.Shell.ShellBase.Shells;
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
            if (ShellManager.UnifiedCommandDict.ContainsKey(Command))
                return true;
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
            return ShellManager.UnifiedCommandDict.ContainsKey(Command)
                 | GetCommands(ShellType.FTPShell).ContainsKey(Command)
                 | GetCommands(ShellType.JsonShell).ContainsKey(Command)
                 | GetCommands(ShellType.MailShell).ContainsKey(Command)
                 | GetCommands(ShellType.RSSShell).ContainsKey(Command)
                 | GetCommands(ShellType.SFTPShell).ContainsKey(Command)
                 | GetCommands(ShellType.Shell).ContainsKey(Command)
                 | GetCommands(ShellType.TextShell).ContainsKey(Command)
                 | GetCommands(ShellType.HTTPShell).ContainsKey(Command)
                 | GetCommands(ShellType.HexShell).ContainsKey(Command)
                 | GetCommands(ShellType.ArchiveShell).ContainsKey(Command)
                 | GetCommands(ShellType.SqlShell).ContainsKey(Command);
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
            Dictionary<string, CommandInfo> FinalCommands = ShellManager.GetShellInfo(ShellType).Commands;

            // Unified commands
            foreach (string UnifiedCommand in ShellManager.UnifiedCommandDict.Keys)
            {
                FinalCommands.Remove(UnifiedCommand);
                FinalCommands.Add(UnifiedCommand, ShellManager.UnifiedCommandDict[UnifiedCommand]);
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
            if (ShellManager.UnifiedCommandDict.ContainsKey(Command))
                return ShellManager.UnifiedCommandDict[Command];
            if (!IsCommandFound(Command, ShellType))
                throw new KernelException(KernelExceptionType.CommandManager, Translate.DoTranslation("Command not found."));
            return commandList[Command];
        }
    }
}
