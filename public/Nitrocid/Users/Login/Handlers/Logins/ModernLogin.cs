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
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Drivers.Encryption;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Kernel.Power;
using Nitrocid.Languages;
using Nitrocid.Users.Login.Widgets;
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

namespace Nitrocid.Users.Login.Handlers.Logins
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

                // Create a screen for the login screen
                var loginScreenBuffer = new ScreenPart();
                ScreenTools.SetCurrent(loginScreen);
                ScreenTools.SetCurrentCyclic(loginScreen);
                ThemeColorsTools.LoadBackground();
                loginScreenBuffer.AddDynamicText(() =>
                {
                    try
                    {
                        if (ModernLogonScreen.screenNum > 0 && ModernLogonScreen.screenNum <= 3)
                            return ModernLogonScreen.PrintConfiguredLogonScreen(ModernLogonScreen.screenNum);
                        else
                        {
                            // Unknown screen!
                            string text = Translate.DoTranslation("Unknown screen number.");
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
                        string text = Translate.DoTranslation("Failed to render the logon screen.") + (KernelEntry.DebugMode ? $"\n\n{Translate.DoTranslation("Investigate the debug logs for more information about the error.")}" : "");
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
                                new InputChoiceInfo("shutdown", Translate.DoTranslation("Shut down")),
                                new InputChoiceInfo("reboot", Translate.DoTranslation("Restart")),
                                new InputChoiceInfo("login", Translate.DoTranslation("Login")),
                            ], Translate.DoTranslation("You've entered the power action menu. Please enter a choice using the left and the right arrow keys and press ENTER, or press ESC to go back to the main screen."));
                            if (answer == 0)
                                PowerManager.PowerManage(PowerMode.Shutdown);
                            else if (answer == 1)
                                PowerManager.PowerManage(PowerMode.Reboot);
                            proceed = answer == 2;
                            exiting = answer >= 0;
                        }
                        else if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.RightArrow)
                        {
                            proceed = false;
                            if (!ModernLogonScreen.enableWidgets)
                                return proceed;
                            CleanWidgetsUp();
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
                                if (ModernLogonScreen.screenNum >= 4)
                                    ModernLogonScreen.screenNum = 3;
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
                string text = Translate.DoTranslation("Failed to render the logon screen.") + (KernelEntry.DebugMode ? $"\n\n{Translate.DoTranslation("Investigate the debug logs for more information about the error.")}" : "");
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
            pass = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Enter the password for user") + $" {user}: ", InfoBoxInputType.Password);
            KernelColorTools.LoadBackground();

            // Validate the password
            if (UserManagement.ValidatePassword(user, pass))
                // Password written correctly. Log in.
                return true;
            else
                // Wrong password.
                InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("Wrong password for user."), new InfoBoxSettings()
                {
                    ForegroundColor = KernelColorTools.GetColor(KernelColorType.Error),
                });
            return false;
        }

        private void CleanWidgetsUp()
        {
            if (ModernLogonScreen.screenNum == 2)
                WidgetTools.CleanupWidget(WidgetTools.GetWidgetName(ModernLogonScreen.FirstWidget));
            else if (ModernLogonScreen.screenNum == 3)
                WidgetTools.CleanupWidget(WidgetTools.GetWidgetName(ModernLogonScreen.SecondWidget));
            ModernLogonScreen.headlineStr = "";
        }
    }
}
