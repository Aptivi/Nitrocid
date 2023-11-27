//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Inputs.Styles.Infobox;
using KS.ConsoleBase.Inputs.Styles.Selection;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.Encryption;
using KS.Kernel.Debugging;
using KS.Languages;
using System.Linq;
using System.Threading;

namespace KS.Users.Login.Handlers.Logins
{
    internal class ModernLogin : BaseLoginHandler, ILoginHandler
    {
        public override void LoginScreen()
        {
            // Clear the console
            ConsoleWrapper.CursorVisible = false;
            ConsoleWrapper.Clear();
            DebugWriter.WriteDebug(DebugLevel.I, "Loading modern logon... This shouldn't take long.");

            // Start the date and time update thread to show time and date in the modern way
            ModernLogonScreen.DateTimeUpdateThread.Start();

            // Wait for the keypress
            DebugWriter.WriteDebug(DebugLevel.I, "Rendering...");
            SpinWait.SpinUntil(() => ModernLogonScreen.renderedFully);
            DebugWriter.WriteDebug(DebugLevel.I, "Rendered fully!");
            Input.DetectKeypress();

            // Stop the thread
            ModernLogonScreen.DateTimeUpdateThread.Stop();
            ModernLogonScreen.renderedFully = false;
        }

        public override string UserSelector()
        {
            // First, get the user number from the selection input
            var users = UserManagement.ListAllUsers().ToArray();
            var userFullNames = users.Select(
                (user) =>
                    UserManagement.GetUser(user).FullName is not null ?
                    UserManagement.GetUser(user).FullName :
                    ""
            ).ToArray();
            int userNum = SelectionStyle.PromptSelection(Translate.DoTranslation("Select a user account you want to log in with."), string.Join("/", users), userFullNames, true);
            return UserManagement.SelectUser(userNum);
        }

        public override bool PasswordHandler(string user, ref string pass)
        {
            // Check if password is empty
            ConsoleWrapper.Clear();
            string UserPassword = UserManagement.GetUser(user).Password;
            if (UserPassword == Encryption.GetEmptyHash("SHA256"))
                return true;

            // The password is not empty. Prompt for password.
            TextWriterColor.Write(Translate.DoTranslation("Enter the password for user") + " {0}: ", false, user);
            string password = Input.ReadLineNoInput();

            // Validate the password
            pass = password;
            if (UserManagement.ValidatePassword(user, password))
                // Password written correctly. Log in.
                return true;
            else
                // Wrong password.
                InfoBoxColor.WriteInfoBoxKernelColor(Translate.DoTranslation("Wrong password for user."), KernelColorType.Error);
            return false;
        }
    }
}
