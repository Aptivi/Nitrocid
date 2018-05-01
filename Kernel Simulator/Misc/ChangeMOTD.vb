
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

Module ChangeMOTD

    Sub ChangeMessage()

        'New message of the day
        System.Console.Write("Write a new Message Of The Day: ")
        Dim newmotd As String
        System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
        newmotd = System.Console.ReadLine()
        System.Console.ResetColor()
        If (newmotd = "") Then
            System.Console.WriteLine("Blank message of the day.")
        ElseIf (newmotd = "q") Then
            System.Console.WriteLine("MOTD changing has been cancelled.")
        Else
            System.Console.Write("Changing MOTD...")
            My.Settings.MOTD = newmotd
            System.Console.WriteLine(" Done!" + vbNewLine + "Please log-out, or use 'showmotd' to see the changes")
        End If

    End Sub

End Module
