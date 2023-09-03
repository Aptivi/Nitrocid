
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
using System.Text;
using System.Threading;
using KS.ConsoleBase;
using KS.Kernel.Debugging;
using KS.Misc.Text;
using KS.ConsoleBase.Colors;
using KS.Kernel.Configuration;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using Terminaux.Colors;

namespace KS.Misc.Splash.Splashes
{
    class SplashPowerLineProgress : BaseSplash, ISplash
    {

        // Standalone splash information
        public override string SplashName => "PowerLineProgress";

        public int ProgressWritePositionY
        {
            get
            {
                return Config.SplashConfig.PowerLineProgressProgressTextLocation switch
                {
                    (int)TextLocation.Top    => 1,
                    (int)TextLocation.Bottom => ConsoleWrapper.WindowHeight - 6,
                    _                        => 1,
                };
            }
        }

        private readonly Color FirstColorSegmentForeground = new(85, 255, 255);
        private readonly Color FirstColorSegmentBackground = new(43, 127, 127);
        private readonly Color SecondColorSegmentForeground = new(0, 0, 0);
        private readonly Color SecondColorSegmentBackground = new(85, 255, 255);
        private readonly Color LastTransitionForeground = new(85, 255, 255);
        private readonly char TransitionChar = Convert.ToChar(0xE0B0);

        // Actual logic
        public override void Display()
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");

                // Display the progress bar
                UpdateProgressReport(SplashReport.Progress, false, false, SplashReport.ProgressText);

                // Loop until closing
                while (!SplashClosing)
                    Thread.Sleep(10);
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
        }

        public override void Report(int Progress, string ProgressReport, params object[] Vars) => 
            UpdateProgressReport(Progress, false, false, ProgressReport, Vars);

        public override void ReportWarning(int Progress, string WarningReport, Exception ExceptionInfo, params object[] Vars) =>
            UpdateProgressReport(Progress, false, true, WarningReport, Vars);

        public override void ReportError(int Progress, string ErrorReport, Exception ExceptionInfo, params object[] Vars) =>
            UpdateProgressReport(Progress, true, false, ErrorReport, Vars);

        /// <summary>
        /// Updates the splash progress
        /// </summary>
        /// <param name="Progress">Progress percentage from 0 to 100</param>
        /// <param name="ProgressErrored">The progress error or not</param>
        /// <param name="ProgressWarning">The progress warning or not</param>
        /// <param name="ProgressReport">The progress text</param>
        /// <param name="Vars">Variables to be formatted in the text</param>
        public void UpdateProgressReport(int Progress, bool ProgressErrored, bool ProgressWarning, string ProgressReport, params object[] Vars)
        {
            // Variables
            var PresetStringBuilder = new StringBuilder();
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
            TextWriterWhereColor.WriteWhere(PresetStringBuilder.ToString(), 0, ProgressWritePositionY, false, KernelColorType.Progress, Vars);
            ConsoleExtensions.ClearLineToRight();

            // Display the progress bar
            if (!string.IsNullOrEmpty(Config.SplashConfig.PowerLineProgressProgressColor) & KernelColorTools.TryParseColor(Config.SplashConfig.PowerLineProgressProgressColor))
            {
                var ProgressColor = new Color(Config.SplashConfig.PowerLineProgressProgressColor);
                ProgressBarColor.WriteProgress(Progress, 4, ConsoleWrapper.WindowHeight - 4, ProgressColor);
            }
            else
            {
                ProgressBarColor.WriteProgress(Progress, 4, ConsoleWrapper.WindowHeight - 4);
            }
        }

    }
}
