//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Newtonsoft.Json;
using Nitrocid.Kernel.Configuration.Settings;
using Nitrocid.Misc.Reflection.Internal;

namespace Nitrocid.Kernel.Configuration.Instances
{
    /// <summary>
    /// Widgets kernel configuration instance
    /// </summary>
    public class KernelWidgetsConfig : BaseKernelConfig, IKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries =>
            ConfigTools.GetSettingsEntries(ResourcesManager.GetData("WidgetsSettingsEntries.json", ResourcesType.Settings));

        #region Analog
        /// <summary>
        /// [Analog] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool AnalogTrueColor { get; set; } = true;
        /// <summary>
        /// [Analog] How many milliseconds to wait before making the next write?
        /// </summary>
        public int AnalogDelay { get; set; } = 1000;
        /// <summary>
        /// [Analog] Shows the seconds hand.
        /// </summary>
        public bool AnalogShowSecondsHand { get; set; } = true;
        /// <summary>
        /// [Analog] The minimum red color level (true color)
        /// </summary>
        public int AnalogMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [Analog] The minimum green color level (true color)
        /// </summary>
        public int AnalogMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [Analog] The minimum blue color level (true color)
        /// </summary>
        public int AnalogMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [Analog] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int AnalogMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [Analog] The maximum red color level (true color)
        /// </summary>
        public int AnalogMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [Analog] The maximum green color level (true color)
        /// </summary>
        public int AnalogMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [Analog] The maximum blue color level (true color)
        /// </summary>
        public int AnalogMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [Analog] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int AnalogMaximumColorLevel { get; set; } = 255;
        #endregion

        #region Digital
        /// <summary>
        /// [Digital] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool DigitalTrueColor { get; set; } = true;
        /// <summary>
        /// [Digital] How many milliseconds to wait before making the next write?
        /// </summary>
        public int DigitalDelay { get; set; } = 1000;
        /// <summary>
        /// [Digital] The minimum red color level (true color)
        /// </summary>
        public int DigitalMinimumRedColorLevel { get; set; } = 0;
        /// <summary>
        /// [Digital] The minimum green color level (true color)
        /// </summary>
        public int DigitalMinimumGreenColorLevel { get; set; } = 0;
        /// <summary>
        /// [Digital] The minimum blue color level (true color)
        /// </summary>
        public int DigitalMinimumBlueColorLevel { get; set; } = 0;
        /// <summary>
        /// [Digital] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int DigitalMinimumColorLevel { get; set; } = 0;
        /// <summary>
        /// [Digital] The maximum red color level (true color)
        /// </summary>
        public int DigitalMaximumRedColorLevel { get; set; } = 255;
        /// <summary>
        /// [Digital] The maximum green color level (true color)
        /// </summary>
        public int DigitalMaximumGreenColorLevel { get; set; } = 255;
        /// <summary>
        /// [Digital] The maximum blue color level (true color)
        /// </summary>
        public int DigitalMaximumBlueColorLevel { get; set; } = 255;
        /// <summary>
        /// [Digital] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int DigitalMaximumColorLevel { get; set; } = 255;
        #endregion
    }
}
