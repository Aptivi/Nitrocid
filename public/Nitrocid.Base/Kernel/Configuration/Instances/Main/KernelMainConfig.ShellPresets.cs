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

using Terminaux.Shell.Prompts;

namespace Nitrocid.Base.Kernel.Configuration.Instances
{
    /// <summary>
    /// Main kernel configuration instance
    /// </summary>
    public partial class KernelMainConfig : BaseKernelConfig
    {
        /// <summary>
        /// Prompt Preset
        /// </summary>
        public string PromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell("Shell").PresetName;
            set => PromptPresetManager.SetPreset(value, "Shell", false);
        }
        /// <summary>
        /// Text Edit Prompt Preset
        /// </summary>
        public string TextEditPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell("TextShell").PresetName;
            set => PromptPresetManager.SetPreset(value, "TextShell", false);
        }
        /// <summary>
        /// Hex Edit Prompt Preset
        /// </summary>
        public string HexEditPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell("HexShell").PresetName;
            set => PromptPresetManager.SetPreset(value, "HexShell", false);
        }
        /// <summary>
        /// Admin Shell Prompt Preset
        /// </summary>
        public string AdminShellPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell("AdminShell").PresetName;
            set => PromptPresetManager.SetPreset(value, "AdminShell", false);
        }
        /// <summary>
        /// Debug Shell Prompt Preset
        /// </summary>
        public string DebugShellPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell("DebugShell").PresetName;
            set => PromptPresetManager.SetPreset(value, "DebugShell", false);
        }
    }
}
