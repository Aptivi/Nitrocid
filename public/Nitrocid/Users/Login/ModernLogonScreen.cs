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
using System.Text;
using System.Threading;
using System.Timers;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Kernel.Threading;
using Nitrocid.Kernel.Time;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Languages;
using Nitrocid.Users.Login.Motd;
using Nitrocid.Users.Login.Widgets;
using Nitrocid.Users.Login.Widgets.Implementations;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Base.Extensions;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.Data.Figlet;
using Textify.General;

namespace Nitrocid.Users.Login
{
    internal static class ModernLogonScreen
    {
        internal static int screenNum = 1;
        internal static bool enableWidgets = true;
        internal static string firstWidgetName = nameof(AnalogClock);
        internal static string secondWidgetName = nameof(DigitalClock);
        internal static string headlineStr = "";

        internal static BaseWidget FirstWidget =>
            WidgetTools.CheckWidget(firstWidgetName) ?
            WidgetTools.GetWidget(firstWidgetName) :
            WidgetTools.GetWidget(nameof(AnalogClock));

        internal static BaseWidget SecondWidget =>
            WidgetTools.CheckWidget(secondWidgetName) ?
            WidgetTools.GetWidget(secondWidgetName) :
            WidgetTools.GetWidget(nameof(DigitalClock));

        internal static string PrintConfiguredLogonScreen(int screenNum)
        {
            var builder = new StringBuilder();

            // Clear the console
            if (ConsoleResizeHandler.WasResized() || (ScreenTools.CurrentScreen?.RefreshWasDone ?? false))
                builder.Append(ConsoleClearing.GetClearWholeScreenSequence());

            // Check the screen
            if (screenNum == 1)
            {
                var display = new StringBuilder();

                // Clear the console and write the time using figlet
                builder.Append(
                    CsiSequences.GenerateCsiCursorPosition(1, 1) +
                    CsiSequences.GenerateCsiEraseInDisplay(0)
                );
                string timeStr = TimeDateRenderers.RenderTime(FormatType.Short);
                var figFont = FigletTools.GetFigletFont(Config.MainConfig.DefaultFigletFontName);
                int figHeight = FigletTools.GetFigletHeight(timeStr, figFont) / 2;
                int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                var timeFiglet = new AlignedFigletText(figFont)
                {
                    Top = consoleY,
                    Text = timeStr,
                    ForegroundColor = KernelColorTools.GetColor(KernelColorType.Stage),
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle,
                    }
                };
                builder.Append(timeFiglet.Render());

                // Print the date
                string dateStr = $"{TimeDateRenderers.RenderDate()}";
                int consoleInfoY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
                var dateText = new AlignedText()
                {
                    Top = consoleInfoY,
                    Text = dateStr,
                    ForegroundColor = KernelColorTools.GetColor(KernelColorType.Stage),
                    OneLine = true,
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle,
                    }
                };
                builder.Append(dateText.Render());

                // Print the headline
                if (Config.MainConfig.ShowHeadlineOnLogin)
                {
                    // Helper function to get the latest headline
                    static string UpdateHeadline()
                    {
                        try
                        {
                            var addonType = InterAddonTools.GetTypeFromAddon(KnownAddons.ExtrasRssShell, "Nitrocid.Extras.RssShell.Tools.RSSShellTools");
                            var Feed = InterAddonTools.ExecuteCustomAddonFunction(KnownAddons.ExtrasRssShell, "GetFirstArticle", addonType, Config.MainConfig.RssHeadlineUrl);
                            if (Feed is (string feedTitle, string articleTitle))
                                return Translate.DoTranslation("From") + $" {feedTitle}: {articleTitle}";
                            return Translate.DoTranslation("No feed.");
                        }
                        catch (KernelException ex) when (ex.ExceptionType == KernelExceptionType.AddonManagement)
                        {
                            DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", vars: [ex.Message]);
                            DebugWriter.WriteDebugStackTrace(ex);
                            return Translate.DoTranslation("Install the RSS Shell Extras addon!");
                        }
                        catch (Exception ex)
                        {
                            DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", vars: [ex.Message]);
                            DebugWriter.WriteDebugStackTrace(ex);
                            return Translate.DoTranslation("Failed to get the latest news.");
                        }
                    }

                    // Render the headline after updating it
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
                        ForegroundColor = KernelColorTools.GetColor(KernelColorType.NeutralText),
                        OneLine = true,
                        Settings = new()
                        {
                            Alignment = TextAlignment.Middle,
                        }
                    };
                    builder.Append(headlineText.Render());
                }

                // Print the MOTD
                string[] motdStrs = TextTools.GetWrappedSentences(MotdParse.MotdMessage, ConsoleWrapper.WindowWidth - 4);
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
                        ForegroundColor = KernelColorTools.GetColor(KernelColorType.NeutralText),
                        OneLine = true,
                        Settings = new()
                        {
                            Alignment = TextAlignment.Middle,
                        }
                    };
                    builder.Append(motdText.Render());
                }

                // Print the instructions
                string instStr = Translate.DoTranslation("Press any key to start, or ESC for more options...");
                int consoleInstY = ConsoleWrapper.WindowHeight - 2;
                var instText = new AlignedText()
                {
                    Top = consoleInstY,
                    Text = instStr,
                    ForegroundColor = KernelColorTools.GetColor(KernelColorType.NeutralText),
                    OneLine = true,
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle,
                    }
                };
                builder.Append(instText.Render());
            }
            else if (screenNum == 2)
            {
                // Place for first widget
                if (ConsoleResizeHandler.WasResized() || (ScreenTools.CurrentScreen?.RefreshWasDone ?? false))
                    builder.Append(FirstWidget.Initialize());
                builder.Append(FirstWidget.Render());
            }
            else if (screenNum == 3)
            {
                // Place for second widget
                if (ConsoleResizeHandler.WasResized() || (ScreenTools.CurrentScreen?.RefreshWasDone ?? false))
                    builder.Append(SecondWidget.Initialize());
                builder.Append(SecondWidget.Render());
            }
            else
            {
                // Unknown screen!
                var errorText = new AlignedText()
                {
                    Text = Translate.DoTranslation("Unknown screen number."),
                    ForegroundColor = KernelColorTools.GetColor(KernelColorType.Error),
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle,
                    }
                };
                builder.Append(errorText.Render());
            }
            return builder.ToString();
        }
    }
}
