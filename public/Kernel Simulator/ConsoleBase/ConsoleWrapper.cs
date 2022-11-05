
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
using KS.ConsoleBase.Colors;
using KS.Kernel;
using System;
using System.IO;
using System.Text;

namespace KS.ConsoleBase
{
    /// <summary>
    /// Wrapper for the <see cref="Console"/> class to ensure safety against dumb consoles.
    /// </summary>
    public static class ConsoleWrapper
    {
        private static bool _dumbSet = false;
        private static bool _dumb = true;

        /// <summary>
        /// Is the console a dumb console?
        /// </summary>
        public static bool IsDumb { 
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
                        int _ = Console.CursorLeft;

                        // If it doesn't get here without throwing exceptions, assume console is dumb. Now, check to see if terminal type is dumb
                        if (TerminalType != "dumb")
                            _dumb = false;
                    }
                }
                catch { }
                return _dumb;
            }
        }

        /// <summary>
        /// The standard output stream that the console uses
        /// </summary>
        public static TextWriter Out => Console.Out;

        /// <summary>
        /// The cursor left position
        /// </summary>
        public static int CursorLeft { 
            get 
            {
                if (IsDumb)
                    return 0;
                return Console.CursorLeft;
            }
            set
            {
                if (!IsDumb)
                    Console.CursorLeft = value;
            }
        }

        /// <summary>
        /// The cursor top position
        /// </summary>
        public static int CursorTop { 
            get 
            {
                if (IsDumb)
                    return 0;
                return Console.CursorTop;
            }
            set
            {
                if (!IsDumb)
                    Console.CursorTop = value;
            }
        }

        /// <summary>
        /// The console window top (rows)
        /// </summary>
        public static int WindowTop
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return Console.WindowTop;
            }
        }

        /// <summary>
        /// The console window width (columns)
        /// </summary>
        public static int WindowWidth
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return Console.WindowWidth;
            }
        }

        /// <summary>
        /// The console window height (rows)
        /// </summary>
        public static int WindowHeight
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return Console.WindowHeight;
            }
        }

        /// <summary>
        /// The console buffer width (columns)
        /// </summary>
        public static int BufferWidth
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return Console.BufferWidth;
            }
        }

        /// <summary>
        /// The console buffer height (rows)
        /// </summary>
        public static int BufferHeight
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return Console.BufferHeight;
            }
        }

        /// <summary>
        /// The foreground color
        /// </summary>
        public static ConsoleColor ForegroundColor
        {
            get
            {
                if (IsDumb)
                    return ConsoleColor.White;
                return Console.ForegroundColor;
            }
            set
            {
                if (!IsDumb)
                    Console.ForegroundColor = value;
                ColorTools.cachedForegroundColor = new Color(Convert.ToInt32(value)).VTSequenceForeground;
            }
        }

        /// <summary>
        /// The background color
        /// </summary>
        public static ConsoleColor BackgroundColor
        {
            get
            {
                if (IsDumb)
                    return ConsoleColor.Black;
                return Console.BackgroundColor;
            }
            set
            {
                if (!IsDumb)
                    Console.BackgroundColor = value;
                ColorTools.cachedBackgroundColor = new Color(Convert.ToInt32(value)).VTSequenceBackground;
            }
        }

        /// <summary>
        /// The cursor visibility mode
        /// </summary>
        public static bool CursorVisible { 
            set
            {
                if (!IsDumb)
                    Console.CursorVisible = value;
            }
        }

        /// <summary>
        /// The output encoding
        /// </summary>
        public static Encoding OutputEncoding { 
            get 
            {
                if (IsDumb)
                    return Encoding.Default;
                return Console.OutputEncoding;
            }
            set
            {
                if (!IsDumb)
                    Console.OutputEncoding = value;
            }
        }

        /// <summary>
        /// The input encoding
        /// </summary>
        public static Encoding InputEncoding { 
            get 
            {
                if (IsDumb)
                    return Encoding.Default;
                return Console.InputEncoding;
            }
            set
            {
                if (!IsDumb)
                    Console.InputEncoding = value;
            }
        }

        /// <summary>
        /// Whether a key is pressed
        /// </summary>
        public static bool KeyAvailable { 
            get 
            {
                if (IsDumb)
                    return false;
                return Console.KeyAvailable;
            }
        }

        /// <summary>
        /// Clears the console screen, filling it with spaces with the selected background color.
        /// </summary>
        public static void Clear(bool loadBack = false)
        {
            if (!IsDumb)
            {
                if (loadBack)
                    ColorTools.LoadBack();
                Console.Clear();
            }
        }

        /// <summary>
        /// Sets the cursor position
        /// </summary>
        /// <param name="left">The left to be set (from 0)</param>
        /// <param name="top">The top to be set (from 0)</param>
        public static void SetCursorPosition(int left, int top)
        {
            if (!IsDumb)
                Console.SetCursorPosition(left, top);
        }

        /// <summary>
        /// Resets console colors
        /// </summary>
        public static void ResetColor()
        {
            if (!IsDumb)
                Console.ResetColor();
        }

        /// <summary>
        /// Opens the standard input
        /// </summary>
        public static Stream OpenStandardInput() => Console.OpenStandardInput();

        /// <summary>
        /// Opens the standard output
        /// </summary>
        public static Stream OpenStandardOutput() => Console.OpenStandardOutput();

        /// <summary>
        /// Opens the standard error
        /// </summary>
        public static Stream OpenStandardError() => Console.OpenStandardError();

        /// <summary>
        /// Sets console output
        /// </summary>
        /// <param name="newOut">New output</param>
        public static void SetOut(TextWriter newOut)
        {
            // We need to reset dumb state because the new output may not support usual console features other then reading/writing.
            _dumbSet = false;
            _dumb = true;
            Console.SetOut(newOut);
        }

        /// <summary>
        /// Beeps the console
        /// </summary>
        public static void Beep() => Console.Beep();

        /// <summary>
        /// Reads a key
        /// </summary>
        /// <param name="intercept">Whether to intercept</param>
        public static ConsoleKeyInfo ReadKey(bool intercept = false) => Console.ReadKey(intercept);

        /// <summary>
        /// Writes a character to console
        /// </summary>
        /// <param name="value">A character</param>
        public static void Write(char value) => Console.Write(value);

        /// <summary>
        /// Writes text to console
        /// </summary>
        /// <param name="text">The text to write</param>
        public static void Write(string text) => Console.Write(text);

        /// <summary>
        /// Writes text to console
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        public static void Write(string text, params object[] args) => Console.Write(text, args);

        /// <summary>
        /// Writes new line to console
        /// </summary>
        public static void WriteLine() => Console.WriteLine();

        /// <summary>
        /// Writes text to console with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        public static void WriteLine(string text) => Console.WriteLine(text);

        /// <summary>
        /// Writes text to console with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        public static void WriteLine(string text, params object[] args) => Console.WriteLine(text, args);
    }
}
