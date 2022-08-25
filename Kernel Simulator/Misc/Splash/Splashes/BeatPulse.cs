
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
using KS.Misc.Animations.BeatPulse;
using KS.Misc.Writers.DebugWriters;

namespace KS.Misc.Splash.Splashes
{
    class SplashBeatPulse : ISplash
    {

        // Standalone splash information
        public string SplashName
        {
            get
            {
                return "BeatPulse";
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

        // BeatPulse-specific variables
        internal BeatPulseSettings BeatPulseSettings;
        internal Random RandomDriver;

        public SplashBeatPulse()
        {
            BeatPulseSettings = new BeatPulseSettings()
            {
                BeatPulse255Colors = false,
                BeatPulseTrueColor = true,
                BeatPulseBeatColor = 17.ToString(),
                BeatPulseCycleColors = true,
                BeatPulseDelay = 50,
                BeatPulseMaxSteps = 30,
                BeatPulseMinimumRedColorLevel = 0,
                BeatPulseMinimumGreenColorLevel = 0,
                BeatPulseMinimumBlueColorLevel = 0,
                BeatPulseMinimumColorLevel = 0,
                BeatPulseMaximumRedColorLevel = 255,
                BeatPulseMaximumGreenColorLevel = 255,
                BeatPulseMaximumBlueColorLevel = 255,
                BeatPulseMaximumColorLevel = 255
            };
        }

        // Actual logic
        public void Opening()
        {
            DebugWriter.Wdbg(DebugLevel.I, "Splash opening. Clearing console...");
            ConsoleBase.ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleBase.ConsoleWrapper.Clear();
            RandomDriver = new Random();
            BeatPulseSettings.RandomDriver = RandomDriver;
        }

        public void Display()
        {
            try
            {
                DebugWriter.Wdbg(DebugLevel.I, "Splash displaying.");

                // Loop until we got a closing notification
                while (!SplashClosing)
                    BeatPulse.Simulate(BeatPulseSettings);
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.Wdbg(DebugLevel.I, "Splash done.");
            }
        }

        public void Closing()
        {
            SplashClosing = true;
            DebugWriter.Wdbg(DebugLevel.I, "Splash closing. Clearing console...");
            ColorTools.SetConsoleColor(ColorTools.BackgroundColor, true);
            ConsoleBase.ConsoleWrapper.Clear();
        }

        public void Report(int Progress, string ProgressReport, params object[] Vars)
        {
        }

    }
}