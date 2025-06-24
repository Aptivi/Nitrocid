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

using Textify.General;
using Terminaux.Base;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Configuration.Settings;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Kernel.Configuration.Instances;

namespace Nitrocid.Base.Kernel.Configuration.Settings.KeyInputs
{
    internal class CharSettingsKeyInput : ISettingsKeyInput
    {
        public object? PromptForSet(SettingsKey key, object? KeyDefaultValue, BaseKernelConfig configType, out bool bail)
        {
            ConsoleWrapper.Clear();

            // Translate the key name and description
            string keyName = LanguageTools.GetLocalized(key.Name);
            string keyDesc = LanguageTools.GetLocalized(key.Description);

            // Write the prompt
            var settings = new InfoBoxSettings()
            {
                Title = keyName,
            };
            string? AnswerString = InfoBoxInputColor.WriteInfoBoxInputChar($"{keyDesc}\n\n{LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TEXT")} [{KeyDefaultValue}]", settings);

            // Neutralize path if required with the assumption that the keytype is not list
            DebugWriter.WriteDebug(DebugLevel.I, "User answered {0}", vars: [AnswerString]);
            bail = !string.IsNullOrEmpty(AnswerString);
            char character = string.IsNullOrEmpty(AnswerString) ? '\0' : AnswerString[0];
            if (CharManager.IsControlChar(character))
                character = '\0';
            return character;
        }

        public object? TranslateStringValue(SettingsKey key, string value)
        {
            char character = value.Length == 0 ? '\0' : value[0];
            return character;
        }

        public object? TranslateStringValueWithDefault(SettingsKey key, string value, object? KeyDefaultValue)
        {
            if (KeyDefaultValue is not char defaultChar)
                return '\0';
            char character = value.Length == 0 ? defaultChar : value[0];
            return character;
        }

        public void SetValue(SettingsKey key, object? value, BaseKernelConfig configType)
        {
            // We're dealing with characters
            DebugWriter.WriteDebug(DebugLevel.I, "Answer is not numeric and key is of the Char (inferred from keytype {0}) type. Setting variable...", vars: [key.Type.ToString()]);

            // Check to see if written answer is empty
            char finalValue = value is char charValue ? charValue : '\0';
            if (value is not char)
            {
                // It could be a string, so check that
                if (value is not string charString)
                    return;
                if (charString.Length < 0)
                    return;
                finalValue = charString[0];
            }

            // Set the value
            var property = PropertyManager.GetProperty(key.Variable, configType.GetType());
            if (property is null)
                return;
            if (property.PropertyType == typeof(char))
                SettingsAppTools.SetPropertyValue(key.Variable, finalValue, configType);
            else
                SettingsAppTools.SetPropertyValue(key.Variable, finalValue.ToString(), configType);
        }

    }
}
