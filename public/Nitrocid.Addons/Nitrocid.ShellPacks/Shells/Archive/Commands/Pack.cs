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
using Nitrocid.ShellPacks.Tools;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;

namespace Nitrocid.ShellPacks.Shells.Archive.Commands
{
    /// <summary>
    /// Compresses a file to a ZIP archive
    /// </summary>
    /// <remarks>
    /// If you want to compress a single file from the ZIP archive, you can use this command.
    /// </remarks>
    class PackCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "pack";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_ARCHIVE_COMMAND_PACK_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "localfile", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_ARCHIVE_COMMAND_PACK_ARGUMENT_LOCALFILE_DESC"
                    }),
                    new CommandArgumentPart(false, "where", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_ARCHIVE_COMMAND_PACK_ARGUMENT_WHERE_DESC"
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string Where = "";
            if (parameters.ArgumentsList.Length > 1)
                Where = FilesystemTools.NeutralizePath(parameters.ArgumentsList[1], ArchiveShellCommon.CurrentDirectory);
            ArchiveTools.PackFile(parameters.ArgumentsList[0], Where);
            return 0;
        }

    }
}
