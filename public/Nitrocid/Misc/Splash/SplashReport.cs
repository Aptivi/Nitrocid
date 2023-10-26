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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Kernel.Journaling;
using KS.Kernel.Time;
using KS.Kernel.Time.Renderers;
using KS.Languages;
using KS.Misc.Text;
using System;
using System.Collections.Generic;

namespace KS.Misc.Splash
{
    /// <summary>
    /// Splash reporting module
    /// </summary>
    public static class SplashReport
    {

        internal static int _Progress = 0;
        internal static string _ProgressText = "";
        internal static bool _KernelBooted = false;
        internal static bool _InSplash = false;
        internal static readonly List<string> logBuffer = new();

        /// <summary>
        /// The progress indicator of the kernel 
        /// </summary>
        public static int Progress =>
            _Progress;

        /// <summary>
        /// The progress text to indicate how did the kernel progress
        /// </summary>
        public static string ProgressText =>
            _ProgressText;

        /// <summary>
        /// Did the kernel boot successfully?
        /// </summary>
        public static bool KernelBooted =>
            _KernelBooted;

        /// <summary>
        /// Did the kernel enter splash screen?
        /// </summary>
        public static bool InSplash =>
            _InSplash;

        /// <summary>
        /// Log buffer of the boot process
        /// </summary>
        public static string[] LogBuffer =>
            logBuffer.ToArray();

        /// <summary>
        /// Reports the progress for the splash screen while the kernel is booting.
        /// </summary>
        /// <param name="Text">The progress text to indicate how did the kernel progress</param>
        /// <param name="Vars">Variables to be expanded to text</param>
        /// <remarks>
        /// If the kernel has booted successfully, it will act like the normal printing command. If this routine was called during boot,<br></br>
        /// it will report the progress to the splash system. You can force it to report the progress by passing force.
        /// </remarks>
        public static void ReportProgress(string Text, params object[] Vars) => 
            ReportProgress(Text, 0, false, SplashManager.CurrentSplash, Vars);

        /// <summary>
        /// Reports the progress for the splash screen while the kernel is booting.
        /// </summary>
        /// <param name="Text">The progress text to indicate how did the kernel progress</param>
        /// <param name="Progress">The progress percentage from 0 to 100 to increment to the overall percentage</param>
        /// <param name="Vars">Variables to be expanded to text</param>
        /// <remarks>
        /// If the kernel has booted successfully, it will act like the normal printing command. If this routine was called during boot,<br></br>
        /// it will report the progress to the splash system. You can force it to report the progress by passing force.
        /// </remarks>
        public static void ReportProgress(string Text, int Progress, params object[] Vars) => 
            ReportProgress(Text, Progress, false, SplashManager.CurrentSplash, Vars);

        /// <summary>
        /// Reports the progress for the splash screen while the kernel is booting.
        /// </summary>
        /// <param name="Text">The progress text to indicate how did the kernel progress</param>
        /// <param name="Progress">The progress percentage from 0 to 100 to increment to the overall percentage</param>
        /// <param name="force">Force report progress to splash</param>
        /// <param name="splash">Splash interface</param>
        /// <param name="Vars">Variables to be expanded to text</param>
        /// <remarks>
        /// If the kernel has booted successfully, it will act like the normal printing command. If this routine was called during boot,<br></br>
        /// it will report the progress to the splash system. You can force it to report the progress by passing force.
        /// </remarks>
        public static void ReportProgress(string Text, int Progress, bool force = false, ISplash splash = null, params object[] Vars)
        {
            if (!KernelBooted && InSplash || force)
            {
                // Check the progress value
                if (Progress < 0)
                    Progress = 0;
                if (Progress > 100)
                    Progress = 100;

                // Increment and check
                _Progress += Progress;
                if (_Progress >= 100)
                    _Progress = 100;

                // Set the progress text
                _ProgressText = Text;

                // Report it
                if (SplashManager.CurrentSplashInfo.DisplaysProgress)
                {
                    if (SplashManager.EnableSplash && splash != null)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Invoking splash to report {0}...", Text);
                        splash.Report(_Progress, Text, Vars);
                    }
                    else if (!KernelEntry.QuietKernel)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Kernel not booted and not quiet. Reporting {0}...", Text);
                        TextWriterColor.WriteKernelColor($"  [{_Progress}%] {Text}", true, KernelColorType.Tip, Vars);
                    }
                }

