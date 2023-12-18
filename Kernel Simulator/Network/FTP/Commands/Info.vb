﻿
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

Namespace Network.FTP.Commands
    Class FTP_InfoCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If FtpConnected Then
                WriteSeparator(DoTranslation("FTP server information"), True)
                Write(DoTranslation("Server address:") + " ", False, GetConsoleColor(ColTypes.ListEntry))
                Write(ClientFTP.Host, False, GetConsoleColor(ColTypes.ListEntry))
                Write(DoTranslation("Server port:") + " ", False, GetConsoleColor(ColTypes.ListEntry))
                Write(ClientFTP.Port.ToString, False, GetConsoleColor(ColTypes.ListEntry))
                Write(DoTranslation("Server type:") + " ", False, GetConsoleColor(ColTypes.ListEntry))
                Write(ClientFTP.ServerType.ToString, False, GetConsoleColor(ColTypes.ListEntry))
                Write(DoTranslation("Server system type:") + " ", False, GetConsoleColor(ColTypes.ListEntry))
                Write(ClientFTP.SystemType, False, GetConsoleColor(ColTypes.ListEntry))
                Write(DoTranslation("Server system:") + " ", False, GetConsoleColor(ColTypes.ListEntry))
                Write(ClientFTP.ServerOS.ToString, False, GetConsoleColor(ColTypes.ListEntry))
                Write(DoTranslation("Server encryption mode:") + " ", False, GetConsoleColor(ColTypes.ListEntry))
                Write(ClientFTP.Config.EncryptionMode.ToString, False, GetConsoleColor(ColTypes.ListEntry))
                Write(DoTranslation("Server data connection type:") + " ", False, GetConsoleColor(ColTypes.ListEntry))
                Write(ClientFTP.Config.DataConnectionType.ToString, False, GetConsoleColor(ColTypes.ListEntry))
                Write(DoTranslation("Server download data type:") + " ", False, GetConsoleColor(ColTypes.ListEntry))
                Write(ClientFTP.Config.DownloadDataType.ToString, False, GetConsoleColor(ColTypes.ListEntry))
                Write(DoTranslation("Server upload data type:") + " ", False, GetConsoleColor(ColTypes.ListEntry))
                Write(ClientFTP.Config.UploadDataType.ToString, False, GetConsoleColor(ColTypes.ListEntry))
            Else
                Write(DoTranslation("You haven't connected to any server yet"), True, GetConsoleColor(ColTypes.Error))
            End If
        End Sub

    End Class
End Namespace