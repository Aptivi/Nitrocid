using System;
using KS.Misc.Reflection;

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

using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using VT.NET;

namespace KS.ConsoleBase
{
    public static class ConsoleExtensions
    {

        /// <summary>
        /// Clears the console buffer, but keeps the current cursor position
        /// </summary>
        public static void ClearKeepPosition()
        {
            int Left = Console.CursorLeft;
            int Top = Console.CursorTop;
            Console.Clear();
            Console.SetCursorPosition(Left, Top);
        }

        /// <summary>
        /// Clears the line to the right
        /// </summary>
        public static void ClearLineToRight()
        {
            Console.Write(Convert.ToString(CharManager.GetEsc()) + "[0K");
        }

        /// <summary>
        /// Gets how many times to repeat the character to represent the appropriate percentage level for the specified number.
        /// </summary>
        /// <param name="CurrentNumber">The current number that is less than or equal to the maximum number.</param>
        /// <param name="MaximumNumber">The maximum number.</param>
        /// <param name="WidthOffset">The console window width offset. It's usually a multiple of 2.</param>
        /// <returns>How many times to repeat the character</returns>
        public static int PercentRepeat(int CurrentNumber, int MaximumNumber, int WidthOffset)
        {
            return (int)Math.Round(CurrentNumber * 100 / (double)MaximumNumber * ((Console.WindowWidth - WidthOffset) * 0.01d));
        }

        /// <summary>
        /// Gets how many times to repeat the character to represent the appropriate percentage level for the specified number.
        /// </summary>
        /// <param name="CurrentNumber">The current number that is less than or equal to the maximum number.</param>
        /// <param name="MaximumNumber">The maximum number.</param>
        /// <param name="TargetWidth">The target width</param>
        /// <returns>How many times to repeat the character</returns>
        public static int PercentRepeatTargeted(int CurrentNumber, int MaximumNumber, int TargetWidth)
        {
            return (int)Math.Round(CurrentNumber * 100 / (double)MaximumNumber * (TargetWidth * 0.01d));
        }

        /// <summary>
        /// Filters the VT sequences that matches the regex
        /// </summary>
        /// <param name="Text">The text that contains the VT sequences</param>
        /// <returns>The text that doesn't contain the VT sequences</returns>
        public static string FilterVTSequences(string Text)
        {
            // Filter all sequences
            Text = Filters.FilterVTSequences(Text);
            return Text;
        }

        /// <summary>
        /// Get the filtered cursor positions (by filtered means filtered from the VT escape sequences that matches the regex in the routine)
        /// </summary>
        /// <param name="Text">The text that contains the VT sequences</param>
        /// <param name="Left">The filtered left position</param>
        /// <param name="Top">The filtered top position</param>
        public static void GetFilteredPositions(string Text, ref int Left, ref int Top, params object[] Vars)
        {
            // First, get the old cursor positions
            int OldLeft = Console.CursorLeft;
            int OldTop = Console.CursorTop;

            // Second, filter all text from the VT escape sequences
            Text = FilterVTSequences(Text);

            // Third, seek through filtered text (make it seem like it came from Linux by removing CR (\r)), return to the old position, and return the filtered positions
            Text = StringManipulate.FormatString(Text, Vars);
            Text = Text.Replace(Convert.ToString(Convert.ToChar(13)), "");
            int LeftSeekPosition = OldLeft;
            int TopSeekPosition = OldTop;
            for (int i = 1, loopTo = Text.Length; i <= loopTo; i++)
            {
                // If we spotted a new line character, get down by one line.
                if (Text[i - 1] == Convert.ToChar(10) & TopSeekPosition < Console.BufferHeight - 1)
                {
                    TopSeekPosition += 1;
                }
                else if (Text[i - 1] != Convert.ToChar(10))
                {
                    // Simulate seeking through text
                    LeftSeekPosition += 1;
                    if (LeftSeekPosition >= Console.WindowWidth)
                    {
                        // We've reached end of line
                        LeftSeekPosition = 0;

                        // Get down by one line
                        TopSeekPosition += 1;
                        if (TopSeekPosition > Console.BufferHeight - 1)
                        {
                            // We're at the end of buffer! Decrement by one.
                            TopSeekPosition -= 1;
                        }
                    }
                }
            }
            Left = LeftSeekPosition;
            Top = TopSeekPosition;

            // Finally, set the correct old position
            Console.SetCursorPosition(OldLeft, OldTop);
        }

        public static void SetTitle(string Text)
        {
            char BellChar = Convert.ToChar(7);
            char EscapeChar = Convert.ToChar(27);
            string Sequence = $"{EscapeChar}]0;{Text}{BellChar}";
            Console.Title = Text;
            TextWriterColor.WritePlain(Sequence, false);
        }

    }
}