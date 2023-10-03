
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

namespace Nitrocid.Extras.NameGen.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public class NameGenSaversConfig : BaseKernelConfig, IKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries =>
            ConfigTools.GetSettingsEntries(Resources.AddonResources.NameGenSaverSettings);

        /// <summary>
        /// [PersonLookup] How many milliseconds to wait before getting the new name?
        /// </summary>
        public int PersonLookupDelay { get; set; } = 75;
        /// <summary>
        /// [PersonLookup] How many milliseconds to show the looked up name?
        /// </summary>
        public int PersonLookupLookedUpDelay { get; set; } = 10000;
        /// <summary>
        /// [PersonLookup] Minimum names count
        /// </summary>
        public int PersonLookupMinimumNames { get; set; } = 10;
        /// <summary>
        /// [PersonLookup] Maximum names count
        /// </summary>
        public int PersonLookupMaximumNames { get; set; } = 100;
        /// <summary>
        /// [PersonLookup] Minimum age years
        /// </summary>
        public int PersonLookupMinimumAgeYears { get; set; } = 18;
        /// <summary>
        /// [PersonLookup] Maximum age years
        /// </summary>
        public int PersonLookupMaximumAgeYears { get; set; } = 100;
    }
}
