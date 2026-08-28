//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Nitrocid.Base.Files;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Finds a file in the specified directory or in the current directory
    /// </summary>
    /// <remarks>
    /// If you are looking for a file and you can't remember where, using this command will help you find it.
    /// </remarks>
    class FindCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "find";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_FIND_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "file", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_MISC_INTERACTIVES_FMTUI_FILENAME"
                    }),
                    new CommandArgumentPart(true, "directory", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DIRINFO_ARGUMENT_DIRECTORY_DESC"
                    }),
                ],
                [
                    new SwitchInfo("recursive", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_FIND_SWITCH_RECURSIVE_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                    new SwitchInfo("exec", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_FIND_SWITCH_EXEC_DESC", new SwitchOptions()
                    {
                        ArgumentsRequired = true
                    })
                ], true)
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string fileToSearch = parameters.ArgumentsList[0];
            string directoryToSearch = FilesystemTools.CurrentDir;
            bool isRecursive = parameters.ContainsSwitch("-recursive");
            string command = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-exec").ReleaseDoubleQuotes();
            if (parameters.ArgumentsList.Length > 1)
                directoryToSearch = FilesystemTools.NeutralizePath(parameters.ArgumentsList[1]);

            // Print the results if found
            var FileEntries = FilesystemTools.GetFilesystemEntries(directoryToSearch, fileToSearch, isRecursive);

            // Print or exec, depending on the command
            if (!string.IsNullOrWhiteSpace(command))
            {
                foreach (var file in FileEntries)
                {
                    ShellManager.AddAlternateThread();
                    ShellManager.GetLine($"{command} \"{file}\"");
                }
            }
            else
                ListWriterColor.WriteList(FileEntries);
            variableValue = string.Join('\n', FileEntries);
            return 0;
        }

    }
}
