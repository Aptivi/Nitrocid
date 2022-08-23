
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
using KS.ConsoleBase.Colors;
using KS.Misc.Writers.ConsoleWriters;

namespace KS.ConsoleBase.Inputs.Styles
{
    public static class SelectionStyle
    {

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        public static int PromptSelection(string Question, string AnswersStr)
        {
            return PromptSelection(Question, AnswersStr, Array.Empty<string>());
        }

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        public static int PromptSelection(string Question, string AnswersStr, string[] AnswersTitles)
        {
            int HighlightedAnswer = 1;
            while (true)
            {
                // Variables
                var answers = AnswersStr.Split('/');
                ConsoleKeyInfo Answer;
                Console.Clear();

                // Check to see if the answer titles are the same
                if (answers.Length != AnswersTitles.Length)
                {
                    Array.Resize(ref AnswersTitles, answers.Length);
                }

                // Ask a question
                TextWriterColor.Write(Question + Kernel.Kernel.NewLine, true, ColorTools.ColTypes.Question);
                for (int AnswerIndex = 0, loopTo = answers.Length - 1; AnswerIndex <= loopTo; AnswerIndex++)
                {
                    string AnswerInstance = answers[AnswerIndex];
                    string AnswerTitle = AnswersTitles[AnswerIndex];
                    TextWriterColor.Write($" {AnswerInstance}) {AnswerTitle}", true, AnswerIndex + 1 == HighlightedAnswer ? ColorTools.ColTypes.SelectedOption : ColorTools.ColTypes.Option);
                }

                // Wait for an answer
                Answer = Console.ReadKey(true);
                Console.WriteLine();

                // Check the answer
                switch (Answer.Key)
                {
                    case ConsoleKey.UpArrow:
                        {
                            HighlightedAnswer -= 1;
                            if (HighlightedAnswer == 0)
                            {
                                HighlightedAnswer = answers.Length;
                            }

                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            if (HighlightedAnswer == answers.Length)
                            {
                                HighlightedAnswer = 0;
                            }
                            HighlightedAnswer += 1;
                            break;
                        }
                    case ConsoleKey.Enter:
                        {
                            return HighlightedAnswer;
                        }
                    case ConsoleKey.Escape:
                        {
                            return -1;
                        }
                }
            }
        }

    }
}
