
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
    ''' Sets data transfer type
    ''' </summary>
    ''' <remarks>
    ''' If you need to change how the data transfer is made, you can use this command to switch between the ASCII transfer and the binary transfer. Please note that the ASCII transfer is highly discouraged in many conditions except if you're only transferring text.
    ''' </remarks>
    Class FTP_TypeCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If ListArgsOnly(0).ToLower = "a" Then
                ClientFTP.DownloadDataType = FtpDataType.ASCII
                ClientFTP.ListingDataType = FtpDataType.ASCII
                ClientFTP.UploadDataType = FtpDataType.ASCII
                Write(DoTranslation("Data type set to ASCII!"), True, ColTypes.Success)
                Write(DoTranslation("Beware that most files won't download or upload properly using this mode, so we highly recommend using the Binary mode on most situations."), True, ColTypes.Warning)
            ElseIf ListArgsOnly(0).ToLower = "b" Then
                ClientFTP.DownloadDataType = FtpDataType.Binary
                ClientFTP.ListingDataType = FtpDataType.Binary
                ClientFTP.UploadDataType = FtpDataType.Binary
                Write(DoTranslation("Data type set to Binary!"), True, ColTypes.Success)
            Else
                Write(DoTranslation("Invalid data type."), True, ColTypes.Error)
            End If
        End Sub

    End Class
End Namespace
