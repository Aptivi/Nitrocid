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
using System.Threading;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for FillFade
    /// </summary>
    public static class FillFadeSettings
    {

        /// <summary>
        /// [FillFade] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool FillFadeTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FillFadeTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.FillFadeTrueColor = value;
            }
        }
        /// <summary>
        /// [FillFade] The minimum red color level (true color)
        /// </summary>
        public static int FillFadeMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FillFadeMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FillFadeMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The minimum green color level (true color)
        /// </summary>
        public static int FillFadeMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FillFadeMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FillFadeMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The minimum blue color level (true color)
        /// </summary>
        public static int FillFadeMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FillFadeMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FillFadeMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int FillFadeMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FillFadeMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.FillFadeMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The maximum red color level (true color)
        /// </summary>
        public static int FillFadeMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FillFadeMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.FillFadeMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FillFadeMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FillFadeMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The maximum green color level (true color)
        /// </summary>
        public static int FillFadeMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FillFadeMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.FillFadeMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FillFadeMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FillFadeMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The maximum blue color level (true color)
        /// </summary>
        public static int FillFadeMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FillFadeMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.FillFadeMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FillFadeMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FillFadeMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int FillFadeMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FillFadeMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.FillFadeMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FillFadeMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.FillFadeMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for FillFade
    /// </summary>
    public class FillFadeDisplay : BaseScreensaver, IScreensaver
    {

        private bool ColorFilled;
        private Color currentColor = Color.Empty;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "FillFade";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorFilled = false;
            ChangeColor();
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            if (ColorFilled)
                Thread.Sleep(1);
            int EndLeft = ConsoleWrapper.WindowWidth - 1;
            int EndTop = ConsoleWrapper.WindowHeight - 1;
            int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            bool goAhead = true;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Dissolving: {0}", ColorFilled);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "End left: {0} | End top: {1}", EndLeft, EndTop);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got left: {0} | Got top: {1}", Left, Top);

            // Fill the color if not filled
            if (!ColorFilled)
            {
                if (ConsoleResizeHandler.WasResized(false))
                {
                    // Refill, because the console is resized
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "We're refilling...");
                    ColorFilled = false;
                    goAhead = false;
                    ColorTools.LoadBack();
                }

                if (goAhead)
                {
                    if (ConsoleWrapper.CursorLeft >= EndLeft && ConsoleWrapper.CursorTop >= EndTop)
                    {
                        ColorTools.SetConsoleColorDry(currentColor, true);
                        TextWriterColor.WritePlain(" ", false);
                        DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "We're now dissolving... L: {0} = {1} | T: {2} = {3}", ConsoleWrapper.CursorLeft, EndLeft, ConsoleWrapper.CursorTop, EndTop);
                        ColorFilled = true;
                    }
                    else
                    {
                        ColorTools.SetConsoleColorDry(currentColor, true);
                        TextWriterColor.WritePlain(" ", false);
                    }
                }
            }
            else
            {
                // Now, fade out
                int maxSteps = 30;
                for (int CurrentStep = 1; CurrentStep <= maxSteps; CurrentStep++)
                {
                    if (ConsoleResizeHandler.WasResized(false))
                        break;

                    // Set the thresholds
                    int RedColorNum = currentColor.R;
                    int GreenColorNum = currentColor.G;
                    int BlueColorNum = currentColor.B;
                    double ThresholdRed = RedColorNum / (double)maxSteps;
                    double ThresholdGreen = GreenColorNum / (double)maxSteps;
                    double ThresholdBlue = BlueColorNum / (double)maxSteps;
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue);

                    // Fade out
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, maxSteps);
                    ThreadManager.SleepNoBlock(50, Thread.CurrentThread);
                    int CurrentColorRedOut = (int)Math.Round(RedColorNum - ThresholdRed * CurrentStep);
                    int CurrentColorGreenOut = (int)Math.Round(GreenColorNum - ThresholdGreen * CurrentStep);
                    int CurrentColorBlueOut = (int)Math.Round(BlueColorNum - ThresholdBlue * CurrentStep);
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);
                    if (!ConsoleResizeHandler.WasResized(false))
                        ColorTools.LoadBackDry(new Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut));
                }
                ChangeColor();
                ColorFilled = false;
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
        }

        private void ChangeColor()
        {
            currentColor = Color.Empty;
            if (FillFadeSettings.FillFadeTrueColor)
            {
                int RedColorNum = RandomDriver.Random(FillFadeSettings.FillFadeMinimumRedColorLevel, FillFadeSettings.FillFadeMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(FillFadeSettings.FillFadeMinimumGreenColorLevel, FillFadeSettings.FillFadeMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(FillFadeSettings.FillFadeMinimumBlueColorLevel, FillFadeSettings.FillFadeMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                currentColor = new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}");
            }
            else
            {
                int ColorNum = RandomDriver.Random(FillFadeSettings.FillFadeMinimumColorLevel, FillFadeSettings.FillFadeMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                currentColor = new Color(ColorNum);
            }
        }

    }
}
