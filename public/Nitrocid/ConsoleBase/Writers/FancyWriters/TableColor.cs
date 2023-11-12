//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters.Tools;
using KS.Drivers;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Text;
using Terminaux.Colors;

namespace KS.ConsoleBase.Writers.FancyWriters
{
    /// <summary>
    /// Table writer with color support
    /// </summary>
    public static class TableColor
    {

        // TODO: Consider re-writing
        /// <summary>
        /// Draw a table with text
        /// </summary>
        /// <param name="Headers">Headers to insert to the table.</param>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="Margin">Safe threshold from left</param>
        /// <param name="SeparateRows">Separate the rows?</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        public static void WriteTablePlain(string[] Headers, string[,] Rows, int Margin, bool SeparateRows = true, List<CellOptions> CellOptions = null)
        {
            try
            {
                TextWriterColor.WritePlain(RenderTablePlain(Headers, Rows, Margin, SeparateRows, CellOptions));
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

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
            try
            {
                var sep = KernelColorTools.GetColor(KernelColorType.TableSeparator);
                var header = KernelColorTools.GetColor(KernelColorType.TableHeader);
                var value = KernelColorTools.GetColor(KernelColorType.TableValue);
                var back = KernelColorTools.GetColor(KernelColorType.Background);
                TextWriterColor.WritePlain(RenderTable(Headers, Rows, Margin, sep, header, value, back, SeparateRows, CellOptions));
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
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
        /// <param name="colorTypeSeparatorForeground">A type of colors that will be changed for the separator foreground color.</param>
        /// <param name="colorTypeHeaderForeground">A type of colors that will be changed for the header foreground color.</param>
        /// <param name="colorTypeValueForeground">A type of colors that will be changed for the value foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        public static void WriteTable(string[] Headers, string[,] Rows, int Margin, KernelColorType colorTypeSeparatorForeground, KernelColorType colorTypeHeaderForeground, KernelColorType colorTypeValueForeground, KernelColorType colorTypeBackground, bool SeparateRows = true, List<CellOptions> CellOptions = null) =>
            WriteTable(Headers, Rows, Margin, KernelColorTools.GetColor(colorTypeSeparatorForeground), KernelColorTools.GetColor(colorTypeHeaderForeground), KernelColorTools.GetColor(colorTypeValueForeground), KernelColorTools.GetColor(colorTypeBackground), SeparateRows, CellOptions);

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
            WriteTable(Headers, Rows, Margin, new Color(SeparatorForegroundColor), new Color(HeaderForegroundColor), new Color(ValueForegroundColor), new Color(BackgroundColor), SeparateRows, CellOptions);

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
            try
            {
                TextWriterColor.WritePlain(RenderTable(Headers, Rows, Margin, SeparatorForegroundColor, HeaderForegroundColor, ValueForegroundColor, BackgroundColor, SeparateRows, CellOptions));
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Renders a table with text
        /// </summary>
        /// <param name="Headers">Headers to insert to the table.</param>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="Margin">Margin offset</param>
        /// <param name="SeparateRows">Separate the rows?</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        public static string RenderTablePlain(string[] Headers, string[,] Rows, int Margin, bool SeparateRows = true, List<CellOptions> CellOptions = null)
        {
            try
            {
                var table = new StringBuilder();
                int ColumnCapacity = (int)Math.Round(ConsoleWrapper.WindowWidth / (double)Headers.Length);
                var ColumnPositions = new List<int>();
                int RepeatTimes;
                int line = 1;

                // Populate the positions
                table.AppendLine();
                for (int ColumnPosition = Margin; ColumnCapacity >= 0 ? ColumnPosition <= ConsoleWrapper.WindowWidth : ColumnPosition >= ConsoleWrapper.WindowWidth; ColumnPosition += ColumnCapacity)
                {
                    if (ColumnPosition < ConsoleWrapper.WindowWidth)
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
                    table.Append(
                        DriverHandler.CurrentConsoleDriverLocal.RenderWherePlain(Header.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, ConsoleWrapper.CursorTop + line, false)
                    );
                }
                table.AppendLine();
                line++;

                // Write the closing minus sign.
                RepeatTimes = ConsoleWrapper.WindowWidth - Margin * 2;
                if (Margin > 0)
                    table.Append(new string(' ', Margin));
                table.AppendLine(new string('═', RepeatTimes));
                line++;

                // Write the rows
                for (int RowIndex = 0; RowIndex <= Rows.GetLength(0) - 1; RowIndex++)
                {
                    for (int RowValueIndex = 0; RowValueIndex <= Rows.GetLength(1) - 1; RowValueIndex++)
                    {
                        var ColoredCell = false;
                        var CellColor = KernelColorTools.GetColor(KernelColorType.NeutralText);
                        var CellBackgroundColor = KernelColorTools.GetColor(KernelColorType.Background);
                        string RowValue = Rows[RowIndex, RowValueIndex];
                        int ColumnPosition = ColumnPositions[RowValueIndex];
                        RowValue ??= "";

                        // Now, write the cell value
                        string FinalRowValue = RowValue.Truncate(ColumnCapacity - 3 - Margin);
                        if (ColoredCell)
                            table.Append(
                                DriverHandler.CurrentConsoleDriverLocal.RenderWherePlain(FinalRowValue, ColumnPosition, ConsoleWrapper.CursorTop + line, false)
                            );
                        else
                            table.Append(
                                DriverHandler.CurrentConsoleDriverLocal.RenderWherePlain(FinalRowValue, ColumnPosition, ConsoleWrapper.CursorTop + line, false)
                            );
                    }
                    table.AppendLine();
                    line++;

                    // Separate the rows optionally
                    if (SeparateRows)
                    {
                        // Write the closing minus sign.
                        RepeatTimes = ConsoleWrapper.WindowWidth - Margin * 2;
                        if (Margin > 0)
                            table.Append(new string(' ', Margin));
                        table.AppendLine(new string('═', RepeatTimes));
                        line++;
                    }
                }
                return table.ToString();
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
            return "";
        }

        /// <summary>
        /// Renders a table with text
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
        public static string RenderTable(string[] Headers, string[,] Rows, int Margin, Color SeparatorForegroundColor, Color HeaderForegroundColor, Color ValueForegroundColor, Color BackgroundColor, bool SeparateRows = true, List<CellOptions> CellOptions = null)
        {
            try
            {
                var table = new StringBuilder();
                int ColumnCapacity = (int)Math.Round(ConsoleWrapper.WindowWidth / (double)Headers.Length);
                var ColumnPositions = new List<int>();
                int RepeatTimes;
                int line = 1;

                // Populate the positions
                table.AppendLine();
                for (int ColumnPosition = Margin; ColumnCapacity >= 0 ? ColumnPosition <= ConsoleWrapper.WindowWidth : ColumnPosition >= ConsoleWrapper.WindowWidth; ColumnPosition += ColumnCapacity)
                {
                    if (ColumnPosition < ConsoleWrapper.WindowWidth)
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
                    table.Append(
                        HeaderForegroundColor.VTSequenceForeground +
                        BackgroundColor.VTSequenceBackground +
                        DriverHandler.CurrentConsoleDriverLocal.RenderWherePlain(Header.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, ConsoleWrapper.CursorTop + line, false)
                    );
                }
                table.AppendLine();
                line++;

                // Write the closing minus sign.
                RepeatTimes = ConsoleWrapper.WindowWidth - Margin * 2;
                table.Append(
                    SeparatorForegroundColor.VTSequenceForeground +
                    BackgroundColor.VTSequenceBackground
                );
                if (Margin > 0)
                    table.Append(new string(' ', Margin));
                table.AppendLine(new string('═', RepeatTimes));
                line++;

                // Write the rows
                for (int RowIndex = 0; RowIndex <= Rows.GetLength(0) - 1; RowIndex++)
                {
                    for (int RowValueIndex = 0; RowValueIndex <= Rows.GetLength(1) - 1; RowValueIndex++)
                    {
                        var ColoredCell = false;
                        var CellColor = KernelColorTools.GetColor(KernelColorType.NeutralText);
                        var CellBackgroundColor = KernelColorTools.GetColor(KernelColorType.Background);
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
                        string FinalRowValue = RowValue.Truncate(ColumnCapacity - 3 - Margin);
                        if (ColoredCell)
                            table.Append(
                                CellColor.VTSequenceForeground +
                                CellBackgroundColor.VTSequenceBackground +
                                DriverHandler.CurrentConsoleDriverLocal.RenderWherePlain(FinalRowValue, ColumnPosition, ConsoleWrapper.CursorTop + line, false)
                            );
                        else
                            table.Append(
                                ValueForegroundColor.VTSequenceForeground +
                                BackgroundColor.VTSequenceBackground +
                                DriverHandler.CurrentConsoleDriverLocal.RenderWherePlain(FinalRowValue, ColumnPosition, ConsoleWrapper.CursorTop + line, false)
                            );
                    }
                    table.AppendLine();
                    line++;

                    // Separate the rows optionally
                    if (SeparateRows)
                    {
                        // Write the closing minus sign.
                        RepeatTimes = ConsoleWrapper.WindowWidth - Margin * 2;
                        if (Margin > 0)
                            table.Append(new string(' ', Margin));
                        table.AppendLine(new string('═', RepeatTimes));
                        line++;
                    }
                }
                return table.ToString();
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
            return "";
        }

    }
}
