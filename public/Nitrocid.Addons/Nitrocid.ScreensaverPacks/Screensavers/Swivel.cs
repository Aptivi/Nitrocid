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

using System;
using System.Collections.Generic;
using System.Linq;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
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
                return ScreensaverPackInit.SaversConfig.SwivelDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                ScreensaverPackInit.SaversConfig.SwivelDelay = value;
            }
        }
        /// <summary>
        /// [Swivel] The level of the horizontal frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>. Use this to create beautiful wavy swivels!
        /// </summary>
        public static double SwivelHorizontalFrequencyLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SwivelHorizontalFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 3;
                ScreensaverPackInit.SaversConfig.SwivelHorizontalFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The level of the vertical frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>. Use this to create beautiful wavy swivels!
        /// </summary>
        public static double SwivelVerticalFrequencyLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SwivelVerticalFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 8;
                ScreensaverPackInit.SaversConfig.SwivelVerticalFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The minimum red color level (true color)
        /// </summary>
        public static int SwivelMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SwivelMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.SwivelMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The minimum green color level (true color)
        /// </summary>
        public static int SwivelMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SwivelMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.SwivelMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The minimum blue color level (true color)
        /// </summary>
        public static int SwivelMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SwivelMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.SwivelMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int SwivelMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SwivelMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.SwivelMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The maximum red color level (true color)
        /// </summary>
        public static int SwivelMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SwivelMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.SwivelMaximumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.SwivelMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.SwivelMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The maximum green color level (true color)
        /// </summary>
        public static int SwivelMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SwivelMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.SwivelMaximumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.SwivelMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.SwivelMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The maximum blue color level (true color)
        /// </summary>
        public static int SwivelMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SwivelMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.SwivelMaximumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.SwivelMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.SwivelMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int SwivelMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SwivelMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.SwivelMaximumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.SwivelMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.SwivelMaximumColorLevel = value;
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
            posIdxVertical = 0;
            posIdxHorizontal = 0;
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
            List<int> CurrentPosVertical = [];
            List<int> CurrentPosHorizontal = [];
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
                TextWriterWhereColor.WriteWhereColorBack(" ", PosHorizontal, PosVertical, Color.Empty, ColorStorage);
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(SwivelSettings.SwivelDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
