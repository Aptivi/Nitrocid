
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files;
using KS.Files.Read;
using KS.Misc.Reflection;
using System.Text;
using Terminaux.Colors;

namespace KS.ConsoleBase.Writers.MiscWriters
{
    /// <summary>
    /// Ranged line handle writer (with start and end positions)
    /// </summary>
    public static class LineHandleRangedWriter
    {

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself as the start.</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        public static void PrintLineWithHandleConditional(bool Condition, string Filename, int LineNumber, int startPos, int endPos) =>
            PrintLineWithHandleConditional(Condition, Filename, LineNumber, startPos, endPos, KernelColorType.NeutralText);

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        public static void PrintLineWithHandleConditional(bool Condition, string[] Array, int LineNumber, int startPos, int endPos) =>
            PrintLineWithHandleConditional(Condition, Array, LineNumber, startPos, endPos, KernelColorType.NeutralText);

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="ColorType">The type of color</param>
        public static void PrintLineWithHandleConditional(bool Condition, string Filename, int LineNumber, int startPos, int endPos, KernelColorType ColorType) =>
            PrintLineWithHandleConditional(Condition, Filename, LineNumber, startPos, endPos, KernelColorTools.GetColor(ColorType));

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="ColorType">The type of color</param>
        public static void PrintLineWithHandleConditional(bool Condition, string[] Array, int LineNumber, int startPos, int endPos, KernelColorType ColorType) =>
            PrintLineWithHandleConditional(Condition, Array, LineNumber, startPos, endPos, KernelColorTools.GetColor(ColorType));

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="color">The color</param>
        public static void PrintLineWithHandleConditional(bool Condition, string Filename, int LineNumber, int startPos, int endPos, Color color)
        {
            if (Condition)
            {
                PrintLineWithHandle(Filename, LineNumber, startPos, endPos, color);
            }
        }

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="color">The color</param>
        public static void PrintLineWithHandleConditional(bool Condition, string[] Array, int LineNumber, int startPos, int endPos, Color color)
        {
            if (Condition)
            {
                PrintLineWithHandle(Array, LineNumber, startPos, endPos, color);
            }
        }

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        public static void PrintLineWithHandle(string Filename, int LineNumber, int startPos, int endPos) =>
            PrintLineWithHandle(Filename, LineNumber, startPos, endPos, KernelColorType.NeutralText);

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        public static void PrintLineWithHandle(string[] Array, int LineNumber, int startPos, int endPos) =>
            PrintLineWithHandle(Array, LineNumber, startPos, endPos, KernelColorType.NeutralText);

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="ColorType">The type of color</param>
        public static void PrintLineWithHandle(string Filename, int LineNumber, int startPos, int endPos, KernelColorType ColorType) =>
            PrintLineWithHandle(Filename, LineNumber, startPos, endPos, KernelColorTools.GetColor(ColorType));

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="ColorType">The type of color</param>
        public static void PrintLineWithHandle(string[] Array, int LineNumber, int startPos, int endPos, KernelColorType ColorType) =>
            PrintLineWithHandle(Array, LineNumber, startPos, endPos, KernelColorTools.GetColor(ColorType));

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="color">The color</param>
        public static void PrintLineWithHandle(string Filename, int LineNumber, int startPos, int endPos, Color color)
        {
            // Read the contents
            Filesystem.ThrowOnInvalidPath(Filename);
            Filename = Filesystem.NeutralizePath(Filename);
            var FileContents = FileRead.ReadContents(Filename);

            // Do the job
            PrintLineWithHandle(FileContents, LineNumber, startPos, endPos, color);
        }

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="color">The color</param>
        public static void PrintLineWithHandle(string[] Array, int LineNumber, int startPos, int endPos, Color color) =>
            TextWriterColor.Write(RenderLineWithHandle(Array, LineNumber, startPos, endPos, color), true, color);

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        public static string RenderLineWithHandleConditional(bool Condition, string Filename, int LineNumber, int startPos, int endPos) =>
            RenderLineWithHandleConditional(Condition, Filename, LineNumber, startPos, endPos, KernelColorType.NeutralText);

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        public static string RenderLineWithHandleConditional(bool Condition, string[] Array, int LineNumber, int startPos, int endPos) =>
            RenderLineWithHandleConditional(Condition, Array, LineNumber, startPos, endPos, KernelColorType.NeutralText);

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="ColorType">The type of color</param>
        public static string RenderLineWithHandleConditional(bool Condition, string Filename, int LineNumber, int startPos, int endPos, KernelColorType ColorType)
        {
            if (Condition)
                return RenderLineWithHandle(Filename, LineNumber, startPos, endPos, ColorType);
            return "";
        }

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="ColorType">The type of color</param>
        public static string RenderLineWithHandleConditional(bool Condition, string[] Array, int LineNumber, int startPos, int endPos, KernelColorType ColorType) =>
            RenderLineWithHandleConditional(Condition, Array, LineNumber, startPos, endPos, KernelColorTools.GetColor(ColorType));

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="color">The color</param>
        public static string RenderLineWithHandleConditional(bool Condition, string Filename, int LineNumber, int startPos, int endPos, Color color)
        {
            if (Condition)
                return RenderLineWithHandle(Filename, LineNumber, startPos, endPos, color);
            return "";
        }

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="color">The color</param>
        public static string RenderLineWithHandleConditional(bool Condition, string[] Array, int LineNumber, int startPos, int endPos, Color color)
        {
            if (Condition)
                return RenderLineWithHandle(Array, LineNumber, startPos, endPos, color);
            return "";
        }

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        public static string RenderLineWithHandle(string Filename, int LineNumber, int startPos, int endPos) =>
            RenderLineWithHandle(Filename, LineNumber, startPos, endPos, KernelColorType.NeutralText);

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        public static string RenderLineWithHandle(string[] Array, int LineNumber, int startPos, int endPos) =>
            RenderLineWithHandle(Array, LineNumber, startPos, endPos, KernelColorType.NeutralText);

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="ColorType">The type of color</param>
        public static string RenderLineWithHandle(string Filename, int LineNumber, int startPos, int endPos, KernelColorType ColorType) =>
            RenderLineWithHandle(Filename, LineNumber, startPos, endPos, KernelColorTools.GetColor(ColorType));

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="ColorType">The type of color</param>
        public static string RenderLineWithHandle(string[] Array, int LineNumber, int startPos, int endPos, KernelColorType ColorType) =>
            RenderLineWithHandle(Array, LineNumber, startPos, endPos, KernelColorTools.GetColor(ColorType));

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="color">The color</param>
        public static string RenderLineWithHandle(string Filename, int LineNumber, int startPos, int endPos, Color color)
        {
            // Read the contents
            Filesystem.ThrowOnInvalidPath(Filename);
            Filename = Filesystem.NeutralizePath(Filename);
            var FileContents = FileRead.ReadContents(Filename);

            // Do the job
            return RenderLineWithHandle(FileContents, LineNumber, startPos, endPos, color);
        }

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="color">The color</param>
        public static string RenderLineWithHandle(string[] Array, int LineNumber, int startPos, int endPos, Color color)
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

            // Now, check the column numbers
            if (startPos < 0 || startPos > LineContent.Length)
                startPos = LineContent.Length;
            if (endPos < 0 || endPos > LineContent.Length)
                endPos = LineContent.Length;

            // Check to see if the start position is smaller than the end position
            startPos.SwapIfSourceLarger(ref endPos);

            // Place the line and the column handle
            int RepeatBlanks = startPos - 1;
            int RepeatMarkers = endPos - startPos;
            if (RepeatBlanks < 0)
                RepeatBlanks = 0;
            if (RepeatMarkers < 0)
                RepeatMarkers = 0;
            builder.AppendLine($"{color.VTSequenceForeground}  | {LineContent}");
            builder.AppendLine($"{color.VTSequenceForeground}  | {new string(' ', RepeatBlanks)}^{new string('~', RepeatMarkers)}");
            return builder.ToString();
        }

    }
}
