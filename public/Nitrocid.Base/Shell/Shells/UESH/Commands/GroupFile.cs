//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using Nitrocid.Base.Security.Permissions;
using Nitrocid.Base.Files;
using Terminaux.Shell.Commands;
using Nitrocid.Base.Kernel.Exceptions;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Themes.Colors;
using Nitrocid.Base.Languages;
using Textify.General;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Groups a file
    /// </summary>
    /// <remarks>
    /// This command lets you group a group of chunks to a single file.
    /// </remarks>
    class GroupFileCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            PermissionsTools.Demand(PermissionTypes.ManageFilesystem);

            // Check the arguments
            string inputFile = parameters.ArgumentsList[0];
            if (!FilesystemTools.FileExists(inputFile + ".C0000"))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_FILENOTFOUND2"), ThemeColorType.Error, inputFile + ".C0000");
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
            }

            // Check the switches
            string outputDirectory = parameters.GetSwitchValue("-outputDir");

            // Group the file now
            if (string.IsNullOrEmpty(outputDirectory))
                FilesystemTools.GroupFile(inputFile);
            else
                FilesystemTools.GroupFile(inputFile, outputDirectory);
            return 0;
        }

    }
}
