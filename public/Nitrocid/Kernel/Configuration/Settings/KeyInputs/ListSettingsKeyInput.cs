
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
            string finalSection = Translate.DoTranslation(key.Name);
            TextWriterColor.Write("\n  * " + finalSection + CharManager.NewLine + CharManager.NewLine + Translate.DoTranslation(key.Description), true, KernelColorType.Question);

            // Write the prompt
            var TargetList = (IEnumerable<object>)MethodManager.GetMethod(key.SelectionFunctionName).Invoke(key.SelectionFunctionType, null);
            TextWriterColor.Write(Translate.DoTranslation("Current items:"), true, KernelColorType.ListTitle);
            ListWriterColor.WriteList(TargetList);
            TextWriterColor.Write();
            TextWriterColor.Write(CharManager.NewLine + " q) " + Translate.DoTranslation("Save Changes...") + CharManager.NewLine, true, KernelColorType.Option);

            // Prompt the user and parse the answer
            TextWriterColor.Write("> ", false, KernelColorType.Input);
            string AnswerString = "";
            while (AnswerString != "q")
            {
                AnswerString = Input.ReadLine();
                if (AnswerString != "q")
                {
                    if (key.IsValuePath)
                    {
                        // Neutralize the path as appropriate
                        string NeutralizeRootPath = key.IsPathCurrentPath ? CurrentDirectory.CurrentDir : Paths.GetKernelPath(key.ValuePathType);
                        AnswerString = Filesystem.NeutralizePath(AnswerString, NeutralizeRootPath);
                    }

                    // Check to see if we're removing an item
                    if (!AnswerString.StartsWith("-"))
                    {
                        // We're not removing an item!
                        TargetList = TargetList.Append(AnswerString);
                    }
                    else
                    {
                        // We're removing an item.
                        var DeletedItems = Enumerable.Empty<object>();
                        DeletedItems = DeletedItems.Append(AnswerString[1..]);
                        TargetList = TargetList.Except(DeletedItems);
                    }
                    DebugWriter.WriteDebug(DebugLevel.I, "Added answer {0} to list.", AnswerString);
                    TextWriterColor.Write("> ", false, KernelColorType.Input);
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
            if (value is not IEnumerable<object>)
                return;
            string JoinedString = string.Join(FinalDelimiter, value);
            SettingsApp.SetPropertyValue(key.Variable, JoinedString, configType);
        }

    }
}
