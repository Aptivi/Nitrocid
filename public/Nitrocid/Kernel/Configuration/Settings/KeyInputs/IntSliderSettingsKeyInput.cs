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

using Nitrocid.ConsoleBase.Colors;
using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Configuration.Instances;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using System;
using Terminaux.Base;
using Terminaux.Inputs;

namespace Nitrocid.Kernel.Configuration.Settings.KeyInputs
{
    internal class IntSliderSettingsKeyInput : ISettingsKeyInput
    {
        public object? PromptForSet(SettingsKey key, object? KeyDefaultValue, BaseKernelConfig configType, out bool bail)
        {
            int CurrentValue = Convert.ToInt32(KeyDefaultValue);

            // Make an introductory banner
            string keyName = key.Name;
            string keyDesc = key.Description;

            CurrentValue = InfoBoxSliderColor.WriteInfoBoxSlider(keyName, CurrentValue, key.MaximumValue, keyDesc, key.MinimumValue);

            // Bail and use selected value
            bail = true;
            return CurrentValue;
        }

        public object? TranslateStringValue(SettingsKey key, string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;
            if (int.TryParse(value, out int answer))
                return answer >= key.MinimumValue && answer <= key.MaximumValue ? answer : 0;
            return 0;
        }

        public object? TranslateStringValueWithDefault(SettingsKey key, string value, object? KeyDefaultValue)
        {
            if (string.IsNullOrEmpty(value))
                return (int?)KeyDefaultValue;
            if (int.TryParse(value, out int answer))
                return answer >= key.MinimumValue && answer <= key.MaximumValue ? answer : (int?)KeyDefaultValue;
            return (int?)KeyDefaultValue;
        }

        public void SetValue(SettingsKey key, object? value, BaseKernelConfig configType)
        {
            // We're dealing with integers with limits
            if (value is not int AnswerInt)
                return;
            DebugWriter.WriteDebug(DebugLevel.I, "Setting variable {0} to {1}...", key.Variable, AnswerInt);
            SettingsAppTools.SetPropertyValue(key.Variable, AnswerInt, configType);
        }

    }
}
