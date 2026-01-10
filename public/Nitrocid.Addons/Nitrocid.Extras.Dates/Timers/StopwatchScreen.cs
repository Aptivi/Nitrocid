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
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Themes.Colors;
using Nitrocid.Base.Languages;
using Terminaux.Base.Buffered;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Nitrocid.Base.Drivers.RNG;
using Terminaux.Inputs;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Colors.Data;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Nitrocid.Extras.Dates.Timers
{
    /// <summary>
    /// Stopwatch CLI module
    /// </summary>
    public static class StopwatchScreen
    {

        internal static List<LapDisplayInfo> Laps = [];
        internal static Color? LapColor;
        internal static Stopwatch Stopwatch = new();
        internal static Stopwatch LappedStopwatch = new();
        internal static bool running;

        private static Keybinding[] KeyBindings =>
        [
            new(LanguageTools.GetLocalized("NKS_DATES_STOPWATCH_STARTSTOP"), ConsoleKey.Enter),
            new(LanguageTools.GetLocalized("NKS_DATES_STOPWATCH_LAP"), ConsoleKey.L),
            new(LanguageTools.GetLocalized("NKS_DATES_STOPWATCH_LAPLIST"), ConsoleKey.L, ConsoleModifiers.Shift),
            new(LanguageTools.GetLocalized("NKS_DATES_STOPWATCH_RESET"), ConsoleKey.R),
            new(LanguageTools.GetLocalized("NKS_DATES_KEYBINDING_EXIT"), ConsoleKey.Escape),
        ];

        /// <summary>
        /// Opens the stopwatch screen
        /// </summary>
        public static void OpenStopwatch()
        {
            Screen watchScreen = new();
            ScreenPart watchScreenPart = new();
            ScreenTools.SetCurrent(watchScreen);
            ThemeColorsTools.LoadBackground();
            string status = LanguageTools.GetLocalized("NKS_DATES_STOPWATCH_STATUS_READY");

            // Set the random lap color
            int RedValue = RandomDriver.Random(255);
            int GreenValue = RandomDriver.Random(255);
            int BlueValue = RandomDriver.Random(255);
            LapColor = new Color(RedValue, GreenValue, BlueValue);

            // Add a dynamic text that shows you the time dynamically
            watchScreenPart.AddDynamicText(() =>
            {
                // If resized, clear the console
                ConsoleWrapper.CursorVisible = false;
                var builder = new StringBuilder();

                // Populate the positions for time
                string LapsText = LanguageTools.GetLocalized("NKS_DATES_STOPWATCH_LAP");
                int HalfWidth = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
                int HalfHeight = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
                var elapsed = Stopwatch.Elapsed;
                string elapsedString = elapsed.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCulture);
                int TimeLeftPosition = (int)Math.Round(HalfWidth * 1.5d - elapsedString.Length / 2d);
                int TimeTopPosition = HalfHeight - 1;
                int LapsCurrentLapLeftPosition = 2;
                int LapsCurrentLapTopPosition = ConsoleWrapper.WindowHeight - 3;

                // Print the keybindings
                int KeysTextTopPosition = ConsoleWrapper.WindowHeight - 1;
                var keybindings = new Keybindings()
                {
                    KeybindingList = KeyBindings,
                    Width = ConsoleWrapper.WindowWidth - 1,
                    BuiltinColor = ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingBuiltin),
                    BuiltinForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingBuiltinForeground),
                    BuiltinBackgroundColor = ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingBuiltinBackground),
                    OptionColor = ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingOption),
                    OptionForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.TuiOptionForeground),
                    OptionBackgroundColor = ThemeColorsTools.GetColor(ThemeColorType.TuiOptionBackground),
                };
                builder.Append(RendererTools.RenderRenderable(keybindings, new(0, KeysTextTopPosition)));

                // Print the time interval and the current lap
                builder.Append(
                    TextWriterWhereColor.RenderWhereColorBack(elapsedString, TimeLeftPosition, TimeTopPosition, true, LapColor, ThemeColorsTools.GetColor(ThemeColorType.Background)) +
                    TextWriterWhereColor.RenderWhereColorBack(LapsText + " {0}: {1}", LapsCurrentLapLeftPosition, LapsCurrentLapTopPosition, true, LapColor, ThemeColorsTools.GetColor(ThemeColorType.Background), Laps.Count + 1, LappedStopwatch.Elapsed.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCulture))
                );

                // Also, print the time difference of the last lap if required
                if (Laps.Count > 1)
                {
                    var firstLastLap = Laps[^1];
                    var secondLastLap = Laps[^2];
                    int lapTopPosition = HalfHeight + 1;
                    var diff = secondLastLap.LapInterval - firstLastLap.LapInterval;
                    bool slower = diff < TimeSpan.Zero;
                    string elapsedDiff = diff.ToString((slower ? "\\+" : "\\-") + @"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCulture);
                    Color finalLapColor = slower ? new Color(ConsoleColors.Red) : new Color(ConsoleColors.Lime);
                    builder.Append(
                        TextWriterWhereColor.RenderWhereColorBack(elapsedDiff, TimeLeftPosition, lapTopPosition, true, finalLapColor, ThemeColorsTools.GetColor(ThemeColorType.Background))
                    );
                }

                // Print the border
                int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
                int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
                int SeparatorMinimumHeight = 1;
                int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
                var lapsBoxFrame = new BoxFrame()
                {
                    Left = 0,
                    Top = SeparatorMinimumHeight,
                    Width = SeparatorHalfConsoleWidthInterior,
                    Height = SeparatorMaximumHeightInterior,
                    FrameColor = ConsoleColoring.GetGray(),
                    BackgroundColor = ThemeColorsTools.GetColor(ThemeColorType.Background),
                };
                var stopwatchBoxFrame = new BoxFrame()
                {
                    Left = SeparatorHalfConsoleWidth,
                    Top = SeparatorMinimumHeight,
                    Width = SeparatorHalfConsoleWidthInterior + (ConsoleWrapper.WindowWidth % 2 != 0 ? 1 : 0),
                    Height = SeparatorMaximumHeightInterior,
                    FrameColor = ConsoleColoring.GetGray(),
                    BackgroundColor = ThemeColorsTools.GetColor(ThemeColorType.Background),
                };
                builder.Append(
                    lapsBoxFrame.Render() +
                    stopwatchBoxFrame.Render()
                );

                // Print informational messages
                builder.Append(
                    TextWriterWhereColor.RenderWhereColorBack(status, 0, 0, false, ThemeColorsTools.GetColor(ThemeColorType.NeutralText), ThemeColorsTools.GetColor(ThemeColorType.Background)) +
                    ConsoleClearing.GetClearLineToRightSequence()
                );

                // Print the laps list
                int LapsLapsListLeftPosition = 2;
                int LapsLapsListTopPosition = 2;
                int LapsListEndBorder = ConsoleWrapper.WindowHeight - 6;
                var LapsListBuilder = new StringBuilder();
                int BorderDifference = Laps.Count - LapsListEndBorder;
                if (BorderDifference < 0)
                    BorderDifference = 0;
                for (int LapIndex = BorderDifference; LapIndex <= Laps.Count - 1; LapIndex++)
                {
                    var Lap = Laps[LapIndex];
                    LapsListBuilder.AppendLine(Lap.LapColor.VTSequenceForeground() + LanguageTools.GetLocalized("NKS_DATES_STOPWATCH_LAP") + $" {LapIndex + 1}: {Lap.LapInterval.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCulture)}");
                }
                builder.Append(
                    TextWriterWhereColor.RenderWhereColorBack(LapsListBuilder.ToString(), LapsLapsListLeftPosition, LapsLapsListTopPosition, true, LapColor, ThemeColorsTools.GetColor(ThemeColorType.Background))
                );

                // Return the resultant buffer
                return builder.ToString();
            });

            watchScreen.AddBufferedPart("Stopwatch Update", watchScreenPart);
            bool exiting = false;
            while (!exiting)
            {
                ScreenTools.Render(watchScreen);

                // Check to see if the timer is running to continually update the timer render
                ConsoleKeyInfo KeysKeypress = default;
                if (running)
                {
                    // Wait for a keypress
                    if (ConsoleWrapper.KeyAvailable)
                        KeysKeypress = Input.ReadKey();
                }
                else
                {
                    // Wait for a keypress
                    KeysKeypress = Input.ReadKey();
                }

                // Check for a keypress
                switch (KeysKeypress.Key)
                {
                    case ConsoleKey.Enter:
                        {
                            if (LappedStopwatch.IsRunning)
                                LappedStopwatch.Stop();
                            else
                                LappedStopwatch.Start();
                            if (Stopwatch.IsRunning)
                                Stopwatch.Stop();
                            else
                                Stopwatch.Start();
                            running = Stopwatch.IsRunning;
                            status = LanguageTools.GetLocalized("NKS_DATES_STOPWATCH_STATUS_RUNNING");
                            break;
                        }
                    case ConsoleKey.L:
                        {
                            if (LappedStopwatch.IsRunning)
                            {
                                if (KeysKeypress.Modifiers == ConsoleModifiers.Shift)
                                {
                                    InfoBoxModalColor.WriteInfoBoxModal(GenerateLapList());
                                    watchScreen.RequireRefresh();
                                    break;
                                }
                                var Lap = new LapDisplayInfo(LapColor, LappedStopwatch.Elapsed);
                                Laps.Add(Lap);
                                LappedStopwatch.Restart();

                                // Select random color
                                RedValue = RandomDriver.Random(255);
                                GreenValue = RandomDriver.Random(255);
                                BlueValue = RandomDriver.Random(255);
                                LapColor = new Color(RedValue, GreenValue, BlueValue);
                                status = LanguageTools.GetLocalized("NKS_DATES_STOPWATCH_STATUS_NEWLAP") + $" {Lap.LapInterval}";
                            }
                            break;
                        }
                    case ConsoleKey.R:
                        {
                            if (LappedStopwatch.IsRunning)
                                LappedStopwatch.Reset();
                            if (Stopwatch.IsRunning)
                                Stopwatch.Reset();
                            running = false;
                            watchScreen.RequireRefresh();

                            // Clear the laps
                            Laps.Clear();
                            LapColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
                            status = LanguageTools.GetLocalized("NKS_DATES_STOPWATCH_STATUS_READY");
                            break;
                        }
                    case ConsoleKey.Escape:
                        {
                            if (LappedStopwatch.IsRunning)
                                LappedStopwatch.Reset();
                            if (Stopwatch.IsRunning)
                                Stopwatch.Reset();
                            LapColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
                            exiting = true;
                            break;
                        }
                }
            }

            // Clear for cleanliness
            ScreenTools.UnsetCurrent(watchScreen);
            running = false;
            Laps.Clear();
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = true;
        }

        private static string GenerateLapList()
        {
            var lapsListBuilder = new StringBuilder();
            for (int i = 0; i < Laps.Count; i++)
            {
                LapDisplayInfo? lap = Laps[i];
                lapsListBuilder.AppendLine(lap.LapColor.VTSequenceForeground() + LanguageTools.GetLocalized("NKS_DATES_STOPWATCH_LAP") + $" {i + 1}: {lap.LapInterval.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCulture)}");
            }
            if (Laps.Count == 0)
                lapsListBuilder.AppendLine(LanguageTools.GetLocalized("NKS_DATES_STOPWATCH_NOLAPS"));
            return lapsListBuilder.ToString();
        }
    }
}
