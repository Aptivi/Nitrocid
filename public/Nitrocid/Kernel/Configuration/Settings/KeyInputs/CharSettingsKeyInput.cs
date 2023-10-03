
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
            // We're dealing with integers
            DebugWriter.WriteDebug(DebugLevel.I, "Answer is not numeric and key is of the String or Char (inferred from keytype {0}) type. Setting variable...", key.Type.ToString());

            // Check to see if written answer is empty
            if (value is not string AnswerString)
                return;

            // Check to see if the user intended to clear the variable to make it consist of nothing
            if (AnswerString.ToLower() == "/clear")
            {
                DebugWriter.WriteDebug(DebugLevel.I, "User requested clear.");
                AnswerString = "";
            }

            // Set the value
            SettingsAppTools.SetPropertyValue(key.Variable, AnswerString, configType);
        }

    }
}
