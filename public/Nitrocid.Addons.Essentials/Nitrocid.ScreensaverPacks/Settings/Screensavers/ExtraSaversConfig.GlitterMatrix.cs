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
        private int glitterMatrixDelay = 1;
        private string glitterMatrixBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private string glitterMatrixForegroundColor = new Color(ConsoleColors.Green).PlainSequence;

        /// <summary>
        /// [GlitterMatrix] How many milliseconds to wait before making the next write?
        /// </summary>
        public int GlitterMatrixDelay
        {
            get
            {
                return glitterMatrixDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                glitterMatrixDelay = value;
            }
        }
        /// <summary>
        /// [GlitterMatrix] Screensaver background color
        /// </summary>
        public string GlitterMatrixBackgroundColor
        {
            get
            {
                return glitterMatrixBackgroundColor;
            }
            set
            {
                glitterMatrixBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [GlitterMatrix] Screensaver foreground color
        /// </summary>
        public string GlitterMatrixForegroundColor
        {
            get
            {
                return glitterMatrixForegroundColor;
            }
            set
            {
                glitterMatrixForegroundColor = new Color(value).PlainSequence;
            }
        }
    }
}
