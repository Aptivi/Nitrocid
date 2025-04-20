﻿//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection.Internal;

namespace Nitrocid.Kernel.Configuration.Instances
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public class KernelSaverConfig : BaseKernelConfig, IKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries =>
            ConfigTools.GetSettingsEntries(ResourcesManager.GetData("ScreensaverSettingsEntries.json", ResourcesType.Settings) ??
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Failed to obtain screensaver settings entries.")));

        #region MatrixBleed
        private int matrixBleedDelay = 10;
        private int matrixBleedMaxSteps = 25;
        private int matrixBleedDropChance = 40;

        /// <summary>
        /// [MatrixBleed] How many milliseconds to wait before making the next write?
        /// </summary>
        public int MatrixBleedDelay
        {
            get
            {
                return matrixBleedDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                matrixBleedDelay = value;
            }
        }

        /// <summary>
        /// [MatrixBleed] How many fade steps to do?
        /// </summary>
        public int MatrixBleedMaxSteps
        {
            get
            {
                return matrixBleedMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                matrixBleedMaxSteps = value;
            }
        }

        /// <summary>
        /// [MatrixBleed] Chance to drop a new falling matrix
        /// </summary>
        public int MatrixBleedDropChance
        {
            get
            {
                return matrixBleedDropChance;
            }
            set
            {
                if (value <= 0)
                    value = 40;
                matrixBleedDropChance = value;
            }
        }
        #endregion
    }
}
