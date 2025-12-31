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
using Terminaux.Colors;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Writer.ConsoleWriters;
using Textify.Tools.Placeholder;
using Terminaux.Colors.Themes.Colors;
using Nitrocid.Base.Kernel.Time;
using Nitrocid.Base.Kernel.Time.Renderers;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Writer.CyclicWriters.Graphical;

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
        public override string ScreensaverName =>
            "ProgressClock";

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

            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Current tick: {0}", vars: [CurrentTicks]);
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
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (Hours) (R;G;B: {0};{1};{2})", vars: [ColorStorageHours.RGB.R, ColorStorageHours.RGB.G, ColorStorageHours.RGB.B]);
                    ColorStorageMinutes =
                        ColorTools.GetRandomColor(type, ScreensaverPackInit.SaversConfig.ProgressClockMinimumColorLevelMinutes, ScreensaverPackInit.SaversConfig.ProgressClockMaximumColorLevelMinutes,
                                                        ScreensaverPackInit.SaversConfig.ProgressClockMinimumRedColorLevelMinutes, ScreensaverPackInit.SaversConfig.ProgressClockMaximumRedColorLevelMinutes,
                                                        ScreensaverPackInit.SaversConfig.ProgressClockMinimumGreenColorLevelMinutes, ScreensaverPackInit.SaversConfig.ProgressClockMaximumGreenColorLevelMinutes,
                                                        ScreensaverPackInit.SaversConfig.ProgressClockMinimumBlueColorLevelMinutes, ScreensaverPackInit.SaversConfig.ProgressClockMaximumBlueColorLevelMinutes);
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (Minutes) (R;G;B: {0};{1};{2})", vars: [ColorStorageMinutes.RGB.R, ColorStorageMinutes.RGB.G, ColorStorageMinutes.RGB.B]);
                    ColorStorageSeconds =
                        ColorTools.GetRandomColor(type, ScreensaverPackInit.SaversConfig.ProgressClockMinimumColorLevelSeconds, ScreensaverPackInit.SaversConfig.ProgressClockMaximumColorLevelSeconds,
                                                        ScreensaverPackInit.SaversConfig.ProgressClockMinimumRedColorLevelSeconds, ScreensaverPackInit.SaversConfig.ProgressClockMaximumRedColorLevelSeconds,
                                                        ScreensaverPackInit.SaversConfig.ProgressClockMinimumGreenColorLevelSeconds, ScreensaverPackInit.SaversConfig.ProgressClockMaximumGreenColorLevelSeconds,
                                                        ScreensaverPackInit.SaversConfig.ProgressClockMinimumBlueColorLevelSeconds, ScreensaverPackInit.SaversConfig.ProgressClockMaximumBlueColorLevelSeconds);
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (Seconds) (R;G;B: {0};{1};{2})", vars: [ColorStorageSeconds.RGB.R, ColorStorageSeconds.RGB.G, ColorStorageSeconds.RGB.B]);
                    ColorStorage =
                        ColorTools.GetRandomColor(type, ScreensaverPackInit.SaversConfig.ProgressClockMinimumColorLevel, ScreensaverPackInit.SaversConfig.ProgressClockMaximumColorLevel,
                                                        ScreensaverPackInit.SaversConfig.ProgressClockMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.ProgressClockMaximumRedColorLevel,
                                                        ScreensaverPackInit.SaversConfig.ProgressClockMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.ProgressClockMaximumGreenColorLevel,
                                                        ScreensaverPackInit.SaversConfig.ProgressClockMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.ProgressClockMaximumBlueColorLevel);
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [ColorStorage.RGB.R, ColorStorage.RGB.G, ColorStorage.RGB.B]);
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
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Fill position for progress (Hours) {0}", vars: [ProgressFillPositionHours]);
            ProgressFillPositionMinutes = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 1;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Fill position for progress (Minutes) {0}", vars: [ProgressFillPositionMinutes]);
            ProgressFillPositionSeconds = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) + 8;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Fill position for progress (Seconds) {0}", vars: [ProgressFillPositionSeconds]);
            InformationPositionHours = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 12;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Fill position for info (Hours) {0}", vars: [InformationPositionHours]);
            InformationPositionMinutes = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 3;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Fill position for info (Minutes) {0}", vars: [InformationPositionMinutes]);
            InformationPositionSeconds = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) + 6;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Fill position for info (Seconds) {0}", vars: [InformationPositionSeconds]);

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
                var hoursBoxFrame = new BoxFrame()
                {
                    Left = 4,
                    Top = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 11,
                    Width = ConsoleWrapper.WindowWidth - 10,
                    Height = 1,
                    Settings = hoursBorder,
                    FrameColor = ColorStorageHours,
                };
                TextWriterRaw.WriteRaw(hoursBoxFrame.Render());

                // Minutes
                var minutesBoxFrame = new BoxFrame()
                {
                    Left = 4,
                    Top = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 2,
                    Width = ConsoleWrapper.WindowWidth - 10,
                    Height = 1,
                    Settings = minutesBorder,
                    FrameColor = ColorStorageMinutes,
                };
                TextWriterRaw.WriteRaw(minutesBoxFrame.Render());

                // Seconds
                var secondsBoxFrame = new BoxFrame()
                {
                    Left = 4,
                    Top = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) + 7,
                    Width = ConsoleWrapper.WindowWidth - 10,
                    Height = 1,
                    Settings = secondsBorder,
                    FrameColor = ColorStorageSeconds,
                };
                TextWriterRaw.WriteRaw(secondsBoxFrame.Render());

                // Fill progress for hours, minutes, and seconds
                if (TimeDateTools.KernelDateTime.Hour != 0)
                {
                    TextWriterWhereColor.WriteWhere(new string(' ', ConsoleWrapper.WindowWidth - 10), 5, ProgressFillPositionHours, true, ThemeColorType.NeutralText, ThemeColorType.Background);
                    TextWriterWhereColor.WriteWhereColorBack(new string(' ', ConsoleMisc.PercentRepeat(TimeDateTools.KernelDateTime.Hour, 24, 10)), 5, ProgressFillPositionHours, true, Color.Empty, ColorStorageHours);
                }
                if (TimeDateTools.KernelDateTime.Minute != 0)
                {
                    TextWriterWhereColor.WriteWhere(new string(' ', ConsoleWrapper.WindowWidth - 10), 5, ProgressFillPositionMinutes, true, ThemeColorType.NeutralText, ThemeColorType.Background);
                    TextWriterWhereColor.WriteWhereColorBack(new string(' ', ConsoleMisc.PercentRepeat(TimeDateTools.KernelDateTime.Minute, 60, 10)), 5, ProgressFillPositionMinutes, true, Color.Empty, ColorStorageMinutes);
                }
                if (TimeDateTools.KernelDateTime.Second != 0)
                {
                    TextWriterWhereColor.WriteWhere(new string(' ', ConsoleWrapper.WindowWidth - 10), 5, ProgressFillPositionSeconds, true, ThemeColorType.NeutralText, ThemeColorType.Background);
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
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.ProgressClockDelay);
        }

    }
}
