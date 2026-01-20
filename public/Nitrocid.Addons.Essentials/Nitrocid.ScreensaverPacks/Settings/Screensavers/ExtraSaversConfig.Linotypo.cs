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
using Nitrocid.ScreensaverPacks.Screensavers;
using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace Nitrocid.ScreensaverPacks.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public partial class ExtraSaversConfig : BaseKernelConfig
    {
        private int linotypoDelay = 50;
        private int linotypoNewScreenDelay = 3000;
        private string linotypoWrite = "Nitrocid KS";
        private int linotypoWritingSpeedMin = 50;
        private int linotypoWritingSpeedMax = 80;
        private int linotypoMissStrikePossibility = 1;
        private int linotypoTextColumns = 3;
        private int linotypoEtaoinThreshold = 5;
        private int linotypoEtaoinCappingPossibility = 5;
        private int linotypoEtaoinType = 0;
        private int linotypoMissPossibility = 10;
        private string linotypoTextColor = new Color(ConsoleColors.White).PlainSequence;

        /// <summary>
        /// [Linotypo] How many milliseconds to wait before making the next write?
        /// </summary>
        public int LinotypoDelay
        {
            get
            {
                return linotypoDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                linotypoDelay = value;
            }
        }
        /// <summary>
        /// [Linotypo] How many milliseconds to wait before writing the text in the new screen again?
        /// </summary>
        public int LinotypoNewScreenDelay
        {
            get
            {
                return linotypoNewScreenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                linotypoNewScreenDelay = value;
            }
        }
        /// <summary>
        /// [Linotypo] Text for Linotypo. Longer is better.
        /// </summary>
        public string LinotypoWrite
        {
            get
            {
                return linotypoWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                linotypoWrite = value;
            }
        }
        /// <summary>
        /// [Linotypo] Minimum writing speed in WPM
        /// </summary>
        public int LinotypoWritingSpeedMin
        {
            get
            {
                return linotypoWritingSpeedMin;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                linotypoWritingSpeedMin = value;
            }
        }
        /// <summary>
        /// [Linotypo] Maximum writing speed in WPM
        /// </summary>
        public int LinotypoWritingSpeedMax
        {
            get
            {
                return linotypoWritingSpeedMax;
            }
            set
            {
                if (value <= 0)
                    value = 80;
                linotypoWritingSpeedMax = value;
            }
        }
        /// <summary>
        /// [Linotypo] Possibility that the writer made a typo in percent
        /// </summary>
        public int LinotypoMissStrikePossibility
        {
            get
            {
                return linotypoMissStrikePossibility;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                linotypoMissStrikePossibility = value;
            }
        }
        /// <summary>
        /// [Linotypo] The text columns to be printed.
        /// </summary>
        public int LinotypoTextColumns
        {
            get
            {
                return linotypoTextColumns;
            }
            set
            {
                if (value <= 0)
                    value = 3;
                if (value > 3)
                    value = 3;
                linotypoTextColumns = value;
            }
        }
        /// <summary>
        /// [Linotypo] How many characters to write before triggering the "line fill"?
        /// </summary>
        public int LinotypoEtaoinThreshold
        {
            get
            {
                return linotypoEtaoinThreshold;
            }
            set
            {
                if (value <= 0)
                    value = 5;
                if (value > 8)
                    value = 8;
                linotypoEtaoinThreshold = value;
            }
        }
        /// <summary>
        /// [Linotypo] Possibility that the Etaoin pattern will be printed in all caps in percent
        /// </summary>
        public int LinotypoEtaoinCappingPossibility
        {
            get
            {
                return linotypoEtaoinCappingPossibility;
            }
            set
            {
                if (value <= 0)
                    value = 5;
                linotypoEtaoinCappingPossibility = value;
            }
        }
        /// <summary>
        /// [Linotypo] Line fill pattern type
        /// </summary>
        public FillType LinotypoEtaoinType
        {
            get
            {
                return (FillType)linotypoEtaoinType;
            }
            set
            {
                linotypoEtaoinType = (int)value;
            }
        }
        /// <summary>
        /// [Linotypo] Possibility that the writer missed a character in percent
        /// </summary>
        public int LinotypoMissPossibility
        {
            get
            {
                return linotypoMissPossibility;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                linotypoMissPossibility = value;
            }
        }
        /// <summary>
        /// [Linotypo] Text color
        /// </summary>
        public string LinotypoTextColor
        {
            get
            {
                return linotypoTextColor;
            }
            set
            {
                linotypoTextColor = new Color(value).PlainSequence;
            }
        }
    }
}
