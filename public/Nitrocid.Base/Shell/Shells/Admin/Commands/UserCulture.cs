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

using System.Globalization;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Users;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Base.Shell.Shells.Admin.Commands
{
    class UserCultureCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "userculture";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_COMMAND_USERCULTURE_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "user", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_ADMIN_COMMAND_USERFLAG_ARGUMENT_USER_DESC"
                    }),
                    new CommandArgumentPart(true, "culture/clear", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_ADMIN_COMMAND_USERCULTURE_ARGUMENT_CULTUREID_DESC"
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string userName = parameters.ArgumentsList[0];
            string culture = parameters.ArgumentsList[1];
            int userIndex = UserManagement.GetUserIndex(userName);
            if (CultureManager.GetCulturesDictionary().TryGetValue(culture, out CultureInfo? cultureInfo) || culture == "clear")
            {
                // If we're doing this on ourselves, change the kernel culture to the system culture
                string finalCulture = culture == "clear" ? CultureManager.currentCulture.Name : culture;
                if (UserManagement.CurrentUser.Username == userName)
                {
                    CultureManager.currentUserCulture = culture == "clear" || cultureInfo is null ? CultureManager.currentCulture : cultureInfo;
                    UserManagement.CurrentUser.PreferredCulture = finalCulture;
                }

                // Now, change the culture in the user config
                UserManagement.Users[userIndex].PreferredCulture = culture == "clear" ? null : culture;
                UserManagement.SaveUsers();
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_USERCULTURE_SUCCESS"), finalCulture);
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_USERCULTURE_FAILURE") + " {0}", culture);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.NoSuchCulture);
            }
            return 0;
        }
    }
}
