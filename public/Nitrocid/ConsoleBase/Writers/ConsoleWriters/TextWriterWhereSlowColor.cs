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
using Terminaux.Colors;
using KS.Misc.Text;
using System.Text;
using Terminaux.Sequences.Tools;

namespace KS.ConsoleBase.Writers.ConsoleWriters
{
    /// <summary>
    /// Console text writer with color support (slow positional write)
    /// </summary>
    public static class TextWriterWhereSlowColor
    {

        /// <summary>
        /// Outputs the text into the terminal prompt with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, params object[] vars) =>
            WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, false, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, params object[] vars) =>
            WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, Return, 0, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, int RightMargin, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Format string as needed
                    if (vars.Length > 0)
                        msg = TextTools.FormatString(msg, vars);

                    // Write text in another place slowly
                    int OldLeft = ConsoleWrapper.CursorLeft;
                    int OldTop = ConsoleWrapper.CursorTop;
                    int width = ConsoleWrapper.WindowWidth - RightMargin;
                    var Paragraphs = msg.SplitNewLines();
                    if (RightMargin > 0)
                        Paragraphs = TextTools.GetWrappedSentences(msg, width);
                    var buffered = new StringBuilder();
                    ConsoleWrapper.SetCursorPosition(Left, Top);
                    for (int MessageParagraphIndex = 0; MessageParagraphIndex <= Paragraphs.Length - 1; MessageParagraphIndex++)
                    {
                        // Get the paragraph
                        string MessageParagraph = Paragraphs[MessageParagraphIndex];

                        // Grab each VT sequence from the paragraph and fetch their indexes
                        var sequences = VtSequenceTools.MatchVTSequences(MessageParagraph);
                        int vtSeqIdx = 0;

                        // Buffer the characters and then write when done
                        int pos = OldLeft;
                        for (int i = 0; i < MessageParagraph.Length; i++)
                        {
                            // Sleep for a few milliseconds
                            Thread.Sleep((int)Math.Round(MsEachLetter));
                            if (MessageParagraph[i] == '\n' || RightMargin > 0 && pos > width)
                            {
                                buffered.Append($"{CharManager.GetEsc()}[1B");
                                buffered.Append($"{CharManager.GetEsc()}[{Left + 1}G");
                                pos = OldLeft;
                            }

                            // Write a character individually
                            if (MessageParagraph[i] != '\n')
                            {
                                string bufferedChar = ConsoleExtensions.BufferChar(MessageParagraph, sequences, ref i, ref vtSeqIdx, out bool isVtSequence);
                                buffered.Append(bufferedChar);
                                if (!isVtSequence)
                                    pos += bufferedChar.Length;
                            }

                            // If we're writing a new line, write it
                            if (Line)
                                ConsoleWrapper.WriteLine(buffered.ToString());
                            else
                                ConsoleWrapper.Write(buffered.ToString());
                        }

                        // We're starting with the new paragraph, so we increase the CursorTop value by 1.
                        if (MessageParagraphIndex != Paragraphs.Length - 1)
                        {
                            buffered.Append($"{CharManager.GetEsc()}[1B");
                            buffered.Append($"{CharManager.GetEsc()}[{Left + 1}G");
                            pos = OldLeft;
                        }
                    }
                    if (Return)
                        ConsoleWrapper.SetCursorPosition(OldLeft, OldTop);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowly(string msg, bool Line, int Left, int Top, double MsEachLetter, params object[] vars) =>
            WriteWhereSlowly(msg, Line, Left, Top, MsEachLetter, false, 0, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowly(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, params object[] vars) =>
            WriteWhereSlowly(msg, Line, Left, Top, MsEachLetter, Return, 0, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowly(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, int RightMargin, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Write text in another place slowly
                    WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, Return, RightMargin, vars);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyKernelColor(string msg, bool Line, int Left, int Top, double MsEachLetter, KernelColorType colorType, params object[] vars) =>
            WriteWhereSlowlyKernelColor(msg, Line, Left, Top, MsEachLetter, false, 0, colorType, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyKernelColor(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, KernelColorType colorType, params object[] vars) =>
            WriteWhereSlowlyKernelColor(msg, Line, Left, Top, MsEachLetter, Return, 0, colorType, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyKernelColor(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, int RightMargin, KernelColorType colorType, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                    KernelColorTools.SetConsoleColor(colorType);

                    // Write text in another place slowly
                    WriteWhereSlowly(msg, Line, Left, Top, MsEachLetter, Return, RightMargin, vars);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyKernelColor(string msg, bool Line, int Left, int Top, double MsEachLetter, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] vars) =>
            WriteWhereSlowlyKernelColor(msg, Line, Left, Top, MsEachLetter, false, 0, colorTypeForeground, colorTypeBackground, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyKernelColor(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] vars) =>
            WriteWhereSlowlyKernelColor(msg, Line, Left, Top, MsEachLetter, Return, 0, colorTypeForeground, colorTypeBackground, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyKernelColor(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, int RightMargin, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                    KernelColorTools.SetConsoleColor(colorTypeForeground);
                    KernelColorTools.SetConsoleColor(colorTypeBackground, true);

                    // Write text in another place slowly
                    WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, Return, RightMargin, vars);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyColor(string msg, bool Line, int Left, int Top, double MsEachLetter, ConsoleColors color, params object[] vars) =>
            WriteWhereSlowlyColor(msg, Line, Left, Top, MsEachLetter, false, 0, color, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyColor(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, ConsoleColors color, params object[] vars) =>
            WriteWhereSlowlyColor(msg, Line, Left, Top, MsEachLetter, Return, 0, color, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyColor(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, int RightMargin, ConsoleColors color, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    KernelColorTools.SetConsoleColor(new Color(color));
                    KernelColorTools.SetConsoleColor(KernelColorType.Background, true);

                    // Write text in another place slowly
                    WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, Return, RightMargin, vars);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyColorBack(string msg, bool Line, int Left, int Top, double MsEachLetter, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] vars) =>
            WriteWhereSlowlyColorBack(msg, Line, Left, Top, MsEachLetter, false, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyColorBack(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] vars) =>
            WriteWhereSlowlyColorBack(msg, Line, Left, Top, MsEachLetter, Return, 0, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyColorBack(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, int RightMargin, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    KernelColorTools.SetConsoleColor(new Color(ForegroundColor));
                    KernelColorTools.SetConsoleColor(new Color(BackgroundColor));

                    // Write text in another place slowly
                    WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, Return, RightMargin, vars);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyColor(string msg, bool Line, int Left, int Top, double MsEachLetter, Color color, params object[] vars) =>
            WriteWhereSlowlyColor(msg, Line, Left, Top, MsEachLetter, false, color, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyColor(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, Color color, params object[] vars) =>
            WriteWhereSlowlyColor(msg, Line, Left, Top, MsEachLetter, Return, 0, color, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyColor(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, int RightMargin, Color color, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    KernelColorTools.SetConsoleColor(color);
                    KernelColorTools.SetConsoleColor(KernelColorType.Background, true);

                    // Write text in another place slowly
                    WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, Return, RightMargin, vars);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyColorBack(string msg, bool Line, int Left, int Top, double MsEachLetter, Color ForegroundColor, Color BackgroundColor, params object[] vars) =>
            WriteWhereSlowlyColorBack(msg, Line, Left, Top, MsEachLetter, false, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyColorBack(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, Color ForegroundColor, Color BackgroundColor, params object[] vars) =>
            WriteWhereSlowlyColorBack(msg, Line, Left, Top, MsEachLetter, Return, 0, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyColorBack(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, int RightMargin, Color ForegroundColor, Color BackgroundColor, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    KernelColorTools.SetConsoleColor(ForegroundColor);
                    KernelColorTools.SetConsoleColor(BackgroundColor, true);

                    // Write text in another place slowly
                    WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, Return, RightMargin, vars);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

    }
}
