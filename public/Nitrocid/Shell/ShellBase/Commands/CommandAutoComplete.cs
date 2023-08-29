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

using KS.Files;
using KS.Files.Folders;
using KS.Files.Querying;
using KS.Kernel.Debugging;
using KS.Misc.Text;
using KS.Shell.ShellBase.Commands.ArgumentsParsers;
using KS.Shell.ShellBase.Shells;
using System;
using System.IO;
using System.Linq;

namespace KS.Shell.ShellBase.Commands
{
    /// <summary>
    /// Command auto completion class
    /// </summary>
    internal static class CommandAutoComplete
    {

        /// <inheritdoc/>
        public static string[] GetSuggestions(string text, int index, char[] delims)
        {
            // First, cut the text to index
            text = text[..index];
            DebugWriter.WriteDebug(DebugLevel.I, "Text to auto complete: {0} (idx: {1})", text, index);

            // Then, check to see is we have shells
            DebugWriter.WriteDebug(DebugLevel.I, "Shell count: {0}", ShellStart.ShellStack.Count);
            if (ShellStart.ShellStack.Count <= 0)
                return Array.Empty<string>();

            // Get the commands based on the current shell type
            var shellType = ShellStart.ShellStack[^1].ShellType;
            var ShellCommands = CommandManager.GetCommands(shellType);
            DebugWriter.WriteDebug(DebugLevel.I, "Commands count for type {0}: {1}", shellType, ShellCommands.Count);

            // If text is not provided, return the command list without filtering
            if (string.IsNullOrEmpty(text))
                return ShellCommands.Keys.ToArray();

            // Get the provided command and argument information
            var commandArgumentInfo = ArgumentsParser.ParseShellCommandArguments(text, shellType);

            // We're providing completion for argument.
            string CommandName = commandArgumentInfo.Command;
            string finalCommandArgs = commandArgumentInfo.ArgumentsText;
            string[] finalCommandArgsEnclosed = finalCommandArgs.SplitEncloseDoubleQuotes();
            string LastArgument = finalCommandArgsEnclosed.Length > 0 ? finalCommandArgsEnclosed[^1] : "";
            DebugWriter.WriteDebug(DebugLevel.I, "Command name: {0}", CommandName);
            DebugWriter.WriteDebug(DebugLevel.I, "Command arguments [{0}]: {1}", finalCommandArgsEnclosed.Length, finalCommandArgs);
            DebugWriter.WriteDebug(DebugLevel.I, "last argument: {0}", LastArgument);

            // Make a file and folder list
            string[] finalCompletions;
            if (!string.IsNullOrEmpty(finalCommandArgs))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Creating list of files and directories starting with argument {0} [{1}]...", LastArgument, LastArgument.Length);
                string lookupPath = Path.IsPathRooted(LastArgument) ? Path.GetDirectoryName(LastArgument) : Filesystem.NeutralizePath(LastArgument, CurrentDirectory.CurrentDir);
                lookupPath = Checking.FolderExists(lookupPath) ? lookupPath : Path.GetDirectoryName(CurrentDirectory.CurrentDir + "/" + LastArgument);
                finalCompletions = Listing.CreateList(lookupPath, true)
                    .Select(x => Path.IsPathRooted(LastArgument) ? Filesystem.NeutralizePath(x.FilePath) : Filesystem.NeutralizePath(x.FilePath).Replace(CurrentDirectory.CurrentDir + "/", ""))
                    .Where(x => x.StartsWith(LastArgument))
                    .Select(x => x[LastArgument.Length..])
                    .ToArray();
                DebugWriter.WriteDebug(DebugLevel.I, "Initially invoked, and got {0} autocompletion suggestions. [{1}]", finalCompletions.Length, string.Join(", ", finalCompletions));
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Creating list of commands starting with command {0} [{1}]...", CommandName, CommandName.Length);
                finalCompletions = ShellCommands.Keys
                    .Where(x => x.StartsWith(CommandName))
                    .Select(x => x[CommandName.Length..])
                    .ToArray();
                DebugWriter.WriteDebug(DebugLevel.I, "Initially invoked, and got {0} autocompletion suggestions. [{1}]", finalCompletions.Length, string.Join(", ", finalCompletions));
            }

            // Check to see if there is such command
            DebugWriter.WriteDebug(DebugLevel.I, "Command {0} exists? {1}", CommandName, ShellCommands.ContainsKey(CommandName));
            if (!ShellCommands.ContainsKey(CommandName))
                return finalCompletions;

            // We have the command. Check its entry for argument info
            var CommandArgumentInfos = ShellCommands[CommandName].CommandArgumentInfo;
            foreach (var CommandArgumentInfo in CommandArgumentInfos)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Command {0} has argument info? {1}", CommandName, CommandArgumentInfo is not null);
                if (CommandArgumentInfo is null)
                    // No arguments. Return file list
                    return finalCompletions;

                // There are arguments! Now, check to see if it has the accessible auto completer
                var AutoCompleter = CommandArgumentInfo.AutoCompleter;
                DebugWriter.WriteDebug(DebugLevel.I, "Command {0} has auto complete info? {1}", CommandName, AutoCompleter is not null);
                if (AutoCompleter is null)
                    // No delegate. Return file list
                    return finalCompletions;

                // We have the delegate! Invoke it.
                DebugWriter.WriteDebug(DebugLevel.I, "If we reach here, it means we have a delegate! Executing delegate with {0} [{1}]...", LastArgument, LastArgument.Length);
                finalCompletions = AutoCompleter.Invoke(LastArgument, index, delims)
                    .Where(x => x.StartsWith(LastArgument))
                    .Select(x => x[LastArgument.Length..])
                    .ToArray();
                DebugWriter.WriteDebug(DebugLevel.I, "Invoked, and got {0} autocompletion suggestions. [{1}]", finalCompletions.Length, string.Join(", ", finalCompletions));
                return finalCompletions;
            }
            return finalCompletions;
        }

    }
}
