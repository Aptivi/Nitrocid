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
        private bool barRotTrueColor = true;
        private bool barRotUseBorderColors;
        private string barRotRightFrameColor = "192;192;192";
        private int barRotDelay = 10;
        private int barRotNextRampDelay = 250;
        private char barRotUpperLeftCornerChar = '╭';
        private char barRotUpperRightCornerChar = '╮';
        private char barRotLowerLeftCornerChar = '╰';
        private char barRotLowerRightCornerChar = '╯';
        private char barRotUpperFrameChar = '─';
        private char barRotLowerFrameChar = '─';
        private char barRotLeftFrameChar = '│';
        private char barRotRightFrameChar = '│';
        private int barRotMinimumRedColorLevelStart = 0;
        private int barRotMinimumGreenColorLevelStart = 0;
        private int barRotMinimumBlueColorLevelStart = 0;
        private int barRotMaximumRedColorLevelStart = 255;
        private int barRotMaximumGreenColorLevelStart = 255;
        private int barRotMaximumBlueColorLevelStart = 255;
        private int barRotMinimumRedColorLevelEnd = 0;
        private int barRotMinimumGreenColorLevelEnd = 0;
        private int barRotMinimumBlueColorLevelEnd = 0;
        private int barRotMaximumRedColorLevelEnd = 255;
        private int barRotMaximumGreenColorLevelEnd = 255;
        private int barRotMaximumBlueColorLevelEnd = 255;
        private string barRotUpperLeftCornerColor = "192;192;192";
        private string barRotUpperRightCornerColor = "192;192;192";
        private string barRotLowerLeftCornerColor = "192;192;192";
        private string barRotLowerRightCornerColor = "192;192;192";
        private string barRotUpperFrameColor = "192;192;192";
        private string barRotLowerFrameColor = "192;192;192";
        private string barRotLeftFrameColor = "192;192;192";

        /// <summary>
        /// [BarRot] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool BarRotTrueColor
        {
            get => barRotTrueColor;
            set => barRotTrueColor = value;
        }
        /// <summary>
        /// [BarRot] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BarRotDelay
        {
            get => barRotDelay;
            set
            {
                if (value <= 0)
                    value = 10;
                barRotDelay = value;
            }
        }
        /// <summary>
        /// [BarRot] How many milliseconds to wait before rotting the next ramp's one end?
        /// </summary>
        public int BarRotNextRampDelay
        {
            get => barRotNextRampDelay;
            set
            {
                if (value <= 0)
                    value = 250;
                barRotNextRampDelay = value;
            }
        }
        /// <summary>
        /// [BarRot] Upper left corner character 
        /// </summary>
        public char BarRotUpperLeftCornerChar
        {
            get => barRotUpperLeftCornerChar;
            set => barRotUpperLeftCornerChar = value;
        }
        /// <summary>
        /// [BarRot] Upper right corner character 
        /// </summary>
        public char BarRotUpperRightCornerChar
        {
            get => barRotUpperRightCornerChar;
            set => barRotUpperRightCornerChar = value;
        }
        /// <summary>
        /// [BarRot] Lower left corner character 
        /// </summary>
        public char BarRotLowerLeftCornerChar
        {
            get => barRotLowerLeftCornerChar;
            set => barRotLowerLeftCornerChar = value;
        }
        /// <summary>
        /// [BarRot] Lower right corner character 
        /// </summary>
        public char BarRotLowerRightCornerChar
        {
            get => barRotLowerRightCornerChar;
            set => barRotLowerRightCornerChar = value;
        }
        /// <summary>
        /// [BarRot] Upper frame character 
        /// </summary>
        public char BarRotUpperFrameChar
        {
            get => barRotUpperFrameChar;
            set => barRotUpperFrameChar = value;
        }
        /// <summary>
        /// [BarRot] Lower frame character 
        /// </summary>
        public char BarRotLowerFrameChar
        {
            get => barRotLowerFrameChar;
            set => barRotLowerFrameChar = value;
        }
        /// <summary>
        /// [BarRot] Left frame character 
        /// </summary>
        public char BarRotLeftFrameChar
        {
            get => barRotLeftFrameChar;
            set => barRotLeftFrameChar = value;
        }
        /// <summary>
        /// [BarRot] Right frame character 
        /// </summary>
        public char BarRotRightFrameChar
        {
            get => barRotRightFrameChar;
            set => barRotRightFrameChar = value;
        }
        /// <summary>
        /// [BarRot] The minimum red color level (true color - start)
        /// </summary>
        public int BarRotMinimumRedColorLevelStart
        {
            get => barRotMinimumRedColorLevelStart;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                barRotMinimumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum green color level (true color - start)
        /// </summary>
        public int BarRotMinimumGreenColorLevelStart
        {
            get => barRotMinimumGreenColorLevelStart;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                barRotMinimumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum blue color level (true color - start)
        /// </summary>
        public int BarRotMinimumBlueColorLevelStart
        {
            get => barRotMinimumBlueColorLevelStart;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                barRotMinimumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum red color level (true color - start)
        /// </summary>
        public int BarRotMaximumRedColorLevelStart
        {
            get => barRotMaximumRedColorLevelStart;
            set
            {
                if (value <= barRotMinimumRedColorLevelStart)
                    value = barRotMinimumRedColorLevelStart;
                if (value > 255)
                    value = 255;
                barRotMaximumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum green color level (true color - start)
        /// </summary>
        public int BarRotMaximumGreenColorLevelStart
        {
            get => barRotMaximumGreenColorLevelStart;
            set
            {
                if (value <= barRotMinimumGreenColorLevelStart)
                    value = barRotMinimumGreenColorLevelStart;
                if (value > 255)
                    value = 255;
                barRotMaximumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum blue color level (true color - start)
        /// </summary>
        public int BarRotMaximumBlueColorLevelStart
        {
            get => barRotMaximumBlueColorLevelStart;
            set
            {
                if (value <= barRotMinimumBlueColorLevelStart)
                    value = barRotMinimumBlueColorLevelStart;
                if (value > 255)
                    value = 255;
                barRotMaximumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum red color level (true color - end)
        /// </summary>
        public int BarRotMinimumRedColorLevelEnd
        {
            get => barRotMinimumRedColorLevelEnd;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                barRotMinimumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum green color level (true color - end)
        /// </summary>
        public int BarRotMinimumGreenColorLevelEnd
        {
            get => barRotMinimumGreenColorLevelEnd;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                barRotMinimumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum blue color level (true color - end)
        /// </summary>
        public int BarRotMinimumBlueColorLevelEnd
        {
            get => barRotMinimumBlueColorLevelEnd;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                barRotMinimumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum red color level (true color - end)
        /// </summary>
        public int BarRotMaximumRedColorLevelEnd
        {
            get => barRotMaximumRedColorLevelEnd;
            set
            {
                if (value <= barRotMinimumRedColorLevelEnd)
                    value = barRotMinimumRedColorLevelEnd;
                if (value > 255)
                    value = 255;
                barRotMaximumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum green color level (true color - end)
        /// </summary>
        public int BarRotMaximumGreenColorLevelEnd
        {
            get => barRotMaximumGreenColorLevelEnd;
            set
            {
                if (value <= barRotMinimumGreenColorLevelEnd)
                    value = barRotMinimumGreenColorLevelEnd;
                if (value > 255)
                    value = 255;
                barRotMaximumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum blue color level (true color - end)
        /// </summary>
        public int BarRotMaximumBlueColorLevelEnd
        {
            get => barRotMaximumBlueColorLevelEnd;
            set
            {
                if (value <= barRotMinimumBlueColorLevelEnd)
                    value = barRotMinimumBlueColorLevelEnd;
                if (value > 255)
                    value = 255;
                barRotMaximumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] Upper left corner color.
        /// </summary>
        public string BarRotUpperLeftCornerColor
        {
            get => barRotUpperLeftCornerColor;
            set => barRotUpperLeftCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Upper right corner color.
        /// </summary>
        public string BarRotUpperRightCornerColor
        {
            get => barRotUpperRightCornerColor;
            set => barRotUpperRightCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Lower left corner color.
        /// </summary>
        public string BarRotLowerLeftCornerColor
        {
            get => barRotLowerLeftCornerColor;
            set => barRotLowerLeftCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Lower right corner color.
        /// </summary>
        public string BarRotLowerRightCornerColor
        {
            get => barRotLowerRightCornerColor;
            set => barRotLowerRightCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Upper frame color.
        /// </summary>
        public string BarRotUpperFrameColor
        {
            get => barRotUpperFrameColor;
            set => barRotUpperFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Lower frame color.
        /// </summary>
        public string BarRotLowerFrameColor
        {
            get => barRotLowerFrameColor;
            set => barRotLowerFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Left frame color.
        /// </summary>
        public string BarRotLeftFrameColor
        {
            get => barRotLeftFrameColor;
            set => barRotLeftFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Right frame color.
        /// </summary>
        public string BarRotRightFrameColor
        {
            get => barRotRightFrameColor;
            set => barRotRightFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Use the border colors.
        /// </summary>
        public bool BarRotUseBorderColors
        {
            get => barRotUseBorderColors;
            set => barRotUseBorderColors = value;
        }
    }
}
