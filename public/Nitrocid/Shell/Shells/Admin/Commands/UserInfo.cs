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

using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Users;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;

namespace Nitrocid.Shell.Shells.Admin.Commands
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
                SeparatorWriterColor.WriteSeparatorColor(Translate.DoTranslation("Basic user info"), KernelColorTools.GetColor(KernelColorType.ListTitle));
                TextWriters.Write(Translate.DoTranslation("Username") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write(user.Username, true, KernelColorType.ListValue);
                TextWriters.Write(Translate.DoTranslation("Full name") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write(user.FullName, true, KernelColorType.ListValue);
                TextWriters.Write(Translate.DoTranslation("Preferred language") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write(user.PreferredLanguage ?? "", true, KernelColorType.ListValue);
                TextWriters.Write(Translate.DoTranslation("Flags") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write(string.Join(", ", user.Flags), true, KernelColorType.ListValue);
                TextWriterRaw.Write();

                // Now, the permissions.
                SeparatorWriterColor.WriteSeparatorColor(Translate.DoTranslation("Permissions"), KernelColorTools.GetColor(KernelColorType.ListTitle));
                foreach (string perm in user.Permissions)
                    TextWriters.Write($"  - {perm}", true, KernelColorType.ListValue);
                TextWriterRaw.Write();

                // Now, the groups.
                SeparatorWriterColor.WriteSeparatorColor(Translate.DoTranslation("Groups"), KernelColorTools.GetColor(KernelColorType.ListTitle));
                foreach (string group in user.Groups)
                    TextWriters.Write($"  - {group}", true, KernelColorType.ListValue);
            }
            return 0;
        }
    }
}
