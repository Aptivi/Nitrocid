
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
using System.Threading;
using KS.Misc.Threading;
using KS.Kernel.Debugging;
using KS.Misc.Probers.Motd;
using KS.Network.RSS.Instance;
using System;
using KS.Network.RSS;

namespace KS.Users.Login
{
    internal static class ModernLogonScreen
    {
        private static readonly KernelThread DateTimeUpdateThread = new("Date and Time Update Thread for Modern Logon", true, DateTimeWidgetUpdater) { isCritical = true };

        internal static void ShowLogon()
        {
            // Clear the console
            ConsoleWrapper.CursorVisible = false;
            ConsoleWrapper.Clear();
            TextWriterColor.Write(Translate.DoTranslation("Loading modern logon... This shouldn't take long."), true, KernelColorType.Progress);

            // Start the date and time update thread to show time and date in the modern way
            DateTimeUpdateThread.Start();

            // Wait for the keypress
            Input.DetectKeypress();

            // Stop the thread
            DateTimeUpdateThread.Stop();

            // Now, clear the console again and prompt for username
            bool loggedIn = false;
            string userName = "";
            while (!loggedIn)
            {
                // First, get the user number from the selection input
                var users = UserManagement.ListAllUsers().ToArray();
                var userFullNames = users.Select(
                    (user) => 
                        UserManagement.GetUserProperty(user, UserManagement.UserProperty.FullName) is not null ? 
                        UserManagement.GetUserProperty(user, UserManagement.UserProperty.FullName).ToString() :
                        ""
                ).ToArray();
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

        private static void DateTimeWidgetUpdater()
        {
            try
            {
                string cachedTimeStr = "";
                while (true)
                {
                    // Print the time
                    string timeStr = TimeDateRenderers.RenderTime(TimeDateTools.FormatType.Short);
                    if (timeStr != cachedTimeStr)
                    {
                        ConsoleWrapper.Clear();
                        cachedTimeStr = TimeDateRenderers.RenderTime(TimeDateTools.FormatType.Short);
                        var figFont = FigletTools.GetFigletFont("Banner3");
                        int figHeight = FigletTools.GetFigletHeight(timeStr, figFont) / 2;
                        CenteredFigletTextColor.WriteCenteredFiglet(figFont, timeStr, KernelColorType.Stage);

                        // Print the date
                        string dateStr = TimeDateRenderers.RenderDate();
                        int consoleInfoY = (ConsoleWrapper.WindowHeight / 2) + figHeight + 2;
                        CenteredTextColor.WriteCentered(consoleInfoY, dateStr);
                        ColorTools.SetConsoleColor(KernelColorType.NeutralText);

                        // Print the date using the alternative calendar, if any
                        if (CalendarTools.EnableAltCalendar)
                        {
                            string dateAltStr = TimeDateRenderers.RenderDate(CalendarTools.GetCultureFromCalendar(CalendarTools.AltCalendar));
                            int consoleAltInfoY = (ConsoleWrapper.WindowHeight / 2) + figHeight + 3;
                            CenteredTextColor.WriteCentered(consoleAltInfoY, dateAltStr);
                        }

                        // Print the headline
                        string headlineStr = "";
                        if (RSSTools.ShowHeadlineOnLogin)
                        {
                            try
                            {
                                var Feed = new RSSFeed(RSSTools.RssHeadlineUrl, RSSFeedType.Infer);
                                if (Feed.FeedArticles.Count > 0)
                                    headlineStr = Translate.DoTranslation("From") + $" {Feed.FeedTitle}: {Feed.FeedArticles[0].ArticleTitle}";
                            }
                            catch (Exception ex)
                            {
                                DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", ex.Message);
                                DebugWriter.WriteDebugStackTrace(ex);
                                headlineStr = Translate.DoTranslation("Failed to get the latest news.");
                            }
                            finally
                            {
                                int consoleHeadlineInfoY = (ConsoleWrapper.WindowHeight / 2) - figHeight - 2;
                                CenteredTextColor.WriteCentered(consoleHeadlineInfoY, headlineStr);
                            }
                        }

                        // Print the MOTD
                        string motdStr = MotdParse.MOTDMessage;
                        int consoleMotdInfoY = (ConsoleWrapper.WindowHeight / 2) - figHeight - 3;
                        CenteredTextColor.WriteCentered(consoleMotdInfoY, motdStr);

                        // Print the instructions
                        string instStr = Translate.DoTranslation("Press any key to start...");
                        int consoleInstY = ConsoleWrapper.WindowHeight - 2;
                        CenteredTextColor.WriteCentered(consoleInstY, instStr);
                    }

                    // Wait for 1 second
                    Thread.Sleep(1000);
                }
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "User pressed a key to exit the date and time update thread for modern logon. Proceeding...");
            }
        }
    }
}
