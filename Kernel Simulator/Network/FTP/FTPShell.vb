
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

    Public ReadOnly FTPCommands As New Dictionary(Of String, CommandInfo) From {{"connect", New CommandInfo("connect", ShellCommandType.FTPShell, "Connects to an FTP server (it must start with ""ftp://"" or ""ftps://"")", {"<server>"}, True, 1, New FTP_ConnectCommand)},
                                                                                {"cdl", New CommandInfo("cdl", ShellCommandType.FTPShell, "Changes local directory to download to or upload from", {"<directory>"}, True, 1, New FTP_CdlCommand)},
                                                                                {"cdr", New CommandInfo("cdr", ShellCommandType.FTPShell, "Changes remote directory to download from or upload to", {"<directory>"}, True, 1, New FTP_CdrCommand)},
                                                                                {"cp", New CommandInfo("cp", ShellCommandType.FTPShell, "Copies file or directory to another file or directory.", {"<sourcefileordir> <targetfileordir>"}, True, 2, New FTP_CpCommand)},
                                                                                {"del", New CommandInfo("del", ShellCommandType.FTPShell, "Deletes remote file from server", {"<file>"}, True, 1, New FTP_DelCommand)},
                                                                                {"disconnect", New CommandInfo("disconnect", ShellCommandType.FTPShell, "Disconnects from server", {"[-f]"}, False, 0, New FTP_DisconnectCommand, False, False, False, False, False, New Action(AddressOf (New FTP_DisconnectCommand).HelpHelper))},
                                                                                {"execute", New CommandInfo("execute", ShellCommandType.FTPShell, "Executes an FTP server command", {"<command>"}, True, 1, New FTP_ExecuteCommand)},
                                                                                {"exit", New CommandInfo("exit", ShellCommandType.FTPShell, "Exits FTP shell and returns to kernel", {}, False, 0, New FTP_ExitCommand)},
                                                                                {"get", New CommandInfo("get", ShellCommandType.FTPShell, "Downloads remote file to local directory using binary or text", {"<file> [output]"}, True, 1, New ZipShell_GetCommand)},
                                                                                {"getfolder", New CommandInfo("getfolder", ShellCommandType.FTPShell, "Downloads remote folder to local directory using binary or text", {"<folder> [outputfolder]"}, True, 1, New FTP_GetFolderCommand)},
                                                                                {"help", New CommandInfo("help", ShellCommandType.FTPShell, "Shows help screen", {"[command]"}, False, 0, New FTP_HelpCommand)},
                                                                                {"info", New CommandInfo("info", ShellCommandType.FTPShell, "FTP server information", {}, False, 0, New FTP_InfoCommand)},
                                                                                {"lsl", New CommandInfo("lsl", ShellCommandType.FTPShell, "Lists local directory", {"[-showdetails|-suppressmessages] [dir]"}, False, 0, New FTP_LslCommand, False, False, False, False, False, New Action(AddressOf (New FTP_LslCommand).HelpHelper))},
                                                                                {"lsr", New CommandInfo("lsr", ShellCommandType.FTPShell, "Lists remote directory", {"[-showdetails] [dir]"}, False, 0, New FTP_LsrCommand, False, False, False, False, False, New Action(AddressOf (New FTP_LsrCommand).HelpHelper))},
                                                                                {"mv", New CommandInfo("mv", ShellCommandType.FTPShell, "Moves file or directory to another file or directory. You can also use that to rename files.", {"<sourcefileordir> <targetfileordir>"}, True, 2, New FTP_MvCommand)},
                                                                                {"put", New CommandInfo("put", ShellCommandType.FTPShell, "Uploads local file to remote directory using binary or text", {"<file> [output]"}, True, 1, New FTP_PutCommand)},
                                                                                {"putfolder", New CommandInfo("putfolder", ShellCommandType.FTPShell, "Uploads local folder to remote directory using binary or text", {"<folder> [outputfolder]"}, True, 1, New FTP_PutFolderCommand)},
                                                                                {"pwdl", New CommandInfo("pwdl", ShellCommandType.FTPShell, "Gets current local directory", {}, False, 0, New FTP_PwdlCommand)},
                                                                                {"pwdr", New CommandInfo("pwdr", ShellCommandType.FTPShell, "Gets current remote directory", {}, False, 0, New FTP_PwdrCommand)},
                                                                                {"perm", New CommandInfo("perm", ShellCommandType.FTPShell, "Sets file permissions. This is supported only on FTP servers that run Unix.", {"<file> <permnumber>"}, True, 2, New FTP_PermCommand)},
                                                                                {"quickconnect", New CommandInfo("quickconnect", ShellCommandType.FTPShell, "Uses information from Speed Dial to connect to any network quickly", {}, False, 0, New FTP_QuickConnectCommand)},
                                                                                {"sumfile", New CommandInfo("sumfile", ShellCommandType.FTPShell, "Calculates file sums.", {"<file> <MD5/SHA1/SHA256/SHA512/CRC>"}, True, 2, New FTP_SumFileCommand)},
                                                                                {"sumfiles", New CommandInfo("sumfiles", ShellCommandType.FTPShell, "Calculates sums of files in specified directory.", {"<file> <MD5/SHA1/SHA256/SHA512/CRC>"}, True, 2, New FTP_SumFilesCommand)},
                                                                                {"type", New CommandInfo("type", ShellCommandType.FTPShell, "Sets the type for this session", {"<a/b>"}, True, 1, New FTP_TypeCommand)}}
    Public FtpConnected As Boolean
    Public FtpSite As String
    Public FtpCurrentDirectory As String
    Public FtpCurrentRemoteDir As String
    Public FtpUser As String
    Public FTPModCommands As New ArrayList
    Public FTPShellPromptStyle As String = ""
    Public ClientFTP As FtpClient
    Public FtpShowDetailsInList As Boolean = True
    Public FtpUserPromptStyle As String = ""
    Public FtpPassPromptStyle As String = ""
    Public FtpUseFirstProfile As Boolean
    Public FtpNewConnectionsToSpeedDial As Boolean = True
    Public FtpTryToValidateCertificate As Boolean = True
    Public FtpRecursiveHashing As Boolean
    Public FtpShowMotd As Boolean = True
    Public FtpAlwaysAcceptInvalidCerts As Boolean
    Public FtpVerifyRetryAttempts As Integer = 3
    Public FtpConnectTimeout As Integer = 15000
    Public FtpDataConnectTimeout As Integer = 15000
    Public FtpProtocolVersions As FtpIpVersion = FtpIpVersion.ANY
    Friend FtpPass As String
    Friend FtpExit As Boolean
    Private FtpCommand As String
    Private FtpInitialized As Boolean

    ''' <summary>
    ''' Initializes the FTP shell
    ''' </summary>
    ''' <param name="Connects">Specifies whether the FTP client is currently connecting</param>
    ''' <param name="Address">An IP address</param>
    Public Sub InitiateShell(Optional Connects As Boolean = False, Optional Address As String = "")
        While True
            SyncLock FTPCancelSync
                Try
                    'Complete initialization
                    If FtpInitialized = False Then
                        Wdbg(DebugLevel.I, $"Completing initialization of FTP: {FtpInitialized}")
                        FtpTrace.AddListener(New FTPTracer)
                        FtpTrace.LogUserName = FTPLoggerUsername
                        FtpTrace.LogPassword = False 'Don't remove this, make a config entry for it, or set it to True! It will introduce security problems.
                        FtpTrace.LogIP = FTPLoggerIP
                        FtpCurrentDirectory = HomePath
                        Kernel.EventManager.RaiseFTPShellInitialized()
                        SwitchCancellationHandler(ShellCommandType.FTPShell)
                        FtpInitialized = True
                    End If

                    'Check if the shell is going to exit
                    If FtpExit = True Then
                        Wdbg(DebugLevel.W, "Exiting shell...")
                        FtpConnected = False
                        ClientFTP?.Disconnect()
                        FtpSite = ""
                        FtpCurrentDirectory = ""
                        FtpCurrentRemoteDir = ""
                        FtpUser = ""
                        FtpPass = ""
                        FtpCommand = ""
                        FtpExit = False
                        FtpInitialized = False
                        SwitchCancellationHandler(LastShellType)
                        Exit Sub
                    End If

                    'Prompt for command
                    If DefConsoleOut IsNot Nothing Then
                        Console.SetOut(DefConsoleOut)
                    End If
                    If Not Connects Then
                        Wdbg(DebugLevel.I, "Preparing prompt...")
                        If FtpConnected Then
                            Wdbg(DebugLevel.I, "FTPShellPromptStyle = {0}", FTPShellPromptStyle)
                            If FTPShellPromptStyle = "" Then
                                Write("[", False, ColTypes.Gray) : Write("{0}", False, ColTypes.UserName, FtpUser) : Write("@", False, ColTypes.Gray) : Write("{0}", False, ColTypes.HostName, FtpSite) : Write("]{0}> ", False, ColTypes.Gray, FtpCurrentRemoteDir)
                            Else
                                Dim ParsedPromptStyle As String = ProbePlaces(FTPShellPromptStyle)
                                ParsedPromptStyle.ConvertVTSequences
                                Write(ParsedPromptStyle, False, ColTypes.Gray)
                            End If
                        Else
                            Write("{0}> ", False, ColTypes.Gray, FtpCurrentDirectory)
                        End If
                    End If

                    'Run garbage collector
                    DisposeAll()

                    'Set input color
                    SetInputColor()

                    'Try to connect if IP address is specified.
                    If Connects Then
                        Wdbg(DebugLevel.I, $"Currently connecting to {Address} by ""ftp (address)""...")
                        FtpCommand = $"connect {Address}"
                        Connects = False
                    Else
                        Wdbg(DebugLevel.I, "Normal shell")
                        FtpCommand = Console.ReadLine()
                    End If
                    Kernel.EventManager.RaiseFTPPreExecuteCommand(FtpCommand)

                    'Parse command
                    If Not (FtpCommand = Nothing Or FtpCommand?.StartsWithAnyOf({" ", "#"})) Then
                        FTPGetLine()
                        Kernel.EventManager.RaiseFTPPostExecuteCommand(FtpCommand)
                    End If
                Catch ex As Exception
                    WStkTrc(ex)
                    Throw New Exceptions.FTPShellException(DoTranslation("There was an error in the FTP shell:") + " {0}", ex, ex.Message)
                End Try
            End SyncLock
        End While
    End Sub

    ''' <summary>
    ''' Parses a command line from FTP shell
    ''' </summary>
    Public Sub FTPGetLine()
        Dim words As String() = FtpCommand.SplitEncloseDoubleQuotes(" ")
        Wdbg(DebugLevel.I, $"Is the command found? {FTPCommands.ContainsKey(words(0))}")
        If FTPCommands.ContainsKey(words(0)) Then
            Wdbg(DebugLevel.I, "Command found.")
            Dim Params As New ExecuteCommandThreadParameters(FtpCommand, ShellCommandType.FTPShell, Nothing)
            FTPStartCommandThread = New Thread(AddressOf ExecuteCommand) With {.Name = "FTP Command Thread"}
            FTPStartCommandThread.Start(Params)
            FTPStartCommandThread.Join()
        ElseIf FTPModCommands.Contains(words(0)) Then
            Wdbg(DebugLevel.I, "Mod command found.")
            ExecuteModCommand(FtpCommand)
        ElseIf FTPShellAliases.Keys.Contains(words(0)) Then
            Wdbg(DebugLevel.I, "FTP shell alias command found.")
            FtpCommand = FtpCommand.Replace($"""{words(0)}""", words(0))
            ExecuteFTPAlias(FtpCommand)
        ElseIf Not FtpCommand.StartsWith(" ") Then
            Wdbg(DebugLevel.E, "Command {0} not found.", FtpCommand)
            Write(DoTranslation("FTP message: The requested command {0} is not found. See 'help' for a list of available commands specified on FTP shell."), True, ColTypes.Error, words(0))
        End If
    End Sub

End Module
