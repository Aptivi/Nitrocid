
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

Imports KS.Shell.Shells.FTP

Namespace Shell.Shells.SFTP.Commands
    ''' <summary>
    ''' Disconnects from the current working server
    ''' </summary>
    ''' <remarks>
    ''' This command sends the quit command to the SFTP server so the server knows that you're going away. It basically disconnects you from the server to connect to the server again or re-connect to the last server connected.
    ''' </remarks>
    Class SFTP_DisconnectCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If SFTPConnected Then
                'Set a connected flag to False
                SFTPConnected = False
                ClientSFTP.Disconnect()
                Write(DoTranslation("Disconnected from {0}"), True, ColTypes.Neutral, FtpSite)

                'Clean up everything
                SFTPSite = ""
                SFTPCurrentRemoteDir = ""
                SFTPUser = ""
                SFTPPass = ""
            Else
                Write(DoTranslation("You haven't connected to any server yet"), True, ColTypes.Error)
            End If
        End Sub

    End Class
End Namespace
