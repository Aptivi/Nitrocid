
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using KS.Users;
using KS.Users.Login;

namespace KS.Shell.Shells.Admin.Commands
{
    class UserLangCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string userName = ListArgsOnly[0];
            string lang = ListArgsOnly[1];
            if (lang == "clear")
            {
                // If we're doing this on ourselves, change the kernel language to the system language
                lang = LanguageManager.currentLanguage.ThreeLetterLanguageName;
                if (Login.CurrentUser.Username == userName)
                {
                    LanguageManager.currentUserLanguage = LanguageManager.currentLanguage;
                    Login.CurrentUser.PreferredLanguage = lang;
                }

                // Now, change the language in the user config
                UserManagement.SetUserProperty(userName, UserManagement.UserProperty.PreferredLanguage, null);
                TextWriterColor.Write(Translate.DoTranslation("Preferred user language set to {0}. You may want to log in again."), lang);
            }
            else if (LanguageManager.Languages.ContainsKey(lang))
            {
                // Do it locally
                LanguageManager.currentUserLanguage = LanguageManager.Languages[lang];

                // Now, change the language in the user config
                UserManagement.SetUserProperty(userName, UserManagement.UserProperty.PreferredLanguage, lang);
                TextWriterColor.Write(Translate.DoTranslation("Preferred user language set to {0}. You may want to log in again."), lang);
            }
            else
                TextWriterColor.Write(Translate.DoTranslation("Invalid language") + " {0}", lang);
        }
    }
}
