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
using Nitrocid.Shell.Prompts;

namespace Nitrocid.Extras.SftpShell.Settings
{
    /// <summary>
    /// Configuration instance for SFTP
    /// </summary>
    public class SftpConfig : BaseKernelConfig, IKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries =>
            ConfigTools.GetSettingsEntries(ResourcesManager.GetData("SftpSettings.json", ResourcesType.Misc, typeof(SftpConfig).Assembly) ??
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Failed to obtain settings entries.")));

        /// <summary>
        /// SFTP Prompt Preset
        /// </summary>
        public string SftpPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell("SFTPShell").PresetName;
            set => PromptPresetManager.SetPreset(value, "SFTPShell", false);
        }
        /// <summary>
        /// Shows the SFTP file details while listing remote directories
        /// </summary>
        public bool SFTPShowDetailsInList { get; set; } = true;
        /// <summary>
        /// Write how you want your login prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string SFTPUserPromptStyle { get; set; } = "";
        /// <summary>
        /// If enabled, adds a new connection to the SFTP speed dial
        /// </summary>
        public bool SFTPNewConnectionsToSpeedDial { get; set; } = true;
    }
}
