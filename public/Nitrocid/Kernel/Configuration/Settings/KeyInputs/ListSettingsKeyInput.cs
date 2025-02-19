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

using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Selection;
using Nitrocid.Kernel.Configuration.Instances;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using Terminaux.Base;
using Terminaux.Inputs.Styles;

namespace Nitrocid.Kernel.Configuration.Settings.KeyInputs
{
    internal class ListSettingsKeyInput : ISettingsKeyInput
    {
        public object? PromptForSet(SettingsKey key, object? KeyDefaultValue, BaseKernelConfig configType, out bool bail)
        {
            ConsoleWrapper.Clear();

            // Make an introductory banner
            string keyName = key.Name;
            string keyDesc = key.Description;
            string finalSection = SettingsApp.RenderHeader(keyName, keyDesc);

            // Write the prompt
            var arguments = SettingsAppTools.ParseParameters(key);
            var type = Type.GetType(key.SelectionFunctionType);
            var TargetEnum =
                type is not null ?
                (IEnumerable<object>?)MethodManager.InvokeMethodStatic(key.SelectionFunctionName, type, args: arguments) :
                (IEnumerable<object>?)MethodManager.InvokeMethodStatic(key.SelectionFunctionName, args: arguments);
            var TargetList = TargetEnum?.ToList() ?? [];
            bool promptBail = false;
            while (!promptBail)
            {
                List<InputChoiceInfo> choices = [];

                // Populate input choices
                int targetNum = 1;
                foreach (var target in TargetList)
                {
                    if (target is string targetStr)
                        choices.Add(new InputChoiceInfo($"{targetNum}", targetStr));
                    else
                        choices.Add(new InputChoiceInfo($"{targetNum}", target?.ToString() ?? ""));
                    targetNum++;
                }
                List<InputChoiceInfo> altChoices =
                [
                    new InputChoiceInfo($"{choices.Count + 1}", Translate.DoTranslation("Exit")),
                ];

                // Wait for an answer and handle it
                int selectionAnswer = SelectionStyle.PromptSelection(finalSection, [.. choices], [.. altChoices], true);
                if (selectionAnswer == choices.Count + 1)
                    promptBail = true;
                else
                {
                    // Tell the user to choose between adding, removing, or exiting
                    int selectedItemIdx = selectionAnswer - 1;
                    int result = InfoBoxButtonsColor.WriteInfoBoxButtons(
                        [
                            new InputChoiceInfo("keep", Translate.DoTranslation("Keep it")),
                            new InputChoiceInfo("remove", Translate.DoTranslation("Remove it")),
                            new InputChoiceInfo("add", Translate.DoTranslation("Add new item")),
                        ],
                        Translate.DoTranslation("What do you want to do with this item?")
                    ) + 1;

                    // Check the action number
                    if (result >= 1 && result <= 3)
                    {
                        // Depending on the action, select whether to add, remove, or keep
                        if (result == 2)
                        {
                            // Removing item
                            TargetList.RemoveAt(selectedItemIdx);
                        }
                        else if (result == 3)
                        {
                            // Adding new item
                            string newItemValue = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a value for the new item"));
                            TargetList.Add(newItemValue);
                        }
                    }
                    else
                        InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("Invalid action selected."));
                }
            }
            bail = true;
            return TargetList;
        }

        public object? TranslateStringValue(SettingsKey key, string value)
        {
            string FinalDelimiter = GetFinalDelimiter(key);

            // Now, split the value with this delimiter
            var values = value.Split(FinalDelimiter) as IEnumerable<object>;
            return values;
        }

        public object? TranslateStringValueWithDefault(SettingsKey key, string value, object? KeyDefaultValue)
        {
            string FinalDelimiter = GetFinalDelimiter(key);

            // Now, split the value with this delimiter
            var values =
                !string.IsNullOrEmpty(value) ?
                value.Split(FinalDelimiter) :
                KeyDefaultValue as IEnumerable<object>;
            return values;
        }

        public void SetValue(SettingsKey key, object? value, BaseKernelConfig configType)
        {
            string FinalDelimiter = GetFinalDelimiter(key);

            // Now, set the value
            string joinedString = value is string stringValue ? stringValue : "";
            if (value is IEnumerable<object> valueList)
                joinedString = string.Join(FinalDelimiter, valueList);
            SettingsAppTools.SetPropertyValue(key.Variable, joinedString, configType);
        }

        private string GetFinalDelimiter(SettingsKey key)
        {
            string? FinalDelimiter;
            string ListJoinString = key.Delimiter;
            string ListJoinStringVariable = key.DelimiterVariable;
            var type = Type.GetType(key.DelimiterVariableType);
            DebugWriter.WriteDebug(DebugLevel.I, "Answer is not numeric and key is of the List type. Adding answers to the list...");

            // Get the delimiter
            if (string.IsNullOrEmpty(ListJoinString))
                FinalDelimiter = Convert.ToString(PropertyManager.GetPropertyValue(ListJoinStringVariable, type));
            else
                FinalDelimiter = ListJoinString;
            return FinalDelimiter ?? ";";
        }

    }
}
