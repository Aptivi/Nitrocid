
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Public Module RemoteDebugger

    Public DebugPort As Integer = 3014
    Public RDebugClient As Socket
    Public DebugTCP As TcpListener
    Public DebugDevices As New Dictionary(Of Socket, String)
    Public dbgConns As New Dictionary(Of StreamWriter, String)
    Public RDebugThread As New Thread(AddressOf StartRDebugger) With {.IsBackground = True, .Name = "Remote Debug Thread"}
    Public RDebugBlocked As New List(Of String) 'Blocked IP addresses
    Public RDebugStopping As Boolean

    ''' <summary>
    ''' Whether to start or stop the remote debugger
    ''' </summary>
    ''' <param name="DebugEnable">If true, starts the Remote Debugger. If false, stops it.</param>
    Sub StartRDebugThread(ByVal DebugEnable As Boolean)
        If DebugMode Then
            If DebugEnable Then
                If Not RDebugThread.IsAlive Then RDebugThread.Start()
            Else
                RDebugStopping = True
                RDebugThread = New Thread(AddressOf StartRDebugger) With {.IsBackground = True, .Name = "Remote Debug Thread"}
            End If
        End If
    End Sub

    ''' <summary>
    ''' Thread to accept connections after the listener starts
    ''' </summary>
    Sub StartRDebugger()
        'Listen to a current IP address
        Try
            DebugTCP = New TcpListener(New IPAddress({0, 0, 0, 0}), DebugPort)
            DebugTCP.Start()
        Catch sex As SocketException
            W(DoTranslation("Remote debug failed to start: {0}"), True, ColTypes.Error, sex.Message)
            WStkTrc(sex)
        End Try

        'Start the listening thread
        Dim RStream As New Thread(AddressOf ReadAndBroadcastAsync) With {.Name = "Remote Debug Listener Thread"}
        RStream.Start()
        W(DoTranslation("Debug listening on all addresses using port {0}."), True, ColTypes.Neutral, DebugPort)

        While Not RDebugStopping
            Thread.Sleep(1)
            Try
                Dim RDebugStream As NetworkStream
                Dim RDebugSWriter As StreamWriter
                Dim RDebugClient As Socket
                Dim RDebugIP As String
                Dim RDebugEndpoint As String
                Dim RDebugName As String
                If DebugTCP.Pending Then
                    RDebugClient = DebugTCP.AcceptSocket
                    RDebugStream = New NetworkStream(RDebugClient)
                    RDebugSWriter = New StreamWriter(RDebugStream) With {.AutoFlush = True}
                    RDebugEndpoint = RDebugClient.RemoteEndPoint.ToString
                    RDebugIP = RDebugEndpoint.Remove(RDebugClient.RemoteEndPoint.ToString.IndexOf(":"))
                    AddDeviceToJson(RDebugIP, False)
                    RDebugName = GetDeviceProperty(RDebugIP, DeviceProperty.Name)
                    If String.IsNullOrEmpty(RDebugName) Then
                        Wdbg("W", "Debug device {0} has no name. Prompting for name...", RDebugIP)
                    End If
                    If RDebugBlocked.Contains(RDebugIP) Then
                        Wdbg("W", "Debug device {0} ({1}) tried to join remote debug, but blocked.", RDebugName, RDebugIP)
                        RDebugClient.Disconnect(True)
                    Else
                        dbgConns.Add(RDebugSWriter, RDebugName)
                        DebugDevices.Add(RDebugClient, RDebugIP)
                        RDebugSWriter.WriteLine(DoTranslation(">> Remote Debug and Chat: version") + " 0.6.3") 'Increment each minor/major change(s)
                        RDebugSWriter.WriteLine(DoTranslation(">> Your address is {0}."), RDebugIP)
                        If String.IsNullOrEmpty(RDebugName) Then
                            RDebugSWriter.WriteLine(DoTranslation(">> Welcome! This is your first time entering remote debug and chat. Use ""/register <name>"" to register.") + " ", RDebugName)
                        Else
                            RDebugSWriter.WriteLine(DoTranslation(">> Your name is {0}."), RDebugName)
                        End If
                        Wdbg("I", "Debug device ""{0}"" ({1}) connected.", RDebugName, RDebugIP)
                        RDebugSWriter.Flush()
                        EventManager.RaiseRemoteDebugConnectionAccepted(RDebugIP)
                    End If
                End If
            Catch ae As ThreadAbortException
                Exit While
            Catch ex As Exception
                W(DoTranslation("Error in connection: {0}"), True, ColTypes.Error, ex.Message)
                WStkTrc(ex)
            End Try
        End While

        RDebugStopping = False
        DebugTCP.Stop()
        dbgConns.Clear()
        Thread.CurrentThread.Abort()
    End Sub

    ''' <summary>
    ''' Thread to listen to messages and post them to the debugger
    ''' </summary>
    Sub ReadAndBroadcastAsync()
        Dim i As Integer = 0 'Because DebugDevices.Keys(i) is zero-based
        While True
            Thread.Sleep(1)
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
                            If DebugCommands.ContainsKey(cmd.Split(" ")(0)) Then 'Command is found or not
                                'Parsing starts here.
                                ParseCmd(cmd, dbgConns.Keys(i - 1), ip)
                            ElseIf RemoteDebugAliases.Keys.Contains(cmd.Split(" ")(0)) Then
                                'Alias parsing starts here.
                                ExecuteRDAlias(cmd, dbgConns.Keys(i - 1), ip)
                            Else
                                dbgConns.Keys(i - 1).WriteLine(DoTranslation("Command {0} not found. Use ""/help"" to see the list."), cmd.Split(" ")(0))
                            End If
                        Else
                            If Not String.IsNullOrEmpty(name) Then 'Prevent no-name people from chatting
                                If RecordChatToDebugLog Then
                                    Wdbg("I", "{0}> {1}", name, msg.Replace(vbNullChar, ""))
                                Else
                                    WdbgDevicesOnly("I", "{0}> {1}", name, msg.Replace(vbNullChar, ""))
                                End If
                                SetDeviceProperty(ip, DeviceProperty.ChatHistory, "[" + Render() + "] " + msg.Replace(vbNullChar, ""))
                            End If
                        End If
                    End If
                Catch ex As Exception
                    Dim SE As SocketException = CType(ex.InnerException, SocketException)
                    If SE IsNot Nothing Then
                        If Not SE.SocketErrorCode = SocketError.TimedOut And Not SE.SocketErrorCode = SocketError.WouldBlock Then
                            Wdbg("E", "Error from host {0}: {1}", ip, SE.SocketErrorCode.ToString)
                            WStkTrc(ex)
                        End If
                    Else
                        WStkTrc(ex)
                    End If
                End Try
            End If
        End While
    End Sub

    ''' <summary>
    ''' Executes the remote debugger alias
    ''' </summary>
    ''' <param name="aliascmd">Aliased command with arguments</param>
    ''' <param name="SocketStream">A socket stream writer</param>
    ''' <param name="Address">IP Address</param>
    Sub ExecuteRDAlias(ByVal aliascmd As String, ByVal SocketStream As IO.StreamWriter, ByVal Address As String)
        Dim FirstWordCmd As String = aliascmd.Split(" "c)(0)
        Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, RemoteDebugAliases(FirstWordCmd))
        ParseCmd(actualCmd, SocketStream, Address)
    End Sub

    ''' <summary>
    ''' Gets the index from an instance of StreamWriter
    ''' </summary>
    ''' <param name="SW">A stream writer</param>
    ''' <returns>An index of it, or -1 if not found.</returns>
    Function GetSWIndex(ByVal SW As StreamWriter) As Integer
        For i As Integer = 0 To dbgConns.Count - 1
            If SW.Equals(dbgConns.Keys(i)) Then
                Return i
            End If
        Next
        Return -1
    End Function

End Module
