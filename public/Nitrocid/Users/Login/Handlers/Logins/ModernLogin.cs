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

using Nitrocid.ConsoleBase.Colors;
using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.Drivers.Encryption;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using System.Linq;
using System.Threading;
using Terminaux.Base;
using Terminaux.Inputs;
using System;
using Nitrocid.Kernel.Power;
using Nitrocid.Users.Login.Widgets;
using Terminaux.Inputs.Styles;
using Nitrocid.Kernel.Exceptions;
using Terminaux.Inputs.Styles.Infobox.Tools;

namespace Nitrocid.Users.Login.Handlers.Logins
{
    internal class ModernLogin : BaseLoginHandler, ILoginHandler
    {
        public override bool LoginScreen()
        {
            // Clear the console
            ConsoleWrapper.CursorVisible = false;
            ConsoleWrapper.Clear();
            DebugWriter.WriteDebug(DebugLevel.I, "Loading modern logon... This shouldn't take long.");

            // Start the date and time update thread to show time and date in the modern way
            ModernLogonScreen.updateThread.Start();

            // Wait for the keypress
            DebugWriter.WriteDebug(DebugLevel.I, "Rendering...");
            SpinWait.SpinUntil(() => ModernLogonScreen.renderedFully);
            DebugWriter.WriteDebug(DebugLevel.I, "Rendered fully!");
            var key = Input.ReadKey().Key;

            // Stop the thread if screen number indicates that we're on the main screen
            ModernLogonScreen.updateThread.Stop();
            ModernLogonScreen.renderedFully = false;

            // Check to see if user requested power actions
            bool proceed = true;
            if (key == ConsoleKey.Escape)
            {
                int answer = InfoBoxButtonsColor.WriteInfoBoxButtons([
                    new InputChoiceInfo("shutdown", Translate.DoTranslation("Shut down")),
                    new InputChoiceInfo("reboot", Translate.DoTranslation("Restart")),
                    new InputChoiceInfo("login", Translate.DoTranslation("Login")),
                ], Translate.DoTranslation("You've entered the power action menu. Please enter a choice using the left and the right arrow keys and press ENTER, or press ESC to go back to the main screen."));
                if (answer == 0)
                    PowerManager.PowerManage(PowerMode.Shutdown);
                else if (answer == 1)
                    PowerManager.PowerManage(PowerMode.Reboot);
                proceed = answer == 2;
            }
            else if (key == ConsoleKey.LeftArrow || key == ConsoleKey.RightArrow)
            {
                proceed = false;
                if (!ModernLogonScreen.enableWidgets)
                    return proceed;
                CleanWidgetsUp();
                if (key == ConsoleKey.LeftArrow)
                {
                    ModernLogonScreen.screenNum--;
                    if (ModernLogonScreen.screenNum <= 0)
                        ModernLogonScreen.screenNum = 1;
                }
                else
                {
                    ModernLogonScreen.screenNum++;
                    if (ModernLogonScreen.screenNum >= 4)
                        ModernLogonScreen.screenNum = 3;
                }
            }
            return proceed;
        }

        public override string UserSelector()
        {
            // First, get the user number from the selection input
            var users = UserManagement.ListAllUsers().Select(
                (user) =>
                {
                    var userInfo = UserManagement.GetUser(user) ??
                    throw new KernelException(KernelExceptionType.LoginHandler, Translate.DoTranslation("Can't get user info for") + $" {user}");
                    var fullName = userInfo.FullName;
                    return (user, string.IsNullOrEmpty(fullName) ? user : fullName);
                }
            ).ToArray();

            // Then, make the choices and prompt for the selection
            KernelColorTools.LoadBackground();
            var choices = InputChoiceTools.GetInputChoices(users);
            int userNum = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choices], Translate.DoTranslation("Select a user account you want to log in with.")) + 1;
            return
                userNum != 0 ?
                UserManagement.SelectUser(userNum) :
                "";
        }

        public override bool PasswordHandler(string user, ref string pass)
        {
            // Check if password is empty
            var userInfo = UserManagement.GetUser(user) ??
            throw new KernelException(KernelExceptionType.LoginHandler, Translate.DoTranslation("Can't get user info for") + $" {user}");
            ConsoleWrapper.Clear();
            string UserPassword = userInfo.Password;
            if (UserPassword == Encryption.GetEmptyHash("SHA256"))
                return true;

            // The password is not empty. Prompt for password.
            pass = InfoBoxInputColor.WriteInfoBoxInputPassword(Translate.DoTranslation("Enter the password for user") + $" {user}: ");
            KernelColorTools.LoadBackground();

            // Validate the password
            if (UserManagement.ValidatePassword(user, pass))
                // Password written correctly. Log in.
                return true;
            else
                // Wrong password.
                InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("Wrong password for user."), new InfoBoxSettings()
                {
                    ForegroundColor = KernelColorTools.GetColor(KernelColorType.Error)
                });
            return false;
        }

        private void CleanWidgetsUp()
        {
            if (ModernLogonScreen.screenNum == 2)
                WidgetTools.CleanupWidget(WidgetTools.GetWidgetName(ModernLogonScreen.FirstWidget));
            else if (ModernLogonScreen.screenNum == 3)
                WidgetTools.CleanupWidget(WidgetTools.GetWidgetName(ModernLogonScreen.SecondWidget));
        }
    }
}
