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
using System.Threading;
using Terminaux.Colors;
using Nitrocid.ConsoleBase;
using Nitrocid.ConsoleBase.Writers.ConsoleWriters;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel.Threading;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.ConsoleBase.Inputs;

namespace Nitrocid.Extras.Timers.Timers
{
    /// <summary>
    /// Stopwatch CLI module
    /// </summary>
    public static class StopwatchScreen
    {

        internal static List<LapDisplayInfo> Laps = [];
        internal static KernelThread StopwatchUpdate = new("Stopwatch ETA Updater", true, UpdateStopwatchElapsedDisplay);
        internal static Color LapColor = KernelColorTools.GetColor(KernelColorType.NeutralText);
        internal static Stopwatch Stopwatch = new();
        internal static Stopwatch LappedStopwatch = new();
        internal static bool NewLapAcknowledged;

        /// <summary>
        /// Opens the stopwatch screen
        /// </summary>
        public static void OpenStopwatch()
        {
            // Clear for cleanliness
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = false;

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
            var KeysKeypress = default(ConsoleKey);

            // Print the keys text
            TextWriters.WriteWhere(KeysText, KeysTextLeftPosition, KeysTextTopPosition, true, KernelColorType.Tip);

            // Print the time interval and the current lap
            TextWriterWhereColor.WriteWhereColor(Stopwatch.Elapsed.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult), TimeLeftPosition, TimeTopPosition, true, LapColor);
            TextWriterWhereColor.WriteWhereColor(LapsText + " {0}: {1}", LapsCurrentLapLeftPosition, LapsCurrentLapTopPosition, true, LapColor, Laps.Count + 1, LappedStopwatch.Elapsed.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult));

            // Print the border
            MakeBorder();

            // Print informational messages
            TextWriters.WriteWhere(Translate.DoTranslation("Ready!"), 1, 0, false, KernelColorType.NeutralText);
            ConsoleExtensions.ClearLineToRight();

