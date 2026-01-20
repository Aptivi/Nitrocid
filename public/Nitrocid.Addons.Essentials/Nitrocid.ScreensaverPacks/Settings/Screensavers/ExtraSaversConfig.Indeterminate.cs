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
        private bool indeterminateTrueColor = true;
        private int indeterminateDelay = 20;
        private char indeterminateUpperLeftCornerChar = '╭';
        private char indeterminateUpperRightCornerChar = '╮';
        private char indeterminateLowerLeftCornerChar = '╰';
        private char indeterminateLowerRightCornerChar = '╯';
        private char indeterminateUpperFrameChar = '─';
        private char indeterminateLowerFrameChar = '─';
        private char indeterminateLeftFrameChar = '│';
        private char indeterminateRightFrameChar = '│';
        private int indeterminateMinimumRedColorLevel = 0;
        private int indeterminateMinimumGreenColorLevel = 0;
        private int indeterminateMinimumBlueColorLevel = 0;
        private int indeterminateMinimumColorLevel = 0;
        private int indeterminateMaximumRedColorLevel = 255;
        private int indeterminateMaximumGreenColorLevel = 255;
        private int indeterminateMaximumBlueColorLevel = 255;
        private int indeterminateMaximumColorLevel = 255;
        private string indeterminateUpperLeftCornerColor = "7";
        private string indeterminateUpperRightCornerColor = "7";
        private string indeterminateLowerLeftCornerColor = "7";
        private string indeterminateLowerRightCornerColor = "7";
        private string indeterminateUpperFrameColor = "7";
        private string indeterminateLowerFrameColor = "7";
        private string indeterminateLeftFrameColor = "7";
        private string indeterminateRightFrameColor = "7";
        private bool indeterminateUseBorderColors;

        /// <summary>
        /// [Indeterminate] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool IndeterminateTrueColor
        {
            get => indeterminateTrueColor;
            set => indeterminateTrueColor = value;
        }
        /// <summary>
        /// [Indeterminate] How many milliseconds to wait before making the next write?
        /// </summary>
        public int IndeterminateDelay
        {
            get => indeterminateDelay;
            set
            {
                if (value <= 0)
                    value = 20;
                indeterminateDelay = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Upper left corner character 
        /// </summary>
        public char IndeterminateUpperLeftCornerChar
        {
            get => indeterminateUpperLeftCornerChar;
            set => indeterminateUpperLeftCornerChar = value;
        }
        /// <summary>
        /// [Indeterminate] Upper right corner character 
        /// </summary>
        public char IndeterminateUpperRightCornerChar
        {
            get => indeterminateUpperRightCornerChar;
            set => indeterminateUpperRightCornerChar = value;
        }
        /// <summary>
        /// [Indeterminate] Lower left corner character 
        /// </summary>
        public char IndeterminateLowerLeftCornerChar
        {
            get => indeterminateLowerLeftCornerChar;
            set => indeterminateLowerLeftCornerChar = value;
        }
        /// <summary>
        /// [Indeterminate] Lower right corner character 
        /// </summary>
        public char IndeterminateLowerRightCornerChar
        {
            get => indeterminateLowerRightCornerChar;
            set => indeterminateLowerRightCornerChar = value;
        }
        /// <summary>
        /// [Indeterminate] Upper frame character 
        /// </summary>
        public char IndeterminateUpperFrameChar
        {
            get => indeterminateUpperFrameChar;
            set => indeterminateUpperFrameChar = value;
        }
        /// <summary>
        /// [Indeterminate] Lower frame character 
        /// </summary>
        public char IndeterminateLowerFrameChar
        {
            get => indeterminateLowerFrameChar;
            set => indeterminateLowerFrameChar = value;
        }
        /// <summary>
        /// [Indeterminate] Left frame character 
        /// </summary>
        public char IndeterminateLeftFrameChar
        {
            get => indeterminateLeftFrameChar;
            set => indeterminateLeftFrameChar = value;
        }
        /// <summary>
        /// [Indeterminate] Right frame character 
        /// </summary>
        public char IndeterminateRightFrameChar
        {
            get => indeterminateRightFrameChar;
            set => indeterminateRightFrameChar = value;
        }
        /// <summary>
        /// [Indeterminate] The minimum red color level (true color)
        /// </summary>
        public int IndeterminateMinimumRedColorLevel
        {
            get => indeterminateMinimumRedColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                indeterminateMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The minimum green color level (true color)
        /// </summary>
        public int IndeterminateMinimumGreenColorLevel
        {
            get => indeterminateMinimumGreenColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                indeterminateMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The minimum blue color level (true color)
        /// </summary>
        public int IndeterminateMinimumBlueColorLevel
        {
            get => indeterminateMinimumBlueColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                indeterminateMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int IndeterminateMinimumColorLevel
        {
            get => indeterminateMinimumColorLevel;
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                indeterminateMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The maximum red color level (true color)
        /// </summary>
        public int IndeterminateMaximumRedColorLevel
        {
            get => indeterminateMaximumRedColorLevel;
            set
            {
                if (value <= indeterminateMinimumRedColorLevel)
                    value = indeterminateMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                indeterminateMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The maximum green color level (true color)
        /// </summary>
        public int IndeterminateMaximumGreenColorLevel
        {
            get => indeterminateMaximumGreenColorLevel;
            set
            {
                if (value <= indeterminateMinimumGreenColorLevel)
                    value = indeterminateMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                indeterminateMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The maximum blue color level (true color)
        /// </summary>
        public int IndeterminateMaximumBlueColorLevel
        {
            get => indeterminateMaximumBlueColorLevel;
            set
            {
                if (value <= indeterminateMinimumBlueColorLevel)
                    value = indeterminateMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                indeterminateMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int IndeterminateMaximumColorLevel
        {
            get => indeterminateMaximumColorLevel;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= indeterminateMinimumColorLevel)
                    value = indeterminateMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                indeterminateMaximumColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Upper left corner color.
        /// </summary>
        public string IndeterminateUpperLeftCornerColor
        {
            get => indeterminateUpperLeftCornerColor;
            set => indeterminateUpperLeftCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Upper right corner color.
        /// </summary>
        public string IndeterminateUpperRightCornerColor
        {
            get => indeterminateUpperRightCornerColor;
            set => indeterminateUpperRightCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Lower left corner color.
        /// </summary>
        public string IndeterminateLowerLeftCornerColor
        {
            get => indeterminateLowerLeftCornerColor;
            set => indeterminateLowerLeftCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Lower right corner color.
        /// </summary>
        public string IndeterminateLowerRightCornerColor
        {
            get => indeterminateLowerRightCornerColor;
            set => indeterminateLowerRightCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Upper frame color.
        /// </summary>
        public string IndeterminateUpperFrameColor
        {
            get => indeterminateUpperFrameColor;
            set => indeterminateUpperFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Lower frame color.
        /// </summary>
        public string IndeterminateLowerFrameColor
        {
            get => indeterminateLowerFrameColor;
            set => indeterminateLowerFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Left frame color.
        /// </summary>
        public string IndeterminateLeftFrameColor
        {
            get => indeterminateLeftFrameColor;
            set => indeterminateLeftFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Right frame color.
        /// </summary>
        public string IndeterminateRightFrameColor
        {
            get => indeterminateRightFrameColor;
            set => indeterminateRightFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Use the border colors.
        /// </summary>
        public bool IndeterminateUseBorderColors
        {
            get => indeterminateUseBorderColors;
            set => indeterminateUseBorderColors = value;
        }
    }
}
