
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

Imports System.IO
Imports System.Net.Sockets

Namespace Network.RemoteDebug
    Public Class RemoteDebugDevice

        ''' <summary>
        ''' The client socket
        ''' </summary>
        Public ReadOnly Property ClientSocket As Socket
        ''' <summary>
        ''' The client stream
        ''' </summary>
        Public ReadOnly Property ClientStream As NetworkStream
        ''' <summary>
        ''' The client stream writer
        ''' </summary>
        Public ReadOnly Property ClientStreamWriter As StreamWriter
        ''' <summary>
        ''' The client IP address
        ''' </summary>
        Public ReadOnly Property ClientIP As String
        ''' <summary>
        ''' The client name
        ''' </summary>
        Public Property ClientName As String

        ''' <summary>
        ''' Makes a new instance of a remote debug device
        ''' </summary>
        Public Sub New(ClientSocket As Socket, ClientStream As NetworkStream, ClientIP As String, ClientName As String)
            Me.ClientSocket = ClientSocket
            Me.ClientStream = ClientStream
            ClientStreamWriter = New StreamWriter(ClientStream) With {.AutoFlush = True}
            Me.ClientIP = ClientIP
            Me.ClientName = ClientName
        End Sub

    End Class
End Namespace
