
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

    Public strcommand As String
    Public availableCommands() As String = {"help", "logout", "version", "currentdir", "list", "changedir", "cdir", "ls", "chdir", "read", "echo", "choice", _
                                            "lsdrivers", "shutdown", "reboot", "disco", "future-eyes-destroyer", "beep", "annoying-sound", "adduser", "chmotd", _
                                            "chhostname", "showmotd"}

    Sub initializeShell()

        If signedinusrnm = "root" Then
            System.Console.Write(vbNewLine + "[" + signedinusrnm + "@" + My.Settings.HostName + "] #: ")
        Else
            System.Console.Write(vbNewLine + "[" + signedinusrnm + "@" + My.Settings.HostName + "] $: ")
        End If
        strcommand = System.Console.ReadLine()
        getLine()

    End Sub

    Sub getLine()

        For i As Integer = 0 To availableCommands.Count - 1
            If (availableCommands.Contains(strcommand)) Then
                GetCommand.ExecuteCommand(strcommand)
                initializeShell()
            Else
                System.Console.Write("Shell message: The requested command " + strcommand + " is not found.")
                initializeShell()
            End If
        Next

    End Sub

End Module
