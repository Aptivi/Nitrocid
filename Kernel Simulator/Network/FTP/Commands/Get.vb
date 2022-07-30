
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
    Class FTP_GetCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim RemoteFile As String = ListArgs(0)
            Dim LocalFile As String = If(ListArgs.Count > 1, ListArgs(1), "")
            Write(DoTranslation("Downloading file {0}..."), False, ColTypes.Progress, RemoteFile)
            Dim Result As Boolean = If(Not String.IsNullOrWhiteSpace(LocalFile), FTPGetFile(RemoteFile, LocalFile), FTPGetFile(RemoteFile))
            If Result Then
                Console.WriteLine()
                Write(DoTranslation("Downloaded file {0}."), True, ColTypes.Success, RemoteFile)
            Else
                Console.WriteLine()
                Write(DoTranslation("Download failed for file {0}."), True, ColTypes.Error, RemoteFile)
            End If
        End Sub

    End Class
End Namespace