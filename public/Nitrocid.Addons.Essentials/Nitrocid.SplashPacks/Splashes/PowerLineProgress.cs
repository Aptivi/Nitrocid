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

using System;
using System.Text;
using System.Threading;
using Terminaux.Colors;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Splash;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Misc.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors.Transformation;
using Terminaux.Writer.CyclicWriters;
using Terminaux.Writer.CyclicWriters.Renderer;

namespace Nitrocid.SplashPacks.Splashes
{
    class SplashPowerLineProgress : BaseSplash, ISplash
    {

        // Standalone splash information
        public override string SplashName => "PowerLineProgress";

        public override bool RequiresBackground => true;

        public int ProgressWritePositionY =>
            SplashPackInit.SplashConfig.PowerLineProgressProgressTextLocation switch
            {
                (int)TextLocation.Top => 1,
                (int)TextLocation.Bottom => ConsoleWrapper.WindowHeight - 6,
                _ => 1,
            };

        private readonly Color FirstColorSegmentForeground = new(85, 255, 255);
        private readonly Color FirstColorSegmentBackground = new(43, 127, 127);
        private readonly Color SecondColorSegmentForeground = new(0, 0, 0);
        private readonly Color SecondColorSegmentBackground = new(85, 255, 255);
        private readonly Color LastTransitionForeground = new(85, 255, 255);
        private readonly char TransitionChar = Convert.ToChar(0xE0B0);

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
            // Variables
            var PresetStringBuilder = new StringBuilder();
            var builder = new StringBuilder();
            string RenderedText = ProgressReport.Truncate(ConsoleWrapper.WindowWidth - 5);

            // Percentage
            PresetStringBuilder.Append(FirstColorSegmentForeground.VTSequenceForeground);
            PresetStringBuilder.Append(FirstColorSegmentBackground.VTSequenceBackground);
            PresetStringBuilder.AppendFormat(" {0:000}% ", Progress);

            // Transition
            PresetStringBuilder.Append(FirstColorSegmentBackground.VTSequenceForeground);
            PresetStringBuilder.Append(SecondColorSegmentBackground.VTSequenceBackground);
            PresetStringBuilder.AppendFormat("{0}", TransitionChar);

            // Progress text
            PresetStringBuilder.Append(SecondColorSegmentForeground.VTSequenceForeground);
            PresetStringBuilder.Append(SecondColorSegmentBackground.VTSequenceBackground);
            if (ProgressErrored)
                PresetStringBuilder.AppendFormat(" [X]");
            if (ProgressWarning)
                PresetStringBuilder.AppendFormat(" [!]");
            PresetStringBuilder.AppendFormat(" {0} ", RenderedText);

            // Transition
            PresetStringBuilder.Append(LastTransitionForeground.VTSequenceForeground);
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.Background).VTSequenceBackground);
            PresetStringBuilder.AppendFormat("{0} ", TransitionChar);

            // Display the text and percentage
            builder.Append(
                KernelColorTools.GetColor(KernelColorType.Progress).VTSequenceForeground +
                TextWriterWhereColor.RenderWhere(PresetStringBuilder.ToString(), 0, ProgressWritePositionY, false, KernelColorType.Progress, Vars) +
                ConsoleClearing.GetClearLineToRightSequence()
            );

            // Display the progress bar
            Color progressColor =
                ColorTools.TryParseColor(SplashPackInit.SplashConfig.PowerLineProgressProgressColor) ?
                SplashPackInit.SplashConfig.PowerLineProgressProgressColor :
                KernelColorTools.GetColor(KernelColorType.Progress);
            var progress = new SimpleProgress(Progress, 100)
            {
                LeftMargin = 4,
                RightMargin = 4,
                ProgressActiveForegroundColor = progressColor,
                ProgressForegroundColor = TransformationTools.GetDarkBackground(progressColor),
                ProgressBackgroundColor = KernelColorTools.GetColor(KernelColorType.Background),
            };
            builder.Append(ContainerTools.RenderRenderable(progress, new(3, ConsoleWrapper.WindowHeight - 2)));
            return builder.ToString();
        }

    }
}
