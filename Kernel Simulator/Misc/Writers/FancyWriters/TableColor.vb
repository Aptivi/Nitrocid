
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
'
'    This file is part of Kernel Simulator
'
'    Kernel Simulator is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    Kernel Simulator is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <https://www.gnu.org/licenses/>.

Imports KS.Misc.Writers.FancyWriters.Tools

Namespace Misc.Writers.FancyWriters
    Public Module TableColor

        ''' <summary>
        ''' Draw a table with text
        ''' </summary>
        ''' <param name="Headers">Headers to insert to the table.</param>
        ''' <param name="Rows">Rows to insert to the table.</param>
        ''' <param name="Margin">Safe threshold from left</param>
        Public Sub WriteTable(Headers() As String, Rows(,) As String, Margin As Integer, Optional SeparateRows As Boolean = True, Optional CellOptions As List(Of CellOptions) = Nothing)
            Dim ColumnCapacity As Integer = Console.WindowWidth / Headers.Length
            Dim ColumnPositions As New List(Of Integer)
            Dim RepeatTimes As Integer

            'Populate the positions
            Console.WriteLine()
            For ColumnPosition As Integer = Margin To Console.WindowWidth Step ColumnCapacity
                If Not ColumnPosition >= Console.WindowWidth Then
                    ColumnPositions.Add(ColumnPosition)
                    If ColumnPositions.Count = 1 Then ColumnPosition = 0
                Else
                    Exit For
                End If
            Next

            'Write the headers
            For HeaderIndex As Integer = 0 To Headers.Length - 1
                Dim Header As String = Headers(HeaderIndex)
                Dim ColumnPosition As Integer = ColumnPositions(HeaderIndex)
                If Header Is Nothing Then Header = ""
                WriteWhere(Header.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, ColTypes.TableHeader)
            Next
            Console.WriteLine()

            'Write the closing minus sign.
            Dim OldTop As Integer = Console.CursorTop
            RepeatTimes = Console.WindowWidth - Console.CursorLeft - (Margin * 2)
            If Margin > 0 Then Write(" ".Repeat(Margin), False, ColTypes.Neutral)
            Write("-".Repeat(RepeatTimes), True, ColTypes.TableSeparator)

            'Fix CursorTop value on Unix systems.
            If IsOnUnix() Then
                If Not Console.CursorTop = Console.WindowHeight - 1 Or OldTop = Console.WindowHeight - 3 Then Console.CursorTop -= 1
            End If

            'Write the rows
            For RowIndex As Integer = 0 To Rows.GetLength(0) - 1
                For RowValueIndex As Integer = 0 To Rows.GetLength(1) - 1
                    Dim ColoredCell As Boolean
                    Dim CellColor As Color = NeutralTextColor
                    Dim CellBackgroundColor As Color = BackgroundColor
                    Dim RowValue As String = Rows(RowIndex, RowValueIndex)
                    Dim ColumnPosition As Integer = ColumnPositions(RowValueIndex)
                    If RowValue Is Nothing Then RowValue = ""

                    'Get the cell options and set them as necessary
                    If CellOptions IsNot Nothing Then
                        For Each CellOption As CellOptions In CellOptions
                            If CellOption.ColumnIndex = RowValueIndex And CellOption.RowIndex = RowIndex Then
                                ColoredCell = CellOption.ColoredCell
                                CellColor = CellOption.CellColor
                                CellBackgroundColor = CellOption.CellBackgroundColor
                            End If
                        Next
                    End If

                    'Now, write the cell value
                    If ColoredCell Then
                        WriteWhere(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, CellColor, CellBackgroundColor)
                    Else
                        WriteWhere(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, ColTypes.TableValue)
                    End If
                Next
                Console.WriteLine()

                'Separate the rows optionally
                If SeparateRows Then
                    'Write the closing minus sign.
                    OldTop = Console.CursorTop
                    RepeatTimes = Console.WindowWidth - Console.CursorLeft - (Margin * 2)
                    If Margin > 0 Then Write(" ".Repeat(Margin), False, ColTypes.Neutral)
                    Write("-".Repeat(RepeatTimes), True, ColTypes.TableSeparator)

                    'Fix CursorTop value on Unix systems.
                    If IsOnUnix() Then
                        If Not Console.CursorTop = Console.WindowHeight - 1 Or OldTop = Console.WindowHeight - 3 Then Console.CursorTop -= 1
                    End If
                End If
            Next
        End Sub

        ''' <summary>
        ''' Draw a table with text
        ''' </summary>
        ''' <param name="Color">A color that will be changed to.</param>
        Public Sub WriteTable(Headers() As String, Rows(,) As String, Margin As Integer, Color As ConsoleColor, Optional SeparateRows As Boolean = True, Optional CellOptions As List(Of CellOptions) = Nothing)
            Dim ColumnCapacity As Integer = Console.WindowWidth / Headers.Length
            Dim ColumnPositions As New List(Of Integer)
            Dim RepeatTimes As Integer

            'Populate the positions
            Console.WriteLine()
            For ColumnPosition As Integer = Margin To Console.WindowWidth Step ColumnCapacity
                If Not ColumnPosition >= Console.WindowWidth Then
                    ColumnPositions.Add(ColumnPosition)
                    If ColumnPositions.Count = 1 Then ColumnPosition = 0
                Else
                    Exit For
                End If
            Next

            'Write the headers
            For HeaderIndex As Integer = 0 To Headers.Length - 1
                Dim Header As String = Headers(HeaderIndex)
                Dim ColumnPosition As Integer = ColumnPositions(HeaderIndex)
                If Header Is Nothing Then Header = ""
                WriteWhere(Header.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, Color)
            Next
            Console.WriteLine()

            'Write the closing minus sign.
            Dim OldTop As Integer = Console.CursorTop
            RepeatTimes = Console.WindowWidth - Console.CursorLeft - (Margin * 2)
            If Margin > 0 Then Write(" ".Repeat(Margin), False, Color)
            Write("-".Repeat(RepeatTimes), True, Color)

            'Fix CursorTop value on Unix systems.
            If IsOnUnix() Then
                If Not Console.CursorTop = Console.WindowHeight - 1 Or OldTop = Console.WindowHeight - 3 Then Console.CursorTop -= 1
            End If

            'Write the rows
            For RowIndex As Integer = 0 To Rows.GetLength(0) - 1
                For RowValueIndex As Integer = 0 To Rows.GetLength(1) - 1
                    Dim ColoredCell As Boolean
                    Dim CellColor As Color = NeutralTextColor
                    Dim CellBackgroundColor As Color = BackgroundColor
                    Dim RowValue As String = Rows(RowIndex, RowValueIndex)
                    Dim ColumnPosition As Integer = ColumnPositions(RowValueIndex)
                    If RowValue Is Nothing Then RowValue = ""

                    'Get the cell options and set them as necessary
                    If CellOptions IsNot Nothing Then
                        For Each CellOption As CellOptions In CellOptions
                            If CellOption.ColumnIndex = RowValueIndex And CellOption.RowIndex = RowIndex Then
                                ColoredCell = CellOption.ColoredCell
                                CellColor = CellOption.CellColor
                                CellBackgroundColor = CellOption.CellBackgroundColor
                            End If
                        Next
                    End If

                    'Now, write the cell value
                    If ColoredCell Then
                        WriteWhere(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, CellColor, CellBackgroundColor)
                    Else
                        WriteWhere(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, Color)
                    End If
                Next
                Console.WriteLine()

                'Separate the rows optionally
                If SeparateRows Then
                    'Write the closing minus sign.
                    OldTop = Console.CursorTop
                    RepeatTimes = Console.WindowWidth - Console.CursorLeft - (Margin * 2)
                    If Margin > 0 Then Write(" ".Repeat(Margin), False, Color)
                    Write("-".Repeat(RepeatTimes), True, Color)

                    'Fix CursorTop value on Unix systems.
                    If IsOnUnix() Then
                        If Not Console.CursorTop = Console.WindowHeight - 1 Or OldTop = Console.WindowHeight - 3 Then Console.CursorTop -= 1
                    End If
                End If
            Next
        End Sub

        ''' <summary>
        ''' Draw a table with text
        ''' </summary>
        ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
        ''' <param name="BackgroundColor">A background color that will be changed to.</param>
        Public Sub WriteTable(Headers() As String, Rows(,) As String, Margin As Integer, ForegroundColor As ConsoleColor, BackgroundColor As ConsoleColor, Optional SeparateRows As Boolean = True, Optional CellOptions As List(Of CellOptions) = Nothing)
            Dim ColumnCapacity As Integer = Console.WindowWidth / Headers.Length
            Dim ColumnPositions As New List(Of Integer)
            Dim RepeatTimes As Integer

            'Populate the positions
            Console.WriteLine()
            For ColumnPosition As Integer = Margin To Console.WindowWidth Step ColumnCapacity
                If Not ColumnPosition >= Console.WindowWidth Then
                    ColumnPositions.Add(ColumnPosition)
                    If ColumnPositions.Count = 1 Then ColumnPosition = 0
                Else
                    Exit For
                End If
            Next

            'Write the headers
            For HeaderIndex As Integer = 0 To Headers.Length - 1
                Dim Header As String = Headers(HeaderIndex)
                Dim ColumnPosition As Integer = ColumnPositions(HeaderIndex)
                If Header Is Nothing Then Header = ""
                WriteWhere(Header.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, ForegroundColor, BackgroundColor)
            Next
            Console.WriteLine()

            'Write the closing minus sign.
            Dim OldTop As Integer = Console.CursorTop
            RepeatTimes = Console.WindowWidth - Console.CursorLeft - (Margin * 2)
            If Margin > 0 Then Write(" ".Repeat(Margin), False, ForegroundColor, BackgroundColor)
            Write("-".Repeat(RepeatTimes), True, ForegroundColor, BackgroundColor)

            'Fix CursorTop value on Unix systems.
            If IsOnUnix() Then
                If Not Console.CursorTop = Console.WindowHeight - 1 Or OldTop = Console.WindowHeight - 3 Then Console.CursorTop -= 1
            End If

            'Write the rows
            For RowIndex As Integer = 0 To Rows.GetLength(0) - 1
                For RowValueIndex As Integer = 0 To Rows.GetLength(1) - 1
                    Dim ColoredCell As Boolean
                    Dim CellColor As Color = NeutralTextColor
                    Dim CellBackgroundColor As Color = ColorTools.BackgroundColor
                    Dim RowValue As String = Rows(RowIndex, RowValueIndex)
                    Dim ColumnPosition As Integer = ColumnPositions(RowValueIndex)
                    If RowValue Is Nothing Then RowValue = ""

                    'Get the cell options and set them as necessary
                    If CellOptions IsNot Nothing Then
                        For Each CellOption As CellOptions In CellOptions
                            If CellOption.ColumnIndex = RowValueIndex And CellOption.RowIndex = RowIndex Then
                                ColoredCell = CellOption.ColoredCell
                                CellColor = CellOption.CellColor
                                CellBackgroundColor = CellOption.CellBackgroundColor
                            End If
                        Next
                    End If

                    'Now, write the cell value
                    If ColoredCell Then
                        WriteWhere(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, CellColor, CellBackgroundColor)
                    Else
                        WriteWhere(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, ForegroundColor, BackgroundColor)
                    End If
                Next
                Console.WriteLine()

                'Separate the rows optionally
                If SeparateRows Then
                    'Write the closing minus sign.
                    OldTop = Console.CursorTop
                    RepeatTimes = Console.WindowWidth - Console.CursorLeft - (Margin * 2)
                    If Margin > 0 Then Write(" ".Repeat(Margin), False, ForegroundColor, BackgroundColor)
                    Write("-".Repeat(RepeatTimes), True, ForegroundColor, BackgroundColor)

                    'Fix CursorTop value on Unix systems.
                    If IsOnUnix() Then
                        If Not Console.CursorTop = Console.WindowHeight - 1 Or OldTop = Console.WindowHeight - 3 Then Console.CursorTop -= 1
                    End If
                End If
            Next
        End Sub

        ''' <summary>
        ''' Draw a table with text
        ''' </summary>
        ''' <param name="Headers">Headers to insert to the table.</param>
        ''' <param name="Rows">Rows to insert to the table.</param>
        ''' <param name="Margin">Safe threshold from left</param>
        ''' <param name="ColTypes">A type of colors that will be changed.</param>
        Public Sub WriteTable(Headers() As String, Rows(,) As String, Margin As Integer, ColTypes As ColTypes, Optional SeparateRows As Boolean = True, Optional CellOptions As List(Of CellOptions) = Nothing)
            Dim ColumnCapacity As Integer = Console.WindowWidth / Headers.Length
            Dim ColumnPositions As New List(Of Integer)
            Dim RepeatTimes As Integer

            'Populate the positions
            Console.WriteLine()
            For ColumnPosition As Integer = Margin To Console.WindowWidth Step ColumnCapacity
                If Not ColumnPosition >= Console.WindowWidth Then
                    ColumnPositions.Add(ColumnPosition)
                    If ColumnPositions.Count = 1 Then ColumnPosition = 0
                Else
                    Exit For
                End If
            Next

            'Write the headers
            For HeaderIndex As Integer = 0 To Headers.Length - 1
                Dim Header As String = Headers(HeaderIndex)
                Dim ColumnPosition As Integer = ColumnPositions(HeaderIndex)
                If Header Is Nothing Then Header = ""
                WriteWhere(Header.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, ColTypes)
            Next
            Console.WriteLine()

            'Write the closing minus sign.
            Dim OldTop As Integer = Console.CursorTop
            RepeatTimes = Console.WindowWidth - Console.CursorLeft - (Margin * 2)
            If Margin > 0 Then Write(" ".Repeat(Margin), False, ColTypes)
            Write("-".Repeat(RepeatTimes), True, ColTypes)

            'Fix CursorTop value on Unix systems.
            If IsOnUnix() Then
                If Not Console.CursorTop = Console.WindowHeight - 1 Or OldTop = Console.WindowHeight - 3 Then Console.CursorTop -= 1
            End If

            'Write the rows
            For RowIndex As Integer = 0 To Rows.GetLength(0) - 1
                For RowValueIndex As Integer = 0 To Rows.GetLength(1) - 1
                    Dim ColoredCell As Boolean
                    Dim CellColor As Color = NeutralTextColor
                    Dim CellBackgroundColor As Color = BackgroundColor
                    Dim RowValue As String = Rows(RowIndex, RowValueIndex)
                    Dim ColumnPosition As Integer = ColumnPositions(RowValueIndex)
                    If RowValue Is Nothing Then RowValue = ""

                    'Get the cell options and set them as necessary
                    If CellOptions IsNot Nothing Then
                        For Each CellOption As CellOptions In CellOptions
                            If CellOption.ColumnIndex = RowValueIndex And CellOption.RowIndex = RowIndex Then
                                ColoredCell = CellOption.ColoredCell
                                CellColor = CellOption.CellColor
                                CellBackgroundColor = CellOption.CellBackgroundColor
                            End If
                        Next
                    End If

                    'Now, write the cell value
                    If ColoredCell Then
                        WriteWhere(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, CellColor, CellBackgroundColor)
                    Else
                        WriteWhere(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, ColTypes)
                    End If
                Next
                Console.WriteLine()

                'Separate the rows optionally
                If SeparateRows Then
                    'Write the closing minus sign.
                    OldTop = Console.CursorTop
                    RepeatTimes = Console.WindowWidth - Console.CursorLeft - (Margin * 2)
                    If Margin > 0 Then Write(" ".Repeat(Margin), False, ColTypes)
                    Write("-".Repeat(RepeatTimes), True, ColTypes)

                    'Fix CursorTop value on Unix systems.
                    If IsOnUnix() Then
                        If Not Console.CursorTop = Console.WindowHeight - 1 Or OldTop = Console.WindowHeight - 3 Then Console.CursorTop -= 1
                    End If
                End If
            Next
        End Sub

        ''' <summary>
        ''' Draw a table with text
        ''' </summary>
        ''' <param name="Headers">Headers to insert to the table.</param>
        ''' <param name="Rows">Rows to insert to the table.</param>
        ''' <param name="Margin">Safe threshold from left</param>
        ''' <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        ''' <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        Public Sub WriteTable(Headers() As String, Rows(,) As String, Margin As Integer, colorTypeForeground As ColTypes, colorTypeBackground As ColTypes, Optional SeparateRows As Boolean = True, Optional CellOptions As List(Of CellOptions) = Nothing)
            Dim ColumnCapacity As Integer = Console.WindowWidth / Headers.Length
            Dim ColumnPositions As New List(Of Integer)
            Dim RepeatTimes As Integer

            'Populate the positions
            Console.WriteLine()
            For ColumnPosition As Integer = Margin To Console.WindowWidth Step ColumnCapacity
                If Not ColumnPosition >= Console.WindowWidth Then
                    ColumnPositions.Add(ColumnPosition)
                    If ColumnPositions.Count = 1 Then ColumnPosition = 0
                Else
                    Exit For
                End If
            Next

            'Write the headers
            For HeaderIndex As Integer = 0 To Headers.Length - 1
                Dim Header As String = Headers(HeaderIndex)
                Dim ColumnPosition As Integer = ColumnPositions(HeaderIndex)
                If Header Is Nothing Then Header = ""
                WriteWhere(Header.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, colorTypeForeground, colorTypeBackground)
            Next
            Console.WriteLine()

            'Write the closing minus sign.
            Dim OldTop As Integer = Console.CursorTop
            RepeatTimes = Console.WindowWidth - Console.CursorLeft - (Margin * 2)
            If Margin > 0 Then Write(" ".Repeat(Margin), False, colorTypeForeground, colorTypeBackground)
            Write("-".Repeat(RepeatTimes), True, colorTypeForeground, colorTypeBackground)

            'Fix CursorTop value on Unix systems.
            If IsOnUnix() Then
                If Not Console.CursorTop = Console.WindowHeight - 1 Or OldTop = Console.WindowHeight - 3 Then Console.CursorTop -= 1
            End If

            'Write the rows
            For RowIndex As Integer = 0 To Rows.GetLength(0) - 1
                For RowValueIndex As Integer = 0 To Rows.GetLength(1) - 1
                    Dim ColoredCell As Boolean
                    Dim CellColor As Color = NeutralTextColor
                    Dim CellBackgroundColor As Color = BackgroundColor
                    Dim RowValue As String = Rows(RowIndex, RowValueIndex)
                    Dim ColumnPosition As Integer = ColumnPositions(RowValueIndex)
                    If RowValue Is Nothing Then RowValue = ""

                    'Get the cell options and set them as necessary
                    If CellOptions IsNot Nothing Then
                        For Each CellOption As CellOptions In CellOptions
                            If CellOption.ColumnIndex = RowValueIndex And CellOption.RowIndex = RowIndex Then
                                ColoredCell = CellOption.ColoredCell
                                CellColor = CellOption.CellColor
                                CellBackgroundColor = CellOption.CellBackgroundColor
                            End If
                        Next
                    End If

                    'Now, write the cell value
                    If ColoredCell Then
                        WriteWhere(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, CellColor, CellBackgroundColor)
                    Else
                        WriteWhere(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, colorTypeForeground, colorTypeBackground)
                    End If
                Next
                Console.WriteLine()

                'Separate the rows optionally
                If SeparateRows Then
                    'Write the closing minus sign.
                    OldTop = Console.CursorTop
                    RepeatTimes = Console.WindowWidth - Console.CursorLeft - (Margin * 2)
                    If Margin > 0 Then Write(" ".Repeat(Margin), False, colorTypeForeground, colorTypeBackground)
                    Write("-".Repeat(RepeatTimes), True, colorTypeForeground, colorTypeBackground)

                    'Fix CursorTop value on Unix systems.
                    If IsOnUnix() Then
                        If Not Console.CursorTop = Console.WindowHeight - 1 Or OldTop = Console.WindowHeight - 3 Then Console.CursorTop -= 1
                    End If
                End If
            Next
        End Sub

        ''' <summary>
        ''' Draw a table with text
        ''' </summary>
        ''' <param name="Color">A color that will be changed to.</param>
        Public Sub WriteTable(Headers() As String, Rows(,) As String, Margin As Integer, Color As Color, Optional SeparateRows As Boolean = True, Optional CellOptions As List(Of CellOptions) = Nothing)
            Dim ColumnCapacity As Integer = Console.WindowWidth / Headers.Length
            Dim ColumnPositions As New List(Of Integer)
            Dim RepeatTimes As Integer

            'Populate the positions
            Console.WriteLine()
            For ColumnPosition As Integer = Margin To Console.WindowWidth Step ColumnCapacity
                If Not ColumnPosition >= Console.WindowWidth Then
                    ColumnPositions.Add(ColumnPosition)
                    If ColumnPositions.Count = 1 Then ColumnPosition = 0
                Else
                    Exit For
                End If
            Next

            'Write the headers
            For HeaderIndex As Integer = 0 To Headers.Length - 1
                Dim Header As String = Headers(HeaderIndex)
                Dim ColumnPosition As Integer = ColumnPositions(HeaderIndex)
                If Header Is Nothing Then Header = ""
                WriteWhere(Header.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, Color)
            Next
            Console.WriteLine()

            'Write the closing minus sign.
            Dim OldTop As Integer = Console.CursorTop
            RepeatTimes = Console.WindowWidth - Console.CursorLeft - (Margin * 2)
            If Margin > 0 Then Write(" ".Repeat(Margin), False, Color)
            Write("-".Repeat(RepeatTimes), True, Color)

            'Fix CursorTop value on Unix systems.
            If IsOnUnix() Then
                If Not Console.CursorTop = Console.WindowHeight - 1 Or OldTop = Console.WindowHeight - 3 Then Console.CursorTop -= 1
            End If

            'Write the rows
            For RowIndex As Integer = 0 To Rows.GetLength(0) - 1
                For RowValueIndex As Integer = 0 To Rows.GetLength(1) - 1
                    Dim ColoredCell As Boolean
                    Dim CellColor As Color = NeutralTextColor
                    Dim CellBackgroundColor As Color = BackgroundColor
                    Dim RowValue As String = Rows(RowIndex, RowValueIndex)
                    Dim ColumnPosition As Integer = ColumnPositions(RowValueIndex)
                    If RowValue Is Nothing Then RowValue = ""

                    'Get the cell options and set them as necessary
                    If CellOptions IsNot Nothing Then
                        For Each CellOption As CellOptions In CellOptions
                            If CellOption.ColumnIndex = RowValueIndex And CellOption.RowIndex = RowIndex Then
                                ColoredCell = CellOption.ColoredCell
                                CellColor = CellOption.CellColor
                                CellBackgroundColor = CellOption.CellBackgroundColor
                            End If
                        Next
                    End If

                    'Now, write the cell value
                    If ColoredCell Then
                        WriteWhere(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, CellColor, CellBackgroundColor)
                    Else
                        WriteWhere(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, Color)
                    End If
                Next
                Console.WriteLine()

                'Separate the rows optionally
                If SeparateRows Then
                    'Write the closing minus sign.
                    OldTop = Console.CursorTop
                    RepeatTimes = Console.WindowWidth - Console.CursorLeft - (Margin * 2)
                    If Margin > 0 Then Write(" ".Repeat(Margin), False, Color)
                    Write("-".Repeat(RepeatTimes), True, Color)

                    'Fix CursorTop value on Unix systems.
                    If IsOnUnix() Then
                        If Not Console.CursorTop = Console.WindowHeight - 1 Or OldTop = Console.WindowHeight - 3 Then Console.CursorTop -= 1
                    End If
                End If
            Next
        End Sub

        ''' <summary>
        ''' Draw a table with text
        ''' </summary>
        ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
        ''' <param name="BackgroundColor">A background color that will be changed to.</param>
        Public Sub WriteTable(Headers() As String, Rows(,) As String, Margin As Integer, ForegroundColor As Color, BackgroundColor As Color, Optional SeparateRows As Boolean = True, Optional CellOptions As List(Of CellOptions) = Nothing)
            Dim ColumnCapacity As Integer = Console.WindowWidth / Headers.Length
            Dim ColumnPositions As New List(Of Integer)
            Dim RepeatTimes As Integer

            'Populate the positions
            Console.WriteLine()
            For ColumnPosition As Integer = Margin To Console.WindowWidth Step ColumnCapacity
                If Not ColumnPosition >= Console.WindowWidth Then
                    ColumnPositions.Add(ColumnPosition)
                    If ColumnPositions.Count = 1 Then ColumnPosition = 0
                Else
                    Exit For
                End If
            Next

            'Write the headers
            For HeaderIndex As Integer = 0 To Headers.Length - 1
                Dim Header As String = Headers(HeaderIndex)
                Dim ColumnPosition As Integer = ColumnPositions(HeaderIndex)
                If Header Is Nothing Then Header = ""
                WriteWhere(Header.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, ForegroundColor, BackgroundColor)
            Next
            Console.WriteLine()

            'Write the closing minus sign.
            Dim OldTop As Integer = Console.CursorTop
            RepeatTimes = Console.WindowWidth - Console.CursorLeft - (Margin * 2)
            If Margin > 0 Then Write(" ".Repeat(Margin), False, ForegroundColor, BackgroundColor)
            Write("-".Repeat(RepeatTimes), True, ForegroundColor, BackgroundColor)

            'Fix CursorTop value on Unix systems.
            If IsOnUnix() Then
                If Not Console.CursorTop = Console.WindowHeight - 1 Or OldTop = Console.WindowHeight - 3 Then Console.CursorTop -= 1
            End If

            'Write the rows
            For RowIndex As Integer = 0 To Rows.GetLength(0) - 1
                For RowValueIndex As Integer = 0 To Rows.GetLength(1) - 1
                    Dim ColoredCell As Boolean
                    Dim CellColor As Color = NeutralTextColor
                    Dim CellBackgroundColor As Color = ColorTools.BackgroundColor
                    Dim RowValue As String = Rows(RowIndex, RowValueIndex)
                    Dim ColumnPosition As Integer = ColumnPositions(RowValueIndex)
                    If RowValue Is Nothing Then RowValue = ""

                    'Get the cell options and set them as necessary
                    If CellOptions IsNot Nothing Then
                        For Each CellOption As CellOptions In CellOptions
                            If CellOption.ColumnIndex = RowValueIndex And CellOption.RowIndex = RowIndex Then
                                ColoredCell = CellOption.ColoredCell
                                CellColor = CellOption.CellColor
                                CellBackgroundColor = CellOption.CellBackgroundColor
                            End If
                        Next
                    End If

                    'Now, write the cell value
                    If ColoredCell Then
                        WriteWhere(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, CellColor, CellBackgroundColor)
                    Else
                        WriteWhere(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, ForegroundColor, BackgroundColor)
                    End If
                Next
                Console.WriteLine()

                'Separate the rows optionally
                If SeparateRows Then
                    'Write the closing minus sign.
                    OldTop = Console.CursorTop
                    RepeatTimes = Console.WindowWidth - Console.CursorLeft - (Margin * 2)
                    If Margin > 0 Then Write(" ".Repeat(Margin), False, ForegroundColor, BackgroundColor)
                    Write("-".Repeat(RepeatTimes), True, ForegroundColor, BackgroundColor)

                    'Fix CursorTop value on Unix systems.
                    If IsOnUnix() Then
                        If Not Console.CursorTop = Console.WindowHeight - 1 Or OldTop = Console.WindowHeight - 3 Then Console.CursorTop -= 1
                    End If
                End If
            Next
        End Sub

    End Module
End Namespace
