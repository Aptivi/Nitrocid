
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

Module HostName

    Sub ChangeHostName()

        'Change host-name to custom name
        System.Console.Write("Write a new host name: ")
        Dim newhost As String
        System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
        newhost = System.Console.ReadLine()
        System.Console.ResetColor()
        If (newhost = "") Then
            System.Console.WriteLine("Blank host name.")
        ElseIf (newhost.Length <= 3) Then
            System.Console.WriteLine("The host name length must be at least 4 characters.")
        ElseIf InStr(newhost, " ") > 0 Then
            System.Console.WriteLine("Spaces are not allowed.")
        ElseIf (newhost.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
            System.Console.WriteLine("Special characters are not allowed.")
        ElseIf (newhost = "q") Then
            System.Console.WriteLine("Host name changing has been cancelled.")
        Else
            System.Console.WriteLine("Changing from: {0} to {1}...", My.Settings.HostName, newhost)
            My.Settings.HostName = newhost
        End If

    End Sub

End Module
