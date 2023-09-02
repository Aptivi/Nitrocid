
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
using KS.Kernel.Configuration;
using KS.Kernel.Threading;
using Terminaux.Colors;

namespace KS.Misc.Screensaver.Displays
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
                return Config.SaverConfig.BloomDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                Config.SaverConfig.BloomDelay = value;
            }
        }

    }

    /// <summary>
    /// Display code for Bloom
    /// </summary>
    public class BloomDisplay : BaseScreensaver, IScreensaver
    {

        private Color nextColor = KernelColorTools.GetRandomColor(ColorType.TrueColor);
        private Color currentColor = KernelColorTools.GetRandomColor(ColorType.TrueColor);
        private readonly int steps = 100;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Bloom";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Prepare the colors
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
                ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Generate new colors
            currentColor = nextColor;
            nextColor = KernelColorTools.GetRandomColor(ColorType.TrueColor);
            ThreadManager.SleepNoBlock(BloomSettings.BloomDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);

            // Reset resize sync
            ConsoleResizeListener.WasResized();
        }

    }
}
