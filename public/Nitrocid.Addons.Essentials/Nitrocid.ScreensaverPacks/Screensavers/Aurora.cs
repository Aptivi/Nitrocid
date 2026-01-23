//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using System.Text;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Nitrocid.Kernel.Configuration;
using Terminaux.Colors;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Aurora
    /// </summary>
    public class AuroraDisplay : BaseScreensaver, IScreensaver
    {

        private int redPosIdx = 0;
        private int greenPosIdx = 0;
        private int bluePosIdx = 0;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Aurora";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Select a color range for the aurora
            double RedFrequency = Math.PI / 24;
            double GreenFrequency = Math.PI / 16;
            double BlueFrequency = Math.PI / 10;
            int[] RedCurrentLevels = GetColorLevels(RedFrequency);
            int[] GreenCurrentLevels = GetColorLevels(GreenFrequency);
            int[] BlueCurrentLevels = GetColorLevels(BlueFrequency);

            // Set some value ranges
            int RedColorNumTo = Math.Abs(RedCurrentLevels[redPosIdx]);
            int GreenColorNumTo = Math.Abs(GreenCurrentLevels[greenPosIdx]);
            int BlueColorNumTo = Math.Abs(BlueCurrentLevels[bluePosIdx]);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "R: {0} [{1}], G: {2} [{3}], B: {4} [{5}]", vars: [RedColorNumTo, redPosIdx, GreenColorNumTo, greenPosIdx, BlueColorNumTo, bluePosIdx]);

            // Advance the indexes
            redPosIdx++;
            if (redPosIdx >= RedCurrentLevels.Length)
                redPosIdx = 0;
            greenPosIdx++;
            if (greenPosIdx >= GreenCurrentLevels.Length)
                greenPosIdx = 0;
            bluePosIdx++;
            if (bluePosIdx >= BlueCurrentLevels.Length)
                bluePosIdx = 0;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Indexes advanced to {0}, {1}, {2}", vars: [redPosIdx, greenPosIdx, bluePosIdx]);

            // Prepare the color bands
            (int, int, int)[] ColorBands = GetColorBands(RedColorNumTo, GreenColorNumTo, BlueColorNumTo);

            // Actually draw the aurora to the background
            StringBuilder builder = new();
            foreach ((int, int, int) colorBand in ColorBands)
            {
                int red = colorBand.Item1;
                int green = colorBand.Item2;
                int blue = colorBand.Item3;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Aurora drawing... {0}, {1}, {2}", vars: [red, green, blue]);
                Color storage = new(red, green, blue);
                builder.Append($"{storage.VTSequenceBackground()}{new string(' ', ConsoleWrapper.WindowWidth)}");
            }
            if (!ConsoleResizeHandler.WasResized(false))
                TextWriterRaw.WriteRaw(builder.ToString());
            ConsoleWrapper.SetCursorPosition(0, 0);
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.AuroraDelay);

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro()
        {
            redPosIdx = 0;
            greenPosIdx = 0;
            bluePosIdx = 0;
        }

        private static int[] GetColorLevels(double Frequency)
        {
            List<int> ColorLevels = [];
            int Count = 10000;
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
            return [.. ColorLevels];
        }

        private static (int, int, int)[] GetColorBands(int redColorNumTo, int greenColorNumTo, int blueColorNumTo)
        {
            List<(int, int, int)> ColorBands = [];
            int Count = ConsoleWrapper.WindowHeight;

            // Set thresholds
            double redBandThreshold = (double)redColorNumTo / Count;
            double greenBandThreshold = (double)greenColorNumTo / Count;
            double blueBandThreshold = (double)blueColorNumTo / Count;

            // Add the color levels to the color bands
            for (int i = 0; i < Count; i++)
            {
                int finalRedLevel = (int)(redBandThreshold * i);
                int finalGreenLevel = (int)(greenBandThreshold * i);
                int finalBlueLevel = (int)(blueBandThreshold * i);
                ColorBands.Add((finalRedLevel, finalGreenLevel, finalBlueLevel));
            }
            return [.. ColorBands];
        }

    }
}
