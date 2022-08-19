
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

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' You can interact with the File Transfer Protocol (FTP) shell to connect to a server and transfer files
    ''' </summary>
    ''' <remarks>
    ''' You can use the FTP shell to connect to your FTP server or the public FTP servers to interact with the files found in the server.
    ''' <br></br>
    ''' You can download files to your computer, upload files to the server, manage files by renaming, deleting, etc., and so on.
    ''' </remarks>
    Class FtpCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Try
                If ListArgsOnly.Length = 0 Then
                    StartShell(ShellType.FTPShell)
                Else
                    StartShell(ShellType.FTPShell, ListArgsOnly(0))
                End If
            Catch ftpex As Exceptions.FTPShellException
                Write(ftpex.Message, True, ColTypes.Error)
            Catch ex As Exception
                WStkTrc(ex)
                Write(DoTranslation("Unknown FTP shell error:") + " {0}", True, ColTypes.Error, ex.Message)
            End Try
        End Sub

    End Class
End Namespace
