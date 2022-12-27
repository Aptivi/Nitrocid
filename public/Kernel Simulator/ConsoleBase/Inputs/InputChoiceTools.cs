
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

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
        public static List<InputChoiceInfo> GetInputChoices(string AnswersStr, string[] AnswersTitles)
        {
            // Variables
            var answers = AnswersStr.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var finalAnswers = new List<string>();
            var finalChoices = new List<InputChoiceInfo>();

            // Check to see if the answer titles are the same
            if (answers.Length != AnswersTitles.Length)
                Array.Resize(ref AnswersTitles, answers.Length);

            // Populate answers to final list for below operation
            foreach (var _answer in answers)
                finalAnswers.Add(_answer);

            // Now, populate choice information from the arrays
            for (int i = 0; i < finalAnswers.Count; i++)
                finalChoices.Add(new InputChoiceInfo(finalAnswers[i], AnswersTitles[i]));
            return finalChoices;
        }
    }
}
