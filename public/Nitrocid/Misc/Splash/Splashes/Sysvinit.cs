
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
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Debugging;

namespace KS.Misc.Splash.Splashes
{
    class SplashSysvinit : BaseSplash, ISplash
    {

        // Standalone splash information
        public override string SplashName => "sysvinit";

        // Private variables
        private bool Beginning = true;

        // Actual logic
        public override void Display()
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");
                while (!SplashClosing)
                    Thread.Sleep(1);
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
        }

        public override void Report(int Progress, string ProgressReport, params object[] Vars)
        {
            if (!Beginning)
                TextWriterColor.Write(".");
            TextWriterColor.Write($"{ProgressReport}:", false, KernelColorType.NeutralText, Vars);
            Beginning = false;
        }

        public override void ReportWarning(int Progress, string WarningReport, Exception ExceptionInfo, params object[] Vars) =>
            Report(Progress, WarningReport, Vars);

        public override void ReportError(int Progress, string ErrorReport, Exception ExceptionInfo, params object[] Vars) =>
            Report(Progress, ErrorReport, Vars);

    }
}
