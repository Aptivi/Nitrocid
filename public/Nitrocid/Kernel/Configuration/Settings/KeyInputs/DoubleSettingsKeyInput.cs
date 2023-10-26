//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Configuration.Instances;
using KS.Kernel.Debugging;
using KS.Languages;

namespace KS.Kernel.Configuration.Settings.KeyInputs
{
    internal class DoubleSettingsKeyInput : ISettingsKeyInput
    {
        public object PromptForSet(SettingsKey key, object KeyDefaultValue, out bool bail)
        {
            ConsoleWrapper.Clear();

            // Make an introductory banner
            string keyName = Translate.DoTranslation(key.Name);
            string keyDesc = Translate.DoTranslation(key.Description);
            string finalSection = SettingsApp.RenderHeader(keyName, keyDesc);
            TextWriterColor.WriteKernelColor(finalSection + "\n", true, KernelColorType.Question);

            // Write the prompt
            TextWriterColor.WriteKernelColor("[{0}] > ", false, KernelColorType.Input, KeyDefaultValue);
            string AnswerString = Input.ReadLine();

            // Neutralize path if required with the assumption that the keytype is not list
            double answer = 0;
            DebugWriter.WriteDebug(DebugLevel.I, "User answered {0}", AnswerString);
            bail = !string.IsNullOrEmpty(AnswerString) && double.TryParse(AnswerString, out answer);
            return answer;
        }

        public void SetValue(SettingsKey key, object value, BaseKernelConfig configType)
        {
            // We're dealing with doubles
            DebugWriter.WriteDebug(DebugLevel.I, "Answer is numeric and key is of the double precision floating point type.");
            if (value is not double number)
                return;
            if (number >= 0.0d)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Setting variable {0} to {1}...", key.Variable, number);

                // Now, set the value
                SettingsAppTools.SetPropertyValue(key.Variable, number, configType);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Negative values are disallowed.");
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("The answer may not be negative."), true, KernelColorType.Error);
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Press any key to go back."), true, KernelColorType.Error);
                Input.DetectKeypress();
            }
        }

    }
}
