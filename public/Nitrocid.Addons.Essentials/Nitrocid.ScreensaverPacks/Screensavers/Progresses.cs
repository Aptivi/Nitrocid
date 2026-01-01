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
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Terminaux.Writer.ConsoleWriters;
using Textify.Tools.Placeholder;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Time.Renderers;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Nitrocid.Kernel.Configuration;
using Terminaux.Writer.CyclicWriters.Graphical;
using Nitrocid.Drivers.RNG;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Progresses
    /// </summary>
    public class ProgressesDisplay : BaseScreensaver, IScreensaver
    {

        private Color ColorStorageFirst = Color.Empty,
                      ColorStorageSecond = Color.Empty,
                      ColorStorageThird = Color.Empty,
                      ColorStorage = Color.Empty;
        private int firstPosition, secondPosition, thirdPosition;
        private double firstPositionThreshold, secondPositionThreshold, thirdPositionThreshold;
        private long CurrentTicks;
        private string lastProgresses = "";

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Progresses";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            CurrentTicks = ScreensaverPackInit.SaversConfig.ProgressesCycleColorsTicks;
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Prepare colors
            int ProgressFillPositionFirst, ProgressFillPositionSecond, ProgressFillPositionThird;
            int InformationPositionFirst, InformationPositionSecond, InformationPositionThird;

            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Current tick: {0}", vars: [CurrentTicks]);
            if (ScreensaverPackInit.SaversConfig.ProgressesCycleColors)
            {
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Cycling colors...");
                if (CurrentTicks >= ScreensaverPackInit.SaversConfig.ProgressesCycleColorsTicks)
                {
                    var type = ScreensaverPackInit.SaversConfig.ProgressesTrueColor ? ColorType.TrueColor : ColorType.EightBitColor;
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Current tick equals the maximum ticks to change color.");
                    ColorStorageFirst =
                        ColorTools.GetRandomColor(type, ScreensaverPackInit.SaversConfig.ProgressesMinimumColorLevelFirst, ScreensaverPackInit.SaversConfig.ProgressesMaximumColorLevelFirst,
                                                        ScreensaverPackInit.SaversConfig.ProgressesMinimumRedColorLevelFirst, ScreensaverPackInit.SaversConfig.ProgressesMaximumRedColorLevelFirst,
                                                        ScreensaverPackInit.SaversConfig.ProgressesMinimumGreenColorLevelFirst, ScreensaverPackInit.SaversConfig.ProgressesMaximumGreenColorLevelFirst,
                                                        ScreensaverPackInit.SaversConfig.ProgressesMinimumBlueColorLevelFirst, ScreensaverPackInit.SaversConfig.ProgressesMaximumBlueColorLevelFirst);
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (First) (R;G;B: {0};{1};{2})", vars: [ColorStorageFirst.RGB.R, ColorStorageFirst.RGB.G, ColorStorageFirst.RGB.B]);
                    ColorStorageSecond =
                        ColorTools.GetRandomColor(type, ScreensaverPackInit.SaversConfig.ProgressesMinimumColorLevelSecond, ScreensaverPackInit.SaversConfig.ProgressesMaximumColorLevelSecond,
                                                        ScreensaverPackInit.SaversConfig.ProgressesMinimumRedColorLevelSecond, ScreensaverPackInit.SaversConfig.ProgressesMaximumRedColorLevelSecond,
                                                        ScreensaverPackInit.SaversConfig.ProgressesMinimumGreenColorLevelSecond, ScreensaverPackInit.SaversConfig.ProgressesMaximumGreenColorLevelSecond,
                                                        ScreensaverPackInit.SaversConfig.ProgressesMinimumBlueColorLevelSecond, ScreensaverPackInit.SaversConfig.ProgressesMaximumBlueColorLevelSecond);
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (Second) (R;G;B: {0};{1};{2})", vars: [ColorStorageSecond.RGB.R, ColorStorageSecond.RGB.G, ColorStorageSecond.RGB.B]);
                    ColorStorageThird =
                        ColorTools.GetRandomColor(type, ScreensaverPackInit.SaversConfig.ProgressesMinimumColorLevelThird, ScreensaverPackInit.SaversConfig.ProgressesMaximumColorLevelThird,
                                                        ScreensaverPackInit.SaversConfig.ProgressesMinimumRedColorLevelThird, ScreensaverPackInit.SaversConfig.ProgressesMaximumRedColorLevelThird,
                                                        ScreensaverPackInit.SaversConfig.ProgressesMinimumGreenColorLevelThird, ScreensaverPackInit.SaversConfig.ProgressesMaximumGreenColorLevelThird,
                                                        ScreensaverPackInit.SaversConfig.ProgressesMinimumBlueColorLevelThird, ScreensaverPackInit.SaversConfig.ProgressesMaximumBlueColorLevelThird);
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (Third) (R;G;B: {0};{1};{2})", vars: [ColorStorageThird.RGB.R, ColorStorageThird.RGB.G, ColorStorageThird.RGB.B]);
                    ColorStorage =
                        ColorTools.GetRandomColor(type, ScreensaverPackInit.SaversConfig.ProgressesMinimumColorLevel, ScreensaverPackInit.SaversConfig.ProgressesMaximumColorLevel,
                                                        ScreensaverPackInit.SaversConfig.ProgressesMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.ProgressesMaximumRedColorLevel,
                                                        ScreensaverPackInit.SaversConfig.ProgressesMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.ProgressesMaximumGreenColorLevel,
                                                        ScreensaverPackInit.SaversConfig.ProgressesMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.ProgressesMaximumBlueColorLevel);
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [ColorStorage.RGB.R, ColorStorage.RGB.G, ColorStorage.RGB.B]);
                    CurrentTicks = 0L;
                }
            }
            else
            {
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Parsing colors...");
                ColorStorageFirst = new Color(ScreensaverPackInit.SaversConfig.ProgressesFirstProgressColor);
                ColorStorageSecond = new Color(ScreensaverPackInit.SaversConfig.ProgressesSecondProgressColor);
                ColorStorageThird = new Color(ScreensaverPackInit.SaversConfig.ProgressesThirdProgressColor);
                ColorStorage = new Color(ScreensaverPackInit.SaversConfig.ProgressesProgressColor);
            }
            ProgressFillPositionFirst = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 10;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Fill position for progress (First) {0}", vars: [ProgressFillPositionFirst]);
            ProgressFillPositionSecond = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 1;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Fill position for progress (Second) {0}", vars: [ProgressFillPositionSecond]);
            ProgressFillPositionThird = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) + 8;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Fill position for progress (Third) {0}", vars: [ProgressFillPositionThird]);
            InformationPositionFirst = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 12;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Fill position for info (First) {0}", vars: [InformationPositionFirst]);
            InformationPositionSecond = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 3;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Fill position for info (Second) {0}", vars: [InformationPositionSecond]);
            InformationPositionThird = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) + 6;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Fill position for info (Third) {0}", vars: [InformationPositionThird]);
            
            // Prepare the positions
            if (firstPosition == 0 || firstPosition >= 100)
            {
                firstPositionThreshold = RandomDriver.RandomDouble(5);
                firstPosition = 0;
            }
            if (secondPosition == 0 || secondPosition >= 100)
            {
                secondPositionThreshold = RandomDriver.RandomDouble(5);
                secondPosition = 0;
            }
            if (thirdPosition == 0 || thirdPosition >= 100)
            {
                thirdPositionThreshold = RandomDriver.RandomDouble(5);
                thirdPosition = 0;
            }
            firstPosition += (int)firstPositionThreshold;
            if (firstPosition > 100)
                firstPosition = 100;
            secondPosition += (int)secondPositionThreshold;
            if (secondPosition > 100)
                secondPosition = 100;
            thirdPosition += (int)thirdPositionThreshold;
            if (thirdPosition > 100)
                thirdPosition = 100;

            // Populate the border settings
            var firstBorder = new BorderSettings()
            {
                BorderUpperLeftCornerChar = ScreensaverPackInit.SaversConfig.ProgressesUpperLeftCornerCharFirst,
                BorderLowerLeftCornerChar = ScreensaverPackInit.SaversConfig.ProgressesLowerLeftCornerCharFirst,
                BorderUpperRightCornerChar = ScreensaverPackInit.SaversConfig.ProgressesUpperRightCornerCharFirst,
                BorderLowerRightCornerChar = ScreensaverPackInit.SaversConfig.ProgressesLowerRightCornerCharFirst,
                BorderUpperFrameChar = ScreensaverPackInit.SaversConfig.ProgressesUpperFrameCharFirst,
                BorderLowerFrameChar = ScreensaverPackInit.SaversConfig.ProgressesLowerFrameCharFirst,
                BorderLeftFrameChar = ScreensaverPackInit.SaversConfig.ProgressesLeftFrameCharFirst,
                BorderRightFrameChar = ScreensaverPackInit.SaversConfig.ProgressesRightFrameCharFirst,
            };
            var secondBorder = new BorderSettings()
            {
                BorderUpperLeftCornerChar = ScreensaverPackInit.SaversConfig.ProgressesUpperLeftCornerCharSecond,
                BorderLowerLeftCornerChar = ScreensaverPackInit.SaversConfig.ProgressesLowerLeftCornerCharSecond,
                BorderUpperRightCornerChar = ScreensaverPackInit.SaversConfig.ProgressesUpperRightCornerCharSecond,
                BorderLowerRightCornerChar = ScreensaverPackInit.SaversConfig.ProgressesLowerRightCornerCharSecond,
                BorderUpperFrameChar = ScreensaverPackInit.SaversConfig.ProgressesUpperFrameCharSecond,
                BorderLowerFrameChar = ScreensaverPackInit.SaversConfig.ProgressesLowerFrameCharSecond,
                BorderLeftFrameChar = ScreensaverPackInit.SaversConfig.ProgressesLeftFrameCharSecond,
                BorderRightFrameChar = ScreensaverPackInit.SaversConfig.ProgressesRightFrameCharSecond,
            };
            var thirdBorder = new BorderSettings()
            {
                BorderUpperLeftCornerChar = ScreensaverPackInit.SaversConfig.ProgressesUpperLeftCornerCharThird,
                BorderLowerLeftCornerChar = ScreensaverPackInit.SaversConfig.ProgressesLowerLeftCornerCharThird,
                BorderUpperRightCornerChar = ScreensaverPackInit.SaversConfig.ProgressesUpperRightCornerCharThird,
                BorderLowerRightCornerChar = ScreensaverPackInit.SaversConfig.ProgressesLowerRightCornerCharThird,
                BorderUpperFrameChar = ScreensaverPackInit.SaversConfig.ProgressesUpperFrameCharThird,
                BorderLowerFrameChar = ScreensaverPackInit.SaversConfig.ProgressesLowerFrameCharThird,
                BorderLeftFrameChar = ScreensaverPackInit.SaversConfig.ProgressesLeftFrameCharThird,
                BorderRightFrameChar = ScreensaverPackInit.SaversConfig.ProgressesRightFrameCharThird,
            };

            // Render the progress clock bars
            if (!ConsoleResizeHandler.WasResized(false))
            {
                // First
                var firstBoxFrame = new BoxFrame()
                {
                    Left = 4,
                    Top = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 11,
                    Width = ConsoleWrapper.WindowWidth - 10,
                    Height = 1,
                    Settings = firstBorder,
                    FrameColor = ColorStorageFirst,
                };
                TextWriterRaw.WriteRaw(firstBoxFrame.Render());

                // Second
                var secondBoxFrame = new BoxFrame()
                {
                    Left = 4,
                    Top = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 2,
                    Width = ConsoleWrapper.WindowWidth - 10,
                    Height = 1,
                    Settings = secondBorder,
                    FrameColor = ColorStorageSecond,
                };
                TextWriterRaw.WriteRaw(secondBoxFrame.Render());

                // Third
                var thirdBoxFrame = new BoxFrame()
                {
                    Left = 4,
                    Top = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) + 7,
                    Width = ConsoleWrapper.WindowWidth - 10,
                    Height = 1,
                    Settings = thirdBorder,
                    FrameColor = ColorStorageThird,
                };
                TextWriterRaw.WriteRaw(thirdBoxFrame.Render());

                // Fill progress for first, second, and third
                if (firstPosition != 0)
                {
                    TextWriters.WriteWhere(new string(' ', ConsoleWrapper.WindowWidth - 10), 5, ProgressFillPositionFirst, true, KernelColorType.NeutralText, KernelColorType.Background);
                    TextWriterWhereColor.WriteWhereColorBack(new string(' ', ConsoleMisc.PercentRepeat(firstPosition, 100, 10)), 5, ProgressFillPositionFirst, true, Color.Empty, ColorStorageFirst);
                }
                if (secondPosition != 0)
                {
                    TextWriters.WriteWhere(new string(' ', ConsoleWrapper.WindowWidth - 10), 5, ProgressFillPositionSecond, true, KernelColorType.NeutralText, KernelColorType.Background);
                    TextWriterWhereColor.WriteWhereColorBack(new string(' ', ConsoleMisc.PercentRepeat(secondPosition, 100, 10)), 5, ProgressFillPositionSecond, true, Color.Empty, ColorStorageSecond);
                }
                if (thirdPosition != 0)
                {
                    TextWriters.WriteWhere(new string(' ', ConsoleWrapper.WindowWidth - 10), 5, ProgressFillPositionThird, true, KernelColorType.NeutralText, KernelColorType.Background);
                    TextWriterWhereColor.WriteWhereColorBack(new string(' ', ConsoleMisc.PercentRepeat(thirdPosition, 100, 10)), 5, ProgressFillPositionThird, true, Color.Empty, ColorStorageThird);
                }

                // Print information
                if (!string.IsNullOrEmpty(ScreensaverPackInit.SaversConfig.ProgressesInfoTextFirst))
                    TextWriterWhereColor.WriteWhereColor(PlaceParse.ProbePlaces(ScreensaverPackInit.SaversConfig.ProgressesInfoTextFirst), 4, InformationPositionFirst, true, ColorStorageFirst, firstPosition);
                else
                    TextWriterWhereColor.WriteWhereColor("1: {0}%  ", 4, InformationPositionFirst, true, ColorStorageFirst, firstPosition);
                if (!string.IsNullOrEmpty(ScreensaverPackInit.SaversConfig.ProgressesInfoTextSecond))
                    TextWriterWhereColor.WriteWhereColor(PlaceParse.ProbePlaces(ScreensaverPackInit.SaversConfig.ProgressesInfoTextSecond), 4, InformationPositionSecond, true, ColorStorageSecond, secondPosition);
                else
                    TextWriterWhereColor.WriteWhereColor("2: {0}%  ", 4, InformationPositionSecond, true, ColorStorageSecond, secondPosition);
                if (!string.IsNullOrEmpty(ScreensaverPackInit.SaversConfig.ProgressesInfoTextFirst))
                    TextWriterWhereColor.WriteWhereColor(PlaceParse.ProbePlaces(ScreensaverPackInit.SaversConfig.ProgressesInfoTextThird), 4, InformationPositionThird, true, ColorStorageThird, thirdPosition);
                else
                    TextWriterWhereColor.WriteWhereColor("3: {0}%  ", 4, InformationPositionThird, true, ColorStorageThird, thirdPosition);

                // Print date information
                TextWriterWhereColor.WriteWhereColor(new string(' ', lastProgresses.Length), (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - lastProgresses.Length / 2d), ConsoleWrapper.WindowHeight - 2, ColorStorage);
                string currentDate = TimeDateRenderers.Render();
                TextWriterWhereColor.WriteWhereColor(currentDate, (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - currentDate.Length / 2d), ConsoleWrapper.WindowHeight - 2, ColorStorage);
                lastProgresses = currentDate;
            }
            if (ScreensaverPackInit.SaversConfig.ProgressesCycleColors)
                CurrentTicks += 1L;

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.ProgressesDelay);
        }

    }
}
