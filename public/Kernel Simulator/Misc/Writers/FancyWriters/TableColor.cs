
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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
using System.Collections.Generic;
using ColorSeq;
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters.Tools;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Writers.FancyWriters
{
    /// <summary>
    /// Table writer with color support
    /// </summary>
    public static class TableColor
    {

        /// <summary>
        /// Draw a table with text
        /// </summary>
        /// <param name="Headers">Headers to insert to the table.</param>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="Margin">Safe threshold from left</param>
        /// <param name="SeparateRows">Separate the rows?</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        public static void WriteTable(string[] Headers, string[,] Rows, int Margin, bool SeparateRows = true, List<CellOptions> CellOptions = null) => 
            WriteTable(Headers, Rows, Margin, KernelColorType.TableSeparator, KernelColorType.TableHeader, KernelColorType.TableValue, KernelColorType.Background, SeparateRows, CellOptions);

        /// <summary>
        /// Draw a table with text
        /// </summary>
        /// <param name="Headers">Headers to insert to the table.</param>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="Margin">Margin offset</param>
        /// <param name="SeparateRows">Separate the rows?</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        /// <param name="colorTypeSeparatorForeground">A type of colors that will be changed for the separator foreground color.</param>
        /// <param name="colorTypeHeaderForeground">A type of colors that will be changed for the header foreground color.</param>
        /// <param name="colorTypeValueForeground">A type of colors that will be changed for the value foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        public static void WriteTable(string[] Headers, string[,] Rows, int Margin, KernelColorType colorTypeSeparatorForeground, KernelColorType colorTypeHeaderForeground, KernelColorType colorTypeValueForeground, KernelColorType colorTypeBackground, bool SeparateRows = true, List<CellOptions> CellOptions = null) =>
            WriteTable(Headers, Rows, Margin, ColorTools.GetColor(colorTypeSeparatorForeground), ColorTools.GetColor(colorTypeHeaderForeground), ColorTools.GetColor(colorTypeValueForeground), ColorTools.GetColor(colorTypeBackground), SeparateRows, CellOptions);

        /// <summary>
        /// Draw a table with text
        /// </summary>
        /// <param name="Headers">Headers to insert to the table.</param>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="Margin">Margin offset</param>
        /// <param name="SeparateRows">Separate the rows?</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        /// <param name="SeparatorForegroundColor">A separator foreground color that will be changed to.</param>
        /// <param name="HeaderForegroundColor">A header foreground color that will be changed to.</param>
        /// <param name="ValueForegroundColor">A value foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        public static void WriteTable(string[] Headers, string[,] Rows, int Margin, ConsoleColors SeparatorForegroundColor, ConsoleColors HeaderForegroundColor, ConsoleColors ValueForegroundColor, ConsoleColors BackgroundColor, bool SeparateRows = true, List<CellOptions> CellOptions = null) =>
            WriteTable(Headers, Rows, Margin, new Color(Convert.ToInt32(SeparatorForegroundColor)), new Color(Convert.ToInt32(HeaderForegroundColor)), new Color(Convert.ToInt32(ValueForegroundColor)), new Color(Convert.ToInt32(BackgroundColor)), SeparateRows, CellOptions);

        /// <summary>
        /// Draw a table with text
        /// </summary>
        /// <param name="Headers">Headers to insert to the table.</param>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="Margin">Margin offset</param>
        /// <param name="SeparateRows">Separate the rows?</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        /// <param name="SeparatorForegroundColor">A separator foreground color that will be changed to.</param>
        /// <param name="HeaderForegroundColor">A header foreground color that will be changed to.</param>
        /// <param name="ValueForegroundColor">A value foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        public static void WriteTable(string[] Headers, string[,] Rows, int Margin, Color SeparatorForegroundColor, Color HeaderForegroundColor, Color ValueForegroundColor, Color BackgroundColor, bool SeparateRows = true, List<CellOptions> CellOptions = null)
        {
            int ColumnCapacity = (int)Math.Round(ConsoleBase.ConsoleWrapper.WindowWidth / (double)Headers.Length);
            var ColumnPositions = new List<int>();
            int RepeatTimes;

            // Populate the positions
            TextWriterColor.Write();
            for (int ColumnPosition = Margin; ColumnCapacity >= 0 ? ColumnPosition <= ConsoleBase.ConsoleWrapper.WindowWidth : ColumnPosition >= ConsoleBase.ConsoleWrapper.WindowWidth; ColumnPosition += ColumnCapacity)
            {
                if (!(ColumnPosition >= ConsoleBase.ConsoleWrapper.WindowWidth))
                {
                    ColumnPositions.Add(ColumnPosition);
                    if (ColumnPositions.Count == 1)
                        ColumnPosition = 0;
                }
                else
                {
                    break;
                }
            }

            // Write the headers
            for (int HeaderIndex = 0; HeaderIndex <= Headers.Length - 1; HeaderIndex++)
            {
                string Header = Headers[HeaderIndex];
                int ColumnPosition = ColumnPositions[HeaderIndex];
                Header ??= "";
                TextWriterWhereColor.WriteWhere(Header.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, ConsoleBase.ConsoleWrapper.CursorTop, false, HeaderForegroundColor, BackgroundColor);
            }
            TextWriterColor.Write();

            // Write the closing minus sign.
            int OldTop = ConsoleBase.ConsoleWrapper.CursorTop;
            RepeatTimes = ConsoleBase.ConsoleWrapper.WindowWidth - ConsoleBase.ConsoleWrapper.CursorLeft - Margin * 2;
            if (Margin > 0)
                TextWriterColor.Write(" ".Repeat(Margin), false, SeparatorForegroundColor, BackgroundColor);
            TextWriterColor.Write("-".Repeat(RepeatTimes), true, SeparatorForegroundColor, BackgroundColor);

            // Fix CursorTop value on Unix systems.
            if (KernelPlatform.IsOnUnix())
            {
                if (!(ConsoleBase.ConsoleWrapper.CursorTop == ConsoleBase.ConsoleWrapper.WindowHeight - 1) | OldTop == ConsoleBase.ConsoleWrapper.WindowHeight - 3)
                    ConsoleBase.ConsoleWrapper.CursorTop -= 1;
            }

            // Write the rows
            for (int RowIndex = 0; RowIndex <= Rows.GetLength(0) - 1; RowIndex++)
            {
                for (int RowValueIndex = 0; RowValueIndex <= Rows.GetLength(1) - 1; RowValueIndex++)
                {
                    var ColoredCell = false;
                    var CellColor = ColorTools.GetColor(KernelColorType.NeutralText);
                    var CellBackgroundColor = ColorTools.GetColor(KernelColorType.Background);
                    string RowValue = Rows[RowIndex, RowValueIndex];
                    int ColumnPosition = ColumnPositions[RowValueIndex];
                    RowValue ??= "";

                    // Get the cell options and set them as necessary
                    if (CellOptions is not null)
                    {
                        foreach (CellOptions CellOption in CellOptions)
                        {
                            if (CellOption.ColumnIndex == RowValueIndex & CellOption.RowIndex == RowIndex)
                            {
                                ColoredCell = CellOption.ColoredCell;
                                CellColor = CellOption.CellColor;
                                CellBackgroundColor = CellOption.CellBackgroundColor;
                            }
                        }
                    }

                    // Now, write the cell value
                    if (ColoredCell)
                    {
                        TextWriterWhereColor.WriteWhere(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, ConsoleBase.ConsoleWrapper.CursorTop, false, CellColor, CellBackgroundColor);
                    }
                    else
                    {
                        TextWriterWhereColor.WriteWhere(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, ConsoleBase.ConsoleWrapper.CursorTop, false, ValueForegroundColor, BackgroundColor);
                    }
                }
                TextWriterColor.Write();

                // Separate the rows optionally
                if (SeparateRows)
                {
                    // Write the closing minus sign.
                    OldTop = ConsoleBase.ConsoleWrapper.CursorTop;
                    RepeatTimes = ConsoleBase.ConsoleWrapper.WindowWidth - ConsoleBase.ConsoleWrapper.CursorLeft - Margin * 2;
                    if (Margin > 0)
                        TextWriterColor.Write(" ".Repeat(Margin), false, SeparatorForegroundColor, BackgroundColor);
                    TextWriterColor.Write("-".Repeat(RepeatTimes), true, SeparatorForegroundColor, BackgroundColor);

                    // Fix CursorTop value on Unix systems.
                    if (KernelPlatform.IsOnUnix())
                    {
                        if (!(ConsoleBase.ConsoleWrapper.CursorTop == ConsoleBase.ConsoleWrapper.WindowHeight - 1) | OldTop == ConsoleBase.ConsoleWrapper.WindowHeight - 3)
                            ConsoleBase.ConsoleWrapper.CursorTop -= 1;
                    }
                }
            }
        }

    }
}
