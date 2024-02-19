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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using System.Text;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for GradientBloom
    /// </summary>
    public static class GradientBloomSettings
    {

        /// <summary>
        /// [GradientBloom] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int GradientBloomDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientBloomDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                ScreensaverPackInit.SaversConfig.GradientBloomDelay = value;
            }
        }
        /// <summary>
        /// [GradientBloom] Whether to use dark colors or not
        /// </summary>
        public static bool GradientBloomDarkColors
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientBloomDarkColors;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.GradientBloomDarkColors = value;
            }
        }
        /// <summary>
        /// [GradientBloom] How many color steps for transitioning between two colors?
        /// </summary>
        public static int GradientBloomSteps
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientBloomSteps;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                ScreensaverPackInit.SaversConfig.GradientBloomSteps = value;
            }
        }

    }

    /// <summary>
    /// Display code for GradientBloom
    /// </summary>
    public class GradientBloomDisplay : BaseScreensaver, IScreensaver
    {

        private Color nextColorLeft;
        private Color currentColorLeft;
        private Color nextColorRight;
        private Color currentColorRight;

        private static int MaxLevel =>
            GradientBloomSettings.GradientBloomDarkColors ? 32 : 255;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "GradientBloom";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            nextColorLeft = ColorTools.GetRandomColor(ColorType.TrueColor, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel);
            currentColorLeft = ColorTools.GetRandomColor(ColorType.TrueColor, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel);
            nextColorRight = ColorTools.GetRandomColor(ColorType.TrueColor, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel);
            currentColorRight = ColorTools.GetRandomColor(ColorType.TrueColor, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel);
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Prepare the colors
            int steps = GradientBloomSettings.GradientBloomSteps;
            double thresholdLeftR = (currentColorLeft.RGB.R - nextColorLeft.RGB.R) / (double)steps;
            double thresholdLeftG = (currentColorLeft.RGB.G - nextColorLeft.RGB.G) / (double)steps;
            double thresholdLeftB = (currentColorLeft.RGB.B - nextColorLeft.RGB.B) / (double)steps;
            double thresholdRightR = (currentColorRight.RGB.R - nextColorRight.RGB.R) / (double)steps;
            double thresholdRightG = (currentColorRight.RGB.G - nextColorRight.RGB.G) / (double)steps;
            double thresholdRightB = (currentColorRight.RGB.B - nextColorRight.RGB.B) / (double)steps;

            // Now, transition from black to the target color
            double currentLeftR = currentColorLeft.RGB.R;
            double currentLeftG = currentColorLeft.RGB.G;
            double currentLeftB = currentColorLeft.RGB.B;
            double currentRightR = currentColorRight.RGB.R;
            double currentRightG = currentColorRight.RGB.G;
            double currentRightB = currentColorRight.RGB.B;
            for (int currentStep = 1; currentStep <= steps; currentStep++)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;

                // Add the values according to the threshold
                currentLeftR -= thresholdLeftR;
                currentLeftG -= thresholdLeftG;
                currentLeftB -= thresholdLeftB;
                currentRightR -= thresholdRightR;
                currentRightG -= thresholdRightG;
                currentRightB -= thresholdRightB;

                // Now, make a color
                Color colLeft = new((int)currentLeftR, (int)currentLeftG, (int)currentLeftB);
                Color colRight = new((int)currentRightR, (int)currentRightG, (int)currentRightB);
                double currentR = colLeft.RGB.R;
                double currentG = colLeft.RGB.G;
                double currentB = colLeft.RGB.B;

                // Populate final thresholds
                double thresholdR = (colLeft.RGB.R - colRight.RGB.R) / (double)ConsoleWrapper.WindowWidth;
                double thresholdG = (colLeft.RGB.G - colRight.RGB.G) / (double)ConsoleWrapper.WindowWidth;
                double thresholdB = (colLeft.RGB.B - colRight.RGB.B) / (double)ConsoleWrapper.WindowWidth;

                // Now, make a final color and make a gradient.
                StringBuilder buffered = new();
                for (int currentStepGradient = 1; currentStepGradient <= ConsoleWrapper.WindowWidth; currentStepGradient++)
                {
                    if (ConsoleResizeHandler.WasResized(false))
                        break;

                    // Add the values according to the threshold
                    currentR -= thresholdR;
                    currentG -= thresholdG;
                    currentB -= thresholdB;

                    // Now, make a color
                    Color col = new((int)currentR, (int)currentG, (int)currentB);
                    buffered.Append(col.VTSequenceBackground);
                    for (int height = 0; height < ConsoleWrapper.WindowHeight; height++)
                        buffered.Append($"{CsiSequences.GenerateCsiCursorPosition(currentStepGradient, height + 1)} ");
                }

                // Render
                TextWriterColor.Write(buffered.ToString(), false);

                // Sleep
                ThreadManager.SleepNoBlock(GradientBloomSettings.GradientBloomDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Generate new colors
            currentColorLeft = nextColorLeft;
            nextColorLeft = ColorTools.GetRandomColor(ColorType.TrueColor, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel);
            currentColorRight = nextColorRight;
            nextColorRight = ColorTools.GetRandomColor(ColorType.TrueColor, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel);
            ThreadManager.SleepNoBlock(GradientBloomSettings.GradientBloomDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
        }

    }
}
