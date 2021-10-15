
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Public Module TableColor

    ''' <summary>
    ''' Draw a table with text
    ''' </summary>
    ''' <param name="Headers">Headers to insert to the table.</param>
    ''' <param name="Rows">Rows to insert to the table.</param>
    ''' <param name="Margin">Safe threshold from left</param>
    ''' <param name="ColTypes">A type of colors that will be changed.</param>
    Public Sub WriteTable(Headers() As String, Rows(,) As String, Margin As Integer, ColTypes As ColTypes)
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
        If Margin > 0 Then W(" ".Repeat(Margin), False, ColTypes)
        W("-".Repeat(RepeatTimes), True, ColTypes)

        'Fix CursorTop value on Unix systems.
        If IsOnUnix() Then
            If Not Console.CursorTop = Console.WindowHeight - 1 Or OldTop = Console.WindowHeight - 3 Then Console.CursorTop -= 1
        End If

        'Write the rows
        For RowIndex As Integer = 0 To Rows.GetLength(0) - 1
            For RowValueIndex As Integer = 0 To Rows.GetLength(1) - 1
                Dim RowValue As String = Rows(RowIndex, RowValueIndex)
                Dim ColumnPosition As Integer = ColumnPositions(RowValueIndex)
                If RowValue Is Nothing Then RowValue = ""
                WriteWhere(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, ColTypes)
            Next
            Console.WriteLine()
        Next
    End Sub

    ''' <summary>
    ''' Draw a table with text
    ''' </summary>
    ''' <param name="Headers">Headers to insert to the table.</param>
    ''' <param name="Rows">Rows to insert to the table.</param>
    ''' <param name="Margin">Safe threshold from left</param>
    Public Sub WriteTable(Headers() As String, Rows(,) As String, Margin As Integer)
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
        If Margin > 0 Then W(" ".Repeat(Margin), False, ColTypes.Neutral)
        W("-".Repeat(RepeatTimes), True, ColTypes.TableSeparator)

        'Fix CursorTop value on Unix systems.
        If IsOnUnix() Then
            If Not Console.CursorTop = Console.WindowHeight - 1 Or OldTop = Console.WindowHeight - 3 Then Console.CursorTop -= 1
        End If

        'Write the rows
        For RowIndex As Integer = 0 To Rows.GetLength(0) - 1
            For RowValueIndex As Integer = 0 To Rows.GetLength(1) - 1
                Dim RowValue As String = Rows(RowIndex, RowValueIndex)
                Dim ColumnPosition As Integer = ColumnPositions(RowValueIndex)
                If RowValue Is Nothing Then RowValue = ""
                WriteWhere(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, ColTypes.TableValue)
            Next
            Console.WriteLine()
        Next
    End Sub

    ''' <summary>
    ''' Draw a table with text
    ''' </summary>
    ''' <param name="Color">A color that will be changed to.</param>
    Public Sub WriteTableC16(Headers() As String, Rows(,) As String, Margin As Integer, Color As ConsoleColor)
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
            WriteWhereC16(Header.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, Color)
        Next
        Console.WriteLine()

        'Write the closing minus sign.
        Dim OldTop As Integer = Console.CursorTop
        RepeatTimes = Console.WindowWidth - Console.CursorLeft - (Margin * 2)
        If Margin > 0 Then WriteC16(" ".Repeat(Margin), False, Color)
        WriteC16("-".Repeat(RepeatTimes), True, Color)

        'Fix CursorTop value on Unix systems.
        If IsOnUnix() Then
            If Not Console.CursorTop = Console.WindowHeight - 1 Or OldTop = Console.WindowHeight - 3 Then Console.CursorTop -= 1
        End If

        'Write the rows
        For RowIndex As Integer = 0 To Rows.GetLength(0) - 1
            For RowValueIndex As Integer = 0 To Rows.GetLength(1) - 1
                Dim RowValue As String = Rows(RowIndex, RowValueIndex)
                Dim ColumnPosition As Integer = ColumnPositions(RowValueIndex)
                If RowValue Is Nothing Then RowValue = ""
                WriteWhereC16(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, Color)
            Next
            Console.WriteLine()
        Next
    End Sub

    ''' <summary>
    ''' Draw a table with text
    ''' </summary>
    ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
    ''' <param name="BackgroundColor">A background color that will be changed to.</param>
    Public Sub WriteTableC16(Headers() As String, Rows(,) As String, Margin As Integer, ForegroundColor As ConsoleColor, BackgroundColor As ConsoleColor)
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
            WriteWhereC16(Header.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, ForegroundColor, BackgroundColor)
        Next
        Console.WriteLine()

        'Write the closing minus sign.
        Dim OldTop As Integer = Console.CursorTop
        RepeatTimes = Console.WindowWidth - Console.CursorLeft - (Margin * 2)
        If Margin > 0 Then WriteC16(" ".Repeat(Margin), False, ForegroundColor, BackgroundColor)
        WriteC16("-".Repeat(RepeatTimes), True, ForegroundColor, BackgroundColor)

        'Fix CursorTop value on Unix systems.
        If IsOnUnix() Then
            If Not Console.CursorTop = Console.WindowHeight - 1 Or OldTop = Console.WindowHeight - 3 Then Console.CursorTop -= 1
        End If

        'Write the rows
        For RowIndex As Integer = 0 To Rows.GetLength(0) - 1
            For RowValueIndex As Integer = 0 To Rows.GetLength(1) - 1
                Dim RowValue As String = Rows(RowIndex, RowValueIndex)
                Dim ColumnPosition As Integer = ColumnPositions(RowValueIndex)
                If RowValue Is Nothing Then RowValue = ""
                WriteWhereC16(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, ForegroundColor, BackgroundColor)
            Next
            Console.WriteLine()
        Next
    End Sub

    ''' <summary>
    ''' Draw a table with text
    ''' </summary>
    ''' <param name="Color">A color that will be changed to.</param>
    Public Sub WriteTableC(Headers() As String, Rows(,) As String, Margin As Integer, Color As Color)
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
            WriteWhereC(Header.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, Color)
        Next
        Console.WriteLine()

        'Write the closing minus sign.
        Dim OldTop As Integer = Console.CursorTop
        RepeatTimes = Console.WindowWidth - Console.CursorLeft - (Margin * 2)
        If Margin > 0 Then WriteC(" ".Repeat(Margin), False, Color)
        WriteC("-".Repeat(RepeatTimes), True, Color)

        'Fix CursorTop value on Unix systems.
        If IsOnUnix() Then
            If Not Console.CursorTop = Console.WindowHeight - 1 Or OldTop = Console.WindowHeight - 3 Then Console.CursorTop -= 1
        End If

        'Write the rows
        For RowIndex As Integer = 0 To Rows.GetLength(0) - 1
            For RowValueIndex As Integer = 0 To Rows.GetLength(1) - 1
                Dim RowValue As String = Rows(RowIndex, RowValueIndex)
                Dim ColumnPosition As Integer = ColumnPositions(RowValueIndex)
                If RowValue Is Nothing Then RowValue = ""
                WriteWhereC(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, Color)
            Next
            Console.WriteLine()
        Next
    End Sub

    ''' <summary>
    ''' Draw a table with text
    ''' </summary>
    ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
    ''' <param name="BackgroundColor">A background color that will be changed to.</param>
    Public Sub WriteTableC(Headers() As String, Rows(,) As String, Margin As Integer, ForegroundColor As Color, BackgroundColor As Color)
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
            WriteWhereC(Header.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, ForegroundColor, BackgroundColor)
        Next
        Console.WriteLine()

        'Write the closing minus sign.
        Dim OldTop As Integer = Console.CursorTop
        RepeatTimes = Console.WindowWidth - Console.CursorLeft - (Margin * 2)
        If Margin > 0 Then WriteC(" ".Repeat(Margin), False, ForegroundColor, BackgroundColor)
        WriteC("-".Repeat(RepeatTimes), True, ForegroundColor, BackgroundColor)

        'Fix CursorTop value on Unix systems.
        If IsOnUnix() Then
            If Not Console.CursorTop = Console.WindowHeight - 1 Or OldTop = Console.WindowHeight - 3 Then Console.CursorTop -= 1
        End If

        'Write the rows
        For RowIndex As Integer = 0 To Rows.GetLength(0) - 1
            For RowValueIndex As Integer = 0 To Rows.GetLength(1) - 1
                Dim RowValue As String = Rows(RowIndex, RowValueIndex)
                Dim ColumnPosition As Integer = ColumnPositions(RowValueIndex)
                If RowValue Is Nothing Then RowValue = ""
                WriteWhereC(RowValue.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, False, ForegroundColor, BackgroundColor)
            Next
            Console.WriteLine()
        Next
    End Sub

End Module
