
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
using KS.ConsoleBase.Colors;
using KS.Misc.Animations.EdgePulse;
using KS.Misc.Writers.DebugWriters;

namespace KS.Misc.Splash.Splashes
{
    class SplashEdgePulse : ISplash
    {

        // Standalone splash information
        public string SplashName
        {
            get
            {
                return "EdgePulse";
            }
        }

        private SplashInfo Info
        {
            get
            {
                return SplashManager.Splashes[SplashName];
            }
        }

        // Property implementations
        public bool SplashClosing { get; set; }

        public bool SplashDisplaysProgress
        {
            get
            {
                return Info.DisplaysProgress;
            }
        }

        // EdgePulse-specific variables
        internal EdgePulseSettings EdgePulseSettings;
        internal Random RandomDriver;

        public SplashEdgePulse()
        {
            EdgePulseSettings = new EdgePulseSettings()
            {
                EdgePulseDelay = 50,
                EdgePulseMaxSteps = 30,
                EdgePulseMinimumRedColorLevel = 0,
                EdgePulseMinimumGreenColorLevel = 0,
                EdgePulseMinimumBlueColorLevel = 0,
                EdgePulseMaximumRedColorLevel = 255,
                EdgePulseMaximumGreenColorLevel = 255,
                EdgePulseMaximumBlueColorLevel = 255
            };
        }

        // Actual logic
        public void Opening()
        {
            DebugWriter.Wdbg(DebugLevel.I, "Splash opening. Clearing console...");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            RandomDriver = new Random();
            EdgePulseSettings.RandomDriver = RandomDriver;
        }

        public void Display()
        {
            try
            {
                DebugWriter.Wdbg(DebugLevel.I, "Splash displaying.");

                // Loop until we got a closing notification
                while (!SplashClosing)
                    EdgePulse.Simulate(EdgePulseSettings);
            }
            catch (ThreadInterruptedException ex)
            {
                DebugWriter.Wdbg(DebugLevel.I, "Splash done.");
            }
        }

        public void Closing()
        {
            SplashClosing = true;
            DebugWriter.Wdbg(DebugLevel.I, "Splash closing. Clearing console...");
            ColorTools.SetConsoleColor(ColorTools.BackgroundColor, true);
            Console.Clear();
        }

        public void Report(int Progress, string ProgressReport, params object[] Vars)
        {
        }

    }
}