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
using Textify.General;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Splits a file
    /// </summary>
    /// <remarks>
    /// This command lets you split a file from your current working directory to a group of chunks.
    /// </remarks>
    class SplitFileCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "splitfile";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SPLITFILE_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SPLITFILE_ARGUMENT_FILE_DESC"
                    }),
                ],
                [
                    new SwitchInfo("outputDir", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SPLITFILE_ARGUMENT_OUTPUTDIR_DESC"),
                    new SwitchInfo("chunkSize", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SPLITFILE_ARGUMENT_CHUNKSIZE_DESC", new SwitchOptions()
                    {
                        IsNumeric = true,
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            PermissionsTools.Demand(PermissionTypes.ManageFilesystem);

            // Check the arguments
            string inputFile = parameters.ArgumentsList[0];
            if (!FilesystemTools.FileExists(inputFile))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_FILENOTFOUND2"), ThemeColorType.Error, inputFile);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
            }

            // Check the switches
            string outputDirectory = parameters.GetSwitchValue("-outputDir");
            string chunkSizeStr = parameters.GetSwitchValue("-chunkSize");
            chunkSizeStr = string.IsNullOrWhiteSpace(chunkSizeStr) || !chunkSizeStr.IsStringNumeric() ? "104857600" : chunkSizeStr;
            long chunkSize = long.Parse(chunkSizeStr);

            // Split the file now
            if (string.IsNullOrEmpty(outputDirectory))
                FilesystemTools.SplitFile(inputFile, chunkSize);
            else
                FilesystemTools.SplitFile(inputFile, outputDirectory, chunkSize);
            return 0;
        }

    }
}
