
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

using ColorSeq;
using KS.Drivers;
using KS.Kernel.Exceptions;
using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace KS.ConsoleBase
{
    /// <summary>
    /// Wrapper for the <see cref="Console"/> class to ensure safety against dumb consoles. This connects to the current console driver of the <see cref="DriverTypes.Console"/> type handled by <see cref="DriverHandler"/>.
    /// </summary>
    public static class ConsoleWrapper
    {

        /// <summary>
        /// The standard output stream that the console uses
        /// </summary>
        public static TextWriter Out
        {
            get
            {
                if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) ||
                    !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                    return DriverHandler.CurrentConsoleDriver.Out;
                else
                    throw new KernelException(KernelExceptionType.DriverPromiseBroken);
            }
        }

        /// <summary>
        /// The cursor left position
        /// </summary>
        public static int CursorLeft
        {
            get
            {
                if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) ||
                    !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                    return DriverHandler.CurrentConsoleDriver.CursorLeft;
                else
                    throw new KernelException(KernelExceptionType.DriverPromiseBroken);
            }

            set
            {
                if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) ||
                    !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                    DriverHandler.CurrentConsoleDriver.CursorLeft = value;
                else
                    throw new KernelException(KernelExceptionType.DriverPromiseBroken);
            }
        }

        /// <summary>
        /// The cursor top position
        /// </summary>
        public static int CursorTop
        {
            get
            {
                if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) ||
                    !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                    return DriverHandler.CurrentConsoleDriver.CursorTop;
                else
                    throw new KernelException(KernelExceptionType.DriverPromiseBroken);
            }

            set
            {
                if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) ||
                    !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                    DriverHandler.CurrentConsoleDriver.CursorTop = value;
                else
                    throw new KernelException(KernelExceptionType.DriverPromiseBroken);
            }
        }

        /// <summary>
        /// The console window top (rows)
        /// </summary>
        public static int WindowTop
        {
            get
            {
                if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) ||
                    !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                    return DriverHandler.CurrentConsoleDriver.WindowTop;
                else
                    throw new KernelException(KernelExceptionType.DriverPromiseBroken);
            }
        }

        /// <summary>
        /// The console window width (columns)
        /// </summary>
        public static int WindowWidth
        {
            get
            {
                if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) ||
                    !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                    return DriverHandler.CurrentConsoleDriver.WindowWidth;
                else
                    throw new KernelException(KernelExceptionType.DriverPromiseBroken);
            }
        }

        /// <summary>
        /// The console window height (rows)
        /// </summary>
        public static int WindowHeight
        {
            get
            {
                if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) ||
                    !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                    return DriverHandler.CurrentConsoleDriver.WindowHeight;
                else
                    throw new KernelException(KernelExceptionType.DriverPromiseBroken);
            }
        }

        /// <summary>
        /// The console buffer width (columns)
        /// </summary>
        public static int BufferWidth
        {
            get
            {
                if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) || !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                    return DriverHandler.CurrentConsoleDriver.BufferWidth;
                else
                    throw new KernelException(KernelExceptionType.DriverPromiseBroken);
            }
        }

        /// <summary>
        /// The console buffer height (rows)
        /// </summary>
        public static int BufferHeight
        {
            get
            {
                if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) || !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                    return DriverHandler.CurrentConsoleDriver.BufferHeight;
                else
                    throw new KernelException(KernelExceptionType.DriverPromiseBroken);
            }
        }

        /// <summary>
        /// The foreground color
        /// </summary>
        public static ConsoleColor ForegroundColor
        {
            get
            {
                if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) ||
                    !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                    return DriverHandler.CurrentConsoleDriver.ForegroundColor;
                else
                    throw new KernelException(KernelExceptionType.DriverPromiseBroken);
            }

            set
            {
                if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) ||
                    !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                    DriverHandler.CurrentConsoleDriver.ForegroundColor = value;
                else
                    throw new KernelException(KernelExceptionType.DriverPromiseBroken);
            }
        }

        /// <summary>
        /// The background color
        /// </summary>
        public static ConsoleColor BackgroundColor
        {
            get
            {
                if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) ||
                    !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                    return DriverHandler.CurrentConsoleDriver.BackgroundColor;
                else
                    throw new KernelException(KernelExceptionType.DriverPromiseBroken);
            }

            set
            {
                if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) ||
                    !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                    DriverHandler.CurrentConsoleDriver.BackgroundColor = value;
                else
                    throw new KernelException(KernelExceptionType.DriverPromiseBroken);
            }
        }

        /// <summary>
        /// The cursor visibility mode
        /// </summary>
        public static bool CursorVisible
        {
            set
            {
                if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) ||
                    !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                    DriverHandler.CurrentConsoleDriver.CursorVisible = value;
                else
                    throw new KernelException(KernelExceptionType.DriverPromiseBroken);
            }
        }

        /// <summary>
        /// The output encoding
        /// </summary>
        public static Encoding OutputEncoding
        {
            get
            {
                if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) ||
                    !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                    return DriverHandler.CurrentConsoleDriver.OutputEncoding;
                else
                    throw new KernelException(KernelExceptionType.DriverPromiseBroken);
            }

            set
            {
                if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) ||
                    !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                    DriverHandler.CurrentConsoleDriver.OutputEncoding = value;
                else
                    throw new KernelException(KernelExceptionType.DriverPromiseBroken);
            }
        }

        /// <summary>
        /// The input encoding
        /// </summary>
        public static Encoding InputEncoding
        {
            get
            {
                if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) ||
                    !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                    return DriverHandler.CurrentConsoleDriver.InputEncoding;
                else
                    throw new KernelException(KernelExceptionType.DriverPromiseBroken);
            }

            set
            {
                if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) ||
                    !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                    DriverHandler.CurrentConsoleDriver.InputEncoding = value;
                else
                    throw new KernelException(KernelExceptionType.DriverPromiseBroken);
            }
        }

        /// <summary>
        /// Whether a key is pressed
        /// </summary>
        public static bool KeyAvailable
        {
            get
            {
                if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) ||
                    !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                    return DriverHandler.CurrentConsoleDriver.KeyAvailable;
                else
                    throw new KernelException(KernelExceptionType.DriverPromiseBroken);
            }
        }

        /// <summary>
        /// Clears the console screen, filling it with spaces with the selected background color.
        /// </summary>
        public static void Clear(bool loadBack = false)
        {
            if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) ||
                !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                DriverHandler.CurrentConsoleDriver.Clear(loadBack);
            else
                throw new KernelException(KernelExceptionType.DriverPromiseBroken);
        }

        /// <summary>
        /// Sets the cursor position
        /// </summary>
        /// <param name="left">The left to be set (from 0)</param>
        /// <param name="top">The top to be set (from 0)</param>
        public static void SetCursorPosition(int left, int top)
        {
            if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) ||
                !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                DriverHandler.CurrentConsoleDriver.SetCursorPosition(left, top);
            else
                throw new KernelException(KernelExceptionType.DriverPromiseBroken);
        }

        /// <summary>
        /// Resets console colors
        /// </summary>
        public static void ResetColor()
        {
            if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) ||
                !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                DriverHandler.CurrentConsoleDriver.ResetColor();
            else
                throw new KernelException(KernelExceptionType.DriverPromiseBroken);
        }

        /// <summary>
        /// Opens the standard input
        /// </summary>
        public static Stream OpenStandardInput()
        {
            if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) ||
                !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                return DriverHandler.CurrentConsoleDriver.OpenStandardInput();
            else
                throw new KernelException(KernelExceptionType.DriverPromiseBroken);
        }

        /// <summary>
        /// Opens the standard output
        /// </summary>
        public static Stream OpenStandardOutput()
        {
            if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) ||
                !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                return DriverHandler.CurrentConsoleDriver.OpenStandardOutput();
            else
                throw new KernelException(KernelExceptionType.DriverPromiseBroken);
        }

        /// <summary>
        /// Opens the standard error
        /// </summary>
        public static Stream OpenStandardError()
        {
            if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) || !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                return DriverHandler.CurrentConsoleDriver.OpenStandardError();
            else
                throw new KernelException(KernelExceptionType.DriverPromiseBroken);
        }

        /// <summary>
        /// Sets console output
        /// </summary>
        /// <param name="newOut">New output</param>
        public static void SetOut(TextWriter newOut)
        {
            if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) || !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                DriverHandler.CurrentConsoleDriver.SetOut(newOut);
            else
                throw new KernelException(KernelExceptionType.DriverPromiseBroken);
        }

        /// <summary>
        /// Beeps the console
        /// </summary>
        public static void Beep()
        {
            if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) || !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                DriverHandler.CurrentConsoleDriver.Beep();
            else
                throw new KernelException(KernelExceptionType.DriverPromiseBroken);
        }

        /// <summary>
        /// Reads a key
        /// </summary>
        /// <param name="intercept">Whether to intercept</param>
        public static ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) || !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                return DriverHandler.CurrentConsoleDriver.ReadKey(intercept);
            else
                throw new KernelException(KernelExceptionType.DriverPromiseBroken);
        }

        /// <summary>
        /// Writes a character to console
        /// </summary>
        /// <param name="value">A character</param>
        public static void Write(char value)
        {
            if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) || !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                DriverHandler.CurrentConsoleDriver.Write(value);
            else
                throw new KernelException(KernelExceptionType.DriverPromiseBroken);
        }

        /// <summary>
        /// Writes text to console
        /// </summary>
        /// <param name="text">The text to write</param>
        public static void Write(string text)
        {
            if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) || !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                DriverHandler.CurrentConsoleDriver.Write(text);
            else
                throw new KernelException(KernelExceptionType.DriverPromiseBroken);
        }

        /// <summary>
        /// Writes text to console
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        public static void Write(string text, params object[] args)
        {
            if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) || !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                DriverHandler.CurrentConsoleDriver.Write(text, args);
            else
                throw new KernelException(KernelExceptionType.DriverPromiseBroken);
        }

        /// <summary>
        /// Writes new line to console
        /// </summary>
        public static void WriteLine()
        {
            if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) || !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                DriverHandler.CurrentConsoleDriver.WriteLine();
            else
                throw new KernelException(KernelExceptionType.DriverPromiseBroken);
        }

        /// <summary>
        /// Writes text to console with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        public static void WriteLine(string text)
        {
            if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) || !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                DriverHandler.CurrentConsoleDriver.WriteLine(text);
            else
                throw new KernelException(KernelExceptionType.DriverPromiseBroken);
        }

        /// <summary>
        /// Writes text to console with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        public static void WriteLine(string text, params object[] args)
        {
            if ((DriverHandler.CurrentConsoleDriver.DriverPromiseRequired && DriverHandler.CurrentConsoleDriver.DriverPromiseAction.Invoke(new object[] { })) || !DriverHandler.CurrentConsoleDriver.DriverPromiseRequired)
                DriverHandler.CurrentConsoleDriver.WriteLine(text, args);
            else
                throw new KernelException(KernelExceptionType.DriverPromiseBroken);
        }
    }
}
