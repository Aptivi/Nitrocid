
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using VT.NET.Tools;

namespace KS.ConsoleBase.Inputs.Styles
{
    /// <summary>
    /// Selection style for input module
    /// </summary>
    public static class SelectionStyle
    {

        private static int savedPos = 1;

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
            // Variables
            int HighlightedAnswer = savedPos;
            List<InputChoiceInfo> AllAnswers = new(Answers);
            AllAnswers.AddRange(AltAnswers);

            // Before we proceed, we need to check the highlighted answer number
            if (HighlightedAnswer > AllAnswers.Count)
                HighlightedAnswer = 1;

            // First alt answer index
            int altAnswersFirstIdx = Answers.Count;
            ConsoleKeyInfo Answer;
            ConsoleWrapper.CursorVisible = false;
            ConsoleWrapper.Clear(true);

            // Ask a question
            TextWriterColor.Write(Question, true, KernelColorType.Question);

            // Make pages based on console window height
            int listStartPosition = ConsoleWrapper.CursorTop;
            int listEndPosition = ConsoleWrapper.WindowHeight - ConsoleWrapper.CursorTop;
            int answersPerPage = listEndPosition - 4;
            int pages = AllAnswers.Count / answersPerPage;
            int lastPage = 1;
            bool refreshRequired = false;

