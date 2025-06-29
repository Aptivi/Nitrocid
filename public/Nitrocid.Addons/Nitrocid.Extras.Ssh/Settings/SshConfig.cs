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
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Configuration.Instances;
using Nitrocid.Base.Kernel.Configuration.Settings;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Reflection.Internal;

namespace Nitrocid.Extras.Ssh.Settings
{
    /// <summary>
    /// Configuration instance for SSH
    /// </summary>
    public class SshConfig : BaseKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries
        {
            get
            {
                var dataStream = ResourcesManager.GetData("SshSettings.json", ResourcesType.Misc, typeof(SshConfig).Assembly) ??
                    throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_SSH_SETTINGS_EXCEPTION_ENTRIESFAILED"));
                string dataString = ResourcesManager.ConvertToString(dataStream);
                return ConfigTools.GetSettingsEntries(dataString);
            }
        }

        /// <summary>
        /// Write how you want your login prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string SSHUserPromptStyle { get; set; } = "";
        /// <summary>
        /// If enabled, adds a new connection to the SSH speed dial
        /// </summary>
        public bool SSHNewConnectionsToSpeedDial { get; set; } = true;
    }
}
