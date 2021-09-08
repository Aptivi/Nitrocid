
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

Module CommandLineArgsParse

    Public AvailableCMDLineArgs As New Dictionary(Of String, ArgumentInfo) From {{"testMod", New ArgumentInfo("testMod", ArgumentType.CommandLineArgs, "Tests mods by providing mod files", True, 1)},
                                                                                 {"testInteractive", New ArgumentInfo("testInteractive", ArgumentType.CommandLineArgs, "Opens a test shell", False, 0)},
                                                                                 {"debug", New ArgumentInfo("debug", ArgumentType.CommandLineArgs, "Enables debug mode", False, 0)},
                                                                                 {"args", New ArgumentInfo("args", ArgumentType.CommandLineArgs, "Prompts for arguments", False, 0)},
                                                                                 {"help", New ArgumentInfo("help", ArgumentType.CommandLineArgs, "Help page", False, 0)}}

    ''' <summary>
    ''' Parses the command line arguments
    ''' </summary>
    ''' <param name="Args">Arguments</param>
    Sub ParseCMDArguments(Args As String())
        Try
            If Args.Length <> 0 Then
                For Each Argument As String In Args
                    'Variables
                    Dim RequiredArgumentsProvided As Boolean = True

                    '1. Get the index of the first space (Used for step 3)
                    Dim index As Integer = Argument.IndexOf(" ")
                    If index = -1 Then index = Argument.Length
                    Wdbg(DebugLevel.I, "Index: {0}", index)

                    '2. Split the requested command string into words
                    Dim words() As String = Argument.Split({" "c})
                    For i As Integer = 0 To words.Length - 1
                        Wdbg(DebugLevel.I, "Word {0}: {1}", i + 1, words(i))
                    Next

                    '3. Get the string of arguments
                    Dim strArgs As String = Argument.Substring(index)
                    Wdbg(DebugLevel.I, "Prototype strArgs: {0}", strArgs)
                    If Not index = Argument.Length Then strArgs = strArgs.Substring(1)
                    Wdbg(DebugLevel.I, "Finished strArgs: {0}", strArgs)
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
                        For i As Integer = 0 To eqargs.Length - 1
                            eqargs(i).Replace("""", "")
                        Next
                        RequiredArgumentsProvided = eqargs?.Length >= AvailableCMDLineArgs(Argument).MinimumArguments
                    ElseIf AvailableCMDLineArgs(Argument).ArgumentsRequired And eqargs Is Nothing Then
                        RequiredArgumentsProvided = False
                    End If

                    '4a. Debug: get all arguments from eqargs()
                    If eqargs IsNot Nothing Then Wdbg(DebugLevel.I, "Arguments parsed from eqargs(): " + String.Join(", ", eqargs))

                    '5. Parse the arguments
                    If AvailableCMDLineArgs.ContainsKey(Argument) Then
                        Select Case Argument
                            Case "testMod"
                                If RequiredArgumentsProvided Then
                                    ParseMod(strArgs)
                                    If scripts.Count = 0 Then
                                        Environment.Exit(1)
                                    Else
                                        Environment.Exit(0)
                                    End If
                                End If
                            Case "testInteractive"
                                InitTShell()
                                If Test_ShutdownFlag Then Environment.Exit(0)
                            Case "debug"
                                DebugMode = True
                            Case "args"
                                argsOnBoot = True
                            Case "help"
                                W("- testMod: ", False, ColTypes.ListEntry) : W(AvailableCMDLineArgs("testMod").GetTranslatedHelpEntry, True, ColTypes.ListValue)
                                W("- testInteractive: ", False, ColTypes.ListEntry) : W(AvailableCMDLineArgs("testInteractive").GetTranslatedHelpEntry, True, ColTypes.ListValue)
                                W("- debug: ", False, ColTypes.ListEntry) : W(AvailableCMDLineArgs("debug").GetTranslatedHelpEntry, True, ColTypes.ListValue)
                                W("- args: ", False, ColTypes.ListEntry) : W(AvailableCMDLineArgs("args").GetTranslatedHelpEntry, True, ColTypes.ListValue)
                                W("- reset: ", False, ColTypes.ListEntry) : W(AvailableCMDLineArgs("reset").GetTranslatedHelpEntry, True, ColTypes.ListValue)
                                W(DoTranslation("* Press any key to start the kernel or ESC to exit."), True, ColTypes.Neutral)
                                If Console.ReadKey(True).Key = ConsoleKey.Escape Then
                                    Environment.Exit(0)
                                End If
                        End Select
                    Else
                        W(DoTranslation("Command line argument {0} not found."), True, ColTypes.Error, Argument)
                    End If
                Next
            End If
        Catch ex As Exception
            W(DoTranslation("Error while parsing real command-line arguments: {0}") + vbNewLine + "{1}", True, ColTypes.Error, ex.Message, ex.StackTrace)
            If Args.Contains("testMod") Then Environment.Exit(1)
        End Try
    End Sub

End Module
