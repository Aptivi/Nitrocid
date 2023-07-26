
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

using System;
using KS.ConsoleBase.Inputs.Styles;
using KS.Kernel.Debugging;

namespace KS.Shell.ShellBase.Scripting.Interaction
{
    /// <summary>
    /// UESH scripting command functions
    /// </summary>
    public static class UESHCommands
    {

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="ScriptVariable">A $variable</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static void PromptChoiceAndSet(string Question, string ScriptVariable, string AnswersStr, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) => PromptChoiceAndSet(Question, ScriptVariable, AnswersStr, Array.Empty<string>(), OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="ScriptVariable">A $variable</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static void PromptChoiceAndSet(string Question, string ScriptVariable, string AnswersStr, string[] AnswersTitles, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false)
        {
            string Answer = ChoiceStyle.PromptChoice(Question, AnswersStr, AnswersTitles, OutputType, PressEnter);
            UESHVariables.SetVariable(ScriptVariable, Answer);
        }

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="ScriptVariable">A $variable</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        public static void PromptSelectionAndSet(string Question, string ScriptVariable, string AnswersStr) => PromptSelectionAndSet(Question, ScriptVariable, AnswersStr, Array.Empty<string>());

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="ScriptVariable">A $variable</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        public static void PromptSelectionAndSet(string Question, string ScriptVariable, string AnswersStr, string[] AnswersTitles)
        {
            int SelectedAnswer = SelectionStyle.PromptSelection(Question, AnswersStr, AnswersTitles);

            // Set the value
            if (SelectedAnswer == -1)
                UESHVariables.SetVariable(ScriptVariable, SelectedAnswer.ToString());
        }

        /// <summary>
        /// Prompts user for input (answer the question with your own answers)
        /// </summary>
        /// <param name="Question">The question to ask</param>
        /// <param name="ScriptVariable">An $variable</param>
        public static void PromptInputAndSet(string Question, string ScriptVariable)
        {
            // Variables
            string Answer = InputStyle.PromptInput(Question);
            DebugWriter.WriteDebug(DebugLevel.I, "Script var: {0} ({1})", ScriptVariable, UESHVariables.ShellVariables.ContainsKey(ScriptVariable));
            DebugWriter.WriteDebug(DebugLevel.I, "Setting to {0}...", Answer);
            UESHVariables.SetVariable(ScriptVariable, Answer);
        }

    }
}
