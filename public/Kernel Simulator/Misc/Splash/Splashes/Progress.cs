
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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
using System.Threading;
using ColorSeq;
using Extensification.StringExts;
using KS.ConsoleBase;
using KS.Kernel.Debugging;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;
using KS.ConsoleBase.Colors;

namespace KS.Misc.Splash.Splashes
{
    class SplashProgress : ISplash
    {

        // Standalone splash information
        public string SplashName => "Progress";

        private SplashInfo Info => SplashManager.Splashes[SplashName];

        // Property implementations
        public bool SplashClosing { get; set; }

        public bool SplashDisplaysProgress => Info.DisplaysProgress;

        public int ProgressWritePositionX => 3;

        public int ProgressWritePositionY
        {
            get
            {
                switch (SplashSettings.ProgressProgressTextLocation)
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

        public int ProgressReportWritePositionX => 9;

        public int ProgressReportWritePositionY
        {
            get
            {
                switch (SplashSettings.ProgressProgressTextLocation)
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
                    Thread.Sleep(1);
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
        }

        public void Closing()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Splash closing. Clearing console...");
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
            // Display the text and percentage
            string RenderedText = ProgressReport.Truncate(ConsoleWrapper.WindowWidth - ProgressReportWritePositionX - ProgressWritePositionX - 3);
            TextWriterWhereColor.WriteWhere("{0}%", ProgressWritePositionX, ProgressWritePositionY, true, KernelColorType.Progress, Progress.ToString().PadLeft(3));
            TextWriterWhereColor.WriteWhere($"{(ProgressErrored ? "[X] " : "")}{RenderedText}", ProgressReportWritePositionX, ProgressReportWritePositionY, false, KernelColorType.NeutralText, Vars);
            ConsoleExtensions.ClearLineToRight();

            // Display the progress bar
            if (!string.IsNullOrEmpty(SplashSettings.ProgressProgressColor) & ColorTools.TryParseColor(SplashSettings.ProgressProgressColor))
            {
                var ProgressColor = new Color(SplashSettings.ProgressProgressColor);
                ProgressBarColor.WriteProgress(Progress, 4, ConsoleWrapper.WindowHeight - 4, ProgressColor);
            }
            else
            {
                ProgressBarColor.WriteProgress(Progress, 4, ConsoleWrapper.WindowHeight - 4);
            }
        }

    }
}
