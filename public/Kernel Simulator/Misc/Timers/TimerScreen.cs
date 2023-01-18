
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;
using Extensification.StringExts;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Misc.Writers.FancyWriters.Tools;

namespace KS.Misc.Timers
{
    /// <summary>
    /// Timer CLI module
    /// </summary>
    public static class TimerScreen
    {
        internal static KernelThread TimerUpdate = new("Timer Remaining Time Updater", true, UpdateTimerElapsedDisplay);
        internal static DateTime TimerStarted;
        internal static int FigletTimeOldWidth;
        internal static int FigletTimeOldWidthEnd;
        private static Timer _Timer;
        private static string timerFigletFont = "Small";

        internal static Timer Timer
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Timer;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Timer != null)
                {
                    _Timer.Elapsed -= TimerElapsed;
                }

                _Timer = value;
                if (_Timer != null)
                {
                    _Timer.Elapsed += TimerElapsed;
                }
            }
        }

        static TimerScreen() => Timer = new Timer();

        /// <summary>
        /// Timer figlet font
        /// </summary>
        public static string TimerFigletFont
        {
            get => timerFigletFont;
            set => timerFigletFont = FigletTools.FigletFonts.ContainsKey(value) ? value : "Small";
        }

        /// <summary>
        /// Opens the timer screen
        /// </summary>
        public static void OpenTimer()
        {
            // Clear for cleanliness
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = false;

            // Populate the figlet font (if any)
            var FigletFont = FigletTools.GetFigletFont(TimerFigletFont);

            // Populate the time
            double TimerInterval = 60000d;

            // Populate the positions for time
            int HalfWidth = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
            int HalfHeight = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            string CurrentRemainingString = TimeDate.TimeDate.GetRemainingTimeFromNow((int)Math.Round(TimerInterval));
            int TimeLeftPosition = 0;
            int TimeTopPosition = 0;
            UpdateRemainingPositions(CurrentRemainingString, ref TimeLeftPosition, ref TimeTopPosition);

            // Update the old positions
            FigletTimeOldWidth = (int)Math.Round(HalfWidth - FigletTools.GetFigletWidth(CurrentRemainingString, FigletFont) / 2d);
            FigletTimeOldWidthEnd = (int)Math.Round(HalfWidth + FigletTools.GetFigletWidth(CurrentRemainingString, FigletFont) / 2d);

            // Populate the keys text variable
            string KeysText = "[ENTER] " + Translate.DoTranslation("Start (re)counting down") + " | [T] " + Translate.DoTranslation("Set interval") + " | [ESC] " + Translate.DoTranslation("Exit");
            int KeysTextLeftPosition = (int)Math.Round(HalfWidth - KeysText.Length / 2d);
            int KeysTextTopPosition = ConsoleWrapper.WindowHeight - 2;
            var KeysKeypress = default(ConsoleKey);

            // Print the keys text
            TextWriterWhereColor.WriteWhere(KeysText, KeysTextLeftPosition, KeysTextTopPosition, true, KernelColorType.Tip);

            // Print the time interval
            if (Flags.EnableFigletTimer)
            {
                FigletWhereColor.WriteFigletWhere(CurrentRemainingString, TimeLeftPosition, TimeTopPosition, true, FigletFont, KernelColorType.NeutralText);
            }
            else
            {
                TextWriterWhereColor.WriteWhere(CurrentRemainingString, TimeLeftPosition, TimeTopPosition, true, KernelColorType.NeutralText);
            }

            // Print the border
            TextWriterWhereColor.WriteWhere("═".Repeat(ConsoleWrapper.WindowWidth), 0, KeysTextTopPosition - 2, true, KernelColorType.Gray);

            // Wait for a keypress
            while (KeysKeypress != ConsoleKey.Escape)
            {
                KeysKeypress = Input.DetectKeypress().Key;

                // Check for a keypress
                switch (KeysKeypress)
                {
                    case ConsoleKey.Enter:
                        {
                            // User requested to start up the timer
                            Timer.Interval = TimerInterval;
                            Timer.Start();
                            TimerStarted = DateTime.Now;
                            if (!TimerUpdate.IsAlive)
                                TimerUpdate.Start();
                            break;
                        }
                    case ConsoleKey.T:
                        {
                            // User requested to specify the timeout in milliseconds
                            if (!Timer.Enabled)
                            {
                                TextWriterWhereColor.WriteWhere(Translate.DoTranslation("Specify the timeout in milliseconds") + " [{0}] ", 2, KeysTextTopPosition - 4, false, KernelColorType.Question, TimerInterval);

                                // Try to parse the interval
                                string UnparsedInterval = Input.ReadLine();
                                if (!double.TryParse(UnparsedInterval, out TimerInterval))
                                {
                                    // Not numeric.
                                    TextWriterWhereColor.WriteWhere(Translate.DoTranslation("Indicated timeout is not numeric."), 2, KeysTextTopPosition - 4, false, KernelColorType.Error);
                                    ConsoleExtensions.ClearLineToRight();
                                    Input.DetectKeypress();
                                }
                                else
                                {
                                    // Update the remaining time
                                    string RemainingString = TimeDate.TimeDate.GetRemainingTimeFromNow((int)Math.Round(TimerInterval));
                                    UpdateRemainingPositions(RemainingString, ref TimeLeftPosition, ref TimeTopPosition);
                                    ClearRemainingTimeDisplay(RemainingString, FigletTimeOldWidth, FigletTimeOldWidthEnd);
                                    if (Flags.EnableFigletTimer)
                                    {
                                        FigletWhereColor.WriteFigletWhere(RemainingString, TimeLeftPosition, TimeTopPosition, true, FigletFont, KernelColorType.NeutralText);
                                    }
                                    else
                                    {
                                        TextWriterWhereColor.WriteWhere(RemainingString, TimeLeftPosition, TimeTopPosition, true, KernelColorType.NeutralText);
                                    }
                                }

                                // Clean up
                                ConsoleWrapper.SetCursorPosition(0, KeysTextTopPosition - 4);
                                ConsoleExtensions.ClearLineToRight();
                            }

                            break;
                        }
                    case ConsoleKey.Escape:
                        {
                            // Stop the timer
                            Timer.Stop();
                            Timer.Dispose();
                            if (TimerUpdate.IsAlive)
                                TimerUpdate.Stop();
                            break;
                        }
                }
            }

            // Clear for cleanliness
            Timer = new Timer();
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = true;
        }

        /// <summary>
        /// Indicates that the timer has elapsed
        /// </summary>
        private static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            var FigletFont = FigletTools.GetFigletFont(TimerFigletFont);
            int HalfWidth = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
            int HalfHeight = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            string ElapsedText = new TimeSpan().ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult);
            int TimeLeftPosition = 0;
            int TimeTopPosition = 0;

            // Prepare the display
            UpdateRemainingPositions(ElapsedText, ref TimeLeftPosition, ref TimeTopPosition);
            ClearRemainingTimeDisplay(ElapsedText, FigletTimeOldWidth, FigletTimeOldWidthEnd);

            // Actually display it
            if (TimerUpdate.IsAlive)
                TimerUpdate.Stop();
            if (Flags.EnableFigletTimer)
            {
                FigletWhereColor.WriteFigletWhere(ElapsedText, TimeLeftPosition, TimeTopPosition, true, FigletFont, KernelColorType.Success);
            }
            else
            {
                TextWriterWhereColor.WriteWhere(ElapsedText, TimeLeftPosition, TimeTopPosition, true, KernelColorType.Success);
            }
            Timer.Stop();
        }

        /// <summary>
        /// Updates the timer elapsed display
        /// </summary>
        private static void UpdateTimerElapsedDisplay()
        {
            var FigletFont = FigletTools.GetFigletFont(TimerFigletFont);
            while (TimerUpdate.IsAlive)
            {
                try
                {
                    var Until = TimerStarted.AddMilliseconds(Timer.Interval) - DateTime.Now;
                    int HalfWidth = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
                    int HalfHeight = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
                    string UntilText = Until.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult);
                    int TimeLeftPosition = 0;
                    int TimeTopPosition = 0;

                    // Prepare the display
                    UpdateRemainingPositions(UntilText, ref TimeLeftPosition, ref TimeTopPosition);
                    ClearRemainingTimeDisplay(UntilText, FigletTimeOldWidth, FigletTimeOldWidthEnd);

                    // Actually display the remaining time
                    if (Flags.EnableFigletTimer)
                    {
                        FigletWhereColor.WriteFigletWhere(UntilText, TimeLeftPosition, TimeTopPosition, true, FigletFont, KernelColorType.NeutralText);
                    }
                    else
                    {
                        TextWriterWhereColor.WriteWhere(UntilText, TimeLeftPosition, TimeTopPosition, true, KernelColorType.NeutralText);
                    }
                }
                catch (ThreadInterruptedException)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Updates the remaining positions for time, adapting to Figlet if possible
        /// </summary>
        private static void UpdateRemainingPositions(string RemainingTimeText, ref int TimeLeftPosition, ref int TimeTopPosition)
        {
            // Some initial variables
            var FigletFont = FigletTools.GetFigletFont(TimerFigletFont);
            int HalfWidth = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
            int HalfHeight = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);

            // Get the Figlet time left and top position
            int FigletTimeLeftPosition = (int)Math.Round(HalfWidth - FigletTools.GetFigletWidth(RemainingTimeText, FigletFont) / 2d);
            int FigletTimeTopPosition = (int)Math.Round(HalfHeight - FigletTools.GetFigletHeight(RemainingTimeText, FigletFont) / 2d);

            // Now, get the normal time left and top position and update the values according to timer type
            TimeLeftPosition = (int)Math.Round(HalfWidth - RemainingTimeText.Length / 2d);
            TimeTopPosition = HalfHeight - 3;
            if (Flags.EnableFigletTimer)
            {
                TimeLeftPosition = FigletTimeLeftPosition;
                TimeTopPosition = FigletTimeTopPosition;
            }
        }

        private static void ClearRemainingTimeDisplay(string RemainingTimeText, int FigletOldWidth, int FigletOldWidthEnd)
        {
            // Some initial variables
            var FigletFont = FigletTools.GetFigletFont(TimerFigletFont);
            int HalfWidth = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
            int HalfHeight = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);

            // Get the Figlet time left and top position
            int FigletTimeLeftPosition = (int)Math.Round(HalfWidth - FigletTools.GetFigletWidth(RemainingTimeText, FigletFont) / 2d);
            int FigletTimeLeftEndPosition = (int)Math.Round(HalfWidth + FigletTools.GetFigletWidth(RemainingTimeText, FigletFont) / 2d);
            int FigletTimeTopPosition = (int)Math.Round(HalfHeight - FigletTools.GetFigletHeight(RemainingTimeText, FigletFont) / 2d);
            int FigletTimeBottomPosition = (int)Math.Round(HalfHeight + FigletTools.GetFigletHeight(RemainingTimeText, FigletFont) / 2d);

            // If figlet is enabled, clear the display
            if (Flags.EnableFigletTimer)
            {
                for (int FigletTimePosition = FigletTimeTopPosition; FigletTimePosition <= FigletTimeBottomPosition; FigletTimePosition++)
                {
                    ConsoleWrapper.CursorTop = FigletTimePosition;
                    for (int Position = FigletOldWidth - 1; Position <= FigletTimeLeftPosition - 1; Position++)
                    {
                        ConsoleWrapper.CursorLeft = Position;
                        TextWriterColor.Write(" ", false, ColorTools.GetColor(KernelColorType.NeutralText), ColorTools.GetColor(KernelColorType.Background));
                    }
                    for (int Position = FigletOldWidthEnd; Position <= FigletTimeLeftEndPosition + 1; Position++)
                    {
                        ConsoleWrapper.CursorLeft = Position;
                        TextWriterColor.Write(" ", false, ColorTools.GetColor(KernelColorType.NeutralText), ColorTools.GetColor(KernelColorType.Background));
                    }
                }
            }

            // Update the old positions
            FigletTimeOldWidth = (int)Math.Round(HalfWidth - FigletTools.GetFigletWidth(RemainingTimeText, FigletFont) / 2d);
            FigletTimeOldWidthEnd = (int)Math.Round(HalfWidth + FigletTools.GetFigletWidth(RemainingTimeText, FigletFont) / 2d);
        }

    }
}
