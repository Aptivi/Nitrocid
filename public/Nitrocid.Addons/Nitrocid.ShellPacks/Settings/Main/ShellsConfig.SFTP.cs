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

using Nitrocid.Base.Kernel.Configuration.Instances;
using Terminaux.Shell.Prompts;

namespace Nitrocid.ShellPacks.Settings
{
    /// <summary>
    /// Configuration instance for all shells
    /// </summary>
    public partial class ShellsConfig : BaseKernelConfig
    {
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
