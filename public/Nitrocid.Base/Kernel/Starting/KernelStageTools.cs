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
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Splash;

namespace Nitrocid.Base.Kernel.Starting
{
    internal static class KernelStageTools
    {
        internal static Stopwatch StageTimer = new();

        internal static List<KernelStage> Stages =>
        [
            new KernelStage(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_STAGE1_DESC"), KernelStageActions.Stage01SystemInitialization),
            new KernelStage(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_STAGE2_DESC"), KernelStageActions.Stage02KernelUpdates),
            new KernelStage(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_STAGE3_DESC"), KernelStageActions.Stage03HardwareProbe),
            new KernelStage(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_STAGE4_DESC"), KernelStageActions.Stage04OptionalComponents, false, false),
            new KernelStage(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_STAGE5_DESC"), KernelStageActions.Stage05UserInitialization, true, false),
            new KernelStage(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_STAGE6_DESC"), KernelStageActions.Stage06SysIntegrity),
            new KernelStage(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_STAGE7_DESC"), KernelStageActions.Stage07Bootables, false, false),
        ];

        internal static void RunKernelStage(int stageNum)
        {
            int stageIdx = stageNum - 1;

            // Check to see if we have this stage
            if (stageIdx >= 0 && stageIdx < Stages.Count)
            {
                var stage = Stages[stageIdx];

                // Report the stage to the splash manager
                ReportNewStage(stageNum, $"{LanguageTools.GetLocalized("NKS_KERNEL_STARTING_STAGE")} {stageNum}: {stage.StageName}");
                if (KernelEntry.SafeMode && stage.StageRunsInSafeMode || !KernelEntry.SafeMode)
                {
                    if (KernelEntry.Maintenance && stage.StageRunsInMaintenance || !KernelEntry.Maintenance)
                        stage.StageAction();
                    else
                        SplashReport.ReportProgress(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_SKIPSTAGE_MAINTENANCE"));
                }
                else
                    SplashReport.ReportProgress(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_SKIPSTAGE_SAFEMODE"));
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
                    SplashReport.ReportProgress(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_STAGE1_FINISHEDIN") + $" {StageTimer.Elapsed}");
                    StageTimer.Restart();
                }
            }
            else if (Config.MainConfig.ShowStageFinishTimes)
            {
                SplashReport.ReportProgress(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_STAGE_FINISHEDIN") + $" {StageTimer.Elapsed}", 10);
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
                    SeparatorWriterColor.WriteSeparatorColor(StageText, ThemeColorsTools.GetColor(ThemeColorType.Stage));
                }
                DebugWriter.WriteDebug(DebugLevel.I, $"- Kernel stage {StageNumber} | Text: {StageText}");
            }
        }
    }
}
