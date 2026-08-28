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

using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.ShellPacks.Tools;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.ShellPacks.Shells.Archive.Commands
{
    /// <summary>
    /// Changes current local directory
    /// </summary>
    /// <remarks>
    /// If you want to interact with the ZIP file in another local directory, you can use this command to change the current local directory. This change isn't applied to the main shell.
    /// </remarks>
    class ChDirCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "chdir";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELLPACKS_ARCHIVE_FS_COMMAND_CHDIR_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_ARCHIVE_COMMAND_CHDIR_ARGUMENT_DIRECTORY_DESC"
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            if (!ArchiveTools.ChangeWorkingArchiveLocalDirectory(parameters.ArgumentsList[0]))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_ARCHIVE_LOCALDIRNOTFOUND"), true, ThemeColorType.Error, parameters.ArgumentsList[0]);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Archive);
            }
            return 0;
        }

    }
}
