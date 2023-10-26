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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Languages;
using KS.Misc.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminaux.Reader;

namespace KS.ConsoleBase.Presentation.Elements
{
    /// <summary>
    /// Multiple choice input element
    /// </summary>
    public class MultipleChoiceInputElement : IElement
    {
        /// <inheritdoc/>
        public bool IsInput => true;

        /// <inheritdoc/>
        public string WrittenInput { get; set; }

        /// <summary>
        /// The first argument denotes the prompt to be written, and the rest for the parameters to be formatted
        /// </summary>
        public object[] Arguments { get; set; }

        /// <summary>
        /// Renders the element
        /// </summary>
        public void Render()
        {
            // Get the text and the arguments
            object[] finalArgs = Arguments.Length > 1 ? Arguments.Skip(1).ToArray() : Array.Empty<object>();
            string text = TextTools.FormatString((string)(Arguments.Length > 0 ? Arguments[0] : ""), finalArgs);

            // Check the bounds
            string[] splitText = TextTools.GetWrappedSentences(text, PresentationTools.PresentationLowerInnerBorderLeft - PresentationTools.PresentationUpperBorderLeft + 2);
            int top = ConsoleWrapper.CursorTop;
            int seekTop = ConsoleWrapper.CursorTop;
            var buffer = new StringBuilder();
            foreach (string split in splitText)
            {
                int maxHeight = PresentationTools.PresentationLowerInnerBorderTop - top + 3;
                if (maxHeight < 0)
                {
                    // If the text is going to overflow the presentation view, clear the presentation and finish writing the parts
                    TextWriterWhereColor.WriteWhereKernelColor(buffer.ToString(), PresentationTools.PresentationUpperInnerBorderLeft, seekTop, false, KernelColorType.NeutralText);
                    Input.DetectKeypress();
                    PresentationTools.ClearPresentation();
                    seekTop = top = PresentationTools.PresentationUpperInnerBorderTop;
                    buffer.Clear();
                }

                // Write the part
                buffer.Append(split + "\n");
                top++;
            }
            TextWriterWhereColor.WriteWhereKernelColor(buffer.ToString(), PresentationTools.PresentationUpperInnerBorderLeft, seekTop, false, KernelColorType.NeutralText);

            // Check the bounds
            int height = PresentationTools.PresentationLowerInnerBorderTop - ConsoleWrapper.CursorTop + 3;
            if (height < 0)
            {
                // If the text is going to overflow the presentation view, clear the presentation
                Input.DetectKeypress();
                PresentationTools.ClearPresentation();
            }

            // Flatten the enumerables to their string value representations
            List<string> choices = new();
            foreach (var finalArg in finalArgs)
            {
                if (finalArg is IEnumerable enumerable && finalArg is not string)
                    foreach (var enumerableValue in enumerable)
                        choices.Add(enumerableValue.ToString());
                else
                    choices.Add(finalArg.ToString());
            }

            // Render the choices (with checking for bounds, again)
            TextWriterWhereColor.WriteWhereKernelColor("\n", PresentationTools.PresentationUpperInnerBorderLeft, Console.CursorTop, false, PresentationTools.PresentationUpperInnerBorderLeft, KernelColorType.NeutralText);
            string[] finalChoices = choices.ToArray();
            int choiceNum = 1;
            var choiceBuffer = new StringBuilder();
            int choiceSeekTop = ConsoleWrapper.CursorTop;
            int choiceTop = ConsoleWrapper.CursorTop;
            foreach (string choice in finalChoices)
            {
                string finalChoice = $"{choiceNum}) {choice}";
                string[] splitTextChoice = TextTools.GetWrappedSentences(finalChoice, PresentationTools.PresentationLowerInnerBorderLeft - PresentationTools.PresentationUpperBorderLeft + 2);
                foreach (string split in splitTextChoice)
                {
                    int maxHeight = PresentationTools.PresentationLowerInnerBorderTop - choiceTop + 3;
                    if (maxHeight < 0)
                    {
                        // If the text is going to overflow the presentation view, clear the presentation and finish writing the parts
                        TextWriterWhereColor.WriteWhereKernelColor(choiceBuffer.ToString(), PresentationTools.PresentationUpperInnerBorderLeft, choiceSeekTop, false, KernelColorType.NeutralText);
                        Input.DetectKeypress();
                        PresentationTools.ClearPresentation();
                        choiceSeekTop = choiceTop = PresentationTools.PresentationUpperInnerBorderTop;
                        choiceBuffer.Clear();
                    }

                    // Write the part
                    choiceBuffer.Append(split + "\n");
                    choiceTop++;
                }
                choiceNum++;
            }
            TextWriterWhereColor.WriteWhereKernelColor(choiceBuffer.ToString(), PresentationTools.PresentationUpperInnerBorderLeft, choiceSeekTop, false, KernelColorType.NeutralText);

            // Check the bounds
            height = PresentationTools.PresentationLowerInnerBorderTop - ConsoleWrapper.CursorTop + 3;
            if (height < 0)
            {
                // If the text is going to overflow the presentation view, clear the presentation
                Input.DetectKeypress();
                PresentationTools.ClearPresentation();
            }

            // Populate relevant settings
            var settings = new TermReaderSettings()
            {
                RightMargin = PresentationTools.PresentationUpperInnerBorderLeft
            };

            // Get the input
            TextWriterWhereColor.WriteWhereKernelColor("\n", PresentationTools.PresentationUpperInnerBorderLeft, Console.CursorTop, false, PresentationTools.PresentationUpperInnerBorderLeft, KernelColorType.NeutralText);
            string[] selected = Array.Empty<string>();
            while (selected.Length == 0 || !selected.All((selectedChoice) => finalChoices.Contains(selectedChoice)))
            {
                ConsoleWrapper.CursorLeft = PresentationTools.PresentationUpperInnerBorderLeft;
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Select your choice separated by semicolons: "), false, KernelColorType.Input);
                ConsoleWrapper.CursorVisible = true;
                WrittenInput = Input.ReadLineWrapped("", "", settings);
                selected = WrittenInput.Split(';', StringSplitOptions.RemoveEmptyEntries);
                ConsoleWrapper.CursorVisible = false;
            }

            // Trim repeated inputs
            List<string> finalSelected = new();
            foreach (string choice in selected)
                if (!finalSelected.Contains(choice))
                    finalSelected.Add(choice);
            WrittenInput = string.Join(";", finalSelected);
        }

