// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Text;

namespace KS.Drivers.Console.Consoles
{
    internal class Null : IConsoleDriver
    {

        public string DriverName => "Null";

        public DriverTypes DriverType => DriverTypes.Console;

        public TextWriter Out => null;

        public int CursorLeft { get => 0; set => throw new NotImplementedException(); }

        public int CursorTop { get => 0; set => throw new NotImplementedException(); }

        public int WindowTop => 0;

        public int WindowWidth => 0;

        public int WindowHeight => 0;

        public int BufferWidth => 0;

        public int BufferHeight => 0;

        public ConsoleColor ForegroundColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ConsoleColor BackgroundColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool CursorVisible { set => throw new NotImplementedException(); }

        public Encoding OutputEncoding { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Encoding InputEncoding { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool KeyAvailable => 
            throw new NotImplementedException();

        public void Beep() => 
            throw new NotImplementedException();

        public void Clear(bool loadBack = false) => 
            throw new NotImplementedException();

        public Stream OpenStandardError() => 
            throw new NotImplementedException();

        public Stream OpenStandardInput() => 
            throw new NotImplementedException();

        public Stream OpenStandardOutput() => 
            throw new NotImplementedException();

        public ConsoleKeyInfo ReadKey(bool intercept = false) => 
            throw new NotImplementedException();

        public void ResetColor() => 
            throw new NotImplementedException();

        public void SetCursorPosition(int left, int top) => 
            throw new NotImplementedException();

        public void SetOut(TextWriter newOut) => 
            throw new NotImplementedException();

        /// <summary>
        /// Outputs text to the void
        /// </summary>
        /// <inheritdoc/>
        public void Write(char value) { }

        /// <summary>
        /// Outputs text to the void
        /// </summary>
        /// <inheritdoc/>
        public void Write(string text) { }

        /// <summary>
        /// Outputs text to the void
        /// </summary>
        /// <inheritdoc/>
        public void Write(string text, params object[] args) { }

        /// <summary>
        /// Outputs text to the void
        /// </summary>
        /// <inheritdoc/>
        public void WriteLine() { }

        /// <summary>
        /// Outputs text to the void
        /// </summary>
        /// <inheritdoc/>
        public void WriteLine(string text) { }

        /// <summary>
        /// Outputs text to the void
        /// </summary>
        /// <inheritdoc/>
        public void WriteLine(string text, params object[] args) { }

        /// <summary>
        /// Outputs text to the void
        /// </summary>
        /// <inheritdoc/>
        public void WritePlain(string Text, bool Line, params object[] vars) { }

        /// <summary>
        /// Outputs new line to the void
        /// </summary>
        /// <inheritdoc/>
        public void WritePlain() { }

        /// <summary>
        /// Outputs text slowly to the void
        /// </summary>
        /// <inheritdoc/>
        public void WriteSlowlyPlain(string msg, bool Line, double MsEachLetter, params object[] vars) { }

        /// <summary>
        /// Just writes text to the void without line terminator, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public void WriteWherePlain(string msg, int Left, int Top, params object[] vars) { }

        /// <summary>
        /// Just writes text to the void without line terminator, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public void WriteWherePlain(string msg, int Left, int Top, bool Return, params object[] vars) { }

        /// <summary>
        /// Just writes text to the void without line terminator, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public void WriteWherePlain(string msg, int Left, int Top, bool Return, int RightMargin, params object[] vars) { }

        /// <summary>
        /// Just writes text slowly to the void, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, params object[] vars) { }

        /// <summary>
        /// Just writes text slowly to the void, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, params object[] vars) { }

        /// <summary>
        /// Just writes text to the void, since we can't do positioning.
        /// </summary>
        /// <inheritdoc/>
        public void WriteWrappedPlain(string Text, bool Line, params object[] vars) { }
    }
}
