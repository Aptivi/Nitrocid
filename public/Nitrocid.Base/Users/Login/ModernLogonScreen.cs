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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Nitrocid.Base.Files;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Threading;
using Nitrocid.Base.Kernel.Time;
using Nitrocid.Base.Kernel.Time.Renderers;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Users.Login.Motd;
using Nitrocid.Base.Misc.Widgets;
using Nitrocid.Base.Misc.Widgets.Canvas;
using Nitrocid.Base.Misc.Widgets.Implementations;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Base.Extensions;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.Data.Figlet;

namespace Nitrocid.Base.Users.Login
{
    internal static class ModernLogonScreen
    {
        internal static bool renderedFully = false;
        internal static int screenNum = 1;
        internal static List<WidgetRenderInfo[]> canvases = [];
        internal readonly static KernelThread updateThread = new("Modern Logon Update Thread", true, ScreenHandler);

        internal static void ScreenHandler()
        {
            // Make a screen
            var screen = new Screen();
            ScreenTools.SetCurrent(screen);

            // Now, do the job
            try
            {
                // Initialize the saved widget canvases
                canvases = GetLogonPages();
                int maxLogonScreens = canvases.Count + 1;

                // Main screen loop
                while (true)
                {
                    try
                    {
                        if (screenNum > 0 && screenNum <= maxLogonScreens)
                        {
                            screen.RemoveBufferedParts();
                            var part = new ScreenPart();
                            part.AddDynamicText(() => PrintConfiguredLogonScreen(screenNum, canvases));
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

        internal static string PrintConfiguredLogonScreen(int screenNum, List<WidgetRenderInfo[]> canvases)
        {
            int actualScreenNum = screenNum - 2;
            var builder = new StringBuilder();

            // Clear the console
            builder.Append(ConsoleClearing.GetClearWholeScreenSequence());

            // Check the screen
            if (actualScreenNum < 0)
            {
                // Write an infobox border
                int height = ConsoleWrapper.WindowHeight - 4;
                int width = ConsoleWrapper.WindowWidth - 8;
                int posX = ConsoleWrapper.WindowWidth / 2 - width / 2 - 1;
                int posY = ConsoleWrapper.WindowHeight / 2 - height / 2 - 1;
                string versionStr = $"{KernelReleaseInfo.ApiVersion}";
                var border = new BoxFrame()
                {
                    Left = posX,
                    Top = posY,
                    Width = width,
                    Height = height,
                    UseColors = true,
                    Text = versionStr,
                };
                builder.Append(border.Render());

                // Write the program name
                int interiorPosX = posX + 3;
                int interiorWidth = width - 6;
                string text = $"Nitrocid KS {KernelReleaseInfo.VersionFullStr}";
                var figFont = FigletTools.GetFigletFont("thin");
                int figHeight = FigletTools.GetFigletHeight(text, figFont) / 2;
                int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight - 2;
                var nameText = new AlignedFigletText(figFont)
                {
                    Left = interiorPosX,
                    Top = consoleY,
                    Width = interiorWidth,
                    UseColors = true,
                    ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Banner),
                    OneLine = true,
                    Text = text,
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle,
                    }
                };
                builder.Append(nameText.Render());

                // Print the time and date
                string timeStr = TimeDateRenderers.RenderTime();
                string dateStr = TimeDateRenderers.RenderDate();
                int consoleInfoY = ConsoleWrapper.WindowHeight / 2 + figHeight - 1;
                var dateText = new AlignedText()
                {
                    Left = interiorPosX,
                    Top = consoleInfoY,
                    Width = interiorWidth,
                    Text = $"{dateStr} {timeStr}",
                    OneLine = true,
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle,
                    }
                };
                builder.Append(dateText.Render());

                // Print the MOTD
                string[] motdStrs = ConsoleMisc.GetWrappedSentencesByWords(MotdParse.MotdMessage, ConsoleWrapper.WindowWidth - 4);
                for (int i = 0; i < motdStrs.Length && i < 2; i++)
                {
                    string motdStr = motdStrs[i];
                    int consoleMotdInfoY = ConsoleWrapper.WindowHeight / 2 + figHeight + 1 + i;
                    var motdText = new AlignedText()
                    {
                        Left = interiorPosX,
                        Top = consoleMotdInfoY,
                        Width = interiorWidth,
                        Text = motdStr,
                        OneLine = true,
                        Settings = new()
                        {
                            Alignment = TextAlignment.Middle,
                        }
                    };
                    builder.Append(motdText.Render());
                }

                // Print notifications area
                int notificationsY = ConsoleWrapper.WindowHeight / 2 - figHeight - 4;
                var notificationsWidget = WidgetTools.GetWidget(nameof(NotificationIcons));
                builder.Append(notificationsWidget.Render(0, notificationsY, interiorWidth, 1));

                // Print the instructions
                string instStr = LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_PRESSKEY");
                int consoleInstY = ConsoleWrapper.WindowHeight - 4;
                var instText = new AlignedText()
                {
                    Left = interiorPosX,
                    Top = consoleInstY,
                    Width = interiorWidth,
                    Text = instStr,
                    OneLine = true,
                };
                builder.Append(instText.Render());
            }
            else
            {
                var canvas = canvases[actualScreenNum];
                builder.Append(WidgetCanvasTools.RenderFromInfos(canvas));
            }
            return builder.ToString();
        }

        internal static List<WidgetRenderInfo[]> GetLogonPages()
        {
            // File path to the logon pages
            string pathToPages = PathsManagement.LogonPagesPath;
            if (!FilesystemTools.FolderExists(pathToPages))
                FilesystemTools.MakeDirectory(pathToPages);

            // Enumerate the logon pages
            string[] logonPageFiles = Directory.GetFiles(pathToPages, "*.json");
            List<WidgetRenderInfo[]> logonPages = [];
            foreach (var logonPageFile in logonPageFiles)
            {
                // Parse the file and convert it to an array of render info instances
                WidgetRenderInfo[] renderInfos = WidgetCanvasTools.GetRenderInfosFromFile(logonPageFile);

                // Add the page
                logonPages.Add(renderInfos);
            }

            // Return the result
            return logonPages;
        }
    }
}
