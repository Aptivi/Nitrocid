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

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// This command allows you to make a file.
    /// </summary>
    /// <remarks>
    /// If you wanted to create a file, this command is for you. It allows you to create every kind of file, including files without extensions, text files, binary files, and so on. If it's a text file, you can use the edit command to add new lines, delete them, and so on.
    /// </remarks>
    class MkFileCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            PermissionsTools.Demand(PermissionTypes.ManageFilesystem);
            FilesystemTools.MakeFile(parameters.ArgumentsList[0]);
            variableValue = parameters.ArgumentsList[0];
            return 0;
        }
    }
}
