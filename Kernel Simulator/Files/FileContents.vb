
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

Module FileContents

    Public AvailableFiles() As String = {"loader", "hdpack", "libuesh.elb", "kernel", "login", "uesh"}

    Sub readContents(ByVal filename As String)

        If (filename = "loader") Then

            Wln("Boot_Version = {0}", "neutralText", KernelVersion)

        ElseIf (filename = "hdpack") Then

            Hddinfo(False, False)

        ElseIf (filename = "libuesh.elb") Then

            Wln("[startelb]=ueshlib-<UESH Library Version 0.0.3>-", "neutralText")

        ElseIf (filename = "kernel") Then

            Wln("Kernel process PID: 1" + vbNewLine + _
                                     "Priority: High" + vbNewLine + _
                                     "Importance: High, and shouldn't be killed.", "neutralText")

        ElseIf (filename = "login") Then

            Wln("Login process PID: 2" + vbNewLine + _
                                     "Priority: Normal" + vbNewLine + _
                                     "Importance: High, and shouldn't be killed.", "neutralText")

        ElseIf (filename = "uesh") Then

            Wln("UESH process PID: 3" + vbNewLine + _
                                     "Priority: Normal" + vbNewLine + _
                                     "Importance: Normal." + vbNewLine + _
                                     "Short For: Unified Eofla SHell", "neutralText")

        End If

    End Sub

End Module
