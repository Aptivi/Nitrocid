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
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Languages;
using Nitrocid.Network.Types.RSS;
using Nitrocid.Users.Login.Widgets;
using Nitrocid.Users.Login.Widgets.Implementations;
using System;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Reader;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.FancyWriters;

namespace Nitrocid.Shell.Homepage
{
    /// <summary>
    /// The Nitrocid Homepage tools
    /// </summary>
    public static class HomepageTools
    {
        internal static bool isHomepageEnabled = false;
        private static bool isOnHomepage = false;

        /// <summary>
        /// Opens The Nitrocid Homepage
        /// </summary>
        public static void OpenHomepage()
        {
            if (isOnHomepage || isHomepageEnabled)
                return;
            isOnHomepage = true;
            var homeScreen = new Screen();

            try
            {
                // Create a screen for the homepage
                var homeScreenBuffer = new ScreenPart();
                bool rssRendered = false;
                ScreenTools.SetCurrent(homeScreen);
                ColorTools.LoadBack();

                // Now, render the homepage
                homeScreenBuffer.AddDynamicText(() =>
                {
                    var builder = new StringBuilder();

                    // Make a master border
                    builder.Append(BorderColor.RenderBorder(0, 1, ConsoleWrapper.WindowWidth - 2, ConsoleWrapper.WindowHeight - 4, KernelColorTools.GetColor(KernelColorType.TuiPaneSelectedSeparator)));

                    // Make a border for an analog clock widget and the first three RSS feeds (if the addon is installed)
                    int widgetLeft = ConsoleWrapper.WindowWidth / 2 + 1;
                    int widgetWidth = ConsoleWrapper.WindowWidth / 2 - 4;
                    int widgetHeight = ConsoleWrapper.WindowHeight - 11;
                    int clockTop = 2;
                    int rssTop = clockTop + widgetHeight + 2;
                    int rssHeight = 3;
                    builder.Append(BorderColor.RenderBorder(widgetLeft, clockTop, widgetWidth, widgetHeight, KernelColorTools.GetColor(KernelColorType.TuiPaneSelectedSeparator)));
                    builder.Append(BorderColor.RenderBorder(widgetLeft, rssTop, widgetWidth, rssHeight, KernelColorTools.GetColor(KernelColorType.TuiPaneSelectedSeparator)));

                    // Render the clock widget
                    var widget = WidgetTools.GetWidget(nameof(AnalogClock));
                    string widgetInit = widget.Initialize(widgetLeft + 1, clockTop + 1, widgetWidth, widgetHeight);
                    string widgetSeq = widget.Render(widgetLeft + 1, clockTop + 1, widgetWidth, widgetHeight);
                    builder.Append(widgetInit + widgetSeq);

                    // Render the first three RSS feeds
                    if (!rssRendered)
                    {
                        rssRendered = true;
                        builder.Append(CsiSequences.GenerateCsiCursorPosition(widgetLeft + 2, rssTop + 2));
                        try
                        {
                            if (!Config.MainConfig.ShowHeadlineOnLogin)
                                builder.Append(Translate.DoTranslation("Enable headlines on login to show RSS feeds").Truncate(widgetWidth));
                            else
                            {
                                var feedsObject = InterAddonTools.ExecuteCustomAddonFunction(KnownAddons.ExtrasRssShell, "GetArticles", RSSTools.RssHeadlineUrl);
                                if (feedsObject is (string feedTitle, string articleTitle)[] feeds)
                                {
                                    for (int i = 0; i < 3; i++)
                                    {
                                        if (i >= feeds.Length)
                                            break;
                                        (string feedTitle, string articleTitle) = feeds[i];
                                        string feedRender = Translate.DoTranslation("From") + $" {feedTitle}: {articleTitle}";
                                        builder.Append(CsiSequences.GenerateCsiCursorPosition(widgetLeft + 2, rssTop + i + 2));
                                        builder.Append(feedRender.Truncate(widgetWidth));
                                    }
                                }
                                builder.Append(Translate.DoTranslation("No feed.").Truncate(widgetWidth));
                            }
                        }
                        catch (KernelException ex) when (ex.ExceptionType == KernelExceptionType.AddonManagement)
                        {
                            DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", ex.Message);
                            DebugWriter.WriteDebugStackTrace(ex);
                            builder.Append(Translate.DoTranslation("Install the RSS Shell Extras addon!").Truncate(widgetWidth));
                        }
                        catch (Exception ex)
                        {
                            DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", ex.Message);
                            DebugWriter.WriteDebugStackTrace(ex);
                            builder.Append(Translate.DoTranslation("Failed to get the latest news.").Truncate(widgetWidth));
                        }
                    }

                    // Return the resulting homepage
                    return builder.ToString();
                });
                homeScreen.AddBufferedPart("The Nitrocid Homepage", homeScreenBuffer);

                // Render the thing and wait for a keypress
                bool exiting = false;
                while (!exiting)
                {
                    ScreenTools.Render();
                    var key = TermReader.ReadKey();
                    if (key.Key == ConsoleKey.Enter)
                        exiting = true;
                }
            }
            catch (Exception ex)
            {
                ColorTools.LoadBack();
                InfoBoxColor.WriteInfoBoxColor(Translate.DoTranslation("The Nitrocid Homepage has crashed and needs to revert back to the shell.") + $": {ex.Message}", true, KernelColorTools.GetColor(KernelColorType.Error));
            }
            finally
            {
                isOnHomepage = false;
                ScreenTools.UnsetCurrent(homeScreen);
                ColorTools.LoadBack();
            }
        }
    }
}
