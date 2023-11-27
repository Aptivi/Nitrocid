//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Kernel.Configuration;
using KS.Languages;
using KS.Misc.Text;

namespace KS.ConsoleBase.Inputs.Styles.Choice
{
    /// <summary>
    /// Choice style for input module
    /// </summary>
    public static class ChoiceStyle
    {

        /// <summary>
        /// Default input choice output type
        /// </summary>
        public static ChoiceOutputType DefaultChoiceOutputType =>
            (ChoiceOutputType)Config.MainConfig.DefaultChoiceOutputType;

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, string AnswersStr, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, AnswersStr, [], OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, string AnswersStr, string[] AnswersTitles, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, AnswersStr, AnswersTitles, "", [], OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="AlternateAnswersStr">Set of alternate answers. They can be written like this: Y/N/C.</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, string AnswersStr, string[] AnswersTitles, string AlternateAnswersStr, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, AnswersStr, AnswersTitles, AlternateAnswersStr, [], OutputType, PressEnter);

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
        public static string PromptChoice(string Question, string AnswersStr, string[] AnswersTitles, string AlternateAnswersStr, string[] AlternateAnswersTitles, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, InputChoiceTools.GetInputChoices(AnswersStr, AnswersTitles), InputChoiceTools.GetInputChoices(AlternateAnswersStr, AlternateAnswersTitles), OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, string[] Answers, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, Answers, [], OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, string[] Answers, string[] AnswersTitles, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, Answers, AnswersTitles, [], [], OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="AlternateAnswers">Set of alternate answers.</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, string[] Answers, string[] AnswersTitles, string[] AlternateAnswers, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, Answers, AnswersTitles, AlternateAnswers, [], OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="AlternateAnswers">Set of alternate answers.</param>
        /// <param name="AlternateAnswersTitles">Working titles for each alternate answer. It must be the same amount as the alternate answers.</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, string[] Answers, string[] AnswersTitles, string[] AlternateAnswers, string[] AlternateAnswersTitles, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, InputChoiceTools.GetInputChoices(Answers, AnswersTitles), InputChoiceTools.GetInputChoices(AlternateAnswers, AlternateAnswersTitles), OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, List<InputChoiceInfo> Answers, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, Answers, [], OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AltAnswers">Set of alternate answers.</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, List<InputChoiceInfo> Answers, List<InputChoiceInfo> AltAnswers, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false)
        {
            while (true)
            {
                string answer;

                // First, check to see if the answers consist of single or multiple characters, and exit if the mode is not appropriate with an error message.
                string[] answers = Answers.Select((ici) => ici.ChoiceName).ToArray();
                string[] altAnswers = AltAnswers.Select((ici) => ici.ChoiceName).ToArray();
                if (!PressEnter && (answers.Any((answer) => answer.Length > 1) || altAnswers.Any((answer) => answer.Length > 1)))
                {
                    TextWriterColor.Write(Translate.DoTranslation("Can't provide answers with more than one character in single-character mode."));
                    return "";
                }

                // Ask a question
                switch (OutputType)
                {
                    case ChoiceOutputType.OneLine:
                        {
                            string answersPlace = altAnswers.Length > 0 ? " <{0}/{1}> " : " <{0}> ";
                            TextWriterColor.WriteKernelColor(Question, false, KernelColorType.Question);
                            TextWriterColor.WriteKernelColor(answersPlace, false, KernelColorType.Input, string.Join("/", answers), string.Join("/", altAnswers));
                            break;
                        }
                    case ChoiceOutputType.TwoLines:
                        {
                            string answersPlace = altAnswers.Length > 0 ? "<{0}/{1}> " : "<{0}> ";
                            TextWriterColor.WriteKernelColor(Question, true, KernelColorType.Question);
                            TextWriterColor.WriteKernelColor(answersPlace, false, KernelColorType.Input, string.Join("/", answers), string.Join("/", altAnswers));
                            break;
                        }
                    case ChoiceOutputType.Modern:
                        {
                            TextWriterColor.WriteKernelColor(Question + CharManager.NewLine, true, KernelColorType.Question);
                            for (int AnswerIndex = 0; AnswerIndex <= Answers.Count - 1; AnswerIndex++)
                            {
                                var AnswerInstance = Answers[AnswerIndex];
                                string AnswerTitle = AnswerInstance.ChoiceTitle ?? "";
                                string AnswerOption = $" {AnswerInstance.ChoiceName}) {AnswerTitle}";
                                int AnswerTitleLeft = answers.Max(x => $" {x}) ".Length);
                                if (AnswerTitleLeft < ConsoleWrapper.WindowWidth)
                                {
                                    int blankRepeats = AnswerTitleLeft - $" {AnswerInstance.ChoiceName}) ".Length;
                                    AnswerOption = $" {AnswerInstance.ChoiceName}) " + new string(' ', blankRepeats) + $"{AnswerTitle}";
                                }
                                TextWriterColor.WriteKernelColor(AnswerOption, true, KernelColorType.Option);
                            }
                            if (AltAnswers.Count > 0)
                                TextWriterColor.WriteKernelColor(" ----------------", true, KernelColorType.AlternativeOption);
                            for (int AnswerIndex = 0; AnswerIndex <= AltAnswers.Count - 1; AnswerIndex++)
                            {
                                var AnswerInstance = AltAnswers[AnswerIndex];
                                string AnswerTitle = AnswerInstance.ChoiceTitle ?? "";
                                string AnswerOption = $" {AnswerInstance.ChoiceName}) {AnswerTitle}";
                                int AnswerTitleLeft = altAnswers.Max(x => $" {x}) ".Length);
                                if (AnswerTitleLeft < ConsoleWrapper.WindowWidth)
                                {
                                    int blankRepeats = AnswerTitleLeft - $" {AnswerInstance.ChoiceName}) ".Length;
                                    AnswerOption = $" {AnswerInstance.ChoiceName}) " + new string(' ', blankRepeats) + $"{AnswerTitle}";
                                }
                                TextWriterColor.WriteKernelColor(AnswerOption, true, KernelColorType.AlternativeOption);
                            }
                            TextWriterColor.WriteKernelColor(CharManager.NewLine + ">> ", false, KernelColorType.Input);
                            break;
                        }
                    case ChoiceOutputType.Table:
                        {
                            var ChoiceHeader = new[] { Translate.DoTranslation("Possible answers"), Translate.DoTranslation("Answer description") };
                            var ChoiceData = new string[Answers.Count + AltAnswers.Count, 2];
                            TextWriterColor.WriteKernelColor(Question, true, KernelColorType.Question);
                            for (int AnswerIndex = 0; AnswerIndex <= Answers.Count - 1; AnswerIndex++)
                            {
                                ChoiceData[AnswerIndex, 0] = Answers[AnswerIndex].ChoiceName;
                                ChoiceData[AnswerIndex, 1] = Answers[AnswerIndex].ChoiceTitle ?? "";
                            }
                            for (int AnswerIndex = 0; AnswerIndex <= AltAnswers.Count - 1; AnswerIndex++)
                            {
                                ChoiceData[Answers.Count - 1 + AnswerIndex, 0] = AltAnswers[AnswerIndex].ChoiceName;
                                ChoiceData[Answers.Count - 1 + AnswerIndex, 1] = AltAnswers[AnswerIndex].ChoiceTitle ?? "";
                            }
                            TableColor.WriteTable(ChoiceHeader, ChoiceData, 2);
                            TextWriterColor.WriteKernelColor(CharManager.NewLine + ">> ", false, KernelColorType.Input);
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
                    answer = Convert.ToString(Input.DetectKeypress().KeyChar);
                    TextWriterColor.Write();
                }

                // Check if answer is correct.
                if (Answers.Select((ici) => ici.ChoiceName).Contains(answer) ||
                    AltAnswers.Select((ici) => ici.ChoiceName).Contains(answer))
                {
                    return answer;
                }
            }
        }

    }
}
