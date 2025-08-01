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

using Nitrocid.Base.Kernel.Configuration.Instances;
using Nitrocid.Base.Languages;
using System.Linq;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;

namespace Nitrocid.Base.Kernel.Configuration.Settings.KeyInputs
{
    internal class MultivarSettingsKeyInput : ISettingsKeyInput
    {
        public object? PromptForSet(SettingsKey key, object? KeyDefaultValue, BaseKernelConfig configType, out bool bail)
        {
            // Get the settings keys from this key and prompt for it
            var keys = key.Variables;
            bool bailLoop = false;
            while (!bailLoop)
            {
                // Prompt for it
                var keysChoices = keys.Select((sk, idx) =>
                {
                    object? currentValue = sk.Masked ? "***" : ConfigTools.GetValueFromEntry(sk, configType);
                    string finalKeyName = sk.Type == SettingsKeyType.SMultivar ? $"{LanguageTools.GetLocalized(sk.Name)}..." : $"{LanguageTools.GetLocalized(sk.Name)} [{currentValue}]";
                    return new InputChoiceInfo($"{idx + 1}", finalKeyName, LanguageTools.GetLocalized(sk.Description));
                }).ToList();
                keysChoices.Add(new($"{keysChoices.Count + 1}", LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_COMMON_EXIT")));
                int choiceIdx = InfoBoxSelectionColor.WriteInfoBoxSelection([.. keysChoices], LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_MULTIVAR_CHOOSE") + $" \"{LanguageTools.GetLocalized(key.Name)}\"");

                // Check to see if exit is requested
                if (choiceIdx < 0)
                {
                    bail = false;
                    return null;
                }
                else if (choiceIdx == keysChoices.Count - 1)
                {
                    bailLoop = true;
                    continue;
                }

                // Now, get the settings key from the master key and set the value as-is
                var selectedKey = keys[choiceIdx];
                var selectedKeyDefaultValue = ConfigTools.GetValueFromEntry(selectedKey, configType);
                var setValue = selectedKey.KeyInput.PromptForSet(selectedKey, selectedKeyDefaultValue, configType, out bool set);
                if (set)
                    selectedKey.KeyInput.SetValue(selectedKey, setValue, configType);
            }

            bail = true;
            return null;
        }

        public object? TranslateStringValue(SettingsKey key, string value) =>
            null;

        public object? TranslateStringValueWithDefault(SettingsKey key, string value, object? KeyDefaultValue) =>
            null;

        public void SetValue(SettingsKey key, object? value, BaseKernelConfig configType)
        {
            // We'll do nothing, since PromptForSet already sets the necessary value
            return;
        }

    }
}
