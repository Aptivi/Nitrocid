//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers.ConsoleWriters;
using Nitrocid.Files;
using Nitrocid.Files.Operations;
using System.Text;
using Terminaux.Colors;

namespace Nitrocid.ConsoleBase.Writers.MiscWriters
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
        public static void PrintLineWithHandleConditional(bool Condition, string Filename, int LineNumber, int ColumnNumber) =>
            PrintLineWithHandleConditional(Condition, Filename, LineNumber, ColumnNumber, KernelColorTools.GetColor(KernelColorType.NeutralText));

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        public static void PrintLineWithHandleConditional(bool Condition, string[] Array, int LineNumber, int ColumnNumber) =>
            PrintLineWithHandleConditional(Condition, Array, LineNumber, ColumnNumber, KernelColorTools.GetColor(KernelColorType.NeutralText));

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="color">The color</param>
        public static void PrintLineWithHandleConditional(bool Condition, string Filename, int LineNumber, int ColumnNumber, Color color)
        {
            if (Condition)
            {
                PrintLineWithHandle(Filename, LineNumber, ColumnNumber, color);
            }
        }

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="color">The color</param>
        public static void PrintLineWithHandleConditional(bool Condition, string[] Array, int LineNumber, int ColumnNumber, Color color)
        {
            if (Condition)
            {
                PrintLineWithHandle(Array, LineNumber, ColumnNumber, color);
            }
        }

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        public static void PrintLineWithHandle(string Filename, int LineNumber, int ColumnNumber) =>
            PrintLineWithHandle(Filename, LineNumber, ColumnNumber, KernelColorTools.GetColor(KernelColorType.NeutralText));

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        public static void PrintLineWithHandle(string[] Array, int LineNumber, int ColumnNumber) =>
            PrintLineWithHandle(Array, LineNumber, ColumnNumber, KernelColorTools.GetColor(KernelColorType.NeutralText));

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="color">The color</param>
        public static void PrintLineWithHandle(string Filename, int LineNumber, int ColumnNumber, Color color)
        {
            // Read the contents
            FilesystemTools.ThrowOnInvalidPath(Filename);
            Filename = FilesystemTools.NeutralizePath(Filename);
            var FileContents = Reading.ReadContents(Filename);

            // Do the job
            PrintLineWithHandle(FileContents, LineNumber, ColumnNumber, color);
        }

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="color">The color</param>
        public static void PrintLineWithHandle(string[] Array, int LineNumber, int ColumnNumber, Color color) =>
            TextWriterColor.WriteColor(RenderLineWithHandle(Array, LineNumber, ColumnNumber, color), true, color);

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        public static string RenderLineWithHandleConditional(bool Condition, string Filename, int LineNumber, int ColumnNumber) =>
            RenderLineWithHandleConditional(Condition, Filename, LineNumber, ColumnNumber, KernelColorTools.GetColor(KernelColorType.NeutralText));

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        public static string RenderLineWithHandleConditional(bool Condition, string[] Array, int LineNumber, int ColumnNumber) =>
            RenderLineWithHandleConditional(Condition, Array, LineNumber, ColumnNumber, KernelColorTools.GetColor(KernelColorType.NeutralText));

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="color">The color</param>
        public static string RenderLineWithHandleConditional(bool Condition, string Filename, int LineNumber, int ColumnNumber, Color color)
        {
            if (Condition)
                return RenderLineWithHandle(Filename, LineNumber, ColumnNumber, color);
            return "";
        }

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="color">The color</param>
        public static string RenderLineWithHandleConditional(bool Condition, string[] Array, int LineNumber, int ColumnNumber, Color color)
        {
            if (Condition)
                return RenderLineWithHandle(Array, LineNumber, ColumnNumber, color);
            return "";
        }

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        public static string RenderLineWithHandle(string Filename, int LineNumber, int ColumnNumber) =>
            RenderLineWithHandle(Filename, LineNumber, ColumnNumber, KernelColorTools.GetColor(KernelColorType.NeutralText));

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        public static string RenderLineWithHandle(string[] Array, int LineNumber, int ColumnNumber) =>
            RenderLineWithHandle(Array, LineNumber, ColumnNumber, KernelColorTools.GetColor(KernelColorType.NeutralText));

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="color">The color</param>
        public static string RenderLineWithHandle(string Filename, int LineNumber, int ColumnNumber, Color color)
        {
            // Read the contents
            FilesystemTools.ThrowOnInvalidPath(Filename);
            Filename = FilesystemTools.NeutralizePath(Filename);
            var FileContents = Reading.ReadContents(Filename);

            // Do the job
            return RenderLineWithHandle(FileContents, LineNumber, ColumnNumber, color);
        }

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="color">The color</param>
        public static string RenderLineWithHandle(string[] Array, int LineNumber, int ColumnNumber, Color color)
        {
            // Get the builder
            StringBuilder builder = new();

            // Get the line index from number
            if (LineNumber <= 0)
                LineNumber = 1;
            if (LineNumber > Array.Length)
                LineNumber = Array.Length;
            int LineIndex = LineNumber - 1;

            // Get the line
            string LineContent = Array[LineIndex];

            // Now, check the column number
            if (ColumnNumber < 0 || ColumnNumber > LineContent.Length)
                ColumnNumber = LineContent.Length;

            // Place the line and the column handle
            int RepeatBlanks = ColumnNumber - 1;
            if (RepeatBlanks < 0)
                RepeatBlanks = 0;
            builder.AppendLine($"{color.VTSequenceForeground}  | {LineContent}");
            builder.AppendLine($"{color.VTSequenceForeground}  | {new string(' ', RepeatBlanks)}^");

            // Write the resulting buffer
            builder.Append(
                KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground +
                KernelColorTools.GetColor(KernelColorType.Background).VTSequenceBackground
            );
            return builder.ToString();
        }

    }
}
