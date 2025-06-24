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
using Nitrocid.Base.Kernel.Configuration.Settings;
using Nitrocid.Base.Kernel.Extensions;
using Nitrocid.Base.Languages;
using Terminaux.Inputs.Styles.Infobox;

namespace Nitrocid.Base.Kernel.Configuration.Settings.KeyInputs
{
    internal class IconSettingsKeyInput : ISettingsKeyInput
    {
        public object? PromptForSet(SettingsKey key, object? KeyDefaultValue, BaseKernelConfig configType, out bool bail)
        {
            if (AddonTools.GetAddon(InterAddonTranslations.GetAddonName(KnownAddons.ExtrasImagesIcons)) is null)
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_ICON_NEEDSADDON"));
                bail = true;
                return KeyDefaultValue;
            }
            string defaultValue = (string?)KeyDefaultValue ?? "heart-suit";
            var type = InterAddonTools.GetTypeFromAddon(KnownAddons.ExtrasImagesIcons, "Nitrocid.Extras.Images.Icons.Tools.IconsTools");
            var hasIcon = (bool?)InterAddonTools.ExecuteCustomAddonFunction(KnownAddons.ExtrasImagesIcons, "HasIcon", type, defaultValue) ?? false;
            defaultValue = hasIcon ? defaultValue : "heart-suit";
            string icon = (string?)InterAddonTools.ExecuteCustomAddonFunction(KnownAddons.ExtrasImagesIcons, "PromptForIcons", type, defaultValue) ?? "heart-suit";

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
