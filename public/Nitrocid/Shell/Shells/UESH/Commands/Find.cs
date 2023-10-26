//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files;
using KS.Files.Folders;
using KS.Kernel.Threading;
using KS.Misc.Text;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.ShellBase.Switches;
using System.Linq;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Finds a file in the specified directory or in the current directory
    /// </summary>
    /// <remarks>
    /// If you are looking for a file and you can't remember where, using this command will help you find it.
    /// </remarks>
    class FindCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string FileToSearch = parameters.ArgumentsList[0];
            string DirectoryToSearch = CurrentDirectory.CurrentDir;
            bool isRecursive = parameters.SwitchesList.Contains("-recursive");
            string command = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-exec").ReleaseDoubleQuotes();
            if (parameters.ArgumentsList.Length > 1)
                DirectoryToSearch = FilesystemTools.NeutralizePath(parameters.ArgumentsList[1]);

            // Print the results if found
            var FileEntries = Listing.GetFilesystemEntries(DirectoryToSearch, FileToSearch, isRecursive);

            // Print or exec, depending on the command
            if (!string.IsNullOrWhiteSpace(command))
            {
                foreach (var file in FileEntries)
                {
                    var AltThreads = ShellManager.ShellStack[^1].AltCommandThreads;
                    if (AltThreads.Count == 0 || AltThreads[^1].IsAlive)
                    {
                        var WrappedCommand = new KernelThread($"Find Shell Command Thread for file {file}", false, (cmdThreadParams) =>
                            CommandExecutor.ExecuteCommand((CommandExecutorParameters)cmdThreadParams));
                        ShellManager.ShellStack[^1].AltCommandThreads.Add(WrappedCommand);
                    }
                    ShellManager.GetLine($"{command} \"{file}\"");
                }
            }
            else
                ListWriterColor.WriteList(FileEntries, true);
            variableValue = string.Join('\n', FileEntries);
            return 0;
        }

    }
}
