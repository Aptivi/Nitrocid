
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
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Threading;
using Terminaux.Colors;

namespace KS.Misc.Screensaver.Displays
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
                return Config.SaverConfig.BoxGridDelay;
            }
            set
            {
                if (value <= 0)
                    value = 5000;
                Config.SaverConfig.BoxGridDelay = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The minimum red color level (true color)
        /// </summary>
        public static int BoxGridMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.BoxGridMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BoxGridMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The minimum green color level (true color)
        /// </summary>
        public static int BoxGridMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.BoxGridMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BoxGridMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The minimum blue color level (true color)
        /// </summary>
        public static int BoxGridMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.BoxGridMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BoxGridMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The maximum red color level (true color)
        /// </summary>
        public static int BoxGridMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.BoxGridMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.BoxGridMinimumRedColorLevel)
                    value = Config.SaverConfig.BoxGridMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BoxGridMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The maximum green color level (true color)
        /// </summary>
        public static int BoxGridMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.BoxGridMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.BoxGridMinimumGreenColorLevel)
                    value = Config.SaverConfig.BoxGridMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BoxGridMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The maximum blue color level (true color)
        /// </summary>
        public static int BoxGridMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.BoxGridMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.BoxGridMinimumBlueColorLevel)
                    value = Config.SaverConfig.BoxGridMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BoxGridMaximumBlueColorLevel = value;
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
            for (int i = 0; i < Console.WindowWidth; i += boxWidthExterior + 1)
                boxColumns++;
            for (int i = 0; i < Console.WindowHeight; i += boxHeightExterior)
                boxRows++;

            // Draw the boxes
            for (int i = 0; i < boxColumns; i++)
            {
                for (int j = 0; j < boxRows; j++)
                {
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
