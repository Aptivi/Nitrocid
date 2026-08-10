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

using System.IO;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.ShellPacks.Shells.Git.Commands
{
    /// <summary>
    /// File status
    /// </summary>
    /// <remarks>
    /// This command prints a file status.
    /// </remarks>
    class FileStatusCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "filestatus";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_FILESTATUS_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_GIT_COMMAND_ARGUMENT_PATH_DESC"
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var gitShell = (GitShell?)shell ??
                throw new KernelException(KernelExceptionType.Git, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            if (gitShell.Repository is null)
                return 43;
            string file = parameters.ArgumentsList[0];
            var status = gitShell.Repository.RetrieveStatus(file);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_FILESTATUS_TITLE") + $" {Path.GetFileName(file)}: {status}");
            return 0;
        }

    }
}
