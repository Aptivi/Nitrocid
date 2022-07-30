
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
Imports KS.Misc.Splash

Namespace Network.RPC
    Public Module RemoteProcedure

        Public RPCListen As UdpClient
        Public RPCPort As Integer = 12345
        Public RPCEnabled As Boolean = True
        Friend RPCThread As New KernelThread("RPC Thread", True, AddressOf ReceiveCommand)

        ''' <summary>
        ''' Whether the RPC started
        ''' </summary>
        Public ReadOnly Property RPCStarted As Boolean
            Get
                Return RPCThread.IsAlive
            End Get
        End Property

        ''' <summary>
        ''' Starts the RPC listener
        ''' </summary>
        Public Sub StartRPC()
            If RPCEnabled Then
                Wdbg(DebugLevel.I, "RPC: Starting...")
                If Not RPCStarted Then
                    RPCListen = New UdpClient(RPCPort) With {.EnableBroadcast = True}
                    Wdbg(DebugLevel.I, "RPC: Listener started")
                    RPCThread.Start()
                    Wdbg(DebugLevel.I, "RPC: Thread started")
                Else
                    Throw New ThreadStateException(DoTranslation("Trying to start RPC while it's already started."))
                End If
            Else
                Throw New ThreadStateException(DoTranslation("Not starting RPC because it's disabled."))
            End If
        End Sub

        ''' <summary>
        ''' The wrapper for <see cref="StartRPC"/>
        ''' </summary>
        Sub WrapperStartRPC()
            If RPCEnabled Then
                Try
                    StartRPC()
                    ReportProgress(DoTranslation("RPC listening on all addresses using port {0}.").FormatString(RPCPort), 5, ColTypes.Neutral)
                Catch ex As ThreadStateException
                    ReportProgress(DoTranslation("RPC is already running."), 5, ColTypes.Error)
                    WStkTrc(ex)
                End Try
            Else
                ReportProgress(DoTranslation("Not starting RPC because it's disabled."), 3, ColTypes.Neutral)
            End If
        End Sub

        ''' <summary>
        ''' Stops the RPC listener
        ''' </summary>
        Sub StopRPC()
            If RPCStarted Then
                RPCThread.Stop()
                RPCListen?.Close()
                RPCListen = Nothing
                Wdbg(DebugLevel.I, "RPC stopped.")
            Else
                Wdbg(DebugLevel.E, "RPC hasn't started yet!")
            End If
        End Sub

    End Module
End Namespace
