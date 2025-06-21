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

using System.Threading;
using System;
using Textify.Data.Figlet;
using System.Text;
using Terminaux.Sequences.Builder.Types;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Time;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Base.Buffered;
using Nitrocid.Kernel.Extensions;
using Textify.General;
using Terminaux.Base;
using Nitrocid.Users.Login.Motd;
using Nitrocid.Users.Login.Widgets;
using Nitrocid.Users.Login.Widgets.Implementations;
using Nitrocid.Kernel;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Base.Extensions;

namespace Nitrocid.Users.Login
{
    internal static class ModernLogonScreen
    {
        internal static bool renderedFully = false;
        internal static int screenNum = 1;
        internal readonly static KernelThread updateThread = new("Modern Logon Update Thread", true, ScreenHandler);
        internal static bool enableWidgets = true;
        internal static string firstWidgetName = nameof(AnalogClock);
        internal static string secondWidgetName = nameof(DigitalClock);

        internal static BaseWidget FirstWidget =>
            WidgetTools.CheckWidget(firstWidgetName) ?
            WidgetTools.GetWidget(firstWidgetName) :
            WidgetTools.GetWidget(nameof(AnalogClock));

        internal static BaseWidget SecondWidget =>
            WidgetTools.CheckWidget(secondWidgetName) ?
            WidgetTools.GetWidget(secondWidgetName) :
            WidgetTools.GetWidget(nameof(DigitalClock));

