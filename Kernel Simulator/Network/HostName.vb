
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
        W("Write a new host name: ", "input")
        Dim newhost As String = System.Console.ReadLine()
        If (newhost = "") Then
            Wln("Blank host name.", "neutralText")
        ElseIf (newhost.Length <= 3) Then
            Wln("The host name length must be at least 4 characters.", "neutralText")
        ElseIf InStr(newhost, " ") > 0 Then
            Wln("Spaces are not allowed.", "neutralText")
        ElseIf (newhost.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
            Wln("Special characters are not allowed.", "neutralText")
        ElseIf (newhost = "q") Then
            Wln("Host name changing has been cancelled.", "neutralText")
        Else
            Wln("Changing from: {0} to {1}...", "neutralText", My.Settings.HostName, newhost)
            My.Settings.HostName = newhost
        End If

    End Sub

End Module
