
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

Imports System.Net.Http
Imports System.Threading

Public Module HTTPShell

    Public ReadOnly HTTPCommands As New Dictionary(Of String, CommandInfo) From {{"delete", New CommandInfo("delete", ShellCommandType.HTTPShell, "Deletes content from HTTP server", {"<request>"}, True, 1, New HTTP_DeleteCommand)},
                                                                                 {"exit", New CommandInfo("exit", ShellCommandType.HTTPShell, "Exits HTTP shell and returns to kernel", {}, False, 0, New HTTP_ExitCommand)},
                                                                                 {"get", New CommandInfo("get", ShellCommandType.HTTPShell, "Gets the response from the HTTP server using the specified request", {"<request>"}, True, 1, New HTTP_GetCommand)},
                                                                                 {"getstring", New CommandInfo("getstring", ShellCommandType.HTTPShell, "Gets the string from the HTTP server using the specified request", {"<request>"}, True, 1, New HTTP_GetStringCommand)},
                                                                                 {"help", New CommandInfo("help", ShellCommandType.HTTPShell, "Shows help screen", {"[command]"}, False, 0, New HTTP_HelpCommand)},
                                                                                 {"setsite", New CommandInfo("setsite", ShellCommandType.HTTPShell, "Sets the HTTP site. Must be a valid URI.", {"<uri>"}, True, 1, New HTTP_SetSiteCommand)}}
    Public HTTPSite As String
    Public HTTPModCommands As New ArrayList
    Public HTTPShellPromptStyle As String = ""
    Public ClientHTTP As New HttpClient()
    Friend HTTPExit As Boolean
    Private HTTPCommand As String

    ''' <summary>
    ''' See if the HTTP shell is connected
    ''' </summary>
    Public ReadOnly Property HTTPConnected As Boolean
        Get
            Return Not String.IsNullOrEmpty(HTTPSite)
        End Get
    End Property

    ''' <summary>
    ''' Initializes the HTTP shell
    ''' </summary>
    Public Sub InitiateHttpShell()
        While Not HTTPExit
            SyncLock HTTPCancelSync
                Try
                    'Prompt for command
                    If DefConsoleOut IsNot Nothing Then
                        Console.SetOut(DefConsoleOut)
                    End If
                    Wdbg(DebugLevel.I, "Preparing prompt...")
                    If HTTPConnected Then
                        Wdbg(DebugLevel.I, "HTTPShellPromptStyle = {0}", HTTPShellPromptStyle)
                        If HTTPShellPromptStyle = "" Then
                            Write("[", False, ColTypes.Gray) : Write("{0}", False, ColTypes.HostName, HTTPSite) : Write("]> ", False, ColTypes.Gray)
                        Else
                            Dim ParsedPromptStyle As String = ProbePlaces(HTTPShellPromptStyle)
                            ParsedPromptStyle.ConvertVTSequences
                            Write(ParsedPromptStyle, False, ColTypes.Gray)
                        End If
                    Else
                        Write("> ", False, ColTypes.Gray)
                    End If

                    'Run garbage collector
                    DisposeAll()

                    'Set input color
                    SetInputColor()

                    'Prompt for command
                    Wdbg(DebugLevel.I, "Normal shell")
                    HTTPCommand = Console.ReadLine()
                    KernelEventManager.RaiseHTTPPreExecuteCommand(HTTPCommand)

                    'Parse command
                    If Not (HTTPCommand = Nothing Or HTTPCommand?.StartsWithAnyOf({" ", "#"})) Then
                        HTTPGetLine()
                        KernelEventManager.RaiseHTTPPostExecuteCommand(HTTPCommand)
                    End If
                Catch ex As Exception
                    WStkTrc(ex)
                    Throw New Exceptions.HTTPShellException(DoTranslation("There was an error in the HTTP shell:") + " {0}", ex, ex.Message)
                End Try
            End SyncLock
        End While

        'Exiting, so reset the site
        HTTPSite = ""
    End Sub

    ''' <summary>
    ''' Parses a command line from HTTP shell
    ''' </summary>
    Public Sub HTTPGetLine()
        Dim words As String() = HTTPCommand.SplitEncloseDoubleQuotes(" ")
        Wdbg(DebugLevel.I, $"Is the command found? {HTTPCommands.ContainsKey(words(0))}")
        If HTTPCommands.ContainsKey(words(0)) Then
            Wdbg(DebugLevel.I, "Command found.")
            Dim Params As New ExecuteCommandThreadParameters(HTTPCommand, ShellCommandType.HTTPShell, Nothing)
            HTTPCommandThread = New Thread(AddressOf ExecuteCommand) With {.Name = "HTTP Command Thread"}
            HTTPCommandThread.Start(Params)
            HTTPCommandThread.Join()
        ElseIf HTTPModCommands.Contains(words(0)) Then
            Wdbg(DebugLevel.I, "Mod command found.")
            ExecuteModCommand(HTTPCommand)
        ElseIf HTTPShellAliases.Keys.Contains(words(0)) Then
            Wdbg(DebugLevel.I, "HTTP shell alias command found.")
            HTTPCommand = HTTPCommand.Replace($"""{words(0)}""", words(0))
            ExecuteHTTPAlias(HTTPCommand)
        ElseIf Not HTTPCommand.StartsWith(" ") Then
            Wdbg(DebugLevel.E, "Command {0} not found.", HTTPCommand)
            Write(DoTranslation("HTTP message: The requested command {0} is not found. See 'help' for a list of available commands specified on HTTP shell."), True, ColTypes.Error, words(0))
        End If
    End Sub

End Module
