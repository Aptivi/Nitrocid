//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using KS.Kernel.Debugging;
using System;
using System.Collections.Generic;

namespace KS.ConsoleBase.Inputs
{
    /// <summary>
    /// Input choice tools
    /// </summary>
    public static class InputChoiceTools
    {
        /// <summary>
        /// Gets the input choices
        /// </summary>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        public static List<InputChoiceInfo> GetInputChoices(string AnswersStr, string[] AnswersTitles) =>
            GetInputChoices(AnswersStr.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries), AnswersTitles);

        /// <summary>
        /// Gets the input choices
        /// </summary>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        public static List<InputChoiceInfo> GetInputChoices(string[] Answers, string[] AnswersTitles)
        {
            // Variables
            var finalAnswers = new List<string>();
            var finalChoices = new List<InputChoiceInfo>();

            // Check to see if the answer titles are the same
            DebugWriter.WriteDebug(DebugLevel.I, "Answers: {0}, Titles: {1}", Answers.Length, AnswersTitles.Length);
            if (Answers.Length != AnswersTitles.Length)
                Array.Resize(ref AnswersTitles, Answers.Length);

            // Populate answers to final list for below operation
            foreach (var _answer in Answers)
                finalAnswers.Add(_answer);

            // Now, populate choice information from the arrays
            for (int i = 0; i < finalAnswers.Count; i++)
                finalChoices.Add(new InputChoiceInfo(finalAnswers[i], AnswersTitles[i]));
            DebugWriter.WriteDebug(DebugLevel.I, "Final choices: {0}", finalChoices.Count);
            return finalChoices;
        }
    }
}
