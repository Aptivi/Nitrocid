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
using Nitrocid.Base.Kernel.Configuration.Instances;
using Terminaux.Shell.Prompts;
using Nitrocid.ShellPacks.Shells.Json;

namespace Nitrocid.ShellPacks.Settings
{
    /// <summary>
    /// Configuration instance for all shells
    /// </summary>
    public partial class ShellsConfig : BaseKernelConfig
    {
        /// <summary>
        /// Prompt Preset
        /// </summary>
        public string JsonShellPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell("JsonShell").PresetName;
            set => PromptPresetManager.SetPreset(value, "JsonShell", false);
        }
        /// <summary>
        /// Turns on or off the hex editor autosave feature
        /// </summary>
        public bool JsonEditAutoSaveFlag { get; set; } = true;
        /// <summary>
        /// If autosave is enabled, the binary file will be saved for each "n" seconds
        /// </summary>
        public int JsonEditAutoSaveInterval
        {
            get => JsonShellCommon.autoSaveInterval;
            set => JsonShellCommon.autoSaveInterval = value < 0 ? 60 : value;
        }
        /// <summary>
        /// Selects the default JSON formatting (beautified or minified) for the JSON shell to save
        /// </summary>
        public int JsonShellFormatting { get; set; } = (int)Formatting.Indented;
    }
}