            while (true)
            {
                // The reason for subtracting the highlighted answer by one is that because while the highlighted answer number is one-based, the indexes are zero-based,
                // causing confusion. Pages, again, are one-based.
                int currentPage = (HighlightedAnswer - 1) / answersPerPage;
                int startIndex = answersPerPage * currentPage;
                int endIndex = (answersPerPage * (currentPage + 1)) - 1;

                // If the current page is different, refresh the entire screen.
                if (currentPage != lastPage || refreshRequired)
                {
                    ConsoleWrapper.Clear(true);
                    TextWriterColor.Write(Question, true, KernelColorType.Question);
                }

                // Populate the answers
                int renderedAnswers = 0;
                for (int AnswerIndex = startIndex; AnswerIndex <= endIndex && AnswerIndex <= AllAnswers.Count - 1; AnswerIndex++)
                {
                    ConsoleWrapper.SetCursorPosition(0, listStartPosition + renderedAnswers);
                    bool AltAnswer = AnswerIndex >= altAnswersFirstIdx;
                    var AnswerInstance = AllAnswers[AnswerIndex];
                    string AnswerTitle = AnswerInstance.ChoiceTitle ?? "";

                    // Get the option
                    string AnswerOption = $" {AnswerInstance}) {AnswerTitle}";
                    int AnswerTitleLeft = AllAnswers.Max(x => $" {x.ChoiceName}) ".Length);
                    if (AnswerTitleLeft < ConsoleWrapper.WindowWidth)
                    {
                        int blankRepeats = AnswerTitleLeft - $" {AnswerInstance.ChoiceName}) ".Length;
                        AnswerOption = $" {AnswerInstance.ChoiceName}) " + new string(' ', blankRepeats) + $"{AnswerTitle}";
                    }
                    var AnswerColor = AnswerIndex + 1 == HighlightedAnswer ? 
                                      KernelColorType.SelectedOption : 
                                      AltAnswer ? KernelColorType.AlternativeOption : KernelColorType.Option;
                    AnswerOption = $"{ColorTools.GetColor(AnswerColor).VTSequenceForeground}{AnswerOption}";
                    TextWriterColor.Write(AnswerOption.Truncate(ConsoleWrapper.WindowWidth - 3 + VtSequenceTools.MatchVTSequences(AnswerOption).Sum((mc) => mc.Sum((m) => m.Length))), AnswerColor);
                    renderedAnswers++;
                }

                // If we need to write the vertical progress bar, do so. But, we need to refresh in case we're told to redraw on demand when
                // we're not switching pages yet. Switching pages requires clearing the console as per the current implementation.
                if (Flags.EnableScrollBarInSelection)
                {
                    for (int y = listStartPosition; y < listEndPosition; y++)
                        TextWriterWhereColor.WriteWhere(" ", ConsoleWrapper.WindowWidth - 1, y);
                    ProgressBarVerticalColor.WriteVerticalProgress(100 * ((double)HighlightedAnswer / AllAnswers.Count), ConsoleWrapper.WindowWidth - 2, listStartPosition - 1, listStartPosition, 4, false);
                }

                // Write description area
                int descSepArea = ConsoleWrapper.WindowHeight - 3;
                int descArea = ConsoleWrapper.WindowHeight - 2;
                var highlightedAnswer = AllAnswers[HighlightedAnswer - 1];
                string descFinal = highlightedAnswer.ChoiceDescription is not null ? highlightedAnswer.ChoiceDescription.Truncate((ConsoleWrapper.WindowWidth * 2) - 3) : "";
                TextWriterWhereColor.WriteWhere(new string('=', ConsoleWrapper.WindowWidth), 0, descSepArea, KernelColorType.Separator);
                TextWriterWhereColor.WriteWhere(new string(' ', ConsoleWrapper.WindowWidth), 0, descArea);
                TextWriterWhereColor.WriteWhere(new string(' ', ConsoleWrapper.WindowWidth), 0, descArea + 1);
                TextWriterWhereColor.WriteWhere(descFinal, 0, descArea, KernelColorType.NeutralText);

                // Write keybindings and page and answer number
                string bindingsRender = $"[{Translate.DoTranslation("ENTER: select")}]==[{Translate.DoTranslation("ESC: exit")}]==[{Translate.DoTranslation("TAB: info")}]";
                string numberRender = $"[{currentPage + 1}/{pages + 1}]==[{HighlightedAnswer}/{AllAnswers.Count}]";
                int bindingsLeft = 2;
                int numbersLeft = ConsoleWrapper.WindowWidth - numberRender.Length - bindingsLeft;
                TextWriterWhereColor.WriteWhere(bindingsRender, bindingsLeft, descSepArea, KernelColorType.Separator);
                TextWriterWhereColor.WriteWhere(numberRender, numbersLeft, descSepArea, KernelColorType.Separator);

                // Wait for an answer
                Answer = Input.DetectKeypress();

                // Check the answer
                switch (Answer.Key)
                {
                    case ConsoleKey.UpArrow:
                        HighlightedAnswer -= 1;
                        if (HighlightedAnswer == 0)
                            HighlightedAnswer = AllAnswers.Count;
                        break;
                    case ConsoleKey.DownArrow:
                        HighlightedAnswer += 1;
                        if (HighlightedAnswer > AllAnswers.Count)
                            HighlightedAnswer = 1;
                        break;
                    case ConsoleKey.Home:
                        HighlightedAnswer = 1;
                        break;
                    case ConsoleKey.End:
                        HighlightedAnswer = AllAnswers.Count;
                        break;
                    case ConsoleKey.PageUp:
                        HighlightedAnswer = startIndex > 0 ? startIndex : 1;
                        break;
                    case ConsoleKey.PageDown:
                        HighlightedAnswer = endIndex > AllAnswers.Count - 1 ? AllAnswers.Count : endIndex + 2;
                        HighlightedAnswer = endIndex == AllAnswers.Count - 1 ? endIndex + 1 : HighlightedAnswer;
                        break;
                    case ConsoleKey.Enter:
                        TextWriterColor.Write();
                        savedPos = HighlightedAnswer;
                        return HighlightedAnswer;
                    case ConsoleKey.Escape:
                        TextWriterColor.Write();
                        savedPos = HighlightedAnswer;
                        return -1;
                    case ConsoleKey.Tab:
                        if (string.IsNullOrEmpty(highlightedAnswer.ChoiceDescription))
                            break;
                        var infoRenderer = new StringBuilder();
                        infoRenderer.AppendJoin("\n",
                            new[]
                            {
                                highlightedAnswer.ChoiceTitle,
                                new string('-', highlightedAnswer.ChoiceTitle.Length),
                                "",
                                highlightedAnswer.ChoiceDescription,
                            }
                        );
                        InfoBoxColor.WriteInfoBox(infoRenderer.ToString());
                        refreshRequired = true;
                        break;
                }

                // Update the last page
                lastPage = currentPage;
            }
        }

    }
}
