
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
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;

namespace KS.ConsoleBase.Inputs.Styles
{
    public static class ChoiceStyle
    {

        public static ChoiceOutputType DefaultChoiceOutputType = ChoiceOutputType.Modern;

        /// <summary>
        /// The enumeration for the choice command output type
        /// </summary>
        public enum ChoiceOutputType
        {
            /// <summary>
            /// A question and a set of answers in one line
            /// </summary>
            OneLine,
            /// <summary>
            /// A question in a line and a set of answers in another line
            /// </summary>
            TwoLines,
            /// <summary>
            /// The modern way of listing choices
            /// </summary>
            Modern,
            /// <summary>
            /// The table of choices
            /// </summary>
            Table
        }

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, string AnswersStr, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false)
        {
            return PromptChoice(Question, AnswersStr, Array.Empty<string>(), OutputType, PressEnter);
        }

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, string AnswersStr, string[] AnswersTitles, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false)
        {
            while (true)
            {
                // Variables
                var answers = AnswersStr.Split('/');
                string answer;

                // Check to see if the answer titles are the same
                if (answers.Length != AnswersTitles.Length)
                {
                    Array.Resize(ref AnswersTitles, answers.Length);
                }

                // Ask a question
                switch (OutputType)
                {
                    case ChoiceOutputType.OneLine:
                        {
                            TextWriterColor.Write(Question, false, ColorTools.ColTypes.Question);
                            TextWriterColor.Write(" <{0}> ", false, ColorTools.ColTypes.Input, AnswersStr);
                            break;
                        }
                    case ChoiceOutputType.TwoLines:
                        {
                            TextWriterColor.Write(Question, true, ColorTools.ColTypes.Question);
                            TextWriterColor.Write("<{0}> ", false, ColorTools.ColTypes.Input, AnswersStr);
                            break;
                        }
                    case ChoiceOutputType.Modern:
                        {
                            TextWriterColor.Write(Question + Kernel.Kernel.NewLine, true, ColorTools.ColTypes.Question);
                            int AnswerTitleLeft = answers.Max(x => $" {x}) ".Length);
                            if (AnswerTitleLeft >= Console.WindowWidth)
                                AnswerTitleLeft = 0;
                            for (int AnswerIndex = 0, loopTo = answers.Length - 1; AnswerIndex <= loopTo; AnswerIndex++)
                            {
                                string AnswerInstance = answers[AnswerIndex];
                                string AnswerTitle = AnswersTitles[AnswerIndex];
                                if (AnswerTitleLeft > 0)
                                {
                                    TextWriterColor.Write($" {AnswerInstance}) ", false, ColorTools.ColTypes.Option);
                                    TextWriterWhereColor.WriteWhere(AnswerTitle, AnswerTitleLeft, Console.CursorTop, false, ColorTools.ColTypes.Option);
                                    TextWriterColor.Write("", true, ColorTools.ColTypes.Option);
                                }
                                else
                                {
                                    TextWriterColor.Write($" {AnswerInstance}) {AnswerTitle}", true, ColorTools.ColTypes.Option);
                                }
                            }
                            TextWriterColor.Write(Kernel.Kernel.NewLine + ">> ", false, ColorTools.ColTypes.Input);
                            break;
                        }
                    case ChoiceOutputType.Table:
                        {
                            var ChoiceHeader = new[] { Translate.DoTranslation("Possible answers"), Translate.DoTranslation("Answer description") };
                            var ChoiceData = new string[answers.Length, 2];
                            TextWriterColor.Write(Question, true, ColorTools.ColTypes.Question);
                            for (int AnswerIndex = 0, loopTo1 = answers.Length - 1; AnswerIndex <= loopTo1; AnswerIndex++)
                            {
                                ChoiceData[AnswerIndex, 0] = answers[AnswerIndex];
                                ChoiceData[AnswerIndex, 1] = AnswersTitles[AnswerIndex];
                            }
                            TableColor.WriteTable(ChoiceHeader, ChoiceData, 2);
                            TextWriterColor.Write(Kernel.Kernel.NewLine + ">> ", false, ColorTools.ColTypes.Input);
                            break;
                        }
                }

                // Wait for an answer
                if (PressEnter)
                {
                    answer = Input.ReadLine();
                }
                else
                {
                    answer = Convert.ToString(Console.ReadKey().KeyChar);
                    Console.WriteLine();
                }

                // Check if answer is correct.
                if (answers.Contains(answer))
                {
                    return answer;
                }
            }
        }

    }
}
