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
using Nitrocid.Base.Kernel.Debugging;
using Terminaux.Shell.Prompts;

namespace Nitrocid.Base.Kernel.Configuration.Settings.KeyInputs
{
    internal class PresetSettingsKeyInput : ISettingsKeyInput
    {
        public object? PromptForSet(SettingsKey key, object? KeyDefaultValue, BaseKernelConfig configType, out bool bail)
        {
            string selectedPreset = PromptPresetManager.PromptForPresets(key.ShellType);

            // Bail and return
            bail = true;
            return selectedPreset;
        }

        public object? TranslateStringValue(SettingsKey key, string value)
        {
            var presets = PromptPresetManager.GetAllPresetsFromShell(key.ShellType);
            if (presets.ContainsKey(value))
                return value;
            return PromptPresetManager.GetCurrentPresetBaseFromShell(key.ShellType);
        }

        public object? TranslateStringValueWithDefault(SettingsKey key, string value, object? KeyDefaultValue)
        {
            var presets = PromptPresetManager.GetAllPresetsFromShell(key.ShellType);
            if (presets.ContainsKey(value))
                return value;
            return
                KeyDefaultValue is string defaultPreset ?
                defaultPreset :
                PromptPresetManager.GetCurrentPresetBaseFromShell(key.ShellType);
        }

        public void SetValue(SettingsKey key, object? value, BaseKernelConfig configType)
        {
            // We're dealing with presets
            DebugWriter.WriteDebug(DebugLevel.I, "Answer is not numeric and key is of the Preset type. Setting variable...");

            // Check to see if written answer is empty
            if (value is not string presetName)
                return;

            // Set the value
            SettingsAppTools.SetPropertyValue(key.Variable, presetName, configType);
        }
    }
}
