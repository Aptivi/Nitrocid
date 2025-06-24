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

using Terminaux.Colors.Themes.Colors;
using System.Collections.Generic;
using System.Linq;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Inputs.Styles;
using Nitrocid.Base.Kernel.Configuration.Settings;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Kernel.Configuration.Instances;

namespace Nitrocid.Base.Kernel.Configuration.Settings.KeyInputs
{
    internal class ColorSettingsKeyInput : ISettingsKeyInput
    {
        public object? PromptForSet(SettingsKey key, object? KeyDefaultValue, BaseKernelConfig configType, out bool bail)
        {
            Color keyColorValue = Color.Empty;

            // Check to see if the color is contained in the dictionary
            if (KeyDefaultValue is KeyValuePair<ThemeColorType, Color> keyColorValuePair)
                keyColorValue = keyColorValuePair.Value;
            else if (KeyDefaultValue is string keyColorString)
                keyColorValue = new Color(keyColorString);

            // Get the color value from the color wheel
            ColorTools.LoadBackDry(new Color(ConsoleColors.Black));
            string ColorValue = ColorSelector.OpenColorSelector(keyColorValue).PlainSequence;

            // Bail and return
            bail = true;
            return ColorValue;
        }

        public object? TranslateStringValue(SettingsKey key, string value)
        {
            var color = new Color(value);
            return color.PlainSequence;
        }

        public object? TranslateStringValueWithDefault(SettingsKey key, string value, object? KeyDefaultValue)
        {
            Color keyColorValue = Color.Empty;

            // Check to see if the color is contained in the dictionary
            if (KeyDefaultValue is KeyValuePair<ThemeColorType, Color> keyColorValuePair)
                keyColorValue = keyColorValuePair.Value;
            else if (KeyDefaultValue is string keyColorString)
                keyColorValue = new Color(keyColorString);

            if (!string.IsNullOrEmpty(value) && ColorTools.TryParseColor(value))
                keyColorValue = new Color(value);
            return keyColorValue.PlainSequence;
        }

        public void SetValue(SettingsKey key, object? value, BaseKernelConfig configType)
        {
            // We're dealing with integers with limits
            if (value is not string colorValue)
                return;
            object FinalColor;

            // key.Variable is not always KernelColors, which is a dictionary. This applies to standard settings. Everything else should
            // be either the Color type or a String type.
            if (PropertyManager.CheckProperty(key.Variable) &&
                SettingsAppTools.GetPropertyValue(key.Variable, configType) is Dictionary<ThemeColorType, Color> colors2)
            {
                var colorTypeOnDict = colors2.ElementAt(key.EnumerableIndex).Key;
                colors2[colorTypeOnDict] = new Color(colorValue);
                FinalColor = colors2;
            }
            else if (PropertyManager.CheckProperty(key.Variable) &&
                     PropertyManager.GetProperty(key.Variable)?.PropertyType == typeof(Color))
            {
                FinalColor = new Color(colorValue);
            }
            else
            {
                FinalColor = colorValue;
            }

            // Now, set the value
            SettingsAppTools.SetPropertyValue(key.Variable, FinalColor, configType);
        }

    }
}
