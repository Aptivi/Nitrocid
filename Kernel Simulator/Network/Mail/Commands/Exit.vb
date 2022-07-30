
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

Namespace Network.Mail.Commands
    Class Mail_ExitCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            KillShell()
            Write(DoTranslation("Do you want to keep connected?") + " <y/n> ", False, ColTypes.Question)
            SetConsoleColor(InputColor)
            Dim Answer As Char = Console.ReadKey.KeyChar
            Console.WriteLine()
            If Answer = "y" Then
                KeepAlive = True
            ElseIf Answer = "n" Then
                KeepAlive = False
            Else
                Write(DoTranslation("Invalid choice. Assuming no..."), True, ColTypes.Input)
            End If
        End Sub

    End Class
End Namespace