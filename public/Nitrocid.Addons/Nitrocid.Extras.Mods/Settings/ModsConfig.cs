//
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
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Configuration.Instances;
using Nitrocid.Kernel.Configuration.Settings;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection.Internal;

namespace Nitrocid.Extras.Mods.Settings
{
    /// <summary>
    /// Configuration instance for Mods
    /// </summary>
    public class ModsConfig : BaseKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries
        {
            get
            {
                var dataStream = ResourcesManager.GetData("ModsSettings.json", ResourcesType.Misc, typeof(ModsConfig).Assembly) ??
                    throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Failed to obtain settings entries."));
                string dataString = ResourcesManager.ConvertToString(dataStream);
                return ConfigTools.GetSettingsEntries(dataString);
            }
        }

        /// <summary>
        /// Automatically start the kernel modifications on boot.
        /// </summary>
        public bool StartKernelMods { get; set; }
        /// <summary>
        /// Allow untrusted mods
        /// </summary>
        public bool AllowUntrustedMods { get; set; }
        /// <summary>
        /// Write the filenames of the mods that will not run on startup. When you're finished, write "q". Write a minus sign next to the path to remove an existing mod.
        /// </summary>
        public string BlacklistedModsString { get; set; } = "";
        /// <summary>
        /// Show the mod commands count on help
        /// </summary>
        public bool ShowModCommandsCount { get; set; } = true;
    }
}
