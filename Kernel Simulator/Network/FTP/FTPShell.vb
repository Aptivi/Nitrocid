
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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

Public Module FTPShell

    Public ftpstream As FtpWebRequest
    Public availftpcmds As String() = {"currlocaldir", "currremotedir", "connect", "changelocaldir", "changeremotedir", "cdl",
                                       "cdr", "delete", "del", "disconnect", "download", "exit", "get", "help", "listlocal", "lsl",
                                       "listremote", "lsr", "put", "pwdl", "pwdr", "rename", "ren", "upload"}
    Public connected As Boolean = False
    Private initialized As Boolean = False
    Public ftpsite As String
    Public currDirect As String 'Current Local Directory
    Public currentremoteDir As String 'Current Remote Directory
    Public user As String
    Public pass As String
    Private strcmd As String
    Public ftpexit As Boolean = False

    Public Sub InitiateShell(Optional ByVal Connects As Boolean = False, Optional ByVal Address As String = "")
        While True
            'Complete initialization
            If initialized = False Then
                Wdbg($"Completing initialization of FTP: {initialized}")
                currDirect = paths("Home")
                initialized = True
            End If

            'Check if the shell is going to exit
            If ftpexit = True Then
                Wdbg("Exiting shell...")
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
                Exit Sub
            End If

            'Prompt for command
            If Not Connects Then
                Wdbg("Preparing prompt...")
                If connected Then
                    W("[", False, ColTypes.Gray) : W("{0}", False, ColTypes.UserName, user) : W("@", False, ColTypes.Gray) : W("{0}", False, ColTypes.HostName, ftpsite) : W("]{0} ", False, ColTypes.Gray, currentremoteDir)
                Else
                    W("{0}> ", False, ColTypes.Gray, currDirect)
                End If
            End If

            'Run garbage collector
            DisposeAll()

            'Set input color
            If ColoredShell = True Then Console.ForegroundColor = CType(inputColor, ConsoleColor)
            If Connects Then
                Wdbg($"Currently connecting to {Address} by ""ftp (address)""...")
                strcmd = $"connect {Address}"
                Connects = False
            Else
                Wdbg("Normal shell")
                strcmd = Console.ReadLine()
            End If

            'Parse command
            If Not (strcmd = Nothing Or strcmd.StartsWith(" ")) Then GetLine()
        End While
    End Sub

    Public Sub GetLine()
        Dim words As String() = strcmd.Split({" "c})
        Wdbg($"Is the command found? {availftpcmds.Contains(words(0))}")
        If availftpcmds.Contains(words(0)) Then
            FTPGetCommand.ExecuteCommand(strcmd)
        Else
            W(DoTranslation("FTP message: The requested command {0} is not found. See 'help' for a list of available commands specified on FTP shell.", currentLang), True, ColTypes.Neutral, strcmd)
        End If
    End Sub

End Module
