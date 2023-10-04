
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using System.Text;
using Terminaux.Colors;
using Terminaux.Sequences.Builder;

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
        private readonly int steps = 100;

        private static int MaxLevel =>
            GradientBloomSettings.GradientBloomDarkColors ? 32 : 255;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "GradientBloom";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            nextColorLeft = KernelColorTools.GetRandomColor(ColorType.TrueColor, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel);
            currentColorLeft = KernelColorTools.GetRandomColor(ColorType.TrueColor, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel);
            nextColorRight = KernelColorTools.GetRandomColor(ColorType.TrueColor, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel);
            currentColorRight = KernelColorTools.GetRandomColor(ColorType.TrueColor, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel);
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Prepare the colors
            double thresholdLeftR = (currentColorLeft.R - nextColorLeft.R) / (double)steps;
            double thresholdLeftG = (currentColorLeft.G - nextColorLeft.G) / (double)steps;
            double thresholdLeftB = (currentColorLeft.B - nextColorLeft.B) / (double)steps;
            double thresholdRightR = (currentColorRight.R - nextColorRight.R) / (double)steps;
            double thresholdRightG = (currentColorRight.G - nextColorRight.G) / (double)steps;
            double thresholdRightB = (currentColorRight.B - nextColorRight.B) / (double)steps;

            // Now, transition from black to the target color
            double currentLeftR = currentColorLeft.R;
            double currentLeftG = currentColorLeft.G;
            double currentLeftB = currentColorLeft.B;
            double currentRightR = currentColorRight.R;
            double currentRightG = currentColorRight.G;
            double currentRightB = currentColorRight.B;
            for (int currentStep = 1; currentStep <= steps; currentStep++)
            {
                if (ConsoleResizeListener.WasResized(false))
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
                double currentR = colLeft.R;
                double currentG = colLeft.G;
                double currentB = colLeft.B;

                // Populate final thresholds
                double thresholdR = (colLeft.R - colRight.R) / (double)ConsoleWrapper.WindowWidth;
                double thresholdG = (colLeft.G - colRight.G) / (double)ConsoleWrapper.WindowWidth;
                double thresholdB = (colLeft.B - colRight.B) / (double)ConsoleWrapper.WindowWidth;

                // Now, make a final color and make a gradient.
                StringBuilder buffered = new();
                for (int currentStepGradient = 1; currentStepGradient <= ConsoleWrapper.WindowWidth; currentStepGradient++)
                {
                    if (ConsoleResizeListener.WasResized(false))
                        break;

                    // Add the values according to the threshold
                    currentR -= thresholdR;
                    currentG -= thresholdG;
                    currentB -= thresholdB;

                    // Now, make a color
                    Color col = new((int)currentR, (int)currentG, (int)currentB);
                    buffered.Append(col.VTSequenceBackground);
                    for (int height = 0; height < ConsoleWrapper.WindowHeight; height++)
                        buffered.Append($"{VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCursorPosition, currentStepGradient, height + 1)} ");
                }

                // Render
                TextWriterColor.Write(buffered.ToString(), false);

                // Sleep
                ThreadManager.SleepNoBlock(GradientBloomSettings.GradientBloomDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Generate new colors
            currentColorLeft = nextColorLeft;
            nextColorLeft = KernelColorTools.GetRandomColor(ColorType.TrueColor, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel);
            currentColorRight = nextColorRight;
            nextColorRight = KernelColorTools.GetRandomColor(ColorType.TrueColor, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel);
            ThreadManager.SleepNoBlock(GradientBloomSettings.GradientBloomDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);

            // Reset resize sync
            ConsoleResizeListener.WasResized();
        }

    }
}
