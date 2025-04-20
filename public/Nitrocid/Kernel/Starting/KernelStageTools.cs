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

using System.Collections.Generic;
using System.Diagnostics;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Splash;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Kernel.Starting
{
    internal static class KernelStageTools
    {
        internal static Stopwatch StageTimer = new();
        internal static List<KernelStage> Stages =
        [
            new KernelStage( /* Localizable */ "System initialization", KernelStageActions.Stage01SystemInitialization),
            new KernelStage( /* Localizable */ "Kernel updates", KernelStageActions.Stage02KernelUpdates),
            new KernelStage( /* Localizable */ "Hardware detection", KernelStageActions.Stage03HardwareProbe),
            new KernelStage( /* Localizable */ "Optional components", KernelStageActions.Stage04OptionalComponents, false, false),
            new KernelStage( /* Localizable */ "User initialization", KernelStageActions.Stage05UserInitialization, true, false),
            new KernelStage( /* Localizable */ "System integrity verification", KernelStageActions.Stage06SysIntegrity),
            new KernelStage( /* Localizable */ "Multiple environments", KernelStageActions.Stage07Bootables, false, false),
        ];

        internal static void RunKernelStage(int stageNum)
        {
            int stageIdx = stageNum - 1;

            // Check to see if we have this stage
            if (stageIdx >= 0 && stageIdx < Stages.Count)
            {
                var stage = Stages[stageIdx];

                // Report the stage to the splash manager
                ReportNewStage(stageNum, $"{Translate.DoTranslation("Stage")} {stageNum}: {Translate.DoTranslation(stage.StageName)}");
                if (KernelEntry.SafeMode && stage.StageRunsInSafeMode || !KernelEntry.SafeMode)
                {
                    if (KernelEntry.Maintenance && stage.StageRunsInMaintenance || !KernelEntry.Maintenance)
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

        internal static void ReportNewStage(int StageNumber, string StageText)
        {
            // Show the stage finish times
            if (StageNumber <= 1)
            {
                if (Config.MainConfig.ShowStageFinishTimes)
                {
                    SplashReport.ReportProgress(Translate.DoTranslation("Internal initialization finished in") + $" {StageTimer.Elapsed}");
                    StageTimer.Restart();
                }
            }
            else if (Config.MainConfig.ShowStageFinishTimes)
            {
                SplashReport.ReportProgress(Translate.DoTranslation("Stage finished in") + $" {StageTimer.Elapsed}", 10);
                if (StageNumber > Stages.Count)
                {
                    StageTimer.Reset();
                    TextWriterRaw.Write();
                }
                else
                    StageTimer.Restart();
            }

            // Actually report the stage
            if (StageNumber >= 1 & StageNumber <= Stages.Count)
            {
                if (!Config.MainConfig.EnableSplash & !KernelEntry.QuietKernel)
                {
                    TextWriterRaw.Write();
                    SeparatorWriterColor.WriteSeparatorColor(StageText, KernelColorTools.GetColor(KernelColorType.Stage));
                }
                DebugWriter.WriteDebug(DebugLevel.I, $"- Kernel stage {StageNumber} | Text: {StageText}");
            }
        }
    }
}
