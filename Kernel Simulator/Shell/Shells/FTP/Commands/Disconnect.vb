
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

Namespace Shell.Shells.FTP.Commands
    ''' <summary>
    ''' Disconnects from the current working server
    ''' </summary>
    ''' <remarks>
    ''' This command sends the quit command to the FTP server so the server knows that you're going away. It basically disconnects you from the server to connect to the server again or re-connect to the last server connected.
    ''' <br></br>
    ''' <list type="table">
    ''' <listheader>
    ''' <term>Switches</term>
    ''' <description>Description</description>
    ''' </listheader>
    ''' <item>
    ''' <term>-f</term>
    ''' <description>Force disconnection</description>
    ''' </item>
    ''' </list>
    ''' <br></br>
    ''' </remarks>
    Class FTP_DisconnectCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If FtpConnected = True Then
                'Set a connected flag to False
                FtpConnected = False
                ClientFTP.DisconnectWithQuit = ListSwitchesOnly.Contains("-f")
                ClientFTP.Disconnect()
                Write(DoTranslation("Disconnected from {0}"), True, ColTypes.Success, FtpSite)

                'Clean up everything
                FtpSite = ""
                FtpCurrentRemoteDir = ""
                FtpUser = ""
                FtpPass = ""
            Else
                Write(DoTranslation("You haven't connected to any server yet"), True, ColTypes.Error)
            End If
        End Sub

        Public Overrides Sub HelpHelper()
            Write(DoTranslation("This command has the below switches that change how it works:"), True, ColTypes.Neutral)
            Write("  -f: ", False, ColTypes.ListEntry) : Write(DoTranslation("Disconnects from server disgracefully"), True, ColTypes.ListValue)
        End Sub

    End Class
End Namespace
