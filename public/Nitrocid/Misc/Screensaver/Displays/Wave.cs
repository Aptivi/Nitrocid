
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
using ColorSeq;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Wave
    /// </summary>
    public static class WaveSettings
    {

        /// <summary>
        /// [Wave] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int WaveDelay
        {
            get
            {
                return Config.SaverConfig.WaveDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                Config.SaverConfig.WaveDelay = value;
            }
        }
        /// <summary>
        /// [Wave] The minimum red color level (true color)
        /// </summary>
        public static int WaveMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.WaveMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.WaveMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The minimum green color level (true color)
        /// </summary>
        public static int WaveMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.WaveMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.WaveMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The minimum blue color level (true color)
        /// </summary>
        public static int WaveMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.WaveMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.WaveMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int WaveMinimumColorLevel
        {
            get
            {
                return Config.SaverConfig.WaveMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.WaveMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The maximum red color level (true color)
        /// </summary>
        public static int WaveMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.WaveMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.WaveMaximumRedColorLevel)
                    value = Config.SaverConfig.WaveMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.WaveMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The maximum green color level (true color)
        /// </summary>
        public static int WaveMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.WaveMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.WaveMaximumGreenColorLevel)
                    value = Config.SaverConfig.WaveMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.WaveMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The maximum blue color level (true color)
        /// </summary>
        public static int WaveMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.WaveMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.WaveMaximumBlueColorLevel)
                    value = Config.SaverConfig.WaveMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.WaveMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int WaveMaximumColorLevel
        {
            get
            {
                return Config.SaverConfig.WaveMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.WaveMaximumColorLevel)
                    value = Config.SaverConfig.WaveMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.WaveMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display for Wave
    /// </summary>
    public class WaveDisplay : BaseScreensaver, IScreensaver
    {
        private int posIdx = 0;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Wave";

        /// <inheritdoc/>
        public override void ScreensaverPreparation() =>
            KernelColorTools.LoadBack();

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // First, prepare how many dots to render according to the console size
            int Height = ConsoleWrapper.WindowHeight - 4;
            int Count = ConsoleWrapper.WindowWidth;

            // Then, go ahead and make these bars wave themselves.
            List<int> CurrentPos = new();
            double Frequency = Math.PI / 3;

            // Set the current positions
            double TimeSecs = 0.0;
            bool isSet = false;
            for (int i = 0; i < Count; i++)
            {
                TimeSecs += 0.1;
                double calculatedHeight = Height * Math.Cos(Frequency * TimeSecs) / 2;
                CurrentPos.Add((int)calculatedHeight);
                if (calculatedHeight == Height / 2 && isSet)
                    break;
                if (!isSet)
                    isSet = true;
            }

            // Render the bars
            int RedColorNum = RandomDriver.Random(WaveSettings.WaveMinimumRedColorLevel, WaveSettings.WaveMaximumRedColorLevel);
            int GreenColorNum = RandomDriver.Random(WaveSettings.WaveMinimumGreenColorLevel, WaveSettings.WaveMaximumGreenColorLevel);
            int BlueColorNum = RandomDriver.Random(WaveSettings.WaveMinimumBlueColorLevel, WaveSettings.WaveMaximumBlueColorLevel);
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
            var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            for (int i = 0; i < Count; i++)
            {
                posIdx++;
                if (posIdx >= CurrentPos.Count)
                    posIdx = 0;
                int Pos = CurrentPos[posIdx] + Math.Abs(CurrentPos.Min()) + 2;
                if (!ConsoleResizeListener.WasResized(false))
                {
                    ThreadManager.SleepNoBlock(WaveSettings.WaveDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                    for (int j = 0; j < ConsoleWrapper.WindowHeight; j++)
                        TextWriterWhereColor.WriteWhere(" ", i, j, Color.Empty, KernelColorTools.GetColor(KernelColorType.Background));
                    TextWriterWhereColor.WriteWhere(" ", i, Pos, Color.Empty, ColorStorage);
                }
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(WaveSettings.WaveDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
