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

using System;
using System.Linq;
using System.Threading;
using Nitrocid.Base.Drivers.Encryption;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Power;
using Nitrocid.Base.Languages;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Base.Extensions;
using Terminaux.Inputs;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Nitrocid.Base.Users.Login.Handlers.Logins
{
    internal class ModernLogin : BaseLoginHandler, ILoginHandler
    {
        public override bool LoginScreen()
        {
            var loginScreen = new Screen
            {
                CycleFrequency = 1000
            };
            bool proceed = true;

            try
            {
                // Clear the console
                ConsoleWrapper.CursorVisible = false;
                ConsoleWrapper.Clear();
                DebugWriter.WriteDebug(DebugLevel.I, "Loading modern logon... This shouldn't take long.");

                // Initialize the saved widget canvases
                ModernLogonScreen.canvases = ModernLogonScreen.GetLogonPages();
                int maxLogonScreens = ModernLogonScreen.canvases.Count + 1;

                // Create a screen for the login screen
                var loginScreenBuffer = new ScreenPart();
                ScreenTools.SetCurrent(loginScreen);
                ScreenTools.SetCurrentCyclic(loginScreen);
                ThemeColorsTools.LoadBackground();
                loginScreenBuffer.AddDynamicText(() =>
                {
                    try
                    {
                        if (ModernLogonScreen.screenNum > 0 && ModernLogonScreen.screenNum <= maxLogonScreens)
                            return ModernLogonScreen.PrintConfiguredLogonScreen(ModernLogonScreen.screenNum, ModernLogonScreen.canvases);
                        else
                        {
                            // Unknown screen!
                            string text = LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_UNKNOWNSCREENNUM");
                            string[] lines = ConsoleMisc.GetWrappedSentencesByWords(text, ConsoleWrapper.WindowWidth);
                            int top = ConsoleWrapper.WindowHeight / 2 - lines.Length / 2;
                            var errorText = new AlignedText()
                            {
                                Top = top,
                                Text = text,
                                ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Error),
                                Settings = new()
                                {
                                    Alignment = TextAlignment.Middle,
                                }
                            };
                            return errorText.Render();
                        }
                    }
                    catch (Exception ex) when (ex is not ThreadInterruptedException)
                    {
                        // An error occurred!
                        DebugWriter.WriteDebug(DebugLevel.E, $"Error rendering the modern logon: {ex.Message}");
                        DebugWriter.WriteDebugStackTrace(ex);
                        string text = LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_RENDERFAILED") + (KernelEntry.DebugMode ? $"\n\n{LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_RENDERFAILTIP")}" : "");
                        string[] lines = ConsoleMisc.GetWrappedSentencesByWords(text, ConsoleWrapper.WindowWidth);
                        int top = ConsoleWrapper.WindowHeight / 2 - lines.Length / 2;
                        var errorText = new AlignedText()
                        {
                            Top = top,
                            Text = text,
                            ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Error),
                            Settings = new()
                            {
                                Alignment = TextAlignment.Middle,
                            }
                        };
                        return errorText.Render();
                    }
                });
                loginScreen.AddBufferedPart("Modern Login", loginScreenBuffer);

                // Main loop
                ScreenTools.StartCyclicScreen();
                bool exiting = false;
                while (!exiting)
                {
                    ScreenTools.Render();

                    // Get input
                    var data = Input.ReadPointerOrKey();
                    if (data?.PointerEventContext is PointerEventContext context)
                    {
                        if (context.ButtonPress == PointerButtonPress.Released)
                            proceed = true;
                        if (proceed)
                            exiting = true;
                    }
                    else if (data?.ConsoleKeyInfo is ConsoleKeyInfo key)
                    {
                        // Check to see if user requested power actions
                        if (key.Key == ConsoleKey.Escape)
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
                            exiting = answer >= 0;
                            loginScreen.RequireRefresh();
                        }
                        else if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.RightArrow)
                        {
                            proceed = false;
                            if (key.Key == ConsoleKey.LeftArrow)
                            {
                                ModernLogonScreen.screenNum--;
                                if (ModernLogonScreen.screenNum <= 0)
                                    ModernLogonScreen.screenNum = 1;
                                else
                                    loginScreen.RequireRefresh();
                            }
                            else
                            {
                                ModernLogonScreen.screenNum++;
                                if (ModernLogonScreen.screenNum >= maxLogonScreens + 1)
                                    ModernLogonScreen.screenNum = maxLogonScreens;
                                else
                                    loginScreen.RequireRefresh();
                            }
                        }
                        else
                            proceed = true;
                        if (proceed)
                            exiting = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // An error occurred!
                ScreenTools.StopCyclicScreen();
                DebugWriter.WriteDebug(DebugLevel.E, $"Error rendering the modern logon: {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                string text = LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_RENDERFAILED") + (KernelEntry.DebugMode ? $"\n\n{LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_RENDERFAILTIP")}" : "");
                InfoBoxModalColor.WriteInfoBoxModal(text);
            }
            finally
            {
                ScreenTools.StopCyclicScreen();
                ScreenTools.UnsetCurrentCyclic();
                ScreenTools.UnsetCurrent(loginScreen);
            }
            return proceed;
        }

        public override string UserSelector()
        {
            // Get the user list first
            var users = GetUsersList();

            // Some common variables
            var logonScreenScreen = new Screen();
            var logonScreenScreenBuffer = new ScreenPart();
            logonScreenScreenBuffer.AddDynamicText(() => ModernLogonScreen.PrintConfiguredLogonScreen(ModernLogonScreen.screenNum, ModernLogonScreen.canvases));
            logonScreenScreen.AddBufferedPart("User selector screen part", logonScreenScreenBuffer);

            // Then, make the choices and prompt for the selection
            ScreenTools.SetCurrent(logonScreenScreen);
            ScreenTools.Render();
            var choices = InputChoiceTools.GetInputChoices(users);
            int userNum = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choices], LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_SELECTUSER")) + 1;
            ScreenTools.UnsetCurrent(logonScreenScreen);
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

            // Some common variables
            var logonScreenScreen = new Screen();
            var logonScreenScreenBuffer = new ScreenPart();
            logonScreenScreenBuffer.AddDynamicText(() => ModernLogonScreen.PrintConfiguredLogonScreen(ModernLogonScreen.screenNum, ModernLogonScreen.canvases));
            logonScreenScreen.AddBufferedPart("User selector screen part", logonScreenScreenBuffer);
            ScreenTools.SetCurrent(logonScreenScreen);
            ScreenTools.Render();

            // The password is not empty. Prompt for password.
            pass = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_PASSWORD") + $" {user}: ", InfoBoxInputType.Password);
            ScreenTools.UnsetCurrent(logonScreenScreen);
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

        private (string user, string fullName)[] GetUsersList()
        {
            var users = UserManagement.ListAllUsers().Select(
                (user) =>
                {
                    var userInfo = UserManagement.GetUser(user) ??
                        throw new KernelException(KernelExceptionType.LoginHandler, LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SUDO_EXCEPTION_USERINFO") + $" {user}");
                    var fullName = userInfo.FullName;
                    return (user, string.IsNullOrEmpty(fullName) ? user : fullName);
                }
            ).ToArray();
            return users;
        }
    }
}
