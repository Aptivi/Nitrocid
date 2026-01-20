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
using Nitrocid.Extras.Amusements.Screensavers.Utilities;

namespace Nitrocid.Extras.Amusements.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public partial class AmusementsSaversConfig : BaseKernelConfig
    {
        private bool bdayCardTrueColor = true;
        private int bdayCardDelay = 1000;
        private BdayCardGender bdayCardGender = BdayCardGender.Male;
        private BdayCardNameType bdayCardNameType = BdayCardNameType.Random;
        private BdayCardTextType bdayCardTextType = BdayCardTextType.Simple;
        private string bdayCardPersonName = "John";
        private int bdayCardMinimumRedColorLevel = 0;
        private int bdayCardMinimumGreenColorLevel = 0;
        private int bdayCardMinimumBlueColorLevel = 0;
        private int bdayCardMinimumColorLevel = 0;
        private int bdayCardMaximumRedColorLevel = 255;
        private int bdayCardMaximumGreenColorLevel = 255;
        private int bdayCardMaximumBlueColorLevel = 255;
        private int bdayCardMaximumColorLevel = 255;

        /// <summary>
        /// [BdayCard] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool BdayCardTrueColor
        {
            get
            {
                return bdayCardTrueColor;
            }
            set
            {
                bdayCardTrueColor = value;
            }
        }
        /// <summary>
        /// [BdayCard] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BdayCardDelay
        {
            get
            {
                return bdayCardDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                bdayCardDelay = value;
            }
        }
        /// <summary>
        /// [BdayCard] Birthday card gender
        /// </summary>
        public BdayCardGender BdayCardGender
        {
            get
            {
                return bdayCardGender;
            }
            set
            {
                bdayCardGender = value;
            }
        }
        /// <summary>
        /// [BdayCard] Birthday card name type
        /// </summary>
        public BdayCardNameType BdayCardNameType
        {
            get
            {
                return bdayCardNameType;
            }
            set
            {
                bdayCardNameType = value;
            }
        }
        /// <summary>
        /// [BdayCard] Birthday card text type
        /// </summary>
        public BdayCardTextType BdayCardTextType
        {
            get
            {
                return bdayCardTextType;
            }
            set
            {
                bdayCardTextType = value;
            }
        }
        /// <summary>
        /// [BdayCard] Person name that will be on the birthday card
        /// </summary>
        public string BdayCardPersonName
        {
            get
            {
                return bdayCardPersonName;
            }
            set
            {
                bdayCardPersonName = value;
            }
        }
        /// <summary>
        /// [BdayCard] The minimum red color level (true color)
        /// </summary>
        public int BdayCardMinimumRedColorLevel
        {
            get
            {
                return bdayCardMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                bdayCardMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BdayCard] The minimum green color level (true color)
        /// </summary>
        public int BdayCardMinimumGreenColorLevel
        {
            get
            {
                return bdayCardMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                bdayCardMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BdayCard] The minimum blue color level (true color)
        /// </summary>
        public int BdayCardMinimumBlueColorLevel
        {
            get
            {
                return bdayCardMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                bdayCardMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BdayCard] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BdayCardMinimumColorLevel
        {
            get
            {
                return bdayCardMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                bdayCardMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BdayCard] The maximum red color level (true color)
        /// </summary>
        public int BdayCardMaximumRedColorLevel
        {
            get
            {
                return bdayCardMaximumRedColorLevel;
            }
            set
            {
                if (value <= bdayCardMinimumRedColorLevel)
                    value = bdayCardMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                bdayCardMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BdayCard] The maximum green color level (true color)
        /// </summary>
        public int BdayCardMaximumGreenColorLevel
        {
            get
            {
                return bdayCardMaximumGreenColorLevel;
            }
            set
            {
                if (value <= bdayCardMinimumGreenColorLevel)
                    value = bdayCardMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                bdayCardMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BdayCard] The maximum blue color level (true color)
        /// </summary>
        public int BdayCardMaximumBlueColorLevel
        {
            get
            {
                return bdayCardMaximumBlueColorLevel;
            }
            set
            {
                if (value <= bdayCardMinimumBlueColorLevel)
                    value = bdayCardMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                bdayCardMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BdayCard] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BdayCardMaximumColorLevel
        {
            get
            {
                return bdayCardMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= bdayCardMinimumColorLevel)
                    value = bdayCardMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                bdayCardMaximumColorLevel = value;
            }
        }
    }
}
