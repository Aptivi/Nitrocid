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
using KS.Languages;
using System.Threading;
using KS.Kernel.Debugging;
using Syndian.Instance;
using System;
using KS.Network.RSS;
using KS.Kernel.Configuration;
using KS.Kernel.Time;
using KS.Kernel.Time.Renderers;
using KS.Kernel.Threading;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Misc.Text;
using Figletize;
using KS.Misc.Text.Probers.Motd;
using System.Text;
using KS.ConsoleBase.Buffered;
using Terminaux.Sequences.Builder.Types;

namespace KS.Users.Login
{
    internal static class ModernLogonScreen
    {
        internal static bool renderedFully = false;
        internal readonly static KernelThread DateTimeUpdateThread = new("Date and Time Update Thread for Modern Logon", true, DateTimeWidgetUpdater);

        /// <summary>
        /// Whether to show the MOTD and the headline at the bottom or at the top of the clock
        /// </summary>
        public static bool MotdHeadlineBottom =>
            Config.MainConfig.MotdHeadlineBottom;

        internal static void DateTimeWidgetUpdater()
        {
            // Make a screen
            var screen = new Screen();
            ScreenTools.SetCurrent(screen);

            // Now, do the job
            try
            {
                string cachedTimeStr = "";

                // First, get the headline
                static string UpdateHeadline()
                {
                    try
                    {
                        if (!RSSTools.ShowHeadlineOnLogin)
                            return "";
                        var Feed = new RSSFeed(RSSTools.RssHeadlineUrl, RSSFeedType.Infer);
                        if (Feed.FeedArticles.Count > 0)
                            return Translate.DoTranslation("From") + $" {Feed.FeedTitle}: {Feed.FeedArticles[0].ArticleTitle}";
                        return Translate.DoTranslation("No feed.");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", ex.Message);
                        DebugWriter.WriteDebugStackTrace(ex);
                        return Translate.DoTranslation("Failed to get the latest news.");
                    }
                }
                string headlineStr = UpdateHeadline();
                while (true)
                {
                    // Print the time
                    string timeStr = TimeDateRenderers.RenderTime(FormatType.Short);
                    if (timeStr != cachedTimeStr)
                    {
                        screen.RemoveBufferedParts();
                        var part = new ScreenPart();
                        part.AddDynamicText(() =>
                        {
                            var display = new StringBuilder();

                            // Clear the console and write the time using figlet
                            display.Append(CsiSequences.GenerateCsiEraseInDisplay(2));
                            cachedTimeStr = TimeDateRenderers.RenderTime(FormatType.Short);
                            var figFont = FigletTools.GetFigletFont(TextTools.DefaultFigletFontName);
                            int figHeight = FigletTools.GetFigletHeight(timeStr, figFont) / 2;
                            display.Append(
                                KernelColorTools.GetColor(KernelColorType.Stage).VTSequenceForeground +
                                CenteredFigletTextColor.RenderCenteredFiglet(figFont, timeStr)
                            );

                            // Print the date
                            string dateStr = $"{TimeDateRenderers.RenderDate()}";
                            int consoleInfoY = (ConsoleWrapper.WindowHeight / 2) + figHeight + 2;
                            display.Append(
                                CenteredTextColor.RenderCenteredOneLine(consoleInfoY, dateStr) +
                                KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground
                            );

                            // Print the headline
                            if (RSSTools.ShowHeadlineOnLogin)
                            {
                                int consoleHeadlineInfoY =
                                    MotdHeadlineBottom ?
                                    (ConsoleWrapper.WindowHeight / 2) + figHeight + 3 :
                                    (ConsoleWrapper.WindowHeight / 2) - figHeight - 2;
                                display.Append(
                                    CenteredTextColor.RenderCenteredOneLine(consoleHeadlineInfoY, headlineStr)
                                );
                            }

                            // Print the MOTD
                            string[] motdStrs = TextTools.GetWrappedSentences(MotdParse.MotdMessage, ConsoleWrapper.WindowWidth - 4);
                            for (int i = 0; i < motdStrs.Length && i < 2; i++)
                            {
                                string motdStr = motdStrs[i];
                                int consoleMotdInfoY =
                                    MotdHeadlineBottom ? 
                                    (ConsoleWrapper.WindowHeight / 2) + figHeight + 4 + i :
                                    (ConsoleWrapper.WindowHeight / 2) - figHeight - (RSSTools.ShowHeadlineOnLogin ? 5 : 3) + i;
                                display.Append(
                                    CenteredTextColor.RenderCenteredOneLine(consoleMotdInfoY, motdStr)
                                );
                            }

                            // Print the instructions
                            string instStr = Translate.DoTranslation("Press any key to start...");
                            int consoleInstY = ConsoleWrapper.WindowHeight - 2;
                            display.Append(
                                CenteredTextColor.RenderCenteredOneLine(consoleInstY, instStr)
                            );

                            // Print everything
                            return display.ToString();
                        });
                        screen.AddBufferedPart(part);

                        // Render it now
                        ScreenTools.Render();
                    }

                    // Wait for 1 second
                    renderedFully = true;
                    Thread.Sleep(1000);
                }
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "User pressed a key to exit the date and time update thread for modern logon. Proceeding...");
            }
            ScreenTools.UnsetCurrent(screen);
        }
    }
}
