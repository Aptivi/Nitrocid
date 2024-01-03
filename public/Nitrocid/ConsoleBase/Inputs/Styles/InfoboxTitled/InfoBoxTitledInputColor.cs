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

using System;
using System.Threading;
using System.Collections.Generic;
using Terminaux.Colors;
using Terminaux.Reader;
using System.Text;
using Textify.Sequences.Builder.Types;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Writers.FancyWriters;
using Nitrocid.Misc.Text;
using Nitrocid.ConsoleBase.Writers.FancyWriters.Tools;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Buffered;
using Nitrocid.ConsoleBase.Writers.ConsoleWriters;

namespace Nitrocid.ConsoleBase.Inputs.Styles.InfoboxTitled
{
    /// <summary>
    /// Info box writer with input and color support
    /// </summary>
    public static class InfoBoxTitledInputColor
    {
        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxTitledPlainInput(string title, string text, params object[] vars) =>
            WriteInfoBoxTitledPlainInput(title, text,
                                   BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                                   BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                                   BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                                   BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxTitledPlainInput(string title, string text,
                                                    char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                                    char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, params object[] vars)
        {
            bool initialCursorVisible = ConsoleWrapper.CursorVisible;
            bool initialScreenIsNull = ScreenTools.CurrentScreen is null;
            var infoBoxScreenPart = new ScreenPart();
            var screen = new Screen();
            if (initialScreenIsNull)
            {
                infoBoxScreenPart.AddDynamicText(() =>
                {
                    KernelColorTools.SetConsoleColor(KernelColorTools.GetColor(KernelColorType.Background), true);
                    return CsiSequences.GenerateCsiEraseInDisplay(2) + CsiSequences.GenerateCsiCursorPosition(1, 1);
                });
                ScreenTools.SetCurrent(screen);
            }
            ScreenTools.CurrentScreen.AddBufferedPart("Informational box", infoBoxScreenPart);
            try
            {
                int rightMargin = 0;
                infoBoxScreenPart.AddDynamicText(() =>
                {
                    // Deal with the lines to actually fit text in the infobox
                    string finalInfoRendered = TextTools.FormatString(text, vars);
                    string[] splitLines = finalInfoRendered.ToString().SplitNewLines();
                    List<string> splitFinalLines = [];
                    foreach (var line in splitLines)
                    {
                        var lineSentences = TextTools.GetWrappedSentences(line, ConsoleWrapper.WindowWidth - 4);
                        foreach (var lineSentence in lineSentences)
                            splitFinalLines.Add(lineSentence);
                    }

                    // Trim the new lines until we reach a full line
                    for (int i = splitFinalLines.Count - 1; i >= 0; i--)
                    {
                        string line = splitFinalLines[i];
                        if (!string.IsNullOrWhiteSpace(line))
                            break;
                        splitFinalLines.RemoveAt(i);
                    }

                    // Fill the info box with text inside it
                    int maxWidth = ConsoleWrapper.WindowWidth - 4;
                    int maxHeight = splitFinalLines.Count + 5;
                    if (maxHeight >= ConsoleWrapper.WindowHeight)
                        maxHeight = ConsoleWrapper.WindowHeight - 4;
                    int maxRenderWidth = ConsoleWrapper.WindowWidth - 6;
                    int borderX = ConsoleWrapper.WindowWidth / 2 - maxWidth / 2 - 1;
                    int borderY = ConsoleWrapper.WindowHeight / 2 - maxHeight / 2 - 1;
                    var boxBuffer = new StringBuilder();
                    string border = BorderTextColor.RenderBorderTextPlain(title, borderX, borderY, maxWidth, maxHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar);
                    boxBuffer.Append(border);

                    // Render text inside it
                    ConsoleWrapper.CursorVisible = false;
                    for (int i = 0; i < splitFinalLines.Count; i++)
                    {
                        var line = splitFinalLines[i];
                        if (i % maxHeight == 0 && i > 0)
                        {
                            // Reached the end of the box. Bail, because we need to print the input box.
                            break;
                        }
                        boxBuffer.Append(
                            $"{CsiSequences.GenerateCsiCursorPosition(borderX + 2, borderY + 1 + i % maxHeight + 1)}" +
                            $"{line}"
                        );
                    }

                    // Render the final result
                    int inputPosX = borderX + 4;
                    rightMargin = inputPosX - 2;
                    int inputPosY = borderY + maxHeight - 3;
                    int maxInputWidth = maxWidth - inputPosX * 2 + 4;
                    boxBuffer.Append(
                        BorderColor.RenderBorderPlain(inputPosX, inputPosY, maxInputWidth, 1) +
                        CsiSequences.GenerateCsiCursorPosition(inputPosX + 2, inputPosY + 2)
                    );
                    return boxBuffer.ToString();
                });

                // Render the screen
                ScreenTools.Render();

                // Wait until the user presses any key to close the box
                var settings = new TermReaderSettings()
                {
                    RightMargin = rightMargin,
                };
                string input = Input.ReadLineWrapped("", "", settings);
                return input;
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
            finally
            {
                ConsoleWrapper.CursorVisible = initialCursorVisible;
                ScreenTools.CurrentScreen.RemoveBufferedPart("Informational box");
                if (initialScreenIsNull)
                    ScreenTools.UnsetCurrent(screen);
            }
            return "";
        }

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxTitledInput(string title, string text, params object[] vars) =>
            WriteInfoBoxTitledInputKernelColor(title, text,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        KernelColorType.Separator, KernelColorType.Background, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxTitledInputKernelColor(string title, string text, KernelColorType InfoBoxTitledColor, params object[] vars) =>
            WriteInfoBoxTitledInputKernelColor(title, text,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxTitledColor, KernelColorType.Background, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">InfoBoxTitled background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxTitledInputKernelColor(string title, string text, KernelColorType InfoBoxTitledColor, KernelColorType BackgroundColor, params object[] vars) =>
            WriteInfoBoxTitledInputKernelColor(title, text,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxTitledColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxTitledInputColor(string title, string text, ConsoleColors InfoBoxTitledColor, params object[] vars) =>
            WriteInfoBoxTitledInputColorBack(title, text,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(InfoBoxTitledColor), KernelColorTools.GetColor(KernelColorType.Background), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="BackgroundColor">InfoBoxTitled background color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxTitledInputColorBack(string title, string text, ConsoleColors InfoBoxTitledColor, ConsoleColors BackgroundColor, params object[] vars) =>
            WriteInfoBoxTitledInputColorBack(title, text,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(InfoBoxTitledColor), new Color(BackgroundColor), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxTitledInputColor(string title, string text, Color InfoBoxTitledColor, params object[] vars) =>
            WriteInfoBoxTitledInputColorBack(title, text,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxTitledColor, KernelColorTools.GetColor(KernelColorType.Background), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="BackgroundColor">InfoBoxTitled background color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxTitledInputColorBack(string title, string text, Color InfoBoxTitledColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxTitledInputColorBack(title, text,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxTitledColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxTitledInput(string title, string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, params object[] vars) =>
            WriteInfoBoxTitledInputKernelColor(title, text, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, KernelColorType.Separator, KernelColorType.Background, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxTitledInputKernelColor(string title, string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       KernelColorType InfoBoxTitledColor, params object[] vars) =>
            WriteInfoBoxTitledInputKernelColor(title, text, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, InfoBoxTitledColor, KernelColorType.Background, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">InfoBoxTitled background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxTitledInputKernelColor(string title, string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       KernelColorType InfoBoxTitledColor, KernelColorType BackgroundColor, params object[] vars) =>
            WriteInfoBoxTitledInputColorBack(title, text,
                UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar,
                KernelColorTools.GetColor(InfoBoxTitledColor), KernelColorTools.GetColor(BackgroundColor), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxTitledInputColor(string title, string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       ConsoleColors InfoBoxTitledColor, params object[] vars) =>
            WriteInfoBoxTitledInputColorBack(title, text, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, new Color(InfoBoxTitledColor), KernelColorTools.GetColor(KernelColorType.Background), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color</param>
        /// <param name="BackgroundColor">InfoBoxTitled background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxTitledInputColorBack(string title, string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       ConsoleColors InfoBoxTitledColor, ConsoleColors BackgroundColor, params object[] vars) =>
            WriteInfoBoxTitledInputColorBack(title, text,
                UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar,
                new Color(InfoBoxTitledColor), new Color(BackgroundColor), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxTitledInputColor(string title, string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       Color InfoBoxTitledColor, params object[] vars) =>
            WriteInfoBoxTitledInputColorBack(title, text, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, InfoBoxTitledColor, KernelColorTools.GetColor(KernelColorType.Background), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color</param>
        /// <param name="BackgroundColor">InfoBoxTitled background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxTitledInputColorBack(string title, string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       Color InfoBoxTitledColor, Color BackgroundColor, params object[] vars)
        {
            bool initialCursorVisible = ConsoleWrapper.CursorVisible;
            bool initialScreenIsNull = ScreenTools.CurrentScreen is null;
            var infoBoxScreenPart = new ScreenPart();
            var screen = new Screen();
            if (initialScreenIsNull)
            {
                infoBoxScreenPart.AddDynamicText(() =>
                {
                    KernelColorTools.SetConsoleColor(KernelColorTools.GetColor(KernelColorType.Background), true);
                    return CsiSequences.GenerateCsiEraseInDisplay(2) + CsiSequences.GenerateCsiCursorPosition(1, 1);
                });
                ScreenTools.SetCurrent(screen);
            }
            ScreenTools.CurrentScreen.AddBufferedPart("Informational box", infoBoxScreenPart);
            try
            {
                int rightMargin = 0;
                infoBoxScreenPart.AddDynamicText(() =>
                {
                    // Deal with the lines to actually fit text in the infobox
                    string finalInfoRendered = TextTools.FormatString(text, vars);
                    string[] splitLines = finalInfoRendered.ToString().SplitNewLines();
                    List<string> splitFinalLines = [];
                    foreach (var line in splitLines)
                    {
                        var lineSentences = TextTools.GetWrappedSentences(line, ConsoleWrapper.WindowWidth - 4);
                        foreach (var lineSentence in lineSentences)
                            splitFinalLines.Add(lineSentence);
                    }

                    // Trim the new lines until we reach a full line
                    for (int i = splitFinalLines.Count - 1; i >= 0; i--)
                    {
                        string line = splitFinalLines[i];
                        if (!string.IsNullOrWhiteSpace(line))
                            break;
                        splitFinalLines.RemoveAt(i);
                    }

                    // Fill the info box with text inside it
                    int maxWidth = ConsoleWrapper.WindowWidth - 4;
                    int maxHeight = splitFinalLines.Count + 5;
                    if (maxHeight >= ConsoleWrapper.WindowHeight)
                        maxHeight = ConsoleWrapper.WindowHeight - 4;
                    int maxRenderWidth = ConsoleWrapper.WindowWidth - 6;
                    int borderX = ConsoleWrapper.WindowWidth / 2 - maxWidth / 2;
                    int borderY = ConsoleWrapper.WindowHeight / 2 - maxHeight / 2;
                    var boxBuffer = new StringBuilder();
                    string border = BorderTextColor.RenderBorderTextPlain(title, borderX, borderY, maxWidth, maxHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar);
                    boxBuffer.Append(
                        $"{InfoBoxTitledColor.VTSequenceForeground}" +
                        $"{BackgroundColor.VTSequenceBackground}" +
                        $"{border}"
                    );

                    // Render text inside it
                    ConsoleWrapper.CursorVisible = false;
                    for (int i = 0; i < splitFinalLines.Count; i++)
                    {
                        var line = splitFinalLines[i];
                        if (i % maxHeight == 0 && i > 0)
                        {
                            // Reached the end of the box. Bail, because we need to print the input box.
                            break;
                        }
                        boxBuffer.Append(
                            $"{CsiSequences.GenerateCsiCursorPosition(borderX + 2, borderY + 1 + i % maxHeight + 1)}" +
                            $"{line}"
                        );
                    }

                    // Write the input bar and set the cursor position
                    int inputPosX = borderX + 4;
                    rightMargin = inputPosX - 2;
                    int inputPosY = borderY + maxHeight - 3;
                    int maxInputWidth = maxWidth - inputPosX * 2 + 4;
                    boxBuffer.Append(
                        BorderColor.RenderBorder(inputPosX, inputPosY, maxInputWidth, 1, InfoBoxTitledColor, BackgroundColor) +
                        CsiSequences.GenerateCsiCursorPosition(inputPosX + 2, inputPosY + 2) +
                        $"{InfoBoxTitledColor.VTSequenceForeground}" +
                        $"{BackgroundColor.VTSequenceBackground}"
                    );
                    return boxBuffer.ToString();
                });

                // Render the screen
                ScreenTools.Render();

                // Wait until the user presses any key to close the box
                var settings = new TermReaderSettings()
                {
                    RightMargin = rightMargin,
                };
                string input = Input.ReadLineWrapped("", "", settings);
                return input;
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
            finally
            {
                TextWriterColor.WritePlain(
                    KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground +
                    KernelColorTools.GetColor(KernelColorType.Background).VTSequenceBackground
                , false);
                ConsoleWrapper.CursorVisible = initialCursorVisible;
                ScreenTools.CurrentScreen.RemoveBufferedPart("Informational box");
                if (initialScreenIsNull)
                    ScreenTools.UnsetCurrent(screen);
            }
            return "";
        }
    }
}
