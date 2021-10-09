
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

Class FTP_InfoCommand
    Inherits CommandExecutor
    Implements ICommand

    Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
        If FtpConnected Then
            WriteSeparator(DoTranslation("FTP server information"), True)
            W(DoTranslation("Server address:") + " ", False, ColTypes.ListEntry)
            W(ClientFTP.Host, False, ColTypes.ListEntry)
            W(DoTranslation("Server port:") + " ", False, ColTypes.ListEntry)
            W(ClientFTP.Port, False, ColTypes.ListEntry)
            W(DoTranslation("Server type:") + " ", False, ColTypes.ListEntry)
            W(ClientFTP.ServerType.ToString, False, ColTypes.ListEntry)
            W(DoTranslation("Server system type:") + " ", False, ColTypes.ListEntry)
            W(ClientFTP.SystemType, False, ColTypes.ListEntry)
            W(DoTranslation("Server system:") + " ", False, ColTypes.ListEntry)
            W(ClientFTP.ServerOS.ToString, False, ColTypes.ListEntry)
            W(DoTranslation("Server encryption mode:") + " ", False, ColTypes.ListEntry)
            W(ClientFTP.EncryptionMode.ToString, False, ColTypes.ListEntry)
            W(DoTranslation("Server data connection type:") + " ", False, ColTypes.ListEntry)
            W(ClientFTP.DataConnectionType.ToString, False, ColTypes.ListEntry)
            W(DoTranslation("Server download data type:") + " ", False, ColTypes.ListEntry)
            W(ClientFTP.DownloadDataType.ToString, False, ColTypes.ListEntry)
            W(DoTranslation("Server upload data type:") + " ", False, ColTypes.ListEntry)
            W(ClientFTP.UploadDataType.ToString, False, ColTypes.ListEntry)
        Else
            W(DoTranslation("You haven't connected to any server yet"), True, ColTypes.Error)
        End If
    End Sub

End Class