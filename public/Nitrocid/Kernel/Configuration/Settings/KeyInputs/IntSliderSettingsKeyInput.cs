
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Kernel.Configuration.Instances;
using KS.Kernel.Debugging;
using KS.Languages;
using System;

namespace KS.Kernel.Configuration.Settings.KeyInputs
{
    internal class IntSliderSettingsKeyInput : ISettingsKeyInput
    {
        public object PromptForSet(SettingsKey key, object KeyDefaultValue, out bool bail)
        {
            var PressedKey = default(ConsoleKey);
            int CurrentValue = Convert.ToInt32(KeyDefaultValue);
            ConsoleWrapper.CursorVisible = false;
            ConsoleWrapper.Clear();

            // Make an introductory banner
            string keyName = Translate.DoTranslation(key.Name);
            string keyDesc = Translate.DoTranslation(key.Description);
            string finalSection = SettingsApp.RenderHeader(keyName, keyDesc);
            TextWriterColor.WriteKernelColor(finalSection, true, KernelColorType.Question);
            while (PressedKey != ConsoleKey.Enter)
            {
                // Draw the progress bar
                ProgressBarColor.WriteProgress(100d * (CurrentValue / (double)key.MaximumValue), 4, ConsoleWrapper.WindowHeight - 4);

                // Show the current value
                TextWriterWhereColor.WriteWhereKernelColor(Translate.DoTranslation("Current value:") + " {0} / {1} - {2}" + $"{ConsoleExtensions.GetClearLineToRightSequence()}", 5, ConsoleWrapper.WindowHeight - 5, false, KernelColorType.NeutralText, CurrentValue, key.MinimumValue, key.MaximumValue);

                // Parse the user input
                PressedKey = Input.DetectKeypress().Key;
                switch (PressedKey)
                {
                    case ConsoleKey.LeftArrow:
                        if (CurrentValue > key.MinimumValue)
                            CurrentValue -= 1;
                        break;
                    case ConsoleKey.RightArrow:
                        if (CurrentValue < key.MaximumValue)
                            CurrentValue += 1;
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

        public void SetValue(SettingsKey key, object value, BaseKernelConfig configType)
        {
            // We're dealing with integers with limits
            if (value is not int AnswerInt)
                return;
            DebugWriter.WriteDebug(DebugLevel.I, "Setting variable {0} to {1}...", key.Variable, AnswerInt);
            SettingsAppTools.SetPropertyValue(key.Variable, AnswerInt, configType);
        }

    }
}
