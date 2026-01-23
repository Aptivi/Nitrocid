//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel.Debugging;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;

namespace Nitrocid.Misc.Splash
{
    /// <summary>
    /// Base splash screen class
    /// </summary>
    public class BaseSplash : ISplash
    {

        /// <inheritdoc/>
        public virtual string SplashName => "Blank";

        /// <inheritdoc/>
        public virtual bool RequiresBackground => false;

        /// <summary>
        /// Whether the splash is closing. If true, the thread of which handles the display should close itself. <see cref="Closing(SplashContext, out bool)"/> should set this property to True.
        /// </summary>
        public static bool SplashClosing { get; internal set; }

        internal static bool initialBackgroundAllowed = false;

        internal virtual SplashInfo Info => SplashManager.GetSplashFromName(SplashName);

        // Actual logic
        /// <inheritdoc/>
        public virtual string Opening(SplashContext context)
        {
            var builder = new StringBuilder();
            DebugWriter.WriteDebug(DebugLevel.I, "Splash opening. Clearing console...");
            ConsoleColoring.SetConsoleColor(KernelColorTools.GetColor(KernelColorType.Background), true);
            builder.Append(
                CsiSequences.GenerateCsiCursorPosition(1, 1) +
                CsiSequences.GenerateCsiEraseInDisplay(0)
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
            ConsoleColoring.SetConsoleColor(KernelColorTools.GetColor(KernelColorType.Background), true);
            builder.Append(
                CsiSequences.GenerateCsiCursorPosition(1, 1) +
                CsiSequences.GenerateCsiEraseInDisplay(0)
            );
            delayRequired = false;
            return builder.ToString();
        }

        /// <inheritdoc/>
        public virtual string Report(int Progress, string ProgressReport, params object[] Vars) =>
            "";

        /// <inheritdoc/>
        public virtual string ReportWarning(int Progress, string WarningReport, Exception? ExceptionInfo, params object[] Vars) =>
            "";

        /// <inheritdoc/>
        public virtual string ReportError(int Progress, string ErrorReport, Exception? ExceptionInfo, params object[] Vars) =>
            "";

    }
}