        /// <summary>
        /// Checks to see if the text is possibly overflowing the slideshow display
        /// </summary>
        public bool IsPossibleOutOfBounds()
        {
            // Get the text, the arguments, and the choices
            object[] finalArgs = Arguments.Length > 1 ? Arguments.Skip(1).ToArray() : Array.Empty<object>();

            // Flatten the enumerables to their string value representations
            List<string> choices = new();
            foreach (var finalArg in finalArgs)
            {
                if (finalArg is IEnumerable enumerable && finalArg is not string)
                    foreach (var enumerableValue in enumerable)
                        choices.Add(enumerableValue.ToString());
                else
                    choices.Add(finalArg.ToString());
            }

            string[] finalChoices = choices.ToArray();
            string text = TextTools.FormatString((string)(Arguments.Length > 0 ? Arguments[0] : ""), finalArgs) + "\n\n";

            // Add the choices to the text
            for (int choice = 0; choice < finalChoices.Length; choice++)
                text += $"\n{choice + 1}) {finalChoices[choice]}";
            text += "\n\n";

            // Check the bounds
            string[] splitText = TextTools.GetWrappedSentences(text, PresentationTools.PresentationLowerInnerBorderLeft - PresentationTools.PresentationUpperInnerBorderLeft);
            int maxHeight = PresentationTools.PresentationLowerInnerBorderTop - ConsoleWrapper.CursorTop + 3;
            return splitText.Length > maxHeight;
        }

        /// <inheritdoc/>
        public Action<object[]> InvokeActionInput { get; set; }

        /// <inheritdoc/>
        public Action InvokeAction { get; }
    }
}
