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
using System.Threading;
using Terminaux.Colors;
using System.Text;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Splash;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Misc.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors.Transformation;
using Terminaux.Writer.CyclicWriters.Renderer;
using Textify.General;
using Terminaux.Writer.CyclicWriters.Simple;

namespace Nitrocid.SplashPacks.Splashes
{
    class SplashProgress : BaseSplash, ISplash
    {

        // Standalone splash information
        public override string SplashName => "Progress";

        public int ProgressWritePositionX => 3;

        public int ProgressWritePositionY =>
            SplashPackInit.SplashConfig.ProgressProgressTextLocation switch
            {
                (int)TextLocation.Top => 1,
                (int)TextLocation.Bottom => ConsoleWrapper.WindowHeight - 4,
                _ => 1,
            };

        public int ProgressReportWritePositionX => 9;

        public int ProgressReportWritePositionY =>
            SplashPackInit.SplashConfig.ProgressProgressTextLocation switch
            {
                (int)TextLocation.Top => 1,
                (int)TextLocation.Bottom => ConsoleWrapper.WindowHeight - 4,
                _ => 1,
            };

        // Actual logic
        public override string Display(SplashContext context)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");

                // Display the progress bar
                return UpdateProgressReport(SplashReport.Progress, false, false, SplashReport.ProgressText);
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
            return "";
        }

        public override string Report(int Progress, string ProgressReport, params object[] Vars) =>
            UpdateProgressReport(Progress, false, false, ProgressReport, Vars);

        public override string ReportWarning(int Progress, string WarningReport, Exception? ExceptionInfo, params object[] Vars) =>
            UpdateProgressReport(Progress, false, true, WarningReport, Vars);

        public override string ReportError(int Progress, string ErrorReport, Exception? ExceptionInfo, params object[] Vars) =>
            UpdateProgressReport(Progress, true, false, ErrorReport, Vars);

        /// <summary>
        /// Updates the splash progress
        /// </summary>
        /// <param name="Progress">Progress percentage from 0 to 100</param>
        /// <param name="ProgressErrored">The progress error or not</param>
        /// <param name="ProgressWarning">The progress warning or not</param>
        /// <param name="ProgressReport">The progress text</param>
        /// <param name="Vars">Variables to be formatted in the text</param>
        public string UpdateProgressReport(int Progress, bool ProgressErrored, bool ProgressWarning, string ProgressReport, params object[] Vars)
        {
            var PresetStringBuilder = new StringBuilder();

            // Display the text and percentage
            var finalColor =
                ProgressErrored ? KernelColorTools.GetColor(KernelColorType.Error) :
                ProgressWarning ? KernelColorTools.GetColor(KernelColorType.Warning) :
                KernelColorTools.GetColor(KernelColorType.Progress);
            string indicator =
                ProgressErrored ? "[X] " :
                ProgressWarning ? "[!] " :
                "    ";
            string RenderedText = ProgressReport.FormatString(Vars).Truncate(ConsoleWrapper.WindowWidth - ProgressReportWritePositionX - ProgressWritePositionX - 3);
            PresetStringBuilder.Append(
                KernelColorTools.GetColor(KernelColorType.Progress).VTSequenceForeground() +
                TextWriterWhereColor.RenderWhere("{0,4:##0\\%}", ProgressWritePositionX, ProgressWritePositionY, true, vars: Progress) +
                finalColor.VTSequenceForeground() +
                TextWriterWhereColor.RenderWhere($"{indicator}{RenderedText}", ProgressReportWritePositionX, ProgressReportWritePositionY, false) +
                ConsoleClearing.GetClearLineToRightSequence()
            );

            // Display the progress bar
            Color progressColor =
                ColorTools.TryParseColor(SplashPackInit.SplashConfig.ProgressProgressColor) ?
                SplashPackInit.SplashConfig.ProgressProgressColor :
                KernelColorTools.GetColor(KernelColorType.Progress);
            var progress = new SimpleProgress(Progress, 100)
            {
                Width = ConsoleWrapper.WindowWidth - 8,
                ProgressActiveForegroundColor = progressColor,
                ProgressForegroundColor = TransformationTools.GetDarkBackground(progressColor),
                ProgressBackgroundColor = KernelColorTools.GetColor(KernelColorType.Background),
                ShowPercentage = false,
            };
            PresetStringBuilder.Append(RendererTools.RenderRenderable(progress, new(3, ConsoleWrapper.WindowHeight - 2)));
            return PresetStringBuilder.ToString();
        }

    }
}
