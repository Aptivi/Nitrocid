
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

Public Module SFTPShell

    Public ReadOnly SFTPCommands As New Dictionary(Of String, CommandInfo) From {{"connect", New CommandInfo("connect", ShellCommandType.SFTPShell, "Connects to an SFTP server (it must start with ""sftp://"")", "<server>", True, 1)},
                                                                                 {"cdl", New CommandInfo("cdl", ShellCommandType.SFTPShell, "Changes local directory to download to or upload from", "<directory>", True, 1)},
                                                                                 {"cdr", New CommandInfo("cdr", ShellCommandType.SFTPShell, "Changes remote directory to download from or upload to", "<directory>", True, 1)},
                                                                                 {"del", New CommandInfo("del", ShellCommandType.SFTPShell, "Deletes remote file from server", "<file>", True, 1)},
                                                                                 {"disconnect", New CommandInfo("disconnect", ShellCommandType.SFTPShell, "Disconnects from server", "", False, 0)},
                                                                                 {"exit", New CommandInfo("exit", ShellCommandType.SFTPShell, "Exits SFTP shell and returns to kernel", "", False, 0)},
                                                                                 {"get", New CommandInfo("get", ShellCommandType.SFTPShell, "Downloads remote file to local directory using binary or text", "<file>", True, 1)},
                                                                                 {"help", New CommandInfo("help", ShellCommandType.SFTPShell, "Shows help screen", "", False, 0)},
                                                                                 {"lsl", New CommandInfo("lsl", ShellCommandType.SFTPShell, "Lists local directory", "[dir]", False, 0)},
                                                                                 {"lsr", New CommandInfo("lsr", ShellCommandType.SFTPShell, "Lists remote directory", "[dir]", False, 0)},
                                                                                 {"put", New CommandInfo("put", ShellCommandType.SFTPShell, "Uploads local file to remote directory using binary or text", "<file>", True, 1)},
                                                                                 {"pwdl", New CommandInfo("pwdl", ShellCommandType.SFTPShell, "Gets current local directory", "", False, 0)},
                                                                                 {"pwdr", New CommandInfo("pwdr", ShellCommandType.SFTPShell, "Gets current remote directory", "", False, 0)},
                                                                                 {"quickconnect", New CommandInfo("quickconnect", ShellCommandType.SFTPShell, "Uses information from Speed Dial to connect to any network quickly", "", False, 0)}}
    Public SFTPConnected As Boolean = False
    Private SFTPInitialized As Boolean = False
    Public sftpsite As String
    Public SFTPCurrDirect As String
    Public SFTPCurrentRemoteDir As String
    Public SFTPUser As String
    Friend SFTPPass As String
    Private SFTPStrCmd As String
    Public sftpexit As Boolean = False
    Public SFTPModCommands As New ArrayList
    Public SFTPShellPromptStyle As String = ""
    Public ClientSFTP As SftpClient

    ''' <summary>
    ''' Initializes the SFTP shell
    ''' </summary>
    ''' <param name="Connects">Specifies whether the SFTP client is currently connecting</param>
    ''' <param name="Address">An IP address</param>
    Public Sub SFTPInitiateShell(Optional Connects As Boolean = False, Optional Address As String = "")
        While True
            Try
                'Complete initialization
                If SFTPInitialized = False Then
                    Wdbg("I", $"Completing initialization of SFTP: {SFTPInitialized}")
                    SFTPCurrDirect = GetOtherPath(OtherPathType.Home)
                    EventManager.RaiseSFTPShellInitialized()

                    'This is the workaround for a bug in .NET Framework regarding Console.CancelKeyPress event. More info can be found below:
                    'https://stackoverflow.com/a/22717063/6688914
                    AddHandler Console.CancelKeyPress, AddressOf SFTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
                    SFTPInitialized = True
                End If

                'Check if the shell is going to exit
                If sftpexit = True Then
                    Wdbg("W", "Exiting shell...")
                    SFTPConnected = False
                    ClientSFTP?.Disconnect()
                    sftpsite = ""
                    SFTPCurrDirect = ""
                    SFTPCurrentRemoteDir = ""
                    SFTPUser = ""
                    SFTPPass = ""
                    SFTPStrCmd = ""
                    sftpexit = False
                    SFTPInitialized = False
                    AddHandler Console.CancelKeyPress, AddressOf CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf SFTPCancelCommand
                    Exit Sub
                End If

                'Prompt for command
                If DefConsoleOut IsNot Nothing Then
                    Console.SetOut(DefConsoleOut)
                End If
                If Not Connects Then
                    Wdbg("I", "Preparing prompt...")
                    If SFTPConnected Then
                        Wdbg("I", "SFTPShellPromptStyle = {0}", SFTPShellPromptStyle)
                        If SFTPShellPromptStyle = "" Then
                            W("[", False, ColTypes.Gray) : W("{0}", False, ColTypes.UserName, SFTPUser) : W("@", False, ColTypes.Gray) : W("{0}", False, ColTypes.HostName, sftpsite) : W("]{0}> ", False, ColTypes.Gray, SFTPCurrentRemoteDir)
                        Else
                            Dim ParsedPromptStyle As String = ProbePlaces(SFTPShellPromptStyle)
                            ParsedPromptStyle.ConvertVTSequences
                            W(ParsedPromptStyle, False, ColTypes.Gray)
                        End If
                    Else
                        W("{0}> ", False, ColTypes.Gray, SFTPCurrDirect)
                    End If
                End If

                'Run garbage collector
                DisposeAll()

                'Set input color
                SetInputColor()

                'Try to connect if IP address is specified.
                If Connects Then
                    Wdbg("I", $"Currently connecting to {Address} by ""sftp (address)""...")
                    SFTPStrCmd = $"connect {Address}"
                    Connects = False
                Else
                    Wdbg("I", "Normal shell")
                    SFTPStrCmd = Console.ReadLine()
                End If
                EventManager.RaiseSFTPPreExecuteCommand(SFTPStrCmd)

                'Parse command
                If Not (SFTPStrCmd = Nothing Or SFTPStrCmd?.StartsWithAnyOf({" ", "#"})) Then
                    SFTPGetLine()
                    EventManager.RaiseSFTPPostExecuteCommand(SFTPStrCmd)
                End If

                'This is to fix race condition between SFTP shell initialization and starting the event handler thread
                If SFTPStrCmd Is Nothing Then
                    Thread.Sleep(30)
                End If
            Catch ex As Exception
                WStkTrc(ex)
                Throw New Exceptions.SFTPShellException(DoTranslation("There was an error in the SFTP shell:") + " {0}", ex, ex.Message)
            End Try
        End While
    End Sub

    ''' <summary>
    ''' Parses a command line from FTP shell
    ''' </summary>
    Public Sub SFTPGetLine()
        Dim words As String() = SFTPStrCmd.SplitEncloseDoubleQuotes(" ")
        Wdbg("I", "Command: {0}", SFTPStrCmd)
        Wdbg("I", $"Is the command found? {SFTPCommands.ContainsKey(words(0))}")
        If SFTPCommands.ContainsKey(words(0)) Then
            Wdbg("I", "Command found.")
            SFTPStartCommandThread = New Thread(AddressOf SFTPGetCommand.ExecuteCommand) With {.Name = "SFTP Command Thread"}
            SFTPStartCommandThread.Start(SFTPStrCmd)
            SFTPStartCommandThread.Join()
        ElseIf SFTPModCommands.Contains(words(0)) Then
            Wdbg("I", "Mod command found.")
            ExecuteModCommand(SFTPStrCmd)
        ElseIf SFTPShellAliases.Keys.Contains(words(0)) Then
            Wdbg("I", "Aliased command found.")
            SFTPStrCmd = SFTPStrCmd.Replace($"""{words(0)}""", words(0))
            ExecuteSFTPAlias(SFTPStrCmd)
        ElseIf Not SFTPStrCmd.StartsWith(" ") Then
            Wdbg("E", "Command {0} not found.", SFTPStrCmd)
            W(DoTranslation("SFTP message: The requested command {0} is not found. See 'help' for a list of available commands specified on SFTP shell."), True, ColTypes.Error, words(0))
        End If
    End Sub

    ''' <summary>
    ''' Executes the SFTP shell alias
    ''' </summary>
    ''' <param name="aliascmd">Aliased command with arguments</param>
    Sub ExecuteSFTPAlias(aliascmd As String)
        Dim FirstWordCmd As String = aliascmd.SplitEncloseDoubleQuotes(" ")(0)
        Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, SFTPShellAliases(FirstWordCmd))
        Wdbg("I", "Actual command: {0}", actualCmd)
        SFTPStartCommandThread = New Thread(AddressOf SFTPGetCommand.ExecuteCommand) With {.Name = "SFTP Command Thread"}
        SFTPStartCommandThread.Start(actualCmd)
        SFTPStartCommandThread.Join()
    End Sub

End Module
