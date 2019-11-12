
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

Module RPC_Commands

    Dim Commands As New List(Of String) From {"<Request:Shutdown>", 'Request will be like this: <Request:Shutdown>(IP)
                                              "<Request:Reboot>",   'Request will be like this: <Request:Reboot>(IP)
                                              "<Request:Populate>", 'Request will be like this: <Request:Populate>(Hardware)
                                              "<Request:Exec>"}     'Request will be like this: <Request:Exec>(CMD)

    Sub SendCommand(ByVal Request As String)
        Dim Cmd As String = Request.Remove(Request.IndexOf("("))
        Dim Arg As String = Request.Substring(Request.IndexOf("(") + 1)
        Arg = Arg.Remove(Arg.Count - 1)
        If Commands.Contains(Cmd) Then
            If Cmd = "<Request:Shutdown>" Then
                Dim remotestream As NetworkStream = RPCDrives(Arg)
                Dim ByteMsg() As Byte = Text.Encoding.Default.GetBytes("ShutdownConfirm, " + Arg + vbNewLine)
                Dim WResult As IAsyncResult = remotestream.BeginWrite(ByteMsg, 0, ByteMsg.Count, Nothing, Nothing)
                WResult.AsyncWaitHandle.WaitOne()
                remotestream.EndWrite(WResult)
            ElseIf Cmd = "<Request:Reboot>" Then
                Dim remotestream As NetworkStream = RPCDrives(Arg)
                Dim ByteMsg() As Byte = Text.Encoding.Default.GetBytes("RebootConfirm, " + Arg + vbNewLine)
                Dim WResult As IAsyncResult = remotestream.BeginWrite(ByteMsg, 0, ByteMsg.Count, Nothing, Nothing)
                WResult.AsyncWaitHandle.WaitOne()
                remotestream.EndWrite(WResult)
            ElseIf Cmd = "<Request:Populate>" Then
                Wdbg("Tried to send a non-implemented request.") 'To be done
            ElseIf Cmd = "<Request:Exec>" Then
                Wdbg("Tried to send a non-implemented request.") 'To be done
            Else
                Wdbg("Malformed request.")
            End If
        End If
    End Sub
    Sub RecCommand()
        Dim i As Integer = 0
        While True
            If i > RPCDrives.Count - 1 Then
                i = 0
            Else
                Dim buff(65535) As Byte
                Dim streamnet As NetworkStream = RPCDrives.Values(i)
                Dim ip As String = RPCDrives.Keys(i)
                i += 1
                Try
                    streamnet.Read(buff, 0, 65536)
                    Dim msg As String = Text.Encoding.Default.GetString(buff)
                    msg = msg.Replace(vbCr, vbNullChar) 'Remove all instances of vbCr (macOS newlines) } Windows hosts are affected, too, because it uses
                    msg = msg.Replace(vbLf, vbNullChar) 'Remove all instances of vbLf (Linux newlines) } vbCrLf, which means (vbCr + vbLf)
                    If Not msg.StartsWith(vbNullChar) Then
                        If msg.StartsWith("ShutdownConfirm") Then
                            Wdbg("Shutdown confirmed from remote access.")
                            PowerManage("shutdown")
                        ElseIf msg.StartsWith("RebootConfirm") Then
                            Wdbg("Reboot confirmed from remote access.")
                            PowerManage("reboot")
                        Else
                            Wdbg("Not found")
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

End Module
