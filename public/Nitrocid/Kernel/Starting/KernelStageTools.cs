﻿//
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
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Splash;
using System.Collections.Generic;
using System.Diagnostics;

namespace KS.Kernel.Starting
{
    internal static class KernelStageTools
    {
        internal static Stopwatch StageTimer = new();
        internal static List<KernelStage> Stages =
        [
            new KernelStage( /* Localizable */ "System initialization",             KernelStageActions.Stage01SystemInitialization),
            new KernelStage( /* Localizable */ "Kernel updates",                    KernelStageActions.Stage02KernelUpdates),
            new KernelStage( /* Localizable */ "Hardware detection",                KernelStageActions.Stage03HardwareProbe),
            new KernelStage( /* Localizable */ "Kernel modifications",              KernelStageActions.Stage04KernelModifications, false, false),
            new KernelStage( /* Localizable */ "Optional components",               KernelStageActions.Stage05OptionalComponents, false, false),
            new KernelStage( /* Localizable */ "User initialization",               KernelStageActions.Stage06UserInitialization, true, false),
            new KernelStage( /* Localizable */ "System integrity verification",     KernelStageActions.Stage07SysIntegrity),
        ];

        /// <summary>
        /// Show how much time a stage took on boot
        /// </summary>
        public static bool ShowStageFinishTimes =>
            Config.MainConfig.ShowStageFinishTimes;

        internal static void RunKernelStage(int stageNum)
        {
            int stageIdx = stageNum - 1;

            // Check to see if we have this stage
            if (stageIdx >= 0 && stageIdx < Stages.Count)
            {
                var stage = Stages[stageIdx];

                // Report the stage to the splash manager
                ReportNewStage(stageNum, $"- {Translate.DoTranslation("Stage")} {stageNum}: {Translate.DoTranslation(stage.StageName)}");
                if ((KernelEntry.SafeMode && stage.StageRunsInSafeMode) || !KernelEntry.SafeMode)
                {
                    if ((KernelEntry.Maintenance && stage.StageRunsInMaintenance) || !KernelEntry.Maintenance)
                        stage.StageAction();
                    else
                        SplashReport.ReportProgress(Translate.DoTranslation("Running in maintenance mode. Skipping stage..."));
                }
                else
                    SplashReport.ReportProgress(Translate.DoTranslation("Running in safe mode. Skipping stage..."));
                KernelEntry.CheckErrored();
            }
            else
                ReportNewStage(stageNum, "");
        }

        /// <summary>
        /// Reports the new kernel stage
        /// </summary>
        /// <param name="StageNumber">The stage number</param>
        /// <param name="StageText">The stage text</param>
        internal static void ReportNewStage(int StageNumber, string StageText)
        {
            // Show the stage finish times
            if (StageNumber <= 1)
            {
                if (ShowStageFinishTimes)
                {
                    SplashReport.ReportProgress(Translate.DoTranslation("Internal initialization finished in") + $" {StageTimer.Elapsed}");
                    StageTimer.Restart();
                }
            }
            else if (ShowStageFinishTimes)
            {
                SplashReport.ReportProgress(Translate.DoTranslation("Stage finished in") + $" {StageTimer.Elapsed}", 10);
                if (StageNumber > Stages.Count)
                {
                    StageTimer.Reset();
                    TextWriterColor.Write();
                }
                else
                    StageTimer.Restart();
            }

            // Actually report the stage
            if (StageNumber >= 1 & StageNumber <= Stages.Count)
            {
                if (!SplashManager.EnableSplash & !KernelEntry.QuietKernel)
                {
                    TextWriterColor.Write();
                    SeparatorWriterColor.WriteSeparatorKernelColor(StageText, false, KernelColorType.Stage);
                }
                DebugWriter.WriteDebug(DebugLevel.I, $"- Kernel stage {StageNumber} | Text: {StageText}");
            }
        }
    }
}