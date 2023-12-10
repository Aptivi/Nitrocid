//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using System.Text;
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.Kernel.Debugging;
using Terminaux.Sequences.Builder.Types;

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
        public virtual string Opening(SplashContext context)
        {
            var builder = new StringBuilder();
            DebugWriter.WriteDebug(DebugLevel.I, "Splash opening. Clearing console...");
            KernelColorTools.SetConsoleColor(KernelColorTools.GetColor(KernelColorType.Background), true);
            builder.Append(
                CsiSequences.GenerateCsiEraseInDisplay(2) +
                CsiSequences.GenerateCsiCursorPosition(1, 1)
            );
            return builder.ToString();
        }

        /// <inheritdoc/>
        public virtual string Display(SplashContext context)
        {
            try
            {
                Thread.Sleep(1);
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
            return "";
        }

        /// <inheritdoc/>
        public virtual string Closing(SplashContext context, out bool delayRequired)
        {
            var builder = new StringBuilder();
            DebugWriter.WriteDebug(DebugLevel.I, "Splash closing. Clearing console...");
            KernelColorTools.SetConsoleColor(KernelColorTools.GetColor(KernelColorType.Background), true);
            builder.Append(
                CsiSequences.GenerateCsiEraseInDisplay(2) +
                CsiSequences.GenerateCsiCursorPosition(1, 1)
            );
            delayRequired = false;
            return builder.ToString();
        }

        /// <inheritdoc/>
        public virtual string Report(int Progress, string ProgressReport, params object[] Vars) =>
            "";

        /// <inheritdoc/>
        public virtual string ReportWarning(int Progress, string WarningReport, Exception ExceptionInfo, params object[] Vars) =>
            "";

        /// <inheritdoc/>
        public virtual string ReportError(int Progress, string ErrorReport, Exception ExceptionInfo, params object[] Vars) =>
            "";

    }
}
