
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
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
    /// Settings for Lightspeed
    /// </summary>
    public static class LightspeedSettings
    {

        private static bool _lightspeedCycleColors;
        private static int _lightspeedMinimumRedColorLevel = 0;
        private static int _lightspeedMinimumGreenColorLevel = 0;
        private static int _lightspeedMinimumBlueColorLevel = 0;
        private static int _lightspeedMinimumColorLevel = 0;
        private static int _lightspeedMaximumRedColorLevel = 255;
        private static int _lightspeedMaximumGreenColorLevel = 255;
        private static int _lightspeedMaximumBlueColorLevel = 255;
        private static int _lightspeedMaximumColorLevel = 255;

        /// <summary>
        /// [Lightspeed] Enable color cycling
        /// </summary>
        public static bool LightspeedCycleColors
        {
            get
            {
                return _lightspeedCycleColors;
            }
            set
            {
                _lightspeedCycleColors = value;
            }
        }
        /// <summary>
        /// [Lightspeed] The minimum red color level (true color)
        /// </summary>
        public static int LightspeedMinimumRedColorLevel
        {
            get
            {
                return _lightspeedMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _lightspeedMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Lightspeed] The minimum green color level (true color)
        /// </summary>
        public static int LightspeedMinimumGreenColorLevel
        {
            get
            {
                return _lightspeedMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _lightspeedMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Lightspeed] The minimum blue color level (true color)
        /// </summary>
        public static int LightspeedMinimumBlueColorLevel
        {
            get
            {
                return _lightspeedMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _lightspeedMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Lightspeed] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int LightspeedMinimumColorLevel
        {
            get
            {
                return _lightspeedMinimumColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _lightspeedMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Lightspeed] The maximum red color level (true color)
        /// </summary>
        public static int LightspeedMaximumRedColorLevel
        {
            get
            {
                return _lightspeedMaximumRedColorLevel;
            }
            set
            {
                if (value <= _lightspeedMinimumRedColorLevel)
                    value = _lightspeedMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _lightspeedMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Lightspeed] The maximum green color level (true color)
        /// </summary>
        public static int LightspeedMaximumGreenColorLevel
        {
            get
            {
                return _lightspeedMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _lightspeedMinimumGreenColorLevel)
                    value = _lightspeedMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _lightspeedMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Lightspeed] The maximum blue color level (true color)
        /// </summary>
        public static int LightspeedMaximumBlueColorLevel
        {
            get
            {
                return _lightspeedMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _lightspeedMinimumBlueColorLevel)
                    value = _lightspeedMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _lightspeedMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Lightspeed] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int LightspeedMaximumColorLevel
        {
            get
            {
                return _lightspeedMaximumColorLevel;
            }
            set
            {
                if (value <= _lightspeedMinimumColorLevel)
                    value = _lightspeedMinimumColorLevel;
                if (value > 255)
                    value = 255;
                _lightspeedMaximumColorLevel = value;
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
            int MaximumColors = LightspeedSettings.LightspeedMaximumColorLevel;
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", MaximumColors);
            int MaximumColorsR = LightspeedSettings.LightspeedMaximumRedColorLevel;
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", MaximumColorsR);
            int MaximumColorsG = LightspeedSettings.LightspeedMaximumGreenColorLevel;
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", MaximumColorsG);
            int MaximumColorsB = LightspeedSettings.LightspeedMaximumBlueColorLevel;
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", MaximumColorsB);

            ConsoleBase.ConsoleWrapper.CursorVisible = false;

            // Select the background color
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Cycling colors: {0}", LightspeedSettings.LightspeedCycleColors);
            if (!LightspeedSettings.LightspeedCycleColors)
            {
                int RedColorNum = RandomDriver.Random(255);
                int GreenColorNum = RandomDriver.Random(255);
                int BlueColorNum = RandomDriver.Random(255);
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                ColorTools.SetConsoleColor(ColorStorage, true, true);
            }
            else
            {
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", CurrentColorR, CurrentColorG, CurrentColorB);
                var ColorStorage = new Color(CurrentColorR, CurrentColorG, CurrentColorB);
                ColorTools.SetConsoleColor(ColorStorage, true, true);
            }

            // Make the disco effect!
            ConsoleBase.ConsoleWrapper.Clear();

            // Switch to the next color
            if (LightspeedSettings.LightspeedCycleColors)
            {
                if (CurrentColorR >= MaximumColorsR)
                {
                    DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Red level exceeded maximum color. {0} >= {1}", CurrentColorR, MaximumColorsR);
                    CurrentColorR = 0;
                }
                else
                {
                    DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Stepping one (R)...");
                    CurrentColorR += 1;
                }
                if (CurrentColorG >= MaximumColorsG)
                {
                    DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Green level exceeded maximum color. {0} >= {1}", CurrentColorG, MaximumColorsG);
                    CurrentColorG = 0;
                }
                else if (CurrentColorR == 0)
                {
                    DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Stepping one (G)...");
                    CurrentColorG += 1;
                }
                if (CurrentColorB >= MaximumColorsB)
                {
                    DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Blue level exceeded maximum color. {0} >= {1}", CurrentColorB, MaximumColorsB);
                    CurrentColorB = 0;
                }
                else if (CurrentColorG == 0 & CurrentColorR == 0)
                {
                    DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Stepping one (B)...");
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
