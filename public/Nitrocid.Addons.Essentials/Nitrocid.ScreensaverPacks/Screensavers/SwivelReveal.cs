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
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Themes.Colors;
using Terminaux.Base.Structures;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display for SwivelReveal
    /// </summary>
    public class SwivelRevealDisplay : BaseScreensaver, IScreensaver
    {
        private Color targetColor = ConsoleColors.Lime;
        private List<int> currentPosVertical = [];
        private List<int> currentPosHorizontal = [];
        private Coordinate clearCoords = new(-1, -1);

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "SwivelReveal";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            ThemeColorsTools.LoadBackground();
            currentPosVertical.Clear();
            currentPosHorizontal.Clear();

            // Make an initial color storage
            int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.TrailsMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.TrailsMaximumRedColorLevel);
            int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.TrailsMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.TrailsMaximumGreenColorLevel);
            int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.TrailsMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.TrailsMaximumBlueColorLevel);
            targetColor = new(RedColorNum, GreenColorNum, BlueColorNum);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // First, prepare how many dots to render according to the console size
            int Height = ConsoleWrapper.WindowHeight - 4;
            int Width = ConsoleWrapper.WindowWidth - 4;

            // Then, go ahead and make these bars swivel themselves.
            double FrequencyVertical = Math.PI / ScreensaverPackInit.SaversConfig.SwivelRevealVerticalFrequencyLevel;
            double FrequencyHorizontal = Math.PI / ScreensaverPackInit.SaversConfig.SwivelRevealHorizontalFrequencyLevel;

            // Set the current vertical positions
            double TimeSecsVertical = 0.0;
            bool isSetVertical = false;
            while (true)
            {
                TimeSecsVertical += 0.1;
                double calculatedHeight = Height * Math.Cos(FrequencyVertical * TimeSecsVertical) / 2;
                currentPosVertical.Add((int)calculatedHeight);
                if ((int)calculatedHeight == Height / 2 && isSetVertical)
                    break;
                if (!isSetVertical && (int)calculatedHeight < Height / 2)
                    isSetVertical = true;
            }

            // Set the current horizontal positions
            double TimeSecsHorizontal = 0.0;
            bool isSetHorizontal = false;
            while (true)
            {
                TimeSecsHorizontal += 0.1;
                double calculatedWidth = Width * Math.Cos(FrequencyHorizontal * TimeSecsHorizontal) / 2;
                currentPosHorizontal.Add((int)calculatedWidth);
                if ((int)calculatedWidth == Width / 2 && isSetHorizontal)
                    break;
                if (!isSetHorizontal && (int)calculatedWidth < Width / 2)
                    isSetHorizontal = true;
            }

            // Render the bars
            int currPosVertical = currentPosVertical.FirstOrDefault(0);
            int currPosHorizontal = currentPosHorizontal.FirstOrDefault(0);
            int PosVertical = currPosVertical + Math.Abs(currentPosVertical.Min()) + 2;
            int PosHorizontal = currPosHorizontal + Math.Abs(currentPosHorizontal.Min()) + 2;
            currentPosVertical.RemoveAt(0);
            currentPosHorizontal.RemoveAt(0);
            if (!ConsoleResizeHandler.WasResized(false))
            {
                if (clearCoords.X != -1 && clearCoords.Y != -1)
                    TextWriterWhereColor.WriteWhereColorBack(" ", clearCoords.X, clearCoords.Y, Color.Empty, ThemeColorsTools.GetColor(ThemeColorType.Background));
                TextWriterWhereColor.WriteWhereColorBack(" ", PosHorizontal, PosVertical, Color.Empty, targetColor);
            }
            clearCoords = new(PosHorizontal, PosVertical);
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.SwivelRevealDelay);
        }

    }
}
