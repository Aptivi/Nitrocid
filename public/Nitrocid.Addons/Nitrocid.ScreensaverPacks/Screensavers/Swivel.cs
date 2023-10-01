
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
using System.Collections.Generic;
using System.Linq;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for Swivel
    /// </summary>
    public static class SwivelSettings
    {

        /// <summary>
        /// [Swivel] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int SwivelDelay
        {
            get
            {
                return Config.SaverConfig.SwivelDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                Config.SaverConfig.SwivelDelay = value;
            }
        }
        /// <summary>
        /// [Swivel] The level of the horizontal frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>. Use this to create beautiful wavy swivels!
        /// </summary>
        public static double SwivelHorizontalFrequencyLevel
        {
            get
            {
                return Config.SaverConfig.SwivelHorizontalFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 3;
                Config.SaverConfig.SwivelHorizontalFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The level of the vertical frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>. Use this to create beautiful wavy swivels!
        /// </summary>
        public static double SwivelVerticalFrequencyLevel
        {
            get
            {
                return Config.SaverConfig.SwivelVerticalFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 8;
                Config.SaverConfig.SwivelVerticalFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The minimum red color level (true color)
        /// </summary>
        public static int SwivelMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.SwivelMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.SwivelMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The minimum green color level (true color)
        /// </summary>
        public static int SwivelMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.SwivelMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.SwivelMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The minimum blue color level (true color)
        /// </summary>
        public static int SwivelMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.SwivelMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.SwivelMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int SwivelMinimumColorLevel
        {
            get
            {
                return Config.SaverConfig.SwivelMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.SwivelMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The maximum red color level (true color)
        /// </summary>
        public static int SwivelMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.SwivelMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.SwivelMaximumRedColorLevel)
                    value = Config.SaverConfig.SwivelMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.SwivelMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The maximum green color level (true color)
        /// </summary>
        public static int SwivelMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.SwivelMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.SwivelMaximumGreenColorLevel)
                    value = Config.SaverConfig.SwivelMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.SwivelMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The maximum blue color level (true color)
        /// </summary>
        public static int SwivelMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.SwivelMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.SwivelMaximumBlueColorLevel)
                    value = Config.SaverConfig.SwivelMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.SwivelMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int SwivelMaximumColorLevel
        {
            get
            {
                return Config.SaverConfig.SwivelMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.SwivelMaximumColorLevel)
                    value = Config.SaverConfig.SwivelMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.SwivelMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display for Swivel
    /// </summary>
    public class SwivelDisplay : BaseScreensaver, IScreensaver
    {
        private int posIdxVertical = 0;
        private int posIdxHorizontal = 0;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Swivel";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            KernelColorTools.LoadBack();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // First, prepare how many dots to render according to the console size
            int Height = ConsoleWrapper.WindowHeight - 4;
            int Width = ConsoleWrapper.WindowWidth - 4;

            // Then, go ahead and make these bars swivel themselves.
            List<int> CurrentPosVertical = new();
            List<int> CurrentPosHorizontal = new();
            double FrequencyVertical = Math.PI / SwivelSettings.SwivelVerticalFrequencyLevel;
            double FrequencyHorizontal = Math.PI / SwivelSettings.SwivelHorizontalFrequencyLevel;

            // Set the current vertical positions
            double TimeSecsVertical = 0.0;
            bool isSetVertical = false;
            while (true)
            {
                TimeSecsVertical += 0.1;
                double calculatedHeight = Height * Math.Cos(FrequencyVertical * TimeSecsVertical) / 2;
                CurrentPosVertical.Add((int)calculatedHeight);
                if (calculatedHeight == Height / 2 && isSetVertical)
                    break;
                if (!isSetVertical)
                    isSetVertical = true;
            }

            // Set the current horizontal positions
            double TimeSecsHorizontal = 0.0;
            bool isSetHorizontal = false;
            while (true)
            {
                TimeSecsHorizontal += 0.1;
                double calculatedWidth = Width * Math.Cos(FrequencyHorizontal * TimeSecsHorizontal) / 2;
                CurrentPosHorizontal.Add((int)calculatedWidth);
                if (calculatedWidth == Width / 2 && isSetHorizontal)
                    break;
                if (!isSetHorizontal)
                    isSetHorizontal = true;
            }

            // Render the bars
            int RedColorNum = RandomDriver.Random(SwivelSettings.SwivelMinimumRedColorLevel, SwivelSettings.SwivelMaximumRedColorLevel);
            int GreenColorNum = RandomDriver.Random(SwivelSettings.SwivelMinimumGreenColorLevel, SwivelSettings.SwivelMaximumGreenColorLevel);
            int BlueColorNum = RandomDriver.Random(SwivelSettings.SwivelMinimumBlueColorLevel, SwivelSettings.SwivelMaximumBlueColorLevel);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
            var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            posIdxVertical++;
            if (posIdxVertical >= CurrentPosVertical.Count)
                posIdxVertical = 0;
            posIdxHorizontal++;
            if (posIdxHorizontal >= CurrentPosHorizontal.Count)
                posIdxHorizontal = 0;
            int PosVertical = CurrentPosVertical[posIdxVertical] + Math.Abs(CurrentPosVertical.Min()) + 2;
            int PosHorizontal = CurrentPosHorizontal[posIdxHorizontal] + Math.Abs(CurrentPosHorizontal.Min()) + 2;
            if (!ConsoleResizeListener.WasResized(false))
            {
                ThreadManager.SleepNoBlock(SwivelSettings.SwivelDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                TextWriterWhereColor.WriteWhere(" ", PosHorizontal, PosVertical, Color.Empty, ColorStorage);
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(SwivelSettings.SwivelDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
