//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using Terminaux.Colors;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Misc.Text.Probers.Placeholder;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel.Time;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Kernel.Threading;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Writer.FancyWriters.Tools;
using Nitrocid.Kernel.Configuration;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for ProgressClock
    /// </summary>
    public class ProgressClockDisplay : BaseScreensaver, IScreensaver
    {

        private Color ColorStorageHours = Color.Empty,
                      ColorStorageMinutes = Color.Empty,
                      ColorStorageSeconds = Color.Empty,
                      ColorStorage = Color.Empty;
        private long CurrentTicks;
        private string lastDate = "";

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "ProgressClock";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            CurrentTicks = ScreensaverPackInit.SaversConfig.ProgressClockCycleColorsTicks;
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Prepare colors
            int ProgressFillPositionHours, ProgressFillPositionMinutes, ProgressFillPositionSeconds;
            int InformationPositionHours, InformationPositionMinutes, InformationPositionSeconds;

            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Current tick: {0}", CurrentTicks);
            if (ScreensaverPackInit.SaversConfig.ProgressClockCycleColors)
            {
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Cycling colors...");
                if (CurrentTicks >= ScreensaverPackInit.SaversConfig.ProgressClockCycleColorsTicks)
                {
                    var type = ScreensaverPackInit.SaversConfig.ProgressClockTrueColor ? ColorType.TrueColor : ColorType.EightBitColor;
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Current tick equals the maximum ticks to change color.");
                    ColorStorageHours =
                        ColorTools.GetRandomColor(type, ScreensaverPackInit.SaversConfig.ProgressClockMinimumColorLevelHours, ScreensaverPackInit.SaversConfig.ProgressClockMaximumColorLevelHours,
                                                        ScreensaverPackInit.SaversConfig.ProgressClockMinimumRedColorLevelHours, ScreensaverPackInit.SaversConfig.ProgressClockMaximumRedColorLevelHours,
                                                        ScreensaverPackInit.SaversConfig.ProgressClockMinimumGreenColorLevelHours, ScreensaverPackInit.SaversConfig.ProgressClockMaximumGreenColorLevelHours,
                                                        ScreensaverPackInit.SaversConfig.ProgressClockMinimumBlueColorLevelHours, ScreensaverPackInit.SaversConfig.ProgressClockMaximumBlueColorLevelHours);
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (Hours) (R;G;B: {0};{1};{2})", ColorStorageHours.RGB.R, ColorStorageHours.RGB.G, ColorStorageHours.RGB.B);
                    ColorStorageMinutes =
                        ColorTools.GetRandomColor(type, ScreensaverPackInit.SaversConfig.ProgressClockMinimumColorLevelMinutes, ScreensaverPackInit.SaversConfig.ProgressClockMaximumColorLevelMinutes,
                                                        ScreensaverPackInit.SaversConfig.ProgressClockMinimumRedColorLevelMinutes, ScreensaverPackInit.SaversConfig.ProgressClockMaximumRedColorLevelMinutes,
                                                        ScreensaverPackInit.SaversConfig.ProgressClockMinimumGreenColorLevelMinutes, ScreensaverPackInit.SaversConfig.ProgressClockMaximumGreenColorLevelMinutes,
                                                        ScreensaverPackInit.SaversConfig.ProgressClockMinimumBlueColorLevelMinutes, ScreensaverPackInit.SaversConfig.ProgressClockMaximumBlueColorLevelMinutes);
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (Minutes) (R;G;B: {0};{1};{2})", ColorStorageMinutes.RGB.R, ColorStorageMinutes.RGB.G, ColorStorageMinutes.RGB.B);
                    ColorStorageSeconds =
                        ColorTools.GetRandomColor(type, ScreensaverPackInit.SaversConfig.ProgressClockMinimumColorLevelSeconds, ScreensaverPackInit.SaversConfig.ProgressClockMaximumColorLevelSeconds,
                                                        ScreensaverPackInit.SaversConfig.ProgressClockMinimumRedColorLevelSeconds, ScreensaverPackInit.SaversConfig.ProgressClockMaximumRedColorLevelSeconds,
                                                        ScreensaverPackInit.SaversConfig.ProgressClockMinimumGreenColorLevelSeconds, ScreensaverPackInit.SaversConfig.ProgressClockMaximumGreenColorLevelSeconds,
                                                        ScreensaverPackInit.SaversConfig.ProgressClockMinimumBlueColorLevelSeconds, ScreensaverPackInit.SaversConfig.ProgressClockMaximumBlueColorLevelSeconds);
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (Seconds) (R;G;B: {0};{1};{2})", ColorStorageSeconds.RGB.R, ColorStorageSeconds.RGB.G, ColorStorageSeconds.RGB.B);
                    ColorStorage =
                        ColorTools.GetRandomColor(type, ScreensaverPackInit.SaversConfig.ProgressClockMinimumColorLevel, ScreensaverPackInit.SaversConfig.ProgressClockMaximumColorLevel,
                                                        ScreensaverPackInit.SaversConfig.ProgressClockMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.ProgressClockMaximumRedColorLevel,
                                                        ScreensaverPackInit.SaversConfig.ProgressClockMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.ProgressClockMaximumGreenColorLevel,
                                                        ScreensaverPackInit.SaversConfig.ProgressClockMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.ProgressClockMaximumBlueColorLevel);
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", ColorStorage.RGB.R, ColorStorage.RGB.G, ColorStorage.RGB.B);
                    CurrentTicks = 0L;
                }
            }
            else
            {
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Parsing colors...");
                ColorStorageHours = new Color(ScreensaverPackInit.SaversConfig.ProgressClockHoursProgressColor);
                ColorStorageMinutes = new Color(ScreensaverPackInit.SaversConfig.ProgressClockMinutesProgressColor);
                ColorStorageSeconds = new Color(ScreensaverPackInit.SaversConfig.ProgressClockSecondsProgressColor);
                ColorStorage = new Color(ScreensaverPackInit.SaversConfig.ProgressClockProgressColor);
            }
            ProgressFillPositionHours = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 10;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Fill position for progress (Hours) {0}", ProgressFillPositionHours);
            ProgressFillPositionMinutes = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 1;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Fill position for progress (Minutes) {0}", ProgressFillPositionMinutes);
            ProgressFillPositionSeconds = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) + 8;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Fill position for progress (Seconds) {0}", ProgressFillPositionSeconds);
            InformationPositionHours = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 12;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Fill position for info (Hours) {0}", InformationPositionHours);
            InformationPositionMinutes = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 3;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Fill position for info (Minutes) {0}", InformationPositionMinutes);
            InformationPositionSeconds = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) + 6;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Fill position for info (Seconds) {0}", InformationPositionSeconds);

            // Populate the border settings
            var hoursBorder = new BorderSettings()
            {
                BorderUpperLeftCornerChar = ScreensaverPackInit.SaversConfig.ProgressClockUpperLeftCornerCharHours,
                BorderLowerLeftCornerChar = ScreensaverPackInit.SaversConfig.ProgressClockLowerLeftCornerCharHours,
                BorderUpperRightCornerChar = ScreensaverPackInit.SaversConfig.ProgressClockUpperRightCornerCharHours,
                BorderLowerRightCornerChar = ScreensaverPackInit.SaversConfig.ProgressClockLowerRightCornerCharHours,
                BorderUpperFrameChar = ScreensaverPackInit.SaversConfig.ProgressClockUpperFrameCharHours,
                BorderLowerFrameChar = ScreensaverPackInit.SaversConfig.ProgressClockLowerFrameCharHours,
                BorderLeftFrameChar = ScreensaverPackInit.SaversConfig.ProgressClockLeftFrameCharHours,
                BorderRightFrameChar = ScreensaverPackInit.SaversConfig.ProgressClockRightFrameCharHours,
            };
            var minutesBorder = new BorderSettings()
            {
                BorderUpperLeftCornerChar = ScreensaverPackInit.SaversConfig.ProgressClockUpperLeftCornerCharMinutes,
                BorderLowerLeftCornerChar = ScreensaverPackInit.SaversConfig.ProgressClockLowerLeftCornerCharMinutes,
                BorderUpperRightCornerChar = ScreensaverPackInit.SaversConfig.ProgressClockUpperRightCornerCharMinutes,
                BorderLowerRightCornerChar = ScreensaverPackInit.SaversConfig.ProgressClockLowerRightCornerCharMinutes,
                BorderUpperFrameChar = ScreensaverPackInit.SaversConfig.ProgressClockUpperFrameCharMinutes,
                BorderLowerFrameChar = ScreensaverPackInit.SaversConfig.ProgressClockLowerFrameCharMinutes,
                BorderLeftFrameChar = ScreensaverPackInit.SaversConfig.ProgressClockLeftFrameCharMinutes,
                BorderRightFrameChar = ScreensaverPackInit.SaversConfig.ProgressClockRightFrameCharMinutes,
            };
            var secondsBorder = new BorderSettings()
            {
                BorderUpperLeftCornerChar = ScreensaverPackInit.SaversConfig.ProgressClockUpperLeftCornerCharSeconds,
                BorderLowerLeftCornerChar = ScreensaverPackInit.SaversConfig.ProgressClockLowerLeftCornerCharSeconds,
                BorderUpperRightCornerChar = ScreensaverPackInit.SaversConfig.ProgressClockUpperRightCornerCharSeconds,
                BorderLowerRightCornerChar = ScreensaverPackInit.SaversConfig.ProgressClockLowerRightCornerCharSeconds,
                BorderUpperFrameChar = ScreensaverPackInit.SaversConfig.ProgressClockUpperFrameCharSeconds,
                BorderLowerFrameChar = ScreensaverPackInit.SaversConfig.ProgressClockLowerFrameCharSeconds,
                BorderLeftFrameChar = ScreensaverPackInit.SaversConfig.ProgressClockLeftFrameCharSeconds,
                BorderRightFrameChar = ScreensaverPackInit.SaversConfig.ProgressClockRightFrameCharSeconds,
            };

            // Render the progress clock bars
            if (!ConsoleResizeHandler.WasResized(false))
            {
                // Hours
                BoxFrameColor.WriteBoxFrame(4, (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 11, ConsoleWrapper.WindowWidth - 10, 1, hoursBorder, ColorStorageHours);

                // Minutes
                BoxFrameColor.WriteBoxFrame(4, (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 2, ConsoleWrapper.WindowWidth - 10, 1, minutesBorder, ColorStorageMinutes);

                // Seconds
                BoxFrameColor.WriteBoxFrame(4, (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) + 7, ConsoleWrapper.WindowWidth - 10, 1, secondsBorder, ColorStorageSeconds);

                // Fill progress for hours, minutes, and seconds
                if (TimeDateTools.KernelDateTime.Hour != 0)
                {
                    TextWriters.WriteWhere(new string(' ', ConsoleWrapper.WindowWidth - 10), 5, ProgressFillPositionHours, true, KernelColorType.NeutralText, KernelColorType.Background);
                    TextWriterWhereColor.WriteWhereColorBack(new string(' ', ConsoleMisc.PercentRepeat(TimeDateTools.KernelDateTime.Hour, 24, 10)), 5, ProgressFillPositionHours, true, Color.Empty, ColorStorageHours);
                }
                if (TimeDateTools.KernelDateTime.Minute != 0)
                {
                    TextWriters.WriteWhere(new string(' ', ConsoleWrapper.WindowWidth - 10), 5, ProgressFillPositionMinutes, true, KernelColorType.NeutralText, KernelColorType.Background);
                    TextWriterWhereColor.WriteWhereColorBack(new string(' ', ConsoleMisc.PercentRepeat(TimeDateTools.KernelDateTime.Minute, 60, 10)), 5, ProgressFillPositionMinutes, true, Color.Empty, ColorStorageMinutes);
                }
                if (TimeDateTools.KernelDateTime.Second != 0)
                {
                    TextWriters.WriteWhere(new string(' ', ConsoleWrapper.WindowWidth - 10), 5, ProgressFillPositionSeconds, true, KernelColorType.NeutralText, KernelColorType.Background);
                    TextWriterWhereColor.WriteWhereColorBack(new string(' ', ConsoleMisc.PercentRepeat(TimeDateTools.KernelDateTime.Second, 60, 10)), 5, ProgressFillPositionSeconds, true, Color.Empty, ColorStorageSeconds);
                }

                // Print information
                if (!string.IsNullOrEmpty(ScreensaverPackInit.SaversConfig.ProgressClockInfoTextHours))
                    TextWriterWhereColor.WriteWhereColor(PlaceParse.ProbePlaces(ScreensaverPackInit.SaversConfig.ProgressClockInfoTextHours), 4, InformationPositionHours, true, ColorStorageHours, TimeDateTools.KernelDateTime.Hour);
                else
                    TextWriterWhereColor.WriteWhereColor("H: {0}/24  ", 4, InformationPositionHours, true, ColorStorageHours, TimeDateTools.KernelDateTime.Hour);
                if (!string.IsNullOrEmpty(ScreensaverPackInit.SaversConfig.ProgressClockInfoTextMinutes))
                    TextWriterWhereColor.WriteWhereColor(PlaceParse.ProbePlaces(ScreensaverPackInit.SaversConfig.ProgressClockInfoTextMinutes), 4, InformationPositionMinutes, true, ColorStorageMinutes, TimeDateTools.KernelDateTime.Minute);
                else
                    TextWriterWhereColor.WriteWhereColor("M: {0}/60  ", 4, InformationPositionMinutes, true, ColorStorageMinutes, TimeDateTools.KernelDateTime.Minute);
                if (!string.IsNullOrEmpty(ScreensaverPackInit.SaversConfig.ProgressClockInfoTextHours))
                    TextWriterWhereColor.WriteWhereColor(PlaceParse.ProbePlaces(ScreensaverPackInit.SaversConfig.ProgressClockInfoTextSeconds), 4, InformationPositionSeconds, true, ColorStorageSeconds, TimeDateTools.KernelDateTime.Second);
                else
                    TextWriterWhereColor.WriteWhereColor("S: {0}/60  ", 4, InformationPositionSeconds, true, ColorStorageSeconds, TimeDateTools.KernelDateTime.Second);

                // Print date information
                TextWriterWhereColor.WriteWhereColor(new string(' ', lastDate.Length), (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - lastDate.Length / 2d), ConsoleWrapper.WindowHeight - 2, ColorStorage);
                string currentDate = TimeDateRenderers.Render();
                TextWriterWhereColor.WriteWhereColor(currentDate, (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - currentDate.Length / 2d), ConsoleWrapper.WindowHeight - 2, ColorStorage);
                lastDate = currentDate;
            }
            if (ScreensaverPackInit.SaversConfig.ProgressClockCycleColors)
                CurrentTicks += 1L;

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(ScreensaverPackInit.SaversConfig.ProgressClockDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
