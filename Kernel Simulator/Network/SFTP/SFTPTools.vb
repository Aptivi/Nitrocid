
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

Public Module SFTPTools

    ''' <summary>
    ''' Prompts user for a password
    ''' </summary>
    ''' <param name="user">A user name</param>
    ''' <param name="Address">A host address</param>
    ''' <param name="Port">A port for the address</param>
    Public Sub SFTPPromptForPassword(ByVal user As String, ByVal Address As String, ByVal Port As Integer)
        'Prompt for password
        W(DoTranslation("Password for {0}: ", currentLang), False, ColTypes.Input, user)

        'Get input
        SFTPPass = ReadLineNoInput("*")
        Console.WriteLine()
        ClientSFTP = New SftpClient(Address, Port, user, SFTPPass)

        'Connect to FTP
        ConnectSFTP()
    End Sub

    ''' <summary>
    ''' Tries to connect to the FTP server
    ''' </summary>
    ''' <param name="address">An FTP server. You may specify it like "[address]" or "[address]:[port]"</param>
    Public Sub SFTPTryToConnect(ByVal address As String)
        If connected = True Then
            W(DoTranslation("You should disconnect from server before connecting to another server", currentLang), True, ColTypes.Err)
        Else
            Try
                'Create an SFTP stream to connect to
                Dim SftpHost As String = address.Replace("sftp://", "").Replace(address.Substring(address.LastIndexOf(":")), "")
                Dim SftpPort As String = address.Replace("sftp://", "").Replace(SftpHost + ":", "")

                'Check to see if no port is provided by client
                If SftpHost = SftpPort Then
                    SftpPort = 22
                End If

                'Prompt for username
                W(DoTranslation("Username for {0}: ", currentLang), False, ColTypes.Input, address)
                user = Console.ReadLine()
                If user = "" Then
                    Wdbg("W", "User is not provided. Fallback to ""anonymous""")
                    user = "anonymous"
                End If

                SFTPPromptForPassword(user, SftpHost, SftpPort)
            Catch ex As Exception
                Wdbg("W", "Error connecting to {0}: {1}", address, ex.Message)
                WStkTrc(ex)
                If DebugMode = True Then
                    W(DoTranslation("Error when trying to connect to {0}: {1}", currentLang) + vbNewLine +
                      DoTranslation("Stack Trace: {2}", currentLang), True, ColTypes.Err, address, ex.Message, ex.StackTrace)
                Else
                    W(DoTranslation("Error when trying to connect to {0}: {1}", currentLang), True, ColTypes.Err, address, ex.Message)
                End If
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Tries to connect to the SFTP server.
    ''' </summary>
    Private Sub ConnectSFTP()
        'Connect
        W(DoTranslation("Trying to connect to {0}...", currentLang), True, ColTypes.Neutral, ClientSFTP.ConnectionInfo.Host)
        Wdbg("I", "Connecting to {0} with {1}...", ClientSFTP.ConnectionInfo.Host)
        ClientSFTP.Connect()

        'Show that it's connected
        W(DoTranslation("Connected to {0}", currentLang), True, ColTypes.Neutral, ClientSFTP.ConnectionInfo.Host)
        Wdbg("I", "Connected.")
        SFTPConnected = True

        'Prepare to print current SFTP directory
        SFTPCurrentRemoteDir = ClientSFTP.WorkingDirectory
        Wdbg("I", "Working directory: {0}", SFTPCurrentRemoteDir)
        sftpsite = ClientSFTP.ConnectionInfo.Host
        SFTPUser = ClientSFTP.ConnectionInfo.Username
    End Sub

End Module