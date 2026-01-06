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

using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.ScreensaverPacks.Screensavers.Utilities;
using Terminaux.Base;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Colorology
    /// </summary>
    public class ColorologyDisplay : BaseScreensaver, IScreensaver
    {
        // TODO: NKS_SCREENSAVERPACKS_COLOROLOGY_SETTINGS_DESC -> Simulates slow color bending and transitioning
        private Color? nextColor;
        private Color? currentColor;
        private int shadesHue = 0;
        private int pingpongHue = 0;

        private static int MaxLevel =>
            ScreensaverPackInit.SaversConfig.ColorologyDarkColors ? 32 : 255;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Colorology";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Set colors, if persistence is supported
            switch (ScreensaverPackInit.SaversConfig.ColorologyMethod)
            {
                case ColorTransitionMethod.HslFull:
                    {
                        (int nextHueAngle, int currentHueAngle) = (RandomDriver.Random(360), RandomDriver.Random(360));
                        (int nextSatAngle, int currentSatAngle) = (RandomDriver.Random(100), RandomDriver.Random(100));
                        (int nextLigAngle, int currentLigAngle) = (RandomDriver.Random(100), RandomDriver.Random(100));
                        nextColor = new Color($"hsl:{nextHueAngle};{nextSatAngle};{nextLigAngle}");
                        currentColor = new Color($"hsl:{currentHueAngle};{currentSatAngle};{currentLigAngle}");
                    }
                    break;
                case ColorTransitionMethod.Rgb:
                case ColorTransitionMethod.Intermediate:
                    {
                        nextColor = ColorTools.GetRandomColor(ColorType.TrueColor, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel);
                        currentColor = ColorTools.GetRandomColor(ColorType.TrueColor, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel);
                    }
                    break;
                case ColorTransitionMethod.Grays:
                    {
                        (int nextMonoColor, int currentMonoColor) = (RandomDriver.Random(255), RandomDriver.Random(255));
                        nextColor = new Color($"{nextMonoColor};{nextMonoColor};{nextMonoColor}");
                        currentColor = new Color($"{currentMonoColor};{currentMonoColor};{currentMonoColor}");
                    }
                    break;
                case ColorTransitionMethod.Shades:
                    {
                        shadesHue = RandomDriver.Random(360);
                        (int nextSatAngle, int currentSatAngle) = (RandomDriver.Random(100), RandomDriver.Random(100));
                        (int nextLigAngle, int currentLigAngle) = (RandomDriver.Random(100), RandomDriver.Random(100));
                        nextColor = new Color($"hsl:{shadesHue};{nextSatAngle};{nextLigAngle}");
                        currentColor = new Color($"hsl:{shadesHue};{currentSatAngle};{currentLigAngle}");
                    }
                    break;
                case ColorTransitionMethod.PingPong:
                    pingpongHue = RandomDriver.Random(360);
                    break;
            }
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Prepare the colors
            switch (ScreensaverPackInit.SaversConfig.ColorologyMethod)
            {
                case ColorTransitionMethod.Hsl:
                    {
                        int angleThreshold = 60;
                        (int nextHueAngle, int currentHueAngle) = (angleThreshold, 0);

                        // Add the angle threshold 6 times to spin through the entire hue. There is no persistence here.
                        for (int i = 0; i < 6; i++)
                        {
                            var hslTransitionNext = new Color($"hsl:{nextHueAngle};100;50");
                            var hslTransitionCurrent = new Color($"hsl:{currentHueAngle};100;50");
                            Transition(hslTransitionCurrent, hslTransitionNext);
                            nextHueAngle += angleThreshold;
                            currentHueAngle += angleThreshold;
                        }
                    }
                    break;
                case ColorTransitionMethod.HslFull:
                case ColorTransitionMethod.Rgb:
                case ColorTransitionMethod.Grays:
                case ColorTransitionMethod.Shades:
                    if (currentColor is not null && nextColor is not null)
                        Transition(currentColor, nextColor);
                    break;
                case ColorTransitionMethod.Intermediate:
                    if (currentColor is not null && nextColor is not null)
                    {
                        Color intermediateColor = ColorTools.GetRandomColor(ColorType.TrueColor, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel);
                        Transition(currentColor, intermediateColor);
                        Transition(intermediateColor, nextColor);
                    }
                    break;
                case ColorTransitionMethod.PingPong:
                    {
                        int angleThreshold = -180;
                        (int nextHueAngle, int currentHueAngle) = (pingpongHue + angleThreshold, pingpongHue);
                        if (nextHueAngle < 0)
                            nextHueAngle = 360 + nextHueAngle;

                        // Add the angle threshold 6 times to spin through the entire hue. There is no persistence here.
                        var hslTransitionNext = new Color($"hsl:{nextHueAngle};100;50");
                        var hslTransitionCurrent = new Color($"hsl:{currentHueAngle};100;50");
                        Transition(hslTransitionCurrent, hslTransitionNext);
                        Transition(hslTransitionNext, hslTransitionCurrent);
                    }
                    break;
            }

            // Helper function to help with the transition
            static void Transition(Color chosenCurrentColor, Color chosenNextColor)
            {
                int steps = ScreensaverPackInit.SaversConfig.ColorologySteps;
                double thresholdR = (chosenCurrentColor.RGB.R - chosenNextColor.RGB.R) / (double)steps;
                double thresholdG = (chosenCurrentColor.RGB.G - chosenNextColor.RGB.G) / (double)steps;
                double thresholdB = (chosenCurrentColor.RGB.B - chosenNextColor.RGB.B) / (double)steps;

                // Now, transition from black to the target color
                double currentR = chosenCurrentColor.RGB.R;
                double currentG = chosenCurrentColor.RGB.G;
                double currentB = chosenCurrentColor.RGB.B;
                for (int currentStep = 1; currentStep <= steps; currentStep++)
                {
                    if (ConsoleResizeHandler.WasResized(false))
                        break;
                    if (ScreensaverManager.Bailing)
                        return;

                    // Add the values according to the threshold
                    currentR -= thresholdR;
                    currentG -= thresholdG;
                    currentB -= thresholdB;

                    // Now, make a color and fill the console with it
                    Color col = new((int)currentR, (int)currentG, (int)currentB);
                    ColorTools.LoadBackDry(col);

                    // Sleep
                    ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.ColorologyDelay);
                }
            }

            // Reset colors, if persistence is supported
            switch (ScreensaverPackInit.SaversConfig.ColorologyMethod)
            {
                case ColorTransitionMethod.HslFull:
                    {
                        int hueAngle = RandomDriver.Random(360);
                        int satAngle = RandomDriver.Random(100);
                        int ligAngle = RandomDriver.Random(100);
                        currentColor = nextColor;
                        nextColor = new Color($"hsl:{hueAngle};{satAngle};{ligAngle}");
                    }
                    break;
                case ColorTransitionMethod.Rgb:
                case ColorTransitionMethod.Intermediate:
                    {
                        currentColor = nextColor;
                        nextColor = ColorTools.GetRandomColor(ColorType.TrueColor, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel);
                    }
                    break;
                case ColorTransitionMethod.Grays:
                    {
                        int nextMonoColor = RandomDriver.Random(255);
                        currentColor = nextColor;
                        nextColor = new Color($"{nextMonoColor};{nextMonoColor};{nextMonoColor}");
                    }
                    break;
                case ColorTransitionMethod.Shades:
                    {
                        int nextSatAngle = RandomDriver.Random(100);
                        int nextLigAngle = RandomDriver.Random(100);
                        currentColor = nextColor;
                        nextColor = new Color($"hsl:{shadesHue};{nextSatAngle};{nextLigAngle}");
                    }
                    break;
            }

            // Reset resize sync
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.ColorologyDelay);
            ConsoleResizeHandler.WasResized();
        }

    }
}
