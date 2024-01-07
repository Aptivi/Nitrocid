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
using Terminaux.Writer.FancyWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for BarWave
    /// </summary>
    public static class BarWaveSettings
    {

        /// <summary>
        /// [BarWave] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool BarWaveTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BarWaveTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.BarWaveTrueColor = value;
            }
        }
        /// <summary>
        /// [BarWave] The level of the frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>.
        /// </summary>
        public static double BarWaveFrequencyLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BarWaveFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 2;
                ScreensaverPackInit.SaversConfig.BarWaveFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int BarWaveDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BarWaveDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                ScreensaverPackInit.SaversConfig.BarWaveDelay = value;
            }
        }
        /// <summary>
        /// [BarWave] The minimum red color level (true color)
        /// </summary>
        public static int BarWaveMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BarWaveMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BarWaveMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The minimum green color level (true color)
        /// </summary>
        public static int BarWaveMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BarWaveMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BarWaveMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The minimum blue color level (true color)
        /// </summary>
        public static int BarWaveMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BarWaveMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BarWaveMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int BarWaveMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BarWaveMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.BarWaveMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The maximum red color level (true color)
        /// </summary>
        public static int BarWaveMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BarWaveMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.BarWaveMaximumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.BarWaveMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BarWaveMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The maximum green color level (true color)
        /// </summary>
        public static int BarWaveMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BarWaveMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.BarWaveMaximumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.BarWaveMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BarWaveMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The maximum blue color level (true color)
        /// </summary>
        public static int BarWaveMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BarWaveMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.BarWaveMaximumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.BarWaveMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BarWaveMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int BarWaveMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BarWaveMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.BarWaveMaximumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.BarWaveMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.BarWaveMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display for BarWave
    /// </summary>
    public class BarWaveDisplay : BaseScreensaver, IScreensaver
    {
        private double TimeSecs = 0.0d;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "BarWave";

        /// <inheritdoc/>
        public override void ScreensaverPreparation() =>
            ColorTools.LoadBack();

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // First, prepare how many bars to render according to the console size
            int BarHeight = ConsoleWrapper.WindowHeight - 4;
            int BarWidthOutside = 3;
            int BarCount = ConsoleWrapper.WindowWidth / 4;

            // Then, go ahead and make these bars wave themselves.
            int[] CurrentPos = new int[BarCount];
            double Frequency = Math.PI / BarWaveSettings.BarWaveFrequencyLevel;

            // Set the current positions
            for (int i = 0; i < BarCount; i++)
            {
                TimeSecs += 0.1;
                if (TimeSecs > 2.0)
                    TimeSecs = 0.0;
                CurrentPos[i] = (int)Math.Abs(BarHeight * Math.Cos(Frequency * TimeSecs));
            }

            // Render the bars
            for (int i = 0; i < BarCount; i++)
            {
                int ThisBarLeft = (BarWidthOutside + 1) * i + 1;
                int Pos = (int)(100 * (CurrentPos[i] / (double)BarHeight));
                int RedColorNum = RandomDriver.Random(BarWaveSettings.BarWaveMinimumRedColorLevel, BarWaveSettings.BarWaveMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(BarWaveSettings.BarWaveMinimumGreenColorLevel, BarWaveSettings.BarWaveMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(BarWaveSettings.BarWaveMinimumBlueColorLevel, BarWaveSettings.BarWaveMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                    ProgressBarVerticalColor.WriteVerticalProgress(Pos, ThisBarLeft, 1, 4, ColorStorage);
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(BarWaveSettings.BarWaveDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro()
        {
            TimeSecs = 0.0d;
        }

    }
}
