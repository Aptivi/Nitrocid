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

using System.Threading;
using System;
using Figletize;
using System.Text;
using Textify.Sequences.Builder.Types;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Time;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Terminaux.Writer.FancyWriters;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Base.Buffered;
using Nitrocid.Kernel.Extensions;
using Textify.General;
using Terminaux.Base;
using Nitrocid.Users.Login.Motd;
using Nitrocid.Network.Types.RSS;

namespace Nitrocid.Users.Login
{
    internal static class ModernLogonScreen
    {
        internal static bool renderedFully = false;
        internal readonly static KernelThread DateTimeUpdateThread = new("Modern Logon Update Thread", true, DateTimeWidgetUpdater);

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
                        var Feed = InterAddonTools.ExecuteCustomAddonFunction(KnownAddons.ExtrasRssShell, "GetFirstArticle", RSSTools.RssHeadlineUrl);
                        if (Feed is (string feedTitle, string articleTitle))
                            return Translate.DoTranslation("From") + $" {feedTitle}: {articleTitle}";
                        return Translate.DoTranslation("No feed.");
                    }
                    catch (KernelException ex) when (ex.ExceptionType == KernelExceptionType.AddonManagement)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", ex.Message);
                        DebugWriter.WriteDebugStackTrace(ex);
                        return Translate.DoTranslation("Install the RSS Shell Extras addon!");
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
                            display.Append(
                                CsiSequences.GenerateCsiCursorPosition(1, 1) +
                                CsiSequences.GenerateCsiEraseInDisplay(0)
                            );
                            cachedTimeStr = TimeDateRenderers.RenderTime(FormatType.Short);
                            var figFont = FigletTools.GetFigletFont(Config.MainConfig.DefaultFigletFontName);
                            int figHeight = FigletTools.GetFigletHeight(timeStr, figFont) / 2;
                            display.Append(
                                KernelColorTools.GetColor(KernelColorType.Stage).VTSequenceForeground +
                                CenteredFigletTextColor.RenderCenteredFiglet(figFont, timeStr)
                            );

                            // Print the date
                            string dateStr = $"{TimeDateRenderers.RenderDate()}";
                            int consoleInfoY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
                            display.Append(
                                CenteredTextColor.RenderCenteredOneLine(consoleInfoY, dateStr) +
                                KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground
                            );

                            // Print the headline
                            if (RSSTools.ShowHeadlineOnLogin)
                            {
                                int consoleHeadlineInfoY =
                                    MotdHeadlineBottom ?
                                    ConsoleWrapper.WindowHeight / 2 + figHeight + 3 :
                                    ConsoleWrapper.WindowHeight / 2 - figHeight - 2;
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
                                    ConsoleWrapper.WindowHeight / 2 + figHeight + 4 + i :
                                    ConsoleWrapper.WindowHeight / 2 - figHeight - (RSSTools.ShowHeadlineOnLogin ? 4 : 2) + i;
                                display.Append(
                                    CenteredTextColor.RenderCenteredOneLine(consoleMotdInfoY, motdStr)
                                );
                            }

                            // Print the instructions
                            string instStr = Translate.DoTranslation("Press any key to start, or ESC for more options...");
                            int consoleInstY = ConsoleWrapper.WindowHeight - 2;
                            display.Append(
                                CenteredTextColor.RenderCenteredOneLine(consoleInstY, instStr)
                            );

                            // Print everything
                            return display.ToString();
                        });
                        screen.AddBufferedPart("Date/time widget updater", part);

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
