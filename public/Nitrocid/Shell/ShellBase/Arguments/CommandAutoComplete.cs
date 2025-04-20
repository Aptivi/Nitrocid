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

using Nitrocid.Files;
using Nitrocid.Files.Folders;
using Nitrocid.Files.Operations.Querying;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using System.IO;
using System.Linq;
using Textify.General;

namespace Nitrocid.Shell.ShellBase.Arguments
{
    /// <summary>
    /// Command auto completion class
    /// </summary>
    internal static class CommandAutoComplete
    {

        internal static string[] GetSuggestions(string text, int index)
        {
            // First, cut the text to index
            text = text[..index];
            DebugWriter.WriteDebug(DebugLevel.I, "Text to auto complete: {0} (idx: {1})", text, index);

            // Then, check to see is we have shells
            DebugWriter.WriteDebug(DebugLevel.I, "Shell count: {0}", ShellManager.ShellStack.Count);
            if (ShellManager.ShellStack.Count <= 0)
                return [];

            // Get the commands based on the current shell type
            var shellType = ShellManager.CurrentShellType;
            var ShellCommandNames = CommandManager.GetCommandNames(shellType);
            DebugWriter.WriteDebug(DebugLevel.I, "Commands count for type {0}: {1}", shellType, ShellCommandNames.Length);

            // If text is not provided, return the command list without filtering
            if (string.IsNullOrEmpty(text))
                return ShellCommandNames;

            // Get the provided command and argument information
            var commandArgumentInfo = ArgumentsParser.ParseShellCommandArguments(text, shellType).total[0];

            // We're providing completion for argument.
            string CommandName = commandArgumentInfo.Command;
            string finalCommandArgs = commandArgumentInfo.ArgumentsText;
            string[] finalCommandArgsEnclosed = finalCommandArgs.SplitEncloseDoubleQuotes();
            int LastArgumentIndex = finalCommandArgsEnclosed.Length - 1;
            string LastArgument = finalCommandArgsEnclosed.Length > 0 ? finalCommandArgsEnclosed[LastArgumentIndex] : "";
            DebugWriter.WriteDebug(DebugLevel.I, "Command name: {0}", CommandName);
            DebugWriter.WriteDebug(DebugLevel.I, "Command arguments [{0}]: {1}", finalCommandArgsEnclosed.Length, finalCommandArgs);
            DebugWriter.WriteDebug(DebugLevel.I, "last argument: {0}", LastArgument);

            // Make a file and folder list
            string[] finalCompletions;
            if (!string.IsNullOrEmpty(finalCommandArgs))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Creating list of files and directories starting with argument {0} [{1}]...", LastArgument, LastArgument.Length);
                string lookupPath = Path.IsPathRooted(LastArgument) ? Path.GetDirectoryName(LastArgument) ?? "" : FilesystemTools.NeutralizePath(LastArgument, CurrentDirectory.CurrentDir);
                lookupPath = Checking.FolderExists(lookupPath) ? lookupPath : Path.GetDirectoryName(CurrentDirectory.CurrentDir + "/" + LastArgument) ?? "";
                finalCompletions = Listing.CreateList(lookupPath, true)
                    .Select(x => Path.IsPathRooted(LastArgument) ? FilesystemTools.NeutralizePath(x.FilePath) : FilesystemTools.NeutralizePath(x.FilePath).Replace(CurrentDirectory.CurrentDir + "/", ""))
                    .Where(x => x.StartsWith(LastArgument))
                    .Select(x => x[LastArgument.Length..])
                    .ToArray();
                DebugWriter.WriteDebug(DebugLevel.I, "Initially invoked, and got {0} autocompletion suggestions. [{1}]", finalCompletions.Length, string.Join(", ", finalCompletions));
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Creating list of commands starting with command {0} [{1}]...", CommandName, CommandName.Length);
                finalCompletions = ShellCommandNames
                    .Where(x => x.StartsWith(CommandName))
                    .Select(x => x[CommandName.Length..])
                    .ToArray();
                DebugWriter.WriteDebug(DebugLevel.I, "Initially invoked, and got {0} autocompletion suggestions. [{1}]", finalCompletions.Length, string.Join(", ", finalCompletions));
            }

            // Check to see if there is such command
            DebugWriter.WriteDebug(DebugLevel.I, "Command {0} exists? {1}", CommandName, ShellCommandNames.Contains(CommandName));
            if (!ShellCommandNames.Contains(CommandName))
                return finalCompletions;

            // We have the command. Check its entry for argument info
            var cmdInfo = CommandManager.GetCommand(CommandName, shellType);
            var CommandArgumentInfos = cmdInfo.CommandArgumentInfo;
            foreach (var CommandArgumentInfo in CommandArgumentInfos)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Command {0} has argument info? {1}", CommandName, CommandArgumentInfo is not null);
                if (CommandArgumentInfo is null)
                    // No arguments. Return file list
                    return finalCompletions;

                // There are arguments! Now, check to see if it has the accessible auto completer from the last argument
                var AutoCompleter =
                    LastArgumentIndex < CommandArgumentInfo.Arguments.Length && LastArgumentIndex >= 0 ?
                    CommandArgumentInfo.Arguments[LastArgumentIndex].Options.AutoCompleter :
                    null;
                DebugWriter.WriteDebug(DebugLevel.I, "Command {0} has auto complete info? {1}", CommandName, AutoCompleter is not null);
                if (AutoCompleter is null)
                    // No delegate. Return file list
                    return finalCompletions;

                // We have the delegate! Invoke it.
                DebugWriter.WriteDebug(DebugLevel.I, "If we reach here, it means we have a delegate! Executing delegate with {0} [{1}]...", LastArgument, LastArgument.Length);
                finalCompletions = AutoCompleter.Invoke(finalCommandArgsEnclosed)
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
