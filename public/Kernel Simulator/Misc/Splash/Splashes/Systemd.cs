
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
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.Kernel.Debugging;
using KS.Misc.Writers.ConsoleWriters;

namespace KS.Misc.Splash.Splashes
{
    class SplashSystemd : ISplash
    {

        // Standalone splash information
        public string SplashName => "systemd";

        private SplashInfo Info => SplashManager.Splashes[SplashName];

        // Property implementations
        public bool SplashClosing { get; set; }

        public bool SplashDisplaysProgress => Info.DisplaysProgress;

        // Private variables
        private int IndicatorLeft;
        private int IndicatorTop;
        private bool Beginning = true;

        // Actual logic
        public void Opening()
        {
            Beginning = true;
            DebugWriter.WriteDebug(DebugLevel.I, "Splash opening. Clearing console...");
            ConsoleBase.ConsoleWrapper.Clear();
        }

        public void Display()
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");
                IndicatorLeft = ConsoleBase.ConsoleWrapper.CursorLeft + 2;
                IndicatorTop = ConsoleBase.ConsoleWrapper.CursorTop;
                while (!SplashClosing)
                    Thread.Sleep(1);
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
        }

        public void Closing()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Splash closing. Clearing console...");
            ConsoleBase.ConsoleWrapper.Clear();
        }

        public void Report(int Progress, string ProgressReport, params object[] Vars)
        {
            if (!Beginning)
                TextWriterWhereColor.WriteWhere("  OK  ", IndicatorLeft, IndicatorTop, true, KernelColorType.Success);
            TextWriterColor.Write($" [      ] {ProgressReport}", Vars);
            if (!Beginning)
            {
                IndicatorLeft = 2;
                IndicatorTop = ConsoleBase.ConsoleWrapper.CursorTop - 1;
            }
            Beginning = false;
        }

        public void ReportError(int Progress, string ErrorReport, Exception ExceptionInfo, params object[] Vars)
        {
            if (!Beginning)
                TextWriterWhereColor.WriteWhere("FAILED", IndicatorLeft, IndicatorTop, true, KernelColorType.Error);
            TextWriterColor.Write($" [      ] {ErrorReport}", Vars);
            if (!Beginning)
            {
                IndicatorLeft = 2;
                IndicatorTop = ConsoleBase.ConsoleWrapper.CursorTop - 1;
            }
            Beginning = false;
        }

    }
}
