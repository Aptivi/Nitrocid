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

using Terminaux.Themes;
using Nitrocid.Base.Kernel.Configuration.Instances;

namespace Nitrocid.ScreensaverPacks.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public partial class ExtraSaversConfig : BaseKernelConfig
    {
        private int sirenThemeDelay = 500;
        private string sirenThemeStyle = "Default";

        /// <summary>
        /// [SirenTheme] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SirenThemeDelay
        {
            get
            {
                return sirenThemeDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                sirenThemeDelay = value;
            }
        }

        /// <summary>
        /// [SirenTheme] The siren style
        /// </summary>
        public string SirenThemeStyle
        {
            get
            {
                return sirenThemeStyle;
            }
            set
            {
                sirenThemeStyle = ThemeTools.GetInstalledThemes().ContainsKey(value) ? value : "Default";
            }
        }
    }
}
