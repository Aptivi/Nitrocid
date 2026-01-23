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
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Nitrocid.Users;
using System.Globalization;

namespace Nitrocid.Shell.Shells.Admin.Commands
{
    class UserCultureCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string userName = parameters.ArgumentsList[0];
            string culture = parameters.ArgumentsList[1];
            int userIndex = UserManagement.GetUserIndex(userName);
            if (culture == "clear")
            {
                // If we're doing this on ourselves, change the kernel culture to the system culture
                culture = CultureManager.currentCulture.Name;
                if (UserManagement.CurrentUser.Username == userName)
                {
                    CultureManager.currentUserCulture = CultureManager.currentCulture;
                    UserManagement.CurrentUser.PreferredCulture = culture;
                }

                // Now, change the culture in the user config
                UserManagement.Users[userIndex].PreferredCulture = null;
                UserManagement.SaveUsers();
                TextWriterColor.Write(Translate.DoTranslation("Preferred user culture set to {0}. You may want to log in again."), culture);
            }
            else if (CultureManager.GetCulturesDictionary().TryGetValue(culture, out CultureInfo? cultureInfo))
            {
                // Do it locally
                if (UserManagement.CurrentUser.Username == userName)
                {
                    CultureManager.currentUserCulture = cultureInfo;
                    UserManagement.CurrentUser.PreferredCulture = culture;
                }

                // Now, change the culture in the user config
                UserManagement.Users[userIndex].PreferredCulture = culture;
                UserManagement.SaveUsers();
                TextWriterColor.Write(Translate.DoTranslation("Preferred user culture set to {0}. You may want to log in again."), culture);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Invalid culture") + " {0}", culture);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.NoSuchCulture);
            }
            return 0;
        }
    }
}
