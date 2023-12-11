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
using KS.Kernel.Debugging;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Text;
using System.Collections.Generic;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters.Tools;
using Terminaux.Colors;
using System.Text;
using Terminaux.Sequences.Builder.Types;
using KS.ConsoleBase.Writers.FancyWriters;

namespace KS.ConsoleBase.Inputs.Styles.Infobox
{
    /// <summary>
    /// Info box writer with progress and color support
    /// </summary>
    public static class InfoBoxProgressColor
    {
        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgressPlain(double progress, string text, params object[] vars) =>
            WriteInfoBoxProgressPlain(progress, text,
                             BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                             BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                             BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                             BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
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
        public static void WriteInfoBoxProgressPlain(double progress, string text,
                                            char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                            char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, params object[] vars)
        {
            bool initialCursorVisible = ConsoleWrapper.CursorVisible;
            try
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
                string border = BorderColor.RenderBorderPlain(borderX, borderY, maxWidth, maxHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar);
                boxBuffer.Append(border);

                // Render text inside it
                ConsoleWrapper.CursorVisible = false;
                for (int i = 0; i < splitFinalLines.Count; i++)
                {
                    var line = splitFinalLines[i];
                    if (i % (maxHeight - 5) == 0 && i > 0)
                    {
                        // Reached the end of the box. Bail, because we need to print the progress.
                        break;
                    }
                    boxBuffer.Append($"{CsiSequences.GenerateCsiCursorPosition(borderX + 2, borderY + 1 + i % maxHeight + 1)}{line}");
                }

                // Render the final result and write the progress bar
                int progressPosX = borderX + 4;
                int progressPosY = borderY + maxHeight - 3;
                int maxProgressWidth = maxWidth - 4;
                TextWriterColor.WritePlain(boxBuffer.ToString(), false);
                ProgressBarColor.WriteProgressPlain(progress, progressPosX, progressPosY, maxProgressWidth);
                boxBuffer.Clear();
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
            finally
            {
                ConsoleWrapper.CursorVisible = initialCursorVisible;
            }
        }

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgress(double progress, string text, params object[] vars) =>
            WriteInfoBoxProgressKernelColor(progress, text, true,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        KernelColorType.Separator, KernelColorType.Background, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgress(double progress, string text, bool waitForInput, params object[] vars) =>
            WriteInfoBoxProgressKernelColor(progress, text, waitForInput,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        KernelColorType.Separator, KernelColorType.Background, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgressKernelColor(double progress, string text, KernelColorType InfoBoxProgressColor, params object[] vars) =>
            WriteInfoBoxProgressKernelColor(progress, text, true,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxProgressColor, KernelColorType.Background, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgressKernelColor(double progress, string text, bool waitForInput, KernelColorType InfoBoxProgressColor, params object[] vars) =>
            WriteInfoBoxProgressKernelColor(progress, text, waitForInput,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxProgressColor, KernelColorType.Background, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">InfoBoxProgress background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgressKernelColor(double progress, string text, KernelColorType InfoBoxProgressColor, KernelColorType BackgroundColor, params object[] vars) =>
            WriteInfoBoxProgressKernelColor(progress, text, true,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxProgressColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">InfoBoxProgress background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgressKernelColor(double progress, string text, bool waitForInput, KernelColorType InfoBoxProgressColor, KernelColorType BackgroundColor, params object[] vars) =>
            WriteInfoBoxProgressKernelColor(progress, text, waitForInput,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxProgressColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgressColor(double progress, string text, ConsoleColors InfoBoxProgressColor, params object[] vars) =>
            WriteInfoBoxProgressColorBack(progress, text, true,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(InfoBoxProgressColor), KernelColorTools.GetColor(KernelColorType.Background), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgressColor(double progress, string text, bool waitForInput, ConsoleColors InfoBoxProgressColor, params object[] vars) =>
            WriteInfoBoxProgressColorBack(progress, text, waitForInput,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(InfoBoxProgressColor), KernelColorTools.GetColor(KernelColorType.Background), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="BackgroundColor">InfoBoxProgress background color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgressColorBack(double progress, string text, ConsoleColors InfoBoxProgressColor, ConsoleColors BackgroundColor, params object[] vars) =>
            WriteInfoBoxProgressColorBack(progress, text, true,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(InfoBoxProgressColor), new Color(BackgroundColor), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="BackgroundColor">InfoBoxProgress background color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgressColorBack(double progress, string text, bool waitForInput, ConsoleColors InfoBoxProgressColor, ConsoleColors BackgroundColor, params object[] vars) =>
            WriteInfoBoxProgressColorBack(progress, text, waitForInput,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(InfoBoxProgressColor), new Color(BackgroundColor), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgressColor(double progress, string text, Color InfoBoxProgressColor, params object[] vars) =>
            WriteInfoBoxProgressColorBack(progress, text, true,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxProgressColor, KernelColorTools.GetColor(KernelColorType.Background), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgressColor(double progress, string text, bool waitForInput, Color InfoBoxProgressColor, params object[] vars) =>
            WriteInfoBoxProgressColorBack(progress, text, waitForInput,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxProgressColor, KernelColorTools.GetColor(KernelColorType.Background), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="BackgroundColor">InfoBoxProgress background color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgressColorBack(double progress, string text, Color InfoBoxProgressColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxProgressColorBack(progress, text, true,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxProgressColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="BackgroundColor">InfoBoxProgress background color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgressColorBack(double progress, string text, bool waitForInput, Color InfoBoxProgressColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxProgressColorBack(progress, text, waitForInput,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxProgressColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
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
        public static void WriteInfoBoxProgress(double progress, string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, params object[] vars) =>
            WriteInfoBoxProgressKernelColor(progress, text, true,
                UpperLeftCornerChar, LowerLeftCornerChar,
                UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar,
                LeftFrameChar, RightFrameChar,
                KernelColorType.Separator, KernelColorType.Background, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgress(double progress, string text, bool waitForInput,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, params object[] vars) =>
            WriteInfoBoxProgressKernelColor(progress, text, waitForInput,
                UpperLeftCornerChar, LowerLeftCornerChar,
                UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar,
                LeftFrameChar, RightFrameChar,
                KernelColorType.Separator, KernelColorType.Background, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgressKernelColor(double progress, string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       KernelColorType InfoBoxProgressColor, params object[] vars) =>
            WriteInfoBoxProgressKernelColor(progress, text, true,
                UpperLeftCornerChar, LowerLeftCornerChar,
                UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar,
                LeftFrameChar, RightFrameChar,
                InfoBoxProgressColor, KernelColorType.Background, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgressKernelColor(double progress, string text, bool waitForInput,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       KernelColorType InfoBoxProgressColor, params object[] vars) =>
            WriteInfoBoxProgressKernelColor(progress, text, waitForInput,
                UpperLeftCornerChar, LowerLeftCornerChar,
                UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar,
                LeftFrameChar, RightFrameChar,
                InfoBoxProgressColor, KernelColorType.Background, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">InfoBoxProgress background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgressKernelColor(double progress, string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       KernelColorType InfoBoxProgressColor, KernelColorType BackgroundColor, params object[] vars) =>
            WriteInfoBoxProgressKernelColor(progress, text, true,
                UpperLeftCornerChar, LowerLeftCornerChar,
                UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar,
                LeftFrameChar, RightFrameChar,
                InfoBoxProgressColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">InfoBoxProgress background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgressKernelColor(double progress, string text, bool waitForInput,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       KernelColorType InfoBoxProgressColor, KernelColorType BackgroundColor, params object[] vars) =>
            WriteInfoBoxProgressColorBack(
                progress, text, waitForInput,
                UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar,
                KernelColorTools.GetColor(InfoBoxProgressColor), KernelColorTools.GetColor(BackgroundColor), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgressColor(double progress, string text, bool waitForInput,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       ConsoleColors InfoBoxProgressColor, params object[] vars) =>
            WriteInfoBoxProgressColorBack(progress, text, waitForInput, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, new Color(InfoBoxProgressColor), KernelColorTools.GetColor(KernelColorType.Background), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color</param>
        /// <param name="BackgroundColor">InfoBoxProgress background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgressColorBack(double progress, string text, bool waitForInput,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       ConsoleColors InfoBoxProgressColor, ConsoleColors BackgroundColor, params object[] vars) =>
            WriteInfoBoxProgressColorBack(
                progress, text, waitForInput,
                UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar,
                new Color(InfoBoxProgressColor), new Color(BackgroundColor), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgressColor(double progress, string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       Color InfoBoxProgressColor, params object[] vars) =>
            WriteInfoBoxProgressColorBack(progress, text, true, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, InfoBoxProgressColor, KernelColorTools.GetColor(KernelColorType.Background), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color</param>
        /// <param name="BackgroundColor">InfoBoxProgress background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgressColorBack(double progress, string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       Color InfoBoxProgressColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxProgressColorBack(progress, text, true, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, InfoBoxProgressColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgressColor(double progress, string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       ConsoleColors InfoBoxProgressColor, params object[] vars) =>
            WriteInfoBoxProgressColorBack(progress, text, true, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, new Color(InfoBoxProgressColor), KernelColorTools.GetColor(KernelColorType.Background), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color</param>
        /// <param name="BackgroundColor">InfoBoxProgress background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgressColorBack(double progress, string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       ConsoleColors InfoBoxProgressColor, ConsoleColors BackgroundColor, params object[] vars) =>
            WriteInfoBoxProgressColorBack(progress, text, true, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, InfoBoxProgressColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgressColor(double progress, string text, bool waitForInput,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       Color InfoBoxProgressColor, params object[] vars) =>
            WriteInfoBoxProgressColorBack(progress, text, waitForInput, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, InfoBoxProgressColor, KernelColorTools.GetColor(KernelColorType.Background), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color</param>
        /// <param name="BackgroundColor">InfoBoxProgress background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgressColorBack(double progress, string text, bool waitForInput,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       Color InfoBoxProgressColor, Color BackgroundColor, params object[] vars)
        {
            bool initialCursorVisible = ConsoleWrapper.CursorVisible;
            try
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
                string border = BorderColor.RenderBorderPlain(borderX, borderY, maxWidth, maxHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar);
                boxBuffer.Append(
                    $"{InfoBoxProgressColor.VTSequenceForeground}" +
                    $"{BackgroundColor.VTSequenceBackground}" +
                    $"{border}"
                );

                // Render text inside it
                ConsoleWrapper.CursorVisible = false;
                for (int i = 0; i < splitFinalLines.Count; i++)
                {
                    var line = splitFinalLines[i];
                    if (i % (maxHeight - 5) == 0 && i > 0)
                    {
                        // Reached the end of the box. Bail, because we need to print the progress.
                        break;
                    }
                    boxBuffer.Append($"{CsiSequences.GenerateCsiCursorPosition(borderX + 2, borderY + 1 + i % maxHeight + 1)}{line}");
                }

                // Render the final result and write the progress bar
                int progressPosX = borderX + 4;
                int progressPosY = borderY + maxHeight - 3;
                int maxProgressWidth = maxWidth - 4;
                boxBuffer.Append(
                    KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground +
                    KernelColorTools.GetColor(KernelColorType.Background).VTSequenceBackground
                );
                TextWriterColor.WritePlain(boxBuffer.ToString(), false);
                ProgressBarColor.WriteProgress(progress, progressPosX, progressPosY, progressPosX * 2 + 2, InfoBoxProgressColor, InfoBoxProgressColor, BackgroundColor);
                boxBuffer.Clear();
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
            finally
            {
                ConsoleWrapper.CursorVisible = initialCursorVisible;
            }
        }
    }
}
