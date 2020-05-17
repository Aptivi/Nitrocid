
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

Imports System.IO

Public Module UESHParse

    'TODO: Support arguments parsing
    'TODO: Make documentation at the end of development of 0.0.10
    Public Sub Execute(ByVal scriptpath As String)
        Try
            'Open the script file for reading
            Dim FileStream As New StreamReader(scriptpath)
            Dim LineNo As Integer = 1
            Wdbg("I", "Stream opened. Parsing script")

            'Look for $variables and initialize them
            While Not FileStream.EndOfStream
                'Get line
                Dim Line As String = FileStream.ReadLine
                Wdbg("I", "Line {0}: ""{1}""", LineNo, Line)

                'If $variable is found in string, initialize it
                Dim SplitWords() As String = Line.Split(" ")
                For i As Integer = 0 To SplitWords.Count - 1
                    If Not ScriptVariables.ContainsKey(SplitWords(i)) And SplitWords(i).StartsWith("$") Then
                        InitializeVariable(SplitWords(i))
                    End If
                Next
            End While

            'Seek to the beginning
            FileStream.BaseStream.Seek(0, SeekOrigin.Begin)

            'Get all lines, parse comments, and parse commands
            While Not FileStream.EndOfStream
                'Get line
                Dim Line As String = FileStream.ReadLine
                Wdbg("I", "Line {0}: ""{1}""", LineNo, Line)

                'See if the line contains variable, and replace every instance of it with its value
                Dim SplitWords() As String = Line.Split(" ")
                For i As Integer = 0 To SplitWords.Count - 1
                    If SplitWords(i).StartsWith("$") Then
                        Line = GetVariable(SplitWords(i), Line)
                    End If
                Next

                'See if the line is a comment or command
                If Not Line.StartsWith("#") And Not Line.StartsWith(" ") Then
                    Wdbg("I", "Line {0} is not a comment.", Line)
                    GetLine(False, Line)
                Else 'For debugging purposes
                    Wdbg("I", "Line {0} is a comment.", Line)
                End If
            End While
        Catch ex As Exception
            W(DoTranslation("Error trying to execute script: {0}", currentLang), True, ColTypes.Err, ex.Message)
            WStkTrc(ex)
        End Try
    End Sub

End Module
