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
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Security.Permissions;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

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
        public override string Command => 
            "groupfile";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_GROUPFILE_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GROUPFILE_ARGUMENT_FILE_DESC"
                    }),
                ],
                [
                    new SwitchInfo("outputDir", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GROUPFILE_ARGUMENT_OUTPUTDIR_DESC"),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
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
