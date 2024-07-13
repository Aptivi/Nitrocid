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
using Nitrocid.Users;
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
        private static readonly HomepageBinding[] bindings =
        [
            new(/* Localizable */ "Execute", ConsoleKey.Enter),
            new(/* Localizable */ "Logout", ConsoleKey.Escape),
            new(/* Localizable */ "Logout", ConsoleKey.Escape),
        ];

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
                string rssSequence = "";
                ScreenTools.SetCurrent(homeScreen);
                ColorTools.LoadBack();

                // Now, render the homepage
                homeScreenBuffer.AddDynamicText(() =>
                {
                    var builder = new StringBuilder();

                    // Make a master border
                    builder.Append(BorderColor.RenderBorder(0, 3, ConsoleWrapper.WindowWidth - 2, ConsoleWrapper.WindowHeight - 6, KernelColorTools.GetColor(KernelColorType.TuiPaneSelectedSeparator)));

                    // Show username at the top
                    builder.Append(CenteredTextColor.RenderCenteredOneLine(1, Translate.DoTranslation("Hi, {0}! Welcome to Nitrocid!"), Vars: [string.IsNullOrWhiteSpace(UserManagement.CurrentUser.FullName) ? UserManagement.CurrentUser.Username : UserManagement.CurrentUser.FullName]));

                    // Show bindings
                    var bindingsBuilder = new StringBuilder(CsiSequences.GenerateCsiCursorPosition(1, ConsoleWrapper.WindowHeight));
                    foreach (HomepageBinding binding in bindings)
                    {
                        // Check the binding mode
                        if (binding.BindingUsesMouse)
                            continue;

                        // First, check to see if the rendered binding info is going to exceed the console window width
                        string renderedBinding = $"{GetBindingKeyShortcut(binding, false)} {binding.BindingName}  ";
                        int bindingLength = ConsoleChar.EstimateCellWidth(renderedBinding);
                        int actualLength = ConsoleChar.EstimateCellWidth(VtSequenceTools.FilterVTSequences(bindingsBuilder.ToString()));
                        bool canDraw = bindingLength + actualLength < ConsoleWrapper.WindowWidth - 3;
                        bool isBuiltin = !interactiveTui.Bindings.Contains(binding);
                        if (canDraw)
                        {
                            bindingsBuilder.Append(
                                $"{ColorTools.RenderSetConsoleColor(isBuiltin ? InteractiveTuiStatus.KeyBindingBuiltinColor : InteractiveTuiStatus.KeyBindingOptionColor, false, true)}" +
                                $"{ColorTools.RenderSetConsoleColor(isBuiltin ? InteractiveTuiStatus.KeyBindingBuiltinBackgroundColor : InteractiveTuiStatus.OptionBackgroundColor, true)}" +
                                GetBindingKeyShortcut(binding, false) +
                                $"{ColorTools.RenderSetConsoleColor(isBuiltin ? InteractiveTuiStatus.KeyBindingBuiltinForegroundColor : InteractiveTuiStatus.OptionForegroundColor)}" +
                                $"{ColorTools.RenderSetConsoleColor(InteractiveTuiStatus.BackgroundColor, true)}" +
                                $" {binding.BindingName}  "
                            );
                        }
                        else
                        {
                            // We can't render anymore, so just break and write a binding to show more
                            bindingsBuilder.Append(
                                $"{CsiSequences.GenerateCsiCursorPosition(ConsoleWrapper.WindowWidth - 2, ConsoleWrapper.WindowHeight)}" +
                                $"{ColorTools.RenderSetConsoleColor(InteractiveTuiStatus.KeyBindingBuiltinColor, false, true)}" +
                                $"{ColorTools.RenderSetConsoleColor(InteractiveTuiStatus.KeyBindingBuiltinBackgroundColor, true)}" +
                                " K "
                            );
                            break;
                        }
                    }

                    // Make a border for an analog clock widget and the first three RSS feeds (if the addon is installed)
                    int widgetLeft = ConsoleWrapper.WindowWidth / 2 + ConsoleWrapper.WindowWidth % 2;
                    int widgetWidth = ConsoleWrapper.WindowWidth / 2 - 4;
                    int widgetHeight = ConsoleWrapper.WindowHeight - 13;
                    int clockTop = 4;
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
                    if (string.IsNullOrEmpty(rssSequence))
                    {
                        var rssSequenceBuilder = new StringBuilder();
                        rssSequenceBuilder.Append(CsiSequences.GenerateCsiCursorPosition(widgetLeft + 2, rssTop + 2));
                        try
                        {
                            if (!Config.MainConfig.ShowHeadlineOnLogin)
                                rssSequenceBuilder.Append(Translate.DoTranslation("Enable headlines on login to show RSS feeds").Truncate(widgetWidth));
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
                                        rssSequenceBuilder.Append(CsiSequences.GenerateCsiCursorPosition(widgetLeft + 2, rssTop + i + 2));
                                        rssSequenceBuilder.Append(feedRender.Truncate(widgetWidth));
                                    }
                                }
                                rssSequenceBuilder.Append(Translate.DoTranslation("No feed.").Truncate(widgetWidth));
                            }
                        }
                        catch (KernelException ex) when (ex.ExceptionType == KernelExceptionType.AddonManagement)
                        {
                            DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", ex.Message);
                            DebugWriter.WriteDebugStackTrace(ex);
                            rssSequenceBuilder.Append(Translate.DoTranslation("Install the RSS Shell Extras addon!").Truncate(widgetWidth));
                        }
                        catch (Exception ex)
                        {
                            DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", ex.Message);
                            DebugWriter.WriteDebugStackTrace(ex);
                            rssSequenceBuilder.Append(Translate.DoTranslation("Failed to get the latest news.").Truncate(widgetWidth));
                        }
                        rssSequence = rssSequenceBuilder.ToString();
                    }
                    builder.Append(rssSequence);

                    // Populate the settings button
                    int settingsButtonPosX = 2;
                    int settingsButtonPosY = ConsoleWrapper.WindowHeight - 5;
                    int settingsButtonWidth = ConsoleWrapper.WindowWidth / 2 - 5 + ConsoleWrapper.WindowWidth % 2;
                    int settingsButtonHeight = 1;
                    builder.Append(BorderColor.RenderBorder(settingsButtonPosX, settingsButtonPosY, settingsButtonWidth, settingsButtonHeight, KernelColorTools.GetColor(KernelColorType.TuiPaneSeparator)));
                    builder.Append(CenteredTextColor.RenderCenteredOneLine(settingsButtonPosY + 1, Translate.DoTranslation("Settings") + "wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww", KernelColorTools.GetColor(KernelColorType.TuiPaneSeparator), settingsButtonPosX + 1, settingsButtonWidth + settingsButtonPosX + 5 - ConsoleWrapper.WindowWidth % 2));

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
