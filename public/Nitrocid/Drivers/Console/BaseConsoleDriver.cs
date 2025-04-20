
//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

extern alias TextifyDep;

using System;
using System.IO;
using System.Threading;
using SystemConsole = System.Console;
using TextEncoding = System.Text.Encoding;
using Terminaux.Sequences.Builder.Types;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors;
using System.Runtime.Serialization;
using System.Diagnostics.CodeAnalysis;
using Nitrocid.ConsoleBase;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using System.Text;
using TextifyDep::Textify.General;
using Terminaux.Sequences;

namespace Nitrocid.Drivers.Console
{
    /// <summary>
    /// Base console driver
    /// </summary>
    [DataContract]
    public abstract class BaseConsoleDriver : IConsoleDriver
    {
        internal static readonly object WriteLock = new();

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
        [IgnoreDataMember]
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
        [IgnoreDataMember]
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
        public virtual bool TreatCtrlCAsInput
        {
            get => SystemConsole.TreatControlCAsInput;
            set => SystemConsole.TreatControlCAsInput = value;
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
                    SystemConsole.Write(ColorTools.CurrentBackgroundColor.VTSequenceBackground);
                SetCursorPosition(0, 0);
                SystemConsole.Write(CsiSequences.GenerateCsiEraseInDisplay(0));
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
        public virtual void SetWindowDimensions(int width, int height)
        {
            if (!IsDumb)
            {
                if (KernelPlatform.IsOnWindows())
                {
                    SystemConsole.WindowWidth = width;
                    SystemConsole.WindowHeight = height;
                }
                else
                {
                    TextWriterRaw.WriteRaw($"\u001b[8;{height};{width}t");
                    Thread.Sleep(35);
                }
            }
            _moved = true;
        }

        /// <inheritdoc/>
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "KernelPlatform.IsOnWindows()")]
        public virtual void SetBufferDimensions(int width, int height)
        {
            if (!IsDumb)
            {
                if (KernelPlatform.IsOnWindows())
                {
                    SystemConsole.BufferWidth = width;
                    SystemConsole.BufferHeight = height;
                }
                else
                {
                    TextWriterRaw.WriteRaw($"\u001b[8;{height};{width}t");
                    Thread.Sleep(35);
                }
            }
            _moved = true;
        }

        /// <inheritdoc/>
        public virtual void SetWindowWidth(int width)
        {
            if (!IsDumb)
            {
                if (KernelPlatform.IsOnWindows())
                    SystemConsole.WindowWidth = width;
                else
                {
                    TextWriterRaw.WriteRaw($"\u001b[8;{SystemConsole.WindowHeight};{width}t");
                    Thread.Sleep(35);
                }
            }
        }

        /// <inheritdoc/>
        public virtual void SetWindowHeight(int height)
        {
            if (!IsDumb)
            {
                if (KernelPlatform.IsOnWindows())
                    SystemConsole.WindowHeight = height;
                else
                {
                    TextWriterRaw.WriteRaw($"\u001b[8;{height};{SystemConsole.WindowWidth}t");
                    Thread.Sleep(35);
                }
            }
        }

