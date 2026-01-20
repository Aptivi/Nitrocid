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

namespace Nitrocid.Extras.Dates.Settings
{
    /// <summary>
    /// Configuration instance for date tools
    /// </summary>
    public partial class DatesConfig : BaseKernelConfig
    {
        /// <summary>
        /// If enabled, will use figlet for timer. Please note that it needs a big console screen in order to render the time properly with Figlet enabled.
        /// </summary>
        public bool EnableFigletTimer { get; set; } = true;
        /// <summary>
        /// Write a figlet font that is supported by the Figlet library. Consult the library documentation for more information
        /// </summary>
        public string TimerFigletFont { get; set; } = "small";
    }
}
