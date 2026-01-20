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
using Terminaux.Colors.Data;

namespace Nitrocid.ScreensaverPacks.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public partial class ExtraSaversConfig : BaseKernelConfig
    {
        private int typoDelay = 50;
        private int typoWriteAgainDelay = 3000;
        private string typoWrite = "Nitrocid KS";
        private int typoWritingSpeedMin = 50;
        private int typoWritingSpeedMax = 80;
        private int typoMissStrikePossibility = 20;
        private int typoMissPossibility = 10;
        private string typoTextColor = new Color(ConsoleColors.White).PlainSequence;

        /// <summary>
        /// [Typo] How many milliseconds to wait before making the next write?
        /// </summary>
        public int TypoDelay
        {
            get
            {
                return typoDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                typoDelay = value;
            }
        }
        /// <summary>
        /// [Typo] How many milliseconds to wait before writing the text again?
        /// </summary>
        public int TypoWriteAgainDelay
        {
            get
            {
                return typoWriteAgainDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                typoWriteAgainDelay = value;
            }
        }
        /// <summary>
        /// [Typo] Text for Typo. Longer is better.
        /// </summary>
        public string TypoWrite
        {
            get
            {
                return typoWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                typoWrite = value;
            }
        }
        /// <summary>
        /// [Typo] Minimum writing speed in WPM
        /// </summary>
        public int TypoWritingSpeedMin
        {
            get
            {
                return typoWritingSpeedMin;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                typoWritingSpeedMin = value;
            }
        }
        /// <summary>
        /// [Typo] Maximum writing speed in WPM
        /// </summary>
        public int TypoWritingSpeedMax
        {
            get
            {
                return typoWritingSpeedMax;
            }
            set
            {
                if (value <= 0)
                    value = 80;
                typoWritingSpeedMax = value;
            }
        }
        /// <summary>
        /// [Typo] Possibility that the writer made a typo in percent
        /// </summary>
        public int TypoMissStrikePossibility
        {
            get
            {
                return typoMissStrikePossibility;
            }
            set
            {
                if (value <= 0)
                    value = 20;
                typoMissStrikePossibility = value;
            }
        }
        /// <summary>
        /// [Typo] Possibility that the writer missed a character in percent
        /// </summary>
        public int TypoMissPossibility
        {
            get
            {
                return typoMissPossibility;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                typoMissPossibility = value;
            }
        }
        /// <summary>
        /// [Typo] Text color
        /// </summary>
        public string TypoTextColor
        {
            get
            {
                return typoTextColor;
            }
            set
            {
                typoTextColor = new Color(value).PlainSequence;
            }
        }
    }
}
