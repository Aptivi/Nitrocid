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
        private int mirrorWriteDelay = 50;
        private int mirrorWriteNewScreenDelay = 3000;
        private string mirrorWriteWrite = "Nitrocid KS";
        private int mirrorWriteWritingSpeedMin = 50;
        private int mirrorWriteWritingSpeedMax = 80;
        private string mirrorWriteTextColor = new Color(ConsoleColors.White).PlainSequence;

        /// <summary>
        /// [MirrorWrite] How many milliseconds to wait before making the next write?
        /// </summary>
        public int MirrorWriteDelay
        {
            get
            {
                return mirrorWriteDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                mirrorWriteDelay = value;
            }
        }
        /// <summary>
        /// [MirrorWrite] How many milliseconds to wait before writing the text in the new screen again?
        /// </summary>
        public int MirrorWriteNewScreenDelay
        {
            get
            {
                return mirrorWriteNewScreenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                mirrorWriteNewScreenDelay = value;
            }
        }
        /// <summary>
        /// [MirrorWrite] Text for MirrorWrite. Longer is better.
        /// </summary>
        public string MirrorWriteWrite
        {
            get
            {
                return mirrorWriteWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                mirrorWriteWrite = value;
            }
        }
        /// <summary>
        /// [MirrorWrite] Minimum writing speed in WPM
        /// </summary>
        public int MirrorWriteWritingSpeedMin
        {
            get
            {
                return mirrorWriteWritingSpeedMin;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                mirrorWriteWritingSpeedMin = value;
            }
        }
        /// <summary>
        /// [MirrorWrite] Maximum writing speed in WPM
        /// </summary>
        public int MirrorWriteWritingSpeedMax
        {
            get
            {
                return mirrorWriteWritingSpeedMax;
            }
            set
            {
                if (value <= 0)
                    value = 80;
                mirrorWriteWritingSpeedMax = value;
            }
        }
        /// <summary>
        /// [MirrorWrite] Text color
        /// </summary>
        public string MirrorWriteTextColor
        {
            get
            {
                return mirrorWriteTextColor;
            }
            set
            {
                mirrorWriteTextColor = new Color(value).PlainSequence;
            }
        }
    }
}
