
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

Module RemoteDebugger
    Public DebugPort As Integer = 3014
    Public RDebugClient As Socket
    Public DebugTCP As TcpListener
    Public DebugDevices As New List(Of Socket)

    Sub StartRDebugThread()
        If DebugMode Then
            Dim RDebugThread As New Thread(AddressOf StartRDebugger) With {.IsBackground = True}
            RDebugThread.Start()
        End If
    End Sub
    Sub StartRDebugger()
        'Listen to a current IP address
        DebugTCP = New TcpListener(New IPAddress({0, 0, 0, 0}), DebugPort)
        DebugTCP.Start()
        W(DoTranslation("Debug listening on all addresses using port {0}.", currentLang), True, ColTypes.Neutral, DebugPort)
        While Not RebootRequested
            Try
                Dim RDebugStream As NetworkStream
                Dim RDebugClient As Socket
                If DebugTCP.Pending Then
                    RDebugClient = DebugTCP.AcceptSocket
                    RDebugStream = New NetworkStream(RDebugClient)
                    dbgConns.Add(New IO.StreamWriter(RDebugStream) With {.AutoFlush = True})
                    DebugDevices.Add(RDebugClient)
                    Wdbg("Debug device {0} connected.", RDebugClient.RemoteEndPoint.ToString.Remove(RDebugClient.RemoteEndPoint.ToString.IndexOf(":")))
                End If
            Catch ex As Exception
                W(DoTranslation("Error in connection: {0}", currentLang), True, ColTypes.Neutral, ex.Message)
            End Try
        End While
        DebugTCP.Stop()
        dbgConns.Clear()
        Thread.CurrentThread.Abort()
    End Sub
    Sub DisconnectDbgDevCmd(ByVal IPAddr As String)
        Dim Found As Boolean
        For i As Integer = 0 To DebugDevices.Count - 1
            If Found Then
                Exit Sub
            Else
                If IPAddr = DebugDevices(i).RemoteEndPoint.ToString.Remove(DebugDevices(i).RemoteEndPoint.ToString.IndexOf(":")) Then
                    Wdbg("Debug device {0} disconnected.", DebugDevices(i).RemoteEndPoint.ToString.Remove(DebugDevices(i).RemoteEndPoint.ToString.IndexOf(":")))
                    Found = True
                    DebugDevices(i).Disconnect(True)
                    dbgConns.RemoveAt(i)
                    DebugDevices.RemoveAt(i)
                    W(DoTranslation("Device {0} disconnected.", currentLang), True, ColTypes.Neutral, IPAddr)
                End If
            End If
        Next
        If Not Found Then
            W(DoTranslation("Debug device {0} not found.", currentLang), True, ColTypes.Neutral, IPAddr)
        End If
    End Sub
    Function GetSWIndex(ByVal SW As IO.StreamWriter) As Integer
        For i As Integer = 0 To dbgConns.Count - 1
            If SW.Equals(dbgConns(i)) Then
                Return i
            End If
        Next
        Return -1
    End Function
End Module
