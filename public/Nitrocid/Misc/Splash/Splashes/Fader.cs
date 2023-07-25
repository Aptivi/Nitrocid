
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
using ColorSeq;
using KS.Kernel.Debugging;
using KS.ConsoleBase.Colors;

namespace KS.Misc.Splash.Splashes
{
    class SplashFader : ISplash
    {

        // Standalone splash information
        public string SplashName => "Fader";

        private SplashInfo Info => SplashManager.Splashes[SplashName];

        // Property implementations
        public bool SplashClosing { get; set; }

        public bool SplashDisplaysProgress => Info.DisplaysProgress;

        // Fader-specific variables
        internal Animations.Fader.FaderSettings FaderSettingsInstance;

        public SplashFader() => FaderSettingsInstance = new Animations.Fader.FaderSettings()
        {
            FaderDelay = 50,
            FaderWrite = "Nitrocid KS",
            FaderBackgroundColor = new Color(ConsoleColors.Black).PlainSequence,
            FaderFadeOutDelay = 3000,
            FaderMaxSteps = 30,
            FaderMinimumRedColorLevel = 0,
            FaderMinimumGreenColorLevel = 0,
            FaderMinimumBlueColorLevel = 0,
            FaderMaximumRedColorLevel = 255,
            FaderMaximumGreenColorLevel = 255,
            FaderMaximumBlueColorLevel = 255,
        };

        // Actual logic
        public void Opening()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Splash opening. Clearing console...");
            ConsoleBase.ConsoleWrapper.Clear();
        }

        public void Display()
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");
                while (!SplashClosing)
                    Animations.Fader.Fader.Simulate(FaderSettingsInstance);
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
        }

        public void Closing()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Splash closing. Clearing console...");
            KernelColorTools.SetConsoleColor(KernelColorType.Background, true);
            ConsoleBase.ConsoleWrapper.Clear();
        }

        public void Report(int Progress, string ProgressReport, params object[] Vars) { }

        public void ReportWarning(int Progress, string WarningReport, Exception ExceptionInfo, params object[] Vars) { }

        public void ReportError(int Progress, string ErrorReport, Exception ExceptionInfo, params object[] Vars) { }

    }
}
