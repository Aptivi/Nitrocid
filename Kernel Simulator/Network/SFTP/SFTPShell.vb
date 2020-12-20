
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

    Public sftpstream As FtpWebRequest
    Public availsftpcmds As String() = {"currlocaldir", "currremotedir", "connect", "changelocaldir", "changeremotedir", "cdl",
                                       "cdr", "delete", "del", "disconnect", "download", "exit", "get", "help", "listlocal", "lsl",
                                       "listremote", "lsr", "put", "pwdl", "pwdr", "move", "mv", "copy", "cp", "upload", "quickconnect"}
    Public SFTPConnected As Boolean = False
    Private SFTPInitialized As Boolean = False
    Public sftpsite As String
    Public SFTPCurrDirect As String 'Current Local Directory
    Public SFTPCurrentRemoteDir As String 'Current Remote Directory
    Public SFTPUser As String
    Friend SFTPPass As String
    Private SFTPStrCmd As String
    Public sftpexit As Boolean = False
    Public SFTPModCommands As New ArrayList

    ''' <summary>
    ''' Initializes the SFTP shell
    ''' </summary>
    ''' <param name="Connects">Specifies whether the SFTP client is currently connecting</param>
    ''' <param name="Address">An IP address</param>
    Public Sub SFTPInitiateShell(Optional ByVal Connects As Boolean = False, Optional ByVal Address As String = "")
        While True
            'Complete initialization
            If SFTPInitialized = False Then
                Wdbg("I", $"Completing initialization of SFTP: {SFTPInitialized}")
                SFTPCurrDirect = paths("Home")
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
            If Not IsNothing(DefConsoleOut) Then
                Console.SetOut(DefConsoleOut)
            End If
            If Not Connects Then
                Wdbg("I", "Preparing prompt...")
                If SFTPConnected Then
                    W("[", False, ColTypes.Gray) : W("{0}", False, ColTypes.UserName, SFTPUser) : W("@", False, ColTypes.Gray) : W("{0}", False, ColTypes.HostName, sftpsite) : W("]{0} ", False, ColTypes.Gray, SFTPCurrentRemoteDir)
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
            If Not (SFTPStrCmd = Nothing Or SFTPStrCmd?.StartsWith(" ")) Then
                SFTPGetLine()
                EventManager.RaiseSFTPPostExecuteCommand(SFTPStrCmd)
            End If

            'When pressing CTRL+C on shell after command execution, it can generate another prompt without making newline, so fix this.
            If IsNothing(SFTPStrCmd) Then
                Console.WriteLine()
                Thread.Sleep(30) 'This is to fix race condition between SFTP shell initialization and starting the event handler thread
            End If
        End While
    End Sub

    ''' <summary>
    ''' Parses a command line from FTP shell
    ''' </summary>
    Public Sub SFTPGetLine()
        Dim words As String() = SFTPStrCmd.Split({" "c})
        Wdbg("I", $"Is the command found? {availsftpcmds.Contains(words(0))}")
        If availsftpcmds.Contains(words(0)) Then
            Wdbg("I", "Command found.")
            SFTPStartCommandThread = New Thread(AddressOf SFTPGetCommand.ExecuteCommand)
            SFTPStartCommandThread.Start(SFTPStrCmd)
            SFTPStartCommandThread.Join()
        ElseIf FTPModCommands.Contains(words(0)) Then
            Wdbg("I", "Mod command found.")
            ExecuteModCommand(SFTPStrCmd)
        ElseIf Not SFTPStrCmd.StartsWith(" ") Then
            Wdbg("E", "Command {0} not found.", SFTPStrCmd)
            W(DoTranslation("SFTP message: The requested command {0} is not found. See 'help' for a list of available commands specified on SFTP shell.", currentLang), True, ColTypes.Err, words(0))
        End If
    End Sub

End Module
