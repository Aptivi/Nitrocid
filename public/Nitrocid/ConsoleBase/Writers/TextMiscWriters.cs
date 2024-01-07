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
using Terminaux.Writer.MiscWriters;

namespace Nitrocid.ConsoleBase.Writers
{
    /// <summary>
    /// Miscellaneous text writer wrapper for writing with <see cref="KernelColorType"/> (<see cref="Terminaux.Writer.MiscWriters"/>)
    /// </summary>
    public static class TextMiscWriters
    {
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
            LineHandleRangedWriter.PrintLineWithHandleConditional(Condition, Filename, LineNumber, startPos, endPos, KernelColorTools.GetColor(ColorType));

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
            LineHandleRangedWriter.PrintLineWithHandleConditional(Condition, Array, LineNumber, startPos, endPos, KernelColorTools.GetColor(ColorType));

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="ColorType">The type of color</param>
        public static void PrintLineWithHandle(string Filename, int LineNumber, int startPos, int endPos, KernelColorType ColorType) =>
            LineHandleRangedWriter.PrintLineWithHandle(Filename, LineNumber, startPos, endPos, KernelColorTools.GetColor(ColorType));

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="ColorType">The type of color</param>
        public static void PrintLineWithHandle(string[] Array, int LineNumber, int startPos, int endPos, KernelColorType ColorType) =>
            LineHandleRangedWriter.PrintLineWithHandle(Array, LineNumber, startPos, endPos, KernelColorTools.GetColor(ColorType));

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
            LineHandleRangedWriter.RenderLineWithHandleConditional(Condition, Array, LineNumber, startPos, endPos, KernelColorTools.GetColor(ColorType));

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="ColorType">The type of color</param>
        public static string RenderLineWithHandle(string Filename, int LineNumber, int startPos, int endPos, KernelColorType ColorType) =>
            LineHandleRangedWriter.RenderLineWithHandle(Filename, LineNumber, startPos, endPos, KernelColorTools.GetColor(ColorType));

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="ColorType">The type of color</param>
        public static string RenderLineWithHandle(string[] Array, int LineNumber, int startPos, int endPos, KernelColorType ColorType) =>
            LineHandleRangedWriter.RenderLineWithHandle(Array, LineNumber, startPos, endPos, KernelColorTools.GetColor(ColorType));

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="ColorType">The type of color</param>
        public static void PrintLineWithHandleConditional(bool Condition, string Filename, int LineNumber, int ColumnNumber, KernelColorType ColorType) =>
            LineHandleWriter.PrintLineWithHandleConditional(Condition, Filename, LineNumber, ColumnNumber, KernelColorTools.GetColor(ColorType));

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="ColorType">The type of color</param>
        public static void PrintLineWithHandleConditional(bool Condition, string[] Array, int LineNumber, int ColumnNumber, KernelColorType ColorType) =>
            LineHandleWriter.PrintLineWithHandleConditional(Condition, Array, LineNumber, ColumnNumber, KernelColorTools.GetColor(ColorType));

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="ColorType">The type of color</param>
        public static void PrintLineWithHandle(string Filename, int LineNumber, int ColumnNumber, KernelColorType ColorType) =>
            LineHandleWriter.PrintLineWithHandle(Filename, LineNumber, ColumnNumber, KernelColorTools.GetColor(ColorType));

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="ColorType">The type of color</param>
        public static void PrintLineWithHandle(string[] Array, int LineNumber, int ColumnNumber, KernelColorType ColorType) =>
            LineHandleWriter.PrintLineWithHandle(Array, LineNumber, ColumnNumber, KernelColorTools.GetColor(ColorType));

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="ColorType">The type of color</param>
        public static string RenderLineWithHandleConditional(bool Condition, string Filename, int LineNumber, int ColumnNumber, KernelColorType ColorType)
        {
            if (Condition)
                return RenderLineWithHandle(Filename, LineNumber, ColumnNumber, ColorType);
            return "";
        }

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="ColorType">The type of color</param>
        public static string RenderLineWithHandleConditional(bool Condition, string[] Array, int LineNumber, int ColumnNumber, KernelColorType ColorType) =>
            LineHandleWriter.RenderLineWithHandleConditional(Condition, Array, LineNumber, ColumnNumber, KernelColorTools.GetColor(ColorType));

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="ColorType">The type of color</param>
        public static string RenderLineWithHandle(string Filename, int LineNumber, int ColumnNumber, KernelColorType ColorType) =>
            LineHandleWriter.RenderLineWithHandle(Filename, LineNumber, ColumnNumber, KernelColorTools.GetColor(ColorType));

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="ColorType">The type of color</param>
        public static string RenderLineWithHandle(string[] Array, int LineNumber, int ColumnNumber, KernelColorType ColorType) =>
            LineHandleWriter.RenderLineWithHandle(Array, LineNumber, ColumnNumber, KernelColorTools.GetColor(ColorType));
    }
}
