
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

using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Read;
using KS.Misc.Writers.ConsoleWriters;

namespace KS.Misc.Writers.MiscWriters
{
    /// <summary>
    /// Line handle writer
    /// </summary>
    public static class LineHandleWriter
    {

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        public static void PrintLineWithHandleConditional(bool Condition, string Filename, int LineNumber, int ColumnNumber) => PrintLineWithHandleConditional(Condition, Filename, LineNumber, ColumnNumber, KernelColorType.NeutralText);

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        public static void PrintLineWithHandleConditional(bool Condition, string[] Array, int LineNumber, int ColumnNumber) => PrintLineWithHandleConditional(Condition, Array, LineNumber, ColumnNumber, KernelColorType.NeutralText);

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="ColorType">The type of color</param>
        public static void PrintLineWithHandleConditional(bool Condition, string Filename, int LineNumber, int ColumnNumber, KernelColorType ColorType)
        {
            if (Condition)
            {
                PrintLineWithHandle(Filename, LineNumber, ColumnNumber, ColorType);
            }
        }

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="ColorType">The type of color</param>
        public static void PrintLineWithHandleConditional(bool Condition, string[] Array, int LineNumber, int ColumnNumber, KernelColorType ColorType)
        {
            if (Condition)
            {
                PrintLineWithHandle(Array, LineNumber, ColumnNumber, ColorType);
            }
        }

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        public static void PrintLineWithHandle(string Filename, int LineNumber, int ColumnNumber) => PrintLineWithHandle(Filename, LineNumber, ColumnNumber, KernelColorType.NeutralText);

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        public static void PrintLineWithHandle(string[] Array, int LineNumber, int ColumnNumber) => PrintLineWithHandle(Array, LineNumber, ColumnNumber, KernelColorType.NeutralText);

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="ColorType">The type of color</param>
        public static void PrintLineWithHandle(string Filename, int LineNumber, int ColumnNumber, KernelColorType ColorType)
        {
            // Read the contents
            Filesystem.ThrowOnInvalidPath(Filename);
            Filename = Filesystem.NeutralizePath(Filename);
            var FileContents = FileRead.ReadContents(Filename);

            // Do the job
            PrintLineWithHandle(FileContents, LineNumber, ColumnNumber, ColorType);
        }

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="ColorType">The type of color</param>
        public static void PrintLineWithHandle(string[] Array, int LineNumber, int ColumnNumber, KernelColorType ColorType)
        {
            // Get the line index from number
            if (LineNumber <= 0)
                LineNumber = 1;
            if (LineNumber > Array.Length)
                LineNumber = Array.Length;
            int LineIndex = LineNumber - 1;

            // Get the line
            string LineContent = Array[LineIndex];
            TextWriterColor.Write(" | " + LineContent, true, ColorType);

            // Place the column handle
            int RepeatBlanks = ColumnNumber - 1;
            if (RepeatBlanks < 0)
                RepeatBlanks = 0;
            TextWriterColor.Write(" | " + " ".Repeat(RepeatBlanks) + "^", true, ColorType);
        }

    }
}
