
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

Imports System.Net.Sockets
Imports System.Threading

Module RemoteProcedure

    Public RPCDrives As New Dictionary(Of String, NetworkStream)
    Public RPCListen As TcpListener
    Public RPCClient As Socket
    Public RPCPort As Integer = 12345
    Public RPCThread As New Thread(AddressOf ListenRPC) With {.IsBackground = True}

    Sub StartRPC()
        Try
            RPCThread.Start()
        Catch ex As ThreadStateException
            W(DoTranslation("RPC is already running.", currentLang), True, ColTypes.Neutral)
        End Try
    End Sub
    Sub ListenRPC()
        RPCListen = New TcpListener(New IPAddress({0, 0, 0, 0}), RPCPort)
        RPCListen.Start()
        Dim RPCListener As New Thread(AddressOf RecCommand)
        RPCListener.Start()
        W(DoTranslation("RPC listening on all addresses using port {0}.", currentLang), True, ColTypes.Neutral, RPCPort)

        While Not RebootRequested
            Try
                Dim RPCStream As NetworkStream
                Dim RPCClient As Socket
                Dim RPCIP As String
                RPCClient = RPCListen.AcceptSocket
                RPCStream = New NetworkStream(RPCClient) With {.ReadTimeout = 50}
                RPCIP = RPCClient.RemoteEndPoint.ToString.Remove(RPCClient.RemoteEndPoint.ToString.IndexOf(":"))
                If Not RPCDrives.Keys.Contains(RPCIP) Then
                    RPCDrives.Add(RPCIP, RPCStream)
                End If
            Catch ae As ThreadAbortException
                Exit While
            Catch ex As Exception
                W(DoTranslation("Error in connection: {0}", currentLang), True, ColTypes.Neutral, ex.Message)
                WStkTrc(ex)
            End Try
        End While

        'TODO: Uncomment below comment
        'RebootRequested = False
        RPCListen.Stop()
        RPCDrives.Clear()
        Thread.CurrentThread.Abort()
    End Sub

End Module
