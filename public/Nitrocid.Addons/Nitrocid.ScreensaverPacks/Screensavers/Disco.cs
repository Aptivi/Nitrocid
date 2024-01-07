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
using Nitrocid.ConsoleBase;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for Disco
    /// </summary>
    public static class DiscoSettings
    {

        /// <summary>
        /// [Disco] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool DiscoTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DiscoTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.DiscoTrueColor = value;
            }
        }
        /// <summary>
        /// [Disco] Enable color cycling
        /// </summary>
        public static bool DiscoCycleColors
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DiscoCycleColors;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.DiscoCycleColors = value;
            }
        }
        /// <summary>
        /// [Disco] How many milliseconds, or beats per minute, to wait before making the next write?
        /// </summary>
        public static int DiscoDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DiscoDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                ScreensaverPackInit.SaversConfig.DiscoDelay = value;
            }
        }
        /// <summary>
        /// [Disco] Whether to use the Beats Per Minute (1/4) to change the writing delay. If False, will use the standard milliseconds delay instead.
        /// </summary>
        public static bool DiscoUseBeatsPerMinute
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DiscoUseBeatsPerMinute;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.DiscoUseBeatsPerMinute = value;
            }
        }
        /// <summary>
        /// [Disco] Uses the black and white cycle to produce the same effect as the legacy "fed" screensaver introduced back in v0.0.1
        /// </summary>
        public static bool DiscoEnableFedMode
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DiscoEnableFedMode;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.DiscoEnableFedMode = value;
            }
        }
        /// <summary>
        /// [Disco] The minimum red color level (true color)
        /// </summary>
        public static int DiscoMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DiscoMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.DiscoMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Disco] The minimum green color level (true color)
        /// </summary>
        public static int DiscoMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DiscoMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.DiscoMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Disco] The minimum blue color level (true color)
        /// </summary>
        public static int DiscoMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DiscoMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.DiscoMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Disco] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int DiscoMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DiscoMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.DiscoMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Disco] The maximum red color level (true color)
        /// </summary>
        public static int DiscoMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DiscoMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.DiscoMaximumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.DiscoMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.DiscoMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Disco] The maximum green color level (true color)
        /// </summary>
        public static int DiscoMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DiscoMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.DiscoMaximumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.DiscoMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.DiscoMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Disco] The maximum blue color level (true color)
        /// </summary>
        public static int DiscoMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DiscoMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.DiscoMaximumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.DiscoMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.DiscoMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Disco] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int DiscoMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DiscoMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.DiscoMaximumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.DiscoMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.DiscoMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for Disco
    /// </summary>
    public class DiscoDisplay : BaseScreensaver, IScreensaver
    {

        private int CurrentColor = 0;
        private int CurrentColorR, CurrentColorG, CurrentColorB;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Disco";

        /// <inheritdoc/>
        public override bool ScreensaverContainsFlashingImages { get; set; } = true;

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            int MaximumColors = DiscoSettings.DiscoMaximumColorLevel;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", MaximumColors);
            int MaximumColorsR = DiscoSettings.DiscoMaximumRedColorLevel;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", MaximumColorsR);
            int MaximumColorsG = DiscoSettings.DiscoMaximumGreenColorLevel;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", MaximumColorsG);
            int MaximumColorsB = DiscoSettings.DiscoMaximumBlueColorLevel;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", MaximumColorsB);

            // Select the background color
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Cycling colors: {0}", DiscoSettings.DiscoCycleColors);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "fed (future-eyes-destroyer) mode: {0}", DiscoSettings.DiscoEnableFedMode);
            if (!DiscoSettings.DiscoEnableFedMode)
            {
                if (DiscoSettings.DiscoTrueColor)
                {
                    if (!DiscoSettings.DiscoCycleColors)
                    {
                        int RedColorNum = RandomDriver.Random(255);
                        int GreenColorNum = RandomDriver.Random(255);
                        int BlueColorNum = RandomDriver.Random(255);
                        DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                        var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                        ColorTools.SetConsoleColor(ColorStorage, true);
                    }
                    else
                    {
                        DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", CurrentColorR, CurrentColorG, CurrentColorB);
                        var ColorStorage = new Color(CurrentColorR, CurrentColorG, CurrentColorB);
                        ColorTools.SetConsoleColor(ColorStorage, true);
                    }
                }
                else
                {
                    if (!DiscoSettings.DiscoCycleColors)
                    {
                        int color = RandomDriver.Random(255);
                        DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", color);
                        ColorTools.SetConsoleColor(new Color(color), true);
                    }
                    else
                    {
                        DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", CurrentColor);
                        ColorTools.SetConsoleColor(new Color(CurrentColor), true);
                    }
                }
            }
            else
            {
                if (CurrentColor == (int)ConsoleColors.Black)
                {
                    CurrentColor = (int)ConsoleColors.White;
                }
                else
                {
                    CurrentColor = (int)ConsoleColors.Black;
                }
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", CurrentColor);
                ColorTools.SetConsoleColor(new Color(CurrentColor), true);
            }

            // Make the disco effect!
            ConsoleWrapper.Clear();

            // Switch to the next color
            if (DiscoSettings.DiscoTrueColor)
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
            else if (CurrentColor >= MaximumColors)
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Color level exceeded maximum color. {0} >= {1}", CurrentColor, MaximumColors);
                CurrentColor = 0;
            }
            else
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Stepping one...");
                CurrentColor += 1;
            }

            // Check to see if we're dealing with beats per minute
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Using BPM: {0}", DiscoSettings.DiscoUseBeatsPerMinute);
            if (DiscoSettings.DiscoUseBeatsPerMinute)
            {
                int BeatInterval = (int)Math.Round(60000d / DiscoSettings.DiscoDelay);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Beat interval from {0} BPM: {1} ms", DiscoSettings.DiscoDelay, BeatInterval);
                ThreadManager.SleepNoBlock(BeatInterval, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }
            else
            {
                ThreadManager.SleepNoBlock(DiscoSettings.DiscoDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }
        }

    }
}
