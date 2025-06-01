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
    /// Splash kernel configuration instance
    /// </summary>
    public class KernelSplashConfig : BaseKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override string Name =>
            Translate.DoTranslation("Kernel splash settings");

        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries
        {
            get
            {
                var dataStream = ResourcesManager.GetData("SplashSettingsEntries.json", ResourcesType.Settings) ??
                    throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Failed to obtain splash settings entries."));
                string dataString = ResourcesManager.ConvertToString(dataStream);
                return ConfigTools.GetSettingsEntries(dataString);
            }
        }

        #region Welcome
        private bool welcomeShowProgress = false;

        /// <summary>
        /// [Welcome] Show progress or not
        /// </summary>
        public bool WelcomeShowProgress
        {
            get => welcomeShowProgress;
            set => welcomeShowProgress = value;
        }
        #endregion
    }
}
