
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
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;

namespace KS.Misc.Screensaver.Displays
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
                return Config.SaverConfig.BarWaveTrueColor;
            }
            set
            {
                Config.SaverConfig.BarWaveTrueColor = value;
            }
        }
        /// <summary>
        /// [BarWave] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int BarWaveDelay
        {
            get
            {
                return Config.SaverConfig.BarWaveDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                Config.SaverConfig.BarWaveDelay = value;
            }
        }
        /// <summary>
        /// [BarWave] The minimum red color level (true color)
        /// </summary>
        public static int BarWaveMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.BarWaveMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BarWaveMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The minimum green color level (true color)
        /// </summary>
        public static int BarWaveMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.BarWaveMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BarWaveMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The minimum blue color level (true color)
        /// </summary>
        public static int BarWaveMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.BarWaveMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BarWaveMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int BarWaveMinimumColorLevel
        {
            get
            {
                return Config.SaverConfig.BarWaveMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.BarWaveMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The maximum red color level (true color)
        /// </summary>
        public static int BarWaveMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.BarWaveMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.BarWaveMaximumRedColorLevel)
                    value = Config.SaverConfig.BarWaveMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BarWaveMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The maximum green color level (true color)
        /// </summary>
        public static int BarWaveMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.BarWaveMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.BarWaveMaximumGreenColorLevel)
                    value = Config.SaverConfig.BarWaveMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BarWaveMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The maximum blue color level (true color)
        /// </summary>
        public static int BarWaveMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.BarWaveMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.BarWaveMaximumBlueColorLevel)
                    value = Config.SaverConfig.BarWaveMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BarWaveMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int BarWaveMaximumColorLevel
        {
            get
            {
                return Config.SaverConfig.BarWaveMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.BarWaveMaximumColorLevel)
                    value = Config.SaverConfig.BarWaveMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.BarWaveMaximumColorLevel = value;
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
            KernelColorTools.LoadBack();

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
            double Frequency = Math.PI / 2;

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
                int ThisBarLeft = ((BarWidthOutside + 1) * i) + 1;
                int Pos = (int)(100 * (CurrentPos[i] / (double)BarHeight));
                int RedColorNum = RandomDriver.Random(BarWaveSettings.BarWaveMinimumRedColorLevel, BarWaveSettings.BarWaveMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(BarWaveSettings.BarWaveMinimumGreenColorLevel, BarWaveSettings.BarWaveMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(BarWaveSettings.BarWaveMinimumBlueColorLevel, BarWaveSettings.BarWaveMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                    ProgressBarVerticalColor.WriteVerticalProgress(Pos, ThisBarLeft, 1, 4, ColorStorage);
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(BarWaveSettings.BarWaveDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
