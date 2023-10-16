
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

using System;
using System.Runtime.InteropServices;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel;
using KS.Misc.Text;
using Terminaux.Sequences.Builder;
using Terminaux.Sequences.Tools;

namespace KS.ConsoleBase
{
    /// <summary>
    /// Additional routines for the console
    /// </summary>
    public static class ConsoleExtensions
    {

        private static readonly int OldBufferHeight = ConsoleWrapper.BufferHeight;
        private static readonly int OldBufferWidth = ConsoleWrapper.BufferWidth;

        /// <summary>
        /// Clears the console buffer, but keeps the current cursor position
        /// </summary>
        public static void ClearKeepPosition()
        {
            int Left = ConsoleWrapper.CursorLeft;
            int Top = ConsoleWrapper.CursorTop;
            ConsoleWrapper.Clear();
            ConsoleWrapper.SetCursorPosition(Left, Top);
        }

        /// <summary>
        /// Clears the line to the right
        /// </summary>
        public static string GetClearLineToRightSequence() =>
            VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiEraseInLine, 0);

        /// <summary>
        /// Clears the line to the right
        /// </summary>
        public static void ClearLineToRight() =>
            ConsoleWrapper.Write(GetClearLineToRightSequence());

        /// <summary>
        /// Gets how many times to repeat the character to represent the appropriate percentage level for the specified number.
        /// </summary>
        /// <param name="CurrentNumber">The current number that is less than or equal to the maximum number.</param>
        /// <param name="MaximumNumber">The maximum number.</param>
        /// <param name="WidthOffset">The console window width offset. It's usually a multiple of 2.</param>
        /// <returns>How many times to repeat the character</returns>
        public static int PercentRepeat(int CurrentNumber, int MaximumNumber, int WidthOffset) =>
            (int)Math.Round(CurrentNumber * 100 / (double)MaximumNumber * ((ConsoleWrapper.WindowWidth - WidthOffset) * 0.01d));

        /// <summary>
        /// Gets how many times to repeat the character to represent the appropriate percentage level for the specified number.
        /// </summary>
        /// <param name="CurrentNumber">The current number that is less than or equal to the maximum number.</param>
        /// <param name="MaximumNumber">The maximum number.</param>
        /// <param name="TargetWidth">The target width</param>
        /// <returns>How many times to repeat the character</returns>
        public static int PercentRepeatTargeted(int CurrentNumber, int MaximumNumber, int TargetWidth) =>
            (int)Math.Round(CurrentNumber * 100 / (double)MaximumNumber * (TargetWidth * 0.01d));

        /// <summary>
        /// Filters the VT sequences that matches the regex
        /// </summary>
        /// <param name="Text">The text that contains the VT sequences</param>
        /// <returns>The text that doesn't contain the VT sequences</returns>
        public static string FilterVTSequences(string Text)
        {
            // Filter all sequences
            Text = VtSequenceTools.FilterVTSequences(Text);
            return Text;
        }

