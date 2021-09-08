
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

    Public ReadOnly FTPCommands As New Dictionary(Of String, CommandInfo) From {{"connect", New CommandInfo("connect", ShellCommandType.FTPShell, "Connects to an FTP server (it must start with ""ftp://"" or ""ftps://"")", "<server>", True, 1)},
                                                                                {"cdl", New CommandInfo("cdl", ShellCommandType.FTPShell, "Changes local directory to download to or upload from", "<directory>", True, 1)},
                                                                                {"cdr", New CommandInfo("cdr", ShellCommandType.FTPShell, "Changes remote directory to download from or upload to", "<directory>", True, 1)},
                                                                                {"cp", New CommandInfo("cp", ShellCommandType.FTPShell, "Copies file or directory to another file or directory.", "<sourcefileordir> <targetfileordir>", True, 2)},
                                                                                {"del", New CommandInfo("del", ShellCommandType.FTPShell, "Deletes remote file from server", "<file>", True, 1)},
                                                                                {"disconnect", New CommandInfo("disconnect", ShellCommandType.FTPShell, "Disconnects from server", "", False, 0)},
                                                                                {"exit", New CommandInfo("exit", ShellCommandType.FTPShell, "Exits FTP shell and returns to kernel", "", False, 0)},
                                                                                {"get", New CommandInfo("get", ShellCommandType.FTPShell, "Downloads remote file to local directory using binary or text", "<file> [output]", True, 1)},
                                                                                {"getfolder", New CommandInfo("getfolder", ShellCommandType.FTPShell, "Downloads remote folder to local directory using binary or text", "<folder> [outputfolder]", True, 1)},
                                                                                {"help", New CommandInfo("help", ShellCommandType.FTPShell, "Shows help screen", "[command]", False, 0)},
                                                                                {"lsl", New CommandInfo("lsl", ShellCommandType.FTPShell, "Lists local directory", "[dir]", False, 0)},
                                                                                {"lsr", New CommandInfo("lsr", ShellCommandType.FTPShell, "Lists remote directory", "[dir]", False, 0)},
                                                                                {"put", New CommandInfo("put", ShellCommandType.FTPShell, "Uploads local file to remote directory using binary or text", "<file>", True, 1)},
                                                                                {"putfolder", New CommandInfo("putfolder", ShellCommandType.FTPShell, "Uploads local folder to remote directory using binary or text", "<folder>", True, 1)},
                                                                                {"pwdl", New CommandInfo("pwdl", ShellCommandType.FTPShell, "Gets current local directory", "", False, 0)},
                                                                                {"pwdr", New CommandInfo("pwdr", ShellCommandType.FTPShell, "Gets current remote directory", "", False, 0)},
                                                                                {"mv", New CommandInfo("mv", ShellCommandType.FTPShell, "Moves file or directory to another file or directory. You can also use that to rename files.", "<sourcefileordir> <targetfileordir>", True, 2)},
                                                                                {"perm", New CommandInfo("perm", ShellCommandType.FTPShell, "Sets file permissions. This is supported only on FTP servers that run Unix.", "<file> <permnumber>", True, 2)},
                                                                                {"type", New CommandInfo("type", ShellCommandType.FTPShell, "Sets the type for this session", "<a/b>", True, 1)},
                                                                                {"quickconnect", New CommandInfo("quickconnect", ShellCommandType.FTPShell, "Uses information from Speed Dial to connect to any network quickly", "", False, 0)}}
    Public FtpConnected As Boolean
    Private FtpInitialized As Boolean
    Public ftpsite As String
    Public FtpCurrentDirectory As String
    Public FtpCurrentRemoteDir As String
    Public FtpUser As String
    Friend FtpPass As String
    Private FtpCommand As String
    Public ftpexit As Boolean
    Public FTPModCommands As New ArrayList
    Public FTPShellPromptStyle As String = ""
    Public ClientFTP As FtpClient

    ''' <summary>
    ''' Initializes the FTP shell
    ''' </summary>
    ''' <param name="Connects">Specifies whether the FTP client is currently connecting</param>
    ''' <param name="Address">An IP address</param>
    Public Sub InitiateShell(Optional Connects As Boolean = False, Optional Address As String = "")
        While True
            Try
                'Complete initialization
                If FtpInitialized = False Then
                    Wdbg("I", $"Completing initialization of FTP: {FtpInitialized}")
                    FtpTrace.AddListener(New FTPTracer)
                    FtpTrace.LogUserName = FTPLoggerUsername
                    FtpTrace.LogPassword = False 'Don't remove this, make a config entry for it, or set it to True! It will introduce security problems.
                    FtpTrace.LogIP = FTPLoggerIP
                    FtpCurrentDirectory = GetOtherPath(OtherPathType.Home)
                    EventManager.RaiseFTPShellInitialized()

                    'This is the workaround for a bug in .NET Framework regarding Console.CancelKeyPress event. More info can be found below:
                    'https://stackoverflow.com/a/22717063/6688914
                    AddHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
                    FtpInitialized = True
                End If

                'Check if the shell is going to exit
                If ftpexit = True Then
                    Wdbg("W", "Exiting shell...")
                    FtpConnected = False
                    ClientFTP?.Disconnect()
                    ftpsite = ""
                    FtpCurrentDirectory = ""
                    FtpCurrentRemoteDir = ""
                    FtpUser = ""
                    FtpPass = ""
                    FtpCommand = ""
                    ftpexit = False
                    FtpInitialized = False
                    AddHandler Console.CancelKeyPress, AddressOf CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                    Exit Sub
                End If

                'Prompt for command
                If DefConsoleOut IsNot Nothing Then
                    Console.SetOut(DefConsoleOut)
                End If
                If Not Connects Then
                    Wdbg("I", "Preparing prompt...")
                    If FtpConnected Then
                        Wdbg("I", "FTPShellPromptStyle = {0}", FTPShellPromptStyle)
                        If FTPShellPromptStyle = "" Then
                            W("[", False, ColTypes.Gray) : W("{0}", False, ColTypes.UserName, FtpUser) : W("@", False, ColTypes.Gray) : W("{0}", False, ColTypes.HostName, ftpsite) : W("]{0}> ", False, ColTypes.Gray, FtpCurrentRemoteDir)
                        Else
                            Dim ParsedPromptStyle As String = ProbePlaces(FTPShellPromptStyle)
                            ParsedPromptStyle.ConvertVTSequences
                            W(ParsedPromptStyle, False, ColTypes.Gray)
                        End If
                    Else
                        W("{0}> ", False, ColTypes.Gray, FtpCurrentDirectory)
                    End If
                End If

                'Run garbage collector
                DisposeAll()

                'Set input color
                SetInputColor()

                'Try to connect if IP address is specified.
                If Connects Then
                    Wdbg("I", $"Currently connecting to {Address} by ""ftp (address)""...")
                    FtpCommand = $"connect {Address}"
                    Connects = False
                Else
                    Wdbg("I", "Normal shell")
                    FtpCommand = Console.ReadLine()
                End If
                EventManager.RaiseFTPPreExecuteCommand(FtpCommand)

                'Parse command
                If Not (FtpCommand = Nothing Or FtpCommand?.StartsWithAnyOf({" ", "#"})) Then
                    FTPGetLine()
                    EventManager.RaiseFTPPostExecuteCommand(FtpCommand)
                End If

                'This is to fix race condition between FTP shell initialization and starting the event handler thread
                If FtpCommand Is Nothing Then
                    Thread.Sleep(30)
                End If
            Catch ex As Exception
                WStkTrc(ex)
                Throw New Exceptions.FTPShellException(DoTranslation("There was an error in the FTP shell:") + " {0}", ex, ex.Message)
            End Try
        End While
    End Sub

    ''' <summary>
    ''' Parses a command line from FTP shell
    ''' </summary>
    Public Sub FTPGetLine()
        Dim words As String() = FtpCommand.SplitEncloseDoubleQuotes(" ")
        Wdbg("I", $"Is the command found? {FTPCommands.ContainsKey(words(0))}")
        If FTPCommands.ContainsKey(words(0)) Then
            Wdbg("I", "Command found.")
            FTPStartCommandThread = New Thread(AddressOf FTPGetCommand.ExecuteCommand) With {.Name = "FTP Command Thread"}
            FTPStartCommandThread.Start(FtpCommand)
            FTPStartCommandThread.Join()
        ElseIf FTPModCommands.Contains(words(0)) Then
            Wdbg("I", "Mod command found.")
            ExecuteModCommand(FtpCommand)
        ElseIf FTPShellAliases.Keys.Contains(words(0)) Then
            Wdbg("I", "FTP shell alias command found.")
            FtpCommand = FtpCommand.Replace($"""{words(0)}""", words(0))
            ExecuteFTPAlias(FtpCommand)
        ElseIf Not FtpCommand.StartsWith(" ") Then
            Wdbg("E", "Command {0} not found.", FtpCommand)
            W(DoTranslation("FTP message: The requested command {0} is not found. See 'help' for a list of available commands specified on FTP shell."), True, ColTypes.Error, words(0))
        End If
    End Sub

    ''' <summary>
    ''' Executes the FTP shell alias
    ''' </summary>
    ''' <param name="aliascmd">Aliased command with arguments</param>
    Sub ExecuteFTPAlias(aliascmd As String)
        Dim FirstWordCmd As String = aliascmd.SplitEncloseDoubleQuotes(" ")(0)
        Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, FTPShellAliases(FirstWordCmd))
        Wdbg("I", "Actual command: {0}", actualCmd)
        FTPStartCommandThread = New Thread(AddressOf FTPGetCommand.ExecuteCommand) With {.Name = "FTP Command Thread"}
        FTPStartCommandThread.Start(actualCmd)
        FTPStartCommandThread.Join()
    End Sub

End Module
