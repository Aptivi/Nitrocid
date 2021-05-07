
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

Imports System.Threading

Public Module FTPShell

    Public ftpstream As FtpWebRequest
    Public FTPCommands As New Dictionary(Of String, CommandInfo) From {{"currlocaldir", New CommandInfo("currlocaldir", ShellCommandType.FTPShell, False, 0, False, False, False, False)},
                                                                       {"currremotedir", New CommandInfo("currremotedir", ShellCommandType.FTPShell, False, 0, False, False, False, False)},
                                                                       {"connect", New CommandInfo("connect", ShellCommandType.FTPShell, True, 1, False, False, False, False)},
                                                                       {"changelocaldir", New CommandInfo("changelocaldir", ShellCommandType.FTPShell, True, 1, False, False, False, False)},
                                                                       {"changeremotedir", New CommandInfo("changeremotedir", ShellCommandType.FTPShell, True, 1, False, False, False, False)},
                                                                       {"cdl", New CommandInfo("cdl", ShellCommandType.FTPShell, True, 1, False, False, False, False)},
                                                                       {"cdr", New CommandInfo("cdr", ShellCommandType.FTPShell, True, 1, False, False, False, False)},
                                                                       {"delete", New CommandInfo("delete", ShellCommandType.FTPShell, True, 1, False, False, False, False)},
                                                                       {"del", New CommandInfo("del", ShellCommandType.FTPShell, True, 2, False, False, False, False)},
                                                                       {"disconnect", New CommandInfo("disconnect", ShellCommandType.FTPShell, False, 0, False, False, False, False)},
                                                                       {"download", New CommandInfo("download", ShellCommandType.FTPShell, True, 1, False, False, False, False)},
                                                                       {"exit", New CommandInfo("exit", ShellCommandType.FTPShell, False, 0, False, False, False, False)},
                                                                       {"get", New CommandInfo("get", ShellCommandType.FTPShell, True, 1, False, False, False, False)},
                                                                       {"help", New CommandInfo("help", ShellCommandType.FTPShell, False, 0, False, False, False, False)},
                                                                       {"listlocal", New CommandInfo("listlocal", ShellCommandType.FTPShell, False, 0, False, False, False, False)},
                                                                       {"lsl", New CommandInfo("lsl", ShellCommandType.FTPShell, False, 0, False, False, False, False)},
                                                                       {"listremote", New CommandInfo("listremote", ShellCommandType.FTPShell, False, 0, False, False, False, False)},
                                                                       {"lsr", New CommandInfo("lsr", ShellCommandType.FTPShell, False, 0, False, False, False, False)},
                                                                       {"put", New CommandInfo("put", ShellCommandType.FTPShell, True, 1, False, False, False, False)},
                                                                       {"pwdl", New CommandInfo("pwdl", ShellCommandType.FTPShell, False, 0, False, False, False, False)},
                                                                       {"pwdr", New CommandInfo("pwdr", ShellCommandType.FTPShell, False, 0, False, False, False, False)},
                                                                       {"move", New CommandInfo("move", ShellCommandType.FTPShell, True, 2, False, False, False, False)},
                                                                       {"mv", New CommandInfo("mv", ShellCommandType.FTPShell, True, 2, False, False, False, False)},
                                                                       {"copy", New CommandInfo("copy", ShellCommandType.FTPShell, True, 2, False, False, False, False)},
                                                                       {"cp", New CommandInfo("cp", ShellCommandType.FTPShell, True, 2, False, False, False, False)},
                                                                       {"upload", New CommandInfo("upload", ShellCommandType.FTPShell, True, 1, False, False, False, False)},
                                                                       {"quickconnect", New CommandInfo("quickconnect", ShellCommandType.FTPShell, False, 0, False, False, False, False)}}
    Public connected As Boolean = False
    Private initialized As Boolean = False
    Public ftpsite As String
    Public currDirect As String 'Current Local Directory
    Public currentremoteDir As String 'Current Remote Directory
    Public user As String
    Friend pass As String
    Private strcmd As String
    Public ftpexit As Boolean = False
    Public FTPModCommands As New ArrayList
    Public FTPShellPromptStyle As String = ""

    ''' <summary>
    ''' Initializes the FTP shell
    ''' </summary>
    ''' <param name="Connects">Specifies whether the FTP client is currently connecting</param>
    ''' <param name="Address">An IP address</param>
    Public Sub InitiateShell(Optional ByVal Connects As Boolean = False, Optional ByVal Address As String = "")
        While True
            Try
                'Complete initialization
                If initialized = False Then
                    Wdbg("I", $"Completing initialization of FTP: {initialized}")
                    FtpTrace.AddListener(New FTPTracer)
                    FtpTrace.LogUserName = FTPLoggerUsername
                    FtpTrace.LogPassword = False 'Don't remove this, make a config entry for it, or set it to True! It will introduce security problems.
                    FtpTrace.LogIP = FTPLoggerIP
                    currDirect = paths("Home")
                    EventManager.RaiseFTPShellInitialized()

                    'This is the workaround for a bug in .NET Framework regarding Console.CancelKeyPress event. More info can be found below:
                    'https://stackoverflow.com/a/22717063/6688914
                    AddHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
                    initialized = True
                End If

                'Check if the shell is going to exit
                If ftpexit = True Then
                    Wdbg("W", "Exiting shell...")
                    connected = False
                    ClientFTP?.Disconnect()
                    ftpsite = ""
                    currDirect = ""
                    currentremoteDir = ""
                    user = ""
                    pass = ""
                    strcmd = ""
                    ftpexit = False
                    initialized = False
                    AddHandler Console.CancelKeyPress, AddressOf CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                    Exit Sub
                End If

                'Prompt for command
                If Not IsNothing(DefConsoleOut) Then
                    Console.SetOut(DefConsoleOut)
                End If
                If Not Connects Then
                    Wdbg("I", "Preparing prompt...")
                    If connected Then
                        Wdbg("I", "FTPShellPromptStyle = {0}", FTPShellPromptStyle)
                        If FTPShellPromptStyle = "" Then
                            W("[", False, ColTypes.Gray) : W("{0}", False, ColTypes.UserName, user) : W("@", False, ColTypes.Gray) : W("{0}", False, ColTypes.HostName, ftpsite) : W("]{0} ", False, ColTypes.Gray, currentremoteDir)
                        Else
                            Dim ParsedPromptStyle As String = ProbePlaces(FTPShellPromptStyle)
                            ParsedPromptStyle.ConvertVTSequences
                            W(ParsedPromptStyle, False, ColTypes.Gray)
                        End If
                    Else
                        W("{0}> ", False, ColTypes.Gray, currDirect)
                    End If
                End If

                'Run garbage collector
                DisposeAll()

                'Set input color
                SetInputColor()

                'Try to connect if IP address is specified.
                If Connects Then
                    Wdbg("I", $"Currently connecting to {Address} by ""ftp (address)""...")
                    strcmd = $"connect {Address}"
                    Connects = False
                Else
                    Wdbg("I", "Normal shell")
                    strcmd = Console.ReadLine()
                End If
                EventManager.RaiseFTPPreExecuteCommand(strcmd)

                'Parse command
                If Not (strcmd = Nothing Or strcmd?.StartsWith(" ")) Then
                    FTPGetLine()
                    EventManager.RaiseFTPPostExecuteCommand(strcmd)
                End If

                'When pressing CTRL+C on shell after command execution, it can generate another prompt without making newline, so fix this.
                If IsNothing(strcmd) Then
                    Console.WriteLine()
                    Thread.Sleep(30) 'This is to fix race condition between FTP shell initialization and starting the event handler thread
                End If
            Catch ex As Exception
                WStkTrc(ex)
                Throw New Exceptions.FTPShellException(DoTranslation("There was an error in the FTP shell:") + " {0}".FormatString(ex.Message), ex)
            End Try
        End While
    End Sub

    ''' <summary>
    ''' Parses a command line from FTP shell
    ''' </summary>
    Public Sub FTPGetLine()
        Dim words As String() = strcmd.Split({" "c})
        Wdbg("I", $"Is the command found? {FTPCommands.ContainsKey(words(0))}")
        If FTPCommands.ContainsKey(words(0)) Then
            Wdbg("I", "Command found.")
            FTPStartCommandThread = New Thread(AddressOf FTPGetCommand.ExecuteCommand)
            FTPStartCommandThread.Start(strcmd)
            FTPStartCommandThread.Join()
        ElseIf FTPModCommands.Contains(words(0)) Then
            Wdbg("I", "Mod command found.")
            ExecuteModCommand(strcmd)
        ElseIf FTPShellAliases.Keys.Contains(words(0)) Then
            Wdbg("I", "FTP shell alias command found.")
            ExecuteFTPAlias(strcmd)
        ElseIf Not strcmd.StartsWith(" ") Then
            Wdbg("E", "Command {0} not found.", strcmd)
            W(DoTranslation("FTP message: The requested command {0} is not found. See 'help' for a list of available commands specified on FTP shell."), True, ColTypes.Err, words(0))
        End If
    End Sub

    ''' <summary>
    ''' Executes the FTP shell alias
    ''' </summary>
    ''' <param name="aliascmd">Aliased command with arguments</param>
    Sub ExecuteFTPAlias(ByVal aliascmd As String)
        Dim FirstWordCmd As String = aliascmd.Split(" "c)(0)
        Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, FTPShellAliases(FirstWordCmd))
        Wdbg("I", "Actual command: {0}", actualCmd)
        FTPStartCommandThread = New Thread(AddressOf FTPGetCommand.ExecuteCommand)
        FTPStartCommandThread.Start(actualCmd)
        FTPStartCommandThread.Join()
    End Sub

End Module