        /// <summary>
        /// Get the filtered cursor positions (by filtered means filtered from the VT escape sequences that matches the regex in the routine)
        /// </summary>
        /// <param name="Text">The text that contains the VT sequences</param>
        /// <param name="line">Whether to simulate the new line at the end of text or not</param>
        /// <param name="Vars">Variables to be formatted in the text</param>
        public static (int, int) GetFilteredPositions(string Text, bool line, params object[] Vars)
        {
            int LeftSeekPosition = ConsoleWrapper.CursorLeft;
            int TopSeekPosition = ConsoleWrapper.CursorTop;

            // If the string is null before or after processing the text, don't seek.
            bool noSeek = false;
            if (string.IsNullOrEmpty(Text))
                noSeek = true;

            // Filter all text from the VT escape sequences
            Text = FilterVTSequences(Text);

            // Seek through filtered text (make it seem like it came from Linux by removing CR (\r)), return to the old position, and return the filtered positions
            Text = TextTools.FormatString(Text, Vars);
            Text = Text.Replace(Convert.ToString(Convert.ToChar(13)), "");
            Text = Text.Replace(Convert.ToString(Convert.ToChar(0)), "");
            if (string.IsNullOrEmpty(Text))
                noSeek = true;

            // Really seek if we need to
            if (!noSeek)
            {
                var texts = TextTools.GetWrappedSentences(Text, ConsoleWrapper.WindowWidth, ConsoleWrapper.CursorLeft);
                for (int i = 0; i < texts.Length; i++)
                {
                    string text = texts[i];
                    for (int j = 1; j <= text.Length; j++)
                    {
                        // If we spotted a new line character, get down by one line.
                        if (text[j - 1] == Convert.ToChar(10))
                        {
                            if (TopSeekPosition < ConsoleWrapper.BufferHeight - 1)
                                TopSeekPosition += 1;
                            LeftSeekPosition = 0;
                        }
                        else
                        {
                            // Simulate seeking through text
                            LeftSeekPosition += 1;
                            if (LeftSeekPosition >= ConsoleWrapper.WindowWidth)
                            {
                                // We've reached end of line
                                LeftSeekPosition = 0;
                            }
                        }
                    }

                    // Get down by one line
                    if (i < texts.Length - 1)
                        TopSeekPosition += 1;
                    if (TopSeekPosition > ConsoleWrapper.BufferHeight - 1)
                    {
                        // We're at the end of buffer! Decrement by one and bail.
                        TopSeekPosition -= 1;
                        LeftSeekPosition = texts[^1].Length;
                        if (LeftSeekPosition >= ConsoleWrapper.WindowWidth)
                            LeftSeekPosition = ConsoleWrapper.WindowWidth - 1;
                        break;
                    }
                }
            }

            // If new line is to be appended at the end of text, just simulate going down.
            if (line)
            {
                // Do the same as if we've inserted a new line in the middle of the text, but make
                // sure that the left seek position is not zero for text that fill the whole line.
                //
                // There are legitimate writers, like SeparatorColor, that attempt to fill the whole
                // line with the separator character. For this very reason, consoles tend to wrap the
                // whole line to the new row with the left position set to zero. For writers that use
                // the Line argument, if the left seek position is above zero after the write, the
                // top will increase by one and the buffer check is done.
                //
                // However, filling the line, as seen by the above logic, requires us to set the left
                // seek position to zero, causing the top seek position to go down one row.
                TopSeekPosition += 1;
                if (TopSeekPosition > ConsoleWrapper.BufferHeight - 1)
                    TopSeekPosition -= 1;
                LeftSeekPosition = 0;
            }

            // Return the filtered positions
            return (LeftSeekPosition, TopSeekPosition);
        }

        /// <summary>
        /// Sets the console title
        /// </summary>
        /// <param name="Text">The text to be set</param>
        public static void SetTitle(string Text)
        {
            char BellChar = Convert.ToChar(7);
            char EscapeChar = Convert.ToChar(27);
            string Sequence = $"{EscapeChar}]0;{Text}{BellChar}";
            TextWriterColor.WritePlain(Sequence, false);
        }

        /// <summary>
        /// Resets the entire console
        /// </summary>
        public static void ResetAll()
        {
            ConsoleWrapper.Write(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.EscFullReset));
            ConsoleWrapper.Write(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiSoftTerminalReset));
        }

        /// <summary>
        /// Resets the console colors without clearing screen
        /// </summary>
        public static void ResetColors()
        {
            ConsoleWrapper.Write($"{CharManager.GetEsc()}[39m");
            ConsoleWrapper.Write($"{CharManager.GetEsc()}[49m");
        }

        #region Windows-specific
        #pragma warning disable CA1416
        private const string winKernel = "kernel32.dll";

        [DllImport(winKernel, SetLastError = true)]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);

        [DllImport(winKernel, SetLastError = true)]
        private static extern bool GetConsoleMode(IntPtr handle, out int mode);

        [DllImport(winKernel, SetLastError = true)]
        private static extern IntPtr GetStdHandle(int handle);

        internal static bool InitializeSequences()
        {
            IntPtr stdHandle = GetStdHandle(-11);
            int mode = CheckForConHostSequenceSupport();
            if (mode != 7)
                return SetConsoleMode(stdHandle, mode | 4);
            return true;
        }

        internal static int CheckForConHostSequenceSupport()
        {
            IntPtr stdHandle = GetStdHandle(-11);
            GetConsoleMode(stdHandle, out int mode);
            return mode;
        }

        internal static void SetBufferSize()
        {
            if (KernelPlatform.IsOnWindows())
            {
                Console.BufferWidth = ConsoleWrapper.WindowWidth;
                Console.BufferHeight = ConsoleWrapper.WindowHeight;
            }
        }

        internal static void RestoreBufferSize()
        {
            if (KernelPlatform.IsOnWindows())
            {
                Console.BufferWidth = OldBufferWidth;
                Console.BufferHeight = OldBufferHeight;
            }
        }
        #pragma warning restore CA1416
        #endregion

    }
}
