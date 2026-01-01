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

using Nitrocid.Kernel.Configuration.Instances;
using Terminaux.Images.Icons;

namespace Nitrocid.Kernel.Configuration.Settings.KeyInputs
{
    internal class IconSettingsKeyInput : ISettingsKeyInput
    {
        public object? PromptForSet(SettingsKey key, object? KeyDefaultValue, BaseKernelConfig configType, out bool bail)
        {
            string icon = IconsSelector.PromptForIcons((string?)KeyDefaultValue ?? "heart-suit");

            // Bail and return
            bail = true;
            return icon;
        }

        public object? TranslateStringValue(SettingsKey key, string value) =>
            value;

        public object? TranslateStringValueWithDefault(SettingsKey key, string value, object? KeyDefaultValue) =>
            value;

        public void SetValue(SettingsKey key, object? value, BaseKernelConfig configType)
        {
            // Already set by SetPresetInternal
            if (value is not string icon)
                return;
            SettingsAppTools.SetPropertyValue(key.Variable, icon, configType);
            return;
        }

    }
}
