
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
using System.Text;
using System.Threading;
using ColorSeq;
using Extensification.StringExts;
using KS.ConsoleBase;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;
using KS.ConsoleBase.Colors;

namespace KS.Misc.Splash.Splashes
{
    class SplashPowerLineProgress : ISplash
    {

        // Standalone splash information
        public string SplashName => "PowerLineProgress";

        private SplashInfo Info => SplashManager.Splashes[SplashName];

        // Property implementations
        public bool SplashClosing { get; set; }

        public bool SplashDisplaysProgress => Info.DisplaysProgress;

        public int ProgressWritePositionY
        {
            get
            {
                switch (SplashSettings.PowerLineProgressProgressTextLocation)
                {
                    case TextLocation.Top:
                        {
                            return 1;
                        }
                    case TextLocation.Bottom:
                        {
                            return ConsoleWrapper.WindowHeight - 6;
                        }

                    default:
                        {
                            return 1;
                        }
                }
            }
        }

        private readonly Color FirstColorSegmentForeground = new(85, 255, 255);
        private readonly Color FirstColorSegmentBackground = new(43, 127, 127);
        private readonly Color SecondColorSegmentForeground = new(0, 0, 0);
        private readonly Color SecondColorSegmentBackground = new(85, 255, 255);
        private readonly Color LastTransitionForeground = new(85, 255, 255);
        private readonly char TransitionChar = Convert.ToChar(0xE0B0);

        // Actual logic
        public void Opening()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Splash opening. Clearing console...");
            ConsoleWrapper.Clear();
        }

        public void Display()
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");

                // Display the progress bar
                UpdateProgressReport(SplashReport.Progress, false, SplashReport.ProgressText);

                // Loop until closing
                while (!SplashClosing)
                    Thread.Sleep(10);
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
        }

        public void Closing()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Splash closing. Clearing console...");
            ColorTools.SetConsoleColor(KernelColorType.NeutralText);
            ColorTools.SetConsoleColor(KernelColorType.Background, true);
            ConsoleWrapper.Clear();
        }

        public void Report(int Progress, string ProgressReport, params object[] Vars) => 
            UpdateProgressReport(Progress, false, ProgressReport, Vars);

        public void ReportError(int Progress, string ErrorReport, Exception ExceptionInfo, params object[] Vars) =>
            UpdateProgressReport(Progress, true, ErrorReport, Vars);

        /// <summary>
        /// Updates the splash progress
        /// </summary>
        /// <param name="Progress">Progress percentage from 0 to 100</param>
        /// <param name="ProgressErrored">The progress error or not</param>
        /// <param name="ProgressReport">The progress text</param>
        /// <param name="Vars">Variables to be formatted in the text</param>
        public void UpdateProgressReport(int Progress, bool ProgressErrored, string ProgressReport, params object[] Vars)
        {
            // Variables
            var PresetStringBuilder = new StringBuilder();
            string RenderedText = ProgressReport.Truncate(ConsoleWrapper.WindowWidth - 5);

            // Percentage
            PresetStringBuilder.Append(FirstColorSegmentForeground.VTSequenceForeground);
            PresetStringBuilder.Append(FirstColorSegmentBackground.VTSequenceBackground);
            PresetStringBuilder.AppendFormat(" {0}% ", Progress.ToString().PadLeft(3));

            // Transition
            PresetStringBuilder.Append(FirstColorSegmentBackground.VTSequenceForeground);
            PresetStringBuilder.Append(SecondColorSegmentBackground.VTSequenceBackground);
            PresetStringBuilder.AppendFormat("{0}", TransitionChar);

            // Progress text
            PresetStringBuilder.Append(SecondColorSegmentForeground.VTSequenceForeground);
            PresetStringBuilder.Append(SecondColorSegmentBackground.VTSequenceBackground);
            if (ProgressErrored)
                PresetStringBuilder.AppendFormat(" [X]");
            PresetStringBuilder.AppendFormat(" {0} ", RenderedText);

            // Transition
            PresetStringBuilder.Append(LastTransitionForeground.VTSequenceForeground);
            PresetStringBuilder.Append(Flags.SetBackground ? ColorTools.GetColor(KernelColorType.Background).VTSequenceBackground : Convert.ToString(CharManager.GetEsc()) + $"[49m");
            PresetStringBuilder.AppendFormat("{0} ", TransitionChar);

            // Display the text and percentage
            TextWriterWhereColor.WriteWhere(PresetStringBuilder.ToString(), 0, ProgressWritePositionY, false, KernelColorType.Progress, Vars);
            ConsoleExtensions.ClearLineToRight();

            // Display the progress bar
            if (!string.IsNullOrEmpty(SplashSettings.PowerLineProgressProgressColor) & ColorTools.TryParseColor(SplashSettings.PowerLineProgressProgressColor))
            {
                var ProgressColor = new Color(SplashSettings.PowerLineProgressProgressColor);
                ProgressBarColor.WriteProgress(Progress, 4, ConsoleWrapper.WindowHeight - 4, ProgressColor);
            }
            else
            {
                ProgressBarColor.WriteProgress(Progress, 4, ConsoleWrapper.WindowHeight - 4);
            }
        }

    }
}
