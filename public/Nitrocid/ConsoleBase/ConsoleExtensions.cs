
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
using KS.Misc.Text;
using Terminaux.Sequences.Tools;

namespace KS.ConsoleBase
{
    /// <summary>
    /// Additional routines for the console
    /// </summary>
    public static class ConsoleExtensions
    {

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
        public static void ClearLineToRight() => ConsoleWrapper.Write(Convert.ToString(CharManager.GetEsc()) + "[0K");

        /// <summary>
        /// Gets how many times to repeat the character to represent the appropriate percentage level for the specified number.
        /// </summary>
        /// <param name="CurrentNumber">The current number that is less than or equal to the maximum number.</param>
        /// <param name="MaximumNumber">The maximum number.</param>
        /// <param name="WidthOffset">The console window width offset. It's usually a multiple of 2.</param>
        /// <returns>How many times to repeat the character</returns>
        public static int PercentRepeat(int CurrentNumber, int MaximumNumber, int WidthOffset) => (int)Math.Round(CurrentNumber * 100 / (double)MaximumNumber * ((ConsoleWrapper.WindowWidth - WidthOffset) * 0.01d));

        /// <summary>
        /// Gets how many times to repeat the character to represent the appropriate percentage level for the specified number.
        /// </summary>
        /// <param name="CurrentNumber">The current number that is less than or equal to the maximum number.</param>
        /// <param name="MaximumNumber">The maximum number.</param>
        /// <param name="TargetWidth">The target width</param>
        /// <returns>How many times to repeat the character</returns>
        public static int PercentRepeatTargeted(int CurrentNumber, int MaximumNumber, int TargetWidth) => (int)Math.Round(CurrentNumber * 100 / (double)MaximumNumber * (TargetWidth * 0.01d));

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
        /// <param name="Vars">Variables to be formatted in the text</param>
        public static (int, int) GetFilteredPositions(string Text, params object[] Vars)
        {
            // Filter all text from the VT escape sequences
            Text = FilterVTSequences(Text);

            // Seek through filtered text (make it seem like it came from Linux by removing CR (\r)), return to the old position, and return the filtered positions
            Text = TextTools.FormatString(Text, Vars);
            Text = Text.Replace(Convert.ToString(Convert.ToChar(13)), "");
            Text = Text.Replace(Convert.ToString(Convert.ToChar(0)), "");
            int LeftSeekPosition = ConsoleWrapper.CursorLeft;
            int TopSeekPosition = ConsoleWrapper.CursorTop;
            for (int i = 1; i <= Text.Length; i++)
            {
                // If we spotted a new line character, get down by one line.
                if (Text[i - 1] == Convert.ToChar(10))
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

                        // Get down by one line
                        TopSeekPosition += 1;
                        if (TopSeekPosition > ConsoleWrapper.BufferHeight - 1)
                        {
                            // We're at the end of buffer! Decrement by one.
                            TopSeekPosition -= 1;
                        }
                    }
                }
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

        #region Windows-specific
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleMode(IntPtr handle, out int mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int handle);

        internal static void InitializeSequences()
        {
            IntPtr stdHandle = GetStdHandle(-11);
            GetConsoleMode(stdHandle, out var mode);
            if (mode != 7)
            {
                SetConsoleMode(stdHandle, mode | 4);
            }
        }
        #endregion

    }
}