            while (KeysKeypress != ConsoleKey.Escape)
            {
                // Wait for a keypress
                KeysKeypress = Input.DetectKeypress().Key;

                // Check for a keypress
                switch (KeysKeypress)
                {
                    case ConsoleKey.Enter:
                        {
                            if (!StopwatchUpdate.IsAlive)
                                StopwatchUpdate.Start();
                            else
                                StopwatchUpdate.Stop();
                            if (LappedStopwatch.IsRunning)
                                LappedStopwatch.Stop();
                            else
                                LappedStopwatch.Start();
                            if (Stopwatch.IsRunning)
                                Stopwatch.Stop();
                            else
                                Stopwatch.Start();
                            TextWriters.WriteWhere(Translate.DoTranslation("Stopwatch running!"), 1, 0, false, KernelColorType.NeutralText);
                            ConsoleExtensions.ClearLineToRight();
                            break;
                        }
                    case ConsoleKey.L:
                        {
                            if (LappedStopwatch.IsRunning)
                            {
                                var Lap = new LapDisplayInfo(LapColor, LappedStopwatch.Elapsed);
                                Laps.Add(Lap);
                                LappedStopwatch.Restart();
                                NewLapAcknowledged = true;

                                // Select random color
                                var Randomizer = new Random();
                                int RedValue = Randomizer.Next(255);
                                int GreenValue = Randomizer.Next(255);
                                int BlueValue = Randomizer.Next(255);
                                LapColor = new Color(RedValue, GreenValue, BlueValue);
                            }

                            break;
                        }
                    case ConsoleKey.R:
                        {
                            if (StopwatchUpdate.IsAlive)
                                StopwatchUpdate.Stop();
                            if (LappedStopwatch.IsRunning)
                                LappedStopwatch.Reset();
                            if (Stopwatch.IsRunning)
                                Stopwatch.Reset();

                            // Clear the laps and the laps list
                            Laps.Clear();
                            for (int Y = 1; Y <= LapsCurrentLapTopPosition - 1; Y++)
                            {
                                ConsoleWrapper.SetCursorPosition(LapsCurrentLapLeftPosition, Y);
                                ConsoleExtensions.ClearLineToRight();
                            }

                            // Reset the indicators
                            LapColor = KernelColorTools.GetColor(KernelColorType.NeutralText);
                            TextWriterWhereColor.WriteWhereColor(Translate.DoTranslation("Lap") + " {0}: {1}", LapsCurrentLapLeftPosition, LapsCurrentLapTopPosition, false, LapColor, Laps.Count + 1, LappedStopwatch.Elapsed.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult));
                            ConsoleExtensions.ClearLineToRight();
                            ConsoleWrapper.SetCursorPosition(0, TimeTopPosition);
                            ConsoleExtensions.ClearLineToRight();
                            TextWriterWhereColor.WriteWhereColor(Stopwatch.Elapsed.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult), TimeLeftPosition, TimeTopPosition, false, LapColor);
                            MakeBorder();
                            TextWriters.WriteWhere(Translate.DoTranslation("Ready!"), 1, 0, false, KernelColorType.NeutralText);
                            ConsoleExtensions.ClearLineToRight();
                            break;
                        }
                    case ConsoleKey.Escape:
                        {
                            if (LappedStopwatch.IsRunning)
                                LappedStopwatch.Reset();
                            if (Stopwatch.IsRunning)
                                Stopwatch.Reset();
                            LapColor = KernelColorTools.GetColor(KernelColorType.NeutralText);
                            if (StopwatchUpdate.IsAlive)
                                StopwatchUpdate.Stop();
                            break;
                        }
                }
            }

            // Clear for cleanliness
            Laps.Clear();
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = true;
        }

        /// <summary>
        /// Updates the elapsed display for stopwatch
        /// </summary>
        private static void UpdateStopwatchElapsedDisplay()
        {
            // Populate the positions for time and for lap list
            string LapsText = Translate.DoTranslation("Lap");
            int HalfWidth = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
            int HalfHeight = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            int TimeLeftPosition = (int)Math.Round(HalfWidth * 1.5d - Stopwatch.Elapsed.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult).Length / 2d);
            int TimeTopPosition = HalfHeight - 2;
            int LapsCurrentLapLeftPosition = 4;
            int LapsCurrentLapTopPosition = ConsoleWrapper.WindowHeight - 6;
            int LapsLapsListLeftPosition = 4;
            int LapsLapsListTopPosition = 3;

            while (!StopwatchUpdate.IsStopping)
            {
                try
                {
                    // Update the elapsed display
                    TextWriterWhereColor.WriteWhereColor(Stopwatch.Elapsed.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult), TimeLeftPosition, TimeTopPosition, true, LapColor);
                    TextWriterWhereColor.WriteWhereColor(LapsText + " {0}: {1}", LapsCurrentLapLeftPosition, LapsCurrentLapTopPosition, true, LapColor, Laps.Count + 1, LappedStopwatch.Elapsed.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult));

                    // Update the laps list if new lap is acknowledged
                    if (NewLapAcknowledged)
                    {
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
                        TextWriterWhereColor.WriteWhereColor(LapsListBuilder.ToString(), LapsLapsListLeftPosition, LapsLapsListTopPosition, true, LapColor);
                        NewLapAcknowledged = false;
                    }
                }
                catch (ThreadInterruptedException)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Makes the display border
        /// </summary>
        public static void MakeBorder()
        {
            int KeysTextTopPosition = ConsoleWrapper.WindowHeight - 2;
            int HalfWidth = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
            TextWriterWhereColor.WriteWhereColor(new string('═', ConsoleWrapper.WindowWidth), 0, KeysTextTopPosition - 2, true, KernelColorTools.GetGray());
            TextWriterWhereColor.WriteWhereColor(new string('═', ConsoleWrapper.WindowWidth), 0, 1, true, KernelColorTools.GetGray());
            for (int Height = 2; Height <= KeysTextTopPosition - 2; Height++)
                TextWriterWhereColor.WriteWhereColor("║", HalfWidth, Height, true, KernelColorTools.GetGray());
            TextWriterWhereColor.WriteWhereColor("╩", HalfWidth, KeysTextTopPosition - 2, true, KernelColorTools.GetGray());
            TextWriterWhereColor.WriteWhereColor("╦", HalfWidth, 1, true, KernelColorTools.GetGray());
        }

    }
}
