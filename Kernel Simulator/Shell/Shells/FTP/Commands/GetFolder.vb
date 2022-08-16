
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

Namespace Shell.Shells.FTP.Commands
    ''' <summary>
    ''' Downloads a folder from the current working directory
    ''' </summary>
    ''' <remarks>
    ''' Downloads the binary or text folder and saves it to the current working local directory for you to use the downloaded folder that is provided in the FTP server.
    ''' </remarks>
    Class FTP_GetFolderCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim RemoteFolder As String = ListArgs(0)
            Dim LocalFolder As String = If(ListArgs.Count > 1, ListArgs(1), "")
            Write(DoTranslation("Downloading folder {0}..."), True, ColTypes.Progress, RemoteFolder)
            Dim Result As Boolean = If(Not String.IsNullOrWhiteSpace(LocalFolder), FTPGetFolder(RemoteFolder, LocalFolder), FTPGetFolder(RemoteFolder))
            If Result Then
                Console.WriteLine()
                Write(DoTranslation("Downloaded folder {0}."), True, ColTypes.Success, RemoteFolder)
            Else
                Console.WriteLine()
                Write(DoTranslation("Download failed for folder {0}."), True, ColTypes.Error, RemoteFolder)
            End If
        End Sub

    End Class
End Namespace
