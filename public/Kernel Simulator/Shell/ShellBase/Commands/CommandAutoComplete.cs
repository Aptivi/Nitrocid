// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

namespace KS.Shell.ShellBase.Commands
{
#warning TermRead needs to implement auto complete handler here.
#if false
    /// <summary>
    /// Command auto completion class
    /// </summary>
    public class CommandAutoComplete : IAutoCompleteHandler
    {

        private readonly string ShellTypeToAutocomplete = "Shell";

        /// <summary>
        /// Command separators
        /// </summary>
        public char[] Separators { get; set; } = new[] { ' ' };

        /// <inheritdoc/>
        public string[] GetSuggestions(string text, int index)
        {
            var ShellCommands = CommandManager.GetCommands(ShellTypeToAutocomplete);
            if (ShellStart.ShellStack.Count > 0)
            {
                if (string.IsNullOrEmpty(text))
                {
                    return ShellCommands.Keys.ToArray();
                }
                else if (text.Contains(" "))
                {
                    // We're providing completion for argument.
                    string CommandName = text.SplitEncloseDoubleQuotes(" ")[0];
                    string LastArgument = text.SplitEncloseDoubleQuotes(" ")[text.SplitEncloseDoubleQuotes(" ").Length - 1];
                    var FileFolderList = Listing.CreateList(CurrentDirectory.CurrentDir, true)
                                                .Select(x => x.Name)
                                                .Where(x => x.StartsWith(LastArgument))
                                                .ToArray();
                    if (ShellCommands.ContainsKey(CommandName))
                    {
                        // We have the command. Check its entry for argument info
                        var CommandArgumentInfo = ShellCommands[CommandName].CommandArgumentInfo;
                        if (CommandArgumentInfo is not null)
                        {
                            // There are arguments! Now, check to see if it has the accessible auto completer
                            var AutoCompleter = CommandArgumentInfo.AutoCompleter;
                            if (AutoCompleter is not null)
                            {
                                // We have the delegate! Invoke it.
                                return AutoCompleter.Invoke(LastArgument);
                            }
                            else
                            {
                                // No delegate. Return file list
                                return FileFolderList;
                            }
                        }
                        else
                        {
                            // No arguments. Return file list
                            return FileFolderList;
                        }
                    }
                    return FileFolderList;
                }
                else
                {
                    // We're providing completion for command.
                    return ShellCommands.Keys.Where(x => x.StartsWith(text)).ToArray();
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Command auto complete constructor
        /// </summary>
        /// <param name="ShellType">Shell type</param>
        protected internal CommandAutoComplete(ShellType ShellType = ShellType.Shell) => ShellTypeToAutocomplete = Shell.GetShellTypeName(ShellType);

        /// <summary>
        /// Command auto complete constructor
        /// </summary>
        /// <param name="ShellType">Shell type</param>
        protected internal CommandAutoComplete(string ShellType = "Shell") => ShellTypeToAutocomplete = ShellType;

    }
#endif
}
