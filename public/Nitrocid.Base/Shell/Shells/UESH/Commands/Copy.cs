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
using Nitrocid.Base.Languages;
using Nitrocid.Base.Security.Permissions;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can copy the file to a destination.
    /// </summary>
    /// <remarks>
    /// This command allows you to create copies of files or folders to a different name, or different path. This is useful for many purposes.
    /// </remarks>
    class CopyCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "copy";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_COPY_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "source", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_COPY_ARGUMENT_SOURCE_DESC"
                    }),
                    new CommandArgumentPart(true, "target", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_COPY_ARGUMENT_TARGET_DESC"
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            PermissionsTools.Demand(PermissionTypes.ManageFilesystem);
            string source = parameters.ArgumentsList[0];
            string target = parameters.ArgumentsList[1];
            FilesystemTools.CopyFileOrDir(source, target);
            return 0;
        }
    }
}
