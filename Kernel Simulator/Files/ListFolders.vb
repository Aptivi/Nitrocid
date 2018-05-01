
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

Module ListFolders

    Public AvailableDirs() As String = {"boot", "bin", "dev", "etc", "lib", "proc", "usr", "var"}

    Sub list(ByVal folder As String)

        If (folder = "bin") Then

            System.Console.WriteLine(String.Join("* ", availableCommands) + "*")

        ElseIf (folder = "boot") Then

            System.Console.WriteLine("loader~")

        ElseIf (folder = "dev") Then

            System.Console.WriteLine("{0}hdpack", slotsUsedName)

        ElseIf (folder = "etc") Then

            System.Console.WriteLine("There is nothing.")

        ElseIf (folder = "lib") Then

            System.Console.WriteLine("libuesh.elb")

        ElseIf (folder = "proc") Then

            System.Console.WriteLine("kernel~ login~ uesh~")

        ElseIf (folder = "usr") Then

            System.Console.WriteLine("There is nothing.")

        ElseIf (folder = "var") Then

            System.Console.WriteLine("There is nothing.")

        End If

    End Sub

End Module
