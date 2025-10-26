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

using Nitrocid.Base.Files;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Textify.General;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Finds a file in the specified directory or in the current directory using regular expressions
    /// </summary>
    /// <remarks>
    /// If you are looking for a file and you can't remember where, using this command will help you find it.
    /// </remarks>
    class FindRegCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string RegexToMatch = parameters.ArgumentsList[0];
            string DirectoryToSearch = FilesystemTools.CurrentDir;
            bool isRecursive = parameters.ContainsSwitch("-recursive");
            string command = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-exec").ReleaseDoubleQuotes();
            if (parameters.ArgumentsList.Length > 1)
                DirectoryToSearch = FilesystemTools.NeutralizePath(parameters.ArgumentsList[1]);

            // Print the results if found
            var AllFileEntries = FilesystemTools.GetFilesystemEntriesRegex(DirectoryToSearch, RegexToMatch, isRecursive);

            // Print or exec, depending on the command
            if (!string.IsNullOrWhiteSpace(command))
            {
                foreach (var file in AllFileEntries)
                {
                    ShellManager.AddAlternateThread();
                    ShellManager.GetLine($"{command} \"{file}\"");
                }
            }
            else
                ListWriterColor.WriteList(AllFileEntries);
            variableValue = string.Join('\n', AllFileEntries);
            return 0;
        }

    }
}
