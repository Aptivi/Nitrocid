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
using System.Linq;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Colors;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display for Wave
    /// </summary>
    public class WaveDisplay : BaseScreensaver, IScreensaver
    {
        private int posIdx = 0;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Wave";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            posIdx = 0;
            ThemeColorsTools.LoadBackground();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // First, prepare how many dots to render according to the console size
            int Height = ConsoleWrapper.WindowHeight - 4;
            int Count = ConsoleWrapper.WindowWidth;

            // Then, go ahead and make these bars wave themselves.
            List<int> CurrentPos = [];
            double Frequency = Math.PI / ScreensaverPackInit.SaversConfig.WaveFrequencyLevel;

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
            int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WaveMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.WaveMaximumRedColorLevel);
            int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WaveMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.WaveMaximumGreenColorLevel);
            int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WaveMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.WaveMaximumBlueColorLevel);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
            var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            for (int i = 0; i < Count; i++)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                if (ScreensaverManager.Bailing)
                    return;

                posIdx++;
                if (posIdx >= CurrentPos.Count)
                    posIdx = 0;
                int Pos = CurrentPos[posIdx] + Math.Abs(CurrentPos.Min()) + 2;
                ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.WaveDelay);
                for (int j = 0; j < ConsoleWrapper.WindowHeight; j++)
                    TextWriterWhereColor.WriteWhereColorBack(" ", i, j, Color.Empty, ThemeColorsTools.GetColor(ThemeColorType.Background));
                TextWriterWhereColor.WriteWhereColorBack(" ", i, Pos, Color.Empty, ColorStorage);
            }
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.WaveDelay);
        }

    }
}
