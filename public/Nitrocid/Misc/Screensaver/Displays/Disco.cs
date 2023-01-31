
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
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Misc.Threading;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Disco
    /// </summary>
    public static class DiscoSettings
    {

        private static bool _TrueColor = true;
        private static bool _CycleColors;
        private static int _Delay = 100;
        private static bool _UseBeatsPerMinute;
        private static bool _EnableFedMode;
        private static int _MinimumRedColorLevel = 0;
        private static int _MinimumGreenColorLevel = 0;
        private static int _MinimumBlueColorLevel = 0;
        private static int _MinimumColorLevel = 0;
        private static int _MaximumRedColorLevel = 255;
        private static int _MaximumGreenColorLevel = 255;
        private static int _MaximumBlueColorLevel = 255;
        private static int _MaximumColorLevel = 255;

        /// <summary>
        /// [Disco] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool DiscoTrueColor
        {
            get
            {
                return _TrueColor;
            }
            set
            {
                _TrueColor = value;
            }
        }
        /// <summary>
        /// [Disco] Enable color cycling
        /// </summary>
        public static bool DiscoCycleColors
        {
            get
            {
                return _CycleColors;
            }
            set
            {
                _CycleColors = value;
            }
        }
        /// <summary>
        /// [Disco] How many milliseconds, or beats per minute, to wait before making the next write?
        /// </summary>
        public static int DiscoDelay
        {
            get
            {
                return _Delay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                _Delay = value;
            }
        }
        /// <summary>
        /// [Disco] Whether to use the Beats Per Minute (1/4) to change the writing delay. If False, will use the standard milliseconds delay instead.
        /// </summary>
        public static bool DiscoUseBeatsPerMinute
        {
            get
            {
                return _UseBeatsPerMinute;
            }
            set
            {
                _UseBeatsPerMinute = value;
            }
        }
        /// <summary>
        /// [Disco] Uses the black and white cycle to produce the same effect as the legacy "fed" screensaver introduced back in v0.0.1
        /// </summary>
        public static bool DiscoEnableFedMode
        {
            get
            {
                return _EnableFedMode;
            }
            set
            {
                _EnableFedMode = value;
            }
        }
        /// <summary>
        /// [Disco] The minimum red color level (true color)
        /// </summary>
        public static int DiscoMinimumRedColorLevel
        {
            get
            {
                return _MinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Disco] The minimum green color level (true color)
        /// </summary>
        public static int DiscoMinimumGreenColorLevel
        {
            get
            {
                return _MinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Disco] The minimum blue color level (true color)
        /// </summary>
        public static int DiscoMinimumBlueColorLevel
        {
            get
            {
                return _MinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Disco] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int DiscoMinimumColorLevel
        {
            get
            {
                return _MinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _MinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Disco] The maximum red color level (true color)
        /// </summary>
        public static int DiscoMaximumRedColorLevel
        {
            get
            {
                return _MaximumRedColorLevel;
            }
            set
            {
                if (value <= _MinimumRedColorLevel)
                    value = _MinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _MaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Disco] The maximum green color level (true color)
        /// </summary>
        public static int DiscoMaximumGreenColorLevel
        {
            get
            {
                return _MaximumGreenColorLevel;
            }
            set
            {
                if (value <= _MinimumGreenColorLevel)
                    value = _MinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _MaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Disco] The maximum blue color level (true color)
        /// </summary>
        public static int DiscoMaximumBlueColorLevel
        {
            get
            {
                return _MaximumBlueColorLevel;
            }
            set
            {
                if (value <= _MinimumBlueColorLevel)
                    value = _MinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _MaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Disco] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int DiscoMaximumColorLevel
        {
            get
            {
                return _MaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _MinimumColorLevel)
                    value = _MinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _MaximumColorLevel = value;
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
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ConsoleBase.ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleBase.ConsoleWrapper.Clear();
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleBase.ConsoleWrapper.WindowWidth, ConsoleBase.ConsoleWrapper.WindowHeight);
        }

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

            ConsoleBase.ConsoleWrapper.CursorVisible = false;

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
                        ColorTools.SetConsoleColor(ColorStorage, true, true);
                    }
                    else
                    {
                        DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", CurrentColorR, CurrentColorG, CurrentColorB);
                        var ColorStorage = new Color(CurrentColorR, CurrentColorG, CurrentColorB);
                        ColorTools.SetConsoleColor(ColorStorage, true, true);
                    }
                }
                else
                {
                    if (!DiscoSettings.DiscoCycleColors)
                    {
                        int color = RandomDriver.Random(255);
                        DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", color);
                        ColorTools.SetConsoleColor(new Color(color), true, true);
                    }
                    else
                    {
                        DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", CurrentColor);
                        ColorTools.SetConsoleColor(new Color(CurrentColor), true, true);
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
                ColorTools.SetConsoleColor(new Color(CurrentColor), true, true);
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
