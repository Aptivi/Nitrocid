
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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

Imports KS.Files.Querying
Imports System.IO
Imports Renci.SshNet.Common

Namespace Network.SSH
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
        ''' Prompts the user for the connection info
        ''' </summary>
        ''' <param name="Address">An IP address or hostname</param>
        ''' <param name="Port">A port of the SSH/SFTP server. It's usually 22</param>
        ''' <param name="Username">A username to authenticate with</param>
        Public Function PromptConnectionInfo(Address As String, Port As Integer, Username As String) As ConnectionInfo
            'Authentication
            Wdbg(DebugLevel.I, "Address: {0}:{1}, Username: {2}", Address, Port, Username)
            Dim AuthenticationMethods As New List(Of AuthenticationMethod)
            Dim Answer As Integer
            While True
                'Ask for authentication method
                Write(DoTranslation("How do you want to authenticate?") + NewLine, True, ColTypes.Question)
                Write("1) " + DoTranslation("Private key file"), True, ColTypes.Option)
                Write("2) " + DoTranslation("Password") + NewLine, True, ColTypes.Option)
                Write(">> ", False, ColTypes.Input)
                If Integer.TryParse(ReadLine(False), Answer) Then
                    'Check for answer
                    Select Case Answer
                        Case 1, 2
                            Exit While
                        Case Else
                            Wdbg(DebugLevel.W, "Option is not valid. Returning...")
                            Write(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Error, Answer)
                            Write(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                            Console.ReadKey()
                    End Select
                Else
                    Wdbg(DebugLevel.W, "Answer is not numeric.")
                    Write(DoTranslation("The answer must be numeric."), True, ColTypes.Error)
                    Write(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                    Console.ReadKey()
                End If
            End While

            Select Case Answer
                Case 1 'Private key file
                    Dim AuthFiles As New List(Of PrivateKeyFile)

                    'Prompt user
                    While True
                        Dim PrivateKeyFile, PrivateKeyPassphrase As String
                        Dim PrivateKeyAuth As PrivateKeyFile

                        'Ask for location
                        Write(DoTranslation("Enter the location of the private key for {0}. Write ""q"" to finish adding keys: "), False, ColTypes.Input, Username)
                        PrivateKeyFile = ReadLine(False)
                        PrivateKeyFile = NeutralizePath(PrivateKeyFile)
                        If FileExists(PrivateKeyFile) Then
                            'Ask for passphrase
                            Write(DoTranslation("Enter the passphrase for key {0}: "), False, ColTypes.Input, PrivateKeyFile)
                            PrivateKeyPassphrase = ReadLineNoInput()

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
                                Wdbg(DebugLevel.E, "Error trying to add private key authentication method: {0}", ex.Message)
                                Write(DoTranslation("Error trying to add private key:") + " {0}", True, ColTypes.Error, ex.Message)
                            End Try
                        ElseIf PrivateKeyFile.EndsWith("/q") Then
                            Exit While
                        Else
                            Write(DoTranslation("Key file {0} doesn't exist."), True, ColTypes.Error, PrivateKeyFile)
                        End If
                    End While

                    'Add authentication method
                    AuthenticationMethods.Add(New PrivateKeyAuthenticationMethod(Username, AuthFiles.ToArray))
                Case 2 'Password
                    Dim Pass As String

                    'Ask for password
                    Write(DoTranslation("Enter the password for {0}: "), False, ColTypes.Input, Username)
                    Pass = ReadLineNoInput()

                    'Add authentication method
                    AuthenticationMethods.Add(New PasswordAuthenticationMethod(Username, Pass))
            End Select
            Return GetConnectionInfo(Address, Port, Username, AuthenticationMethods)
        End Function

        ''' <summary>
        ''' Gets connection info from the information that the user provided
        ''' </summary>
        ''' <param name="Address">An IP address or hostname</param>
        ''' <param name="Port">A port of the SSH/SFTP server. It's usually 22</param>
        ''' <param name="Username">A username to authenticate with</param>
        ''' <param name="AuthMethods">Authentication methods list.</param>
        Public Function GetConnectionInfo(Address As String, Port As Integer, Username As String, AuthMethods As List(Of AuthenticationMethod)) As ConnectionInfo
            Return New ConnectionInfo(Address, Port, Username, AuthMethods.ToArray)
        End Function

        ''' <summary>
        ''' Opens a session to specified address using the specified port and the username
        ''' </summary>
        ''' <param name="Address">An IP address or hostname</param>
        ''' <param name="Port">A port of the SSH server. It's usually 22</param>
        ''' <param name="Username">A username to authenticate with</param>
        Sub InitializeSSH(Address As String, Port As Integer, Username As String, Connection As ConnectionType, Optional Command As String = "")
            Try
                'Connection
                Dim SSH As New SshClient(PromptConnectionInfo(Address, Port, Username))
                SSH.ConnectionInfo.Timeout = TimeSpan.FromSeconds(30)
                If SSHBanner Then AddHandler SSH.ConnectionInfo.AuthenticationBanner, AddressOf ShowBanner
                Wdbg(DebugLevel.I, "Connecting to {0}...", Address)
                SSH.Connect()

                'Open SSH connection
                If Connection = ConnectionType.Shell Then
                    OpenShell(SSH)
                Else
                    OpenCommand(SSH, Command)
                End If
            Catch ex As Exception
                KernelEventManager.RaiseSSHError(ex)
                Write(DoTranslation("Error trying to connect to SSH server: {0}"), True, ColTypes.Error, ex.Message)
                WStkTrc(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Shows the SSH banner
        ''' </summary>
        Private Sub ShowBanner(sender As Object, e As AuthenticationBannerEventArgs)
            Wdbg(DebugLevel.I, "Banner language: {0}", e.Language)
            Wdbg(DebugLevel.I, "Banner username: {0}", e.Username)
            Wdbg(DebugLevel.I, "Banner length: {0}", e.BannerMessage.Length)
            Wdbg(DebugLevel.I, "Banner:")
            Dim BannerMessageLines() As String = e.BannerMessage.SplitNewLines
            For Each BannerLine As String In BannerMessageLines
                Wdbg(DebugLevel.I, BannerLine)
                Write(BannerLine, True, ColTypes.Neutral)
            Next
        End Sub

        ''' <summary>
        ''' Opens an SSH shell
        ''' </summary>
        ''' <param name="SSHClient">SSH client instance</param>
        Sub OpenShell(SSHClient As SshClient)
            Try
                'Add handler for SSH
                AddHandler Console.CancelKeyPress, AddressOf SSHDisconnect
                RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
                KernelEventManager.RaiseSSHConnected(SSHClient.ConnectionInfo.Host + ":" + CStr(SSHClient.ConnectionInfo.Port))

                'Shell creation. Note that $TERM is what kind of terminal being used (vt100, xterm, ...). Always vt100 on Windows.
                Wdbg(DebugLevel.I, "Opening shell...")
                Dim SSHS As Renci.SshNet.Shell = SSHClient.CreateShell(Console.OpenStandardInput, Console.OpenStandardOutput, Console.OpenStandardError, If(IsOnUnix(), GetTerminalType(), "vt100"), Console.WindowWidth, Console.WindowHeight, Console.BufferWidth, Console.BufferHeight, New Dictionary(Of Common.TerminalModes, UInteger))
                SSHS.Start()

                'Wait until disconnection
                While SSHClient.IsConnected
                    Threading.Thread.Sleep(1)
                    If DisconnectionRequested Or SSHS.GetType.GetField("_input", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance).GetValue(SSHS) Is Nothing Then
                        SSHS.Stop()
                        SSHClient.Disconnect()
                    End If
                End While
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Error on SSH shell in {0}: {1}", SSHClient.ConnectionInfo.Host, ex.Message)
                WStkTrc(ex)
                Write(DoTranslation("Error on SSH shell") + ": {0}", True, ColTypes.Error, ex.Message)
            Finally
                Wdbg(DebugLevel.I, "Connected: {0}", SSHClient.IsConnected)
                Write(NewLine + DoTranslation("SSH Disconnected."), True, ColTypes.Neutral)
                DisconnectionRequested = False

                'Remove handler for SSH
                AddHandler Console.CancelKeyPress, AddressOf CancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf SSHDisconnect
            End Try
        End Sub

        ''' <summary>
        ''' Opens an SSH shell for a command
        ''' </summary>
        ''' <param name="SSHClient">SSH client instance</param>
        Sub OpenCommand(SSHClient As SshClient, Command As String)
            Try
                'Add handler for SSH
                AddHandler Console.CancelKeyPress, AddressOf SSHDisconnect
                RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
                KernelEventManager.RaiseSSHConnected(SSHClient.ConnectionInfo.Host + ":" + CStr(SSHClient.ConnectionInfo.Port))

                'Shell creation
                Wdbg(DebugLevel.I, "Opening shell...")
                KernelEventManager.RaiseSSHPreExecuteCommand(SSHClient.ConnectionInfo.Host + ":" + CStr(SSHClient.ConnectionInfo.Port), Command)
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
                        Write(SSHCErrorReader.ReadLine(), True, ColTypes.Neutral)
                    End While
                    While Not SSHCOutputReader.EndOfStream
                        Write(SSHCOutputReader.ReadLine(), True, ColTypes.Neutral)
                    End While
                End While
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Error trying to execute SSH command ""{0}"" to {1}: {2}", Command, SSHClient.ConnectionInfo.Host, ex.Message)
                WStkTrc(ex)
                Write(DoTranslation("Error executing SSH command") + " {0}: {1}", True, ColTypes.Error, Command, ex.Message)
                KernelEventManager.RaiseSSHCommandError(SSHClient.ConnectionInfo.Host + ":" + CStr(SSHClient.ConnectionInfo.Port), Command, ex)
            Finally
                Wdbg(DebugLevel.I, "Connected: {0}", SSHClient.IsConnected)
                Write(NewLine + DoTranslation("SSH Disconnected."), True, ColTypes.Neutral)
                DisconnectionRequested = False
                KernelEventManager.RaiseSSHPostExecuteCommand(SSHClient.ConnectionInfo.Host + ":" + CStr(SSHClient.ConnectionInfo.Port), Command)

                'Remove handler for SSH
                AddHandler Console.CancelKeyPress, AddressOf CancelCommand
                RemoveHandler Console.CancelKeyPress, AddressOf SSHDisconnect
            End Try
        End Sub

        Private Sub SSHDisconnect(sender As Object, e As ConsoleCancelEventArgs)
            If e.SpecialKey = ConsoleSpecialKey.ControlC Then
                e.Cancel = True
                DisconnectionRequested = True
                KernelEventManager.RaiseSSHDisconnected()
            End If
        End Sub

    End Module
End Namespace
