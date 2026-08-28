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

using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Nitrocid.Security.Permissions;
using Nitrocid.Users;
using Nitrocid.Users.Login;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

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
        public override string Command => 
            "chusrname";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHUSRNAME_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "oldUserName", new CommandArgumentPartOptions()
                    {
                        AutoCompleter = (_) => [.. UserManagement.ListAllUsers()],
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHUSRNAME_ARGUMENT_OLDNAME_DESC",
                    }),
                    new CommandArgumentPart(true, "newUserName", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHUSRNAME_ARGUMENT_NEWNAME_DESC"
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            if (!PermissionsTools.IsPermissionGranted(PermissionTypes.RunStrictCommands) &&
                !UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", vars: [parameters.CommandText]);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_NEEDSPERM"), true, ThemeColorType.Error, parameters.CommandText);
                return -4;
            }

            string oldUserName = parameters.ArgumentsList[0];
            string newUserName = parameters.ArgumentsList[1];
            UserManagement.ChangeUsername(oldUserName, newUserName);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHUSRNAME_SUCCESS"), newUserName);
            if (oldUserName == UserManagement.CurrentUser.Username)
                Login.LogoutRequested = true;
            return 0;
        }

    }
}
