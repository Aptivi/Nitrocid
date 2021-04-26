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

Imports System.IO
Imports Renci.SshNet.Common

Public Module SSH

    ''' <summary>
    ''' Whether or not if disconnection is requested
    ''' </summary>
    Private DisconnectionRequested As Boolean
    ''' <summary>
    ''' Whether or not to show the SSH banner on connection
    ''' </summary>
    Public SSHBanner As Boolean

    ''' <summary>
    ''' Specifies SSH connection type
    ''' </summary>
    Enum ConnectionType As Integer
        ''' <summary>
        ''' Connecting to SSH to use a shell
        ''' </summary>
        Shell
        ''' <summary>
        ''' Connecting to SSH to use a single command
        ''' </summary>
        Command
    End Enum

    ''' <summary>
    ''' Opens a session to specified address using the specified port and the username
    ''' </summary>
    ''' <param name="Address">An IP address or hostname</param>
    ''' <param name="Port">A port of the SSH server. It's usually 22</param>
    ''' <param name="Username">A username to authenticate with</param>
    Sub InitializeSSH(ByVal Address As String, ByVal Port As Integer, ByVal Username As String, ByVal Connection As ConnectionType, Optional ByVal Command As String = "")
        Try
            'Authentication
            Wdbg("I", "Address: {0}:{1}, Username: {2}", Address, Port, Username)
            Dim AuthenticationMethods As New List(Of AuthenticationMethod)
            Dim Answer As Integer = 0
            While True
                'Ask for authentication method
                W(DoTranslation("How do you want to authenticate?") + vbNewLine, True, ColTypes.Neutral)
                W("1) " + DoTranslation("Private key file"), True, ColTypes.Option)
                W("2) " + DoTranslation("Password") + vbNewLine, True, ColTypes.Option)
                W(">> ", False, ColTypes.Input)
                Answer = Val(Console.ReadKey(True).KeyChar)
                Console.WriteLine()

                'Check for answer
                Select Case Answer
                    Case 1, 2
                        Exit While
                    Case Else
                        Wdbg("W", "Option is not valid. Returning...")
                        W(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Err, Answer)
                        W(DoTranslation("Press any key to go back."), True, ColTypes.Err)
                        Console.ReadKey()
                End Select
            End While

            Select Case Answer
                Case 1 'Private key file
                    Dim AuthFiles As New List(Of PrivateKeyFile)

                    'Prompt user
                    While True
                        Dim PrivateKeyFile, PrivateKeyPassphrase As String
                        Dim PrivateKeyAuth As PrivateKeyFile

                        'Ask for location
                        W(DoTranslation("Enter the location of the private key for {0}. Write ""q"" to finish adding keys: "), False, ColTypes.Input, Username)
                        PrivateKeyFile = Console.ReadLine()
                        PrivateKeyFile = NeutralizePath(PrivateKeyFile)
                        If File.Exists(PrivateKeyFile) Then
                            'Ask for passphrase
                            W(DoTranslation("Enter the passphrase for key {0}: "), False, ColTypes.Input, PrivateKeyFile)
                            PrivateKeyPassphrase = ReadLineNoInput("*")
                            Console.WriteLine()

                            'Add authentication method
                            Try
                                If String.IsNullOrEmpty(PrivateKeyPassphrase) Then
                                    PrivateKeyAuth = New PrivateKeyFile(PrivateKeyFile)
                                Else
                                    PrivateKeyAuth = New PrivateKeyFile(PrivateKeyFile, PrivateKeyPassphrase)
                                End If
                                AuthFiles.Add(PrivateKeyAuth)
                            Catch ex As Exception
                                WStkTrc(ex)
                                Wdbg("E", "Error trying to add private key authentication method: {0}", ex.Message)
                                W(DoTranslation("Error trying to add private key:") + " {0}", True, ColTypes.Err, ex.Message)
                            End Try
                        ElseIf PrivateKeyFile.EndsWith("/q") Then
                            Exit While
                        Else
                            W(DoTranslation("Key file {0} doesn't exist."), True, ColTypes.Err, PrivateKeyFile)
                        End If
                    End While

                    'Add authentication method
                    AuthenticationMethods.Add(New PrivateKeyAuthenticationMethod(Username, AuthFiles.ToArray))
                Case 2 'Password
                    Dim Pass As String

                    'Ask for password
                    W(DoTranslation("Enter the password for {0}: "), False, ColTypes.Input, Username)
                    Pass = ReadLineNoInput("*")
                    Console.WriteLine()

                    'Add authentication method
                    AuthenticationMethods.Add(New PasswordAuthenticationMethod(Username, Pass))
            End Select

            'Connection
            Dim SSH As New SshClient(New ConnectionInfo(Address, Port, Username, AuthenticationMethods.ToArray))
            SSH.ConnectionInfo.Timeout = TimeSpan.FromSeconds(30)
            If SSHBanner Then AddHandler SSH.ConnectionInfo.AuthenticationBanner, AddressOf ShowBanner
            Wdbg("I", "Connecting to {0}...", Address)
            SSH.Connect()

            'Open SSH connection
            If Connection = ConnectionType.Shell Then
                OpenShell(SSH)
            Else
                OpenCommand(SSH, Command)
            End If
        Catch ex As Exception
            EventManager.RaiseSSHError(ex)
            W(DoTranslation("Error trying to connect to SSH server: {0}"), True, ColTypes.Err, ex.Message)
            WStkTrc(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Shows the SSH banner
    ''' </summary>
    Private Sub ShowBanner(sender As Object, e As AuthenticationBannerEventArgs)
        Wdbg("I", "Banner language: {0}", e.Language)
        Wdbg("I", "Banner username: {0}", e.Username)
        Wdbg("I", "Banner length: {0}", e.BannerMessage.Length)
        Wdbg("I", "Banner:")
        Dim BannerMessageLines() As String = e.BannerMessage.Replace(Chr(13), "").Split(Chr(10))
        For Each BannerLine As String In BannerMessageLines
            Wdbg("I", BannerLine)
            W(BannerLine, True, ColTypes.Neutral)
        Next
    End Sub

    ''' <summary>
    ''' Opens an SSH shell
    ''' </summary>
    ''' <param name="SSHClient">SSH client instance</param>
    Sub OpenShell(SSHClient As SshClient)
        'Add handler for SSH
        AddHandler Console.CancelKeyPress, AddressOf SSHDisconnect
        RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
        EventManager.RaiseSSHConnected(SSHClient.ConnectionInfo.Host + ":" + CStr(SSHClient.ConnectionInfo.Port))

        'Shell creation
        Wdbg("I", "Opening shell...")
        Dim SSHS As Renci.SshNet.Shell = SSHClient.CreateShell(Console.OpenStandardInput, Console.OpenStandardOutput, Console.OpenStandardError, "vt100", Console.WindowWidth, Console.WindowHeight, Console.BufferWidth, Console.BufferHeight, New Dictionary(Of Common.TerminalModes, UInteger))
        SSHS.Start()

        'Wait until disconnection
        While SSHClient.IsConnected
            Threading.Thread.Sleep(1)
            If DisconnectionRequested Then
                SSHS.Stop()
                SSHClient.Disconnect()
            End If
        End While
        Wdbg("I", "Connected: {0}", SSHClient.IsConnected)
        W(vbNewLine + DoTranslation("SSH Disconnected."), True, ColTypes.Neutral)
        DisconnectionRequested = False

        'Remove handler for SSH
        AddHandler Console.CancelKeyPress, AddressOf CancelCommand
        RemoveHandler Console.CancelKeyPress, AddressOf SSHDisconnect
    End Sub

    ''' <summary>
    ''' Opens an SSH shell for a command
    ''' </summary>
    ''' <param name="SSHClient">SSH client instance</param>
    Sub OpenCommand(SSHClient As SshClient, Command As String)
        'Add handler for SSH
        AddHandler Console.CancelKeyPress, AddressOf SSHDisconnect
        RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
        EventManager.RaiseSSHConnected(SSHClient.ConnectionInfo.Host + ":" + CStr(SSHClient.ConnectionInfo.Port))

        'Shell creation
        Wdbg("I", "Opening shell...")
        Dim SSHC As SshCommand = SSHClient.CreateCommand(Command)
        Dim SSHCAsyncResult As IAsyncResult = SSHC.BeginExecute()
        'TODO: SshCommand doesn't have input support.
        Dim SSHCOutputReader As New StreamReader(SSHC.OutputStream)
        Dim SSHCErrorReader As New StreamReader(SSHC.ExtendedOutputStream)

        'Wait until disconnection
        While Not SSHCAsyncResult.IsCompleted
            Threading.Thread.Sleep(1)
            If DisconnectionRequested Then
                SSHC.CancelAsync()
                SSHClient.Disconnect()
            End If
            While Not SSHCErrorReader.EndOfStream
                W(SSHCErrorReader.ReadLine(), True, ColTypes.Neutral)
            End While
            While Not SSHCOutputReader.EndOfStream
                W(SSHCOutputReader.ReadLine(), True, ColTypes.Neutral)
            End While
        End While
        Wdbg("I", "Connected: {0}", SSHClient.IsConnected)
        W(vbNewLine + DoTranslation("SSH Disconnected."), True, ColTypes.Neutral)
        DisconnectionRequested = False

        'Remove handler for SSH
        AddHandler Console.CancelKeyPress, AddressOf CancelCommand
        RemoveHandler Console.CancelKeyPress, AddressOf SSHDisconnect
    End Sub

    Private Sub SSHDisconnect(sender As Object, e As ConsoleCancelEventArgs)
        If e.SpecialKey = ConsoleSpecialKey.ControlC Then
            e.Cancel = True
            DisconnectionRequested = True
            EventManager.RaiseSSHDisconnected()
        End If
    End Sub

End Module
