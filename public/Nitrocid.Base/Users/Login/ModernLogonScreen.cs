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
using Terminaux.Colors.Themes.Colors;
using Terminaux.Base.Buffered;
using Terminaux.Base;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Base.Extensions;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Time;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Time.Renderers;
using Nitrocid.Base.Kernel.Threading;
using Nitrocid.Base.Users.Login.Widgets;
using Nitrocid.Base.Users.Login.Widgets.Implementations;
using Nitrocid.Base.Users.Login.Motd;

namespace Nitrocid.Base.Users.Login
{
    internal static class ModernLogonScreen
    {
        internal static bool renderedFully = false;
        internal static int screenNum = 1;
        internal readonly static KernelThread updateThread = new("Modern Logon Update Thread", true, ScreenHandler);

        internal static void ScreenHandler()
        {
            // Make a screen
            var screen = new Screen();
            ScreenTools.SetCurrent(screen);

            // Now, do the job
            try
            {
                while (true)
                {
                    try
                    {
                        if (screenNum == 1)
                        {
                            // TODO: Screen number 1, for now, while we introduce Widget Canvas JSON
                            screen.RemoveBufferedParts();
                            var part = new ScreenPart();
                            part.AddDynamicText(() => PrintConfiguredLogonScreen(screenNum));
                            screen.AddBufferedPart($"Modern logon screen {screenNum} update part", part);

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Function under development")]
        internal static string PrintConfiguredLogonScreen(int screenNum)
        {
            // TODO: screenNum is just an accessory argument for now
            var display = new StringBuilder();

            // Clear the console and write the time using figlet
            display.Append(
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

            // Print the MOTD
            string[] motdStrs = ConsoleMisc.GetWrappedSentencesByWords(MotdParse.MotdMessage, ConsoleWrapper.WindowWidth - 4);
            for (int i = 0; i < motdStrs.Length && i < 2; i++)
            {
                string motdStr = motdStrs[i];
                int consoleMotdInfoY = ConsoleWrapper.WindowHeight / 2 - figHeight - 2 + i;
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

            // Print notifications
            var notificationsWidget = WidgetTools.GetWidget(nameof(NotificationIcons));
            display.Append(notificationsWidget.Render(0, consoleInstY - 2, ConsoleWrapper.WindowWidth, 1));

            // Print everything
            return display.ToString();
        }
    }
}
