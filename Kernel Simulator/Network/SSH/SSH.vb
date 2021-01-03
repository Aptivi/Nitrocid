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

Module SSH

    Private DisconnectionRequested As Boolean

    ''' <summary>
    ''' Opens a session to specified address using the specified port and the username
    ''' </summary>
    ''' <param name="Address">An IP address or hostname</param>
    ''' <param name="Port">A port of the SSH server. It's usually 22</param>
    ''' <param name="Username">A username to authenticate with</param>
    Sub InitializeSSH(ByVal Address As String, ByVal Port As Integer, ByVal Username As String)
        Try
            'Authentication
            Wdbg("I", "Address: {0}:{1}, Username: {2}", Address, Port, Username)
            W(DoTranslation("Enter the password for {0}: ", currentLang), False, ColTypes.Input, Username)
            Dim Pass As String = ReadLineNoInput("*")
            Console.WriteLine()

            'Connection
            Dim SSH As New SshClient(Address, Port, Username, Pass)
            SSH.ConnectionInfo.Timeout = TimeSpan.FromSeconds(30)
            Wdbg("I", "Connecting to {0}...", Address)
            SSH.Connect()

            'Add handler for SSH
            AddHandler Console.CancelKeyPress, AddressOf SSHDisconnect
            RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
            EventManager.RaiseSSHConnected(Address + ":" + CStr(Port))

            'Shell creation
            Wdbg("I", "Opening shell...")
            Dim SSHS As Renci.SshNet.Shell = SSH.CreateShell(Console.OpenStandardInput, Console.OpenStandardOutput, Console.OpenStandardError)
            SSHS.Start()

            'Wait until disconnection
            While SSH.IsConnected
                Threading.Thread.Sleep(1)
                If DisconnectionRequested Then
                    SSHS.Stop()
                    SSH.Disconnect()
                End If
            End While
            Wdbg("I", "Connected: {0}", SSH.IsConnected)
            W(vbNewLine + DoTranslation("SSH Disconnected.", currentLang), True, ColTypes.Neutral)
            DisconnectionRequested = False

            'Remove handler for SSH
            AddHandler Console.CancelKeyPress, AddressOf CancelCommand
            RemoveHandler Console.CancelKeyPress, AddressOf SSHDisconnect
        Catch ex As Exception
            EventManager.RaiseSSHError(ex)
            W(DoTranslation("Error trying to connect to SSH server: {0}", currentLang), True, ColTypes.Err, ex.Message)
            WStkTrc(ex)
        End Try
    End Sub

    Private Sub SSHDisconnect(sender As Object, e As ConsoleCancelEventArgs)
        If e.SpecialKey = ConsoleSpecialKey.ControlC Then
            e.Cancel = True
            DisconnectionRequested = True
            EventManager.RaiseSSHDisconnected()
        End If
    End Sub

End Module
