
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
using KS.Kernel.Configuration.Instances;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Text;
using System;

namespace KS.Kernel.Configuration.Settings.KeyInputs
{
    internal class CharSettingsKeyInput : ISettingsKeyInput
    {
        public object PromptForSet(SettingsKey key, object KeyDefaultValue, out bool bail)
        {
            ConsoleWrapper.Clear();

            // Make an introductory banner
            string finalSection = Translate.DoTranslation(key.Name);
            TextWriterColor.Write("\n  * " + finalSection + CharManager.NewLine + CharManager.NewLine + Translate.DoTranslation(key.Description), true, KernelColorType.Question);

            // Write the prompt
            TextWriterColor.Write("[{0}] > ", false, KernelColorType.Input, KeyDefaultValue);
            string AnswerString = Convert.ToString(Input.DetectKeypress().KeyChar);

            // Neutralize path if required with the assumption that the keytype is not list
            DebugWriter.WriteDebug(DebugLevel.I, "User answered {0}", AnswerString);
            bail = !string.IsNullOrEmpty(AnswerString) && AnswerString.Length == 1;
            return AnswerString;
        }

        public void SetValue(SettingsKey key, object value, BaseKernelConfig configType)
        {
            // We're dealing with boolean
            DebugWriter.WriteDebug(DebugLevel.I, "Answer is numeric and key is of the Boolean type.");
            var FinalBool = true;

            // Set boolean
            switch (value)
            {
                case 0: // False
                    DebugWriter.WriteDebug(DebugLevel.I, "Setting to False...");
                    FinalBool = false;
                    break;
                case 1: // True
                    DebugWriter.WriteDebug(DebugLevel.I, "Setting to True...");
                    FinalBool = true;
                    break;
            }

            // Now, set the value
            SettingsApp.SetPropertyValue(key.Variable, FinalBool, configType);
        }

    }
}
