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

using Nitrocid.Base.Kernel.Configuration.Instances;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public partial class ExtraSaversConfig : BaseKernelConfig
    {
        private bool progressesTrueColor = true;
        private bool progressesCycleColors = true;
        private int progressesCycleColorsTicks = 20;
        private string progressesThirdProgressColor = "4";
        private string progressesSecondProgressColor = "5";
        private string progressesFirstProgressColor = "6";
        private string progressesProgressColor = "7";
        private int progressesDelay = 500;
        private char progressesUpperLeftCornerCharFirst = '╭';
        private char progressesUpperLeftCornerCharSecond = '╭';
        private char progressesUpperLeftCornerCharThird = '╭';
        private char progressesUpperRightCornerCharFirst = '╮';
        private char progressesUpperRightCornerCharSecond = '╮';
        private char progressesUpperRightCornerCharThird = '╮';
        private char progressesLowerLeftCornerCharFirst = '╰';
        private char progressesLowerLeftCornerCharSecond = '╰';
        private char progressesLowerLeftCornerCharThird = '╰';
        private char progressesLowerRightCornerCharFirst = '╯';
        private char progressesLowerRightCornerCharSecond = '╯';
        private char progressesLowerRightCornerCharThird = '╯';
        private char progressesUpperFrameCharFirst = '─';
        private char progressesUpperFrameCharSecond = '─';
        private char progressesUpperFrameCharThird = '─';
        private char progressesLowerFrameCharFirst = '─';
        private char progressesLowerFrameCharSecond = '─';
        private char progressesLowerFrameCharThird = '─';
        private char progressesLeftFrameCharFirst = '│';
        private char progressesLeftFrameCharSecond = '│';
        private char progressesLeftFrameCharThird = '│';
        private char progressesRightFrameCharFirst = '│';
        private char progressesRightFrameCharSecond = '│';
        private char progressesRightFrameCharThird = '│';
        private string progressesInfoTextFirst = "";
        private string progressesInfoTextSecond = "";
        private string progressesInfoTextThird = "";
        private int progressesMinimumRedColorLevelFirst = 0;
        private int progressesMinimumGreenColorLevelFirst = 0;
        private int progressesMinimumBlueColorLevelFirst = 0;
        private int progressesMinimumColorLevelFirst = 1;
        private int progressesMaximumRedColorLevelFirst = 255;
        private int progressesMaximumGreenColorLevelFirst = 255;
        private int progressesMaximumBlueColorLevelFirst = 255;
        private int progressesMaximumColorLevelFirst = 255;
        private int progressesMinimumRedColorLevelSecond = 0;
        private int progressesMinimumGreenColorLevelSecond = 0;
        private int progressesMinimumBlueColorLevelSecond = 0;
        private int progressesMinimumColorLevelSecond = 1;
        private int progressesMaximumRedColorLevelSecond = 255;
        private int progressesMaximumGreenColorLevelSecond = 255;
        private int progressesMaximumBlueColorLevelSecond = 255;
        private int progressesMaximumColorLevelSecond = 255;
        private int progressesMinimumRedColorLevelThird = 0;
        private int progressesMinimumGreenColorLevelThird = 0;
        private int progressesMinimumBlueColorLevelThird = 0;
        private int progressesMinimumColorLevelThird = 1;
        private int progressesMaximumRedColorLevelThird = 255;
        private int progressesMaximumGreenColorLevelThird = 255;
        private int progressesMaximumBlueColorLevelThird = 255;
        private int progressesMaximumColorLevelThird = 255;
        private int progressesMinimumRedColorLevel = 0;
        private int progressesMinimumGreenColorLevel = 0;
        private int progressesMinimumBlueColorLevel = 0;
        private int progressesMinimumColorLevel = 1;
        private int progressesMaximumRedColorLevel = 255;
        private int progressesMaximumGreenColorLevel = 255;
        private int progressesMaximumBlueColorLevel = 255;
        private int progressesMaximumColorLevel = 255;

        /// <summary>
        /// [Progresses] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool ProgressesTrueColor
        {
            get => progressesTrueColor;
            set => progressesTrueColor = value;
        }
        /// <summary>
        /// [Progresses] Enable color cycling (uses RNG. If disabled, uses the <see cref="ProgressesThirdProgressColor"/>, <see cref="ProgressesSecondProgressColor"/>, and <see cref="ProgressesFirstProgressColor"/> colors.)
        /// </summary>
        public bool ProgressesCycleColors
        {
            get => progressesCycleColors;
            set => progressesCycleColors = value;
        }
        /// <summary>
        /// [Progresses] The color of third progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string ProgressesThirdProgressColor
        {
            get => progressesThirdProgressColor;
            set => progressesThirdProgressColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Progresses] The color of second progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string ProgressesSecondProgressColor
        {
            get => progressesSecondProgressColor;
            set => progressesSecondProgressColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Progresses] The color of first progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string ProgressesFirstProgressColor
        {
            get => progressesFirstProgressColor;
            set => progressesFirstProgressColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Progresses] The color of date information. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string ProgressesProgressColor
        {
            get => progressesProgressColor;
            set => progressesProgressColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Progresses] If color cycling is enabled, how many ticks before changing colors? 1 tick = 0.5 third
        /// </summary>
        public long ProgressesCycleColorsTicks
        {
            get => progressesCycleColorsTicks;
            set
            {
                if (value <= 0L)
                    value = 20L;
                progressesCycleColorsTicks = (int)value;
            }
        }
        /// <summary>
        /// [Progresses] How many milliseconds to wait before making the next write?
        /// </summary>
        public int ProgressesDelay
        {
            get => progressesDelay;
            set
            {
                if (value <= 0)
                    value = 500;
                progressesDelay = value;
            }
        }
        /// <summary>
        /// [Progresses] Upper left corner character for the first bar
        /// </summary>
        public char ProgressesUpperLeftCornerCharFirst
        {
            get => progressesUpperLeftCornerCharFirst;
            set => progressesUpperLeftCornerCharFirst = value;
        }
        /// <summary>
        /// [Progresses] Upper left corner character for the second bar
        /// </summary>
        public char ProgressesUpperLeftCornerCharSecond
        {
            get => progressesUpperLeftCornerCharSecond;
            set => progressesUpperLeftCornerCharSecond = value;
        }
        /// <summary>
        /// [Progresses] Upper left corner character for the third bar
        /// </summary>
        public char ProgressesUpperLeftCornerCharThird
        {
            get => progressesUpperLeftCornerCharThird;
            set => progressesUpperLeftCornerCharThird = value;
        }
        /// <summary>
        /// [Progresses] Upper right corner character for the first bar
        /// </summary>
        public char ProgressesUpperRightCornerCharFirst
        {
            get => progressesUpperRightCornerCharFirst;
            set => progressesUpperRightCornerCharFirst = value;
        }
        /// <summary>
        /// [Progresses] Upper right corner character for the second bar
        /// </summary>
        public char ProgressesUpperRightCornerCharSecond
        {
            get => progressesUpperRightCornerCharSecond;
            set => progressesUpperRightCornerCharSecond = value;
        }
        /// <summary>
        /// [Progresses] Upper right corner character for the third bar
        /// </summary>
        public char ProgressesUpperRightCornerCharThird
        {
            get => progressesUpperRightCornerCharThird;
            set => progressesUpperRightCornerCharThird = value;
        }
        /// <summary>
        /// [Progresses] Lower left corner character for the first bar
        /// </summary>
        public char ProgressesLowerLeftCornerCharFirst
        {
            get => progressesLowerLeftCornerCharFirst;
            set => progressesLowerLeftCornerCharFirst = value;
        }
        /// <summary>
        /// [Progresses] Lower left corner character for the second bar
        /// </summary>
        public char ProgressesLowerLeftCornerCharSecond
        {
            get => progressesLowerLeftCornerCharSecond;
            set => progressesLowerLeftCornerCharSecond = value;
        }
        /// <summary>
        /// [Progresses] Lower left corner character for the third bar
        /// </summary>
        public char ProgressesLowerLeftCornerCharThird
        {
            get => progressesLowerLeftCornerCharThird;
            set => progressesLowerLeftCornerCharThird = value;
        }
        /// <summary>
        /// [Progresses] Lower right corner character for the first bar
        /// </summary>
        public char ProgressesLowerRightCornerCharFirst
        {
            get => progressesLowerRightCornerCharFirst;
            set => progressesLowerRightCornerCharFirst = value;
        }
        /// <summary>
        /// [Progresses] Lower right corner character for the second bar
        /// </summary>
        public char ProgressesLowerRightCornerCharSecond
        {
            get => progressesLowerRightCornerCharSecond;
            set => progressesLowerRightCornerCharSecond = value;
        }
        /// <summary>
        /// [Progresses] Lower right corner character for the third bar
        /// </summary>
        public char ProgressesLowerRightCornerCharThird
        {
            get => progressesLowerRightCornerCharThird;
            set => progressesLowerRightCornerCharThird = value;
        }
        /// <summary>
        /// [Progresses] Upper frame character for the first bar
        /// </summary>
        public char ProgressesUpperFrameCharFirst
        {
            get => progressesUpperFrameCharFirst;
            set => progressesUpperFrameCharFirst = value;
        }
        /// <summary>
        /// [Progresses] Upper frame character for the second bar
        /// </summary>
        public char ProgressesUpperFrameCharSecond
        {
            get => progressesUpperFrameCharSecond;
            set => progressesUpperFrameCharSecond = value;
        }
        /// <summary>
        /// [Progresses] Upper frame character for the third bar
        /// </summary>
        public char ProgressesUpperFrameCharThird
        {
            get => progressesUpperFrameCharThird;
            set => progressesUpperFrameCharThird = value;
        }
        /// <summary>
        /// [Progresses] Lower frame character for the first bar
        /// </summary>
        public char ProgressesLowerFrameCharFirst
        {
            get => progressesLowerFrameCharFirst;
            set => progressesLowerFrameCharFirst = value;
        }
        /// <summary>
        /// [Progresses] Lower frame character for the second bar
        /// </summary>
        public char ProgressesLowerFrameCharSecond
        {
            get => progressesLowerFrameCharSecond;
            set => progressesLowerFrameCharSecond = value;
        }
        /// <summary>
        /// [Progresses] Lower frame character for the third bar
        /// </summary>
        public char ProgressesLowerFrameCharThird
        {
            get => progressesLowerFrameCharThird;
            set => progressesLowerFrameCharThird = value;
        }
        /// <summary>
        /// [Progresses] Left frame character for the first bar
        /// </summary>
        public char ProgressesLeftFrameCharFirst
        {
            get => progressesLeftFrameCharFirst;
            set => progressesLeftFrameCharFirst = value;
        }
        /// <summary>
        /// [Progresses] Left frame character for the second bar
        /// </summary>
        public char ProgressesLeftFrameCharSecond
        {
            get => progressesLeftFrameCharSecond;
            set => progressesLeftFrameCharSecond = value;
        }
        /// <summary>
        /// [Progresses] Left frame character for the third bar
        /// </summary>
        public char ProgressesLeftFrameCharThird
        {
            get => progressesLeftFrameCharThird;
            set => progressesLeftFrameCharThird = value;
        }
        /// <summary>
        /// [Progresses] Right frame character for the first bar
        /// </summary>
        public char ProgressesRightFrameCharFirst
        {
            get => progressesRightFrameCharFirst;
            set => progressesRightFrameCharFirst = value;
        }
        /// <summary>
        /// [Progresses] Right frame character for the second bar
        /// </summary>
        public char ProgressesRightFrameCharSecond
        {
            get => progressesRightFrameCharSecond;
            set => progressesRightFrameCharSecond = value;
        }
        /// <summary>
        /// [Progresses] Right frame character for the third bar
        /// </summary>
        public char ProgressesRightFrameCharThird
        {
            get => progressesRightFrameCharThird;
            set => progressesRightFrameCharThird = value;
        }
        /// <summary>
        /// [Progresses] Information text for the first bar
        /// </summary>
        public string ProgressesInfoTextFirst
        {
            get => progressesInfoTextFirst;
            set => progressesInfoTextFirst = value;
        }
        /// <summary>
        /// [Progresses] Information text for the second bar
        /// </summary>
        public string ProgressesInfoTextSecond
        {
            get => progressesInfoTextSecond;
            set => progressesInfoTextSecond = value;
        }
        /// <summary>
        /// [Progresses] Information text for the third bar
        /// </summary>
        public string ProgressesInfoTextThird
        {
            get => progressesInfoTextThird;
            set => progressesInfoTextThird = value;
        }
        /// <summary>
        /// [Progresses] The minimum red color level (true color - first)
        /// </summary>
        public int ProgressesMinimumRedColorLevelFirst
        {
            get => progressesMinimumRedColorLevelFirst;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressesMinimumRedColorLevelFirst = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum green color level (true color - first)
        /// </summary>
        public int ProgressesMinimumGreenColorLevelFirst
        {
            get => progressesMinimumGreenColorLevelFirst;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressesMinimumGreenColorLevelFirst = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum blue color level (true color - first)
        /// </summary>
        public int ProgressesMinimumBlueColorLevelFirst
        {
            get => progressesMinimumBlueColorLevelFirst;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressesMinimumBlueColorLevelFirst = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum color level (255 colors or 16 colors - first)
        /// </summary>
        public int ProgressesMinimumColorLevelFirst
        {
            get => progressesMinimumColorLevelFirst;
            set
            {
                int FinalMinimumLevel = 255;
                if (value < 0)
                    value = 1;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                progressesMinimumColorLevelFirst = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum red color level (true color - first)
        /// </summary>
        public int ProgressesMaximumRedColorLevelFirst
        {
            get => progressesMaximumRedColorLevelFirst;
            set
            {
                if (value <= progressesMinimumRedColorLevelFirst)
                    value = progressesMinimumRedColorLevelFirst;
                if (value > 255)
                    value = 255;
                progressesMaximumRedColorLevelFirst = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum green color level (true color - first)
        /// </summary>
        public int ProgressesMaximumGreenColorLevelFirst
        {
            get => progressesMaximumGreenColorLevelFirst;
            set
            {
                if (value <= progressesMinimumGreenColorLevelFirst)
                    value = progressesMinimumGreenColorLevelFirst;
                if (value > 255)
                    value = 255;
                progressesMaximumGreenColorLevelFirst = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum blue color level (true color - first)
        /// </summary>
        public int ProgressesMaximumBlueColorLevelFirst
        {
            get => progressesMaximumBlueColorLevelFirst;
            set
            {
                if (value <= progressesMinimumBlueColorLevelFirst)
                    value = progressesMinimumBlueColorLevelFirst;
                if (value > 255)
                    value = 255;
                progressesMaximumBlueColorLevelFirst = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum color level (255 colors or 16 colors - first)
        /// </summary>
        public int ProgressesMaximumColorLevelFirst
        {
            get => progressesMaximumColorLevelFirst;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= progressesMinimumColorLevelFirst)
                    value = progressesMinimumColorLevelFirst;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                progressesMaximumColorLevelFirst = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum red color level (true color - second)
        /// </summary>
        public int ProgressesMinimumRedColorLevelSecond
        {
            get => progressesMinimumRedColorLevelSecond;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressesMinimumRedColorLevelSecond = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum green color level (true color - second)
        /// </summary>
        public int ProgressesMinimumGreenColorLevelSecond
        {
            get => progressesMinimumGreenColorLevelSecond;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressesMinimumGreenColorLevelSecond = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum blue color level (true color - second)
        /// </summary>
        public int ProgressesMinimumBlueColorLevelSecond
        {
            get => progressesMinimumBlueColorLevelSecond;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressesMinimumBlueColorLevelSecond = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum color level (255 colors or 16 colors - second)
        /// </summary>
        public int ProgressesMinimumColorLevelSecond
        {
            get => progressesMinimumColorLevelSecond;
            set
            {
                int FinalMinimumLevel = 255;
                if (value < 0)
                    value = 1;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                progressesMinimumColorLevelSecond = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum red color level (true color - second)
        /// </summary>
        public int ProgressesMaximumRedColorLevelSecond
        {
            get => progressesMaximumRedColorLevelSecond;
            set
            {
                if (value <= progressesMinimumRedColorLevelSecond)
                    value = progressesMinimumRedColorLevelSecond;
                if (value > 255)
                    value = 255;
                progressesMaximumRedColorLevelSecond = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum green color level (true color - second)
        /// </summary>
        public int ProgressesMaximumGreenColorLevelSecond
        {
            get => progressesMaximumGreenColorLevelSecond;
            set
            {
                if (value <= progressesMinimumGreenColorLevelSecond)
                    value = progressesMinimumGreenColorLevelSecond;
                if (value > 255)
                    value = 255;
                progressesMaximumGreenColorLevelSecond = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum blue color level (true color - second)
        /// </summary>
        public int ProgressesMaximumBlueColorLevelSecond
        {
            get => progressesMaximumBlueColorLevelSecond;
            set
            {
                if (value <= progressesMinimumBlueColorLevelSecond)
                    value = progressesMinimumBlueColorLevelSecond;
                if (value > 255)
                    value = 255;
                progressesMaximumBlueColorLevelSecond = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum color level (255 colors or 16 colors - second)
        /// </summary>
        public int ProgressesMaximumColorLevelSecond
        {
            get => progressesMaximumColorLevelSecond;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= progressesMinimumColorLevelSecond)
                    value = progressesMinimumColorLevelSecond;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                progressesMaximumColorLevelSecond = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum red color level (true color - third)
        /// </summary>
        public int ProgressesMinimumRedColorLevelThird
        {
            get => progressesMinimumRedColorLevelThird;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressesMinimumRedColorLevelThird = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum green color level (true color - third)
        /// </summary>
        public int ProgressesMinimumGreenColorLevelThird
        {
            get => progressesMinimumGreenColorLevelThird;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressesMinimumGreenColorLevelThird = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum blue color level (true color - third)
        /// </summary>
        public int ProgressesMinimumBlueColorLevelThird
        {
            get => progressesMinimumBlueColorLevelThird;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressesMinimumBlueColorLevelThird = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum color level (255 colors or 16 colors - third)
        /// </summary>
        public int ProgressesMinimumColorLevelThird
        {
            get => progressesMinimumColorLevelThird;
            set
            {
                int FinalMinimumLevel = 255;
                if (value < 0)
                    value = 1;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                progressesMinimumColorLevelThird = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum red color level (true color - third)
        /// </summary>
        public int ProgressesMaximumRedColorLevelThird
        {
            get => progressesMaximumRedColorLevelThird;
            set
            {
                if (value <= progressesMinimumRedColorLevelThird)
                    value = progressesMinimumRedColorLevelThird;
                if (value > 255)
                    value = 255;
                progressesMaximumRedColorLevelThird = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum green color level (true color - third)
        /// </summary>
        public int ProgressesMaximumGreenColorLevelThird
        {
            get => progressesMaximumGreenColorLevelThird;
            set
            {
                if (value <= progressesMinimumGreenColorLevelThird)
                    value = progressesMinimumGreenColorLevelThird;
                if (value > 255)
                    value = 255;
                progressesMaximumGreenColorLevelThird = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum blue color level (true color - third)
        /// </summary>
        public int ProgressesMaximumBlueColorLevelThird
        {
            get => progressesMaximumBlueColorLevelThird;
            set
            {
                if (value <= progressesMinimumBlueColorLevelThird)
                    value = progressesMinimumBlueColorLevelThird;
                if (value > 255)
                    value = 255;
                progressesMaximumBlueColorLevelThird = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum color level (255 colors or 16 colors - third)
        /// </summary>
        public int ProgressesMaximumColorLevelThird
        {
            get => progressesMaximumColorLevelThird;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= progressesMinimumColorLevelThird)
                    value = progressesMinimumColorLevelThird;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                progressesMaximumColorLevelThird = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum red color level (true color)
        /// </summary>
        public int ProgressesMinimumRedColorLevel
        {
            get => progressesMinimumRedColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressesMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum green color level (true color)
        /// </summary>
        public int ProgressesMinimumGreenColorLevel
        {
            get => progressesMinimumGreenColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressesMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum blue color level (true color)
        /// </summary>
        public int ProgressesMinimumBlueColorLevel
        {
            get => progressesMinimumBlueColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressesMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Progresses] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int ProgressesMinimumColorLevel
        {
            get => progressesMinimumColorLevel;
            set
            {
                int FinalMinimumLevel = 255;
                if (value < 0)
                    value = 1;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                progressesMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum red color level (true color)
        /// </summary>
        public int ProgressesMaximumRedColorLevel
        {
            get => progressesMaximumRedColorLevel;
            set
            {
                if (value <= progressesMinimumRedColorLevel)
                    value = progressesMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                progressesMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum green color level (true color)
        /// </summary>
        public int ProgressesMaximumGreenColorLevel
        {
            get => progressesMaximumGreenColorLevel;
            set
            {
                if (value <= progressesMinimumGreenColorLevel)
                    value = progressesMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                progressesMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum blue color level (true color)
        /// </summary>
        public int ProgressesMaximumBlueColorLevel
        {
            get => progressesMaximumBlueColorLevel;
            set
            {
                if (value <= progressesMinimumBlueColorLevel)
                    value = progressesMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                progressesMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Progresses] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int ProgressesMaximumColorLevel
        {
            get => progressesMaximumColorLevel;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= progressesMinimumColorLevel)
                    value = progressesMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                progressesMaximumColorLevel = value;
            }
        }
    }
}
