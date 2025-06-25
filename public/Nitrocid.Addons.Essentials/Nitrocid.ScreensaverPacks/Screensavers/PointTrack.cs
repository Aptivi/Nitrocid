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
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Colors.Data;
using System.Text;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display for PointTrack
    /// </summary>
    public class PointTrackDisplay : BaseScreensaver, IScreensaver
    {
        private Color targetColor = ConsoleColors.Lime;
        private int posIdxVertical = 0;
        private int posIdxHorizontal = 0;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "PointTrack";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            posIdxVertical = 0;
            posIdxHorizontal = 0;

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
            ConsoleWrapper.CursorVisible = false;
            ThemeColorsTools.LoadBackground();

            // First, prepare how many dots to render according to the console size
            int Height = ConsoleWrapper.WindowHeight - 4;
            int Width = ConsoleWrapper.WindowWidth - 4;

            // Then, go ahead and make these bars swivel themselves.
            List<int> CurrentPosVertical = [];
            List<int> CurrentPosHorizontal = [];
            double FrequencyVertical = Math.PI / ScreensaverPackInit.SaversConfig.PointTrackVerticalFrequencyLevel;
            double FrequencyHorizontal = Math.PI / ScreensaverPackInit.SaversConfig.PointTrackHorizontalFrequencyLevel;

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

            // Render the block and the position rulers
            posIdxVertical++;
            if (posIdxVertical >= CurrentPosVertical.Count)
                posIdxVertical = 0;
            posIdxHorizontal++;
            if (posIdxHorizontal >= CurrentPosHorizontal.Count)
                posIdxHorizontal = 0;
            int PosVertical = CurrentPosVertical[posIdxVertical] + Math.Abs(CurrentPosVertical.Min()) + 2;
            int PosHorizontal = CurrentPosHorizontal[posIdxHorizontal] + Math.Abs(CurrentPosHorizontal.Min()) + 2;
            if (!ConsoleResizeHandler.WasResized(false))
            {
                // Render the block
                TextWriterWhereColor.WriteWhereColorBack(" ", PosHorizontal, PosVertical, Color.Empty, targetColor);

                // Now, make the position ruler writers
                var lineBuilder = new StringBuilder();
                var topRuler = new Line()
                {
                    DoubleWidth = false,
                    StartPos = new(PosHorizontal, 0),
                    EndPos = new(PosHorizontal, PosVertical - 4),
                    Color = ConsoleColors.Lime,
                };
                var bottomRuler = new Line()
                {
                    DoubleWidth = false,
                    StartPos = new(PosHorizontal, PosVertical + 2),
                    EndPos = new(PosHorizontal, ConsoleWrapper.WindowHeight - 1),
                    Color = ConsoleColors.Lime,
                };
                var leftRuler = new Line()
                {
                    DoubleWidth = false,
                    StartPos = new(0, PosVertical - 1),
                    EndPos = new(PosHorizontal - 4, PosVertical - 1),
                    Color = ConsoleColors.Lime,
                };
                var rightRuler = new Line()
                {
                    DoubleWidth = false,
                    StartPos = new(PosHorizontal + 4, PosVertical - 1),
                    EndPos = new(ConsoleWrapper.WindowWidth - 1, PosVertical - 1),
                    Color = ConsoleColors.Lime,
                };

                // Check the ruler states
                if (topRuler.EndPos.Y >= topRuler.StartPos.Y)
                    lineBuilder.Append(topRuler.Render());
                if (bottomRuler.EndPos.Y >= bottomRuler.StartPos.Y)
                    lineBuilder.Append(bottomRuler.Render());
                if (leftRuler.EndPos.X >= leftRuler.StartPos.X)
                    lineBuilder.Append(leftRuler.Render());
                if (rightRuler.EndPos.X >= rightRuler.StartPos.X)
                    lineBuilder.Append(rightRuler.Render());

                // Render the bars
                TextWriterRaw.WriteRaw(lineBuilder.ToString());
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.PointTrackDelay);
        }

    }
}
