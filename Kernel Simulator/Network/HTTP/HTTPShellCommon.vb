
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

Imports System.Net.Http
Imports System.Threading

Public Module HTTPShellCommon

    Public ReadOnly HTTPCommands As New Dictionary(Of String, CommandInfo) From {{"delete", New CommandInfo("delete", ShellType.HTTPShell, "Deletes content from HTTP server", {"<request>"}, True, 1, New HTTP_DeleteCommand)},
                                                                                 {"exit", New CommandInfo("exit", ShellType.HTTPShell, "Exits HTTP shell and returns to kernel", {}, False, 0, New HTTP_ExitCommand)},
                                                                                 {"get", New CommandInfo("get", ShellType.HTTPShell, "Gets the response from the HTTP server using the specified request", {"<request>"}, True, 1, New HTTP_GetCommand)},
                                                                                 {"getstring", New CommandInfo("getstring", ShellType.HTTPShell, "Gets the string from the HTTP server using the specified request", {"<request>"}, True, 1, New HTTP_GetStringCommand)},
                                                                                 {"help", New CommandInfo("help", ShellType.HTTPShell, "Shows help screen", {"[command]"}, False, 0, New HTTP_HelpCommand)},
                                                                                 {"setsite", New CommandInfo("setsite", ShellType.HTTPShell, "Sets the HTTP site. Must be a valid URI.", {"<uri>"}, True, 1, New HTTP_SetSiteCommand)}}
    Public HTTPSite As String
    Public HTTPModCommands As New ArrayList
    Public HTTPShellPromptStyle As String = ""
    Public ClientHTTP As New HttpClient()

    ''' <summary>
    ''' See if the HTTP shell is connected
    ''' </summary>
    Public ReadOnly Property HTTPConnected As Boolean
        Get
            Return Not String.IsNullOrEmpty(HTTPSite)
        End Get
    End Property

    ''' <summary>
    ''' Parses a command line from HTTP shell
    ''' </summary>
    Public Sub HTTPGetLine(HttpCommand As String)
        Dim words As String() = HttpCommand.SplitEncloseDoubleQuotes(" ")
        Wdbg(DebugLevel.I, $"Is the command found? {HTTPCommands.ContainsKey(words(0))}")
        If HTTPCommands.ContainsKey(words(0)) Then
            Wdbg(DebugLevel.I, "Command found.")
            Dim Params As New ExecuteCommandThreadParameters(HttpCommand, ShellType.HTTPShell, Nothing)
            HTTPCommandThread = New Thread(AddressOf ExecuteCommand) With {.Name = "HTTP Command Thread"}
            HTTPCommandThread.Start(Params)
            HTTPCommandThread.Join()
        ElseIf HTTPModCommands.Contains(words(0)) Then
            Wdbg(DebugLevel.I, "Mod command found.")
            ExecuteModCommand(HttpCommand)
        ElseIf HTTPShellAliases.Keys.Contains(words(0)) Then
            Wdbg(DebugLevel.I, "HTTP shell alias command found.")
            HttpCommand = HttpCommand.Replace($"""{words(0)}""", words(0))
            ExecuteHTTPAlias(HttpCommand)
        ElseIf Not HttpCommand.StartsWith(" ") Then
            Wdbg(DebugLevel.E, "Command {0} not found.", HttpCommand)
            Write(DoTranslation("HTTP message: The requested command {0} is not found. See 'help' for a list of available commands specified on HTTP shell."), True, ColTypes.Error, words(0))
        End If
    End Sub

End Module
