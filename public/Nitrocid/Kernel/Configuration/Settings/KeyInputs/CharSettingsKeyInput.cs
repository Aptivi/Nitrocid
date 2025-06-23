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
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Configuration.Instances;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using System;
using Textify.General;
using Terminaux.Base;
using Terminaux.Inputs;

namespace Nitrocid.Kernel.Configuration.Settings.KeyInputs
{
    internal class CharSettingsKeyInput : ISettingsKeyInput
    {
        public object? PromptForSet(SettingsKey key, object? KeyDefaultValue, BaseKernelConfig configType, out bool bail)
        {
            ConsoleWrapper.Clear();

            // Make an introductory banner
            string keyName = key.Name;
            string keyDesc = key.Description;
            string finalSection = SettingsApp.RenderHeader(keyName, keyDesc);
            TextWriterColor.Write(finalSection + "\n", true, ThemeColorType.Question);

            // Write the prompt
            TextWriterColor.Write($"{LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_PRESSLETTER")}\n", ThemeColorType.Tip);
            TextWriterColor.Write("[{0}] ", false, ThemeColorType.Input, KeyDefaultValue ?? ' ');
            var keypressTerm = Input.ReadKey();
            var keypress = keypressTerm.KeyChar;
            keypress =
                CharManager.IsControlChar(keypress) ? '\0' :
                keypressTerm.Key == ConsoleKey.Enter ? '\0' :
                keypress;
            string AnswerString = Convert.ToString(keypress);

            // Neutralize path if required with the assumption that the keytype is not list
            DebugWriter.WriteDebug(DebugLevel.I, "User answered {0}", vars: [keypress]);
            bail = !string.IsNullOrEmpty(AnswerString);
            return keypress;
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
            SettingsAppTools.SetPropertyValue(key.Variable, value is char ? finalValue : finalValue.ToString(), configType);
        }

    }
}
