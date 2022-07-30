
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

Imports KS.Files.Read

Namespace Misc.Writers.MiscWriters
    Public Module LineHandleWriter

        ''' <summary>
        ''' Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        ''' </summary>
        ''' <param name="Condition">The condition to satisfy</param>
        ''' <param name="Filename">Path to text file</param>
        ''' <param name="LineNumber">Line number (not index)</param>
        ''' <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        Public Sub PrintLineWithHandleConditional(Condition As Boolean, Filename As String, LineNumber As Integer, ColumnNumber As Integer)
            PrintLineWithHandleConditional(Condition, Filename, LineNumber, ColumnNumber, ColTypes.Neutral)
        End Sub

        ''' <summary>
        ''' Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        ''' </summary>
        ''' <param name="Condition">The condition to satisfy</param>
        ''' <param name="Array">A string array containing the contents of the file</param>
        ''' <param name="LineNumber">Line number (not index)</param>
        ''' <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        Public Sub PrintLineWithHandleConditional(Condition As Boolean, Array() As String, LineNumber As Integer, ColumnNumber As Integer)
            PrintLineWithHandleConditional(Condition, Array, LineNumber, ColumnNumber, ColTypes.Neutral)
        End Sub

        ''' <summary>
        ''' Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        ''' </summary>
        ''' <param name="Condition">The condition to satisfy</param>
        ''' <param name="Filename">Path to text file</param>
        ''' <param name="LineNumber">Line number (not index)</param>
        ''' <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        ''' <param name="ColorType">The type of color</param>
        Public Sub PrintLineWithHandleConditional(Condition As Boolean, Filename As String, LineNumber As Integer, ColumnNumber As Integer, ColorType As ColTypes)
            If Condition Then
                PrintLineWithHandle(Filename, LineNumber, ColumnNumber, ColorType)
            End If
        End Sub

        ''' <summary>
        ''' Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        ''' </summary>
        ''' <param name="Condition">The condition to satisfy</param>
        ''' <param name="Array">A string array containing the contents of the file</param>
        ''' <param name="LineNumber">Line number (not index)</param>
        ''' <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        ''' <param name="ColorType">The type of color</param>
        Public Sub PrintLineWithHandleConditional(Condition As Boolean, Array() As String, LineNumber As Integer, ColumnNumber As Integer, ColorType As ColTypes)
            If Condition Then
                PrintLineWithHandle(Array, LineNumber, ColumnNumber, ColorType)
            End If
        End Sub

        ''' <summary>
        ''' Prints the line of a text file with the specified line number and the column number
        ''' </summary>
        ''' <param name="Filename">Path to text file</param>
        ''' <param name="LineNumber">Line number (not index)</param>
        ''' <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        Public Sub PrintLineWithHandle(Filename As String, LineNumber As Integer, ColumnNumber As Integer)
            PrintLineWithHandle(Filename, LineNumber, ColumnNumber, ColTypes.Neutral)
        End Sub

        ''' <summary>
        ''' Prints the line of a text file with the specified line number and the column number
        ''' </summary>
        ''' <param name="Array">A string array containing the contents of the file</param>
        ''' <param name="LineNumber">Line number (not index)</param>
        ''' <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        Public Sub PrintLineWithHandle(Array() As String, LineNumber As Integer, ColumnNumber As Integer)
            PrintLineWithHandle(Array, LineNumber, ColumnNumber, ColTypes.Neutral)
        End Sub

        ''' <summary>
        ''' Prints the line of a text file with the specified line number and the column number
        ''' </summary>
        ''' <param name="Filename">Path to text file</param>
        ''' <param name="LineNumber">Line number (not index)</param>
        ''' <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        ''' <param name="ColorType">The type of color</param>
        Public Sub PrintLineWithHandle(Filename As String, LineNumber As Integer, ColumnNumber As Integer, ColorType As ColTypes)
            'Read the contents
            ThrowOnInvalidPath(Filename)
            Filename = NeutralizePath(Filename)
            Dim FileContents() As String = ReadContents(Filename)

            'Do the job
            PrintLineWithHandle(FileContents, LineNumber, ColumnNumber, ColorType)
        End Sub

        ''' <summary>
        ''' Prints the line of a text file with the specified line number and the column number
        ''' </summary>
        ''' <param name="Array">A string array containing the contents of the file</param>
        ''' <param name="LineNumber">Line number (not index)</param>
        ''' <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        ''' <param name="ColorType">The type of color</param>
        Public Sub PrintLineWithHandle(Array() As String, LineNumber As Integer, ColumnNumber As Integer, ColorType As ColTypes)
            'Get the line index from number
            If LineNumber <= 0 Then LineNumber = 1
            If LineNumber > Array.Length Then LineNumber = Array.Length
            Dim LineIndex As Integer = LineNumber - 1

            'Get the line
            Dim LineContent As String = Array(LineIndex)
            Write(" | " + LineContent, True, ColorType)

            'Place the column handle
            Dim RepeatBlanks As Integer = ColumnNumber - 1
            If RepeatBlanks < 0 Then RepeatBlanks = 0
            Write(" | " + " ".Repeat(RepeatBlanks) + "^", True, ColorType)
        End Sub

    End Module
End Namespace
