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

using KS.ConsoleBase.Inputs;
using KS.ConsoleBase;
using KS.Kernel.Debugging;
using KS.Languages;
using System;
using System.IO;
using System.Text;
using System.Threading;
using KS.Kernel;
using SystemConsole = System.Console;
using TextEncoding = System.Text.Encoding;
using KS.Misc.Text;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using Terminaux.Sequences.Tools;
using System.Text.RegularExpressions;

namespace KS.Drivers.Console
{
    /// <summary>
    /// Base console driver
    /// </summary>
    public abstract class BaseConsoleDriver : IConsoleDriver
    {

        /// <inheritdoc/>
        public virtual string DriverName => "Default";

        /// <inheritdoc/>
        public virtual DriverTypes DriverType => DriverTypes.Console;

        /// <inheritdoc/>
        public virtual bool DriverInternal => false;

        /// <summary>
        /// Checks to see if the console has moved. Only set this to true if the console has really moved, for example, each call to
        /// setting cursor position, key reading, writing text, etc.
        /// </summary>
        protected bool _moved = false;

        private static bool _dumbSet = false;
        private static bool _dumb = true;

        /// <summary>
        /// Is the console a dumb console?
        /// </summary>
        public virtual bool IsDumb
        {
            get
            {
                try
                {
                    // Get terminal type
                    string TerminalType = KernelPlatform.GetTerminalType();

                    // Try to cache the value
                    if (!_dumbSet)
                    {
                        _dumbSet = true;
                        int _ = SystemConsole.CursorLeft;

                        // If it doesn't get here without throwing exceptions, assume console is dumb. Now, check to see if terminal type is dumb
                        if (TerminalType != "dumb" && TerminalType != "unknown")
                            _dumb = false;
                    }
                }
                catch { }
                return _dumb;
            }
        }

        /// <summary>
        /// Has the console moved? Should be set by Write*, Set*, and all console functions that have to do with moving the console.
        /// </summary>
        public virtual bool MovementDetected
        {
            get
            {
                bool moved = _moved;
                _moved = false;
                return moved;
            }
        }

        /// <inheritdoc/>
        public virtual int CursorLeft
        {
            get
            {
                if (IsDumb)
                    return 0;
                return SystemConsole.CursorLeft;
            }
            set
            {
                if (!IsDumb)
                    SystemConsole.CursorLeft = value;
                _moved = true;
            }
        }

        /// <inheritdoc/>
        public virtual int CursorTop
        {
            get
            {
                if (IsDumb)
                    return 0;
                return SystemConsole.CursorTop;
            }
            set
            {
                if (!IsDumb)
                    SystemConsole.CursorTop = value;
                _moved = true;
            }
        }

