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

using Nitrocid.ConsoleBase;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for Bloom
    /// </summary>
    public static class BloomSettings
    {

        /// <summary>
        /// [Bloom] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int BloomDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BloomDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                ScreensaverPackInit.SaversConfig.BloomDelay = value;
            }
        }
        /// <summary>
        /// [Bloom] Whether to use dark colors or not
        /// </summary>
        public static bool BloomDarkColors
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BloomDarkColors;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.BloomDarkColors = value;
            }
        }
        /// <summary>
        /// [Bloom] How many color steps for transitioning between two colors?
        /// </summary>
        public static int BloomSteps
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BloomSteps;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                ScreensaverPackInit.SaversConfig.BloomSteps = value;
            }
        }

    }

    /// <summary>
    /// Display code for Bloom
    /// </summary>
    public class BloomDisplay : BaseScreensaver, IScreensaver
    {

        private Color nextColor;
        private Color currentColor;

        private static int MaxLevel =>
            BloomSettings.BloomDarkColors ? 32 : 255;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Bloom";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            nextColor = KernelColorTools.GetRandomColor(ColorType.TrueColor, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel);
            currentColor = KernelColorTools.GetRandomColor(ColorType.TrueColor, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel);
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Prepare the colors
            int steps = BloomSettings.BloomSteps;
            double thresholdR = (currentColor.R - nextColor.R) / (double)steps;
            double thresholdG = (currentColor.G - nextColor.G) / (double)steps;
            double thresholdB = (currentColor.B - nextColor.B) / (double)steps;

            // Now, transition from black to the target color
            double currentR = currentColor.R;
            double currentG = currentColor.G;
            double currentB = currentColor.B;
            for (int currentStep = 1; currentStep <= steps; currentStep++)
            {
                if (ConsoleResizeListener.WasResized(false))
                    break;

                // Add the values according to the threshold
                currentR -= thresholdR;
                currentG -= thresholdG;
                currentB -= thresholdB;

                // Now, make a color and fill the console with it
                Color col = new((int)currentR, (int)currentG, (int)currentB);
                KernelColorTools.LoadBack(col);

                // Sleep
                ThreadManager.SleepNoBlock(BloomSettings.BloomDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Generate new colors
            currentColor = nextColor;
            nextColor = KernelColorTools.GetRandomColor(ColorType.TrueColor, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel);
            ThreadManager.SleepNoBlock(BloomSettings.BloomDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);

            // Reset resize sync
            ConsoleResizeListener.WasResized();
        }

    }
}
