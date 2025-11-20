//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Terminaux.Base.Buffered;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using System;
using System.Collections.Generic;
using Textify.General;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Time;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Journaling;

namespace Nitrocid.Base.Misc.Splash
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
        internal static readonly List<SplashReportInfo> logBuffer = [];

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
        public static SplashReportInfo[] LogBuffer =>
            [.. logBuffer];

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
            ReportProgress(Text, 0, false, null, SplashReportSeverity.Info, SplashManager.CurrentSplash, Vars);

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
            ReportProgress(Text, Progress, false, null, SplashReportSeverity.Info, SplashManager.CurrentSplash, Vars);

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
        public static void ReportProgressWarning(string Text, Exception? exception, params object[] Vars) =>
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
        public static void ReportProgressWarning(string Text, bool force = false, ISplash? splash = null, Exception? exception = null, params object[] Vars) =>
            ReportProgress(Text, 0, force, exception, SplashReportSeverity.Warning, splash, Vars);

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
        public static void ReportProgressError(string Text, Exception? exception, params object[] Vars) =>
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
        public static void ReportProgressError(string Text, bool force = false, ISplash? splash = null, Exception? exception = null, params object[] Vars) =>
            ReportProgress(Text, 0, force, exception, SplashReportSeverity.Error, splash, Vars);

        /// <summary>
        /// Reports the progress for the splash screen while the kernel is booting.
        /// </summary>
        /// <param name="Text">The progress text to indicate how did the kernel progress</param>
        /// <param name="Progress">The progress percentage from 0 to 100 to increment to the overall percentage</param>
        /// <param name="force">Force report progress to splash</param>
        /// <param name="exception">Exception to use for warnings and errors</param>
        /// <param name="severity">The severity of the report</param>
        /// <param name="splash">Splash interface. Use <see langword="null"/> to use default splash.</param>
        /// <param name="Vars">Variables to be expanded to text</param>
        /// <remarks>
        /// If the kernel has booted successfully, it will act like the normal printing command. If this routine was called during boot,<br></br>
        /// it will report the progress to the splash system. You can force it to report the progress by passing force.
        /// </remarks>
        public static void ReportProgress(string Text, int Progress, bool force = false, Exception? exception = null, SplashReportSeverity severity = SplashReportSeverity.Info, ISplash? splash = null, params object[] Vars)
        {
            ThemeColorType finalColor =
                severity == SplashReportSeverity.Warning ? ThemeColorType.Warning :
                severity == SplashReportSeverity.Error ? ThemeColorType.Error :
                ThemeColorType.Tip;
            if (!KernelBooted && !KernelEntry.QuietKernel && (Config.MainConfig.EnableSplash && InSplash || !Config.MainConfig.EnableSplash) || force)
            {
                // Check the progress value
                if (Progress < 0)
                    Progress = 0;
                if (Progress > 100)
                    Progress = 100;

                // Increment and check
                if (severity == SplashReportSeverity.Info)
                {
                    _Progress += Progress;
                    if (_Progress >= 100)
                        _Progress = 100;
                }

                // Set the progress text
                _ProgressText = Text.FormatString(Vars);

                // Report it
                if (SplashManager.CurrentSplashInfo.DisplaysProgress)
                {
                    if (Config.MainConfig.EnableSplash && splash != null)
                    {
                        var openingPart = new ScreenPart();
                        DebugWriter.WriteDebug(DebugLevel.I, "Invoking splash to report {0}...", vars: [Text]);
                        openingPart.AddDynamicText(() =>
                        {
                            return
                                severity == SplashReportSeverity.Warning ? splash.ReportWarning(_Progress, Text, exception, Vars) :
                                severity == SplashReportSeverity.Error ? splash.ReportError(_Progress, Text, exception, Vars) :
                                splash.Report(_Progress, Text, Vars);
                        });
                        if (SplashManager.splashScreen.CheckBufferedPart("Splash report"))
                            SplashManager.splashScreen.EditBufferedPart("Splash report", openingPart);
                        else
                            SplashManager.splashScreen.AddBufferedPart("Splash report", openingPart);
                    }
                    else if (!KernelEntry.QuietKernel)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Kernel not booted and not quiet. Reporting {0}...", vars: [Text]);
                        TextWriterColor.Write($"[{_Progress}%] {Text}", true, finalColor, Vars);
                    }
                }

                // Add to the log buffer
                var reportInfo = new SplashReportInfo(TimeDateTools.KernelDateTime, _Progress, severity, TextTools.FormatString(Text, Vars));
                logBuffer.Add(reportInfo);
            }
            else if (KernelBooted && !KernelEntry.QuietKernel && (Config.MainConfig.EnableSplash && !InSplash || !Config.MainConfig.EnableSplash))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Kernel booted or not in splash. Reporting {0}...", vars: [Text]);
                TextWriterColor.Write(Text, true, finalColor, Vars);
            }
            JournalManager.WriteJournal(Text, Vars);
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
        public static void ResetProgressReportArea(ISplash? splash = null) =>
            ReportProgress(LanguageTools.GetLocalized("NKS_MISC_SPLASH_DEFAULTMSG"), 0, false, splash ?? SplashManager.CurrentSplash);

    }
}
