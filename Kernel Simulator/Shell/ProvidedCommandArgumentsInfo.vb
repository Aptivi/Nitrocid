
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

Public Class ProvidedCommandArgumentsInfo

    ''' <summary>
    ''' Target command that the user executed in shell
    ''' </summary>
    Public ReadOnly Property Command As String
    ''' <summary>
    ''' Text version of the provided arguments
    ''' </summary>
    Public ReadOnly Property ArgumentsText As String
    ''' <summary>
    ''' List version of the provided arguments
    ''' </summary>
    Public ReadOnly Property ArgumentsList As String()
    ''' <summary>
    ''' Checks to see if the required arguments are provided
    ''' </summary>
    Public ReadOnly Property RequiredArgumentsProvided As Boolean

    ''' <summary>
    ''' Makes a new instance of the command argument info with the user-provided command text
    ''' </summary>
    ''' <param name="CommandText">Command text that the user provided</param>
    ''' <param name="CommandType">Shell command type. Consult the <see cref="ShellCommandType"/> enum for information about supported shells.</param>
    Friend Sub New(CommandText As String, CommandType As ShellCommandType)
        Dim Command As String
        Dim RequiredArgumentsProvided As Boolean = True
        Dim ShellCommands As Dictionary(Of String, CommandInfo) = Commands

        'Change the available commands list according to command type
        Select Case CommandType
            Case ShellCommandType.FTPShell
                ShellCommands = FTPCommands
            Case ShellCommandType.MailShell
                ShellCommands = MailCommands
            Case ShellCommandType.RemoteDebugShell
                ShellCommands = DebugCommands
            Case ShellCommandType.RSSShell
                ShellCommands = RSSCommands
            Case ShellCommandType.SFTPShell
                ShellCommands = SFTPCommands
            Case ShellCommandType.TestShell
                ShellCommands = Test_Commands
            Case ShellCommandType.TextShell
                ShellCommands = TextEdit_Commands
            Case ShellCommandType.ZIPShell
                ShellCommands = ZipShell_Commands
        End Select

        'Get the index of the first space (Used for step 3)
        Dim index As Integer = CommandText.IndexOf(" ")
        If index = -1 Then index = CommandText.Length
        Wdbg("I", "Index: {0}", index)

        'Split the requested command string into words
        Dim words() As String = CommandText.Split({" "c})
        For i As Integer = 0 To words.Length - 1
            Wdbg("I", "Word {0}: {1}", i + 1, words(i))
        Next
        Command = words(0)

        'Get the string of arguments
        Dim strArgs As String = CommandText.Substring(index)
        Wdbg("I", "Prototype strArgs: {0}", strArgs)
        If Not index = CommandText.Length Then strArgs = strArgs.Substring(1)
        Wdbg("I", "Finished strArgs: {0}", strArgs)

        'Split the arguments with enclosed quotes and set the required boolean variable
        Dim eqargs() As String = strArgs.SplitEncloseDoubleQuotes(" ")
        If eqargs IsNot Nothing Then
            RequiredArgumentsProvided = eqargs?.Length >= ShellCommands(Command).MinimumArguments
        ElseIf ShellCommands(Command).ArgumentsRequired And eqargs Is Nothing Then
            RequiredArgumentsProvided = False
        End If
        If eqargs IsNot Nothing Then Wdbg("I", "Arguments parsed from eqargs(): " + String.Join(", ", eqargs))

        'Install the parsed values to the new class instance
        ArgumentsList = eqargs
        ArgumentsText = strArgs
        Me.Command = Command
        Me.RequiredArgumentsProvided = RequiredArgumentsProvided
    End Sub

End Class
