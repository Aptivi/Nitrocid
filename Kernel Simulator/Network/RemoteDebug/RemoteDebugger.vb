
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

Imports System.Net.Sockets
Imports System.Threading
Imports System.IO
Imports KS.Misc.Notifications
Imports KS.Shell.ShellBase.Aliases
Imports KS.TimeDate

Namespace Network.RemoteDebug
    Public Module RemoteDebugger

        Public DebugPort As Integer = 3014
        Public RDebugClient As Socket
        Public DebugTCP As TcpListener
        Public DebugDevices As New List(Of RemoteDebugDevice)
        Public RDebugThread As New KernelThread("Remote Debug Thread", True, AddressOf StartRDebugger)
        Public RDebugBlocked As New List(Of String) 'Blocked IP addresses
        Public RDebugStopping As Boolean
        Public RDebugAutoStart As Boolean = True
        Public RDebugMessageFormat As String = ""
        Friend RDebugFailed As Boolean
        Friend RDebugFailedReason As Exception
        Private ReadOnly RDebugVersion As String = "0.7.0"
        Private RDebugBail As Boolean

        ''' <summary>
        ''' Whether to start or stop the remote debugger
        ''' </summary>
        Public Sub StartRDebugThread()
            If DebugMode Then
                If Not RDebugThread.IsAlive Then
                    RDebugThread.Start()
                    While Not RDebugBail
                    End While
                    RDebugBail = False
                End If
            End If
        End Sub

        ''' <summary>
        ''' Whether to start or stop the remote debugger
        ''' </summary>
        Public Sub StopRDebugThread()
            If DebugMode Then
                If RDebugThread.IsAlive Then
                    RDebugStopping = True
                    RDebugThread.Stop()
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
                RDebugFailed = True
                RDebugFailedReason = sex
                WStkTrc(sex)
            End Try

            'Start the listening thread
            Dim RStream As New KernelThread("Remote Debug Listener Thread", False, AddressOf ReadAndBroadcastAsync)
            RStream.Start()
            RDebugBail = True

            'Run forever! Until the remote debugger is stopping.
            While Not RDebugStopping
                Try
                    Thread.Sleep(1)

                    'Variables
                    Dim RDebugStream As NetworkStream
                    Dim RDebugSWriter As StreamWriter
                    Dim RDebugClient As Socket
                    Dim RDebugIP As String
                    Dim RDebugEndpoint As String
                    Dim RDebugName As String
                    Dim RDebugInstance As RemoteDebugDevice

                    'Check for pending connections
                    If DebugTCP.Pending Then
                        'Populate the device variables with the information
                        RDebugClient = DebugTCP.AcceptSocket
                        RDebugStream = New NetworkStream(RDebugClient)

                        'Add the device to JSON
                        RDebugEndpoint = RDebugClient.RemoteEndPoint.ToString
                        RDebugIP = RDebugEndpoint.Remove(RDebugClient.RemoteEndPoint.ToString.IndexOf(":"))
                        AddDeviceToJson(RDebugIP, False)

                        'Get the remaining properties
                        RDebugName = GetDeviceProperty(RDebugIP, DeviceProperty.Name)
                        RDebugInstance = New RemoteDebugDevice(RDebugClient, RDebugStream, RDebugIP, RDebugName)
                        RDebugSWriter = RDebugInstance.ClientStreamWriter

                        'Check the name
                        If String.IsNullOrEmpty(RDebugName) Then
                            Wdbg(DebugLevel.W, "Debug device {0} has no name. Prompting for name...", RDebugIP)
                        End If

                        'Check to see if the device is blocked
                        If RDebugBlocked.Contains(RDebugIP) Then
                            'Blocked! Disconnect it.
                            Wdbg(DebugLevel.W, "Debug device {0} ({1}) tried to join remote debug, but blocked.", RDebugName, RDebugIP)
                            RDebugClient.Disconnect(True)
                        Else
                            'Not blocked yet. Add the connection.
                            DebugDevices.Add(RDebugInstance)
                            RDebugSWriter.WriteLine(DoTranslation(">> Remote Debug and Chat: version") + " {0}", RDebugVersion)
                            RDebugSWriter.WriteLine(DoTranslation(">> Your address is {0}."), RDebugIP)
                            If String.IsNullOrEmpty(RDebugName) Then
                                RDebugSWriter.WriteLine(DoTranslation(">> Welcome! This is your first time entering remote debug and chat. Use ""/register <name>"" to register.") + " ", RDebugName)
                            Else
                                RDebugSWriter.WriteLine(DoTranslation(">> Your name is {0}."), RDebugName)
                            End If

                            'Acknowledge the debugger
                            Wdbg(DebugLevel.I, "Debug device ""{0}"" ({1}) connected.", RDebugName, RDebugIP)
                            RDebugSWriter.Flush()
                            KernelEventManager.RaiseRemoteDebugConnectionAccepted(RDebugIP)
                        End If
                    End If
                Catch ae As ThreadInterruptedException
                    Exit While
                Catch ex As Exception
                    If NotifyOnRemoteDebugConnectionError Then
                        Dim RemoteDebugError As New Notification(DoTranslation("Remote debugger connection error"), ex.Message, NotifPriority.Medium, NotifType.Normal)
                        NotifySend(RemoteDebugError)
                    Else
                        Write(DoTranslation("Remote debugger connection error") + ": {0}", True, ColTypes.Error, ex.Message)
                    End If
                    WStkTrc(ex)
                End Try
            End While

            RStream.Wait()
            RDebugStopping = False
            DebugTCP.Stop()
            DebugDevices.Clear()
            Thread.CurrentThread.Interrupt()
        End Sub

        ''' <summary>
        ''' Thread to listen to messages and post them to the debugger
        ''' </summary>
        Sub ReadAndBroadcastAsync()
            While Not RDebugStopping
                For DeviceIndex As Integer = 0 To DebugDevices.Count - 1
                    Thread.Sleep(1)
                    Try
                        'Variables
                        Dim MessageBuffer(65536) As Byte
                        Dim SocketStream As New NetworkStream(DebugDevices(DeviceIndex).ClientSocket)
                        Dim SocketStreamWriter As StreamWriter = DebugDevices(DeviceIndex).ClientStreamWriter
                        Dim SocketIP As String = DebugDevices(DeviceIndex).ClientIP
                        Dim SocketName As String = DebugDevices(DeviceIndex).ClientName

                        'Set the timeout of ten milliseconds to ensure that no device "take turns in messaging"
                        SocketStream.ReadTimeout = 10

                        'Read a message from the stream
                        SocketStream.Read(MessageBuffer, 0, 65536)
                        Dim Message As String = Text.Encoding.Default.GetString(MessageBuffer)

                        'Make some fixups regarding newlines, which means remove all instances of vbCr (Mac OS 9 newlines) and vbLf (Linux newlines).
                        'Windows hosts are affected, too, because it uses vbCrLf, which means (vbCr + vbLf)
                        Message = Message.Replace(vbCr, vbNullChar)
                        Message = Message.Replace(vbLf, vbNullChar)

                        'Don't post message if it starts with a null character. On Unix, the nullchar detection always returns false even if it seems
                        'that the message starts with the actual character, not the null character, so detect nullchar by getting the first character
                        'from the message and comparing it to the null char ASCII number, which is 0.
                        If Not Convert.ToInt32(Message(0)) = 0 Then
                            'Fix the value of the message
                            Message = Message.Replace(vbNullChar, "")

                            'Now, check the message
                            If Message.StartsWith("/") Then
                                'Message is a command
                                Dim FullCommand As String = Message.Replace("/", "").Replace(vbNullChar, "")
                                Dim Command As String = FullCommand.Split(" ")(0)
                                If DebugCommands.ContainsKey(Command) Then
                                    'Parsing starts here.
                                    ParseCmd(FullCommand, SocketStreamWriter, SocketIP)
                                ElseIf RemoteDebugAliases.ContainsKey(Command) Then
                                    'Alias parsing starts here.
                                    ExecuteRDAlias(FullCommand, SocketStreamWriter, SocketIP)
                                Else
                                    SocketStreamWriter.WriteLine(DoTranslation("Command {0} not found. Use ""/help"" to see the list."), Command)
                                End If
                            Else
                                'Check to see if the unnamed stranger is trying to send a message
                                If Not String.IsNullOrEmpty(SocketName) Then
                                    'Check the message format
                                    If String.IsNullOrWhiteSpace(RDebugMessageFormat) Then
                                        RDebugMessageFormat = "{0}> {1}"
                                    End If

                                    'Decide if we're recording the chat to the debug log
                                    If RecordChatToDebugLog Then
                                        Wdbg(DebugLevel.I, ProbePlaces(RDebugMessageFormat), SocketName, Message)
                                    Else
                                        WdbgDevicesOnly(DebugLevel.I, ProbePlaces(RDebugMessageFormat), SocketName, Message)
                                    End If

                                    'Add the message to the chat history
                                    SetDeviceProperty(SocketIP, DeviceProperty.ChatHistory, "[" + Render() + "] " + Message)
                                End If
                            End If
                        End If
                    Catch ioex As IOException When ioex.Message.Contains("non-connected")
                        'HACK: Ugly workaround, but we have to search the message for "non-connected" to get the specific error message that we
                        'need to react appropriately. Removing the device from the debug devices list will allow the kernel to continue working
                        'without crashing just because of this exception. We had to search the above word in this phrase:
                        '
                        '  System.IO.IOException: The operation is not allowed on non-connected sockets.
                        '                                                         ^^^^^^^^^^^^^
                        '
                        'Though, we wish to have a better workaround to detect this specific error message on .NET Framework 4.8.
                        DebugDevices.RemoveAt(DeviceIndex)
                    Catch ex As Exception
                        Dim SE As SocketException = CType(ex.InnerException, SocketException)
                        If SE IsNot Nothing Then
                            If Not SE.SocketErrorCode = SocketError.TimedOut And Not SE.SocketErrorCode = SocketError.WouldBlock Then
                                If DebugDevices.Count > DeviceIndex Then
                                    Dim SocketIP As String = DebugDevices(DeviceIndex)?.ClientIP
                                    Wdbg(DebugLevel.E, "Error from host {0}: {1}", SocketIP, SE.SocketErrorCode.ToString)
                                    WStkTrc(ex)
                                Else
                                    Wdbg(DebugLevel.E, "Error from unknown host: {0}", SE.SocketErrorCode.ToString)
                                    WStkTrc(ex)
                                End If
                            End If
                        Else
                            Wdbg(DebugLevel.E, "Unknown error of remote debug: {0}: {1}", ex.GetType.FullName, ex.Message)
                            WStkTrc(ex)
                        End If
                    End Try
                Next
            End While
        End Sub

    End Module
End Namespace
