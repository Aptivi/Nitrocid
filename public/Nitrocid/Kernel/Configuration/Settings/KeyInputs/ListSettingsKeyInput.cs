
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
using KS.ConsoleBase.Inputs.Styles;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Files;
using KS.Files.Folders;
using KS.Kernel.Configuration.Instances;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Text;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KS.Kernel.Configuration.Settings.KeyInputs
{
    internal class ListSettingsKeyInput : ISettingsKeyInput
    {
        public object PromptForSet(SettingsKey key, object KeyDefaultValue, out bool bail)
        {
            ConsoleWrapper.Clear();

            // Make an introductory banner
            string keyName = Translate.DoTranslation(key.Name);
            string keyDesc = Translate.DoTranslation(key.Description);
            string finalSection = SettingsApp.RenderHeader(keyName, keyDesc);

            // Write the prompt
            var TargetEnum = (IEnumerable<object>)MethodManager.GetMethod(key.SelectionFunctionName).Invoke(key.SelectionFunctionType, null);
            var TargetList = TargetEnum.ToList();
            bool promptBail = false;
            while (!promptBail)
            {
                List<InputChoiceInfo> choices = new();

                // Populate input choices
                int targetNum = 1;
                foreach (var target in TargetList)
                {
                    if (target is string targetStr)
                        choices.Add(new InputChoiceInfo($"{targetNum}", targetStr));
                    else
                        choices.Add(new InputChoiceInfo($"{targetNum}", target.ToString()));
                    targetNum++;
                }
                List<InputChoiceInfo> altChoices = new()
                {
                    new InputChoiceInfo($"{choices.Count + 1}", Translate.DoTranslation("Exit")),
                };

                // Wait for an answer and handle it
                int selectionAnswer = SelectionStyle.PromptSelection(finalSection, choices, altChoices, true);
                if (selectionAnswer == choices.Count + 1)
                    promptBail = true;
                else
                {
                    // Tell the user to choose between adding, removing, or exiting
                    int selectedItemIdx = selectionAnswer - 1;
                    var choice = choices[selectedItemIdx];
                    string result = InfoBoxColor.WriteInfoBoxInput(
                        Translate.DoTranslation("What do you want to do with this item?") + CharManager.NewLine +
                        $"{choice.ChoiceName}: {choice.ChoiceTitle}" + CharManager.NewLine + CharManager.NewLine +
                        $"  1) {Translate.DoTranslation("Keep this item")}" + CharManager.NewLine +
                        $"  2) {Translate.DoTranslation("Remove this item")}" + CharManager.NewLine +
                        $"  3) {Translate.DoTranslation("Add new item")}"
                    );

                    // Check the action number
                    if (int.TryParse(result, out int selectedAction) && selectedAction >= 1 && selectedAction <= 3)
                    {
                        // Depending on the action, select whether to add, remove, or keep
                        if (selectedAction == 2)
                        {
                            // Removing item
                            TargetList.RemoveAt(selectedItemIdx);
                        }
                        else if (selectedAction == 3)
                        {
                            // Adding new item
                            string newItemValue = InfoBoxColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a value for the new item"));
                            TargetList.Add(newItemValue);
                        }
                    }
                    else
                        InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Invalid action selected."));
                }
            }
            bail = true;
            return TargetList;
        }

        public void SetValue(SettingsKey key, object value, BaseKernelConfig configType)
        {
            string FinalDelimiter;
            string ListJoinString = key.Delimiter;
            string ListJoinStringVariable = key.DelimiterVariable;
            DebugWriter.WriteDebug(DebugLevel.I, "Answer is not numeric and key is of the List type. Adding answers to the list...");

            // Get the delimiter
            if (ListJoinString is null)
                FinalDelimiter = Convert.ToString(PropertyManager.GetPropertyValue(ListJoinStringVariable, null, true));
            else
                FinalDelimiter = ListJoinString;

            // Now, set the value
            if (value is not IEnumerable<object> valueList)
                return;
            string JoinedString = string.Join(FinalDelimiter, valueList);
            SettingsAppTools.SetPropertyValue(key.Variable, JoinedString, configType);
        }

    }
}