        internal static void ScreenHandler()
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
                        var addonType = InterAddonTools.GetTypeFromAddon(KnownAddons.ExtrasRssShell, "Nitrocid.Extras.RssShell.Tools.RSSShellTools");
                        var Feed = InterAddonTools.ExecuteCustomAddonFunction(KnownAddons.ExtrasRssShell, "GetFirstArticle", addonType, Config.MainConfig.RssHeadlineUrl);
                        if (Feed is (string feedTitle, string articleTitle))
                            return LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_RSSFEED_FROM") + $" {feedTitle}: {articleTitle}";
                        return LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_RSSFEED_NOFEED");
                    }
                    catch (KernelException ex) when (ex.ExceptionType == KernelExceptionType.AddonManagement)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", vars: [ex.Message]);
                        DebugWriter.WriteDebugStackTrace(ex);
                        return LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_RSSFEED_NEEDSADDON");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", vars: [ex.Message]);
                        DebugWriter.WriteDebugStackTrace(ex);
                        return LanguageTools.GetLocalized("NKS_NETWORK_TYPES_RSS_FETCHFAILED");
                    }
                }

                string headlineStr = "";
                while (true)
                {
                    try
                    {
                        if (screenNum == 1)
                        {
                            // Main screen. Print the time.
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
                                    int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                                    var timeFiglet = new AlignedFigletText(figFont)
                                    {
                                        Top = consoleY,
                                        Text = timeStr,
                                        ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Stage),
                                        Settings = new()
                                        {
                                            Alignment = TextAlignment.Middle,
                                        }
                                    };
                                    display.Append(timeFiglet.Render());

                                    // Print the date
                                    string dateStr = $"{TimeDateRenderers.RenderDate()}";
                                    int consoleInfoY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
                                    var dateText = new AlignedText()
                                    {
                                        Top = consoleInfoY,
                                        Text = dateStr,
                                        ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Stage),
                                        OneLine = true,
                                        Settings = new()
                                        {
                                            Alignment = TextAlignment.Middle,
                                        }
                                    };
                                    display.Append(dateText.Render());

                                    // Print the headline
                                    if (Config.MainConfig.ShowHeadlineOnLogin)
                                    {
                                        if (string.IsNullOrEmpty(headlineStr))
                                            headlineStr = UpdateHeadline();
                                        int consoleHeadlineInfoY =
                                            Config.MainConfig.MotdHeadlineBottom ?
                                            ConsoleWrapper.WindowHeight / 2 + figHeight + 3 :
                                            ConsoleWrapper.WindowHeight / 2 - figHeight - 2;
                                        var headlineText = new AlignedText()
                                        {
                                            Top = consoleHeadlineInfoY,
                                            Text = headlineStr,
                                            ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText),
                                            OneLine = true,
                                            Settings = new()
                                            {
                                                Alignment = TextAlignment.Middle,
                                            }
                                        };
                                        display.Append(headlineText.Render());
                                    }

                                    // Print the MOTD
                                    string[] motdStrs = ConsoleMisc.GetWrappedSentencesByWords(MotdParse.MotdMessage, ConsoleWrapper.WindowWidth - 4);
                                    for (int i = 0; i < motdStrs.Length && i < 2; i++)
                                    {
                                        string motdStr = motdStrs[i];
                                        int consoleMotdInfoY =
                                            Config.MainConfig.MotdHeadlineBottom ?
                                            ConsoleWrapper.WindowHeight / 2 + figHeight + 4 + i :
                                            ConsoleWrapper.WindowHeight / 2 - figHeight - (Config.MainConfig.ShowHeadlineOnLogin ? 4 : 2) + i;
                                        var motdText = new AlignedText()
                                        {
                                            Top = consoleMotdInfoY,
                                            Text = motdStr,
                                            ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText),
                                            OneLine = true,
                                            Settings = new()
                                            {
                                                Alignment = TextAlignment.Middle,
                                            }
                                        };
                                        display.Append(motdText.Render());
                                    }

                                    // Print the instructions
                                    string instStr = LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_PRESSKEY");
                                    int consoleInstY = ConsoleWrapper.WindowHeight - 2;
                                    var instText = new AlignedText()
                                    {
                                        Top = consoleInstY,
                                        Text = instStr,
                                        ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText),
                                        OneLine = true,
                                        Settings = new()
                                        {
                                            Alignment = TextAlignment.Middle,
                                        }
                                    };
                                    display.Append(instText.Render());

                                    // Print everything
                                    return display.ToString();
                                });
                                screen.AddBufferedPart("Date/time widget updater", part);

                                // Render it now
                                ScreenTools.Render();
                            }
                        }
                        else if (screenNum == 2)
                        {
                            // Place for first widget
                            screen.RemoveBufferedParts();
                            var part = new ScreenPart();
                            if (!renderedFully)
                                part.AddDynamicText(FirstWidget.Initialize);
                            part.AddDynamicText(FirstWidget.Render);
                            screen.AddBufferedPart("Widget 1 updater", part);

                            // Render it now
                            ScreenTools.Render();
                        }
                        else if (screenNum == 3)
                        {
                            // Place for second widget
                            screen.RemoveBufferedParts();
                            var part = new ScreenPart();
                            if (!renderedFully)
                                part.AddDynamicText(SecondWidget.Initialize);
                            part.AddDynamicText(SecondWidget.Render);
                            screen.AddBufferedPart("Widget 2 updater", part);

                            // Render it now
                            ScreenTools.Render();
                        }
                        else
                        {
                            // Unknown screen!
                            screen.RemoveBufferedParts();
                            var part = new ScreenPart();
                            part.AddDynamicText(() =>
                            {
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
                            });
                            screen.AddBufferedPart("Unknown widget updater", part);

                            // Render it now
                            ScreenTools.Render();
                        }
                    }
                    catch (Exception ex) when (ex is not ThreadInterruptedException)
                    {
                        // An error occurred!
                        screen.RemoveBufferedParts();
                        var part = new ScreenPart();
                        part.AddDynamicText(() =>
                        {
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
                        });
                        DebugWriter.WriteDebug(DebugLevel.E, $"Error rendering the modern logon: {ex.Message}");
                        DebugWriter.WriteDebugStackTrace(ex);
                        screen.AddBufferedPart("Error updater", part);

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
