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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Languages;
using Terminaux.Base.Buffered;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Reader;

namespace Nitrocid.Extras.Timers.Timers
{
    /// <summary>
    /// Stopwatch CLI module
    /// </summary>
    public static class StopwatchScreen
    {

        internal static List<LapDisplayInfo> Laps = [];
        internal static Color LapColor = KernelColorTools.GetColor(KernelColorType.NeutralText);
        internal static Stopwatch Stopwatch = new();
        internal static Stopwatch LappedStopwatch = new();
        internal static bool running;

        /// <summary>
        /// Opens the stopwatch screen
        /// </summary>
        public static void OpenStopwatch()
        {
            Screen watchScreen = new();
            ScreenPart watchScreenPart = new();
            ScreenTools.SetCurrent(watchScreen);
            ColorTools.LoadBack();
            string status = Translate.DoTranslation("Stopwatch is ready.");
            bool resetting = false;

            // Add a dynamic text that shows you the time dynamically
            watchScreenPart.AddDynamicText(() =>
            {
                // If resized, clear the console
                if (resetting || ConsoleResizeHandler.WasResized())
                {
                    resetting = false;
                    ColorTools.LoadBack();
                }
                ConsoleWrapper.CursorVisible = false;
                var builder = new StringBuilder();

                // Populate the positions for time
                string LapsText = Translate.DoTranslation("Lap");
                int HalfWidth = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
                int HalfHeight = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
                int TimeLeftPosition = (int)Math.Round(HalfWidth * 1.5d - Stopwatch.Elapsed.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult).Length / 2d);
                int TimeTopPosition = HalfHeight - 2;
                int LapsCurrentLapLeftPosition = 4;
                int LapsCurrentLapTopPosition = ConsoleWrapper.WindowHeight - 6;

                // Populate the keys text variable
                string KeysText = "[ENTER] " + Translate.DoTranslation("Start or stop") + " | [L] " + Translate.DoTranslation("Lap") + " | [R] " + Translate.DoTranslation("Reset") + " | [ESC] " + Translate.DoTranslation("Exit");
                int KeysTextLeftPosition = (int)Math.Round(HalfWidth - KeysText.Length / 2d);
                int KeysTextTopPosition = ConsoleWrapper.WindowHeight - 2;

                // Print the keys text
                builder.Append(
                    TextWriterWhereColor.RenderWhere(KeysText, KeysTextLeftPosition, KeysTextTopPosition, true, KernelColorTools.GetColor(KernelColorType.Tip), KernelColorTools.GetColor(KernelColorType.Background))
                );

                // Print the time interval and the current lap
                builder.Append(
                    TextWriterWhereColor.RenderWhere(Stopwatch.Elapsed.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult), TimeLeftPosition, TimeTopPosition, true, LapColor, KernelColorTools.GetColor(KernelColorType.Background)) +
                    TextWriterWhereColor.RenderWhere(LapsText + " {0}: {1}", LapsCurrentLapLeftPosition, LapsCurrentLapTopPosition, true, LapColor, KernelColorTools.GetColor(KernelColorType.Background), Laps.Count + 1, LappedStopwatch.Elapsed.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult))
                );

                // Print the border
                builder.Append(MakeBorder());

                // Print informational messages
                builder.Append(
                    TextWriterWhereColor.RenderWhere(status, 1, 0, false, KernelColorTools.GetColor(KernelColorType.NeutralText), KernelColorTools.GetColor(KernelColorType.Background)) +
                    ConsoleClearing.GetClearLineToRightSequence()
                );

