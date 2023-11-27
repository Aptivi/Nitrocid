//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
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
using System.Runtime.CompilerServices;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Languages;
using KS.Kernel.Time.Renderers;
using KS.Kernel.Threading;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using Figletize;
using KS.ConsoleBase.Inputs.Styles.Infobox;

namespace Nitrocid.Extras.Timers.Timers
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
        internal static string timerFigletFont = "Small";
        private static Timer _Timer;

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
            get => TimersInit.TimersConfig.TimerFigletFont;
            set => TimersInit.TimersConfig.TimerFigletFont = FigletTools.GetFigletFonts().ContainsKey(value) ? value : "small";
        }

        /// <summary>
        /// Opens the timer screen
        /// </summary>
        public static void OpenTimer()
        {
            // Populate the figlet font (if any)
            var FigletFont = FigletTools.GetFigletFont(TimerFigletFont);

            // Populate the time
            double TimerInterval = 60000d;

            // Main loop
            bool exiting = false;
            bool rerender = true;
            while (!exiting)
            {
                // Check to see if we need to clear
                if (rerender)
                {
                    ConsoleWrapper.Clear();
                    ConsoleWrapper.CursorVisible = false;
                    rerender = false;
                }

                // Populate the positions for time
                int HalfWidth = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
                string CurrentRemainingString = TimeDateMiscRenderers.RenderRemainingTimeFromNow((int)Math.Round(TimerInterval));
                int TimeLeftPosition = 0;
                int TimeTopPosition = 0;
                UpdateRemainingPositions(CurrentRemainingString, ref TimeLeftPosition, ref TimeTopPosition);

                // Update the old positions
                FigletTimeOldWidth = (int)Math.Round(HalfWidth - FigletTools.GetFigletWidth(CurrentRemainingString, FigletFont) / 2d);
                FigletTimeOldWidthEnd = (int)Math.Round(HalfWidth + FigletTools.GetFigletWidth(CurrentRemainingString, FigletFont) / 2d);

                // Populate the keys text variable
                string KeysText = "[ENTER] " + Translate.DoTranslation("Start counting down") + " | [T] " + Translate.DoTranslation("Set interval") + " | [ESC] " + Translate.DoTranslation("Exit");
                int KeysTextLeftPosition = (int)Math.Round(HalfWidth - KeysText.Length / 2d);
                int KeysTextTopPosition = ConsoleWrapper.WindowHeight - 2;

                // Print the keys text
                TextWriterWhereColor.WriteWhereKernelColor(KeysText, KeysTextLeftPosition, KeysTextTopPosition, true, KernelColorType.Tip);

                // Print the time interval
                if (TimersInit.TimersConfig.EnableFigletTimer)
                    FigletWhereColor.WriteFigletWhereKernelColor(CurrentRemainingString, TimeLeftPosition, TimeTopPosition, true, FigletFont, KernelColorType.NeutralText);
                else
                    TextWriterWhereColor.WriteWhereKernelColor(CurrentRemainingString, TimeLeftPosition, TimeTopPosition, true, KernelColorType.NeutralText);

                // Print the border
                TextWriterWhereColor.WriteWhereColor(new string('═', ConsoleWrapper.WindowWidth), 0, KeysTextTopPosition - 2, true, KernelColorTools.GetGray());

                // Wait for a keypress
                var KeysKeypress = Input.DetectKeypress().Key;

                // Check for a keypress
                switch (KeysKeypress)
                {
                    case ConsoleKey.Enter:
                        // User requested to start up the timer
                        if (Timer.Enabled)
                            break;
                        Timer.Interval = TimerInterval;
                        Timer.Start();
                        TimerStarted = DateTime.Now;
                        if (!TimerUpdate.IsAlive)
                            TimerUpdate.Start();
                        break;
                    case ConsoleKey.T:
                        // User requested to specify the timeout in milliseconds
                        if (Timer.Enabled)
                            break;

                        // Try to parse the interval
                        string UnparsedInterval = InfoBoxInputColor.WriteInfoBoxInputKernelColor(Translate.DoTranslation("Specify the timeout in milliseconds") + " [{0}] ", KernelColorType.Question, TimerInterval);
                        if (!double.TryParse(UnparsedInterval, out TimerInterval))
                        {
                            // Not numeric.
                            InfoBoxColor.WriteInfoBoxKernelColor(Translate.DoTranslation("Indicated timeout is not numeric."), KernelColorType.Error);
                            TimerInterval = 60000d;
                        }
                        rerender = true;
                        break;
                    case ConsoleKey.Escape:
                        // Stop the timer
                        Timer.Stop();
                        Timer.Dispose();
                        if (TimerUpdate.IsAlive)
                            TimerUpdate.Stop();
                        exiting = true;
                        break;
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
            string ElapsedText = new TimeSpan().ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult);
            int TimeLeftPosition = 0;
            int TimeTopPosition = 0;

            // Prepare the display
            UpdateRemainingPositions(ElapsedText, ref TimeLeftPosition, ref TimeTopPosition);
            ClearRemainingTimeDisplay(ElapsedText, FigletTimeOldWidth, FigletTimeOldWidthEnd);

            // Actually display it
            if (TimerUpdate.IsAlive)
                TimerUpdate.Stop();
            if (TimersInit.TimersConfig.EnableFigletTimer)
                FigletWhereColor.WriteFigletWhereKernelColor(ElapsedText, TimeLeftPosition, TimeTopPosition, true, FigletFont, KernelColorType.Success);
            else
                TextWriterWhereColor.WriteWhereKernelColor(ElapsedText, TimeLeftPosition, TimeTopPosition, true, KernelColorType.Success);
            Timer.Stop();
        }

        /// <summary>
        /// Updates the timer elapsed display
        /// </summary>
        private static void UpdateTimerElapsedDisplay()
        {
            var FigletFont = FigletTools.GetFigletFont(TimerFigletFont);
            while (!TimerUpdate.IsStopping)
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
                    if (TimersInit.TimersConfig.EnableFigletTimer)
                        FigletWhereColor.WriteFigletWhereKernelColor(UntilText, TimeLeftPosition, TimeTopPosition, true, FigletFont, KernelColorType.NeutralText);
                    else
                        TextWriterWhereColor.WriteWhereKernelColor(UntilText, TimeLeftPosition, TimeTopPosition, true, KernelColorType.NeutralText);
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
            if (TimersInit.TimersConfig.EnableFigletTimer)
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
            if (TimersInit.TimersConfig.EnableFigletTimer)
            {
                for (int FigletTimePosition = FigletTimeTopPosition; FigletTimePosition <= FigletTimeBottomPosition; FigletTimePosition++)
                {
                    ConsoleWrapper.CursorTop = FigletTimePosition;
                    for (int Position = FigletOldWidth - 1; Position <= FigletTimeLeftPosition - 1; Position++)
                    {
                        ConsoleWrapper.CursorLeft = Position;
                        TextWriterColor.WriteColorBack(" ", false, KernelColorTools.GetColor(KernelColorType.NeutralText), KernelColorTools.GetColor(KernelColorType.Background));
                    }
                    for (int Position = FigletOldWidthEnd; Position <= FigletTimeLeftEndPosition + 1; Position++)
                    {
                        ConsoleWrapper.CursorLeft = Position;
                        TextWriterColor.WriteColorBack(" ", false, KernelColorTools.GetColor(KernelColorType.NeutralText), KernelColorTools.GetColor(KernelColorType.Background));
                    }
                }
            }

            // Update the old positions
            FigletTimeOldWidth = (int)Math.Round(HalfWidth - FigletTools.GetFigletWidth(RemainingTimeText, FigletFont) / 2d);
            FigletTimeOldWidthEnd = (int)Math.Round(HalfWidth + FigletTools.GetFigletWidth(RemainingTimeText, FigletFont) / 2d);
        }

    }
}
