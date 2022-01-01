
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

Imports System.Threading

Public Module SFTPShell

    Public ReadOnly SFTPCommands As New Dictionary(Of String, CommandInfo) From {{"connect", New CommandInfo("connect", ShellCommandType.SFTPShell, "Connects to an SFTP server (it must start with ""sftp://"")", {"<server>"}, True, 1, New SFTP_ConnectCommand)},
                                                                                 {"cdl", New CommandInfo("cdl", ShellCommandType.SFTPShell, "Changes local directory to download to or upload from", {"<directory>"}, True, 1, New SFTP_CdlCommand)},
                                                                                 {"cdr", New CommandInfo("cdr", ShellCommandType.SFTPShell, "Changes remote directory to download from or upload to", {"<directory>"}, True, 1, New SFTP_CdrCommand)},
                                                                                 {"del", New CommandInfo("del", ShellCommandType.SFTPShell, "Deletes remote file from server", {"<file>"}, True, 1, New SFTP_DelCommand)},
                                                                                 {"disconnect", New CommandInfo("disconnect", ShellCommandType.SFTPShell, "Disconnects from server", {}, False, 0, New SFTP_DisconnectCommand)},
                                                                                 {"exit", New CommandInfo("exit", ShellCommandType.SFTPShell, "Exits SFTP shell and returns to kernel", {}, False, 0, New SFTP_ExitCommand)},
                                                                                 {"get", New CommandInfo("get", ShellCommandType.SFTPShell, "Downloads remote file to local directory using binary or text", {"<file>"}, True, 1, New SFTP_GetCommand)},
                                                                                 {"help", New CommandInfo("help", ShellCommandType.SFTPShell, "Shows help screen", {}, False, 0, New SFTP_HelpCommand)},
                                                                                 {"lsl", New CommandInfo("lsl", ShellCommandType.SFTPShell, "Lists local directory", {"[-showdetails|-suppressmessages] [dir]"}, False, 0, New SFTP_LslCommand, False, False, False, False, False, New Action(AddressOf (New SFTP_LslCommand).HelpHelper))},
                                                                                 {"lsr", New CommandInfo("lsr", ShellCommandType.SFTPShell, "Lists remote directory", {"[-showdetails] [dir]"}, False, 0, New SFTP_LsrCommand, False, False, False, False, False, New Action(AddressOf (New SFTP_LsrCommand).HelpHelper))},
                                                                                 {"put", New CommandInfo("put", ShellCommandType.SFTPShell, "Uploads local file to remote directory using binary or text", {"<file>"}, True, 1, New SFTP_PutCommand)},
                                                                                 {"pwdl", New CommandInfo("pwdl", ShellCommandType.SFTPShell, "Gets current local directory", {}, False, 0, New SFTP_PwdlCommand)},
                                                                                 {"pwdr", New CommandInfo("pwdr", ShellCommandType.SFTPShell, "Gets current remote directory", {}, False, 0, New SFTP_PwdrCommand)},
                                                                                 {"quickconnect", New CommandInfo("quickconnect", ShellCommandType.SFTPShell, "Uses information from Speed Dial to connect to any network quickly", {}, False, 0, New SFTP_QuickConnectCommand)}}
    Public SFTPConnected As Boolean
    Public SFTPSite As String
    Public SFTPCurrDirect As String
    Public SFTPCurrentRemoteDir As String
    Public SFTPUser As String
    Public SFTPModCommands As New ArrayList
    Public SFTPShellPromptStyle As String = ""
    Public SFTPShowDetailsInList As Boolean = True
    Public SFTPUserPromptStyle As String = ""
    Public SFTPNewConnectionsToSpeedDial As Boolean = True
    Public ClientSFTP As SftpClient
    Friend SFTPPass As String
    Friend SFTPExit As Boolean
    Private SFTPStrCmd As String
    Private SFTPInitialized As Boolean

    ''' <summary>
    ''' Initializes the SFTP shell
    ''' </summary>
    ''' <param name="Connects">Specifies whether the SFTP client is currently connecting</param>
    ''' <param name="Address">An IP address</param>
    Public Sub SFTPInitiateShell(Optional Connects As Boolean = False, Optional Address As String = "")
        While True
            SyncLock SFTPCancelSync
                Try
                    'Complete initialization
                    If SFTPInitialized = False Then
                        Wdbg(DebugLevel.I, $"Completing initialization of SFTP: {SFTPInitialized}")
                        SFTPCurrDirect = HomePath
                        KernelEventManager.RaiseSFTPShellInitialized()
                        SwitchCancellationHandler(ShellCommandType.SFTPShell)
                        SFTPInitialized = True
                    End If

                    'Check if the shell is going to exit
                    If SFTPExit = True Then
                        Wdbg(DebugLevel.W, "Exiting shell...")
                        SFTPConnected = False
                        ClientSFTP?.Disconnect()
                        SFTPSite = ""
                        SFTPCurrDirect = ""
                        SFTPCurrentRemoteDir = ""
                        SFTPUser = ""
                        SFTPPass = ""
                        SFTPStrCmd = ""
                        SFTPExit = False
                        SFTPInitialized = False
                        SwitchCancellationHandler(LastShellType)
                        Exit Sub
                    End If

                    'Prompt for command
                    If DefConsoleOut IsNot Nothing Then
                        Console.SetOut(DefConsoleOut)
                    End If
                    If Not Connects Then
                        Wdbg(DebugLevel.I, "Preparing prompt...")
                        If SFTPConnected Then
                            Wdbg(DebugLevel.I, "SFTPShellPromptStyle = {0}", SFTPShellPromptStyle)
                            If SFTPShellPromptStyle = "" Then
                                Write("[", False, ColTypes.Gray) : Write("{0}", False, ColTypes.UserName, SFTPUser) : Write("@", False, ColTypes.Gray) : Write("{0}", False, ColTypes.HostName, SFTPSite) : Write("]{0}> ", False, ColTypes.Gray, SFTPCurrentRemoteDir)
                            Else
                                Dim ParsedPromptStyle As String = ProbePlaces(SFTPShellPromptStyle)
                                ParsedPromptStyle.ConvertVTSequences
                                Write(ParsedPromptStyle, False, ColTypes.Gray)
                            End If
                        Else
                            Write("{0}> ", False, ColTypes.Gray, SFTPCurrDirect)
                        End If
                    End If

                    'Run garbage collector
                    DisposeAll()

                    'Set input color
                    SetInputColor()

                    'Try to connect if IP address is specified.
                    If Connects Then
                        Wdbg(DebugLevel.I, $"Currently connecting to {Address} by ""sftp (address)""...")
                        SFTPStrCmd = $"connect {Address}"
                        Connects = False
                    Else
                        Wdbg(DebugLevel.I, "Normal shell")
                        SFTPStrCmd = Console.ReadLine()
                    End If
                    KernelEventManager.RaiseSFTPPreExecuteCommand(SFTPStrCmd)

                    'Parse command
                    If Not (SFTPStrCmd = Nothing Or SFTPStrCmd?.StartsWithAnyOf({" ", "#"})) Then
                        SFTPGetLine()
                        KernelEventManager.RaiseSFTPPostExecuteCommand(SFTPStrCmd)
                    End If
                Catch ex As Exception
                    WStkTrc(ex)
                    Throw New Exceptions.SFTPShellException(DoTranslation("There was an error in the SFTP shell:") + " {0}", ex, ex.Message)
                End Try
            End SyncLock
        End While
    End Sub

    ''' <summary>
    ''' Parses a command line from FTP shell
    ''' </summary>
    Public Sub SFTPGetLine()
        Dim words As String() = SFTPStrCmd.SplitEncloseDoubleQuotes(" ")
        Wdbg(DebugLevel.I, "Command: {0}", SFTPStrCmd)
        Wdbg(DebugLevel.I, $"Is the command found? {SFTPCommands.ContainsKey(words(0))}")
        If SFTPCommands.ContainsKey(words(0)) Then
            Wdbg(DebugLevel.I, "Command found.")
            Dim Params As New ExecuteCommandThreadParameters(SFTPStrCmd, ShellCommandType.SFTPShell, Nothing)
            SFTPStartCommandThread = New Thread(AddressOf ExecuteCommand) With {.Name = "SFTP Command Thread"}
            SFTPStartCommandThread.Start(Params)
            SFTPStartCommandThread.Join()
        ElseIf SFTPModCommands.Contains(words(0)) Then
            Wdbg(DebugLevel.I, "Mod command found.")
            ExecuteModCommand(SFTPStrCmd)
        ElseIf SFTPShellAliases.Keys.Contains(words(0)) Then
            Wdbg(DebugLevel.I, "Aliased command found.")
            SFTPStrCmd = SFTPStrCmd.Replace($"""{words(0)}""", words(0))
            ExecuteSFTPAlias(SFTPStrCmd)
        ElseIf Not SFTPStrCmd.StartsWith(" ") Then
            Wdbg(DebugLevel.E, "Command {0} not found.", SFTPStrCmd)
            Write(DoTranslation("SFTP message: The requested command {0} is not found. See 'help' for a list of available commands specified on SFTP shell."), True, ColTypes.Error, words(0))
        End If
    End Sub

End Module
