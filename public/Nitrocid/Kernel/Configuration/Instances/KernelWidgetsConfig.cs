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
        /// [Analog] Shows the seconds hand.
        /// </summary>
        public bool AnalogShowSecondsHand { get; set; } = true;
        #endregion

        #region Digital
        /// <summary>
        /// [Digital] Displays the date in the digital clock.
        /// </summary>
        public bool DigitalDisplayDate { get; set; } = true;
        #endregion
    }
}
