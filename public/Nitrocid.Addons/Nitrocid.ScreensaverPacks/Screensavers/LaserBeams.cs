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

using System.Collections.Generic;
using System.Text;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;
using Textify.Sequences.Builder.Types;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for LaserBeams
    /// </summary>
    public static class LaserBeamsSettings
    {

        /// <summary>
        /// [LaserBeams] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool LaserBeamsTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LaserBeamsTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.LaserBeamsTrueColor = value;
            }
        }
        /// <summary>
        /// [LaserBeams] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int LaserBeamsDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LaserBeamsDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                ScreensaverPackInit.SaversConfig.LaserBeamsDelay = value;
            }
        }
        /// <summary>
        /// [LaserBeams] Line character
        /// </summary>
        public static string LaserBeamsLineChar
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LaserBeamsLineChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "-";
                ScreensaverPackInit.SaversConfig.LaserBeamsLineChar = value;
            }
        }
        /// <summary>
        /// [LaserBeams] Screensaver background color
        /// </summary>
        public static string LaserBeamsBackgroundColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LaserBeamsBackgroundColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.LaserBeamsBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [LaserBeams] The minimum red color level (true color)
        /// </summary>
        public static int LaserBeamsMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LaserBeamsMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.LaserBeamsMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The minimum green color level (true color)
        /// </summary>
        public static int LaserBeamsMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LaserBeamsMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.LaserBeamsMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The minimum blue color level (true color)
        /// </summary>
        public static int LaserBeamsMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LaserBeamsMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.LaserBeamsMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int LaserBeamsMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LaserBeamsMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.LaserBeamsMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The maximum red color level (true color)
        /// </summary>
        public static int LaserBeamsMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LaserBeamsMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.LaserBeamsMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.LaserBeamsMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.LaserBeamsMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The maximum green color level (true color)
        /// </summary>
        public static int LaserBeamsMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LaserBeamsMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.LaserBeamsMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.LaserBeamsMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.LaserBeamsMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The maximum blue color level (true color)
        /// </summary>
        public static int LaserBeamsMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LaserBeamsMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.LaserBeamsMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.LaserBeamsMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.LaserBeamsMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int LaserBeamsMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LaserBeamsMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.LaserBeamsMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.LaserBeamsMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.LaserBeamsMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for LaserBeams
    /// </summary>
    public class LaserBeamsDisplay : BaseScreensaver, IScreensaver
    {

        private static readonly List<(int, int)> laserEnds = [];

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "LaserBeams";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            laserEnds.Clear();
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            ColorTools.LoadBack(new Color(LaserBeamsSettings.LaserBeamsBackgroundColor));

            // Populate the laser ends
            laserEnds.AddRange(
                new[]
                {
                    (0, (ConsoleWrapper.WindowHeight * 5 / 5) - 1),
                    (0, (ConsoleWrapper.WindowHeight * 4 / 5) - 1),
                    (0, (ConsoleWrapper.WindowHeight * 3 / 5) - 1),
                    (0, (ConsoleWrapper.WindowHeight * 2 / 5) - 1),
                    (0, (ConsoleWrapper.WindowHeight / 5) - 1),
                    (0, 0),
                    ((ConsoleWrapper.WindowWidth / 5) - 1, 0),
                    ((ConsoleWrapper.WindowWidth * 2 / 5) - 1, 0),
                    ((ConsoleWrapper.WindowWidth * 3 / 5) - 1, 0),
                    ((ConsoleWrapper.WindowWidth * 4 / 5) - 1, 0),
                    ((ConsoleWrapper.WindowWidth * 5 / 5) - 1, 0),
                }
            );

            // Draw few beams
            int laserStartPosX = ConsoleWrapper.WindowWidth - 1;
            int laserStartPosY = ConsoleWrapper.WindowHeight - 1;
            StringBuilder laserBeamsBuilder = new();
            for (int i = 0; i < 11; i++)
            {
                // Select a color
                Color colorStorage;
                if (LaserBeamsSettings.LaserBeamsTrueColor)
                {
                    int RedColorNum = RandomDriver.Random(LaserBeamsSettings.LaserBeamsMinimumRedColorLevel, LaserBeamsSettings.LaserBeamsMaximumRedColorLevel);
                    int GreenColorNum = RandomDriver.Random(LaserBeamsSettings.LaserBeamsMinimumGreenColorLevel, LaserBeamsSettings.LaserBeamsMaximumGreenColorLevel);
                    int BlueColorNum = RandomDriver.Random(LaserBeamsSettings.LaserBeamsMinimumBlueColorLevel, LaserBeamsSettings.LaserBeamsMaximumBlueColorLevel);
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                    colorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                }
                else
                {
                    int color = RandomDriver.Random(LaserBeamsSettings.LaserBeamsMinimumColorLevel, LaserBeamsSettings.LaserBeamsMaximumColorLevel);
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", color);
                    colorStorage = new Color(color);
                }

                // Get the laser end positions
                int laserEndPosX = laserEnds[i].Item1;
                int laserEndPosY = laserEnds[i].Item2;

                // Now, draw a laser beam
                double laserPosThresholdX = -(laserEndPosX - laserStartPosX) / (double)ConsoleWrapper.WindowWidth;
                double laserPosThresholdY = -(laserEndPosY - laserStartPosY) / ((double)ConsoleWrapper.WindowHeight * 4);
                int currentPosX = laserStartPosX;
                int currentPosY = laserStartPosY;
                int step = 0;
                while (currentPosX > 0 && currentPosY > 0)
                {
                    currentPosX = (int)(laserStartPosX - (laserPosThresholdX * step));
                    currentPosY = (int)(laserStartPosY - (laserPosThresholdY * step));
                    laserBeamsBuilder.Append($"{colorStorage.VTSequenceBackground}{CsiSequences.GenerateCsiCursorPosition(currentPosX + 1, currentPosY + 1)} ");
                    step++;
                }
            }

            // Make a beaming white particle to make it as if the lasers are fired
            laserBeamsBuilder.Append($"{new Color(ConsoleColors.White).VTSequenceBackground}{CsiSequences.GenerateCsiCursorPosition(ConsoleWrapper.WindowWidth + 1, ConsoleWrapper.WindowHeight + 1)} ");

            // Write the result
            TextWriterColor.WritePlain(laserBeamsBuilder.ToString(), false);

            // Reset resize sync
            laserEnds.Clear();
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(LaserBeamsSettings.LaserBeamsDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro() =>
            laserEnds.Clear();

    }
}
