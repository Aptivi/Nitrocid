
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
Imports System.Text
Imports Microsoft.VisualBasic.FileIO

Module ArgumentParse

    ''' <summary>
    ''' Parses specified arguments
    ''' </summary>
    Public Sub ParseArguments()
        'Check for the arguments written by the user
        Try
            For i As Integer = 0 To EnteredArguments.Count - 1
                'Variables
                Dim Argument As String = EnteredArguments(i)

                '1. Get the index of the first space (Used for step 3)
                Dim index As Integer = Argument.IndexOf(" ")
                If index = -1 Then index = Argument.Length
                Wdbg("I", "Index: {0}", index)

                '2. Split the requested command string into words
                Dim words() As String = Argument.Split({" "c})
                For w As Integer = 0 To words.Length - 1
                    Wdbg("I", "Word {0}: {1}", w + 1, words(w))
                Next

                '3. Get the string of arguments
                Dim strArgs As String = Argument.Substring(index)
                Wdbg("I", "Prototype strArgs: {0}", strArgs)
                If Not index = Argument.Length Then strArgs = strArgs.Substring(1)
                Wdbg("I", "Finished strArgs: {0}", strArgs)
                Argument = words(0)

                '4. Split the arguments with enclosed quotes and set the required boolean variable
                Dim eqargs() As String
                Dim TStream As New MemoryStream(Encoding.Default.GetBytes(strArgs))
                Dim Parser As New TextFieldParser(TStream) With {
                    .Delimiters = {" "},
                    .HasFieldsEnclosedInQuotes = True,
                    .TrimWhiteSpace = False
                }
                eqargs = Parser.ReadFields
                If eqargs IsNot Nothing Then
                    For j As Integer = 0 To eqargs.Length - 1
                        eqargs(j).Replace("""", "")
                    Next
                End If

                '4a. Debug: get all arguments from eqargs()
                If eqargs IsNot Nothing Then Wdbg("I", "Arguments parsed from eqargs(): " + String.Join(", ", eqargs))

                '5. Parse the arguments
                If AvailableArgs.Contains(Argument) Then
                    Select Case Argument
                        Case "quiet"
                            DefConsoleOut = Console.Out
                            Console.SetOut(StreamWriter.Null)
                        Case "cmdinject"
                            If eqargs IsNot Nothing Then
                                For Each InjectedCommand As String In eqargs
                                    InjectedCommands.AddRange(InjectedCommand.Split({" : "}, StringSplitOptions.RemoveEmptyEntries))
                                    CommandFlag = True
                                Next
                            Else
                                W(DoTranslation("Available commands: {0}"), True, ColTypes.Neutral, String.Join(", ", Commands.Keys))
                                W(">> ", False, ColTypes.Input)
                                InjectedCommands.AddRange(Console.ReadLine().Split({" : "}, StringSplitOptions.RemoveEmptyEntries))
                                If String.Join(", ", InjectedCommands) <> "q" Then
                                    CommandFlag = True
                                Else
                                    W(DoTranslation("Command injection has been cancelled."), True, ColTypes.Neutral)
                                End If
                            End If
                        Case "debug"
                            DebugMode = True
                        Case "maintenance"
                            maintenance = True
                        Case "safe"
                            SafeMode = True
                        Case "testInteractive"
                            InitTShell()
                            If Test_ShutdownFlag Then Environment.Exit(0)
                    End Select
                Else
                    W(DoTranslation("The requested argument {0} is not found."), True, ColTypes.Error, Argument)
                End If
            Next
        Catch ex As Exception
            KernelError("U", True, 5, DoTranslation("Unrecoverable error in argument:") + " " + ex.Message, ex)
        End Try

    End Sub

End Module
