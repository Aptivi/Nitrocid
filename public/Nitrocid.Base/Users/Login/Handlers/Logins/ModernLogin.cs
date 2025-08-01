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

using Terminaux.Colors.Themes.Colors;
using Terminaux.Inputs.Styles.Infobox;
using System.Linq;
using System.Threading;
using Terminaux.Base;
using Terminaux.Inputs;
using System;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Users.Login.Widgets;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Power;
using Nitrocid.Base.Drivers.Encryption;

namespace Nitrocid.Base.Users.Login.Handlers.Logins
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
                    new InputChoiceInfo("shutdown", LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_SHUTDOWN")),
                    new InputChoiceInfo("reboot", LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_RESTART")),
                    new InputChoiceInfo("login", LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_LOGIN")),
                ], LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_POWERACTION"));
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
                    throw new KernelException(KernelExceptionType.LoginHandler, LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SUDO_EXCEPTION_USERINFO") + $" {user}");
                    var fullName = userInfo.FullName;
                    return (user, string.IsNullOrEmpty(fullName) ? user : fullName);
                }
            ).ToArray();

            // Then, make the choices and prompt for the selection
            ThemeColorsTools.LoadBackground();
            var choices = InputChoiceTools.GetInputChoices(users);
            int userNum = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choices], LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_SELECTUSER")) + 1;
            return
                userNum != 0 ?
                UserManagement.SelectUser(userNum) :
                "";
        }

        public override bool PasswordHandler(string user, ref string pass)
        {
            // Check if password is empty
            var userInfo = UserManagement.GetUser(user) ??
            throw new KernelException(KernelExceptionType.LoginHandler, LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SUDO_EXCEPTION_USERINFO") + $" {user}");
            ConsoleWrapper.Clear();
            string UserPassword = userInfo.Password;
            if (UserPassword == Encryption.GetEmptyHash("SHA256"))
                return true;

            // The password is not empty. Prompt for password.
            pass = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_PASSWORD") + $" {user}: ", InfoBoxInputType.Password);
            ThemeColorsTools.LoadBackground();

            // Validate the password
            if (UserManagement.ValidatePassword(user, pass))
                // Password written correctly. Log in.
                return true;
            else
                // Wrong password.
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_WRONGPASSWORD"), new InfoBoxSettings()
                {
                    ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Error)
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
