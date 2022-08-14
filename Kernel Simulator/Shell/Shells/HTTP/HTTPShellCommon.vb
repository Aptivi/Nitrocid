
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

Imports System.Net.Http
Imports KS.Shell.Shells.HTTP.Commands

Namespace Shell.Shells.HTTP
    Public Module HTTPShellCommon

        Public ReadOnly HTTPCommands As New Dictionary(Of String, CommandInfo) From {
            {"delete", New CommandInfo("delete", ShellType.HTTPShell, "Deletes content from HTTP server", New CommandArgumentInfo({"<request>"}, True, 1), New HTTP_DeleteCommand)},
            {"get", New CommandInfo("get", ShellType.HTTPShell, "Gets the response from the HTTP server using the specified request", New CommandArgumentInfo({"<request>"}, True, 1), New HTTP_GetCommand)},
            {"getstring", New CommandInfo("getstring", ShellType.HTTPShell, "Gets the string from the HTTP server using the specified request", New CommandArgumentInfo({"<request>"}, True, 1), New HTTP_GetStringCommand)},
            {"setsite", New CommandInfo("setsite", ShellType.HTTPShell, "Sets the HTTP site. Must be a valid URI.", New CommandArgumentInfo({"<uri>"}, True, 1), New HTTP_SetSiteCommand)}
        }
        Public HTTPSite As String
        Public HTTPShellPromptStyle As String = ""
        Public ClientHTTP As New HttpClient()
        Friend ReadOnly HTTPModCommands As New Dictionary(Of String, CommandInfo)

        ''' <summary>
        ''' See if the HTTP shell is connected
        ''' </summary>
        Public ReadOnly Property HTTPConnected As Boolean
            Get
                Return Not String.IsNullOrEmpty(HTTPSite)
            End Get
        End Property

    End Module
End Namespace
