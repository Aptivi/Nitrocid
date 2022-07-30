
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
Imports KS.Misc.Notifications
Imports KS.Misc.Screensaver

Namespace Network.RPC
    Public Module RPCCommands

        ''' <summary>
        ''' List of RPC commands.<br/>
        ''' <br/>&lt;Request:Shutdown&gt;: Shuts down the remote kernel. Usage: &lt;Request:Shutdown&gt;(IP)
        ''' <br/>&lt;Request:Reboot&gt;: Reboots the remote kernel. Usage: &lt;Request:Reboot&gt;(IP)
        ''' <br/>&lt;Request:RebootSafe&gt;: Reboots the remote kernel to safe mode. Usage: &lt;Request:RebootSafe&gt;(IP)
        ''' <br/>&lt;Request:Lock&gt;: Locks the computer remotely. Usage: &lt;Request:Lock&gt;(IP)
        ''' <br/>&lt;Request:SaveScr&gt;: Saves the screen remotely. Usage: &lt;Request:SaveScr&gt;(IP)
        ''' <br/>&lt;Request:Exec&gt;: Executes a command remotely. Usage: &lt;Request:Exec&gt;(Lock)
        ''' <br/>&lt;Request:Acknowledge&gt;: Pings the remote kernel silently. Usage: &lt;Request:Acknowledge&gt;(IP)
        ''' <br/>&lt;Request:Ping&gt;: Pings the remote kernel with notification. Usage: &lt;Request:Ping&gt;(IP)
        ''' </summary>
        ReadOnly RPCCommands As New List(Of String) From {
            "<Request:Shutdown>",
            "<Request:Reboot>",
            "<Request:RebootSafe>",
            "<Request:Lock>",
            "<Request:SaveScr>",
            "<Request:Exec>",
            "<Request:Acknowledge>",
            "<Request:Ping>"
        }

        ''' <summary>
        ''' Send an RPC command to another instance of KS using the specified address
        ''' </summary>
        ''' <param name="Request">A request</param>
        ''' <param name="IP">An IP address which the RPC is hosted</param>
        Public Sub SendCommand(Request As String, IP As String)
            SendCommand(Request, IP, RPCPort)
        End Sub

        ''' <summary>
        ''' Send an RPC command to another instance of KS using the specified address
        ''' </summary>
        ''' <param name="Request">A request</param>
        ''' <param name="IP">An IP address which the RPC is hosted</param>
        ''' <param name="Port">A port which the RPC is hosted</param>
        ''' <exception cref="InvalidOperationException"></exception>
        Public Sub SendCommand(Request As String, IP As String, Port As Integer)
            If RPCEnabled Then
                'Get the command and the argument
                Dim Cmd As String = Request.Remove(Request.IndexOf("("))
                Wdbg(DebugLevel.I, "Command: {0}", Cmd)
                Dim Arg As String = Request.Substring(Request.IndexOf("(") + 1)
                Wdbg(DebugLevel.I, "Prototype Arg: {0}", Arg)
                Arg = Arg.Remove(Arg.Length - 1)
                Wdbg(DebugLevel.I, "Finished Arg: {0}", Arg)

                'Check the command
                Dim Malformed As Boolean
                If RPCCommands.Contains(Cmd) Then
                    Wdbg(DebugLevel.I, "Command found.")

                    'Check the request type
                    Dim RequestType As String = Cmd.Substring(Cmd.IndexOf(":") + 1, Finish:=Cmd.IndexOf(">"))
                    Dim ByteMsg() As Byte = Array.Empty(Of Byte)()
                    Select Case RequestType
                        Case "Shutdown", "Reboot", "RebootSafe", "Lock", "SaveScr", "Exec", "Acknowledge", "Ping"
                            'Populate the byte message to send the confirmation to
                            Wdbg(DebugLevel.I, "Stream opened for device {0}", Arg)
                            ByteMsg = Text.Encoding.Default.GetBytes($"{RequestType}Confirm, " + Arg + NewLine)
                        Case Else
                            'Rare case reached. Drop it.
                            Wdbg(DebugLevel.E, "Malformed request. {0}", Cmd)
                            Malformed = True
                    End Select

                    'Send the response
                    If Not Malformed Then
                        Wdbg(DebugLevel.I, "Sending response to device...")
                        RPCListen.Send(ByteMsg, ByteMsg.Length, IP, Port)
                        KernelEventManager.RaiseRPCCommandSent(Cmd, Arg, IP, Port)
                    End If
                End If
            Else
                Throw New InvalidOperationException(DoTranslation("Trying to send an RPC command while RPC didn't start."))
            End If
        End Sub

        ''' <summary>
        ''' Thread to listen to commands.
        ''' </summary>
        Sub ReceiveCommand()
            Dim RemoteEndpoint As New IPEndPoint(IPAddress.Any, RPCPort)
            While RPCStarted
                Dim MessageBuffer() As Byte
                Dim Message As String = ""
                Try
                    MessageBuffer = RPCListen.Receive(RemoteEndpoint)
                    Message = Text.Encoding.Default.GetString(MessageBuffer)

                    'If the message is not empty, parse it
                    If Not String.IsNullOrEmpty(Message) Then
                        Wdbg("RPC: Received message {0}", Message)
                        KernelEventManager.RaiseRPCCommandReceived(Message, RemoteEndpoint.Address.ToString, RemoteEndpoint.Port)

                        'Iterate through every confirmation message
                        If Message.StartsWith("ShutdownConfirm") Then
                            Wdbg(DebugLevel.I, "Shutdown confirmed from remote access.")
                            RPCPowerListener.Start(PowerMode.Shutdown)
                        ElseIf Message.StartsWith("RebootConfirm") Then
                            Wdbg(DebugLevel.I, "Reboot confirmed from remote access.")
                            RPCPowerListener.Start(PowerMode.Reboot)
                        ElseIf Message.StartsWith("RebootSafeConfirm") Then
                            Wdbg(DebugLevel.I, "Reboot to safe mode confirmed from remote access.")
                            RPCPowerListener.Start(PowerMode.RebootSafe)
                        ElseIf Message.StartsWith("LockConfirm") Then
                            Wdbg(DebugLevel.I, "Lock confirmed from remote access.")
                            LockScreen()
                        ElseIf Message.StartsWith("SaveScrConfirm") Then
                            Wdbg(DebugLevel.I, "Save screen confirmed from remote access.")
                            ShowSavers(DefSaverName)
                        ElseIf Message.StartsWith("ExecConfirm") Then
                            If LoggedIn Then
                                Wdbg(DebugLevel.I, "Exec confirmed from remote access.")
                                Console.WriteLine()
                                GetLine(Message.Replace("ExecConfirm, ", "").Replace(NewLine, ""))
                            Else
                                Wdbg(DebugLevel.W, "Tried to exec from remote access while not logged in. Dropping packet...")
                            End If
                        ElseIf Message.StartsWith("AckConfirm") Then
                            Wdbg(DebugLevel.I, "{0} says ""Hello.""", Message.Replace("AckConfirm, ", "").Replace(NewLine, ""))
                        ElseIf Message.StartsWith("PingConfirm") Then
                            Dim IPAddr As String = Message.Replace("PingConfirm, ", "").Replace(NewLine, "")
                            Wdbg(DebugLevel.I, "{0} pinged this device!", IPAddr)
                            NotifySend(New Notification(DoTranslation("Ping!"),
                                                        DoTranslation("{0} pinged you.").FormatString(IPAddr),
                                                        NotifPriority.Low, NotifType.Normal))
                        Else
                            Wdbg(DebugLevel.W, "Not found. Message was {0}", Message)
                        End If
                    End If
                Catch ex As Exception
                    Dim SE As SocketException = CType(ex.InnerException, SocketException)
                    If SE IsNot Nothing Then
                        If Not SE.SocketErrorCode = SocketError.TimedOut Then
                            Wdbg(DebugLevel.E, "Error from host: {0}", SE.SocketErrorCode.ToString)
                            WStkTrc(ex)
                        End If
                    Else
                        Wdbg(DebugLevel.E, "Fatal error: {0}", ex.Message)
                        WStkTrc(ex)
                        KernelEventManager.RaiseRPCCommandError(Message, ex, RemoteEndpoint.Address.ToString, RemoteEndpoint.Port)
                    End If
                End Try
            End While
        End Sub

    End Module
End Namespace
