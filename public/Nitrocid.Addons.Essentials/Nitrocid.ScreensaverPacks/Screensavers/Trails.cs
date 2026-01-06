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
using Terminaux.Base.Structures;
using Terminaux.Colors.Data;
using Terminaux.Colors.Themes.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display for Trails
    /// </summary>
    public class TrailsDisplay : BaseScreensaver, IScreensaver
    {
        private Color targetColor = ConsoleColors.Lime;
        private List<Coordinate> positions = [];
        private int posIdxVertical = 0;
        private int posIdxHorizontal = 0;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Trails";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            posIdxVertical = 0;
            posIdxHorizontal = 0;
            positions.Clear();

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
            ThemeColorsTools.LoadBackground();

            // First, prepare how many dots to render according to the console size
            int Height = ConsoleWrapper.WindowHeight - 4;
            int Width = ConsoleWrapper.WindowWidth - 4;

            // Then, go ahead and make these bars swivel themselves.
            List<int> CurrentPosVertical = [];
            List<int> CurrentPosHorizontal = [];
            double FrequencyVertical = Math.PI / ScreensaverPackInit.SaversConfig.TrailsVerticalFrequencyLevel;
            double FrequencyHorizontal = Math.PI / ScreensaverPackInit.SaversConfig.TrailsHorizontalFrequencyLevel;

            // Set the current vertical positions
            double TimeSecsVertical = 0.0;
            bool isSetVertical = false;
            while (true)
            {
                TimeSecsVertical += 0.1;
                double calculatedHeight = Height * Math.Cos(FrequencyVertical * TimeSecsVertical) / 2;
                CurrentPosVertical.Add((int)calculatedHeight);
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
                CurrentPosHorizontal.Add((int)calculatedWidth);
                if ((int)calculatedWidth == Width / 2 && isSetHorizontal)
                    break;
                if (!isSetHorizontal && (int)calculatedWidth < Width / 2)
                    isSetHorizontal = true;
            }

            // Increment position indexes
            posIdxVertical++;
            if (posIdxVertical >= CurrentPosVertical.Count)
                posIdxVertical = 0;
            posIdxHorizontal++;
            if (posIdxHorizontal >= CurrentPosHorizontal.Count)
                posIdxHorizontal = 0;

            // Get the position, store it, and write the trails
            int PosVertical = CurrentPosVertical[posIdxVertical] + Math.Abs(CurrentPosVertical.Min()) + 2;
            int PosHorizontal = CurrentPosHorizontal[posIdxHorizontal] + Math.Abs(CurrentPosHorizontal.Min()) + 2;
            (int r, int g, int b) = (targetColor.RGB.R, targetColor.RGB.G, targetColor.RGB.B);
            double thresholdRed = r / (double)ScreensaverPackInit.SaversConfig.TrailsTrailLength;
            double thresholdGreen = g / (double)ScreensaverPackInit.SaversConfig.TrailsTrailLength;
            double thresholdBlue = b / (double)ScreensaverPackInit.SaversConfig.TrailsTrailLength;
            positions.Add(new(PosHorizontal, PosVertical));
            if (positions.Count > ScreensaverPackInit.SaversConfig.TrailsTrailLength)
                positions.RemoveAt(0);
            for (int i = 0; i < positions.Count; i++)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                if (ScreensaverManager.Bailing)
                    return;

                Coordinate item = positions[i];
                int finalR = (int)(r - (thresholdRed * (positions.Count - i)));
                int finalG = (int)(g - (thresholdGreen * (positions.Count - i)));
                int finalB = (int)(b - (thresholdBlue * (positions.Count - i)));
                var ColorStorage = new Color(finalR, finalG, finalB);
                TextWriterWhereColor.WriteWhereColorBack(" ", item.X, item.Y, Color.Empty, ColorStorage);
            }
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.TrailsDelay);
        }

    }
}
