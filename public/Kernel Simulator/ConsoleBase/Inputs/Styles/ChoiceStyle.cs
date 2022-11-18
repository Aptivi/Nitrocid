
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
using System.Linq;
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;

namespace KS.ConsoleBase.Inputs.Styles
{
    /// <summary>
    /// Choice style for input module
    /// </summary>
    public static class ChoiceStyle
    {

        /// <summary>
        /// Default input choice output type
        /// </summary>
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
        public static string PromptChoice(string Question, string AnswersStr, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) => 
            PromptChoice(Question, AnswersStr, Array.Empty<string>(), OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, string AnswersStr, string[] AnswersTitles, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, AnswersStr, AnswersTitles, "", Array.Empty<string>(), OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="AlternateAnswersStr">Set of alternate answers. They can be written like this: Y/N/C.</param>
        /// <param name="AlternateAnswersTitles">Working titles for each alternate answer. It must be the same amount as the alternate answers.</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, string AnswersStr, string[] AnswersTitles, string AlternateAnswersStr, string[] AlternateAnswersTitles, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false)
        {
            while (true)
            {
                // Variables
                var answers = AnswersStr.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                var altAnswers = AlternateAnswersStr.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                var finalAnswers = new List<string>();
                var finalAnswerTitles = new List<string>();
                string answer;

                // Check to see if the answer titles are the same
                if (answers.Length != AnswersTitles.Length)
                    Array.Resize(ref AnswersTitles, answers.Length);
                if (altAnswers.Length != AlternateAnswersTitles.Length)
                    Array.Resize(ref AlternateAnswersTitles, altAnswers.Length);

                // Populate answers to final list for below operation
                foreach (var _answer in answers)
                    finalAnswers.Add(_answer);
                foreach (var _answer in altAnswers)
                    finalAnswers.Add(_answer);

                // Ask a question
                switch (OutputType)
                {
                    case ChoiceOutputType.OneLine:
                        {
                            TextWriterColor.Write(Question, false, ColorTools.ColTypes.Question);
                            TextWriterColor.Write(" <{0}/{1}> ", false, ColorTools.ColTypes.Input, AnswersStr, AlternateAnswersStr);
                            break;
                        }
                    case ChoiceOutputType.TwoLines:
                        {
                            TextWriterColor.Write(Question, true, ColorTools.ColTypes.Question);
                            TextWriterColor.Write("<{0}/{1}> ", false, ColorTools.ColTypes.Input, AnswersStr, AlternateAnswersStr);
                            break;
                        }
                    case ChoiceOutputType.Modern:
                        {
                            TextWriterColor.Write(Question + CharManager.NewLine, true, ColorTools.ColTypes.Question);
                            for (int AnswerIndex = 0; AnswerIndex <= answers.Length - 1; AnswerIndex++)
                            {
                                string AnswerInstance = answers[AnswerIndex];
                                string AnswerTitle = AnswersTitles[AnswerIndex] ?? "";
                                string AnswerOption = $" {AnswerInstance}) {AnswerTitle}";
                                int AnswerTitleLeft = answers.Max(x => $" {x}) ".Length);
                                if (AnswerTitleLeft < ConsoleWrapper.WindowWidth)
                                {
                                    int blankRepeats = AnswerTitleLeft - $" {AnswerInstance}) ".Length;
                                    AnswerOption = $" {AnswerInstance}) " + " ".Repeat(blankRepeats) + $"{AnswerTitle}";
                                }
                                TextWriterColor.Write(AnswerOption, true, ColorTools.ColTypes.Option);
                            }
                            if (altAnswers.Length > 0)
                                TextWriterColor.Write(" ----------------", true, ColorTools.ColTypes.AlternativeOption);
                            for (int AnswerIndex = 0; AnswerIndex <= altAnswers.Length - 1; AnswerIndex++)
                            {
                                string AnswerInstance = altAnswers[AnswerIndex];
                                string AnswerTitle = AlternateAnswersTitles[AnswerIndex] ?? "";
                                string AnswerOption = $" {AnswerInstance}) {AnswerTitle}";
                                int AnswerTitleLeft = altAnswers.Max(x => $" {x}) ".Length);
                                if (AnswerTitleLeft < ConsoleWrapper.WindowWidth)
                                {
                                    int blankRepeats = AnswerTitleLeft - $" {AnswerInstance}) ".Length;
                                    AnswerOption = $" {AnswerInstance}) " + " ".Repeat(blankRepeats) + $"{AnswerTitle}";
                                }
                                TextWriterColor.Write(AnswerOption, true, ColorTools.ColTypes.AlternativeOption);
                            }
                            TextWriterColor.Write(CharManager.NewLine + ">> ", false, ColorTools.ColTypes.Input);
                            break;
                        }
                    case ChoiceOutputType.Table:
                        {
                            var ChoiceHeader = new[] { Translate.DoTranslation("Possible answers"), Translate.DoTranslation("Answer description") };
                            var ChoiceData = new string[answers.Length + altAnswers.Length, 2];
                            TextWriterColor.Write(Question, true, ColorTools.ColTypes.Question);
                            for (int AnswerIndex = 0; AnswerIndex <= answers.Length - 1; AnswerIndex++)
                            {
                                ChoiceData[AnswerIndex, 0] = answers[AnswerIndex];
                                ChoiceData[AnswerIndex, 1] = AnswersTitles[AnswerIndex] ?? "";
                            }
                            for (int AnswerIndex = 0; AnswerIndex <= altAnswers.Length - 1; AnswerIndex++)
                            {
                                ChoiceData[answers.Length - 1 + AnswerIndex, 0] = altAnswers[AnswerIndex];
                                ChoiceData[answers.Length - 1 + AnswerIndex, 1] = AlternateAnswersTitles[AnswerIndex] ?? "";
                            }
                            TableColor.WriteTable(ChoiceHeader, ChoiceData, 2);
                            TextWriterColor.Write(CharManager.NewLine + ">> ", false, ColorTools.ColTypes.Input);
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
                    answer = Convert.ToString(ConsoleWrapper.ReadKey().KeyChar);
                    TextWriterColor.Write();
                }

                // Check if answer is correct.
                if (answers.Contains(answer) || altAnswers.Contains(answer))
                {
                    return answer;
                }
            }
        }

    }
}
