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
        private bool rampTrueColor = true;
        private int rampDelay = 20;
        private int rampNextRampDelay = 250;
        private char rampUpperLeftCornerChar = '╭';
        private char rampUpperRightCornerChar = '╮';
        private char rampLowerLeftCornerChar = '╰';
        private char rampLowerRightCornerChar = '╯';
        private char rampUpperFrameChar = '─';
        private char rampLowerFrameChar = '─';
        private char rampLeftFrameChar = '│';
        private char rampRightFrameChar = '│';
        private int rampMinimumRedColorLevelStart = 0;
        private int rampMinimumGreenColorLevelStart = 0;
        private int rampMinimumBlueColorLevelStart = 0;
        private int rampMinimumColorLevelStart = 0;
        private int rampMaximumRedColorLevelStart = 255;
        private int rampMaximumGreenColorLevelStart = 255;
        private int rampMaximumBlueColorLevelStart = 255;
        private int rampMaximumColorLevelStart = 255;
        private int rampMinimumRedColorLevelEnd = 0;
        private int rampMinimumGreenColorLevelEnd = 0;
        private int rampMinimumBlueColorLevelEnd = 0;
        private int rampMinimumColorLevelEnd = 0;
        private int rampMaximumRedColorLevelEnd = 255;
        private int rampMaximumGreenColorLevelEnd = 255;
        private int rampMaximumBlueColorLevelEnd = 255;
        private int rampMaximumColorLevelEnd = 255;
        private string rampUpperLeftCornerColor = "7";
        private string rampUpperRightCornerColor = "7";
        private string rampLowerLeftCornerColor = "7";
        private string rampLowerRightCornerColor = "7";
        private string rampUpperFrameColor = "7";
        private string rampLowerFrameColor = "7";
        private string rampLeftFrameColor = "7";
        private string rampRightFrameColor = "7";
        private bool rampUseBorderColors;

        /// <summary>
        /// [Ramp] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool RampTrueColor
        {
            get => rampTrueColor;
            set => rampTrueColor = value;
        }
        /// <summary>
        /// [Ramp] How many milliseconds to wait before making the next write?
        /// </summary>
        public int RampDelay
        {
            get => rampDelay;
            set
            {
                if (value <= 0)
                    value = 20;
                rampDelay = value;
            }
        }
        /// <summary>
        /// [Ramp] How many milliseconds to wait before starting the next ramp?
        /// </summary>
        public int RampNextRampDelay
        {
            get => rampNextRampDelay;
            set
            {
                if (value <= 0)
                    value = 250;
                rampNextRampDelay = value;
            }
        }
        /// <summary>
        /// [Ramp] Upper left corner character 
        /// </summary>
        public char RampUpperLeftCornerChar
        {
            get => rampUpperLeftCornerChar;
            set => rampUpperLeftCornerChar = value;
        }
        /// <summary>
        /// [Ramp] Upper right corner character 
        /// </summary>
        public char RampUpperRightCornerChar
        {
            get => rampUpperRightCornerChar;
            set => rampUpperRightCornerChar = value;
        }
        /// <summary>
        /// [Ramp] Lower left corner character 
        /// </summary>
        public char RampLowerLeftCornerChar
        {
            get => rampLowerLeftCornerChar;
            set => rampLowerLeftCornerChar = value;
        }
        /// <summary>
        /// [Ramp] Lower right corner character 
        /// </summary>
        public char RampLowerRightCornerChar
        {
            get => rampLowerRightCornerChar;
            set => rampLowerRightCornerChar = value;
        }
        /// <summary>
        /// [Ramp] Upper frame character 
        /// </summary>
        public char RampUpperFrameChar
        {
            get => rampUpperFrameChar;
            set => rampUpperFrameChar = value;
        }
        /// <summary>
        /// [Ramp] Lower frame character 
        /// </summary>
        public char RampLowerFrameChar
        {
            get => rampLowerFrameChar;
            set => rampLowerFrameChar = value;
        }
        /// <summary>
        /// [Ramp] Left frame character 
        /// </summary>
        public char RampLeftFrameChar
        {
            get => rampLeftFrameChar;
            set => rampLeftFrameChar = value;
        }
        /// <summary>
        /// [Ramp] Right frame character 
        /// </summary>
        public char RampRightFrameChar
        {
            get => rampRightFrameChar;
            set => rampRightFrameChar = value;
        }
        /// <summary>
        /// [Ramp] The minimum red color level (true color - start)
        /// </summary>
        public int RampMinimumRedColorLevelStart
        {
            get => rampMinimumRedColorLevelStart;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                rampMinimumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum green color level (true color - start)
        /// </summary>
        public int RampMinimumGreenColorLevelStart
        {
            get => rampMinimumGreenColorLevelStart;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                rampMinimumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum blue color level (true color - start)
        /// </summary>
        public int RampMinimumBlueColorLevelStart
        {
            get => rampMinimumBlueColorLevelStart;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                rampMinimumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum color level (255 colors or 16 colors - start)
        /// </summary>
        public int RampMinimumColorLevelStart
        {
            get => rampMinimumColorLevelStart;
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                rampMinimumColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum red color level (true color - start)
        /// </summary>
        public int RampMaximumRedColorLevelStart
        {
            get => rampMaximumRedColorLevelStart;
            set
            {
                if (value <= rampMinimumRedColorLevelStart)
                    value = rampMinimumRedColorLevelStart;
                if (value > 255)
                    value = 255;
                rampMaximumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum green color level (true color - start)
        /// </summary>
        public int RampMaximumGreenColorLevelStart
        {
            get => rampMaximumGreenColorLevelStart;
            set
            {
                if (value <= rampMinimumGreenColorLevelStart)
                    value = rampMinimumGreenColorLevelStart;
                if (value > 255)
                    value = 255;
                rampMaximumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum blue color level (true color - start)
        /// </summary>
        public int RampMaximumBlueColorLevelStart
        {
            get => rampMaximumBlueColorLevelStart;
            set
            {
                if (value <= rampMinimumBlueColorLevelStart)
                    value = rampMinimumBlueColorLevelStart;
                if (value > 255)
                    value = 255;
                rampMaximumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum color level (255 colors or 16 colors - start)
        /// </summary>
        public int RampMaximumColorLevelStart
        {
            get => rampMaximumColorLevelStart;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= rampMinimumColorLevelStart)
                    value = rampMinimumColorLevelStart;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                rampMaximumColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum red color level (true color - end)
        /// </summary>
        public int RampMinimumRedColorLevelEnd
        {
            get => rampMinimumRedColorLevelEnd;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                rampMinimumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum green color level (true color - end)
        /// </summary>
        public int RampMinimumGreenColorLevelEnd
        {
            get => rampMinimumGreenColorLevelEnd;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                rampMinimumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum blue color level (true color - end)
        /// </summary>
        public int RampMinimumBlueColorLevelEnd
        {
            get => rampMinimumBlueColorLevelEnd;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                rampMinimumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum color level (255 colors or 16 colors - end)
        /// </summary>
        public int RampMinimumColorLevelEnd
        {
            get => rampMinimumColorLevelEnd;
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                rampMinimumColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum red color level (true color - end)
        /// </summary>
        public int RampMaximumRedColorLevelEnd
        {
            get => rampMaximumRedColorLevelEnd;
            set
            {
                if (value <= rampMinimumRedColorLevelEnd)
                    value = rampMinimumRedColorLevelEnd;
                if (value > 255)
                    value = 255;
                rampMaximumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum green color level (true color - end)
        /// </summary>
        public int RampMaximumGreenColorLevelEnd
        {
            get => rampMaximumGreenColorLevelEnd;
            set
            {
                if (value <= rampMinimumGreenColorLevelEnd)
                    value = rampMinimumGreenColorLevelEnd;
                if (value > 255)
                    value = 255;
                rampMaximumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum blue color level (true color - end)
        /// </summary>
        public int RampMaximumBlueColorLevelEnd
        {
            get => rampMaximumBlueColorLevelEnd;
            set
            {
                if (value <= rampMinimumBlueColorLevelEnd)
                    value = rampMinimumBlueColorLevelEnd;
                if (value > 255)
                    value = 255;
                rampMaximumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum color level (255 colors or 16 colors - end)
        /// </summary>
        public int RampMaximumColorLevelEnd
        {
            get => rampMaximumColorLevelEnd;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= rampMinimumColorLevelEnd)
                    value = rampMinimumColorLevelEnd;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                rampMaximumColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] Upper left corner color.
        /// </summary>
        public string RampUpperLeftCornerColor
        {
            get => rampUpperLeftCornerColor;
            set => rampUpperLeftCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Upper right corner color.
        /// </summary>
        public string RampUpperRightCornerColor
        {
            get => rampUpperRightCornerColor;
            set => rampUpperRightCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Lower left corner color.
        /// </summary>
        public string RampLowerLeftCornerColor
        {
            get => rampLowerLeftCornerColor;
            set => rampLowerLeftCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Lower right corner color.
        /// </summary>
        public string RampLowerRightCornerColor
        {
            get => rampLowerRightCornerColor;
            set => rampLowerRightCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Upper frame color.
        /// </summary>
        public string RampUpperFrameColor
        {
            get => rampUpperFrameColor;
            set => rampUpperFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Lower frame color.
        /// </summary>
        public string RampLowerFrameColor
        {
            get => rampLowerFrameColor;
            set => rampLowerFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Left frame color.
        /// </summary>
        public string RampLeftFrameColor
        {
            get => rampLeftFrameColor;
            set => rampLeftFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Right frame color.
        /// </summary>
        public string RampRightFrameColor
        {
            get => rampRightFrameColor;
            set => rampRightFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Use the border colors.
        /// </summary>
        public bool RampUseBorderColors
        {
            get => rampUseBorderColors;
            set => rampUseBorderColors = value;
        }
    }
}
