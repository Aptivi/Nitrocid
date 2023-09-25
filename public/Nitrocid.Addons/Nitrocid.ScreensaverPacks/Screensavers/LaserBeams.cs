
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
using System.Text;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Sequences.Builder;

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
                return Config.SaverConfig.LaserBeamsTrueColor;
            }
            set
            {
                Config.SaverConfig.LaserBeamsTrueColor = value;
            }
        }
        /// <summary>
        /// [LaserBeams] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int LaserBeamsDelay
        {
            get
            {
                return Config.SaverConfig.LaserBeamsDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                Config.SaverConfig.LaserBeamsDelay = value;
            }
        }
        /// <summary>
        /// [LaserBeams] Line character
        /// </summary>
        public static string LaserBeamsLineChar
        {
            get
            {
                return Config.SaverConfig.LaserBeamsLineChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "-";
                Config.SaverConfig.LaserBeamsLineChar = value;
            }
        }
        /// <summary>
        /// [LaserBeams] Screensaver background color
        /// </summary>
        public static string LaserBeamsBackgroundColor
        {
            get
            {
                return Config.SaverConfig.LaserBeamsBackgroundColor;
            }
            set
            {
                Config.SaverConfig.LaserBeamsBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [LaserBeams] The minimum red color level (true color)
        /// </summary>
        public static int LaserBeamsMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.LaserBeamsMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.LaserBeamsMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The minimum green color level (true color)
        /// </summary>
        public static int LaserBeamsMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.LaserBeamsMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.LaserBeamsMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The minimum blue color level (true color)
        /// </summary>
        public static int LaserBeamsMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.LaserBeamsMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.LaserBeamsMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int LaserBeamsMinimumColorLevel
        {
            get
            {
                return Config.SaverConfig.LaserBeamsMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.LaserBeamsMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The maximum red color level (true color)
        /// </summary>
        public static int LaserBeamsMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.LaserBeamsMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.LaserBeamsMinimumRedColorLevel)
                    value = Config.SaverConfig.LaserBeamsMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.LaserBeamsMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The maximum green color level (true color)
        /// </summary>
        public static int LaserBeamsMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.LaserBeamsMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.LaserBeamsMinimumGreenColorLevel)
                    value = Config.SaverConfig.LaserBeamsMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.LaserBeamsMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The maximum blue color level (true color)
        /// </summary>
        public static int LaserBeamsMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.LaserBeamsMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.LaserBeamsMinimumBlueColorLevel)
                    value = Config.SaverConfig.LaserBeamsMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.LaserBeamsMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int LaserBeamsMaximumColorLevel
        {
            get
            {
                return Config.SaverConfig.LaserBeamsMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.LaserBeamsMinimumColorLevel)
                    value = Config.SaverConfig.LaserBeamsMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.LaserBeamsMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for LaserBeams
    /// </summary>
    public class LaserBeamsDisplay : BaseScreensaver, IScreensaver
    {

        private static readonly List<(int, int)> laserEnds = new();

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "LaserBeams";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            KernelColorTools.LoadBack(new Color(LaserBeamsSettings.LaserBeamsBackgroundColor));

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
            int laserStartPosX = Console.WindowWidth - 1;
            int laserStartPosY = Console.WindowHeight - 1;
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
                for (int j = 0; j < ConsoleWrapper.WindowWidth; j++)
                {
                    int currentPosX = (int)(laserStartPosX - (laserPosThresholdX * j));
                    int currentPosY = (int)(laserStartPosY - (laserPosThresholdY * j));
                    laserBeamsBuilder.Append($"{colorStorage.VTSequenceBackground}{VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCursorPosition, currentPosX + 1, currentPosY + 1)} ");
                }
            }

            // Make a beaming white particle to make it as if the lasers are fired
            laserBeamsBuilder.Append($"{new Color(ConsoleColors.White).VTSequenceBackground}{VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCursorPosition, ConsoleWrapper.WindowWidth + 1, ConsoleWrapper.WindowHeight + 1)} ");

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
