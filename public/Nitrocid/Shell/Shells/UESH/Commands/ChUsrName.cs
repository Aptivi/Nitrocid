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
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Users;
using Nitrocid.Users.Login;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can change your username or someone else's username
    /// </summary>
    /// <remarks>
    /// If your username or someone else's username needs to be changed to a new username, you need to change them if it's your username or if someone allows you to change their username to another name.
    /// <br></br>
    /// You need to specify the current user name before the new user name so the tool knows how to change someone else's name or your name to another name.
    /// <br></br>
    /// When you're changing your name to someone else's name, you will be logged off for changes to take effect. Use your new username, not the old one.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class ChUsrNameCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            UserManagement.ChangeUsername(parameters.ArgumentsList[0], parameters.ArgumentsList[1]);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHUSRNAME_SUCCESS"), parameters.ArgumentsList[1]);
            if (parameters.ArgumentsList[0] == UserManagement.CurrentUser.Username)
                Login.LogoutRequested = true;
            return 0;
        }

    }
}
