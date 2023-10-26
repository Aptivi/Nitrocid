//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.Kernel.Debugging;

namespace KS.Misc.Splash
{
    /// <summary>
    /// Base splash screen class
    /// </summary>
    public class BaseSplash : ISplash
    {

        // Standalone splash information
        /// <inheritdoc/>
        public virtual string SplashName => "Blank";

        // Property implementations
        /// <inheritdoc/>
        public virtual bool SplashClosing { get; set; }

        /// <inheritdoc/>
        public virtual bool SplashDisplaysProgress => Info.DisplaysProgress;

        internal virtual SplashInfo Info => SplashManager.Splashes[SplashName];

        // Actual logic
        /// <inheritdoc/>
        public virtual void Opening(SplashContext context)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Splash opening. Clearing console...");
            KernelColorTools.LoadBack();
        }

        /// <inheritdoc/>
        public virtual void Display(SplashContext context)
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

        /// <inheritdoc/>
        public virtual void Closing(SplashContext context)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Splash closing. Clearing console...");
            KernelColorTools.LoadBack();
        }

        /// <inheritdoc/>
        public virtual void Report(int Progress, string ProgressReport, params object[] Vars) { }

        /// <inheritdoc/>
        public virtual void ReportWarning(int Progress, string WarningReport, Exception ExceptionInfo, params object[] Vars) { }

        /// <inheritdoc/>
        public virtual void ReportError(int Progress, string ErrorReport, Exception ExceptionInfo, params object[] Vars) { }

    }
}
