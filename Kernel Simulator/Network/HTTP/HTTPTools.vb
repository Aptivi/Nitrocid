
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
Imports System.Threading.Tasks
Imports KS.Shell.Shells.HTTP

Namespace Network.HTTP
    Public Module HTTPTools

        ''' <summary>
        ''' Deletes the specified content from HTTP server
        ''' </summary>
        ''' <param name="ContentUri">Content URI (starts after the HTTP hostname, e.g. "filetodelete.html")</param>
        Public Async Function HttpDelete(ContentUri As String) As Task
            If HTTPConnected Then
                Dim TargetUri As New Uri(NeutralizeUri(ContentUri))
                Await ClientHTTP.DeleteAsync(TargetUri)
            Else
                Throw New InvalidOperationException(DoTranslation("You must connect to server with administrative privileges before performing the deletion."))
            End If
        End Function

        ''' <summary>
        ''' Gets the specified content string from HTTP server
        ''' </summary>
        ''' <param name="ContentUri">Content URI (starts after the HTTP hostname, e.g. "filetoget.html")</param>
        Public Async Function HttpGetString(ContentUri As String) As Task(Of String)
            If HTTPConnected Then
                Dim TargetUri As New Uri(NeutralizeUri(ContentUri))
                Return Await ClientHTTP.GetStringAsync(TargetUri)
            Else
                Throw New InvalidOperationException(DoTranslation("You must connect to server before performing transmission."))
            End If
        End Function

        ''' <summary>
        ''' Gets the specified content from HTTP server
        ''' </summary>
        ''' <param name="ContentUri">Content URI (starts after the HTTP hostname, e.g. "filetoget.html")</param>
        Public Async Function HttpGet(ContentUri As String) As Task(Of HttpResponseMessage)
            If HTTPConnected Then
                Dim TargetUri As New Uri(NeutralizeUri(ContentUri))
                Return Await ClientHTTP.GetAsync(TargetUri)
            Else
                Throw New InvalidOperationException(DoTranslation("You must connect to server before performing transmission."))
            End If
        End Function

        ''' <summary>
        ''' Neutralize the URI so the host name, <see cref="HTTPSite"/>, doesn't appear twice.
        ''' </summary>
        ''' <param name="ContentUri">Content URI (starts after the HTTP hostname, e.g. "filetoget.html")</param>
        Public Function NeutralizeUri(ContentUri As String) As String
            Dim NeutralizedUri As String = ""
            If Not ContentUri.StartsWith(HTTPSite) Then NeutralizedUri += HTTPSite
            NeutralizedUri += ContentUri
            Return NeutralizedUri
        End Function

    End Module
End Namespace
