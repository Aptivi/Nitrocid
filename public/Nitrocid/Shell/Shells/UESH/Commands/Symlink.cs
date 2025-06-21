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

using Terminaux.Colors.Themes.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Security.Permissions;
using Terminaux.Shell.Commands;
using System;

namespace Nitrocid.Shell.Shells.UESH.Commands
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
            if (!FilesystemTools.Exists(target))
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_TARGETNOTFOUND"), ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
            }
            if (FilesystemTools.Exists(linkName))
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_OVERWRITEWITHSYMLINK"), ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
            }
            try
            {
                FilesystemTools.MakeSymlink(linkName, target);
            }
            catch (Exception ex)
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SYMLINK_FAILED") + $" {ex.Message}", ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
            }
            return 0;
        }

    }
}
