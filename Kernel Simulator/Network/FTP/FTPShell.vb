
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

Public Module FTPShell

    Public ftpstream As FtpWebRequest
    Public availftpcmds As String() = {"currlocaldir", "currremotedir", "connect", "changelocaldir", "changeremotedir", "cdl",
                                       "cdr", "delete", "del", "disconnect", "download", "exit", "get", "help", "listlocal", "lsl",
                                       "listremote", "lsr", "put", "pwdl", "pwdr", "move", "mv", "copy", "cp", "upload", "quickconnect"}
    Public connected As Boolean = False
    Private initialized As Boolean = False
    Public ftpsite As String
    Public currDirect As String 'Current Local Directory
    Public currentremoteDir As String 'Current Remote Directory
    Public user As String
    Public pass As String
    Private strcmd As String
    Public ftpexit As Boolean = False
    Public FTPModCommands As New ArrayList

    ''' <summary>
    ''' Initializes the FTP shell
    ''' </summary>
    ''' <param name="Connects">Specifies whether the FTP client is currently connecting</param>
    ''' <param name="Address">An IP address</param>
    Public Sub InitiateShell(Optional ByVal Connects As Boolean = False, Optional ByVal Address As String = "")
        While True
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
                    W("[", False, ColTypes.Gray) : W("{0}", False, ColTypes.UserName, user) : W("@", False, ColTypes.Gray) : W("{0}", False, ColTypes.HostName, ftpsite) : W("]{0} ", False, ColTypes.Gray, currentremoteDir)
                Else
                    W("{0}> ", False, ColTypes.Gray, currDirect)
                End If
            End If

            'Run garbage collector
            DisposeAll()

            'Set input color
            Dim esc As Char = GetEsc()
            If ColoredShell = True Then Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
            If Connects Then
                Wdbg("I", $"Currently connecting to {Address} by ""ftp (address)""...")
                strcmd = $"connect {Address}"
                Connects = False
            Else
                Wdbg("I", "Normal shell")
                strcmd = Console.ReadLine()
            End If
            EventManager.RaiseFTPPreExecuteCommand()

            'Parse command
            If Not (strcmd = Nothing Or strcmd?.StartsWith(" ")) Then
                FTPGetLine()
                EventManager.RaiseFTPPostExecuteCommand()
            End If

            'When pressing CTRL+C on shell after command execution, it can generate another prompt without making newline, so fix this.
            If IsNothing(strcmd) Then
                Console.WriteLine()
                Thread.Sleep(30) 'This is to fix race condition between FTP shell initialization and starting the event handler thread
            End If
        End While
    End Sub

    ''' <summary>
    ''' Parses a command line from FTP shell
    ''' </summary>
    Public Sub FTPGetLine()
        Dim words As String() = strcmd.Split({" "c})
        Wdbg("I", $"Is the command found? {availftpcmds.Contains(words(0))}")
        If availftpcmds.Contains(words(0)) Then
            Wdbg("I", "Command found.")
            FTPStartCommandThread = New Thread(AddressOf FTPGetCommand.ExecuteCommand)
            FTPStartCommandThread.Start(strcmd)
            FTPStartCommandThread.Join()
        ElseIf FTPModCommands.Contains(words(0)) Then
            Wdbg("I", "Mod command found.")
            ExecuteModCommand(strcmd)
        ElseIf Not strcmd.StartsWith(" ") Then
            Wdbg("E", "Command {0} not found.", strcmd)
            W(DoTranslation("FTP message: The requested command {0} is not found. See 'help' for a list of available commands specified on FTP shell.", currentLang), True, ColTypes.Err, words(0))
        End If
    End Sub

End Module