        /// <inheritdoc/>
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "KernelPlatform.IsOnWindows()")]
        public virtual void SetBufferWidth(int width)
        {
            if (!IsDumb)
            {
                if (KernelPlatform.IsOnWindows())
                    SystemConsole.BufferWidth = width;
                else
                {
                    TextWriterRaw.WriteRaw($"\u001b[8;{SystemConsole.WindowHeight};{width}t");
                    Thread.Sleep(35);
                }
            }
        }

        /// <inheritdoc/>
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "KernelPlatform.IsOnWindows()")]
        public virtual void SetBufferHeight(int height)
        {
            if (!IsDumb)
            {
                if (KernelPlatform.IsOnWindows())
                    SystemConsole.BufferHeight = height;
                else
                {
                    TextWriterRaw.WriteRaw($"\u001b[8;{height};{SystemConsole.WindowWidth}t");
                    Thread.Sleep(35);
                }
            }
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
            lock (WriteLock)
            {
                SystemConsole.Write(value);
                _moved = true;
            }
        }

        /// <inheritdoc/>
        public virtual void Write(string text)
        {
            lock (WriteLock)
            {
                SystemConsole.Write(text);
                _moved = true;
            }
        }

        /// <inheritdoc/>
        public virtual void Write(string text, params object[] args)
        {
            lock (WriteLock)
            {
                SystemConsole.Write(text, args);
                _moved = true;
            }
        }

        /// <inheritdoc/>
        public virtual void WriteLine()
        {
            lock (WriteLock)
            {
                SystemConsole.WriteLine();
                _moved = true;
            }
        }

        /// <inheritdoc/>
        public virtual void WriteLine(string text)
        {
            lock (WriteLock)
            {
                SystemConsole.WriteLine(text);
                _moved = true;
            }
        }

        /// <inheritdoc/>
        public virtual void WriteLine(string text, params object[] args)
        {
            lock (WriteLock)
            {
                SystemConsole.WriteLine(text, args);
                _moved = true;
            }
        }

        /// <inheritdoc/>
        public virtual void WritePlain(string Text, bool Line, params object[] vars)
        {
            lock (WriteLock)
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
                    DebugWriter.WriteDebug(DebugLevel.E, "There is a serious error when printing text. {0}", ex.Message);
                }
            }
        }

        /// <inheritdoc/>
        public virtual void WritePlain() =>
            TextWriterRaw.Write();

        /// <inheritdoc/>
        public virtual void WriteSlowlyPlain(string msg, bool Line, double MsEachLetter, params object[] vars) =>
            TextWriterSlowColor.WriteSlowlyPlain(msg, Line, MsEachLetter, vars);

        /// <inheritdoc/>
        public virtual void WriteWherePlain(string msg, int Left, int Top, params object[] vars) =>
            TextWriterWhereColor.WriteWherePlain(msg, Left, Top, false, 0, vars);

        /// <inheritdoc/>
        public virtual void WriteWherePlain(string msg, int Left, int Top, bool Return, params object[] vars) =>
            TextWriterWhereColor.WriteWherePlain(msg, Left, Top, Return, 0, vars);

        /// <inheritdoc/>
        public virtual void WriteWherePlain(string msg, int Left, int Top, bool Return, int RightMargin, params object[] vars) =>
            TextWriterWhereColor.WriteWherePlain(msg, Left, Top, Return, RightMargin, vars);

        /// <inheritdoc/>
        public virtual string RenderWherePlain(string msg, int Left, int Top, params object[] vars) =>
            TextWriterWhereColor.RenderWhere(msg, Left, Top, false, 0, vars);

        /// <inheritdoc/>
        public virtual string RenderWherePlain(string msg, int Left, int Top, bool Return, params object[] vars) =>
            TextWriterWhereColor.RenderWhere(msg, Left, Top, Return, 0, vars);

        /// <inheritdoc/>
        public virtual string RenderWherePlain(string msg, int Left, int Top, bool Return, int RightMargin, params object[] vars) =>
            TextWriterWhereColor.RenderWhere(msg, Left, Top, Return, RightMargin, vars);

        /// <inheritdoc/>
        public virtual void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, params object[] vars) =>
            TextWriterWhereSlowColor.WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, false, vars);

        /// <inheritdoc/>
        public virtual void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, params object[] vars) =>
            TextWriterWhereSlowColor.WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, Return, 0, vars);

        /// <inheritdoc/>
        public virtual void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, int RightMargin, params object[] vars) =>
            TextWriterWhereSlowColor.WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, Return, RightMargin, vars);

        /// <inheritdoc/>
        public virtual void WriteWrappedPlain(string Text, bool Line, params object[] vars)
        {
            var LinesMade = 0;
            try
            {
                // Format string as needed
                if (vars.Length > 0)
                    Text = TextTools.FormatString(Text, vars);
                Text = Text.Replace(Convert.ToChar(13), default);

                // First, split the text to wrap
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth);

                // Iterate through sentences
                var buffered = new StringBuilder();
                for (int idx = 0; idx < sentences.Length; idx++)
                {
                    string sentence = sentences[idx];

                    // Grab each VT sequence from the paragraph and fetch their indexes
                    var sequences = VtSequenceTools.MatchVTSequences(sentence);
                    int vtSeqIdx = 0;
                    bool bail = false;
                    for (int i = 0; i < sentence.Length; i++)
                    {
                        char TextChar = sentence[i];

                        // Write a character individually
                        if (LinesMade == ConsoleWrapper.WindowHeight - 1)
                        {
                            TextWriterRaw.WriteRaw(buffered.ToString());
                            buffered.Clear();
                            var key = ReadKey().Key;
                            switch (key)
                            {
                                case ConsoleKey.Escape:
                                    return;
                                case ConsoleKey.PageUp:
                                    bail = true;
                                    idx -= ConsoleWrapper.WindowHeight * 2 - 1;
                                    if (idx < 0)
                                        idx = -1;
                                    break;
                                case ConsoleKey.PageDown:
                                    bail = true;
                                    if (idx > sentences.Length - 1 - ConsoleWrapper.WindowHeight)
                                        idx = sentences.Length - 1 - ConsoleWrapper.WindowHeight;
                                    else
                                        idx--;
                                    break;
                                case ConsoleKey.UpArrow:
                                    bail = true;
                                    idx -= ConsoleWrapper.WindowHeight + 1;
                                    if (idx < 0)
                                        idx = -1;
                                    break;
                                case ConsoleKey.DownArrow:
                                    bail = true;
                                    if (idx >= sentences.Length - 1)
                                    {
                                        idx = sentences.Length - 1 - ConsoleWrapper.WindowHeight;
                                        break;
                                    }
                                    idx -= ConsoleWrapper.WindowHeight - 1;
                                    if (idx < 0)
                                        idx = -1;
                                    break;
                                case ConsoleKey.Home:
                                    bail = true;
                                    idx = -1;
                                    break;
                                case ConsoleKey.End:
                                    bail = true;
                                    idx = sentences.Length - 1 - ConsoleWrapper.WindowHeight;
                                    if (idx < 0)
                                        idx = -1;
                                    break;
                            }
                            LinesMade = 0;
                        }
                        if (bail)
                        {
                            ColorTools.LoadBack(KernelColorTools.GetColor(KernelColorType.Background));
                            break;
                        }
                        buffered.Append(ConsoleTools.BufferChar(sentence, sequences, ref i, ref vtSeqIdx, out _));
                    }
                    if (!bail && idx < sentences.Length - 1)
                    {
                        buffered.AppendLine();
                        LinesMade++;
                    }
                }
                TextWriterRaw.WriteRaw(buffered.ToString());
                buffered.Clear();
                if (Line)
                    TextWriterRaw.Write();
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"There is a serious error when printing wrapped text. {ex.Message}");
                DebugWriter.WriteDebug(DebugLevel.T, ex.StackTrace ?? "No stack trace");
            }
        }

    }
}
