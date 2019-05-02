
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

Public Module Filesystem

    'Variables
    'TODO: Change the file system to real file system in 0.0.5.13
    Public AvailableFiles() As String = {"loader", "libuesh.elb", "kernel", "login", "uesh"}
    Public currDir As String = "/"
    Public AvailableDirs As New List(Of String)

    Public Sub SetCurrDir(ByVal dir As String)
        If (AvailableDirs.Contains(dir)) Then
            currDir = "/" + dir
        ElseIf (dir = "") Then
            currDir = "/"
        Else
            Wln(DoTranslation("Cannot change directory to /{0} because that directory leads nowhere.", currentLang), "neutralText", dir)
        End If
    End Sub
    Public Sub ReadContents(ByVal filename As String)
        If (filename = "loader") Then

            Wln("Boot_Version = {0}", "neutralText", KernelVersion)

        ElseIf (filename = "libuesh.elb") Then

            Wln("[startelb]=ueshlib-<UESH Library Version {0}>-", "neutralText", KernelVersion)

        ElseIf (filename = "kernel") Then

            Wln(DoTranslation("Kernel process PID: 1", currentLang) + vbNewLine +
                DoTranslation("Priority: High", currentLang) + vbNewLine +
                DoTranslation("Importance: High, and shouldn't be killed.", currentLang), "neutralText")

        ElseIf (filename = "login") Then

            Wln(DoTranslation("Login process PID: 2", currentLang) + vbNewLine +
                DoTranslation("Priority: Normal", currentLang) + vbNewLine +
                DoTranslation("Importance: High, and shouldn't be killed.", currentLang), "neutralText")

        ElseIf (filename = "uesh") Then

            Wln(DoTranslation("UESH process PID: 3", currentLang) + vbNewLine +
                DoTranslation("Priority: Normal", currentLang) + vbNewLine +
                DoTranslation("Importance: Normal.", currentLang) + vbNewLine +
                DoTranslation("Short For: Unified Eofla SHell", currentLang), "neutralText")

        End If
    End Sub
    Public Sub Init()
        AvailableDirs.AddRange({"boot", "bin", "dev", "etc", "lib", "proc", "usr", "var"})
    End Sub
    Public Sub List(ByVal folder As String)
        If (folder = "bin" Or (folder.StartsWith("/") Or folder.StartsWith("..")) And folder.Substring(1) = "bin") Then
            Wln(String.Join("* ", availableCommands) + "*", "neutralText")
        ElseIf (folder = "boot" Or (folder.StartsWith("/") Or folder.StartsWith("..")) And folder.Substring(1) = "boot") Then
            Wln("loader~", "neutralText")
        ElseIf (folder = "dev" Or (folder.StartsWith("/") Or folder.StartsWith("..")) And folder.Substring(1) = "dev") Then
            Wln("{0}", "neutralText", slotsUsedName)
        ElseIf (folder = "etc" Or (folder.StartsWith("/") Or folder.StartsWith("..")) And folder.Substring(1) = "etc") Then
            Wln(DoTranslation("There is nothing.", currentLang), "neutralText")
        ElseIf (folder = "lib" Or (folder.StartsWith("/") Or folder.StartsWith("..")) And folder.Substring(1) = "lib") Then
            Wln("libuesh.elb", "neutralText")
        ElseIf (folder = "proc" Or (folder.StartsWith("/") Or folder.StartsWith("..")) And folder.Substring(1) = "proc") Then
            Wln("kernel~ login~ uesh~", "neutralText")
        ElseIf (folder = "usr" Or (folder.StartsWith("/") Or folder.StartsWith("..")) And folder.Substring(1) = "usr") Then
            Wln(DoTranslation("There is nothing.", currentLang), "neutralText")
        ElseIf (folder = "var" Or (folder.StartsWith("/") Or folder.StartsWith("..")) And folder.Substring(1) = "var") Then
            Wln(DoTranslation("There is nothing.", currentLang), "neutralText")
        ElseIf (folder = ".." Or folder = "/") Then
            Wln(String.Join(", ", AvailableDirs), "neutralText")
        Else
            Wln(DoTranslation("There is nothing.", currentLang), "neutralText")
        End If
    End Sub

End Module
