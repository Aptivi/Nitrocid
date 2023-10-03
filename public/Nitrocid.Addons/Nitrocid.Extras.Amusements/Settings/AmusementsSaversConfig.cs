
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Kernel.Configuration;
using KS.Kernel.Configuration.Instances;
using KS.Kernel.Configuration.Settings;
using Newtonsoft.Json;

namespace Nitrocid.Extras.Amusements.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public class AmusementsSaversConfig : BaseKernelConfig, IKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries =>
            ConfigTools.GetSettingsEntries(Resources.AddonResources.AmusementsSaverSettings);

        /// <summary>
        /// [Snaker] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool SnakerTrueColor { get; set; } = true;
        /// <summary>
        /// [Snaker] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SnakerDelay { get; set; } = 100;
        /// <summary>
        /// [Snaker] How many milliseconds to wait before making the next stage?
        /// </summary>
        public int SnakerStageDelay { get; set; } = 5000;
        /// <summary>
        /// [Snaker] The minimum red color level (true color)
        /// </summary>
        public int SnakerMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [Snaker] The minimum green color level (true color)
        /// </summary>
        public int SnakerMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [Snaker] The minimum blue color level (true color)
        /// </summary>
        public int SnakerMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [Snaker] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int SnakerMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [Snaker] The maximum red color level (true color)
        /// </summary>
        public int SnakerMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [Snaker] The maximum green color level (true color)
        /// </summary>
        public int SnakerMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [Snaker] The maximum blue color level (true color)
        /// </summary>
        public int SnakerMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [Snaker] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int SnakerMaximumColorLevel { get; set; } = 255;
        /// <summary>
        /// [Quote] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool QuoteTrueColor { get; set; } = true;
        /// <summary>
        /// [Quote] How many milliseconds to wait before making the next write?
        /// </summary>
        public int QuoteDelay { get; set; } = 10000;
        /// <summary>
        /// [Quote] The minimum red color level (true color)
        /// </summary>
        public int QuoteMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [Quote] The minimum green color level (true color)
        /// </summary>
        public int QuoteMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [Quote] The minimum blue color level (true color)
        /// </summary>
        public int QuoteMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [Quote] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int QuoteMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [Quote] The maximum red color level (true color)
        /// </summary>
        public int QuoteMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [Quote] The maximum green color level (true color)
        /// </summary>
        public int QuoteMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [Quote] The maximum blue color level (true color)
        /// </summary>
        public int QuoteMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [Quote] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int QuoteMaximumColorLevel { get; set; } = 255;
    }
}
