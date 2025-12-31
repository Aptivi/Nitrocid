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

using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel.Configuration.Instances;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Files;
using System;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;

namespace Nitrocid.Base.Kernel.Configuration.Settings.KeyInputs
{
    internal class StringSettingsKeyInput : ISettingsKeyInput
    {
        public object? PromptForSet(SettingsKey key, object? KeyDefaultValue, BaseKernelConfig configType, out bool bail)
        {
            // Translate the key name and description
            string keyName = LanguageTools.GetLocalized(key.Name);
            string keyDesc = LanguageTools.GetLocalized(key.Description);

            // Write the prompt
            var settings = new InfoBoxSettings()
            {
                Title = keyName,
            };
            string? AnswerString =
                key.Masked ?
                InfoBoxInputColor.WriteInfoBoxInput($"{keyDesc}\n\n{LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TEXT")}", settings, InfoBoxInputType.Password) :
                InfoBoxInputColor.WriteInfoBoxInput($"{keyDesc}\n\n{LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TEXT")} [{KeyDefaultValue}]", settings);

            // Neutralize path if required with the assumption that the keytype is not list
            AnswerString = (string?)TranslateStringValueWithDefault(key, AnswerString, KeyDefaultValue);
            bail = true;
            return AnswerString;
        }

        public object? TranslateStringValue(SettingsKey key, string value)
        {
            // Neutralize path if required with the assumption that the keytype is not list
            if (key.IsValuePath)
            {
                string NeutralizeRootPath = key.IsPathCurrentPath ? FilesystemTools.CurrentDir : PathsManagement.GetKernelPath(key.ValuePathType);
                value = FilesystemTools.NeutralizePath(value, NeutralizeRootPath);
            }
            return value;
        }

        public object? TranslateStringValueWithDefault(SettingsKey key, string value, object? KeyDefaultValue)
        {
            // Neutralize path if required with the assumption that the keytype is not list
            if (key.IsValuePath)
            {
                string NeutralizeRootPath = key.IsPathCurrentPath ? FilesystemTools.CurrentDir : PathsManagement.GetKernelPath(key.ValuePathType);
                value = FilesystemTools.NeutralizePath(value, NeutralizeRootPath);
            }

            // Set to default is nothing is written
            if (string.IsNullOrWhiteSpace(value))
            {
                if (KeyDefaultValue is string KeyValue)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Answer is nothing. Setting to {0}...", vars: [KeyValue]);
                    value = Convert.ToString(KeyValue);
                }
            }
            return value;
        }

        public void SetValue(SettingsKey key, object? value, BaseKernelConfig configType)
        {
            // We're dealing with integers
            DebugWriter.WriteDebug(DebugLevel.I, "Answer is not numeric and key is of the String or Char (inferred from keytype {0}) type. Setting variable...", vars: [key.Type.ToString()]);

            // Check to see if written answer is empty
            if (value is not string AnswerString)
                return;

            // Check to see if the user intended to clear the variable to make it consist of nothing
            if (AnswerString.Equals("/clear", StringComparison.OrdinalIgnoreCase))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "User requested clear.");
                AnswerString = "";
            }

            // Set the value
            SettingsAppTools.SetPropertyValue(key.Variable, AnswerString, configType);
        }
    }
}
