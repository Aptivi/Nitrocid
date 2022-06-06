
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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
Imports KS.Network.HTTP.Commands

Namespace Network.HTTP
    Public Module HTTPShellCommon

        Public ReadOnly HTTPCommands As New Dictionary(Of String, CommandInfo) From {{"delete", New CommandInfo("delete", ShellType.HTTPShell, "Deletes content from HTTP server", {"<request>"}, True, 1, New HTTP_DeleteCommand)},
                                                                                 {"exit", New CommandInfo("exit", ShellType.HTTPShell, "Exits HTTP shell and returns to kernel", {}, False, 0, New HTTP_ExitCommand)},
                                                                                 {"get", New CommandInfo("get", ShellType.HTTPShell, "Gets the response from the HTTP server using the specified request", {"<request>"}, True, 1, New HTTP_GetCommand)},
                                                                                 {"getstring", New CommandInfo("getstring", ShellType.HTTPShell, "Gets the string from the HTTP server using the specified request", {"<request>"}, True, 1, New HTTP_GetStringCommand)},
                                                                                 {"help", New CommandInfo("help", ShellType.HTTPShell, "Shows help screen", {"[command]"}, False, 0, New HTTP_HelpCommand)},
                                                                                 {"setsite", New CommandInfo("setsite", ShellType.HTTPShell, "Sets the HTTP site. Must be a valid URI.", {"<uri>"}, True, 1, New HTTP_SetSiteCommand)}}
        Public HTTPSite As String
        Public HTTPModCommands As New Dictionary(Of String, CommandInfo)
        Public HTTPShellPromptStyle As String = ""
        Public ClientHTTP As New HttpClient()

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
