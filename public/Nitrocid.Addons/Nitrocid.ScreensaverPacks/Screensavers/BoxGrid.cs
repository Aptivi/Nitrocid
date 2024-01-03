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

using Nitrocid.ConsoleBase;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers.FancyWriters;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for BoxGrid
    /// </summary>
    public static class BoxGridSettings
    {

        /// <summary>
        /// [BoxGrid] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int BoxGridDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BoxGridDelay;
            }
            set
            {
                if (value <= 0)
                    value = 5000;
                ScreensaverPackInit.SaversConfig.BoxGridDelay = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The minimum red color level (true color)
        /// </summary>
        public static int BoxGridMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BoxGridMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BoxGridMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The minimum green color level (true color)
        /// </summary>
        public static int BoxGridMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BoxGridMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BoxGridMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The minimum blue color level (true color)
        /// </summary>
        public static int BoxGridMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BoxGridMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BoxGridMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The maximum red color level (true color)
        /// </summary>
        public static int BoxGridMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BoxGridMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.BoxGridMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.BoxGridMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BoxGridMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The maximum green color level (true color)
        /// </summary>
        public static int BoxGridMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BoxGridMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.BoxGridMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.BoxGridMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BoxGridMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The maximum blue color level (true color)
        /// </summary>
        public static int BoxGridMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BoxGridMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.BoxGridMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.BoxGridMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BoxGridMaximumBlueColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for BoxGrid
    /// </summary>
    public class BoxGridDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "BoxGrid";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Get how many boxes to write
            int boxWidthExterior = 4;
            int boxHeightExterior = 3;
            int boxWidth = boxWidthExterior - 2;
            int boxHeight = boxHeightExterior - 2;
            int boxColumns = 0;
            int boxRows = 0;
            for (int i = 0; i < ConsoleWrapper.WindowWidth - boxWidthExterior; i += boxWidthExterior + 1)
                boxColumns++;
            for (int i = 0; i < ConsoleWrapper.WindowHeight - boxHeightExterior + 1; i += boxHeightExterior)
                boxRows++;

            // Draw the boxes
            for (int i = 0; i < boxColumns; i++)
            {
                for (int j = 0; j < boxRows; j++)
                {
                    if (ConsoleResizeListener.WasResized(false))
                        break;
                    var color = KernelColorTools.GetRandomColor(ColorType.TrueColor);
                    BorderColor.WriteBorder(i * (boxWidthExterior + 1), j * boxHeightExterior, boxWidth, boxHeight, color);
                }
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(BoxGridSettings.BoxGridDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
