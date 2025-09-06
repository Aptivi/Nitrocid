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
using Nitrocid.Security.Permissions;
using Terminaux.Shell.Commands;
using System.IO;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Bulk renames the files
    /// </summary>
    /// <remarks>
    /// If you wanted to rename a group of files matching the pattern, use this command to rename it to the specified name with file number at the end.
    /// </remarks>
    class BulkRenameCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            PermissionsTools.Demand(PermissionTypes.ManageFilesystem);
            string targetDir = parameters.ArgumentsList[0];
            string pattern = parameters.ArgumentsList[1];
            string newName = parameters.ArgumentsList[2];

            // Get the list of files from the pattern
            var files = FilesystemTools.GetFilesystemEntries(targetDir, pattern);

            // Enumerate the files and rename them
            int fileNo = 1;
            foreach (var file in files)
            {
                // Check to see if the path is really a file
                if (!FilesystemTools.FileExists(file))
                    continue;

                // Now, construct the file name and rename it to that
                string finalName = $"{newName}-{fileNo}{Path.GetExtension(file)}";
                FilesystemTools.MoveFileOrDir(file, FilesystemTools.NeutralizePath(finalName, targetDir));

                // Increment the number
                fileNo += 1;
            }
            return 0;
        }

    }
}
