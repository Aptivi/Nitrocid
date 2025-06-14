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
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Users;

namespace Nitrocid.Shell.Shells.Admin.Commands
{
    class UserLangCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string userName = parameters.ArgumentsList[0];
            string lang = parameters.ArgumentsList[1];
            int userIndex = UserManagement.GetUserIndex(userName);
            if (lang == "clear")
            {
                // If we're doing this on ourselves, change the kernel language to the system language
                lang = LanguageManager.currentLanguage.ThreeLetterLanguageName;
                if (UserManagement.CurrentUser.Username == userName)
                {
                    LanguageManager.currentUserLanguage = LanguageManager.currentLanguage;
                    UserManagement.CurrentUser.PreferredLanguage = lang;
                }

                // Now, change the language in the user config
                UserManagement.Users[userIndex].PreferredLanguage = null;
                UserManagement.SaveUsers();
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_USERLANG_SUCCESS"), lang);
            }
            else if (LanguageManager.Languages.TryGetValue(lang, out LanguageInfo? langInfo))
            {
                // Do it locally
                if (UserManagement.CurrentUser.Username == userName)
                {
                    LanguageManager.currentUserLanguage = langInfo;
                    UserManagement.CurrentUser.PreferredLanguage = lang;
                }

                // Now, change the language in the user config
                UserManagement.Users[userIndex].PreferredLanguage = lang;
                UserManagement.SaveUsers();
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_USERLANG_SUCCESS"), lang);
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_LANGUAGES_EXCEPTION_INVALIDLANG") + " {0}", lang);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.NoSuchLanguage);
            }
            return 0;
        }
    }
}
