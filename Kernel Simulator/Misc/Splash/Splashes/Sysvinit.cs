
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
using KS.Misc.Writers.ConsoleWriters;

namespace KS.Misc.Splash.Splashes
{
    class SplashSysvinit : ISplash
    {

        // Standalone splash information
        public string SplashName
        {
            get
            {
                return "sysvinit";
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

        // Private variables
        private bool Beginning = true;

        // Actual logic
        public void Opening()
        {
            Beginning = true;
            DebugWriter.Wdbg(DebugLevel.I, "Splash opening. Clearing console...");
            ConsoleBase.ConsoleWrapper.Clear();
        }

        public void Display()
        {
            try
            {
                DebugWriter.Wdbg(DebugLevel.I, "Splash displaying.");
                while (!SplashClosing)
                    Thread.Sleep(1);
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
            ConsoleBase.ConsoleWrapper.Clear();
        }

        public void Report(int Progress, string ProgressReport, params object[] Vars)
        {
            if (!Beginning)
                TextWriterColor.Write(".", true, ColorTools.ColTypes.Neutral);
            TextWriterColor.Write($"{ProgressReport}:", false, ColorTools.ColTypes.Neutral, Vars);
            Beginning = false;
        }

    }
}