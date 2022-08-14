
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

Imports KS.Network.FTP.Filesystem

Namespace Shell.Shells.FTP.Commands
    Class FTP_PermCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If FtpConnected Then
                If FTPChangePermissions(ListArgs(0), ListArgs(1)) Then
                    Write(DoTranslation("Permissions set successfully for file") + " {0}", True, ColTypes.Success, ListArgs(0))
                Else
                    Write(DoTranslation("Failed to set permissions of {0} to {1}."), True, ColTypes.Error, ListArgs(0), ListArgs(1))
                End If
            Else
                Write(DoTranslation("You must connect to server before performing filesystem operations."), True, ColTypes.Error)
            End If
        End Sub

    End Class
End Namespace