                // Print the laps list
                int LapsLapsListLeftPosition = 4;
                int LapsLapsListTopPosition = 3;
                int LapsListEndBorder = ConsoleWrapper.WindowHeight - 10;
                var LapsListBuilder = new StringBuilder();
                int BorderDifference = Laps.Count - LapsListEndBorder;
                if (BorderDifference < 0)
                    BorderDifference = 0;
                for (int LapIndex = BorderDifference; LapIndex <= Laps.Count - 1; LapIndex++)
                {
                    var Lap = Laps[LapIndex];
                    LapsListBuilder.AppendLine(Lap.LapColor.VTSequenceForeground + Translate.DoTranslation("Lap") + $" {LapIndex + 1}: {Lap.LapInterval.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult)}");
                }
                builder.Append(
                    TextWriterWhereColor.RenderWhere(LapsListBuilder.ToString(), LapsLapsListLeftPosition, LapsLapsListTopPosition, true, LapColor, KernelColorTools.GetColor(KernelColorType.Background))
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
                ConsoleKey KeysKeypress = default;
                if (running)
                {
                    // Wait for a keypress
                    if (ConsoleWrapper.KeyAvailable)
                        KeysKeypress = TermReader.ReadKey().Key;
                }
                else
                {
                    // Wait for a keypress
                    KeysKeypress = TermReader.ReadKey().Key;
                }

                // Check for a keypress
                switch (KeysKeypress)
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
                            status = Translate.DoTranslation("Stopwatch running!");
                            break;
                        }
                    case ConsoleKey.L:
                        {
                            if (LappedStopwatch.IsRunning)
                            {
                                var Lap = new LapDisplayInfo(LapColor, LappedStopwatch.Elapsed);
                                Laps.Add(Lap);
                                LappedStopwatch.Restart();

                                // Select random color
                                var Randomizer = new Random();
                                int RedValue = Randomizer.Next(255);
                                int GreenValue = Randomizer.Next(255);
                                int BlueValue = Randomizer.Next(255);
                                LapColor = new Color(RedValue, GreenValue, BlueValue);
                                status = Translate.DoTranslation("New lap!") + $" {Lap.LapInterval}";
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
                            resetting = true;

                            // Clear the laps
                            Laps.Clear();
                            LapColor = KernelColorTools.GetColor(KernelColorType.NeutralText);
                            status = Translate.DoTranslation("Stopwatch is ready.");
                            break;
                        }
                    case ConsoleKey.Escape:
                        {
                            if (LappedStopwatch.IsRunning)
                                LappedStopwatch.Reset();
                            if (Stopwatch.IsRunning)
                                Stopwatch.Reset();
                            LapColor = KernelColorTools.GetColor(KernelColorType.NeutralText);
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

        /// <summary>
        /// Makes the display border
        /// </summary>
        public static string MakeBorder()
        {
            var border = new StringBuilder();
            int KeysTextTopPosition = ConsoleWrapper.WindowHeight - 2;
            int HalfWidth = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
            border.Append(
                TextWriterWhereColor.RenderWhere(new string('═', ConsoleWrapper.WindowWidth), 0, KeysTextTopPosition - 2, true, ColorTools.GetGray(), KernelColorTools.GetColor(KernelColorType.Background)) +
                TextWriterWhereColor.RenderWhere(new string('═', ConsoleWrapper.WindowWidth), 0, 1, true, ColorTools.GetGray(), KernelColorTools.GetColor(KernelColorType.Background))
            );
            for (int Height = 2; Height <= KeysTextTopPosition - 2; Height++)
            {
                border.Append(
                    TextWriterWhereColor.RenderWhere("║", HalfWidth, Height, true, ColorTools.GetGray(), KernelColorTools.GetColor(KernelColorType.Background))
                );
            }
            border.Append(
                TextWriterWhereColor.RenderWhere("╩", HalfWidth, KeysTextTopPosition - 2, true, ColorTools.GetGray(), KernelColorTools.GetColor(KernelColorType.Background)) +
                TextWriterWhereColor.RenderWhere("╦", HalfWidth, 1, true, ColorTools.GetGray(), KernelColorTools.GetColor(KernelColorType.Background))
            );
            return border.ToString();
        }

    }
}
