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

using Newtonsoft.Json;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Configuration.Instances;
using Nitrocid.Kernel.Configuration.Settings;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection.Internal;

namespace Nitrocid.Extras.NameGen.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public class NameGenSaversConfig : BaseKernelConfig, IKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries
        {
            get
            {
                var dataStream = ResourcesManager.GetData("NameGenSaverSettings.json", ResourcesType.Misc, typeof(NameGenSaversConfig).Assembly) ??
                    throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Failed to obtain settings entries."));
                string dataString = ResourcesManager.ConvertToString(dataStream);
                return ConfigTools.GetSettingsEntries(dataString);
            }
        }

        #region PersonLookup
        private int personLookupDelay = 75;
        private int personLookupLookedUpDelay = 10000;
        private int personLookupMinimumNames = 10;
        private int personLookupMaximumNames = 100;
        private int personLookupMinimumAgeYears = 18;
        private int personLookupMaximumAgeYears = 100;

        /// <summary>
        /// [PersonLookup] How many milliseconds to wait before getting the new name?
        /// </summary>
        public int PersonLookupDelay
        {
            get
            {
                return personLookupDelay;
            }
            set
            {
                if (value <= 0)
                    value = 75;
                personLookupDelay = value;
            }
        }
        /// <summary>
        /// [PersonLookup] How many milliseconds to show the looked up name?
        /// </summary>
        public int PersonLookupLookedUpDelay
        {
            get
            {
                return personLookupLookedUpDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10000;
                personLookupLookedUpDelay = value;
            }
        }
        /// <summary>
        /// [PersonLookup] Minimum names count
        /// </summary>
        public int PersonLookupMinimumNames
        {
            get
            {
                return personLookupMinimumNames;
            }
            set
            {
                if (value <= 10)
                    value = 10;
                if (value > 1000)
                    value = 1000;
                personLookupMinimumNames = value;
            }
        }
        /// <summary>
        /// [PersonLookup] Maximum names count
        /// </summary>
        public int PersonLookupMaximumNames
        {
            get
            {
                return personLookupMaximumNames;
            }
            set
            {
                if (value <= personLookupMinimumNames)
                    value = personLookupMinimumNames;
                if (value > 1000)
                    value = 1000;
                personLookupMaximumNames = value;
            }
        }
        /// <summary>
        /// [PersonLookup] Minimum age years
        /// </summary>
        public int PersonLookupMinimumAgeYears
        {
            get
            {
                return personLookupMinimumAgeYears;
            }
            set
            {
                if (value <= 18)
                    value = 18;
                if (value > 100)
                    value = 100;
                personLookupMinimumAgeYears = value;
            }
        }
        /// <summary>
        /// [PersonLookup] Maximum age years
        /// </summary>
        public int PersonLookupMaximumAgeYears
        {
            get
            {
                return personLookupMaximumAgeYears;
            }
            set
            {
                if (value <= personLookupMinimumAgeYears)
                    value = personLookupMinimumAgeYears;
                if (value > 100)
                    value = 100;
                personLookupMaximumAgeYears = value;
            }
        }
        #endregion
    }
}
