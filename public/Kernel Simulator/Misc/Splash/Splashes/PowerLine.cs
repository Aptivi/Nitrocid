
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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
using System.Threading;
using ColorSeq;
using Extensification.StringExts;
using KS.ConsoleBase;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;
using KS.ConsoleBase.Colors;

namespace KS.Misc.Splash.Splashes
{
    class SplashPowerLine : ISplash
    {

        // Standalone splash information
        public string SplashName => "PowerLine";

        private SplashInfo Info => SplashManager.Splashes[SplashName];

        // Property implementations
        public bool SplashClosing { get; set; }

        public bool SplashDisplaysProgress => Info.DisplaysProgress;

        private Color FirstColorSegmentBackground = Color.Empty;
        private Color LastTransitionForeground = Color.Empty;
        private int PowerLineLength = 0;
        private bool LengthDecreasing;
        private readonly char TransitionChar = Convert.ToChar(0xE0B0);

        // Actual logic
        public void Opening()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Splash opening. Clearing console...");
            ConsoleWrapper.Clear();

            // Select the color segment background and mirror it to the transition foreground color
            FirstColorSegmentBackground = new Color(RandomDriver.Random(255), RandomDriver.Random(255), RandomDriver.Random(255));
            LastTransitionForeground = FirstColorSegmentBackground;
        }

        public void Display()
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");
                while (!SplashClosing)
                {
                    // As the length increases, draw the PowerLine lines
                    for (int Top = 0; Top <= ConsoleWrapper.WindowHeight - 1; Top++)
                    {
                        if (SplashClosing)
                            break;
                        TextWriterWhereColor.WriteWhere(" ".Repeat(PowerLineLength), 0, Top, Color.Empty, FirstColorSegmentBackground);
                        TextWriterWhereColor.WriteWhere(Convert.ToString(TransitionChar), PowerLineLength, Top, LastTransitionForeground);
                        ConsoleExtensions.ClearLineToRight();
                    }

                    // Increase the length until we reach the window width, then decrease it.
                    if (LengthDecreasing)
                    {
                        PowerLineLength -= 1;

                        // If we reached the start, increase the length
                        if (PowerLineLength == 0)
                        {
                            LengthDecreasing = false;
                        }
                    }
                    else
                    {
                        PowerLineLength += 1;

                        // If we reached the end, decrease the length
                        if (PowerLineLength == ConsoleWrapper.WindowWidth - 1)
                        {
                            LengthDecreasing = true;
                        }
                    }

                    // Sleep to draw
                    ThreadManager.SleepNoBlock(10, SplashManager.SplashThread);
                }
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
        }

        public void Closing()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Splash closing. Clearing console...");
            ColorTools.SetConsoleColor(KernelColorType.NeutralText);
            ColorTools.SetConsoleColor(KernelColorType.Background, true);
            ConsoleWrapper.Clear();
        }

        public void Report(int Progress, string ProgressReport, params object[] Vars)
        {
        }

        public void ReportError(int Progress, string ErrorReport, Exception ExceptionInfo, params object[] Vars)
        {
        }

    }
}
