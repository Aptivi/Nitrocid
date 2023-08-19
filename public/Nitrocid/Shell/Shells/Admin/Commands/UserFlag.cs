
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

using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.Users;

namespace KS.Shell.Shells.Admin.Commands
{
    class UserFlagCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            string userName = ListArgsOnly[0];
            string type = ListArgsOnly[1];
            bool enabled = bool.Parse(ListArgsOnly[2]);
            int userIndex = UserManagement.GetUserIndex(userName);
            switch (type)
            {
                case "admin":
                    UserManagement.Users[userIndex].Admin = enabled;
                    break;
                case "disabled":
                    UserManagement.Users[userIndex].Disabled = enabled;
                    break;
                case "anonymous":
                    UserManagement.Users[userIndex].Anonymous = enabled;
                    break;
                default:
                    TextWriterColor.Write(Translate.DoTranslation("The specified main flag type is invalid") + ": {0}", type);
                    return 10000 + (int)KernelExceptionType.UserManagement;
            }
            return 0;
        }
    }
}
