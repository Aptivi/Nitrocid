
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
using ColorSeq;
using KS.ConsoleBase;
using KS.Kernel.Configuration;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Aurora
    /// </summary>
    public static class AuroraSettings
    {

        /// <summary>
        /// [Aurora] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int AuroraDelay
        {
            get
            {
                return Config.SaverConfig.AuroraDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                Config.SaverConfig.AuroraDelay = value;
            }
        }

    }

    /// <summary>
    /// Display code for Aurora
    /// </summary>
    public class AuroraDisplay : BaseScreensaver, IScreensaver
    {

        private int greenPosIdx = 0;
        private int bluePosIdx = 0;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Aurora";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Select a color range for the aurora
            double GreenFrequency = Math.PI / 16;
            double BlueFrequency = Math.PI / 8;
            int[] GreenCurrentLevels = GetColorLevels(GreenFrequency);
            int[] BlueCurrentLevels = GetColorLevels(BlueFrequency);

            // Set some value ranges
            int GreenColorNumTo = Math.Abs(GreenCurrentLevels[greenPosIdx]);
            int BlueColorNumTo = Math.Abs(BlueCurrentLevels[bluePosIdx]);

            // Advance the indexes
            greenPosIdx++;
            if (greenPosIdx >= GreenCurrentLevels.Length)
                greenPosIdx = 0;
            bluePosIdx++;
            if (bluePosIdx >= BlueCurrentLevels.Length)
                bluePosIdx = 0;

            // Prepare the color bands
            (int, int)[] ColorBands = GetColorBands(GreenColorNumTo, BlueColorNumTo);

            // Actually draw the aurora to the background
            foreach ((int, int) colorBand in ColorBands)
            {
                int red = 0;
                int green = colorBand.Item1;
                int blue = colorBand.Item1;
                Color storage = new(red, green, blue);
                if (!ConsoleResizeListener.WasResized(false))
                    TextWriterColor.Write(new string(' ', ConsoleWrapper.WindowWidth), false, Color.Empty, storage);
            }
            ConsoleWrapper.SetCursorPosition(0, 0);
            ThreadManager.SleepNoBlock(AuroraSettings.AuroraDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);

            // Reset resize sync
            ConsoleResizeListener.WasResized();
        }

        private static int[] GetColorLevels(double Frequency)
        {
            List<int> ColorLevels = new();
            int Count = 200;
            int AuroraMaxColor = 80;
            double TimeSecs = 0.0;
            bool isSet = false;
            for (int i = 0; i < Count; i++)
            {
                TimeSecs += 0.1;
                double calculatedHeight = AuroraMaxColor * Math.Cos(Frequency * TimeSecs + Math.PI / 2) / 2;
                ColorLevels.Add((int)calculatedHeight);
                if ((int)calculatedHeight == 0 && isSet)
                    break;
                if (!isSet)
                    isSet = true;
            }
            return ColorLevels.ToArray();
        }

        private static (int, int)[] GetColorBands(int greenColorNumTo, int blueColorNumTo)
        {
            List<(int, int)> ColorBands = new();
            int Count = ConsoleWrapper.WindowHeight;

            // Set thresholds
            double greenBandThreshold = (double)greenColorNumTo / Count;
            double blueBandThreshold = (double)blueColorNumTo / Count;

            // Add the color levels to the color bands
            for (int i = 0; i < Count; i++)
            {
                int finalGreenLevel = (int)(greenBandThreshold * i);
                int finalBlueLevel = (int)(blueBandThreshold * i);
                ColorBands.Add((finalGreenLevel, finalBlueLevel));
            }
            return ColorBands.ToArray();
        }

    }
}
