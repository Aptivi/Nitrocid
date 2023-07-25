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

using KS.ConsoleBase.Inputs;
using KS.ConsoleBase;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using System;
using System.IO;
using System.Text;
using System.Threading;
using KS.Kernel;
using ColorSeq;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;
using con = System.Console;
using VT.NET.Tools;
using KS.Misc.Text;

namespace KS.Drivers.Console.Consoles
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
                        int _ = con.CursorLeft;

                        // If it doesn't get here without throwing exceptions, assume console is dumb. Now, check to see if terminal type is dumb
                        if (TerminalType != "dumb" && TerminalType != "unknown")
                            _dumb = false;
                    }
                }
                catch { }
                return _dumb;
            }
        }

        /// <inheritdoc/>
        public virtual TextWriter Out => con.Out;

        /// <inheritdoc/>
        public virtual int CursorLeft
        {
            get
            {
                if (IsDumb)
                    return 0;
                return con.CursorLeft;
            }
            set
            {
                if (!IsDumb)
                    con.CursorLeft = value;
            }
        }

        /// <inheritdoc/>
        public virtual int CursorTop
        {
            get
            {
                if (IsDumb)
                    return 0;
                return con.CursorTop;
            }
            set
            {
                if (!IsDumb)
                    con.CursorTop = value;
            }
        }

        /// <inheritdoc/>
        public virtual int WindowWidth
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return con.WindowWidth;
            }
        }

        /// <inheritdoc/>
        public int WindowTop
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return con.WindowTop;
            }
        }

        /// <inheritdoc/>
        public virtual int WindowHeight
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return con.WindowHeight;
            }
        }

        /// <inheritdoc/>
        public virtual int BufferWidth
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return con.BufferWidth;
            }
        }

        /// <inheritdoc/>
        public virtual int BufferHeight
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return con.BufferHeight;
            }
        }

        /// <inheritdoc/>
        public virtual ConsoleColor ForegroundColor
        {
            get
            {
                if (IsDumb)
                    return ConsoleColor.White;
                return con.ForegroundColor;
            }
            set
            {
                if (!IsDumb)
                    con.ForegroundColor = value;
                ColorTools.cachedForegroundColor = new Color(Convert.ToInt32(value)).VTSequenceForeground;
            }
        }

        /// <inheritdoc/>
        public virtual ConsoleColor BackgroundColor
        {
            get
            {
                if (IsDumb)
                    return ConsoleColor.Black;
                return con.BackgroundColor;
            }
            set
            {
                if (!IsDumb)
                    con.BackgroundColor = value;
                ColorTools.cachedBackgroundColor = new Color(Convert.ToInt32(value)).VTSequenceBackground;
            }
        }

        /// <inheritdoc/>
        public virtual bool CursorVisible
        {
            set
            {
                if (!IsDumb)
                    con.CursorVisible = value;
            }
        }

        /// <inheritdoc/>
        public virtual Encoding OutputEncoding
        {
            get
            {
                if (IsDumb)
                    return Encoding.Default;
                return con.OutputEncoding;
            }
            set
            {
                if (!IsDumb)
                    con.OutputEncoding = value;
            }
        }

        /// <inheritdoc/>
        public virtual Encoding InputEncoding
        {
            get
            {
                if (IsDumb)
                    return Encoding.Default;
                return con.InputEncoding;
            }
            set
            {
                if (!IsDumb)
                    con.InputEncoding = value;
            }
        }

        /// <inheritdoc/>
        public virtual bool KeyAvailable
        {
            get
            {
                if (IsDumb)
                    return false;
                return con.KeyAvailable;
            }
        }

        /// <inheritdoc/>
        public virtual void Beep() =>
            con.Beep();

        /// <inheritdoc/>
        public virtual void Clear(bool loadBack = false)
        {
            if (!IsDumb)
            {
                if (loadBack)
                    ColorTools.LoadBack();
                con.Clear();
            }
        }

        /// <inheritdoc/>
        public virtual Stream OpenStandardError() =>
            con.OpenStandardError();

        /// <inheritdoc/>
        public virtual Stream OpenStandardInput() =>
            con.OpenStandardInput();

        /// <inheritdoc/>
        public virtual Stream OpenStandardOutput() =>
            con.OpenStandardOutput();

        /// <inheritdoc/>
        public virtual ConsoleKeyInfo ReadKey(bool intercept = false) =>
            con.ReadKey(intercept);

        /// <inheritdoc/>
        public virtual void ResetColor()
        {
            if (!IsDumb)
                con.ResetColor();
        }

        /// <inheritdoc/>
        public virtual void SetCursorPosition(int left, int top)
        {
            if (!IsDumb)
                con.SetCursorPosition(left, top);
        }

        /// <inheritdoc/>
        public virtual void SetOut(TextWriter newOut)
        {
            // We need to reset dumb state because the new output may not support usual console features other then reading/writing.
            _dumbSet = false;
            _dumb = true;
            con.SetOut(newOut);
        }

        /// <inheritdoc/>
        public virtual void Write(char value) =>
            con.Write(value);

        /// <inheritdoc/>
        public virtual void Write(string text) =>
            con.Write(text);

        /// <inheritdoc/>
        public virtual void Write(string text, params object[] args) =>
            con.Write(text, args);

        /// <inheritdoc/>
        public virtual void WriteLine() =>
            con.WriteLine();

        /// <inheritdoc/>
        public virtual void WriteLine(string text) =>
            con.WriteLine(text);

        /// <inheritdoc/>
        public virtual void WriteLine(string text, params object[] args) =>
            con.WriteLine(text, args);

        /// <inheritdoc/>
        public virtual void WritePlain(string Text, bool Line, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Get the filtered positions first.
                    int FilteredLeft = default, FilteredTop = default;
                    var pos = ConsoleExtensions.GetFilteredPositions(Line ? Text + "\n" : Text, vars);
                    FilteredLeft = pos.Item1;
                    FilteredTop = pos.Item2;

                    // Actually write
                    if (Line)
                    {
                        if (!(vars.Length == 0))
                        {
                            ConsoleWrapper.WriteLine(Text, vars);
                        }
                        else
                        {
                            ConsoleWrapper.WriteLine(Text);
                        }
                    }
                    else if (!(vars.Length == 0))
                    {
                        ConsoleWrapper.Write(Text, vars);
                    }
                    else
                    {
                        ConsoleWrapper.Write(Text);
                    }

                    // Return to the processed position
                    ConsoleWrapper.SetCursorPosition(FilteredLeft, FilteredTop);
                }
                catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
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
                    if (!(vars.Length == 0))
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
                        ConsoleWrapper.Write(BufferChar(msg, ref i, ref vtSeqIdx));
                    }

                    // If we're writing a new line, write it
                    if (Line)
                        ConsoleWrapper.WriteLine();
                }
                catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
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
                    if (!(vars.Length == 0))
                        msg = TextTools.FormatString(msg, vars);

                    // Write text in another place. By the way, we check the text for newlines and console width excess
                    int OldLeft = ConsoleWrapper.CursorLeft;
                    int OldTop = ConsoleWrapper.CursorTop;
                    var Paragraphs = msg.SplitNewLines();
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
                        for (int i = 0; i < MessageParagraph.Length; i++)
                        {
                            if (ConsoleWrapper.CursorLeft == ConsoleWrapper.WindowWidth - RightMargin ||
                                MessageParagraph[i] == '\n')
                            {
                                buffered.Append($"{CharManager.GetEsc()}[1B");
                                buffered.Append($"{CharManager.GetEsc()}[{Left + 1}G");
                            }

                            // Write a character individually
                            if (MessageParagraph[i] != '\n')
                                buffered.Append(BufferChar(MessageParagraph, ref i, ref vtSeqIdx));
                        }

                        // We're starting with the new paragraph, so we increase the CursorTop value by 1.
                        if (!(MessageParagraphIndex == Paragraphs.Length - 1))
                        {
                            buffered.Append($"{CharManager.GetEsc()}[1B");
                            buffered.Append($"{CharManager.GetEsc()}[{Left + 1}G");
                        }
                    }

                    // Write the resulting buffer
                    ConsoleWrapper.Write(buffered.ToString());

                    // Return if we're told to
                    if (Return)
                        ConsoleWrapper.SetCursorPosition(OldLeft, OldTop);
                }
                catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
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
                    if (!(vars.Length == 0))
                        msg = TextTools.FormatString(msg, vars);

                    // Write text in another place slowly
                    int OldLeft = ConsoleWrapper.CursorLeft;
                    int OldTop = ConsoleWrapper.CursorTop;
                    var Paragraphs = msg.SplitNewLines();
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
                        for (int i = 0; i < MessageParagraph.Length; i++)
                        {
                            // Sleep for a few milliseconds
                            Thread.Sleep((int)Math.Round(MsEachLetter));
                            if (ConsoleWrapper.CursorLeft == ConsoleWrapper.WindowWidth - RightMargin ||
                                MessageParagraph[i] == '\n')
                            {
                                buffered.Append($"{CharManager.GetEsc()}[1B");
                                buffered.Append($"{CharManager.GetEsc()}[{Left + 1}G");
                            }

                            // Write a character individually
                            if (MessageParagraph[i] != '\n')
                                buffered.Append(BufferChar(MessageParagraph, ref i, ref vtSeqIdx));

                            // If we're writing a new line, write it
                            if (Line)
                                ConsoleWrapper.WriteLine(buffered.ToString());
                            else
                                ConsoleWrapper.Write(buffered.ToString());
                        }

                        // We're starting with the new paragraph, so we increase the CursorTop value by 1.
                        if (!(MessageParagraphIndex == Paragraphs.Length - 1))
                        {
                            buffered.Append($"{CharManager.GetEsc()}[1B");
                            buffered.Append($"{CharManager.GetEsc()}[{Left + 1}G");
                        }
                    }
                    if (Return)
                        ConsoleWrapper.SetCursorPosition(OldLeft, OldTop);
                }
                catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
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
                    if (!(vars.Length == 0))
                        Text = TextTools.FormatString(Text, vars);
                    Text = Text.Replace(Convert.ToChar(13), default);

                    // Grab each VT sequence from the paragraph and fetch their indexes
                    var sequences = VtSequenceTools.MatchVTSequences(Text);
                    int vtSeqIdx = 0;
                    var buffered = new StringBuilder();
                    for (int i = 0; i < Text.Length; i++)
                    {
                        char TextChar = Text[i];

                        // Write a character individually
                        if (TextChar == '\n')
                        {
                            buffered.AppendLine();
                            LinesMade++;
                        }
                        else
                            buffered.Append(BufferChar(Text, ref i, ref vtSeqIdx));
                        if (LinesMade == ConsoleWrapper.WindowHeight - 1)
                        {
                            ConsoleWrapper.Write(buffered.ToString());
                            buffered.Clear();
                            if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                break;
                            LinesMade = 0;
                        }
                    }
                    ConsoleWrapper.Write(buffered.ToString());
                    buffered.Clear();
                    if (Line)
                        ConsoleWrapper.WriteLine();
                }
                catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        internal static string BufferChar(string text, ref int i, ref int vtSeqIdx)
        {
            // Grab each VT sequence from the message
            char ch = text[i];
            var sequencesCollections = VtSequenceTools.MatchVTSequences(text);

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
            string seq = "";
            foreach (var sequences in sequencesCollections)
            {
                if (sequences.Count > 0 && sequences[vtSeqIdx].Index == i)
                {
                    // We're at an index which is the same as the captured VT sequence. Get the sequence
                    seq = sequences[vtSeqIdx].Value;

                    // Raise the index in case we have the next sequence, but only if we're sure that we have another
                    if (vtSeqIdx + 1 < sequences.Count)
                        vtSeqIdx++;

                    // Raise the paragraph index by the length of the sequence
                    i += seq.Length - 1;
                }
            }
            return !string.IsNullOrEmpty(seq) ? seq : ch.ToString();
        }

    }
}
