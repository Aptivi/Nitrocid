
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

Imports System.IO

Public Module UESHParse

    ''' <summary>
    ''' Executes the UESH script
    ''' </summary>
    ''' <param name="scriptpath">Full path to script</param>
    ''' <param name="scriptarguments">Script arguments</param>
    Public Sub Execute(ByVal scriptpath As String, ByVal scriptarguments As String)
        Try
            'Raise event
            EventManager.RaiseUESHPreExecute(scriptpath + " " + scriptarguments)

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
                For i As Integer = 0 To SplitWords.Length - 1
                    If Not ShellVariables.ContainsKey(SplitWords(i)) And SplitWords(i).StartsWith("$") Then
                        InitializeVariable(SplitWords(i))
                    End If
                Next
            End While

            'Seek to the beginning
            FileStream.BaseStream.Seek(0, SeekOrigin.Begin)

            'Get all lines and parse comments, commands, and arguments
            While Not FileStream.EndOfStream
                'Get line
                Dim Line As String = FileStream.ReadLine
                Wdbg("I", "Line {0}: ""{1}""", LineNo, Line)

                'See if the line contains variable, and replace every instance of it with its value
                Dim SplitWords() As String = Line.Split(" ")
                For i As Integer = 0 To SplitWords.Length - 1
                    If SplitWords(i).StartsWith("$") Then
                        Line = GetVariableCommand(SplitWords(i).Replace("""", ""), Line)
                    End If
                Next

                'See if the line contains argument placeholder, and replace every instance of it with its value
                Dim SplitArguments() As String = scriptarguments.Split(" ")
                For i As Integer = 0 To SplitWords.Length - 1
                    For j As Integer = 0 To SplitArguments.Length - 1
                        If SplitWords(i) = $"{{{j}}}" Then
                            Line = Line.Replace(SplitWords(i), SplitArguments(j))
                        End If
                    Next
                Next

                'See if the line is a comment or command
                If Not Line.StartsWith("#") And Not Line.StartsWith(" ") Then
                    Wdbg("I", "Line {0} is not a comment.", Line)
                    GetLine(False, Line)
                Else 'For debugging purposes
                    Wdbg("I", "Line {0} is a comment.", Line)
                End If
            End While
            EventManager.RaiseUESHPostExecute(scriptpath + " " + scriptarguments)
        Catch ex As Exception
            EventManager.RaiseUESHError(scriptpath + " " + scriptarguments, ex)
            W(DoTranslation("Error trying to execute script: {0}"), True, ColTypes.Error, ex.Message)
            WStkTrc(ex)
        End Try
    End Sub

End Module
