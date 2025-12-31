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

using Terminaux.Inputs.Styles.Infobox;
using System;
using System.Collections.Generic;
using System.Linq;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Kernel.Configuration.Instances;

namespace Nitrocid.Base.Kernel.Configuration.Settings.KeyInputs
{
    internal class ListSettingsKeyInput : ISettingsKeyInput
    {
        public object? PromptForSet(SettingsKey key, object? KeyDefaultValue, BaseKernelConfig configType, out bool bail)
        {
            // Make an introductory banner
            string keyName = LanguageTools.GetLocalized(key.Name);
            string keyDesc = LanguageTools.GetLocalized(key.Description);

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
                    new InputChoiceInfo($"{choices.Count + 1}", LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_COMMON_EXIT")),
                ];

                // Wait for an answer and handle it
                int selectionAnswer = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choices, .. altChoices], keyDesc, new InfoBoxSettings()
                {
                    Title = keyName,
                });
                if (selectionAnswer == choices.Count || selectionAnswer == -1)
                    promptBail = true;
                else
                {
                    // Tell the user to choose between adding, removing, or exiting
                    int result = InfoBoxButtonsColor.WriteInfoBoxButtons(
                        [
                            new InputChoiceInfo("keep", LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_LIST_KEEP")),
                            new InputChoiceInfo("remove", LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_LIST_REMOVE")),
                            new InputChoiceInfo("add", LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_LIST_ADD")),
                        ],
                        LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_LIST_PROMPT")
                    ) + 1;

                    // Check the action number
                    if (result >= 1 && result <= 3)
                    {
                        // Depending on the action, select whether to add, remove, or keep
                        if (result == 2)
                        {
                            // Removing item
                            TargetList.RemoveAt(selectionAnswer);
                        }
                        else if (result == 3)
                        {
                            // Adding new item
                            string newItemValue = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_LIST_ENTERVALUE"));
                            TargetList.Add(newItemValue);
                        }
                    }
                    else
                        InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_LIST_INVALIDACTION"));
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
