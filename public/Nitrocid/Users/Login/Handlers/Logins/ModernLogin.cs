//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Inputs;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.Encryption;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using System.Linq;
using System.Threading;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Inputs;
using System;
using Nitrocid.Kernel.Power;

namespace Nitrocid.Users.Login.Handlers.Logins
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
            var key = Input.DetectKeypress().Key;

            // Stop the thread
            ModernLogonScreen.DateTimeUpdateThread.Stop();
            ModernLogonScreen.renderedFully = false;

            // Check to see if user requested power actions
            if (key == ConsoleKey.Escape)
            {
                int answer = InfoBoxButtonsColor.WriteInfoBoxButtons([
                    new InputChoiceInfo("shutdown", Translate.DoTranslation("Shut down")),
                    new InputChoiceInfo("reboot", Translate.DoTranslation("Restart")),
                    new InputChoiceInfo("login", Translate.DoTranslation("Login")),
                ], Translate.DoTranslation("What do you want to do?"));
                if (answer == 0)
                    PowerManager.PowerManage(PowerMode.Shutdown);
                else if (answer == 1)
                    PowerManager.PowerManage(PowerMode.Reboot);
            }
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

            // Then, make the choices and prompt for the selection
            ColorTools.LoadBack();
            var choices = InputChoiceTools.GetInputChoices(users, userFullNames);
            int userNum = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choices], Translate.DoTranslation("Select a user account you want to log in with.")) + 1;
            return
                userNum != 0 ?
                UserManagement.SelectUser(userNum) :
                "";
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
            string password = InputTools.ReadLineNoInput();

            // Validate the password
            pass = password;
            if (UserManagement.ValidatePassword(user, password))
                // Password written correctly. Log in.
                return true;
            else
                // Wrong password.
                InfoBoxColor.WriteInfoBoxColor(Translate.DoTranslation("Wrong password for user."), KernelColorTools.GetColor(KernelColorType.Error));
            return false;
        }
    }
}
