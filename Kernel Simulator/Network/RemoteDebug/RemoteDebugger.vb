
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
Imports System.IO

Module RemoteDebugger
    Public DebugPort As Integer = 3014
    Public RDebugClient As Socket
    Public DebugTCP As TcpListener
    Public DebugDevices As New Dictionary(Of Socket, String)
    Public dbgConns As New List(Of StreamWriter)

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
        Dim RStream As New Thread(AddressOf ReadAndBroadcastAsync)
        RStream.Start()
        W(DoTranslation("Debug listening on all addresses using port {0}.", currentLang), True, ColTypes.Neutral, DebugPort)

        While Not RebootRequested
            Try
                Dim RDebugStream As NetworkStream
                Dim RDebugClient As Socket
                If DebugTCP.Pending Then
                    RDebugClient = DebugTCP.AcceptSocket
                    RDebugStream = New NetworkStream(RDebugClient)
                    dbgConns.Add(New StreamWriter(RDebugStream) With {.AutoFlush = True})
                    DebugDevices.Add(RDebugClient, RDebugClient.RemoteEndPoint.ToString.Remove(RDebugClient.RemoteEndPoint.ToString.IndexOf(":")))
                    dbgConns.Last().WriteLine(">> Chat version 0.1") 'Increment each minor/major change(s)
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
    Sub ReadAndBroadcastAsync()
        While True
            For i As Integer = 0 To DebugDevices.Count - 1
                Dim buff(65536) As Byte
                Dim streamnet As New NetworkStream(DebugDevices.Keys(i))
                streamnet.Read(buff, 0, 65536)
                Dim msg As String = Text.Encoding.Default.GetString(buff)
                Wdbg("{0}> {1}", DebugDevices.Values(i), msg)
            Next
        End While
    End Sub
    Sub DisconnectDbgDevCmd(ByVal IPAddr As String)
        Dim Found As Boolean
        For i As Integer = 0 To DebugDevices.Count - 1
            If Found Then
                Exit Sub
            Else
                If IPAddr = DebugDevices.Values(i) Then
                    Wdbg("Debug device {0} disconnected.", DebugDevices.Values(i))
                    Found = True
                    DebugDevices.Keys(i).Disconnect(True)
                    dbgConns.RemoveAt(i)
                    DebugDevices.Remove(DebugDevices.Keys(i))
                    W(DoTranslation("Device {0} disconnected.", currentLang), True, ColTypes.Neutral, IPAddr)
                End If
            End If
        Next
        If Not Found Then
            W(DoTranslation("Debug device {0} not found.", currentLang), True, ColTypes.Neutral, IPAddr)
        End If
    End Sub
    Function GetSWIndex(ByVal SW As StreamWriter) As Integer
        For i As Integer = 0 To dbgConns.Count - 1
            If SW.Equals(dbgConns(i)) Then
                Return i
            End If
        Next
        Return -1
    End Function
End Module
