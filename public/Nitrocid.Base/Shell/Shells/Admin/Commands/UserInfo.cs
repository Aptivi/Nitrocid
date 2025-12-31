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

using Terminaux.Shell.Commands;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Themes.Colors;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Users;

namespace Nitrocid.Base.Shell.Shells.Admin.Commands
{
    /// <summary>
    /// Gets the user information
    /// </summary>
    /// <remarks>
    /// This command gets the user information either from the current user or from a specific user
    /// </remarks>
    class UserInfoCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Get the requested username
            string userName = parameters.ArgumentsList.Length > 0 ? parameters.ArgumentsList[0] : UserManagement.CurrentUser.Username;

            // Now, try to get the username and print its information
            var user = UserManagement.GetUser(userName);
            if (user is not null)
            {
                // First off, basic user information
                SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_USERINFO_TITLE"), ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_USERNAME_PROMPT") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write(user.Username, true, ThemeColorType.ListValue);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FULLNAME") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write(user.FullName, true, ThemeColorType.ListValue);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_USERINFO_PREFLANG") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write(user.PreferredLanguage ?? "", true, ThemeColorType.ListValue);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_USERINFO_PREFCULTURE") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write(user.PreferredCulture ?? "", true, ThemeColorType.ListValue);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_USERINFO_FLAGS") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write(string.Join(", ", user.Flags), true, ThemeColorType.ListValue);
                TextWriterRaw.Write();

                // Now, the permissions.
                SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_USERINFO_PERMS"), ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                foreach (string perm in user.Permissions)
                    TextWriterColor.Write($"  - {perm}", true, ThemeColorType.ListValue);
                TextWriterRaw.Write();

                // Now, the groups.
                SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_USERINFO_GROUPS"), ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                foreach (string group in user.Groups)
                    TextWriterColor.Write($"  - {group}", true, ThemeColorType.ListValue);
            }
            return 0;
        }
    }
}
