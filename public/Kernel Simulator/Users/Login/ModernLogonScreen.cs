
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters.Tools;
using KS.Misc.Writers.FancyWriters;
using KS.ConsoleBase.Colors;
using KS.TimeDate;
using KS.ConsoleBase.Inputs;
using KS.Languages;
using KS.ConsoleBase.Inputs.Styles;
using KS.Drivers.Encryption;
using System.Linq;
using KS.Misc.Calendar;

namespace KS.Users.Login
{
    internal static class ModernLogonScreen
    {
        internal static void ShowLogon()
        {
            // Clear the console
            ConsoleWrapper.CursorVisible = false;
            ConsoleWrapper.Clear();

            // Show the date and the time in the modern way
            // TODO: update this while waiting for input at the same time
            string timeStr = TimeDateRenderers.RenderTime(TimeDate.TimeDate.FormatType.Short);
            var figFont = FigletTools.GetFigletFont("Banner3");
            int figWidth = FigletTools.GetFigletWidth(timeStr, figFont) / 2;
            int figHeight = FigletTools.GetFigletHeight(timeStr, figFont) / 2;
            int consoleX = (ConsoleWrapper.WindowWidth / 2) - figWidth;
            int consoleY = (ConsoleWrapper.WindowHeight / 2) - figHeight;
            FigletWhereColor.WriteFigletWhere(timeStr, consoleX, consoleY, true, figFont, KernelColorType.Stage);

            // Print the date
            string dateStr = TimeDateRenderers.RenderDate();
            int consoleInfoX = (ConsoleWrapper.WindowWidth / 2) - (dateStr.Length / 2);
            int consoleInfoY = (ConsoleWrapper.WindowHeight / 2) + figHeight + 2;
            TextWriterWhereColor.WriteWhere(dateStr, consoleInfoX, consoleInfoY);

            // Print the date using the alternative calendar, if any
            if (CalendarTools.EnableAltCalendar)
            {
                string dateAltStr = TimeDateRenderers.RenderDate(CalendarTools.GetCultureFromCalendar(CalendarTools.AltCalendar));
                int consoleAltInfoX = (ConsoleWrapper.WindowWidth / 2) - (dateAltStr.Length / 2);
                int consoleAltInfoY = (ConsoleWrapper.WindowHeight / 2) + figHeight + 3;
                TextWriterWhereColor.WriteWhere(dateAltStr, consoleAltInfoX, consoleAltInfoY);
            }

            // Print the instructions
            string instStr = Translate.DoTranslation("Press any key to start...");
            int consoleInstX = (ConsoleWrapper.WindowWidth / 2) - (dateStr.Length / 2);
            int consoleInstY = ConsoleWrapper.WindowHeight - 2;
            TextWriterWhereColor.WriteWhere(instStr, consoleInstX, consoleInstY);

            // Wait for the keypress
            Input.DetectKeypress();

            // Now, clear the console again and prompt for username
            bool loggedIn = false;
            string userName = "";
            while (!loggedIn)
            {
                // First, get the user number from the selection input
                var users = UserManagement.ListAllUsers().ToArray();
                var userFullNames = users.Select((user) => UserManagement.GetUserProperty(user, UserManagement.UserProperty.FullName).ToString()).ToArray();
                int userNum = SelectionStyle.PromptSelection(Translate.DoTranslation("Select a user account you want to log in with."), string.Join("/", users), userFullNames);

                // Then, get the user from the number and prompt for password if found
                userName = UserManagement.SelectUser(userNum);
                if (Login.Users[userName].Password != Encryption.GetEmptyHash("SHA256"))
                {
                    // The password is not empty. Prompt for password.
                    TextWriterColor.Write(Translate.DoTranslation("Enter the password for user") + " {0}: ", false, userName);
                    string pass = Input.ReadLineNoInput();

                    // Validate the password
                    if (UserManagement.ValidatePassword(userName, pass))
                        // Password written correctly. Log in.
                        loggedIn = true;
                    else
                        // Wrong password.
                        TextWriterColor.Write(Translate.DoTranslation("Wrong password for user."), KernelColorType.Error);
                }
                else
                    loggedIn = true;
            }

            // Finally, launch the shell
            ConsoleWrapper.Clear();
            Login.SignIn(userName);
        }
    }
}
