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

using Nitrocid.Kernel.Exceptions;
using System;
using System.IO;
using TextEncoding = System.Text.Encoding;

namespace Nitrocid.Drivers.Console.Bases
{
    internal class Null : BaseConsoleDriver, IConsoleDriver
    {

        public override string DriverName => "Null";

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
        /// Outputs text to the void
        /// </summary>
        /// <inheritdoc/>
        public override void Write(char value) { }

        /// <summary>
        /// Outputs text to the void
        /// </summary>
        /// <inheritdoc/>
        public override void Write(string text) { }

        /// <summary>
        /// Outputs text to the void
        /// </summary>
        /// <inheritdoc/>
        public override void Write(string text, params object[] args) { }

        /// <summary>
        /// Outputs text to the void
        /// </summary>
        /// <inheritdoc/>
        public override void WriteLine() { }

        /// <summary>
        /// Outputs text to the void
        /// </summary>
        /// <inheritdoc/>
        public override void WriteLine(string text) { }

        /// <summary>
        /// Outputs text to the void
        /// </summary>
        /// <inheritdoc/>
        public override void WriteLine(string text, params object[] args) { }

        /// <summary>
        /// Outputs text to the void
        /// </summary>
        /// <inheritdoc/>
        public override void WritePlain(string Text, bool Line, params object[] vars) { }

        /// <summary>
        /// Outputs new line to the void
        /// </summary>
        /// <inheritdoc/>
        public override void WritePlain() { }

        /// <summary>
        /// Outputs text slowly to the void
        /// </summary>
        /// <inheritdoc/>
        public override void WriteSlowlyPlain(string msg, bool Line, double MsEachLetter, params object[] vars) { }

        /// <summary>
        /// Just writes text to the void without line terminator, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public override void WriteWherePlain(string msg, int Left, int Top, params object[] vars) { }

        /// <summary>
        /// Just writes text to the void without line terminator, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public override void WriteWherePlain(string msg, int Left, int Top, bool Return, params object[] vars) { }

        /// <summary>
        /// Just writes text to the void without line terminator, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public override void WriteWherePlain(string msg, int Left, int Top, bool Return, int RightMargin, params object[] vars) { }

        /// <summary>
        /// Just writes text slowly to the void, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public override void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, params object[] vars) { }

        /// <summary>
        /// Just writes text slowly to the void, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public override void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, params object[] vars) { }

        /// <summary>
        /// Just writes text slowly to the void, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public override void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, int RightMargin, params object[] vars) { }

        /// <summary>
        /// Just writes text to the void, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public override void WriteWrappedPlain(string Text, bool Line, params object[] vars) { }
    }
}
