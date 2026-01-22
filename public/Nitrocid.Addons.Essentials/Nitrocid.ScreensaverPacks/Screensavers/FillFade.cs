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
using System.Threading;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Nitrocid.Kernel.Configuration;
using Terminaux.Colors;
using Terminaux.Base;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Base.Extensions;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for FillFade
    /// </summary>
    public class FillFadeDisplay : BaseScreensaver, IScreensaver
    {

        private bool ColorFilled;
        private Color currentColor = Color.Empty;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "FillFade";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorFilled = false;
            ChangeColor();
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
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
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Dissolving: {0}", vars: [ColorFilled]);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "End left: {0} | End top: {1}", vars: [EndLeft, EndTop]);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got left: {0} | Got top: {1}", vars: [Left, Top]);

            // Fill the color if not filled
            if (!ColorFilled)
            {
                if (ConsoleResizeHandler.WasResized(false))
                {
                    // Refill, because the console is resized
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "We're refilling...");
                    ColorFilled = false;
                    goAhead = false;
                    KernelColorTools.LoadBackground();
                }

                if (goAhead)
                {
                    if (ConsoleWrapper.CursorLeft >= EndLeft && ConsoleWrapper.CursorTop >= EndTop)
                    {
                        ConsoleColoring.SetConsoleColorDry(currentColor, true);
                        TextWriterRaw.WritePlain(" ", false);
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "We're now dissolving... L: {0} = {1} | T: {2} = {3}", vars: [ConsoleWrapper.CursorLeft, EndLeft, ConsoleWrapper.CursorTop, EndTop]);
                        ColorFilled = true;
                    }
                    else
                    {
                        ConsoleColoring.SetConsoleColorDry(currentColor, true);
                        TextWriterRaw.WritePlain(" ", false);
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
                    if (ScreensaverManager.Bailing)
                        return;

                    // Set the thresholds
                    int RedColorNum = currentColor.RGB.R;
                    int GreenColorNum = currentColor.RGB.G;
                    int BlueColorNum = currentColor.RGB.B;
                    double ThresholdRed = RedColorNum / (double)maxSteps;
                    double ThresholdGreen = GreenColorNum / (double)maxSteps;
                    double ThresholdBlue = BlueColorNum / (double)maxSteps;
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", vars: [ThresholdRed, ThresholdGreen, ThresholdBlue]);

                    // Fade out
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", vars: [CurrentStep, maxSteps]);
                    ThreadManager.SleepNoBlock(50, Thread.CurrentThread);
                    int CurrentColorRedOut = (int)Math.Round(RedColorNum - ThresholdRed * CurrentStep);
                    int CurrentColorGreenOut = (int)Math.Round(GreenColorNum - ThresholdGreen * CurrentStep);
                    int CurrentColorBlueOut = (int)Math.Round(BlueColorNum - ThresholdBlue * CurrentStep);
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", vars: [CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut]);
                    if (!ConsoleResizeHandler.WasResized(false))
                        ConsoleColoring.LoadBackDry(new Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut));
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
            if (ScreensaverPackInit.SaversConfig.FillFadeTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.FillFadeMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.FillFadeMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.FillFadeMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.FillFadeMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.FillFadeMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.FillFadeMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                currentColor = new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}");
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.FillFadeMinimumColorLevel, ScreensaverPackInit.SaversConfig.FillFadeMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [ColorNum]);
                currentColor = new Color(ColorNum);
            }
        }

    }
}
