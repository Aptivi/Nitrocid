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
        private int spotWriteDelay = 100;
        private int spotWriteNewScreenDelay = 3000;
        private string spotWriteWrite = "Nitrocid KS";
        private string spotWriteTextColor = new Color(ConsoleColors.White).PlainSequence;

        /// <summary>
        /// [SpotWrite] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SpotWriteDelay
        {
            get
            {
                return spotWriteDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                spotWriteDelay = value;
            }
        }
        /// <summary>
        /// [SpotWrite] Text for SpotWrite. Longer is better.
        /// </summary>
        public string SpotWriteWrite
        {
            get
            {
                return spotWriteWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                spotWriteWrite = value;
            }
        }
        /// <summary>
        /// [SpotWrite] How many milliseconds to wait before writing the text in the new screen again?
        /// </summary>
        public int SpotWriteNewScreenDelay
        {
            get
            {
                return spotWriteNewScreenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                spotWriteNewScreenDelay = value;
            }
        }
        /// <summary>
        /// [SpotWrite] Text color
        /// </summary>
        public string SpotWriteTextColor
        {
            get
            {
                return spotWriteTextColor;
            }
            set
            {
                spotWriteTextColor = new Color(value).PlainSequence;
            }
        }
    }
}
