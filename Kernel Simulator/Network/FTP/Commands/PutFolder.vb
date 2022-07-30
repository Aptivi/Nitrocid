
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

Imports KS.Network.FTP.Transfer

Namespace Network.FTP.Commands
    Class FTP_PutFolderCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim LocalFolder As String = ListArgs(0)
            Dim RemoteFolder As String = If(ListArgs.Count > 1, ListArgs(1), "")
            Write(DoTranslation("Uploading folder {0}..."), True, ColTypes.Progress, ListArgs(0))
            Dim Result As Boolean = If(Not String.IsNullOrWhiteSpace(LocalFolder), FTPUploadFolder(RemoteFolder, LocalFolder), FTPUploadFolder(RemoteFolder))
            If Result Then
                Console.WriteLine()
                Write(NewLine + DoTranslation("Uploaded folder {0}"), True, ColTypes.Success, ListArgs(0))
            Else
                Console.WriteLine()
                Write(NewLine + DoTranslation("Failed to upload {0}"), True, ColTypes.Error, ListArgs(0))
            End If
        End Sub

    End Class
End Namespace