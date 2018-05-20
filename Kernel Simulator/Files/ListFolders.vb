
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

    Public AvailableDirs As New List(Of String)

    Sub list(ByVal folder As String)

        If (folder = "bin" Or (folder.StartsWith("/") Or folder.StartsWith("..")) And folder.Substring(1) = "bin") Then

            Wln(String.Join("* ", availableCommands) + "*", "neutralText")

        ElseIf (folder = "boot" Or (folder.StartsWith("/") Or folder.StartsWith("..")) And folder.Substring(1) = "boot") Then

            Wln("loader~", "neutralText")

        ElseIf (folder = "dev" Or (folder.StartsWith("/") Or folder.StartsWith("..")) And folder.Substring(1) = "dev") Then

            Wln("{0}hdpack", "neutralText", slotsUsedName)

        ElseIf (folder = "etc" Or (folder.StartsWith("/") Or folder.StartsWith("..")) And folder.Substring(1) = "etc") Then

            Wln("There is nothing.", "neutralText")

        ElseIf (folder = "lib" Or (folder.StartsWith("/") Or folder.StartsWith("..")) And folder.Substring(1) = "lib") Then

            Wln("libuesh.elb", "neutralText")

        ElseIf (folder = "proc" Or (folder.StartsWith("/") Or folder.StartsWith("..")) And folder.Substring(1) = "proc") Then

            Wln("kernel~ login~ uesh~", "neutralText")

        ElseIf (folder = "usr" Or (folder.StartsWith("/") Or folder.StartsWith("..")) And folder.Substring(1) = "usr") Then

            Wln("There is nothing.", "neutralText")

        ElseIf (folder = "var" Or (folder.StartsWith("/") Or folder.StartsWith("..")) And folder.Substring(1) = "var") Then

            Wln("There is nothing.", "neutralText")

        ElseIf (folder = ".." Or folder = "/") Then

            Wln(String.Join(", ", AvailableDirs), "neutralText")

        Else

            Wln("There is nothing.", "neutralText")

        End If

    End Sub

End Module
