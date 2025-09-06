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

using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Nitrocid.Users;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// This command prints your current user name
    /// </summary>
    /// <remarks>
    /// This command prints your current user name. If invoked with -set, will also set the indicated variable to the current user name
    /// </remarks>
    class WhoamiCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string currentUser = UserManagement.CurrentUser.Username;
            TextWriterColor.Write(currentUser);
            variableValue = currentUser;
            return 0;
        }

    }
}
