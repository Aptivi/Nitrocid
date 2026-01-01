//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using System.IO;
using System.Linq;
using System.Threading;
using System.Text;
using TextEncoding = System.Text.Encoding;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Textify.General;

namespace Nitrocid.Drivers.Console.Bases
{
    internal class Buffered : BaseConsoleDriver, IConsoleDriver
    {

        internal StringBuilder consoleBuffer = new();

        public override string DriverName => "Buffered";

        public override DriverTypes DriverType => DriverTypes.Console;

        public override bool DriverInternal => true;

        public override bool IsDumb => true;

        public override int CursorLeft { get => 0; set => throw new KernelException(KernelExceptionType.NotImplementedYet); }

        public override int CursorTop { get => 0; set => throw new KernelException(KernelExceptionType.NotImplementedYet); }

        public override int WindowWidth => 0;

        public override int WindowHeight => 0;

        public override int BufferWidth => 0;

        public override int BufferHeight => 0;

        public override bool CursorVisible { set => throw new KernelException(KernelExceptionType.NotImplementedYet); }

        public override TextEncoding OutputEncoding { get => throw new KernelException(KernelExceptionType.NotImplementedYet); set => throw new KernelException(KernelExceptionType.NotImplementedYet); }

        public override TextEncoding InputEncoding { get => throw new KernelException(KernelExceptionType.NotImplementedYet); set => throw new KernelException(KernelExceptionType.NotImplementedYet); }

        public override bool KeyAvailable =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override void Beep() =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override void Clear(bool loadBack = false) =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override Stream OpenStandardError() =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override Stream OpenStandardInput() =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override Stream OpenStandardOutput() =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override bool TreatCtrlCAsInput =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override ConsoleKeyInfo ReadKey(bool intercept = false) =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override void ResetColor() =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override void SetCursorPosition(int left, int top) =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override void SetCursorLeft(int left) =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override void SetCursorTop(int top) =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override void SetWindowDimensions(int width, int height) =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override void SetBufferDimensions(int width, int height) =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override void SetWindowWidth(int width) =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override void SetWindowHeight(int height) =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override void SetBufferWidth(int width) =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override void SetBufferHeight(int height) =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public override void SetOut(TextWriter newOut) =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        /// <summary>
        /// Outputs text to the buffered string builder
        /// </summary>
        /// <inheritdoc/>
        public override void Write(char value) =>
            WritePlain($"{value}", false, []);

        /// <summary>
        /// Outputs text to the buffered string builder
        /// </summary>
        /// <inheritdoc/>
        public override void Write(string text) =>
            WritePlain(text, false, []);

        /// <summary>
        /// Outputs text to the buffered string builder
        /// </summary>
        /// <inheritdoc/>
        public override void Write(string text, params object[] args) =>
            WritePlain(text, false, args);

        /// <summary>
        /// Outputs text to the buffered string builder
        /// </summary>
        /// <inheritdoc/>
        public override void WriteLine() =>
            WritePlain("", true, []);

        /// <summary>
        /// Outputs text to the buffered string builder
        /// </summary>
        /// <inheritdoc/>
        public override void WriteLine(string text) =>
            WritePlain(text, true, []);

        /// <summary>
        /// Outputs text to the buffered string builder
        /// </summary>
        /// <inheritdoc/>
        public override void WriteLine(string text, params object[] args) =>
            WritePlain(text, true, args);

        /// <summary>
        /// Outputs text to the buffered string builder
        /// </summary>
        /// <inheritdoc/>
        public override void WritePlain(string Text, bool Line, params object[] vars)
        {
            lock (WriteLock)
            {
                // Open the stream
                try
                {
                    string formatted = TextTools.FormatString(Text, vars);
                    if (Line)
                        consoleBuffer.AppendLine(formatted);
                    else
                        consoleBuffer.Append(formatted);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, "There is a serious error when printing text. {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Outputs new line to the buffered string builder
        /// </summary>
        /// <inheritdoc/>
        public override void WritePlain()
        {
            lock (WriteLock)
            {
                try
                {
                    consoleBuffer.AppendLine();
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, "There is a serious error when printing text. {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Outputs text slowly to the buffered string builder
        /// </summary>
        /// <inheritdoc/>
        public override void WriteSlowlyPlain(string msg, bool Line, double MsEachLetter, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Format string as needed
                    if (vars.Length > 0)
                        msg = TextTools.FormatString(msg, vars);

                    // Write text slowly
                    var chars = msg.ToCharArray().ToList();
                    foreach (char ch in chars)
                    {
                        Thread.Sleep((int)Math.Round(MsEachLetter));
                        consoleBuffer.Append(ch);
                    }
                    if (Line)
                    {
                        consoleBuffer.AppendLine();
                    }
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, "There is a serious error when printing text. {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Just writes text to the buffered string builder without line terminator, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public override void WriteWherePlain(string msg, int Left, int Top, params object[] vars) =>
            WriteWherePlain(msg, Left, Top, false, vars);

        /// <summary>
        /// Just writes text to the buffered string builder without line terminator, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public override void WriteWherePlain(string msg, int Left, int Top, bool Return, params object[] vars) =>
            WriteWherePlain(msg, Left, Top, Return, 0, vars);

        /// <summary>
        /// Just writes text to the buffered string builder without line terminator, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public override void WriteWherePlain(string msg, int Left, int Top, bool Return, int RightMargin, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // We can't do positioning, so change writing mode to WritePlain
                    WritePlain(msg, false, vars);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, "There is a serious error when printing text. {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Just writes text slowly to the buffered string builder, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public override void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, params object[] vars) =>
            WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, false, 0, vars);

        /// <summary>
        /// Just writes text slowly to the buffered string builder, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public override void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, params object[] vars) =>
            WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, Return, 0, vars);

        /// <summary>
        /// Just writes text slowly to the buffered string builder, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public override void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, int RightMargin, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // We can't do positioning, so change writing mode to WritePlain
                    WriteSlowlyPlain(msg, false, MsEachLetter, vars);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, "There is a serious error when printing text. {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Just writes text to the buffered string builder, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public override void WriteWrappedPlain(string Text, bool Line, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // We can't do positioning, so change writing mode to WritePlain
                    WritePlain(Text, Line, vars);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, "There is a serious error when printing text. {0}", ex.Message);
                }
            }
        }
    }
}
