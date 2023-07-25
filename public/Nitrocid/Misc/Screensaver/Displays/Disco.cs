
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

using System;
using ColorSeq;
using KS.ConsoleBase.Colors;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Misc.Threading;

namespace KS.Misc.Screensaver.Displays
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
                return Config.SaverConfig.DiscoTrueColor;
            }
            set
            {
                Config.SaverConfig.DiscoTrueColor = value;
            }
        }
        /// <summary>
        /// [Disco] Enable color cycling
        /// </summary>
        public static bool DiscoCycleColors
        {
            get
            {
                return Config.SaverConfig.DiscoCycleColors;
            }
            set
            {
                Config.SaverConfig.DiscoCycleColors = value;
            }
        }
        /// <summary>
        /// [Disco] How many milliseconds, or beats per minute, to wait before making the next write?
        /// </summary>
        public static int DiscoDelay
        {
            get
            {
                return Config.SaverConfig.DiscoDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                Config.SaverConfig.DiscoDelay = value;
            }
        }
        /// <summary>
        /// [Disco] Whether to use the Beats Per Minute (1/4) to change the writing delay. If False, will use the standard milliseconds delay instead.
        /// </summary>
        public static bool DiscoUseBeatsPerMinute
        {
            get
            {
                return Config.SaverConfig.DiscoUseBeatsPerMinute;
            }
            set
            {
                Config.SaverConfig.DiscoUseBeatsPerMinute = value;
            }
        }
        /// <summary>
        /// [Disco] Uses the black and white cycle to produce the same effect as the legacy "fed" screensaver introduced back in v0.0.1
        /// </summary>
        public static bool DiscoEnableFedMode
        {
            get
            {
                return Config.SaverConfig.DiscoEnableFedMode;
            }
            set
            {
                Config.SaverConfig.DiscoEnableFedMode = value;
            }
        }
        /// <summary>
        /// [Disco] The minimum red color level (true color)
        /// </summary>
        public static int DiscoMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.DiscoMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.DiscoMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Disco] The minimum green color level (true color)
        /// </summary>
        public static int DiscoMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.DiscoMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.DiscoMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Disco] The minimum blue color level (true color)
        /// </summary>
        public static int DiscoMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.DiscoMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.DiscoMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Disco] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int DiscoMinimumColorLevel
        {
            get
            {
                return Config.SaverConfig.DiscoMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.DiscoMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Disco] The maximum red color level (true color)
        /// </summary>
        public static int DiscoMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.DiscoMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.DiscoMaximumRedColorLevel)
                    value = Config.SaverConfig.DiscoMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.DiscoMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Disco] The maximum green color level (true color)
        /// </summary>
        public static int DiscoMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.DiscoMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.DiscoMaximumGreenColorLevel)
                    value = Config.SaverConfig.DiscoMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.DiscoMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Disco] The maximum blue color level (true color)
        /// </summary>
        public static int DiscoMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.DiscoMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.DiscoMaximumBlueColorLevel)
                    value = Config.SaverConfig.DiscoMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.DiscoMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Disco] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int DiscoMaximumColorLevel
        {
            get
            {
                return Config.SaverConfig.DiscoMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.DiscoMaximumColorLevel)
                    value = Config.SaverConfig.DiscoMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.DiscoMaximumColorLevel = value;
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
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", MaximumColors);
            int MaximumColorsR = DiscoSettings.DiscoMaximumRedColorLevel;
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", MaximumColorsR);
            int MaximumColorsG = DiscoSettings.DiscoMaximumGreenColorLevel;
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", MaximumColorsG);
            int MaximumColorsB = DiscoSettings.DiscoMaximumBlueColorLevel;
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", MaximumColorsB);

            // Select the background color
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Cycling colors: {0}", DiscoSettings.DiscoCycleColors);
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "fed (future-eyes-destroyer) mode: {0}", DiscoSettings.DiscoEnableFedMode);
            if (!DiscoSettings.DiscoEnableFedMode)
            {
                if (DiscoSettings.DiscoTrueColor)
                {
                    if (!DiscoSettings.DiscoCycleColors)
                    {
                        int RedColorNum = RandomDriver.Random(255);
                        int GreenColorNum = RandomDriver.Random(255);
                        int BlueColorNum = RandomDriver.Random(255);
                        DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                        var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                        KernelColorTools.SetConsoleColor(ColorStorage, true, true);
                    }
                    else
                    {
                        DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", CurrentColorR, CurrentColorG, CurrentColorB);
                        var ColorStorage = new Color(CurrentColorR, CurrentColorG, CurrentColorB);
                        KernelColorTools.SetConsoleColor(ColorStorage, true, true);
                    }
                }
                else
                {
                    if (!DiscoSettings.DiscoCycleColors)
                    {
                        int color = RandomDriver.Random(255);
                        DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", color);
                        KernelColorTools.SetConsoleColor(new Color(color), true, true);
                    }
                    else
                    {
                        DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", CurrentColor);
                        KernelColorTools.SetConsoleColor(new Color(CurrentColor), true, true);
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
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", CurrentColor);
                KernelColorTools.SetConsoleColor(new Color(CurrentColor), true, true);
            }

            // Make the disco effect!
            ConsoleBase.ConsoleWrapper.Clear();

            // Switch to the next color
            if (DiscoSettings.DiscoTrueColor)
            {
                if (CurrentColorR >= MaximumColorsR)
                {
                    DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Red level exceeded maximum color. {0} >= {1}", CurrentColorR, MaximumColorsR);
                    CurrentColorR = 0;
                }
                else
                {
                    DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Stepping one (R)...");
                    CurrentColorR += 1;
                }
                if (CurrentColorG >= MaximumColorsG)
                {
                    DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Green level exceeded maximum color. {0} >= {1}", CurrentColorG, MaximumColorsG);
                    CurrentColorG = 0;
                }
                else if (CurrentColorR == 0)
                {
                    DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Stepping one (G)...");
                    CurrentColorG += 1;
                }
                if (CurrentColorB >= MaximumColorsB)
                {
                    DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Blue level exceeded maximum color. {0} >= {1}", CurrentColorB, MaximumColorsB);
                    CurrentColorB = 0;
                }
                else if (CurrentColorG == 0 & CurrentColorR == 0)
                {
                    DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Stepping one (B)...");
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
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Color level exceeded maximum color. {0} >= {1}", CurrentColor, MaximumColors);
                CurrentColor = 0;
            }
            else
            {
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Stepping one...");
                CurrentColor += 1;
            }

            // Check to see if we're dealing with beats per minute
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Using BPM: {0}", DiscoSettings.DiscoUseBeatsPerMinute);
            if (DiscoSettings.DiscoUseBeatsPerMinute)
            {
                int BeatInterval = (int)Math.Round(60000d / DiscoSettings.DiscoDelay);
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Beat interval from {0} BPM: {1} ms", DiscoSettings.DiscoDelay, BeatInterval);
                ThreadManager.SleepNoBlock(BeatInterval, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }
            else
            {
                ThreadManager.SleepNoBlock(DiscoSettings.DiscoDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }
        }

    }
}
