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
        private int typewriterDelay = 50;
        private int typewriterNewScreenDelay = 3000;
        private string typewriterWrite = "Nitrocid KS";
        private int typewriterWritingSpeedMin = 50;
        private int typewriterWritingSpeedMax = 80;
        private bool typewriterShowArrowPos = true;
        private string typewriterTextColor = new Color(ConsoleColors.White).PlainSequence;

        /// <summary>
        /// [Typewriter] How many milliseconds to wait before making the next write?
        /// </summary>
        public int TypewriterDelay
        {
            get
            {
                return typewriterDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                typewriterDelay = value;
            }
        }
        /// <summary>
        /// [Typewriter] How many milliseconds to wait before writing the text in the new screen again?
        /// </summary>
        public int TypewriterNewScreenDelay
        {
            get
            {
                return typewriterNewScreenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                typewriterNewScreenDelay = value;
            }
        }
        /// <summary>
        /// [Typewriter] Text for Typewriter. Longer is better.
        /// </summary>
        public string TypewriterWrite
        {
            get
            {
                return typewriterWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                typewriterWrite = value;
            }
        }
        /// <summary>
        /// [Typewriter] Minimum writing speed in WPM
        /// </summary>
        public int TypewriterWritingSpeedMin
        {
            get
            {
                return typewriterWritingSpeedMin;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                typewriterWritingSpeedMin = value;
            }
        }
        /// <summary>
        /// [Typewriter] Maximum writing speed in WPM
        /// </summary>
        public int TypewriterWritingSpeedMax
        {
            get
            {
                return typewriterWritingSpeedMax;
            }
            set
            {
                if (value <= 0)
                    value = 80;
                typewriterWritingSpeedMax = value;
            }
        }
        /// <summary>
        /// [Typewriter] Shows the typewriter letter column position by showing this key on the bottom of the screen: <code>^</code>
        /// </summary>
        public bool TypewriterShowArrowPos
        {
            get
            {
                return typewriterShowArrowPos;
            }
            set
            {
                typewriterShowArrowPos = value;
            }
        }
        /// <summary>
        /// [Typewriter] Text color
        /// </summary>
        public string TypewriterTextColor
        {
            get
            {
                return typewriterTextColor;
            }
            set
            {
                typewriterTextColor = new Color(value).PlainSequence;
            }
        }
    }
}
