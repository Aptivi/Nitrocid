
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
using KS.Kernel.Debugging;

namespace KS.Misc.Splash.Splashes
{
    class SplashFaderBack : ISplash
    {

        // Standalone splash information
        public string SplashName
        {
            get
            {
                return "FaderBack";
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

        internal Random RandomDriver = new();
        internal Animations.FaderBack.FaderBackSettings FaderBackSettingsInstance;

        public SplashFaderBack()
        {
            FaderBackSettingsInstance = new Animations.FaderBack.FaderBackSettings()
            {
                FaderBackDelay = 10,
                FaderBackFadeOutDelay = 3000,
                FaderBackMaxSteps = 30,
                FaderBackMinimumRedColorLevel = 0,
                FaderBackMinimumGreenColorLevel = 0,
                FaderBackMinimumBlueColorLevel = 0,
                FaderBackMaximumRedColorLevel = 255,
                FaderBackMaximumGreenColorLevel = 255,
                FaderBackMaximumBlueColorLevel = 255,
                RandomDriver = RandomDriver
            };
        }

        // Actual logic
        public void Opening()
        {
            DebugWriter.Wdbg(DebugLevel.I, "Splash opening. Clearing console...");
            ConsoleBase.ConsoleWrapper.Clear();
        }

        public void Display()
        {
            try
            {
                DebugWriter.Wdbg(DebugLevel.I, "Splash displaying.");
                while (!SplashClosing)
                    Animations.FaderBack.FaderBack.Simulate(FaderBackSettingsInstance);
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