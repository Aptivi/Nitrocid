
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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
    Public dbgConns As New Dictionary(Of StreamWriter, String)
    Public RDebugThread As New Thread(AddressOf StartRDebugger) With {.IsBackground = True}
    Public RDebugStopping As Boolean

    Sub StartRDebugThread(ByVal DebugEnable As Boolean)
        If DebugMode Then
            If DebugEnable Then
                RDebugThread.Start()
            Else
                RDebugStopping = True
            End If
        End If
    End Sub
    Sub StartRDebugger()
        'Listen to a current IP address
        DebugTCP = New TcpListener(New IPAddress({0, 0, 0, 0}), DebugPort)
        DebugTCP.Start()
        Dim RStream As New Thread(AddressOf ReadAndBroadcastAsync)
        RStream.Start()
        W(DoTranslation("Debug listening on all addresses using port {0}.", currentLang), True, ColTypes.Neutral, DebugPort)

        While Not RDebugStopping
            Try
                Dim RDebugStream As NetworkStream
                Dim RDebugSWriter As StreamWriter
                Dim RDebugClient As Socket
                Dim RDebugIP As String
                Dim RDebugRandomID As Integer
                Dim RDebugName As String
                If DebugTCP.Pending Then
                    RDebugClient = DebugTCP.AcceptSocket
                    RDebugStream = New NetworkStream(RDebugClient)
                    RDebugSWriter = New StreamWriter(RDebugStream) With {.AutoFlush = True}
                    RDebugIP = RDebugClient.RemoteEndPoint.ToString.Remove(RDebugClient.RemoteEndPoint.ToString.IndexOf(":"))
                    RDebugRandomID = New Random(100000).Next(999999)
                    RDebugName = RDebugDNP + CStr(RDebugRandomID)
                    dbgConns.Add(RDebugSWriter, RDebugName)
                    DebugDevices.Add(RDebugClient, RDebugIP)
                    dbgConns.Keys.Last.WriteLine(">> Chat version 0.3.1") 'Increment each minor/major change(s)
                    dbgConns.Keys.Last.WriteLine(">> Your address is {0}.", RDebugIP)
                    Wdbg("Debug device {0} ({1}) connected.", RDebugName, RDebugIP)
                End If
            Catch ae As ThreadAbortException
                Exit While
            Catch ex As Exception
                W(DoTranslation("Error in connection: {0}", currentLang), True, ColTypes.Neutral, ex.Message)
                WStkTrc(ex)
            End Try
        End While

        RDebugStopping = False
        DebugTCP.Stop()
        dbgConns.Clear()
        Thread.CurrentThread.Abort()
    End Sub
    Sub ReadAndBroadcastAsync()
        Dim i As Integer = 0 'Because DebugDevices.Keys(i) is zero-based
        While True
            If i > DebugDevices.Count - 1 Then
                i = 0
            Else
                Dim buff(65536) As Byte
                Dim streamnet As New NetworkStream(DebugDevices.Keys(i))
                Dim ip As String = DebugDevices.Values(i)
                Dim name As String = dbgConns.Values(i)
                streamnet.ReadTimeout = 10 'Seems to have fixed it
                i += 1
                Try
                    streamnet.Read(buff, 0, 65536)
                    Dim msg As String = Text.Encoding.Default.GetString(buff)
                    msg = msg.Replace(vbCr, vbNullChar) 'Remove all instances of vbCr (macOS newlines) } Windows hosts are affected, too, because it uses
                    msg = msg.Replace(vbLf, vbNullChar) 'Remove all instances of vbLf (Linux newlines) } vbCrLf, which means (vbCr + vbLf)
                    If Not msg.StartsWith(vbNullChar) Then 'Don't post message if it starts with a null character.
                        If msg.StartsWith("/") Then 'Message is a command
                            Dim cmd As String = msg.Replace("/", "").Replace(vbNullChar, "")
                            If DebugCmds.Contains(cmd.Split(" ")(0)) Then 'Command is found or not
                                'Parsing starts here.
                                ParseCmd(cmd, dbgConns.Keys(i - 1), ip)
                            Else
                                dbgConns.Keys(i - 1).WriteLine("Command {0} not found. Use ""/help"" to see the list.", cmd.Split(" ")(0))
                            End If
                        Else
                            Wdbg("{0}> {1}", name, msg.Replace(vbNullChar, ""))
                        End If
                    End If
                Catch ex As Exception
                    Dim SE As SocketException = CType(ex.InnerException, SocketException)
                    If Not IsNothing(SE) Then
                        If Not SE.SocketErrorCode = SocketError.TimedOut Then
                            Wdbg("Error from host {0}: {1}", ip, SE.SocketErrorCode.ToString)
                            WStkTrc(ex)
                        End If
                    Else
                        WStkTrc(ex)
                    End If
                End Try
            End If
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
                    dbgConns.Remove(dbgConns.Keys(i))
                    DebugDevices.Remove(DebugDevices.Keys(i))
                    W(DoTranslation("Device {0} disconnected.", currentLang), True, ColTypes.Neutral, IPAddr)
                End If
            End If
        Next
        If Not Found Then
            W(DoTranslation("Debug device {0} not found.", currentLang), True, ColTypes.Neutral, IPAddr)
        End If
    End Sub
    Sub DisconnectDbgDev(ByVal IPAddr As String)
        Dim Found As Boolean
        For i As Integer = 0 To DebugDevices.Count - 1
            If Found Then
                Exit Sub
            Else
                If IPAddr = DebugDevices.Values(i) Then
                    Wdbg("Debug device {0} disconnected.", DebugDevices.Values(i))
                    Found = True
                    DebugDevices.Keys(i).Disconnect(True)
                    dbgConns.Remove(dbgConns.Keys(i))
                    DebugDevices.Remove(DebugDevices.Keys(i))
                End If
            End If
        Next
        If Not Found Then
            W(DoTranslation("Debug device {0} not found.", currentLang), True, ColTypes.Neutral, IPAddr)
        End If
    End Sub
    Function GetSWIndex(ByVal SW As StreamWriter) As Integer
        For i As Integer = 0 To dbgConns.Count - 1
            If SW.Equals(dbgConns.Keys(i)) Then
                Return i
            End If
        Next
        Return -1
    End Function
End Module
