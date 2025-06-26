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
            var PressedKey = default(ConsoleKeyInfo);
            int CurrentValue = Convert.ToInt32(KeyDefaultValue);

            // Make an introductory banner
            string keyName = Translate.DoTranslation(key.Name);
            string keyDesc = Translate.DoTranslation(key.Description);
            while (PressedKey.Key != ConsoleKey.Enter)
            {
                // Show the current value
                double slider = 100d * (CurrentValue / (double)key.MaximumValue);
                InfoBoxProgressColor.WriteInfoBoxProgress(keyName, slider, $"{keyDesc}\n\n" + Translate.DoTranslation("Current value:") + " {0} / {1} - {2}", CurrentValue, key.MinimumValue, key.MaximumValue);

                // Parse the user input
                PressedKey = Input.ReadKey();
                switch (PressedKey.Key)
                {
                    case ConsoleKey.Home:
                        CurrentValue = key.MinimumValue;
                        break;
                    case ConsoleKey.End:
                        CurrentValue = key.MaximumValue;
                        break;
                    case ConsoleKey.PageUp:
                        if (CurrentValue > key.MinimumValue)
                        {
                            CurrentValue -=
                                PressedKey.Modifiers.HasFlag(ConsoleModifiers.Shift) ? 100000 :
                                PressedKey.Modifiers.HasFlag(ConsoleModifiers.Control) ? 10000 : 1000;
                            if (CurrentValue < key.MinimumValue)
                                CurrentValue = key.MinimumValue;
                        }
                        break;
                    case ConsoleKey.PageDown:
                        if (CurrentValue < key.MaximumValue)
                        {
                            CurrentValue +=
                                PressedKey.Modifiers.HasFlag(ConsoleModifiers.Shift) ? 100000 :
                                PressedKey.Modifiers.HasFlag(ConsoleModifiers.Control) ? 10000 : 1000;
                            if (CurrentValue > key.MaximumValue)
                                CurrentValue = key.MaximumValue;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (CurrentValue > key.MinimumValue)
                        {
                            CurrentValue -=
                                PressedKey.Modifiers.HasFlag(ConsoleModifiers.Shift) ? 100 :
                                PressedKey.Modifiers.HasFlag(ConsoleModifiers.Control) ? 10 : 1;
                            if (CurrentValue < key.MinimumValue)
                                CurrentValue = key.MinimumValue;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (CurrentValue < key.MaximumValue)
                        {
                            CurrentValue +=
                                PressedKey.Modifiers.HasFlag(ConsoleModifiers.Shift) ? 100 :
                                PressedKey.Modifiers.HasFlag(ConsoleModifiers.Control) ? 10 : 1;
                            if (CurrentValue > key.MaximumValue)
                                CurrentValue = key.MaximumValue;
                        }
                        break;
                    case ConsoleKey.Enter:
                        ConsoleWrapper.CursorVisible = true;
                        break;
                }
            }

            // Neutralize path if required with the assumption that the keytype is not list
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
