//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers;
using KS.Files;
using KS.Files.Operations;
using KS.Files.Operations.Querying;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Security.Permissions;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can make a symbolic link to a destination.
    /// </summary>
    /// <remarks>
    /// This command allows you to create symbolic links to a destination file or folder. This is useful for many purposes.
    /// </remarks>
    class SymlinkCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            PermissionsTools.Demand(PermissionTypes.ManageFilesystem);
            string linkName = FilesystemTools.NeutralizePath(parameters.ArgumentsList[0]);
            string target = FilesystemTools.NeutralizePath(parameters.ArgumentsList[1]);
            if (!Checking.Exists(target))
            {
                TextWriters.Write(Translate.DoTranslation("The target file or directory isn't found."), KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Filesystem;
            }
            if (Checking.Exists(linkName))
            {
                TextWriters.Write(Translate.DoTranslation("Can't overwrite an existing file or directory with a symbolic link."), KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Filesystem;
            }
            try
            {
                Making.MakeSymlink(linkName, target);
            }
            catch (Exception ex)
            {
                TextWriters.Write(Translate.DoTranslation("Can't make a symbolic link.") + $"{ex.Message}", KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Filesystem;
            }
            return 0;
        }

    }
}
