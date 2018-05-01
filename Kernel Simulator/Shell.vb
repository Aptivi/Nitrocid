
'    Kernel Simulator  Copyright (C) 2018  EoflaOE
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

Module Shell

    'Available Commands (availableCommands())
    Public ueshversion As String = "0.0.2.0"                'Current shell version
    Public strcommand As String                             'Written Command
    Public availableCommands() As String = {"help", "logout", "version", "currentdir", "list", "changedir", "cdir", "ls", "chdir", "read", "echo", "choice", _
                                            "lsdrivers", "shutdown", "reboot", "disco", "future-eyes-destroyer", "beep", "annoying-sound", "adduser", "chmotd", _
                                            "chhostname", "showmotd", "fed", "hwprobe", "ping", "lsnet", "lsnettree", "showtd", "chpwd", "sysinfo"}
    'For contributors: For each added command, you should also add a command in availableCommands array so there is no problems detecting your new command.

    Sub initializeShell()

        'Initialize Shell
        If signedinusrnm = "root" Then
            System.Console.Write("[")
            System.Console.ForegroundColor = ConsoleColor.Green
            System.Console.Write(signedinusrnm)
            System.Console.ResetColor()
            System.Console.Write("@")
            System.Console.ForegroundColor = ConsoleColor.DarkGreen
            System.Console.Write(My.Settings.HostName)
            System.Console.ResetColor()
            System.Console.Write("] # ")
        Else
            System.Console.Write("[")
            System.Console.ForegroundColor = ConsoleColor.Green
            System.Console.Write(signedinusrnm)
            System.Console.ResetColor()
            System.Console.Write("@")
            System.Console.ForegroundColor = ConsoleColor.DarkGreen
            System.Console.Write(My.Settings.HostName)
            System.Console.ResetColor()
            System.Console.Write("] $ ")
        End If
        System.Console.ForegroundColor = ConsoleColor.White
        strcommand = System.Console.ReadLine()
        System.Console.ResetColor()
        getLine()

    End Sub

    Sub getLine()

        'Reads command written by user
        For i As Integer = 0 To availableCommands.Count - 1
            If (availableCommands.Contains(strcommand)) Then
                GetCommand.ExecuteCommand(strcommand)
                initializeShell()
            ElseIf (strcommand = Nothing) Then
                initializeShell()
            Else
                System.Console.WriteLine("Shell message: The requested command {0} is not found.", strcommand)  'Fix bug
                initializeShell()
            End If
        Next

    End Sub

End Module
