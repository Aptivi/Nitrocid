
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
using KS.ConsoleBase.Colors;
using KS.Kernel.Debugging;
using KS.Misc.Animations.Pulse;

namespace KS.Misc.Splash.Splashes
{
    class SplashPulse : ISplash
    {

        // Standalone splash information
        public string SplashName => "Pulse";

        private SplashInfo Info => SplashManager.Splashes[SplashName];

        // Property implementations
        public bool SplashClosing { get; set; }

        public bool SplashDisplaysProgress => Info.DisplaysProgress;

        // Pulse-specific variables
        internal PulseSettings PulseSettings;

        public SplashPulse() => PulseSettings = new PulseSettings()
        {
            PulseDelay = 50,
            PulseMaxSteps = 30,
            PulseMinimumRedColorLevel = 0,
            PulseMinimumGreenColorLevel = 0,
            PulseMinimumBlueColorLevel = 0,
            PulseMaximumRedColorLevel = 255,
            PulseMaximumGreenColorLevel = 255,
            PulseMaximumBlueColorLevel = 255
        };

        // Actual logic
        public void Opening()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Splash opening. Clearing console...");
            ConsoleBase.ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleBase.ConsoleWrapper.Clear();
        }

        public void Display()
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");

                // Loop until we got a closing notification
                while (!SplashClosing)
                    Pulse.Simulate(PulseSettings);
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
        }

        public void Closing()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Splash closing. Clearing console...");
            ColorTools.SetConsoleColor(KernelColorType.Background, true);
            ConsoleBase.ConsoleWrapper.Clear();
        }

        public void Report(int Progress, string ProgressReport, params object[] Vars) { }

        public void ReportWarning(int Progress, string WarningReport, Exception ExceptionInfo, params object[] Vars) { }

        public void ReportError(int Progress, string ErrorReport, Exception ExceptionInfo, params object[] Vars) { }

    }
}
