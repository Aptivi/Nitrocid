
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Threading;
using KS.ConsoleBase;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using KS.ConsoleBase.Writers.ConsoleWriters;
using Terminaux.Colors;

namespace KS.Misc.Splash.Splashes
{
    class SplashPowerLine : BaseSplash, ISplash
    {

        // Standalone splash information
        public override string SplashName => "PowerLine";

        private Color FirstColorSegmentBackground = Color.Empty;
        private Color LastTransitionForeground = Color.Empty;
        private int PowerLineLength = 0;
        private bool LengthDecreasing;
        private readonly char TransitionChar = Convert.ToChar(0xE0B0);

        // Actual logic
        public override void Opening()
        {
            // Select the color segment background and mirror it to the transition foreground color
            FirstColorSegmentBackground = new Color(RandomDriver.Random(255), RandomDriver.Random(255), RandomDriver.Random(255));
            LastTransitionForeground = FirstColorSegmentBackground;
            base.Opening();
        }

        public override void Display()
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
                        TextWriterWhereColor.WriteWhere(new string(' ', PowerLineLength), 0, Top, Color.Empty, FirstColorSegmentBackground);
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

    }
}
