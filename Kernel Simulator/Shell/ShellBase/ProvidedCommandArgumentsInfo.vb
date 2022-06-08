
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

Imports System.Text.RegularExpressions
Imports KS.Modifications

Namespace Shell.ShellBase
    Public Class ProvidedCommandArgumentsInfo

        ''' <summary>
        ''' Target command that the user executed in shell
        ''' </summary>
        Public ReadOnly Property Command As String
        ''' <summary>
        ''' Text version of the provided arguments and switches
        ''' </summary>
        Public ReadOnly Property ArgumentsText As String
        ''' <summary>
        ''' List version of the provided arguments and switches
        ''' </summary>
        Public ReadOnly Property FullArgumentsList As String()
        ''' <summary>
        ''' List version of the provided arguments
        ''' </summary>
        Public ReadOnly Property ArgumentsList As String()
        ''' <summary>
        ''' List version of the provided switches
        ''' </summary>
        Public ReadOnly Property SwitchesList As String()
        ''' <summary>
        ''' Checks to see if the required arguments are provided
        ''' </summary>
        Public ReadOnly Property RequiredArgumentsProvided As Boolean

        ''' <summary>
        ''' Makes a new instance of the command argument info with the user-provided command text
        ''' </summary>
        ''' <param name="CommandText">Command text that the user provided</param>
        ''' <param name="CommandType">Shell command type. Consult the <see cref="ShellType"/> enum for information about supported shells.</param>
        Friend Sub New(CommandText As String, CommandType As ShellType)
            Dim Command As String
            Dim RequiredArgumentsProvided As Boolean = True
            Dim ShellCommands As Dictionary(Of String, CommandInfo)
            Dim ModCommands As Dictionary(Of String, CommandInfo)

            'Change the available commands list according to command type
            ShellCommands = GetCommands(CommandType)
            ModCommands = ListModCommands(CommandType)

            'Get the index of the first space (Used for step 3)
            Dim index As Integer = CommandText.IndexOf(" ")
            If index = -1 Then index = CommandText.Length
            Wdbg(DebugLevel.I, "Index: {0}", index)

            'Split the requested command string into words
            Dim words() As String = CommandText.Split({" "c})
            For i As Integer = 0 To words.Length - 1
                Wdbg(DebugLevel.I, "Word {0}: {1}", i + 1, words(i))
            Next
            Command = words(0)

            'Get the string of arguments
            Dim strArgs As String = CommandText.Substring(index)
            Wdbg(DebugLevel.I, "Prototype strArgs: {0}", strArgs)
            If Not index = CommandText.Length Then strArgs = strArgs.Substring(1)
            Wdbg(DebugLevel.I, "Finished strArgs: {0}", strArgs)

            'Split the arguments with enclosed quotes and set the required boolean variable
            Dim CommandInfo As CommandInfo = If(ModCommands.ContainsKey(Command), ModCommands(Command), ShellCommands(Command))
            Dim EnclosedArgs As List(Of String) = Regex.Split(strArgs, "(?<=^[^""]*(?:""[^""]*""[^""]*)*) (?=(?:[^""]*""[^""]*"")*[^""]*$)").Select(Function(m) If(m.StartsWith("""") And m.EndsWith(""""), m.ReleaseDoubleQuotes(), m)).ToList()
            If EnclosedArgs IsNot Nothing Then
                RequiredArgumentsProvided = EnclosedArgs?.Count >= CommandInfo.MinimumArguments
            ElseIf CommandInfo.ArgumentsRequired And EnclosedArgs Is Nothing Then
                RequiredArgumentsProvided = False
            End If
            If EnclosedArgs IsNot Nothing Then Wdbg(DebugLevel.I, "Arguments parsed: " + String.Join(", ", EnclosedArgs))

            'Separate the arguments from the switches
            Dim FinalArgs As New List(Of String)
            Dim FinalSwitches As New List(Of String)
            If EnclosedArgs IsNot Nothing Then
                For Each EnclosedArg As String In EnclosedArgs
                    If EnclosedArg.StartsWith("-") Then
                        FinalSwitches.Add(EnclosedArg)
                    Else
                        FinalArgs.Add(EnclosedArg)
                    End If
                Next
            End If

            'Install the parsed values to the new class instance
            FullArgumentsList = EnclosedArgs?.ToArray
            ArgumentsList = FinalArgs.ToArray
            SwitchesList = FinalSwitches.ToArray
            ArgumentsText = strArgs
            Me.Command = Command
            Me.RequiredArgumentsProvided = RequiredArgumentsProvided
        End Sub

    End Class
End Namespace
