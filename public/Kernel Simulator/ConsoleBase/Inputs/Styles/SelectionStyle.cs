
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
using KS.Kernel;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;

namespace KS.ConsoleBase.Inputs.Styles
{
    /// <summary>
    /// Selection style for input module
    /// </summary>
    public static class SelectionStyle
    {

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        public static int PromptSelection(string Question, string AnswersStr) => 
            PromptSelection(Question, AnswersStr, Array.Empty<string>(), "", Array.Empty<string>());

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        public static int PromptSelection(string Question, string AnswersStr, string[] AnswersTitles) =>
            PromptSelection(Question, AnswersStr, AnswersTitles, "", Array.Empty<string>());

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="AlternateAnswersStr">Set of alternate answers. They can be written like this: Y/N/C.</param>
        /// <param name="AlternateAnswersTitles">Working titles for each alternate answer. It must be the same amount as the alternate answers.</param>
        public static int PromptSelection(string Question, string AnswersStr, string[] AnswersTitles, string AlternateAnswersStr, string[] AlternateAnswersTitles) =>
            PromptSelection(Question, InputChoiceTools.GetInputChoices(AnswersStr, AnswersTitles), InputChoiceTools.GetInputChoices(AlternateAnswersStr, AlternateAnswersTitles));

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        public static int PromptSelection(string Question, List<InputChoiceInfo> Answers) =>
            PromptSelection(Question, Answers, new List<InputChoiceInfo>());

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AltAnswers">Set of alternate answers.</param>
        public static int PromptSelection(string Question, List<InputChoiceInfo> Answers, List<InputChoiceInfo> AltAnswers)
        {
            int HighlightedAnswer = 1;
            while (true)
            {
                // Variables
                List<InputChoiceInfo> AllAnswers = new(Answers);
                AllAnswers.AddRange(AltAnswers);
                int altAnswersFirstIdx = Answers.Count;
                ConsoleKeyInfo Answer;
                ConsoleWrapper.CursorVisible = false;
                ConsoleWrapper.Clear(true);

                // Ask a question
                TextWriterColor.Write(Question + CharManager.NewLine, true, ColorTools.ColTypes.Question);

                // Make pages based on console window height
                int listStartPosition = ConsoleWrapper.CursorTop;
                int listEndPosition = ConsoleWrapper.WindowHeight - ConsoleWrapper.CursorTop;
                int pages = AllAnswers.Count / listEndPosition;
                int answersPerPage = listEndPosition - 2;

                // The reason for subtracting the highlighted answer by one is that because while the highlighted answer number is one-based, the indexes are zero-based,
                // causing confusion. Pages, again, are one-based. Highlighting the last option causes us to go to the next page. This is intentional.
                int currentPage = (HighlightedAnswer - 1) / answersPerPage;
                int startIndex = answersPerPage * currentPage;
                int endIndex = answersPerPage * (currentPage + 1);

                // Populate the answers
                for (int AnswerIndex = startIndex; AnswerIndex <= endIndex && AnswerIndex <= AllAnswers.Count - 1; AnswerIndex++)
                {
                    bool AltAnswer = AnswerIndex >= altAnswersFirstIdx;
                    var AnswerInstance = AllAnswers[AnswerIndex];
                    string AnswerTitle = AnswerInstance.ChoiceTitle ?? "";

                    // Just like we told you previously in above few lines, the last option highlight to go to the next page is intentional. So, change the last option
                    // string so it says "Highlight this entry to go to the next page."
                    string AnswerOption = $" {AnswerInstance}) {AnswerTitle}";
                    int AnswerTitleLeft = AllAnswers.Max(x => $" {x.ChoiceName}) ".Length);
                    if (AnswerTitleLeft < ConsoleWrapper.WindowWidth)
                    {
                        int blankRepeats = AnswerTitleLeft - $" {AnswerInstance.ChoiceName}) ".Length;
                        AnswerOption = $" {AnswerInstance.ChoiceName}) " + " ".Repeat(blankRepeats) + $"{AnswerTitle}";
                    }
                    var AnswerColor = AnswerIndex + 1 == HighlightedAnswer ? 
                                      ColorTools.ColTypes.SelectedOption : 
                                      AltAnswer ? ColorTools.ColTypes.AlternativeOption : ColorTools.ColTypes.Option;
                    TextWriterColor.Write(AnswerIndex == endIndex ? " vvvvvvvvvv " + Translate.DoTranslation("Highlight this entry to go to the next page.") + " vvvvvvvvvv " : AnswerOption.Truncate(ConsoleWrapper.WindowWidth - 3), true, AnswerColor);
                }

                // If we need to write the vertical progress bar, do so.
                if (Flags.EnableScrollBarInSelection)
                    ProgressBarVerticalColor.WriteVerticalProgress(100 * ((double)HighlightedAnswer / AllAnswers.Count), ConsoleWrapper.WindowWidth - 2, listStartPosition - 1, listStartPosition, 1, false);

                // Wait for an answer
                Answer = Input.DetectKeypress();

                // Check the answer
                switch (Answer.Key)
                {
                    case ConsoleKey.UpArrow:
                        {
                            HighlightedAnswer -= 1;
                            if (HighlightedAnswer == 0)
                                HighlightedAnswer = AllAnswers.Count;

                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            if (HighlightedAnswer == AllAnswers.Count)
                                HighlightedAnswer = 0;
                            HighlightedAnswer += 1;
                            break;
                        }
                    case ConsoleKey.PageUp:
                        {
                            HighlightedAnswer = 1;
                            break;
                        }
                    case ConsoleKey.PageDown:
                        {
                            HighlightedAnswer = AllAnswers.Count;
                            break;
                        }
                    case ConsoleKey.Enter:
                        {
                            TextWriterColor.Write();
                            return HighlightedAnswer;
                        }
                    case ConsoleKey.Escape:
                        {
                            TextWriterColor.Write();
                            return -1;
                        }
                }
            }
        }

    }
}
