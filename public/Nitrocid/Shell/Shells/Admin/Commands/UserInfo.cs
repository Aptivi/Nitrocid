
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.Users;

namespace KS.Shell.Shells.Admin.Commands
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
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Basic user info"), true, KernelColorType.ListTitle);
                TextWriterColor.Write(Translate.DoTranslation("Username") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.Write(user.Username, true, KernelColorType.ListValue);
                TextWriterColor.Write(Translate.DoTranslation("Full name") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.Write(user.FullName, true, KernelColorType.ListValue);
                TextWriterColor.Write(Translate.DoTranslation("Preferred language") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.Write(user.PreferredLanguage, true, KernelColorType.ListValue);
                TextWriterColor.Write(Translate.DoTranslation("Flags") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.Write(string.Join(", ", user.Flags), true, KernelColorType.ListValue);
                TextWriterColor.Write();

                // Now, the permissions.
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Permissions"), true, KernelColorType.ListTitle);
                foreach (string perm in user.Permissions)
                    TextWriterColor.Write($"  - {perm}", true, KernelColorType.ListValue);
                TextWriterColor.Write();

                // Now, the groups.
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Groups"), true, KernelColorType.ListTitle);
                foreach (string group in user.Groups)
                    TextWriterColor.Write($"  - {group}", true, KernelColorType.ListValue);
            }
            return 0;
        }
    }
}
