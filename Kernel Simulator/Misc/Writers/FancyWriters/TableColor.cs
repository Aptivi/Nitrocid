
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

using System;
using System.Collections.Generic;
using ColorSeq;
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters.Tools;

namespace KS.Misc.Writers.FancyWriters
{
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
        public static void WriteTable(string[] Headers, string[,] Rows, int Margin, bool SeparateRows = true, List<CellOptions> CellOptions = null)
        {
            int ColumnCapacity = (int)Math.Round(ConsoleBase.ConsoleWrapper.WindowWidth / (double)Headers.Length);
            var ColumnPositions = new List<int>();
            int RepeatTimes;

            // Populate the positions
            ConsoleBase.ConsoleWrapper.WriteLine();
            for (int ColumnPosition = Margin, loopTo = ConsoleBase.ConsoleWrapper.WindowWidth; ColumnCapacity >= 0 ? ColumnPosition <= loopTo : ColumnPosition >= loopTo; ColumnPosition += ColumnCapacity)
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
            for (int HeaderIndex = 0, loopTo1 = Headers.Length - 1; HeaderIndex <= loopTo1; HeaderIndex++)
            {
                string Header = Headers[HeaderIndex];
                int ColumnPosition = ColumnPositions[HeaderIndex];
                Header ??= "";
                TextWriterWhereColor.WriteWhere(Header.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, ConsoleBase.ConsoleWrapper.CursorTop, false, ColorTools.ColTypes.TableHeader);
            }
            ConsoleBase.ConsoleWrapper.WriteLine();

            // Write the closing minus sign.
            int OldTop = ConsoleBase.ConsoleWrapper.CursorTop;
            RepeatTimes = ConsoleBase.ConsoleWrapper.WindowWidth - ConsoleBase.ConsoleWrapper.CursorLeft - Margin * 2;
            if (Margin > 0)
                TextWriterColor.Write(" ".Repeat(Margin), false, ColorTools.ColTypes.Neutral);
            TextWriterColor.Write("-".Repeat(RepeatTimes), true, ColorTools.ColTypes.TableSeparator);

            // Fix CursorTop value on Unix systems.
            if (KernelPlatform.IsOnUnix())
            {
                if (!(ConsoleBase.ConsoleWrapper.CursorTop == ConsoleBase.ConsoleWrapper.WindowHeight - 1) | OldTop == ConsoleBase.ConsoleWrapper.WindowHeight - 3)
                    ConsoleBase.ConsoleWrapper.CursorTop -= 1;
            }

            // Write the rows
            for (int RowIndex = 0, loopTo2 = Rows.GetLength(0) - 1; RowIndex <= loopTo2; RowIndex++)
            {
                for (int RowValueIndex = 0, loopTo3 = Rows.GetLength(1) - 1; RowValueIndex <= loopTo3; RowValueIndex++)
                {
                    var ColoredCell = false;
                    var CellColor = ColorTools.NeutralTextColor;
                    var CellBackgroundColor = ColorTools.BackgroundColor;
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
                        TextWriterWhereColor.WriteWhere(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, ConsoleBase.ConsoleWrapper.CursorTop, false, ColorTools.ColTypes.TableValue);
                    }
                }
                ConsoleBase.ConsoleWrapper.WriteLine();

                // Separate the rows optionally
                if (SeparateRows)
                {
                    // Write the closing minus sign.
                    OldTop = ConsoleBase.ConsoleWrapper.CursorTop;
                    RepeatTimes = ConsoleBase.ConsoleWrapper.WindowWidth - ConsoleBase.ConsoleWrapper.CursorLeft - Margin * 2;
                    if (Margin > 0)
                        TextWriterColor.Write(" ".Repeat(Margin), false, ColorTools.ColTypes.Neutral);
                    TextWriterColor.Write("-".Repeat(RepeatTimes), true, ColorTools.ColTypes.TableSeparator);

                    // Fix CursorTop value on Unix systems.
                    if (KernelPlatform.IsOnUnix())
                    {
                        if (!(ConsoleBase.ConsoleWrapper.CursorTop == ConsoleBase.ConsoleWrapper.WindowHeight - 1) | OldTop == ConsoleBase.ConsoleWrapper.WindowHeight - 3)
                            ConsoleBase.ConsoleWrapper.CursorTop -= 1;
                    }
                }
            }
        }

        /// <summary>
        /// Draw a table with text
        /// </summary>
        /// <param name="Headers">Headers to insert to the table.</param>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="Margin">Margin offset</param>
        /// <param name="SeparateRows">Separate the rows?</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        /// <param name="Color">A color that will be changed to.</param>
        public static void WriteTable(string[] Headers, string[,] Rows, int Margin, ConsoleColor Color, bool SeparateRows = true, List<CellOptions> CellOptions = null)
        {
            int ColumnCapacity = (int)Math.Round(ConsoleBase.ConsoleWrapper.WindowWidth / (double)Headers.Length);
            var ColumnPositions = new List<int>();
            int RepeatTimes;

            // Populate the positions
            ConsoleBase.ConsoleWrapper.WriteLine();
            for (int ColumnPosition = Margin, loopTo = ConsoleBase.ConsoleWrapper.WindowWidth; ColumnCapacity >= 0 ? ColumnPosition <= loopTo : ColumnPosition >= loopTo; ColumnPosition += ColumnCapacity)
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
            for (int HeaderIndex = 0, loopTo1 = Headers.Length - 1; HeaderIndex <= loopTo1; HeaderIndex++)
            {
                string Header = Headers[HeaderIndex];
                int ColumnPosition = ColumnPositions[HeaderIndex];
                Header ??= "";
                TextWriterWhereColor.WriteWhere(Header.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, ConsoleBase.ConsoleWrapper.CursorTop, false, Color);
            }
            ConsoleBase.ConsoleWrapper.WriteLine();

            // Write the closing minus sign.
            int OldTop = ConsoleBase.ConsoleWrapper.CursorTop;
            RepeatTimes = ConsoleBase.ConsoleWrapper.WindowWidth - ConsoleBase.ConsoleWrapper.CursorLeft - Margin * 2;
            if (Margin > 0)
                TextWriterColor.Write(" ".Repeat(Margin), false, Color);
            TextWriterColor.Write("-".Repeat(RepeatTimes), true, Color);

            // Fix CursorTop value on Unix systems.
            if (KernelPlatform.IsOnUnix())
            {
                if (!(ConsoleBase.ConsoleWrapper.CursorTop == ConsoleBase.ConsoleWrapper.WindowHeight - 1) | OldTop == ConsoleBase.ConsoleWrapper.WindowHeight - 3)
                    ConsoleBase.ConsoleWrapper.CursorTop -= 1;
            }

            // Write the rows
            for (int RowIndex = 0, loopTo2 = Rows.GetLength(0) - 1; RowIndex <= loopTo2; RowIndex++)
            {
                for (int RowValueIndex = 0, loopTo3 = Rows.GetLength(1) - 1; RowValueIndex <= loopTo3; RowValueIndex++)
                {
                    var ColoredCell = false;
                    var CellColor = ColorTools.NeutralTextColor;
                    var CellBackgroundColor = ColorTools.BackgroundColor;
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
                        TextWriterWhereColor.WriteWhere(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, ConsoleBase.ConsoleWrapper.CursorTop, false, Color);
                    }
                }
                ConsoleBase.ConsoleWrapper.WriteLine();

                // Separate the rows optionally
                if (SeparateRows)
                {
                    // Write the closing minus sign.
                    OldTop = ConsoleBase.ConsoleWrapper.CursorTop;
                    RepeatTimes = ConsoleBase.ConsoleWrapper.WindowWidth - ConsoleBase.ConsoleWrapper.CursorLeft - Margin * 2;
                    if (Margin > 0)
                        TextWriterColor.Write(" ".Repeat(Margin), false, Color);
                    TextWriterColor.Write("-".Repeat(RepeatTimes), true, Color);

                    // Fix CursorTop value on Unix systems.
                    if (KernelPlatform.IsOnUnix())
                    {
                        if (!(ConsoleBase.ConsoleWrapper.CursorTop == ConsoleBase.ConsoleWrapper.WindowHeight - 1) | OldTop == ConsoleBase.ConsoleWrapper.WindowHeight - 3)
                            ConsoleBase.ConsoleWrapper.CursorTop -= 1;
                    }
                }
            }
        }

        /// <summary>
        /// Draw a table with text
        /// </summary>
        /// <param name="Headers">Headers to insert to the table.</param>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="Margin">Margin offset</param>
        /// <param name="SeparateRows">Separate the rows?</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        public static void WriteTable(string[] Headers, string[,] Rows, int Margin, ConsoleColor ForegroundColor, ConsoleColor BackgroundColor, bool SeparateRows = true, List<CellOptions> CellOptions = null)
        {
            int ColumnCapacity = (int)Math.Round(ConsoleBase.ConsoleWrapper.WindowWidth / (double)Headers.Length);
            var ColumnPositions = new List<int>();
            int RepeatTimes;

            // Populate the positions
            ConsoleBase.ConsoleWrapper.WriteLine();
            for (int ColumnPosition = Margin, loopTo = ConsoleBase.ConsoleWrapper.WindowWidth; ColumnCapacity >= 0 ? ColumnPosition <= loopTo : ColumnPosition >= loopTo; ColumnPosition += ColumnCapacity)
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
            for (int HeaderIndex = 0, loopTo1 = Headers.Length - 1; HeaderIndex <= loopTo1; HeaderIndex++)
            {
                string Header = Headers[HeaderIndex];
                int ColumnPosition = ColumnPositions[HeaderIndex];
                Header ??= "";
                TextWriterWhereColor.WriteWhere(Header.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, ConsoleBase.ConsoleWrapper.CursorTop, false, ForegroundColor, BackgroundColor);
            }
            ConsoleBase.ConsoleWrapper.WriteLine();

            // Write the closing minus sign.
            int OldTop = ConsoleBase.ConsoleWrapper.CursorTop;
            RepeatTimes = ConsoleBase.ConsoleWrapper.WindowWidth - ConsoleBase.ConsoleWrapper.CursorLeft - Margin * 2;
            if (Margin > 0)
                TextWriterColor.Write(" ".Repeat(Margin), false, ForegroundColor, BackgroundColor);
            TextWriterColor.Write("-".Repeat(RepeatTimes), true, ForegroundColor, BackgroundColor);

            // Fix CursorTop value on Unix systems.
            if (KernelPlatform.IsOnUnix())
            {
                if (!(ConsoleBase.ConsoleWrapper.CursorTop == ConsoleBase.ConsoleWrapper.WindowHeight - 1) | OldTop == ConsoleBase.ConsoleWrapper.WindowHeight - 3)
                    ConsoleBase.ConsoleWrapper.CursorTop -= 1;
            }

            // Write the rows
            for (int RowIndex = 0, loopTo2 = Rows.GetLength(0) - 1; RowIndex <= loopTo2; RowIndex++)
            {
                for (int RowValueIndex = 0, loopTo3 = Rows.GetLength(1) - 1; RowValueIndex <= loopTo3; RowValueIndex++)
                {
                    var ColoredCell = false;
                    var CellColor = ColorTools.NeutralTextColor;
                    var CellBackgroundColor = ColorTools.BackgroundColor;
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
                        TextWriterWhereColor.WriteWhere(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, ConsoleBase.ConsoleWrapper.CursorTop, false, ForegroundColor, BackgroundColor);
                    }
                }
                ConsoleBase.ConsoleWrapper.WriteLine();

                // Separate the rows optionally
                if (SeparateRows)
                {
                    // Write the closing minus sign.
                    OldTop = ConsoleBase.ConsoleWrapper.CursorTop;
                    RepeatTimes = ConsoleBase.ConsoleWrapper.WindowWidth - ConsoleBase.ConsoleWrapper.CursorLeft - Margin * 2;
                    if (Margin > 0)
                        TextWriterColor.Write(" ".Repeat(Margin), false, ForegroundColor, BackgroundColor);
                    TextWriterColor.Write("-".Repeat(RepeatTimes), true, ForegroundColor, BackgroundColor);

                    // Fix CursorTop value on Unix systems.
                    if (KernelPlatform.IsOnUnix())
                    {
                        if (!(ConsoleBase.ConsoleWrapper.CursorTop == ConsoleBase.ConsoleWrapper.WindowHeight - 1) | OldTop == ConsoleBase.ConsoleWrapper.WindowHeight - 3)
                            ConsoleBase.ConsoleWrapper.CursorTop -= 1;
                    }
                }
            }
        }

        /// <summary>
        /// Draw a table with text
        /// </summary>
        /// <param name="Headers">Headers to insert to the table.</param>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="Margin">Margin offset</param>
        /// <param name="SeparateRows">Separate the rows?</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        public static void WriteTable(string[] Headers, string[,] Rows, int Margin, ColorTools.ColTypes ColTypes, bool SeparateRows = true, List<CellOptions> CellOptions = null)
        {
            int ColumnCapacity = (int)Math.Round(ConsoleBase.ConsoleWrapper.WindowWidth / (double)Headers.Length);
            var ColumnPositions = new List<int>();
            int RepeatTimes;

            // Populate the positions
            ConsoleBase.ConsoleWrapper.WriteLine();
            for (int ColumnPosition = Margin, loopTo = ConsoleBase.ConsoleWrapper.WindowWidth; ColumnCapacity >= 0 ? ColumnPosition <= loopTo : ColumnPosition >= loopTo; ColumnPosition += ColumnCapacity)
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
            for (int HeaderIndex = 0, loopTo1 = Headers.Length - 1; HeaderIndex <= loopTo1; HeaderIndex++)
            {
                string Header = Headers[HeaderIndex];
                int ColumnPosition = ColumnPositions[HeaderIndex];
                Header ??= "";
                TextWriterWhereColor.WriteWhere(Header.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, ConsoleBase.ConsoleWrapper.CursorTop, false, ColTypes);
            }
            ConsoleBase.ConsoleWrapper.WriteLine();

            // Write the closing minus sign.
            int OldTop = ConsoleBase.ConsoleWrapper.CursorTop;
            RepeatTimes = ConsoleBase.ConsoleWrapper.WindowWidth - ConsoleBase.ConsoleWrapper.CursorLeft - Margin * 2;
            if (Margin > 0)
                TextWriterColor.Write(" ".Repeat(Margin), false, ColTypes);
            TextWriterColor.Write("-".Repeat(RepeatTimes), true, ColTypes);

            // Fix CursorTop value on Unix systems.
            if (KernelPlatform.IsOnUnix())
            {
                if (!(ConsoleBase.ConsoleWrapper.CursorTop == ConsoleBase.ConsoleWrapper.WindowHeight - 1) | OldTop == ConsoleBase.ConsoleWrapper.WindowHeight - 3)
                    ConsoleBase.ConsoleWrapper.CursorTop -= 1;
            }

            // Write the rows
            for (int RowIndex = 0, loopTo2 = Rows.GetLength(0) - 1; RowIndex <= loopTo2; RowIndex++)
            {
                for (int RowValueIndex = 0, loopTo3 = Rows.GetLength(1) - 1; RowValueIndex <= loopTo3; RowValueIndex++)
                {
                    var ColoredCell = false;
                    var CellColor = ColorTools.NeutralTextColor;
                    var CellBackgroundColor = ColorTools.BackgroundColor;
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
                        TextWriterWhereColor.WriteWhere(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, ConsoleBase.ConsoleWrapper.CursorTop, false, ColTypes);
                    }
                }
                ConsoleBase.ConsoleWrapper.WriteLine();

                // Separate the rows optionally
                if (SeparateRows)
                {
                    // Write the closing minus sign.
                    OldTop = ConsoleBase.ConsoleWrapper.CursorTop;
                    RepeatTimes = ConsoleBase.ConsoleWrapper.WindowWidth - ConsoleBase.ConsoleWrapper.CursorLeft - Margin * 2;
                    if (Margin > 0)
                        TextWriterColor.Write(" ".Repeat(Margin), false, ColTypes);
                    TextWriterColor.Write("-".Repeat(RepeatTimes), true, ColTypes);

                    // Fix CursorTop value on Unix systems.
                    if (KernelPlatform.IsOnUnix())
                    {
                        if (!(ConsoleBase.ConsoleWrapper.CursorTop == ConsoleBase.ConsoleWrapper.WindowHeight - 1) | OldTop == ConsoleBase.ConsoleWrapper.WindowHeight - 3)
                            ConsoleBase.ConsoleWrapper.CursorTop -= 1;
                    }
                }
            }
        }

        /// <summary>
        /// Draw a table with text
        /// </summary>
        /// <param name="Headers">Headers to insert to the table.</param>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="Margin">Margin offset</param>
        /// <param name="SeparateRows">Separate the rows?</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        public static void WriteTable(string[] Headers, string[,] Rows, int Margin, ColorTools.ColTypes colorTypeForeground, ColorTools.ColTypes colorTypeBackground, bool SeparateRows = true, List<CellOptions> CellOptions = null)
        {
            int ColumnCapacity = (int)Math.Round(ConsoleBase.ConsoleWrapper.WindowWidth / (double)Headers.Length);
            var ColumnPositions = new List<int>();
            int RepeatTimes;

            // Populate the positions
            ConsoleBase.ConsoleWrapper.WriteLine();
            for (int ColumnPosition = Margin, loopTo = ConsoleBase.ConsoleWrapper.WindowWidth; ColumnCapacity >= 0 ? ColumnPosition <= loopTo : ColumnPosition >= loopTo; ColumnPosition += ColumnCapacity)
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
            for (int HeaderIndex = 0, loopTo1 = Headers.Length - 1; HeaderIndex <= loopTo1; HeaderIndex++)
            {
                string Header = Headers[HeaderIndex];
                int ColumnPosition = ColumnPositions[HeaderIndex];
                Header ??= "";
                TextWriterWhereColor.WriteWhere(Header.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, ConsoleBase.ConsoleWrapper.CursorTop, false, colorTypeForeground, colorTypeBackground);
            }
            ConsoleBase.ConsoleWrapper.WriteLine();

            // Write the closing minus sign.
            int OldTop = ConsoleBase.ConsoleWrapper.CursorTop;
            RepeatTimes = ConsoleBase.ConsoleWrapper.WindowWidth - ConsoleBase.ConsoleWrapper.CursorLeft - Margin * 2;
            if (Margin > 0)
                TextWriterColor.Write(" ".Repeat(Margin), false, colorTypeForeground, colorTypeBackground);
            TextWriterColor.Write("-".Repeat(RepeatTimes), true, colorTypeForeground, colorTypeBackground);

            // Fix CursorTop value on Unix systems.
            if (KernelPlatform.IsOnUnix())
            {
                if (!(ConsoleBase.ConsoleWrapper.CursorTop == ConsoleBase.ConsoleWrapper.WindowHeight - 1) | OldTop == ConsoleBase.ConsoleWrapper.WindowHeight - 3)
                    ConsoleBase.ConsoleWrapper.CursorTop -= 1;
            }

            // Write the rows
            for (int RowIndex = 0, loopTo2 = Rows.GetLength(0) - 1; RowIndex <= loopTo2; RowIndex++)
            {
                for (int RowValueIndex = 0, loopTo3 = Rows.GetLength(1) - 1; RowValueIndex <= loopTo3; RowValueIndex++)
                {
                    var ColoredCell = false;
                    var CellColor = ColorTools.NeutralTextColor;
                    var CellBackgroundColor = ColorTools.BackgroundColor;
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
                        TextWriterWhereColor.WriteWhere(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, ConsoleBase.ConsoleWrapper.CursorTop, false, colorTypeForeground, colorTypeBackground);
                    }
                }
                ConsoleBase.ConsoleWrapper.WriteLine();

                // Separate the rows optionally
                if (SeparateRows)
                {
                    // Write the closing minus sign.
                    OldTop = ConsoleBase.ConsoleWrapper.CursorTop;
                    RepeatTimes = ConsoleBase.ConsoleWrapper.WindowWidth - ConsoleBase.ConsoleWrapper.CursorLeft - Margin * 2;
                    if (Margin > 0)
                        TextWriterColor.Write(" ".Repeat(Margin), false, colorTypeForeground, colorTypeBackground);
                    TextWriterColor.Write("-".Repeat(RepeatTimes), true, colorTypeForeground, colorTypeBackground);

                    // Fix CursorTop value on Unix systems.
                    if (KernelPlatform.IsOnUnix())
                    {
                        if (!(ConsoleBase.ConsoleWrapper.CursorTop == ConsoleBase.ConsoleWrapper.WindowHeight - 1) | OldTop == ConsoleBase.ConsoleWrapper.WindowHeight - 3)
                            ConsoleBase.ConsoleWrapper.CursorTop -= 1;
                    }
                }
            }
        }

        /// <summary>
        /// Draw a table with text
        /// </summary>
        /// <param name="Headers">Headers to insert to the table.</param>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="Margin">Margin offset</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="SeparateRows">Separate the rows?</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        public static void WriteTable(string[] Headers, string[,] Rows, int Margin, Color Color, bool SeparateRows = true, List<CellOptions> CellOptions = null)
        {
            int ColumnCapacity = (int)Math.Round(ConsoleBase.ConsoleWrapper.WindowWidth / (double)Headers.Length);
            var ColumnPositions = new List<int>();
            int RepeatTimes;

            // Populate the positions
            ConsoleBase.ConsoleWrapper.WriteLine();
            for (int ColumnPosition = Margin, loopTo = ConsoleBase.ConsoleWrapper.WindowWidth; ColumnCapacity >= 0 ? ColumnPosition <= loopTo : ColumnPosition >= loopTo; ColumnPosition += ColumnCapacity)
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
            for (int HeaderIndex = 0, loopTo1 = Headers.Length - 1; HeaderIndex <= loopTo1; HeaderIndex++)
            {
                string Header = Headers[HeaderIndex];
                int ColumnPosition = ColumnPositions[HeaderIndex];
                Header ??= "";
                TextWriterWhereColor.WriteWhere(Header.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, ConsoleBase.ConsoleWrapper.CursorTop, false, Color);
            }
            ConsoleBase.ConsoleWrapper.WriteLine();

            // Write the closing minus sign.
            int OldTop = ConsoleBase.ConsoleWrapper.CursorTop;
            RepeatTimes = ConsoleBase.ConsoleWrapper.WindowWidth - ConsoleBase.ConsoleWrapper.CursorLeft - Margin * 2;
            if (Margin > 0)
                TextWriterColor.Write(" ".Repeat(Margin), false, Color);
            TextWriterColor.Write("-".Repeat(RepeatTimes), true, Color);

            // Fix CursorTop value on Unix systems.
            if (KernelPlatform.IsOnUnix())
            {
                if (!(ConsoleBase.ConsoleWrapper.CursorTop == ConsoleBase.ConsoleWrapper.WindowHeight - 1) | OldTop == ConsoleBase.ConsoleWrapper.WindowHeight - 3)
                    ConsoleBase.ConsoleWrapper.CursorTop -= 1;
            }

            // Write the rows
            for (int RowIndex = 0, loopTo2 = Rows.GetLength(0) - 1; RowIndex <= loopTo2; RowIndex++)
            {
                for (int RowValueIndex = 0, loopTo3 = Rows.GetLength(1) - 1; RowValueIndex <= loopTo3; RowValueIndex++)
                {
                    var ColoredCell = false;
                    var CellColor = ColorTools.NeutralTextColor;
                    var CellBackgroundColor = ColorTools.BackgroundColor;
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
                        TextWriterWhereColor.WriteWhere(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, ConsoleBase.ConsoleWrapper.CursorTop, false, Color);
                    }
                }
                ConsoleBase.ConsoleWrapper.WriteLine();

                // Separate the rows optionally
                if (SeparateRows)
                {
                    // Write the closing minus sign.
                    OldTop = ConsoleBase.ConsoleWrapper.CursorTop;
                    RepeatTimes = ConsoleBase.ConsoleWrapper.WindowWidth - ConsoleBase.ConsoleWrapper.CursorLeft - Margin * 2;
                    if (Margin > 0)
                        TextWriterColor.Write(" ".Repeat(Margin), false, Color);
                    TextWriterColor.Write("-".Repeat(RepeatTimes), true, Color);

                    // Fix CursorTop value on Unix systems.
                    if (KernelPlatform.IsOnUnix())
                    {
                        if (!(ConsoleBase.ConsoleWrapper.CursorTop == ConsoleBase.ConsoleWrapper.WindowHeight - 1) | OldTop == ConsoleBase.ConsoleWrapper.WindowHeight - 3)
                            ConsoleBase.ConsoleWrapper.CursorTop -= 1;
                    }
                }
            }
        }

        /// <summary>
        /// Draw a table with text
        /// </summary>
        /// <param name="Headers">Headers to insert to the table.</param>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="Margin">Margin offset</param>
        /// <param name="SeparateRows">Separate the rows?</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        public static void WriteTable(string[] Headers, string[,] Rows, int Margin, Color ForegroundColor, Color BackgroundColor, bool SeparateRows = true, List<CellOptions> CellOptions = null)
        {
            int ColumnCapacity = (int)Math.Round(ConsoleBase.ConsoleWrapper.WindowWidth / (double)Headers.Length);
            var ColumnPositions = new List<int>();
            int RepeatTimes;

            // Populate the positions
            ConsoleBase.ConsoleWrapper.WriteLine();
            for (int ColumnPosition = Margin, loopTo = ConsoleBase.ConsoleWrapper.WindowWidth; ColumnCapacity >= 0 ? ColumnPosition <= loopTo : ColumnPosition >= loopTo; ColumnPosition += ColumnCapacity)
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
            for (int HeaderIndex = 0, loopTo1 = Headers.Length - 1; HeaderIndex <= loopTo1; HeaderIndex++)
            {
                string Header = Headers[HeaderIndex];
                int ColumnPosition = ColumnPositions[HeaderIndex];
                Header ??= "";
                TextWriterWhereColor.WriteWhere(Header.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, ConsoleBase.ConsoleWrapper.CursorTop, false, ForegroundColor, BackgroundColor);
            }
            ConsoleBase.ConsoleWrapper.WriteLine();

            // Write the closing minus sign.
            int OldTop = ConsoleBase.ConsoleWrapper.CursorTop;
            RepeatTimes = ConsoleBase.ConsoleWrapper.WindowWidth - ConsoleBase.ConsoleWrapper.CursorLeft - Margin * 2;
            if (Margin > 0)
                TextWriterColor.Write(" ".Repeat(Margin), false, ForegroundColor, BackgroundColor);
            TextWriterColor.Write("-".Repeat(RepeatTimes), true, ForegroundColor, BackgroundColor);

            // Fix CursorTop value on Unix systems.
            if (KernelPlatform.IsOnUnix())
            {
                if (!(ConsoleBase.ConsoleWrapper.CursorTop == ConsoleBase.ConsoleWrapper.WindowHeight - 1) | OldTop == ConsoleBase.ConsoleWrapper.WindowHeight - 3)
                    ConsoleBase.ConsoleWrapper.CursorTop -= 1;
            }

            // Write the rows
            for (int RowIndex = 0, loopTo2 = Rows.GetLength(0) - 1; RowIndex <= loopTo2; RowIndex++)
            {
                for (int RowValueIndex = 0, loopTo3 = Rows.GetLength(1) - 1; RowValueIndex <= loopTo3; RowValueIndex++)
                {
                    var ColoredCell = false;
                    var CellColor = ColorTools.NeutralTextColor;
                    var CellBackgroundColor = ColorTools.BackgroundColor;
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
                        TextWriterWhereColor.WriteWhere(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, ConsoleBase.ConsoleWrapper.CursorTop, false, ForegroundColor, BackgroundColor);
                    }
                }
                ConsoleBase.ConsoleWrapper.WriteLine();

                // Separate the rows optionally
                if (SeparateRows)
                {
                    // Write the closing minus sign.
                    OldTop = ConsoleBase.ConsoleWrapper.CursorTop;
                    RepeatTimes = ConsoleBase.ConsoleWrapper.WindowWidth - ConsoleBase.ConsoleWrapper.CursorLeft - Margin * 2;
                    if (Margin > 0)
                        TextWriterColor.Write(" ".Repeat(Margin), false, ForegroundColor, BackgroundColor);
                    TextWriterColor.Write("-".Repeat(RepeatTimes), true, ForegroundColor, BackgroundColor);

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
