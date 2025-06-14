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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Configuration.Instances;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Terminaux.Base;
using Terminaux.Inputs;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;

namespace Nitrocid.Kernel.Configuration.Settings.KeyInputs
{
    internal class DoubleSettingsKeyInput : ISettingsKeyInput
    {
        public object? PromptForSet(SettingsKey key, object? KeyDefaultValue, BaseKernelConfig configType, out bool bail)
        {
            ConsoleWrapper.Clear();

            // Make an introductory banner
            string keyName = key.Name;
            string keyDesc = key.Description;
            string finalSection = SettingsApp.RenderHeader(keyName, keyDesc);
            TextWriters.Write(finalSection + "\n", true, KernelColorType.Question);

            // Write the prompt
            string AnswerString = InfoBoxInputColor.WriteInfoBoxInput($"{LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_WRITEFLOATINGNUM")}: 0.0 [{KeyDefaultValue}]", new InfoBoxSettings()
            {
                Title = keyName,
            });

            // Neutralize path if required with the assumption that the keytype is not list
            double answer = 0;
            DebugWriter.WriteDebug(DebugLevel.I, "User answered {0}", vars: [AnswerString]);
            bail = !string.IsNullOrEmpty(AnswerString) && double.TryParse(AnswerString, out answer);
            return answer;
        }

        public object? TranslateStringValue(SettingsKey key, string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0d;
            if (double.TryParse(value, out double answer))
                return answer;
            return 0d;
        }

        public object? TranslateStringValueWithDefault(SettingsKey key, string value, object? KeyDefaultValue)
        {
            if (string.IsNullOrEmpty(value))
                return (double?)KeyDefaultValue;
            if (double.TryParse(value, out double answer))
                return answer;
            return (double?)KeyDefaultValue;
        }

        public void SetValue(SettingsKey key, object? value, BaseKernelConfig configType)
        {
            // We're dealing with doubles
            DebugWriter.WriteDebug(DebugLevel.I, "Answer is numeric and key is of the double precision floating point type.");
            if (value is not double number)
                return;
            if (number >= 0.0d)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Setting variable {0} to {1}...", vars: [key.Variable, number]);

                // Now, set the value
                SettingsAppTools.SetPropertyValue(key.Variable, number, configType);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Negative values are disallowed.");
                TextWriters.Write(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_NUMBERPOSITIVE"), true, KernelColorType.Error);
                TextWriters.Write(LanguageTools.GetLocalized("NKS_COMMON_GOBACK"), true, KernelColorType.Error);
                Input.ReadKey();
            }
        }

    }
}