        /// <inheritdoc/>
        public virtual int WindowWidth
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return SystemConsole.WindowWidth;
            }
        }

        /// <inheritdoc/>
        public int WindowTop
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return SystemConsole.WindowTop;
            }
        }

        /// <inheritdoc/>
        public virtual int WindowHeight
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return SystemConsole.WindowHeight;
            }
        }

        /// <inheritdoc/>
        public virtual int BufferWidth
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return SystemConsole.BufferWidth;
            }
        }

        /// <inheritdoc/>
        public virtual int BufferHeight
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return SystemConsole.BufferHeight;
            }
        }

        /// <inheritdoc/>
        public virtual bool CursorVisible
        {
            set
            {
                if (!IsDumb)
                    SystemConsole.CursorVisible = value;
            }
        }

        /// <inheritdoc/>
        public virtual TextEncoding OutputEncoding
        {
            get
            {
                if (IsDumb)
                    return TextEncoding.Default;
                return SystemConsole.OutputEncoding;
            }
            set
            {
                if (!IsDumb)
                    SystemConsole.OutputEncoding = value;
            }
        }

        /// <inheritdoc/>
        public virtual TextEncoding InputEncoding
        {
            get
            {
                if (IsDumb)
                    return TextEncoding.Default;
                return SystemConsole.InputEncoding;
            }
            set
            {
                if (!IsDumb)
                    SystemConsole.InputEncoding = value;
            }
        }

        /// <inheritdoc/>
        public virtual bool KeyAvailable
        {
            get
            {
                if (IsDumb)
                    return false;
                return SystemConsole.KeyAvailable;
            }
        }

        /// <inheritdoc/>
        public virtual void Beep() =>
            SystemConsole.Beep();

        /// <inheritdoc/>
        public virtual void Clear(bool loadBack = false)
        {
            if (!IsDumb)
            {
                if (loadBack)
                    KernelColorTools.LoadBack();
                SystemConsole.Clear();
            }
        }

        /// <inheritdoc/>
        public virtual Stream OpenStandardError() =>
            SystemConsole.OpenStandardError();

        /// <inheritdoc/>
        public virtual Stream OpenStandardInput() =>
            SystemConsole.OpenStandardInput();

        /// <inheritdoc/>
        public virtual Stream OpenStandardOutput() =>
            SystemConsole.OpenStandardOutput();

        /// <inheritdoc/>
        public virtual ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            var keyInfo = SystemConsole.ReadKey(intercept);
            _moved = true;
            return keyInfo;
        }

        /// <inheritdoc/>
        public virtual void ResetColor()
        {
            if (!IsDumb)
                SystemConsole.ResetColor();
        }

        /// <inheritdoc/>
        public virtual void SetCursorPosition(int left, int top)
        {
            if (!IsDumb)
                SystemConsole.SetCursorPosition(left, top);
            _moved = true;
        }

        /// <inheritdoc/>
        public virtual void SetOut(TextWriter newOut)
        {
            // We need to reset dumb state because the new output may not support usual console features other then reading/writing.
            _dumbSet = false;
            _dumb = true;
            SystemConsole.SetOut(newOut);
        }

        /// <inheritdoc/>
        public virtual void Write(char value)
        {
            lock (TextWriterColor.WriteLock)
            {
                SystemConsole.Write(value);
                _moved = true;
            }
        }

        /// <inheritdoc/>
        public virtual void Write(string text)
        {
            lock (TextWriterColor.WriteLock)
            {
                SystemConsole.Write(text);
                _moved = true;
            }
        }

        /// <inheritdoc/>
        public virtual void Write(string text, params object[] args)
        {
            lock (TextWriterColor.WriteLock)
            {
                SystemConsole.Write(text, args);
                _moved = true;
            }
        }

        /// <inheritdoc/>
        public virtual void WriteLine()
        {
            lock (TextWriterColor.WriteLock)
            {
                SystemConsole.WriteLine();
                _moved = true;
            }
        }

        /// <inheritdoc/>
        public virtual void WriteLine(string text)
        {
            lock (TextWriterColor.WriteLock)
            {
                SystemConsole.WriteLine(text);
                _moved = true;
            }
        }

        /// <inheritdoc/>
        public virtual void WriteLine(string text, params object[] args)
        {
            lock (TextWriterColor.WriteLock)
            {
                SystemConsole.WriteLine(text, args);
                _moved = true;
            }
        }

        /// <inheritdoc/>
        public virtual void WritePlain(string Text, bool Line, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Actually write
                    if (Line)
                    {
                        if (vars.Length > 0)
                        {
                            WriteLine(Text, vars);
                        }
                        else
                        {
                            WriteLine(Text);
                        }
                    }
                    else if (vars.Length > 0)
                    {
                        Write(Text, vars);
                    }
                    else
                    {
                        Write(Text);
                    }
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <inheritdoc/>
        public virtual void WritePlain()
        {
            lock (TextWriterColor.WriteLock)
            {
                ConsoleWrapper.WriteLine();
            }
        }

        /// <inheritdoc/>
        public virtual void WriteSlowlyPlain(string msg, bool Line, double MsEachLetter, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Format string as needed
                    if (vars.Length > 0)
                        msg = TextTools.FormatString(msg, vars);

                    // Grab each VT sequence from the message and fetch their indexes
                    var sequences = VtSequenceTools.MatchVTSequences(msg);
                    int vtSeqIdx = 0;

                    // Write text slowly
                    for (int i = 0; i < msg.Length; i++)
                    {
                        // Sleep for a while
                        Thread.Sleep((int)Math.Round(MsEachLetter));

                        // Write a character individually
                        ConsoleWrapper.Write(BufferChar(msg, sequences, ref i, ref vtSeqIdx, out _));
                    }

                    // If we're writing a new line, write it
                    if (Line)
                        ConsoleWrapper.WriteLine();
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <inheritdoc/>
        public virtual void WriteWherePlain(string msg, int Left, int Top, params object[] vars) =>
            WriteWherePlain(msg, Left, Top, false, 0, vars);

        /// <inheritdoc/>
        public virtual void WriteWherePlain(string msg, int Left, int Top, bool Return, params object[] vars) =>
            WriteWherePlain(msg, Left, Top, Return, 0, vars);

        /// <inheritdoc/>
        public virtual void WriteWherePlain(string msg, int Left, int Top, bool Return, int RightMargin, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Format the message as necessary
                    if (vars.Length > 0)
                        msg = TextTools.FormatString(msg, vars);

                    // Write text in another place. By the way, we check the text for newlines and console width excess
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
                        // We can now check to see if we're writing a letter past the console window width
                        string MessageParagraph = Paragraphs[MessageParagraphIndex];

                        // Grab each VT sequence from the paragraph and fetch their indexes
                        var sequences = VtSequenceTools.MatchVTSequences(MessageParagraph);
                        int vtSeqIdx = 0;

                        // Now, parse every character
                        int pos = OldLeft;
                        for (int i = 0; i < MessageParagraph.Length; i++)
                        {
                            if (MessageParagraph[i] == '\n' || RightMargin > 0 && pos > width)
                            {
                                buffered.Append($"{CharManager.GetEsc()}[1B");
                                buffered.Append($"{CharManager.GetEsc()}[{Left + 1}G");
                                pos = OldLeft;
                            }

                            // Write a character individually
                            if (MessageParagraph[i] != '\n')
                            {
                                string bufferedChar = BufferChar(MessageParagraph, sequences, ref i, ref vtSeqIdx, out bool isVtSequence);
                                buffered.Append(bufferedChar);
                                if (!isVtSequence)
                                    pos += bufferedChar.Length;
                            }
                        }

                        // We're starting with the new paragraph, so we increase the CursorTop value by 1.
                        if (MessageParagraphIndex != Paragraphs.Length - 1)
                        {
                            buffered.Append($"{CharManager.GetEsc()}[1B");
                            buffered.Append($"{CharManager.GetEsc()}[{Left + 1}G");
                            pos = OldLeft;
                        }
                    }

                    // Write the resulting buffer
                    ConsoleWrapper.Write(buffered.ToString());

                    // Return if we're told to
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

        /// <inheritdoc/>
        public virtual void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, params object[] vars) =>
            WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, false, vars);

        /// <inheritdoc/>
        public virtual void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, params object[] vars) =>
            WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, Return, 0, vars);

        /// <inheritdoc/>
        public virtual void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, int RightMargin, params object[] vars)
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
                                string bufferedChar = BufferChar(MessageParagraph, sequences, ref i, ref vtSeqIdx, out bool isVtSequence);
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

        /// <inheritdoc/>
        public virtual void WriteWrappedPlain(string Text, bool Line, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                var LinesMade = 0;
                try
                {
                    // Format string as needed
                    if (vars.Length > 0)
                        Text = TextTools.FormatString(Text, vars);
                    Text = Text.Replace(Convert.ToChar(13), default);

                    // First, split the text to wrap
                    string[] sentences = TextTools.GetWrappedSentences(Text, WindowWidth);

                    // Iterate through sentences
                    var buffered = new StringBuilder();
                    foreach (string sentence in sentences)
                    {
                        // Grab each VT sequence from the paragraph and fetch their indexes
                        var sequences = VtSequenceTools.MatchVTSequences(sentence);
                        int vtSeqIdx = 0;
                        for (int i = 0; i < sentence.Length; i++)
                        {
                            char TextChar = sentence[i];

                            // Write a character individually
                            if (LinesMade == ConsoleWrapper.WindowHeight - 1)
                            {
                                ConsoleWrapper.Write(buffered.ToString());
                                buffered.Clear();
                                if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                    return;
                                LinesMade = 0;
                            }
                            buffered.Append(BufferChar(sentence, sequences, ref i, ref vtSeqIdx, out _));
                        }
                        buffered.AppendLine();
                        LinesMade++;
                    }
                    ConsoleWrapper.Write(buffered.ToString());
                    buffered.Clear();
                    if (Line)
                        ConsoleWrapper.WriteLine();
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        internal static string BufferChar(string text, MatchCollection[] sequencesCollections, ref int i, ref int vtSeqIdx, out bool isVtSequence)
        {
            // Before buffering the character, check to see if we're surrounded by the VT sequence. This is to work around
            // the problem in .NET 6.0 Linux that prevents it from actually parsing the VT sequences like it's supposed to
            // do in Windows.
            //
            // Windows 10, Windows 11, and higher contain cmd.exe that checks to see if we passed it the escape character
            // alone, and it tries to parse each sequence passed to it.
            //
            // Linux, on the other hand, the terminal emulator has a completely different behavior, because it just omits
            // the escape character, which results in the entire sequence being printed except the Escape \u001b key, which
            // is not the way that it's supposed to work.
            //
            // To overcome this limitation, we need to print the whole sequence to the console found by the virtual terminal
            // control sequence matcher to match how it works on Windows.
            char ch = text[i];
            string seq = "";
            bool vtSeq = false;
            foreach (var sequences in sequencesCollections)
            {
                if (sequences.Count > 0 && sequences[vtSeqIdx].Index == i)
                {
                    // We're at an index which is the same as the captured VT sequence. Get the sequence
                    seq = sequences[vtSeqIdx].Value;
                    vtSeq = true;

                    // Raise the index in case we have the next sequence, but only if we're sure that we have another
                    if (vtSeqIdx + 1 < sequences.Count)
                        vtSeqIdx++;

                    // Raise the paragraph index by the length of the sequence
                    i += seq.Length - 1;
                }
            }
            isVtSequence = vtSeq;
            return !string.IsNullOrEmpty(seq) ? seq : ch.ToString();
        }

    }
}
