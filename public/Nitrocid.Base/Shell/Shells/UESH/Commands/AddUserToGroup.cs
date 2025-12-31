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

using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Nitrocid.Base.Users.Groups;
using Nitrocid.Base.Languages;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can add a user to a group when needed.
    /// </summary>
    /// <remarks>
    /// If you need to add a user to a group to organize people that use the kernel, you can add a users to the group.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class AddUserToGroupCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_ADDUSERTOGROUP_PROGRESS"), parameters.ArgumentsList[0], parameters.ArgumentsList[1]);
            GroupManagement.AddUserToGroup(parameters.ArgumentsList[0], parameters.ArgumentsList[1]);
            return 0;
        }

    }
}
