//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for Lightspeed
    /// </summary>
    public static class LightspeedSettings
    {

        /// <summary>
        /// [Lightspeed] Enable color cycling
        /// </summary>
        public static bool LightspeedCycleColors
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LightspeedCycleColors;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.LightspeedCycleColors = value;
            }
        }
        /// <summary>
        /// [Lightspeed] The minimum red color level (true color)
        /// </summary>
        public static int LightspeedMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LightspeedMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.LightspeedMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Lightspeed] The minimum green color level (true color)
        /// </summary>
        public static int LightspeedMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LightspeedMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.LightspeedMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Lightspeed] The minimum blue color level (true color)
        /// </summary>
        public static int LightspeedMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LightspeedMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.LightspeedMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Lightspeed] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int LightspeedMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LightspeedMinimumColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.LightspeedMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Lightspeed] The maximum red color level (true color)
        /// </summary>
        public static int LightspeedMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LightspeedMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.LightspeedMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.LightspeedMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.LightspeedMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Lightspeed] The maximum green color level (true color)
        /// </summary>
        public static int LightspeedMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LightspeedMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.LightspeedMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.LightspeedMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.LightspeedMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Lightspeed] The maximum blue color level (true color)
        /// </summary>
        public static int LightspeedMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LightspeedMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.LightspeedMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.LightspeedMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.LightspeedMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Lightspeed] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int LightspeedMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LightspeedMaximumColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.LightspeedMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.LightspeedMinimumColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.LightspeedMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for Lightspeed
    /// </summary>
    public class LightspeedDisplay : BaseScreensaver, IScreensaver
    {

        private int CurrentColorR, CurrentColorG, CurrentColorB;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Lightspeed";

        /// <inheritdoc/>
        public override bool ScreensaverContainsFlashingImages { get; set; } = true;

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            int MaximumColors = LightspeedSettings.LightspeedMaximumColorLevel;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", MaximumColors);
            int MaximumColorsR = LightspeedSettings.LightspeedMaximumRedColorLevel;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", MaximumColorsR);
            int MaximumColorsG = LightspeedSettings.LightspeedMaximumGreenColorLevel;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", MaximumColorsG);
            int MaximumColorsB = LightspeedSettings.LightspeedMaximumBlueColorLevel;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", MaximumColorsB);

            ConsoleWrapper.CursorVisible = false;

            // Select the background color
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Cycling colors: {0}", LightspeedSettings.LightspeedCycleColors);
            if (!LightspeedSettings.LightspeedCycleColors)
            {
                int RedColorNum = RandomDriver.Random(255);
                int GreenColorNum = RandomDriver.Random(255);
                int BlueColorNum = RandomDriver.Random(255);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                KernelColorTools.SetConsoleColor(ColorStorage, true);
            }
            else
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", CurrentColorR, CurrentColorG, CurrentColorB);
                var ColorStorage = new Color(CurrentColorR, CurrentColorG, CurrentColorB);
                KernelColorTools.SetConsoleColor(ColorStorage, true);
            }

            // Make the disco effect!
            ConsoleWrapper.Clear();

            // Switch to the next color
            if (LightspeedSettings.LightspeedCycleColors)
            {
                if (CurrentColorR >= MaximumColorsR)
                {
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Red level exceeded maximum color. {0} >= {1}", CurrentColorR, MaximumColorsR);
                    CurrentColorR = 0;
                }
                else
                {
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Stepping one (R)...");
                    CurrentColorR += 1;
                }
                if (CurrentColorG >= MaximumColorsG)
                {
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Green level exceeded maximum color. {0} >= {1}", CurrentColorG, MaximumColorsG);
                    CurrentColorG = 0;
                }
                else if (CurrentColorR == 0)
                {
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Stepping one (G)...");
                    CurrentColorG += 1;
                }
                if (CurrentColorB >= MaximumColorsB)
                {
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Blue level exceeded maximum color. {0} >= {1}", CurrentColorB, MaximumColorsB);
                    CurrentColorB = 0;
                }
                else if (CurrentColorG == 0 & CurrentColorR == 0)
                {
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Stepping one (B)...");
                    CurrentColorB += 1;
                }
                if (CurrentColorB == 0 & CurrentColorG == 0 & CurrentColorR == 0)
                {
                    CurrentColorB = 0;
                    CurrentColorG = 0;
                    CurrentColorR = 0;
                }
            }

            // Check to see if we're dealing with beats per minute
            ThreadManager.SleepNoBlock(1, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
