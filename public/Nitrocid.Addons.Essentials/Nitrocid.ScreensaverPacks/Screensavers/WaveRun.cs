//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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
using System.Text;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for WaveRun
    /// </summary>
    public class WaveRunDisplay : BaseScreensaver, IScreensaver
    {
        // TODO: NKS_SCREENSAVERPACKS_WAVERUN_SETTINGS_DESC Shows a wave diagram from the right to the left of the console
        private Color waveColor = Color.Empty;
        private int posIdx = 0;
        private int blankAreas = 0;
        private int count = 0;
        private int cycle = 0;
        private List<int> currentPos = [];

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "WaveRun";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Get the color
            int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WaveRunMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.WaveRunMaximumRedColorLevel);
            int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WaveRunMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.WaveRunMaximumGreenColorLevel);
            int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WaveRunMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.WaveRunMaximumBlueColorLevel);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
            waveColor = new Color(RedColorNum, GreenColorNum, BlueColorNum);

            // Prepare how many dots to render according to the console size
            int height = ConsoleWrapper.WindowHeight - 4;
            count = ConsoleWrapper.WindowWidth;

            // Set the current positions
            bool isSet = false;
            double timeSecs = 0.0;
            double frequency = Math.PI / ScreensaverPackInit.SaversConfig.WaveRunFrequencyLevel;
            currentPos.Clear();
            for (int i = 0; i < count; i++)
            {
                timeSecs += 0.1;
                double calculatedHeight = height * Math.Cos(frequency * timeSecs) / 2;
                currentPos.Add((int)calculatedHeight);
                if (calculatedHeight == height / 2 && isSet)
                    break;
                if (!isSet)
                    isSet = true;
            }

            // Reset position
            posIdx = 0;
            cycle = 0;
            blankAreas = ConsoleWrapper.WindowWidth - 1;
            ThemeColorsTools.LoadBackground();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Render the bars
            if (blankAreas > 0)
                posIdx = 0;
            for (int y = 0; y < ConsoleWrapper.WindowHeight; y++)
                TextWriterWhereColor.WriteWhereColorBack(" ", 0, y, Color.Empty, ThemeColorsTools.GetColor(ThemeColorType.Background));
            for (int i = 0; i < count - blankAreas; i++)
            {
                int clearPosIdx = posIdx;
                posIdx++;
                if (posIdx >= currentPos.Count)
                    posIdx = 0;
                if (clearPosIdx >= currentPos.Count)
                    clearPosIdx = 0;
                int pos = currentPos[posIdx] + Math.Abs(currentPos.Min()) + 2;
                int posClear = currentPos[clearPosIdx] + Math.Abs(currentPos.Min()) + 2;
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    TextWriterWhereColor.WriteWhereColorBack(" ", blankAreas + i + 1, posClear, Color.Empty, ThemeColorsTools.GetColor(ThemeColorType.Background));
                    TextWriterWhereColor.WriteWhereColorBack(" ", blankAreas + i, pos, Color.Empty, waveColor);
                }
            }
            if (blankAreas == 0)
            {
                cycle++;
                if (cycle >= currentPos.Count)
                    cycle = 0;
            }
            posIdx = cycle;
            if (blankAreas > 0)
                blankAreas--;

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.WaveRunDelay);
        }
    }
}
