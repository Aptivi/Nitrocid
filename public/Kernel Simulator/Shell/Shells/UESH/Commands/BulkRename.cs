
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

using KS.Files;
using KS.Files.Folders;
using KS.Files.Operations;
using KS.Files.Querying;
using KS.Kernel.Debugging.RemoteDebug;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using System.IO;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Bulk renames the files
    /// </summary>
    /// <remarks>
    /// If you wanted to rename a group of files matching the pattern, use this command to rename it to the specified name with file number at the end.
    /// </remarks>
    class BulkRenameCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string targetDir = ListArgsOnly[0];
            string pattern = ListArgsOnly[1];
            string newName = ListArgsOnly[2];

            // Get the list of files from the pattern
            var files = Listing.GetFilesystemEntries(targetDir, pattern);

            // Enumerate the files and rename them
            int fileNo = 1;
            foreach (var file in files)
            {
                // Check to see if the path is really a file
                if (!Checking.FileExists(file))
                    continue;

                // Now, construct the file name and rename it to that
                string finalName = $"{newName}-{fileNo}{Path.GetExtension(file)}";
                Moving.MoveFileOrDir(file, Filesystem.NeutralizePath(finalName, targetDir));

                // Increment the number
                fileNo += 1;
            }
        }

    }
}