                // Add to the log buffer
                logBuffer.Add($" [{TimeDateRenderers.Render(FormatType.Short)}] [{_Progress}%] Info: {TextTools.FormatString(Text, Vars)}");
            }
            else if (KernelBooted)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Kernel booted. Reporting {0}...", Text);
                TextWriterColor.WriteKernelColor(Text, true, KernelColorType.Tip, Vars);
            }
            JournalManager.WriteJournal(Text, Vars);
        }

        /// <summary>
        /// Reports the progress for the splash screen while the kernel is booting.
        /// </summary>
        /// <param name="Text">The progress text to indicate how did the kernel progress</param>
        /// <param name="Vars">Variables to be expanded to text</param>
        /// <remarks>
        /// If the kernel has booted successfully, it will act like the normal printing command. If this routine was called during boot,<br></br>
        /// it will report the progress to the splash system. You can force it to report the progress by passing force.
        /// </remarks>
        public static void ReportProgressWarning(string Text, params object[] Vars) =>
            ReportProgressWarning(Text, false, SplashManager.CurrentSplash, null, Vars);

        /// <summary>
        /// Reports the progress for the splash screen while the kernel is booting.
        /// </summary>
        /// <param name="Text">The progress text to indicate how did the kernel progress</param>
        /// <param name="exception">Exception information</param>
        /// <param name="Vars">Variables to be expanded to text</param>
        /// <remarks>
        /// If the kernel has booted successfully, it will act like the normal printing command. If this routine was called during boot,<br></br>
        /// it will report the progress to the splash system. You can force it to report the progress by passing force.
        /// </remarks>
        public static void ReportProgressWarning(string Text, Exception exception, params object[] Vars) =>
            ReportProgressWarning(Text, false, SplashManager.CurrentSplash, exception, Vars);

        /// <summary>
        /// Reports the progress for the splash screen while the kernel is booting.
        /// </summary>
        /// <param name="Text">The progress text to indicate how did the kernel progress</param>
        /// <param name="force">Force report progress to splash</param>
        /// <param name="splash">Splash interface</param>
        /// <param name="exception">Exception information</param>
        /// <param name="Vars">Variables to be expanded to text</param>
        /// <remarks>
        /// If the kernel has booted successfully, it will act like the normal printing command. If this routine was called during boot,<br></br>
        /// it will report the progress to the splash system. You can force it to report the progress by passing force.
        /// </remarks>
        public static void ReportProgressWarning(string Text, bool force = false, ISplash splash = null, Exception exception = null, params object[] Vars)
        {
            if (!KernelBooted && InSplash || force)
            {
                // Set the progress text
                _ProgressText = Text;

                // Report it
                if (SplashManager.CurrentSplashInfo.DisplaysProgress)
                {
                    if (SplashManager.EnableSplash && splash != null)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Invoking splash to report {0}...", Text);
                        splash.ReportWarning(_Progress, Text, exception, Vars);
                    }
                    else if (!KernelEntry.QuietKernel)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Kernel not booted and not quiet. Reporting {0}...", Text);
                        TextWriterColor.WriteKernelColor($"  [{_Progress}%] Warning: {Text}", true, KernelColorType.Warning, Vars);
                    }
                }

                // Add to the log buffer
                logBuffer.Add($" [{TimeDateRenderers.Render(FormatType.Short)}] [{_Progress}%] Warning: {TextTools.FormatString(Text, Vars)}");
            }
            else if (KernelBooted)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Kernel booted. Reporting {0}...", Text);
                TextWriterColor.WriteKernelColor(Text, true, KernelColorType.Warning, Vars);
            }
            JournalManager.WriteJournal(Text, JournalStatus.Warning, Vars);
        }

        /// <summary>
        /// Reports the progress for the splash screen while the kernel is booting.
        /// </summary>
        /// <param name="Text">The progress text to indicate how did the kernel progress</param>
        /// <param name="Vars">Variables to be expanded to text</param>
        /// <remarks>
        /// If the kernel has booted successfully, it will act like the normal printing command. If this routine was called during boot,<br></br>
        /// it will report the progress to the splash system. You can force it to report the progress by passing force.
        /// </remarks>
        public static void ReportProgressError(string Text, params object[] Vars) =>
            ReportProgressError(Text, false, SplashManager.CurrentSplash, null, Vars);

        /// <summary>
        /// Reports the progress for the splash screen while the kernel is booting.
        /// </summary>
        /// <param name="Text">The progress text to indicate how did the kernel progress</param>
        /// <param name="exception">Exception information</param>
        /// <param name="Vars">Variables to be expanded to text</param>
        /// <remarks>
        /// If the kernel has booted successfully, it will act like the normal printing command. If this routine was called during boot,<br></br>
        /// it will report the progress to the splash system. You can force it to report the progress by passing force.
        /// </remarks>
        public static void ReportProgressError(string Text, Exception exception, params object[] Vars) =>
            ReportProgressError(Text, false, SplashManager.CurrentSplash, exception, Vars);

        /// <summary>
        /// Reports the progress for the splash screen while the kernel is booting.
        /// </summary>
        /// <param name="Text">The progress text to indicate how did the kernel progress</param>
        /// <param name="force">Force report progress to splash</param>
        /// <param name="splash">Splash interface</param>
        /// <param name="exception">Exception information</param>
        /// <param name="Vars">Variables to be expanded to text</param>
        /// <remarks>
        /// If the kernel has booted successfully, it will act like the normal printing command. If this routine was called during boot,<br></br>
        /// it will report the progress to the splash system. You can force it to report the progress by passing force.
        /// </remarks>
        public static void ReportProgressError(string Text, bool force = false, ISplash splash = null, Exception exception = null, params object[] Vars)
        {
            if (!KernelBooted && InSplash || force)
            {
                // Set the progress text
                _ProgressText = Text;

                // Report it
                if (SplashManager.CurrentSplashInfo.DisplaysProgress)
                {
                    if (SplashManager.EnableSplash && splash != null)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Invoking splash to report {0}...", Text);
                        splash.ReportError(_Progress, Text, exception, Vars);
                    }
                    else if (!KernelEntry.QuietKernel)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Kernel not booted and not quiet. Reporting {0}...", Text);
                        TextWriterColor.WriteKernelColor($"  [{_Progress}%] Error: {Text}", true, KernelColorType.Error, Vars);
                    }
                }

                // Add to the log buffer
                logBuffer.Add($" [{TimeDateRenderers.Render(FormatType.Short)}] [{_Progress}%] Error: {TextTools.FormatString(Text, Vars)}");
            }
            else if (KernelBooted)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Kernel booted. Reporting {0}...", Text);
                TextWriterColor.WriteKernelColor(Text, true, KernelColorType.Error, Vars);
            }
            JournalManager.WriteJournal(Text, JournalStatus.Error, Vars);
        }

        /// <summary>
        /// Resets the splash progress report area with the generic loading text
        /// </summary>
        public static void ResetProgressReportArea() =>
            ResetProgressReportArea(null);

        /// <summary>
        /// Resets the splash progress report area with the generic loading text
        /// </summary>
        /// <param name="splash">Splash screen instance</param>
        public static void ResetProgressReportArea(ISplash splash = null) =>
            ReportProgress(Translate.DoTranslation("Loading..."), 0, false, splash ?? SplashManager.CurrentSplash);

    }
}
